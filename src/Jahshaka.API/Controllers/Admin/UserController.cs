using System;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Jahshaka.API.Constants;
using Jahshaka.API.ViewModels.Mappers;
using Jahshaka.API.ViewModels.Shared;
using Jahshaka.API.ViewModels.User;
using Jahshaka.Core.Data;
using Jahshaka.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jahshaka.API.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("admin/users")]
    public class UsersController : BaseController
    {
        public UsersController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment,
            ILoggerFactory loggerFactory
        ) : base(appDbContext, loggerFactory, environment, userManager)
        {
        }
        
        [HttpGet, Route("")]
        public async Task<IActionResult> Index(int? page)
        {
            try
            {

                var user = await GetCurrentUserAsync();

                if (user == null || !user.UserType.Equals(UserType.Admin))
                {
                    return Unauthorized();
                }

                int pageNumber = page ?? 1;
                int pageSize = 20;

                var queryable = _dbContext.Users
                    .OrderByDescending(c => c.CreatedAt);  
                    
                var total = queryable.Count();

                var pageResult = queryable.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var model = new PagedListViewModel<UserViewModel>()
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
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }
    }
}