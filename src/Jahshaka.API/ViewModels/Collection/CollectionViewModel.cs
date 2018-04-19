using System;
using System.Collections.Generic;
using Jahshaka.API.ViewModels.Asset;
using Jahshaka.Core.Data;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Collection
{
    public class CollectionViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user_id")]
        public Guid? UserId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set;  }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("collections")]
        public ICollection<CollectionViewModel> Collections { get; set; }

        [JsonProperty("assets")]
        public ICollection<AssetViewModel> Assets { get; set; }
    }
}