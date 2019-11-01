using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetCurrency;
using TauCode.WebApi.Host.Tests.Domain.Currencies;
using TauCode.WebApi.Host.Tests.Domain.Currencies.Exceptions;
using TauCode.WebApi.Host.Validation;

namespace TauCode.WebApi.Host.Tests.App.Features.Currencies.GetCurrency
{
    [ApiController]
    public class GetCurrencyController : ControllerBase
    {
        private readonly IValidatingQueryRunner _queryRunner;

        public GetCurrencyController(IValidatingQueryRunner queryRunner)
        {
            _queryRunner = queryRunner;
        }

        [SwaggerOperation(Tags = new[] { "Currencies" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Get currency by id or code", typeof(GetCurrencyQueryResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Currency not found", typeof(ValidationErrorDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Currency not found")]
        [HttpGet]
        [Route("api/currencies/by-prop", Name = "GetCurrency")]
        public IActionResult GetCurrency([FromQuery]CurrencyId id = null, [FromQuery]string code = null)
        {
            var query = new GetCurrencyQuery
            {
                Id = id,
                Code = code,
            };

            try
            {
                _queryRunner.Run(query);
                var result = query.GetResult();
                return this.Ok(result);
            }
            catch (CurrencyNotFoundException ex)
            {
                return this.NotFoundError(ex);
            }
            catch (ValidationException ex)
            {
                return this.ValidationError(ex);
            }
        }
    }
}
