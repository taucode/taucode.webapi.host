using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using TauCode.Cqrs.Commands;
using TauCode.WebApi.Dto;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.CreateFoo;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos.Exceptions;

namespace TauCode.WebApi.Host.Test.FooManagement.AppHost.Features.Foos.CreateFoo
{
    public class CreateFooController : ApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public CreateFooController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        [SwaggerResponse(HttpStatusCode.NoContent, "Foo has been created")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Bad data for foo creation", typeof(ValidationErrorResponseDto))]
        [Route("api/foos", Name = "CreateFoo")]
        [HttpPost]
        public IHttpActionResult CreateFoo([FromBody]CreateFooCommand command, [FromUri]string info = null)
        {
            if (info == "raise-validation-error")
            {
                return this.ValidationError(
                    "the-code",
                    "Raised validation error as requested",
                    new Dictionary<string, ValidationFailureDto>
                    {
                        { "ergo", new ValidationFailureDto("SomethingWrong", "Something is wrong!") },
                        { "magari", new ValidationFailureDto("AnotherThingWrong", "Another thing is wrong!") },
                    });
            }

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
                return this.NoContentWithId(command.GetResult().ToString());
            }

            if (info == "route")
            {
                var route = this.FormatSiblingRoute("GetFooById", new { id = command.GetResult(), });
                return this.NoContentWithId(command.GetResult().ToString(), route);
            }

            if (info == "instance")
            {
                return this.CreatedWithIdAndContent<GetFooByIdQueryResult>("GetFooById", new { id = command.GetResult(), });
            }

            return this.NoContent();
        }
    }
}
