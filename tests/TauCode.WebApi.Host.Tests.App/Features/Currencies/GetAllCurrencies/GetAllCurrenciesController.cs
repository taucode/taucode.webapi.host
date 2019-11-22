using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetAllCurrencies;

namespace TauCode.WebApi.Host.Tests.App.Features.Currencies.GetAllCurrencies
{
    [ApiController]
    public class GetAllCurrenciesController : ControllerBase
    {
        private readonly IQueryRunner _queryRunner;

        public GetAllCurrenciesController(IQueryRunner queryRunner)
        {
            _queryRunner = queryRunner;
        }

        [SwaggerOperation(Tags = new[] { "Currencies" })]
        [SwaggerResponse(StatusCodes.Status200OK, "All currencies", typeof(GetAllCurrenciesQueryResult))]
        [HttpGet]
        [Route("api/currencies", Name = "GetAllCurrencies")]
        public IActionResult GetAllCurrencies()
        {
            var query = new GetAllCurrenciesQuery();
            _queryRunner.Run(query);
            var result = query.GetResult();
            return this.Ok(result);
        }
    }
}
