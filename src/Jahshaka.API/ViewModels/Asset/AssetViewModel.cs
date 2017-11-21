using System;
using System.Collections.Generic;
using Jahshaka.Core.Data;
using Jahshaka.Core.Enums;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Asset
{
    public class AssetViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set;  }
        
        [JsonProperty("type")]
        public AssetType Type { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        
        [JsonProperty("meta_data")]
        public string MetaData { get; set; }
        
        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }
        
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
        
        [JsonProperty("collection_id")]
        public int? CollectionId { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        //[JsonProperty("user")]
        //public ApplicationUser User { get; set;  }
    }
}