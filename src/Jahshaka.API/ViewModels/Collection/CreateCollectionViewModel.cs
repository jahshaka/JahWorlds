using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Jahshaka.API.ViewModels.Asset;
using Jahshaka.Core.Data;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Collection
{
    public class CreateCollectionViewModel
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set;  }

        [JsonProperty("collection_id")]
        public int? CollectionId { get; set; }
    }
}