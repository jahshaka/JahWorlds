using System;

namespace Jahshaka.Core.Data
{
    public class WorldVersionAsset
    {
        public Guid AssetId { get; set; }
        public int WorldVersionId { get; set; }
        
        public virtual WorldVersion WorldVersion { get; set; }
        public virtual Asset Asset { get; set; }
    }
}