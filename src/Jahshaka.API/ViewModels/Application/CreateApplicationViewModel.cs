using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Applications
{
    public class CreateApplicationViewModel
    {
        [Required]
        [JsonProperty("client_id")]
        public string ClientId {get; set;}

        [Required]
        [JsonProperty("client_secret")]
        public string ClientSecret {get; set;}

        [Required]
        [JsonProperty("display_name")]
        public string DisplayName {get; set;}

        [JsonProperty("post_logout_redirect_uris")]
        public string PostLogoutRedirectUris {get; set;}

        [JsonProperty("redirect_uris")]
        public string RedirectUris {get; set;}

        [Required]
        [JsonProperty("type")]
        public string Type {get; set;}
    }
}