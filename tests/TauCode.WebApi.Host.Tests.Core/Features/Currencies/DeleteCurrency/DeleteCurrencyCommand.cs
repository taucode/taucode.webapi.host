using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.DeleteCurrency
{
    public class DeleteCurrencyCommand : ICommand
    {
        public CurrencyId Id { get; set; }
    }
}
