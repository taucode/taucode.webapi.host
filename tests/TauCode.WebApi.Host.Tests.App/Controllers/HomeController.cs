using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host.Tests.App.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("conflict-exception")]
        public IActionResult ConflictException()
        {
            var exception = new InvalidOperationException("Wrong operation.");
            return this.ConflictError(exception);
        }

        [HttpDelete]
        [Route("delete-with-id")]
        public IActionResult DeleteWithId([FromQuery]string desiredId)
        {
            return this.DeletedNoContent(desiredId);
        }
    }
}
