using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.UpdateFoo;
using TauCode.WebApi.Host.Test.App.Domain.Foos;
using TauCode.WebApi.Host.Test.App.Domain.Foos.Exceptions;

namespace TauCode.WebApi.Host.Test.App.AppHost.Features.Foos.UpdateFoo
{
    [ApiController]
    public class UpdateFooController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public UpdateFooController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, "Foo has been updated")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad data for foo updating", typeof(ValidationErrorResponseDto))]
        [Route("api/foos/{id}", Name = "UpdateFoo")]
        [HttpPut]
        public IActionResult UpdateFoo([FromRoute]FooId id, [FromBody]UpdateFooCommand command, [FromQuery]string info = null)
        {
            command.Id = id;

            try
            {
                _commandDispatcher.Dispatch(command);
            }
            catch (FooException e)
            {
                return this.BusinessLogicError(e);
            }
            catch (ForbiddenFooException e)
            {
                return this.ForbiddenError(e);
            }

            if (info == "id")
            {
                return this.NoContentWithId(id.ToString());
            }

            if (info == "route")
            {
                var route = this.FormatSiblingRoute("GetFooById", new { id = id, });
                return this.NoContentWithId(id.ToString(), route);
            }

            if (info == "instance")
            {
                return this.OkWithIdAndContent<GetFooByIdQueryResult>("GetFooById", new { id = id, });
            }

            return this.NoContent();
        }
    }
}
