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
        public async Task<IActionResult> Index(int? page, AssetType? type, int? is_public, string query)
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.UserType != UserType.Admin)
            {
                return Unauthorized();
            }

            int pageNumber = page ?? 1;
            int pageSize = 20;

            var queryable = _appDbContext.Assets
                .AsQueryable();  

            if(type != null){
                queryable = queryable.Where(a => a.Type.Equals(type));
            }

            if(is_public != null){
                bool isPublic = is_public == 0 ? false : true;
                queryable = queryable.Where(a => a.IsPublic.Equals(isPublic));
            }

            if(query != null){
                //queryable = queryable.Where(a => a.Type.Equals(type));
            }
                
            var total = queryable.Count();

            var pageResult = queryable
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PagedListViewModel<AssetViewModel>()
            {
                Items = pageResult.ToViewModel(),

                Paging = new PagingOptionsViewModel()
                {
                    CurrentPage = pageNumber,
                    TotalItems = total,
                    PageSize = pageSize
                }
            };

            return Ok(model);
        }
    }
}