using System;
using System.Collections.Generic;
using OpenIddict.Models;

namespace Jahshaka.AuthServer.Models
{
    public class Application: OpenIddictApplication<Guid, Authorization, Token>
    {
        public Application(): base()
        {
            Versions = new HashSet<ApplicationVersion>();
        }

        public virtual ICollection<ApplicationVersion> Versions { get; set; }

    }
}