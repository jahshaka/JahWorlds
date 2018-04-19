using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jahshaka.API.ViewModels.Application
{
    public class ApplicationViewModel
    {
        public ApplicationViewModel()
        {

            Versions = new List<ApplicationVersionViewModel>();

        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("post_logout_redirect_uris")]
        public string PostLogoutRedirectUris { get; set; }

        [JsonProperty("redirect_uris")]
        public string RedirectUris { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("versions")]
        public IList<ApplicationVersionViewModel> Versions { get; set; }
    }

}