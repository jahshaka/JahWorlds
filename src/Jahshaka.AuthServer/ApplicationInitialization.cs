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

namespace Jahshaka.AuthServer
{
    public class ApplicationInitialization
    {
        private ApplicationDbContext _dbContext;
        private readonly OpenIddictApplicationManager<Application> _applicationManager;

        public ApplicationInitialization(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            _dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _applicationManager = serviceScope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<Application>>();
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

        public void Run()
        {
            InitializeApplications();
        }
    }
}
