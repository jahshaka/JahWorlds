using System;
using System.Linq;
using System.Threading.Tasks;
using Jahshaka.Core.Data;
using Jahshaka.Core.Enums;
using Jahshaka.Core.Services.S3;
using Jahshaka.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Jahshaka.Core.Managers
{
    public class AssetManager
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly S3Service _s3Service;

        private ILogger _logger;

        private const int ThumbnailSize = 100;

        private const string BucketName = "Jahshaka";

        public AssetManager(ApplicationDbContext dbContext, S3Service s3Service, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _s3Service = s3Service;
            _logger = loggerFactory.CreateLogger<AssetManager>();
        }


        public async Task<Asset> SetAssetAsync(Guid userId, IFormFile file, IFormFile thumnbnail, string uploadId, string name, AssetType type, bool isPublic)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id.Equals(userId));

            if (user == null)
            {
                throw new Exception($"No user with id='{userId}' was found.");
            }

            var asset = _dbContext.Assets.FirstOrDefault(p => p.UploadId.Equals(uploadId));
            
            if (asset != null)
            {
                throw new Exception($"The asset with 'UploadId'='{uploadId}' already exists.");
            }

            asset = new Asset()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = name,
                Type = type,
                IsPublic = isPublic,
                UploadId = uploadId,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Assets.Add(asset);

            await _dbContext.SaveChangesAsync();
            
            var fileStream = AssetHelper.ToStream(file);

            asset.Url = await _s3Service.UploadFileFromStreamAsync(fileStream, BucketName, userId.ToString(), $"{asset.Id}-asset.zip");
            
            var thumbnailStream = AssetHelper.Resize(ThumbnailSize, thumnbnail);

            asset.IconUrl = await _s3Service.UploadFileFromStreamAsync(thumbnailStream, BucketName, userId.ToString(), $"{asset.Id}-thumbnail.jpg");
 
            _dbContext.Update(asset);

            await _dbContext.SaveChangesAsync();

            return asset;
        }
    }
}
