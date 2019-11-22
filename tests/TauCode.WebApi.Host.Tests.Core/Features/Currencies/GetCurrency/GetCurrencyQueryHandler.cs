using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Tests.Domain.Currencies;
using TauCode.WebApi.Host.Tests.Domain.Currencies.Exceptions;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetCurrency
{
    public class GetCurrencyQueryHandler : QueryHandlerBase<GetCurrencyQuery>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public GetCurrencyQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public override void Execute(GetCurrencyQuery query)
        {
            Currency currency = null;

            if (query.Id != null)
            {
                currency = _currencyRepository.GetById(query.Id);
            }
            else if (query.Code != null)
            {
                currency = _currencyRepository.GetByCode(query.Code);
            }

            if (currency == null)
            {
                throw new CurrencyNotFoundException();
            }

            var queryResult = new GetCurrencyQueryResult
            {
                Id = currency.Id,
                Code = currency.Code,
                Name = currency.Name,
            };

            query.SetResult(queryResult);
        }
    }
}
