using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jahshaka.Admin.Controllers
{
  [Route("")]
  public class TokenController : Controller
  {
    [Route("connect/token")]
    public async Task<IActionResult> Token()
    {
      await Task.Run(() => { });


      return Ok(new
      {
        access_token = Guid.NewGuid().ToString(),
        refresh_token = Guid.NewGuid().ToString()
      });
    }
  }
}
