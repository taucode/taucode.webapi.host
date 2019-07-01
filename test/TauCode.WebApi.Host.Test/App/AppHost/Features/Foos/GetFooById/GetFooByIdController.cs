using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.AppHost.Features.Foos.GetFooById
{
    [ApiController]
    public class GetFooByIdController : ControllerBase
    {
        private readonly IQueryRunner _queryRunner;

        public GetFooByIdController(IQueryRunner queryRunner)
        {
            _queryRunner = queryRunner;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, "Get foo by ID", typeof(GetFooByIdQueryResult))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Foo not found")]
        [HttpGet]
        [Route("api/foos/{id}", Name = "GetFooById")]
        public IActionResult GetFooById([FromRoute]FooId id)
        {
            var query = new GetFooByIdQuery
            {
                Id = id,
            };

            try
            {
                _queryRunner.Run(query);
                var result = query.GetResult();
                return this.Ok(result);
            }
            catch (ValidationException ex)
            {
                return this.ValidationError(ex);
            }
        }
    }
}
