using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jahshaka.API.Constants;
using Jahshaka.API.ViewModels.Mappers;
using Jahshaka.API.ViewModels.Shared;
using Jahshaka.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNet.Security.OAuth.Validation;
using Jahshaka.API.ViewModels.World;
using Jahshaka.Core.Managers;
using Microsoft.EntityFrameworkCore;

namespace Jahshaka.API.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("world")]
    public class WorldController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _appDbContext;
        private IHostingEnvironment _environment;
        private readonly AssetManager _assetManager;

        public WorldController(
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

            var worlds = _appDbContext.Worlds
                .Include(w => w.WorldVersions)
                .Where(w => w.UserId == user.Id)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            var viewModel = new ListWorldViewModel()
            {
                Worlds = worlds.ToViewModel()
            };

            return Ok(viewModel);
        }
        
        [HttpPost, Route("")]
        public async Task<IActionResult> Create([FromBody] CreateWorldViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return Unauthorized();
                }

                var world = new World()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    ThumbnailUrl = model.ThumbnailUrl,
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.Id
                };

                world.WorldVersions.Add(new WorldVersion()
                {
                    WorldId = world.Id,
                    VersionNumber = model.VersionNumber
                });

                _appDbContext.Worlds.Add(world);

                await _appDbContext.SaveChangesAsync();

                return Ok(world.ToViewModel());
            }
            
            return BadRequest(new ErrorViewModel()
            {
                Error = ErrorCode.ModelError,
                ErrorDescription = ModelState?.GetFirstError()
            });
        }
        
        [HttpGet, Route("{id:guid}")]
        public async Task<IActionResult> GetWorld(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var world = _appDbContext.Worlds.Include(w => w.WorldVersions)
                .FirstOrDefault(w => w.Id == id && w.UserId == user.Id);

            if (world == null)
            {
                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = "World not found."
                });
            }

            return Ok(world.ToViewModel());
        }
        
        [HttpGet, Route("{id:guid}/version/{version_number:float}")]
        public async Task<IActionResult> GetWorldVersion(Guid id, float versionNumber)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var worldVersion = _appDbContext.WorldVersions.Include(w => w.World)
                .FirstOrDefault(w => w.VersionNumber.Equals(versionNumber) && w.World.Id == id && w.World.UserId == user.Id);

            if (worldVersion == null)
            {
                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = $"Version number {versionNumber} was not found for world {id}."
                });
            }

            return Ok(worldVersion.ToVersionViewModel());
        }
        
        
        [HttpGet, Route("{id:guid}/version/{version_number:float}/assets")]
        public async Task<IActionResult> GetWorldVersionAssets(Guid id, float versionNumber)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var assets = _appDbContext.WorldVersionAssets
                .Include(w => w.WorldVersion).ThenInclude(w => w.World)
                .Include(w => w.Asset)
                .Where(w => w.WorldVersion.VersionNumber.Equals(versionNumber) && w.WorldVersion.WorldId == id && w.WorldVersion.World.UserId == user.Id)
                .ToList();

            if (assets == null)
            {
                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = $"Version number {versionNumber} was not found for world {id}."
                });
            }

            return Ok(assets.ToViewModel());
        }
        
        [HttpPost, Route("{id:guid}/version/{version_number:float}/asset/upload")]
        public async Task<IActionResult> Upload(CreateWorldAssetViewModel model)
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
    }
}