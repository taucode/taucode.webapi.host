using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Tests.Core.Exceptions;
using TauCode.WebApi.Host.Tests.Domain.Currencies;
using TauCode.WebApi.Host.Tests.Domain.Currencies.Exceptions;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.UpdateCurrency
{
    public class UpdateCurrencyCommandHandler : CommandHandlerBase<UpdateCurrencyCommand>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public UpdateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public override void Execute(UpdateCurrencyCommand command)
        {
            var currency = _currencyRepository.GetById(command.Id);
            if (currency == null)
            {
                throw new CurrencyNotFoundException();
            }

            if (command.Code != null)
            {
                var currencyWithSameCode = _currencyRepository.GetByCode(command.Code);
                if (currencyWithSameCode != null && currencyWithSameCode.Id != command.Id)
                {
                    throw new CodeAlreadyExistsException();
                }

                currency.ChangeCode(command.Code);
            }

            if (command.Name != null)
            {
                currency.ChangeName(command.Name);
            }

            _currencyRepository.Save(currency);
        }
    }
}
