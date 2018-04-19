using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Jahshaka.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Asset
{
    public class CreateAssetViewModel
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

        [Required]
        [FromForm(Name = "collection_id")]
        public int CollectionId { get; set;  }
        
        /*[FromForm(Name = "is_public")]
        public bool IsPublic { get; set; }*/
        
        [FromForm(Name = "tags")]
        public List<string> Tags { get; set; }
        
        [FromForm(Name = "upload_id")]
        public string UploadId { get; set; }
        
        [FromForm(Name = "world_id")]
        public string WorldId { get; set; }
        
        [FromForm(Name = "world_version_id")]
        public int WorldVersionId { get; set; }
    }
}