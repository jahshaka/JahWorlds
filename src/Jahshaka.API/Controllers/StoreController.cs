using System.Linq;
using Jahshaka.API.ViewModels.Asset;
using Jahshaka.API.ViewModels.Mappers;
using Jahshaka.Core.Data;
using Jahshaka.Core.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Jahshaka.API.Controllers
{
    [Route("store")]
    public class StoreController : Controller
    {
        private readonly AssetManager _assetManager;
        private readonly ApplicationDbContext _appDbContext;

        public StoreController(
            AssetManager assetManager,
            ApplicationDbContext appDbContext
        )
        {
            _assetManager = assetManager;
            _appDbContext = appDbContext;
        }
        /*
        [HttpGet, Route("assets")]
        public IActionResult GetAssets()
        {
            var assets = _appDbContext.Assets
                .Where(a => a.IsPublic)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            var viewModel = new ListAssetViewModel()
            {
                Assets = assets.ToViewModel()
            };

            return Ok(viewModel);
        }
        */
    }
}