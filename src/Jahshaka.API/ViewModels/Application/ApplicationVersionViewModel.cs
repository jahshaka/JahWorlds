using System;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Application
{
    public class ApplicationVersionViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("application_id")]
        public Guid ApplicationId { get; set; }

        [JsonProperty("supported")]
        public bool Supported { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl {get; set;}
    }
}