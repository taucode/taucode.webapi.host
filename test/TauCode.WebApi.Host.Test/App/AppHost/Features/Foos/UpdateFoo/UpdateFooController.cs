using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;
using TauCode.Cqrs.Commands;
using TauCode.WebApi.Dto;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.GetFooById;
using TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.UpdateFoo;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos.Exceptions;

namespace TauCode.WebApi.Host.Test.FooManagement.AppHost.Features.Foos.UpdateFoo
{
    public class UpdateFooController : ApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public UpdateFooController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        [SwaggerResponse(HttpStatusCode.NoContent, "Foo has been updated")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Bad data for foo updating", typeof(ValidationErrorResponseDto))]
        [Route("api/foos/{id}", Name = "UpdateFoo")]
        [HttpPut]
        public IHttpActionResult UpdateFoo([FromUri]FooId id, [FromBody]UpdateFooCommand command, [FromUri]string info = null)
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
