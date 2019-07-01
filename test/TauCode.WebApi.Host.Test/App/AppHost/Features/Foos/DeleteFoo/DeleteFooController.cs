using Microsoft.AspNetCore.Mvc;
using System.Net;
using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.DeleteFoo;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.AppHost.Features.Foos.DeleteFoo
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
