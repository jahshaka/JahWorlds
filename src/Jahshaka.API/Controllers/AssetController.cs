using System.Linq;
using System.Threading.Tasks;
using Jahshaka.API.ViewModels.Asset;
using Jahshaka.API.ViewModels.Mappers;
using Jahshaka.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jahshaka.API.Controllers
{
    [Authorize]
    [Route("assets")]
    public class AssetController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _appDbContext;
        private IHostingEnvironment _environment;

        public AssetController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment
        )
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _environment = environment;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var assets = _appDbContext.Assets
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            var viewModel = new ListAssetViewModel()
            {
                Assets = assets.ToViewModel()
            };

            return Ok(viewModel);

        }
    }
}