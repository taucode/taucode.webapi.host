using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc
{
    [ApiController]
    public class ReturnNotFoundErrorController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/not-found")]
        public IActionResult ReturnNotFoundError()
        {
            var ex = new CultureNotFoundException("Bez kultur net multur.");
            return this.NotFoundError(ex);
        }
    }
}
