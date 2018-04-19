using System;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Applications
{
    public class UpdateVersionViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }

        [JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
        
    }
}