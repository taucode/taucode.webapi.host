using Microsoft.AspNetCore.Mvc;

namespace TauCode.WebApi.Host.Tests.App.Features.Misc.ReturnDeletedNoContent
{
    [ApiController]
    public class ReturnDeletedNoContentController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/deleted-no-content")]
        public IActionResult ReturnDeletedNoContent()
        {
            return this.DeletedNoContent("deleted-id");
        }
    }
}
