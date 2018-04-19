using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jahshaka.API.ViewModels.Shared
{
    public class ErrorViewModel
    {
        [Display(Name = "Error")]
        [JsonProperty("error")]
        public string Error { get; set; }

        [Display(Name = "Description")]
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
