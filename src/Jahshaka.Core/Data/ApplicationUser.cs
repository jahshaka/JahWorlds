﻿using System;
using System.Collections.Generic;
using Jahshaka.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Jahshaka.Core.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {

        public ApplicationUser()
        {
            Assets = new HashSet<Asset>();
            Worlds = new HashSet<World>();
        }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserType UserType { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }
        
        public virtual ICollection<World> Worlds { get; set; }
    }
}
