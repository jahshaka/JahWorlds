using System;
using System.Collections.Generic;

namespace Jahshaka.Core.Data
{
    public class WorldVersion
    {

        public WorldVersion()
        {
            Assets = new HashSet<Asset>();
        }
        
        public int Id { get; set; }
        public float VersionNumber { get; set; }
        public Guid WorldId { get; set; }
        
        public virtual World World { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public ICollection<WorldVersionAsset> WorldVersionAssets { get; set; }
    }
}