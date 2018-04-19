using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Jahshaka.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jahshaka.API.ViewModels.World
{
    public class CreateWorldAssetViewModel
    {
        [Required]
        [FromForm(Name = "name")]
        public string Name { get; set;  }

        [Required]
        [FromForm(Name = "type")]
        public AssetType Type { get; set; }

        [Required]
        [FromForm(Name = "upload")]
        public IFormFile Upload { get; set; }

        [Required]
        [FromForm(Name = "thumbnail")]
        public IFormFile Thumbnail { get; set; }
        
        [FromForm(Name = "is_public")]
        public bool IsPublic { get; set; }
        
        [FromForm(Name = "tags")]
        public List<string> Tags { get; set; }
        
        [FromForm(Name = "upload_id")]
        public string UploadId { get; set; }
        
        [Required]
        [FromForm(Name = "world_id")]
        public string WorldId { get; set; }
        
        [Required]
        [FromForm(Name = "world_version_id")]
        public int WorldVersionId { get; set; }
    }
}