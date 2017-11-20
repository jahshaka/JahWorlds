using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Jahshaka.API.ViewModels.Asset;
using Jahshaka.Core.Data;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Collection
{
    public class RenameCollectionViewModel
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set;  }
    }
}