using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Jahshaka.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Jahshaka.Web.ViewModels.Asset
{
    public class CreateAssetViewModel
    {
        [Required]
        public string Name { get; set;  }

        [Required]
        public AssetType Type { get; set; }

        [Required]
        //[FileExtensions(Extensions = "zip")]
        public IFormFile Upload { get; set; }

        [Required]
        //[FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp,svg")]
        public IFormFile Thumbnail { get; set; }

        public bool IsPublic { get; set; }

        public List<string> Tags { get; set; }
    }
}