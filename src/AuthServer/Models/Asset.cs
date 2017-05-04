using System;
using System.Collections.Generic;
using Jahshaka.AuthServer.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Jahshaka.AuthServer.Models
{
    public class Asset
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set;  }
        public AssetType Type { get; set; }
        public string Url { get; set; }
        public string IconUrl { get; set; }
        public string MetaData { get; set; }
        public bool IsPublic { get; set; }
        public List<string> Tags { get; set; }

        public ApplicationUser User { get; set;  }
    }
}