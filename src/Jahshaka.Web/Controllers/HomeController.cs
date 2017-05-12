using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jahshaka.Core.Data;
using Jahshaka.Web.ViewModels.Asset;
using Jahshaka.Web.ViewModels.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jahshaka.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;

        public HomeController(
            ApplicationDbContext appDbContext,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment
        )
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string query)
        {
            var assets = new List<Asset>();

            var user = await _userManager.GetUserAsync(User);

            string search = query == null ? "" : query;

            Console.WriteLine("Query: "+ query);

            if (user == null)
            {
                assets = _appDbContext.Assets
                    .Where(a => a.IsPublic && a.Name.ToLower().Contains(search.ToLower()))
                    .OrderByDescending(a => a.CreatedAt)
                    .ToList();
            }
            else
            {
                assets = _appDbContext.Assets
                    .Where(a => (a.IsPublic || a.UserId == user.Id) && a.Name.ToLower().Contains(search.ToLower()))
                    .OrderByDescending(a => a.CreatedAt)
                    .ToList();
            }

            var viewModel = new ListAssetViewModel()
            {
                Assets = assets.ToViewModel(),
                query = search
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Download(Guid id)
        {

            var asset = _appDbContext.Assets
                .FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                TempData["Error"] = "Asset not found";
                return RedirectToAction("Index");
            }

            if (!asset.IsPublic)
            {
                TempData["Error"] = "Asset cannot be downloaded";
                return RedirectToAction("Index");
            }

            var filePath = Path.Combine(_environment.WebRootPath, "uploads");
            byte[] fileBytes = System.IO.File.ReadAllBytes($"{filePath}/{asset.Url}");

            return File(fileBytes, "application/x-msdownload", $"{asset.Name}.zip");

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
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
