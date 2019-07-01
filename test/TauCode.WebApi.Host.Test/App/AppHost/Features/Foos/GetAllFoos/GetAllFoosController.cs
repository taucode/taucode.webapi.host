using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.GetAllFoos;

namespace TauCode.WebApi.Host.Test.FooManagement.AppHost.Features.Foos.GetAllFoos
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
