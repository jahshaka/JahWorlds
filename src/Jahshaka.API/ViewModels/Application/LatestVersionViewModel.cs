using System;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Application
{
    public class LatestVersionViewModel
    {
        [JsonProperty("version")]
        public string Id { get; set; }

        [JsonProperty("supported")]
        public bool Supported { get; set; }

        [JsonProperty("should_update")]
        public bool ShouldUpdate { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl {get; set;}

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }
    }
}