using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jahshaka.API.Constants;
using Jahshaka.API.ViewModels.Asset;
using Jahshaka.API.ViewModels.Mappers;
using Jahshaka.API.ViewModels.Shared;
using Jahshaka.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNet.Security.OAuth.Validation;
using Jahshaka.Core.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Jahshaka.Core.Enums;

namespace Jahshaka.API.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("admin/assets")]
    public class AssetController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _appDbContext;
        private IHostingEnvironment _environment;
        private readonly AssetManager _assetManager;
        
        public AssetController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment,
            AssetManager assetManager
        )
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _environment = environment;
            _assetManager = assetManager;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.UserType != UserType.Admin)
            {
                return Unauthorized();
            }

            var assets = _appDbContext.Assets
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