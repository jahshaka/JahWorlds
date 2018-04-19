using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Jahshaka.API.ViewModels.Asset;
using Jahshaka.Core.Data;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Collection
{
    public class UpdateCollectionViewModel
    {
        [Required]
        [JsonProperty("collection_parent_id")]
        public int CollectionParentId { get; set; }
    }
}