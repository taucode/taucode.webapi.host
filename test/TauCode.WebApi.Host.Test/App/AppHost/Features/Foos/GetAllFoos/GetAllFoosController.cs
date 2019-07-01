using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetAllFoos;

namespace TauCode.WebApi.Host.Test.App.AppHost.Features.Foos.GetAllFoos
{
    [ApiController]
    public class GetAllFoosController : ControllerBase
    {
        private readonly IQueryRunner _queryRunner;

        public GetAllFoosController(IQueryRunner queryRunner)
        {
            _queryRunner = queryRunner;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, "All foos", typeof(GetAllFoosQueryResult))]
        [HttpGet]
        [Route("api/foos", Name = "GetAllFoos")]
        public IActionResult GetAllFoos()
        {
            var query = new GetAllFoosQuery();
            _queryRunner.Run(query);
            var result = query.GetResult();
            return this.Ok(result);
        }
    }
}
