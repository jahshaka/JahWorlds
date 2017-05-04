using System;
using OpenIddict.Models;

namespace Jahshaka.AuthServer.Models
{
    public class Authorization : OpenIddictAuthorization<Guid, Application, Token>
    {
    }
}