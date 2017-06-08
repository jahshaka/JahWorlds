using System;
using System.Collections.Generic;

namespace Jahshaka.Core.Data
{
    public class World
    {

        public World()
        {
            WorldVersions = new HashSet<WorldVersion>();
        }
        
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public ApplicationUser User { get; set;  }
        public ICollection<WorldVersion> WorldVersions { get; set; }
    }
}