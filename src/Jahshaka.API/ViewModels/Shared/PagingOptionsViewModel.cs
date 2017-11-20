using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jahshaka.API.ViewModels.Shared
{
    public class PagingOptionsViewModel
    {
        [JsonProperty("total_items")]
        public int TotalItems { get; set; }
        
        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        [JsonProperty("prev_url")]
        public string PrevUrl { get; set; }
    }
}
