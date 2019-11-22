using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.CreateCurrency
{
    public class CreateCurrencyCommand : Command<CurrencyId>
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
