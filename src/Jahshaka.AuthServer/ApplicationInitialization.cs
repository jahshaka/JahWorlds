using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OpenIddict.Core;
using System.Threading;
using Jahshaka.Core.Data;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Models;
using Jahshaka.Core.Enums;

namespace Jahshaka.AuthServer
{
    public class ApplicationInitialization
    {
        private ApplicationDbContext _dbContext;
        private readonly OpenIddictApplicationManager<Application> _applicationManager;
        protected ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationInitialization(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            _dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _applicationManager = serviceScope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<Application>>();
            _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _logger = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<ApplicationInitialization>();
        }

        public void InitializeApplications()
        {
            if (!_dbContext.Applications.Any())
            {
                Config.GetApplications().ToList().ForEach(application =>
                {
                    _applicationManager.CreateAsync(application, default(CancellationToken)).Wait();
                });

                _dbContext.SaveChanges();
            }
        }

        public void InitializeUsers()
        {
            _logger.LogInformation("Initializing users.");

            Config.GetUsers().ToList().ForEach(u =>
            {
                if (!_dbContext.Users.Any(m => m.Email.Equals(u.EmailAddress)))
                {
                    try
                    {

                        var user = new ApplicationUser
                        {
                            UserName = u.EmailAddress,
                            Email = u.EmailAddress,
                            EmailConfirmed = true,
                            CreatedAt = DateTime.UtcNow,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            PhoneNumber = u.PhoneNumber,
                            PhoneNumberConfirmed = true,
                            UserType = UserType.Admin
                        };

                        var task = _userManager.CreateAsync(user, u.Password);

                        task.Wait();

                        var result = task.Result;

                        if (result.Succeeded)
                        {
                            _logger.LogInformation("Initial user created.");
                        }
                        else
                        {
                            _logger.LogError("Unable to create initial user");
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            });
        }

        public void Run()
        {
            InitializeApplications();

            InitializeUsers();
        }
    }
}
