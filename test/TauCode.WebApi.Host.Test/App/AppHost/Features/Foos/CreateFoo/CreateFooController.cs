using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using TauCode.Cqrs.Commands;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.App.AppHost.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.CreateFoo;
using TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.App.Domain.Foos.Exceptions;

namespace TauCode.WebApi.Host.Test.App.AppHost.Features.Foos.CreateFoo
{
    [ApiController]
    public class CreateFooController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryRunner _queryRunner;
        
        public CreateFooController(ICommandDispatcher commandDispatcher, IQueryRunner queryRunner)
        {
            _commandDispatcher = commandDispatcher;
            _queryRunner = queryRunner;
        }

        [SwaggerResponse(StatusCodes.Status204NoContent, "Foo has been created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad data for foo creation", typeof(ValidationErrorResponseDto))]
        [HttpPost]
        [Route("api/foos", Name = "CreateFoo")]
        public IActionResult CreateFoo([FromBody]CreateFooCommand command, [FromQuery]string info = null)
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
                throw new NotImplementedException();

                //var dd = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
                //return this.Forbid();
            }

            if (info == "id")
            {
                return this.NoContentWithId(command.GetResult().ToString());
            }

            if (info == "route")
            {
                var instanceLocation = this.Url.SingleAction(nameof(GetFooByIdController.GetFooById), new { id = command.GetResult(), });
                return this.NoContentWithId(command.GetResult().ToString(), instanceLocation);
            }

            if (info == "instance")
            {
                var query = new GetFooByIdQuery
                {
                    Id = command.GetResult(),
                };

                _queryRunner.Run(query);
                var content = query.GetResult();

                var location = this.Url.SingleAction(nameof(GetFooByIdController.GetFooById), new { id = command.GetResult(), });

                return this.Created(location, content);
            }

            return this.NoContent();
        }
    }
}
