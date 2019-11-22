using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.UpdateCurrency
{
    public class UpdateCurrencyCommand : ICommand
    {
        public CurrencyId Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
