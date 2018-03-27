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
using Jahshaka.Core.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Jahshaka.Core.Enums;
using Jahshaka.API.ViewModels.Application;
using Jahshaka.API.ViewModels.Applications;

namespace Jahshaka.API.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("admin/applications")]
    public class ApplicationController : BaseController
    {
        
        public ApplicationController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment,
            ILoggerFactory loggerFactory
        ) : base(appDbContext, loggerFactory, environment, userManager)
        {
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Index(int? page, int? size)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType != UserType.Admin)
                {
                    return Unauthorized();
                }

                int pageNumber = page ?? 1;
                int pageSize = 20;

                var queryable = _dbContext.Applications
                    .Include(a => a.Versions)
                    .AsQueryable();

                var total = queryable.Count();

                var pageResult = queryable.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = pageResult.GroupBy(u => new {Total = total})
                .FirstOrDefault();

                var model = new PagedListViewModel<ApplicationViewModel>()
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
                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateApplicationViewModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType != UserType.Admin)
                {
                    return Unauthorized();
                }

                if (ModelState.IsValid)
                {
                    
                    var application = new Application{
                        Id = Guid.NewGuid(),
                        ClientId = model.ClientId,
                        ClientSecret = model.ClientSecret,
                        DisplayName = model.DisplayName,
                        PostLogoutRedirectUris = model.PostLogoutRedirectUris,
                        RedirectUris  = model.RedirectUris,
                        Type = model.Type
                    };

                    _dbContext.Applications.Add(application);
                    
                    _dbContext.SaveChanges();

                    return Ok(application.ToViewModel());

                }

                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = ModelState?.GetFirstError()
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }

        [HttpGet, Route("{id:guid}/remove")]
        public async Task<IActionResult> Remove(Guid id)
        {
             try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType != UserType.Admin)
                {
                    return Unauthorized();
                }

                var application = _dbContext.Applications.FirstOrDefault(a => a.Id == id);

                if(application == null){
                    return BadRequest(new ErrorViewModel
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = $"Application '{id}' not found"
                    });
                }

                _dbContext.Remove(application);
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType != UserType.Admin)
                {
                    return Unauthorized();
                }

                var queryable = _dbContext.Applications
                    .Include(a => a.Versions)
                    .FirstOrDefault(a => a.Id == id);

                if(queryable == null){
                    return BadRequest(new ErrorViewModel
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = "Application not found"
                    });
                }

                return Ok(queryable.ToViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }

        [HttpPost, Route("{id:guid}/version/add")]
        public async Task<IActionResult> Add(Guid id, [FromBody] AddVersionViewModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType != UserType.Admin)
                {
                    return Unauthorized();
                }

                if (ModelState.IsValid)
                {
                    var application = _dbContext.Applications
                        .FirstOrDefault(a => a.Id == id);

                    if(application == null){
                        return BadRequest(new ErrorViewModel
                        {
                            Error = ErrorCode.ModelError,
                            ErrorDescription = $"Application '{id}' not found"
                        });
                    }
                    
                    var version = new ApplicationVersion{
                        Id = model.Id,
                        ApplicationId = application.Id,
                        DownloadUrl = model.DownloadUrl,
                        Notes = model.Notes,
                        Supported = model.Supported,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt  = DateTime.UtcNow
                    };

                    _dbContext.ApplicationVersions.Add(version);
                    
                    _dbContext.SaveChanges();

                    return Ok(version.ToViewModel());

                }

                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = ModelState?.GetFirstError()
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }

        [HttpGet, Route("{id:guid}/versions/{vid}/enable")]
        public async Task<IActionResult> Enable(Guid id, string vid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType != UserType.Admin)
                {
                    return Unauthorized();
                }

                var version = _dbContext.ApplicationVersions
                    .FirstOrDefault(a => a.ApplicationId == id && a.Id.Equals(vid));

                if(version == null){
                    return BadRequest(new ErrorViewModel
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = $"Application '{id}' and version '{vid}' not found"
                    });
                }

                version.Supported = true;

                _dbContext.Update(version);
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }

        [HttpGet, Route("{id:guid}/versions/{vid}/disable")]
        public async Task<IActionResult> Disable(Guid id, string vid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType != UserType.Admin)
                {
                    return Unauthorized();
                }

                var version = _dbContext.ApplicationVersions
                    .FirstOrDefault(a => a.ApplicationId == id && a.Id.Equals(vid));

                if(version == null){
                    return BadRequest(new ErrorViewModel
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = $"Application '{id}' and version '{vid}' not found"
                    });
                }

                version.Supported = false;

                _dbContext.Update(version);
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }

        [HttpGet, Route("{id:guid}/versions/{vid}/remove")]
        public async Task<IActionResult> RemoveVersion(Guid id, string vid)
        {
             try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || user.UserType != UserType.Admin)
                {
                    return Unauthorized();
                }

                var version = _dbContext.ApplicationVersions
                    .FirstOrDefault(a => a.ApplicationId == id && a.Id.Equals(vid));

                if(version == null){
                    return BadRequest(new ErrorViewModel
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = $"Application '{id}' and version '{vid}' not found"
                    });
                }

                _dbContext.Remove(version);
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorViewModel { Error = ErrorCode.ServerError, ErrorDescription = GetEnvironmentErrorMessage(ex) });
            }
        }
    }
}