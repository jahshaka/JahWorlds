using System;
using System.Collections.Generic;
using Jahshaka.Core.Data;

namespace Jahshaka.API.ViewModels.World
{
    public class WorldViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public ICollection<WorldVersion> WorldVersions { get; set; }
    }
}