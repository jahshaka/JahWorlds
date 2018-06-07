using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Applications
{
    public class AddVersionViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }

        [JsonProperty("windows_url")]
        public string WindowsUrl {get; set;}

        [JsonProperty("mac_url")]
        public string MacUrl {get; set;}

        [JsonProperty("linux_url")]
        public string LinuxUrl {get; set;}

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("supported")]
        public bool Supported { get; set; }
        
    }
}