using System;
using OpenIddict.Models;

namespace Jahshaka.AuthServer.Models
{
    public class Token : OpenIddictToken<Guid, Application, Authorization>
    {
    }
}