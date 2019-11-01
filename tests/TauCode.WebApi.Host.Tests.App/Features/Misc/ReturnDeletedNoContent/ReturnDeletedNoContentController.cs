using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.ReturnDeletedNoContent
{
    [ApiController]
    public class ReturnDeletedNoContentController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/deleted-no-content")]
        public IActionResult ReturnDeletedNoContent()
        {
            throw new NotImplementedException();
        }
    }
}
