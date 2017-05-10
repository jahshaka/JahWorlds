using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Jahshaka.Web.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
