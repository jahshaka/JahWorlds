using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Applications
{
    public class AddVersionViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("supported")]
        public bool Supported { get; set; }
        
    }
}