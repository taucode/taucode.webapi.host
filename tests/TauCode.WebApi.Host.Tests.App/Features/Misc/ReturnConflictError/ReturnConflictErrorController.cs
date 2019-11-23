using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.ReturnConflictError
{
    [ApiController]
    public class ReturnConflictErrorController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/conflict")]
        public IActionResult ReturnConflictError()
        {
            var ex = new InvalidOperationException("Bad action!");
            return this.ConflictError(ex);
        }
    }
}
