using System.Collections.Generic;
using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetAllCurrencies
{
    public class GetAllCurrenciesQueryResult
    {
        public IList<CurrencyDto> Items { get; set; }
        public class CurrencyDto
        {
            public CurrencyId Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }
    }
}
