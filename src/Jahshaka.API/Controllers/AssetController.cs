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

namespace Jahshaka.API.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("assets")]
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
        
        [HttpPost, Route("upload")]
        public async Task<IActionResult> Create(CreateAssetViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    
                    if (user == null)
                    {
                        return Unauthorized();
                    }
                    
                    if (model.Upload == null || model.Upload.Length == 0)
                    {
                        return BadRequest(new ErrorViewModel() { Error = ErrorCode.ModelError, ErrorDescription = $"Invalid file data." });
                    }
                    
                    if (model.Thumbnail == null || model.Thumbnail.Length == 0)
                    {
                        return BadRequest(new ErrorViewModel() { Error = ErrorCode.ModelError, ErrorDescription = $"Invalid thumbnail data." });
                    }

                    if (model.UploadId == null)
                    {
                        model.UploadId = Guid.NewGuid().ToString();
                    }
                    
                    var asset = await _assetManager.SetAssetAsync(user.Id, model.Upload, model.Thumbnail, model.UploadId, model.Name, model.Type, model.IsPublic, model.WorldId, model.WorldVersionId);

                    return Ok(asset.ToViewModel());
                    
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = ex.Message
                    });
                }

            }
            
            return BadRequest(new ErrorViewModel()
            {
                Error = ErrorCode.ModelError,
                ErrorDescription = ModelState?.GetFirstError()
            });
            
        }
        
        [HttpGet, Route("download")]
        public async Task<IActionResult> Download(Guid id)
        {

            var user = await _userManager.GetUserAsync(User);

            var asset = _appDbContext.Assets
                .FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = "Asset not found."
                });
            }

            if (!asset.IsPublic && asset.UserId != user.Id)
            {
                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = "Asset not found."
                });
            }

            var filePath = Path.Combine(_environment.WebRootPath, "uploads");
            byte[] fileBytes = System.IO.File.ReadAllBytes($"{filePath}/{asset.Url}");

            return File(fileBytes, "application/x-msdownload", $"{asset.Name}.zip");

        }
    }
}