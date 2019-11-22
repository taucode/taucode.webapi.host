using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Tests.Core.Exceptions;
using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.CreateCurrency
{
    public class CreateCurrencyCommandHandler : CommandHandlerBase<CreateCurrencyCommand>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CreateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public override void Execute(CreateCurrencyCommand command)
        {
            var existingCurrency = _currencyRepository.GetByCode(command.Code);
            if (existingCurrency != null)
            {
                throw new CodeAlreadyExistsException();
            }

            var currency = new Currency(command.Code, command.Name);
            _currencyRepository.Save(currency);
            command.SetResult(currency.Id);
        }
    }
}
