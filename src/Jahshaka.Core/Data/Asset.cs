using System;
using System.Collections.Generic;
using Jahshaka.Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Jahshaka.Core.Data
{
    public class Asset
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int? CollectionId { get; set; }
        public string UploadId { get; set; }
        public string Name { get; set;  }
        public AssetType Type { get; set; }
        public string Url { get; set; }
        public string IconUrl { get; set; }
        public string MetaData { get; set; }
        public bool IsPublic { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser User { get; set;  }
        public Collection Collection { get; set;  }
        public ICollection<WorldVersionAsset> WorldVersionAssets { get; set; }
    }
}