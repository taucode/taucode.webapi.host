using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;
using TauCode.Cqrs.Commands;
using TauCode.WebApi.Dto;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.DeleteFoo;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.AppHost.Features.Foos.DeleteFoo
{
    public class DeleteFooController : ApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public DeleteFooController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        [SwaggerResponse(HttpStatusCode.NoContent, "Foo has been deleted")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Bad data for foo deletion", typeof(ValidationErrorResponseDto))]
        [Route("api/foos/{id}", Name = "DeleteFoo")]
        [HttpDelete]
        public IHttpActionResult DeleteFoo([FromUri]FooId id)
        {
            var command = new DeleteFooCommand
            {
                Id = id,
            };
            _commandDispatcher.Dispatch(command);
            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}
