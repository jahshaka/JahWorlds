using System;
using OpenIddict.Models;

namespace Jahshaka.Core.Models
{
    public class Token : OpenIddictToken<Guid, Application, Authorization>
    {
    }
}