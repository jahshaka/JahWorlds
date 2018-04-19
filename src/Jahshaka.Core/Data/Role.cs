using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Jahshaka.Core.Data
{
    public class Role : IdentityRole<Guid>
    {
        public Role() : base() {}

    }
}
