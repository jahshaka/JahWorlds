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
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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
        public async Task<IActionResult> Index(int? collection_id = null )
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var assets = _appDbContext.Assets
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.CreatedAt)
                .AsQueryable();
            
            if(collection_id != null){
                assets = assets.Where(a => a.CollectionId.Equals(collection_id));
            }

            var viewModel = new ListAssetViewModel()
            {
                Assets = assets.ToList().ToViewModel()
            };

            return Ok(viewModel);
        }
        
        [HttpPost, Route("upload")]
        public async Task<IActionResult> Upload(CreateAssetViewModel model)
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

                    Boolean IsPublic = false;
                    
                    var asset = await _assetManager.SetAssetAsync(user.Id, model.Upload, model.Thumbnail, model.UploadId, model.Name, model.Type, model.CollectionId, IsPublic, null, 0);

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
        
        [HttpPost, Route("{asset_id:Guid}/change_collection/{collection_id}")]
        public async Task<IActionResult> ChangeCollection(Guid asset_id, int collection_id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                
                if (user == null)
                {
                    return Unauthorized();
                }
                
                var asset = _appDbContext.Assets.FirstOrDefault(a => a.UserId.Equals(user.Id) && a.Id.Equals(asset_id));

                if (asset == null)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = "Asset not found for this user"
                    });
                }

                var collection = _appDbContext.Collections.FirstOrDefault(a => a.UserId.Equals(user.Id) && a.Id.Equals(collection_id));

                if (asset == null)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = "Collection not found for this user"
                    });
                }

                asset.CollectionId = collection.Id;

                _appDbContext.Update(asset);

                await _appDbContext.SaveChangesAsync();

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

        [HttpDelete, Route("{id:Guid}/delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try 
            {

                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return Unauthorized();
                }

                var asset = _appDbContext.Assets.FirstOrDefault(a => a.Id == id && a.UserId.Equals(user.Id));

                if (asset == null)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = "Asset not found."
                    });
                }

                await _assetManager.RemoveAssetAsync(asset.Id);

                return Ok();

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

        /*
        [HttpGet, Route("{id:Guid}/download")]
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

            //var filePath = Path.Combine(_environment.WebRootPath, "uploads");
            byte[] fileBytes = System.IO.File.ReadAllBytes(asset.Url);

            return File(asset.Url, "application/x-msdownload", $"{asset.Name}.zip");

        }
        */
    }
}