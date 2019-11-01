using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TauCode.Cqrs.Commands;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Tests.Core.Exceptions;
using TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetCurrency;
using TauCode.WebApi.Host.Tests.Core.Features.Currencies.UpdateCurrency;
using TauCode.WebApi.Host.Tests.Domain.Currencies;
using TauCode.WebApi.Host.Tests.Domain.Currencies.Exceptions;

namespace TauCode.WebApi.Host.Tests.App.Features.Currencies.UpdateCurrency
{
    [ApiController]
    public class UpdateCurrencyController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryRunner _queryRunner;

        public UpdateCurrencyController(ICommandDispatcher commandDispatcher, IQueryRunner queryRunner)
        {
            _commandDispatcher = commandDispatcher;
            _queryRunner = queryRunner;
        }

        [SwaggerOperation(Tags = new[] { "Currencies" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Currency was updated and returned to user")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad data for currency updating", typeof(ValidationErrorDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Currency not found")]
        [Route("api/currencies/{id}", Name = "UpdateCurrency")]
        [HttpPut]
        public IActionResult UpdateCurrency([FromRoute]CurrencyId id, UpdateCurrencyCommand command)
        {
            command.Id = id;
            
            try
            {
                _commandDispatcher.Dispatch(command);

                var query = new GetCurrencyQuery
                {
                    Id = id,
                };

                _queryRunner.Run(query);
                var queryResult = query.GetResult();

                return this.Ok(queryResult);
            }
            catch (CurrencyNotFoundException ex)
            {
                return this.NotFoundError(ex);
            }
            catch (CodeAlreadyExistsException ex)
            {
                return this.ConflictError(ex);
            }
        }
    }
}
