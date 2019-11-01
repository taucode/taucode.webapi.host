using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.ReturnBadRequestError
{
    [ApiController]
    public class ReturnBadRequestErrorController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/bad-request")]
        public IActionResult ReturnBadRequestError()
        {
            throw new NotImplementedException();
        }
    }
}
