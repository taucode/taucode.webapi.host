using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.ReturnNotFoundError
{
    [ApiController]
    public class ReturnNotFoundErrorController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/not-found")]
        public IActionResult ReturnNotFoundError()
        {
            throw new NotImplementedException();
        }
    }
}
