using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Tests.Core.Features.Currencies.DeleteCurrency;
using TauCode.WebApi.Host.Tests.Domain.Currencies;
using TauCode.WebApi.Host.Tests.Domain.Currencies.Exceptions;

namespace TauCode.WebApi.Host.Tests.App.Features.Currencies.DeleteCurrency
{
    [ApiController]
    public class DeleteCurrencyController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteCurrencyController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [SwaggerOperation(Tags = new[] { "Currencies" })]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Currency was deleted")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Currency was not found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Currency is in use")]
        [Route("api/currencies/{id}", Name = "DeleteCurrency")]
        [HttpDelete]
        public IActionResult DeleteCurrency([FromRoute]CurrencyId id)
        {
            var command = new DeleteCurrencyCommand
            {
                Id = id,
            };

            try
            {
                _commandDispatcher.Dispatch(command);

                return this.DeletedNoContent(id.ToString());
            }
            catch (CurrencyNotFoundException ex)
            {
                return this.NotFoundError(ex);
            }
        }
    }
}
