using AspNet.Security.OpenIdConnect.Primitives;
using Jahshaka.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Jahshaka.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet, HttpPost, Route("~/error")]
        public IActionResult Error(OpenIdConnectResponse response)
        {
            // If the error was not caused by an invalid
            // OIDC request, display a generic error page.
            if (response == null)
            {
                return View(new ErrorViewModel());
            }

            return View(new ErrorViewModel
            {
                Error = response.Error,
                ErrorDescription = response.ErrorDescription
            });
        }
    }
}