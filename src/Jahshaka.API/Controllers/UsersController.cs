using System;
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
using Microsoft.Extensions.Logging;

namespace Jahshaka.API.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("users")]
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _appDbContext;
        private IHostingEnvironment _environment;
        protected ILogger _logger;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment,
            ILoggerFactory loggerFactory
        ) : base(appDbContext, loggerFactory, environment, userManager)
        {
            _appDbContext = appDbContext;
            _environment = environment;
            _logger = loggerFactory.CreateLogger<BaseController>();
        }
        
        [HttpGet, Route("me")]
        public async Task<IActionResult> GetCurrentUserAction()
        {
            try
            {

                var user = await GetCurrentUserAsync();

                if (user == null)
                {
                    return Unauthorized();
                }

                return Ok(user.ToViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }
    }
}