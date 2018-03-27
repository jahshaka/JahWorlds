using System;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Jahshaka.API.Constants;
using Jahshaka.API.ViewModels.Mappers;
using Jahshaka.API.ViewModels.Shared;
using Jahshaka.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jahshaka.API.Controllers
{
    [Route("applications")]
    public class ApplicationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _appDbContext;
        private IHostingEnvironment _environment;
        
        public ApplicationController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment
        )
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _environment = environment;
        }

        [HttpGet, Route("{application_id:Guid}/latest_version/{version_id}")]
        public  IActionResult LatestVersion(Guid application_id, string version_id)
        {
            try{
                var latest_version = _appDbContext.ApplicationVersions
                    .Where(a => a.ApplicationId.Equals(application_id))
                    .OrderByDescending(a => a.CreatedAt)
                    .FirstOrDefault();

                if(latest_version == null)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = "No version found"
                    });
                }

                var latest = latest_version.ToLatestViewModel();

                var application_version = _appDbContext.ApplicationVersions
                    .Where(a => a.Id.Equals(version_id) && a.ApplicationId.Equals(application_id))
                    .FirstOrDefault();
                    
                if(application_version == null )
                {
                    return Ok(latest);
                }

                var current = application_version.ToLatestViewModel();            

                if(current.CreatedAt >= latest.CreatedAt)
                {
                    current.ShouldUpdate = false;
                    return Ok(current);
                } else {
                    return Ok(latest);
                }

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
    }
}