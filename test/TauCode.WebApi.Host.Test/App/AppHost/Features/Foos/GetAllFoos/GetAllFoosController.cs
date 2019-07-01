using Microsoft.AspNetCore.Mvc;
using System.Net;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetAllFoos;

namespace TauCode.WebApi.Host.Test.App.AppHost.Features.Foos.GetAllFoos
{
    public class GetAllFoosController : ApiController
    {
        private readonly IQueryRunner _queryRunner;
        public GetAllFoosController(IQueryRunner queryRunner)
        {
            _queryRunner = queryRunner;
        }
        [SwaggerResponse(HttpStatusCode.OK, "All foos", typeof(GetAllFoosQueryResult))]
        [HttpGet]
        [Route("api/foos", Name = "GetAllFoos")]
        public IHttpActionResult GetAllFoos()
        {
            var query = new GetAllFoosQuery();
            _queryRunner.Run(query);
            var result = query.GetResult();
            return this.Ok(result);
        }
    }
}
