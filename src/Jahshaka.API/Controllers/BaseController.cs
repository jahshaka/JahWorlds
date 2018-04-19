using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Jahshaka.Core.Data;
using Microsoft.AspNetCore.Identity;

namespace Jahshaka.API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected ILogger _logger;
        protected readonly IHostingEnvironment _env;

        protected BaseController(ApplicationDbContext dbContext, ILoggerFactory loggerFactory, IHostingEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<BaseController>();
            _env = env;
            _userManager = userManager;
        }

        protected async Task<ApplicationUser> GetUserByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.Users.FirstOrDefaultAsync(e => e.Id.Equals(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return await Task.FromResult<ApplicationUser>(null);
            }
        }

        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var id = _userManager.GetUserId(User);

            Console.WriteLine(id);

            if (id == null)
            {
                return await Task.FromResult<ApplicationUser>(null);
            }

            try
            {
                return await GetUserByIdAsync(Guid.Parse(id));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);

                return await Task.FromResult<ApplicationUser>(null);
            }
        }

        protected string GetEnvironmentErrorMessage(Exception exception)
        {
            if(_env != null)
            {
                var exceptionMessage = exception.InnerException != null ? exception.InnerException.Message : exception.Message;

                return _env.IsProduction() ? "An unexpected error has occured." : exceptionMessage;
            }

            return "Unable to determine environment.";
        }
    }
}
