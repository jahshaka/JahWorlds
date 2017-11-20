using System;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Jahshaka.API.Constants;
using Jahshaka.API.ViewModels.Collection;
using Jahshaka.API.ViewModels.Shared;
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
    [Route("admin/collections")]
    public class CollectionController : BaseController
    {
        private readonly ApplicationDbContext _appDbContext;
        protected ILogger _logger;
        
        public CollectionController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment,
            ILoggerFactory loggerFactory
        ) : base(appDbContext, loggerFactory, environment, userManager)
        {
            _logger = loggerFactory.CreateLogger<CollectionController>();
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Index(int? page = null)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType.Equals(UserType.Admin))
                {
                    return Unauthorized();
                }

                int pageNumber = page ?? 1;
                int pageSize = 20;

                var queryable = _dbContext.Collection.Where(c => c.CollectionId == 0).ToList();    
                    
                var total = queryable.Count();

                var pageResult = queryable.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = pageResult.GroupBy(u => new {Total = total})
                .FirstOrDefault();

                var model = new PagedListViewModel<CollectionViewModel>()
                {
                    Items = pageResult.ToViewModel(),

                    Paging = new PagingOptionsViewModel()
                    {
                        CurrentPage = pageNumber,
                        TotalItems = result?.Key.Total ?? 0,
                        PageSize = pageSize
                    }
                };

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return BadRequest(new ErrorViewModel { 
                    Error = ErrorCode.ServerError, 
                    ErrorDescription = GetEnvironmentErrorMessage(ex) 
                });
            }
        }
    }
}