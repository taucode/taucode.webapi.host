using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetCurrency
{
    public class GetCurrencyQueryResult
    {
        public CurrencyId Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
