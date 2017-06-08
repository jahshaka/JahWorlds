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

        public WorldController(
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
        
        [HttpGet, Route("{id:guid}/{version_number:float}")]
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
        
        
        [HttpGet, Route("{id:guid}/{version_number:float}/assets")]
        public async Task<IActionResult> GetWorldVersionAssets(Guid id, float versionNumber)
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
    }
}