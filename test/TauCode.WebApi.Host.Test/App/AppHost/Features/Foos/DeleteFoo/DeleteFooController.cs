using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.DeleteFoo;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.AppHost.Features.Foos.DeleteFoo
{
    [ApiController]
    public class DeleteFooController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteFooController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [SwaggerResponse(StatusCodes.Status204NoContent, "Foo has been deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad data for foo deletion", typeof(ValidationErrorResponseDto))]
        [Route("api/foos/{id}", Name = "DeleteFoo")]
        [HttpDelete]
        public IActionResult DeleteFoo([FromRoute]FooId id)
        {
            var command = new DeleteFooCommand
            {
                Id = id,
            };
            _commandDispatcher.Dispatch(command);
            return this.NoContent();
        }
    }
}
