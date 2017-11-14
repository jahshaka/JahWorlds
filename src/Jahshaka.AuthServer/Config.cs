using System.Collections.Generic;
using CryptoHelper;
using OpenIddict.Core;
using System;
using System.Linq;

namespace Jahshaka.AuthServer
{
    public class Config
    {
        public static IEnumerable<OpenIddictApplicationDescriptor> GetApplications()
        {
            var applications = new List<OpenIddictApplicationDescriptor>();


            // Add Mvc.Client to the known applications.
            applications.Add(new OpenIddictApplicationDescriptor
            {
                ClientId = "admin",
                ClientSecret = Crypto.HashPassword("secret_secret_secret"),
                DisplayName = "Admin Portal",
                PostLogoutRedirectUris = { new Uri("http://localhost:7000/signout-callback-oidc") },
                RedirectUris = { new Uri("http://localhost:7000/signin-oidc") },
                Type = OpenIddictConstants.ClientTypes.Confidential
            });

            // Add Mvc.Client to the known applications.
            applications.Add(new OpenIddictApplicationDescriptor
            {
                ClientId = "myClient",
                ClientSecret = Crypto.HashPassword("secret_secret_secret"),
                DisplayName = "My client application",
                PostLogoutRedirectUris = { new Uri("http://localhost:5006/signout-callback-oidc") },
                RedirectUris = { new Uri("http://localhost:5006/signin-oidc") },
                Type = OpenIddictConstants.ClientTypes.Confidential
            });

            // To test this sample with Postman, use the following settings:
            //
            // * Authorization URL: http://localhost:54540/connect/authorize
            // * Access token URL: http://localhost:54540/connect/token
            // * Client ID: postman
            // * Client secret: [blank] (not used with public clients)
            // * Scope: openid email profile roles
            // * Grant type: authorization code
            // * Request access token locally: yes
            applications.Add(new OpenIddictApplicationDescriptor
            {
                ClientId = "postman",
                DisplayName = "Postman",
                RedirectUris = { new Uri("https://www.getpostman.com/oauth2/callback") },
                Type = OpenIddictConstants.ClientTypes.Public
            });

            return applications;
        }
    }
}
