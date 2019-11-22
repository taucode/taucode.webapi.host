using NHibernate;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Tests.Domain.Currencies;
using System.Linq;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetAllCurrencies
{
    public class GetAllCurrenciesQueryHandler : QueryHandlerBase<GetAllCurrenciesQuery>
    {
        private readonly ISession _session;

        public GetAllCurrenciesQueryHandler(ISession session)
        {
            _session = session;
        }

        public override void Execute(GetAllCurrenciesQuery query)
        {
            var currencies = _session
                .Query<Currency>()
                .OrderBy(x => x.Name)
                .ToList();

            var queryResult = new GetAllCurrenciesQueryResult
            {
                Items = currencies
                    .Select(x => new GetAllCurrenciesQueryResult.CurrencyDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    })
                    .ToList(),
            };
            query.SetResult(queryResult);
        }
    }
}
