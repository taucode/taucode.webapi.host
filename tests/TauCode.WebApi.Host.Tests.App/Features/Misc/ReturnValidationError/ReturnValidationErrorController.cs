using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.ReturnValidationError
{
    [ApiController]
    public class ReturnValidationErrorController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/validation")]
        public IActionResult ReturnValidationError()
        {
            throw new NotImplementedException();
        }
    }
}
