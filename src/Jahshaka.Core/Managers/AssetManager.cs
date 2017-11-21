using System;
using System.Linq;
using System.Threading.Tasks;
using Jahshaka.Core.Data;
using Jahshaka.Core.Enums;
using Jahshaka.Core.Services.S3;
using Jahshaka.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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


        public async Task<Asset> SetAssetAsync(Guid userId, IFormFile file, IFormFile thumnbnail, string uploadId, string name, AssetType type, int collectionId, bool isPublic, string worldId, int worldVersionId){
            
            var user = _dbContext.Users.FirstOrDefault(u => u.Id.Equals(userId));

            if (user == null)
            {
                throw new Exception($"No user with id='{userId}' was found.");
            }

            var collection = _dbContext.Collections.FirstOrDefault(c => c.Id.Equals(collectionId));

            if(collection == null)
            {
                throw new Exception($"Collection not found.");
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
                CollectionId = collection.Id,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Assets.Add(asset);

            await _dbContext.SaveChangesAsync();
            
            if (!string.IsNullOrEmpty(worldId))
            {
                var worldVersion = _dbContext.WorldVersions
                    .FirstOrDefault(w => w.Id == worldVersionId && w.WorldId.ToString().Equals(worldId));

                if (worldVersion == null)
                {
                    throw new Exception($"World with id {worldId} and version {worldVersionId} not found.");
                }

                var worldVersionAsset = new WorldVersionAsset()
                {
                    WorldId = worldVersion.WorldId,
                    WorldVersionId = worldVersion.Id,
                    AssetId = asset.Id
                };
                
                asset.WorldVersionAssets.Add(worldVersionAsset);
            }
            
            var fileStream = AssetHelper.ToStream(file);

            asset.Url = await _s3Service.UploadFileFromStreamAsync(fileStream, BucketName+"/Assets", userId.ToString(), $"{asset.Id}-{file.FileName}");
            
            var thumbnailStream = AssetHelper.Resize(ThumbnailSize, thumnbnail);

            asset.IconUrl = await _s3Service.UploadFileFromStreamAsync(thumbnailStream, BucketName+"/Thumbnails", userId.ToString(), $"{asset.Id}-{thumnbnail.FileName}");
 
            _dbContext.Update(asset);

            await _dbContext.SaveChangesAsync();

            return asset;
        }
    }
}
