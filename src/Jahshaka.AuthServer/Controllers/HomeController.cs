using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Jahshaka.AuthServer.ViewModels.Shared;
using Jahshaka.Core.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jahshaka.AuthServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            // Inside on of your controllers
            if (User.Identity.IsAuthenticated)
            {

                var user = await _userManager.GetUserAsync(HttpContext.User);
                
                Console.WriteLine($"------------------> User:{user.Id}");

                return RedirectToAction("GenerateToken", "AuthorizationController");


                /*
                string accessToken = await HttpContext.GetTokenAsync(IdentityConstants.ExternalScheme, "access_token");
                //string idToken = await HttpContext.GetTokenAsync("id_token");

                // Now you can use them. For more info on when and how to use the 
                // access_token and id_token, see https://auth0.com/docs/tokens

                Console.WriteLine($"----------------ACCESS TOKEN: '{accessToken}'");
                accessToken = User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
                Console.WriteLine($"----------------ACCESS TOKEN: '{accessToken}'");
                accessToken = await HttpContext.GetTokenAsync("access_token");
                Console.WriteLine($"----------------ACCESS TOKEN: '{accessToken}'");
                */               
                
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new Jahshaka.ViewModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
