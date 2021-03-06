﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Jahshaka.Core.Services.S3
{
    public class S3Service
    {
        private readonly ILogger _logger;
        private readonly IOptions<S3ServiceOptions> _options;
        private readonly IAmazonS3 _s3client;

        public S3Service(IOptions<S3ServiceOptions> options, ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<S3Service>();
            this._options = options;

            var credentials = new BasicAWSCredentials(accessKey: _options.Value.AccessKey,
                secretKey: _options.Value.SecretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            };

            this._s3client = new AmazonS3Client(credentials, config);
        }

        public async Task<string> UploadFileFromStreamAsync(Stream fileStream, string bucket, string dir,
            string filename)
        {
            try
            {
                String fileKey = $"{dir}/{filename}";
				
				await _s3client.EnsureBucketExistsAsync(bucket);

                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = fileKey,
                    InputStream = fileStream,
                    CannedACL = S3CannedACL.PublicRead
                };

                PutObjectResponse response = await _s3client.PutObjectAsync(request);

                return $"https://s3.amazonaws.com/{bucket}/{fileKey}";
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogCritical(ex.Message);

                throw ex;
            }
        }

        public async Task<bool> CreateBucketAsync(string bucketName)
        {
            try
            {
                var putBucketRequest = new PutBucketRequest {BucketName = bucketName};

                await _s3client.PutBucketAsync(putBucketRequest);

                return true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogCritical(e.Message);

                return false;
            }
        }

        public async Task<bool> RemoveFileAsync(string bucket, string dir, string filename)
        {
            try
            {

                String fileKey = $"{dir}/{filename}";

                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest {
                    BucketName = bucket,
                    Key = fileKey
                };

                await _s3client.DeleteObjectAsync(deleteObjectRequest);

                return true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogCritical(e.Message);

                return false;
            }
        }
        
    }
}