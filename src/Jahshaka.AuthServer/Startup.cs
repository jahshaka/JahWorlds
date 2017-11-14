using System;
using System.Reflection;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Models;
using Jahshaka.Core.Data;
using Jahshaka.Core.DataProtection.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Jahshaka.AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            // Add framework services.
            services.AddDataProtection()
                .SetApplicationName("Jahshaka")
                .AddKeyManagementOptions(options =>
                {
                    options.XmlRepository = services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<IXmlRepository>();
                });

            services.AddOptions();

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Configure the context to use an in-memory store.
                //options.UseInMemoryDatabase();
                options.UseNpgsql
                    (Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(migrationsAssembly));

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict<Application, Authorization, Scope, Token, Guid>();
            });

            // Register the Identity services.
            /*
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext, Guid>()
                .AddDefaultTokenProviders();
            */

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");

            services.AddAuthentication().AddCookie()
                .AddOAuthValidation(options => {
                    options.SaveToken = true;
                })
                /*.AddOpenIdConnect(options => {
                    options.SaveTokens = true;
                    options.ClientId = "myClient";
                    options.ClientSecret = "secret_secret_secret";
                }) */
                .AddFacebook(options => {
                    options.AppId = "1788029188150533"; //Configuration["Facebook:AppId"]; 
                    options.AppSecret = "829d42ac7193d0522ee8e6f50d98e473"; //Configuration["Facebook:AppSecret"];
                    options.SaveTokens = true;
                })
                .AddGoogle(options => {
                    options.ClientId = "312477755191-aq6a2esar48u7pavkhob4kli7m4295ic.apps.googleusercontent.com";
                    options.ClientSecret = "5UxcqWRQkDnTudLZGYniOG5t";
                    options.SaveTokens = true;
                });

            // Register the OpenIddict services.
            services.AddOpenIddict<Application, Authorization, Scope, Token>(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<ApplicationDbContext>();

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

                // Enable the token endpoint.
                options.EnableAuthorizationEndpoint("/connect/authorize")
                       .EnableLogoutEndpoint("/connect/logout")
                       .EnableTokenEndpoint("/connect/token")
                       .EnableUserinfoEndpoint("/api/userinfo");

                // Enable the password flow.
                options.AllowAuthorizationCodeFlow()
                       .AllowPasswordFlow()
                       .AllowRefreshTokenFlow()
                       .AllowCustomFlow("urn:ietf:params:oauth:grant-type:external_account");

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                // Note: to use JWT access tokens instead of the default
                // encrypted format, the following lines are required:
                //
                // options.UseJsonWebTokens();
                // options.AddEphemeralSigningKey();

                services.AddCors();
            });

            services.AddTransient<IXmlRepository, DatabaseXmlRepository>();
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            app.UseDeveloperExceptionPage();

            app.UseCors(policyBuilder =>
            {
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowCredentials();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpMethodOverride();

            // Add a middleware used to validate access
            // tokens and protect the API endpoints.

            //app.UseOAuthValidation();

            // If you prefer using JWT, don't forget to disable the automatic
            // JWT -> WS-Federation claims mapping used by the JWT middleware:
            //
            // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            // JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            //
            // app.UseJwtBearerAuthentication(new JwtBearerOptions
            // {
            //     Authority = "http://localhost:58795/",
            //     Audience = "resource_server",
            //     RequireHttpsMetadata = false,
            //     TokenValidationParameters = new TokenValidationParameters
            //     {
            //         NameClaimType = OpenIdConnectConstants.Claims.Subject,
            //         RoleClaimType = OpenIdConnectConstants.Claims.Role
            //     }
            // });

            // Alternatively, you can also use the introspection middleware.
            // Using it is recommended if your resource server is in a
            // different application/separated from the authorization server.
            //
            // app.UseOAuthIntrospection(options =>
            // {
            //     options.Authority = new Uri("http://localhost:58795/");
            //     options.Audiences.Add("resource_server");
            //     options.ClientId = "resource_server";
            //     options.ClientSecret = "875sqd4s5d748z78z7ds1ff8zz8814ff88ed8ea4z4zzd";
            //     options.RequireHttpsMetadata = false;
            // });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            loggerFactory.CreateLogger<Startup>().LogInformation("Application Configuration completed.");

            var initializer = new ApplicationInitialization(app);

            initializer.Run();
        }
        
    }
}
