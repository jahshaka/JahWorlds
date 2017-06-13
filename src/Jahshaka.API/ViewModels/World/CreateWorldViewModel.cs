using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.World
{
    public class CreateWorldViewModel
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
    }
}