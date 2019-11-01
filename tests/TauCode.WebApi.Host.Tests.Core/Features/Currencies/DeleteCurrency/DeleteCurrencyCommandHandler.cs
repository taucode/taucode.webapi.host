using Microsoft.AspNetCore.Http;
using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Tests.Domain.Currencies;
using TauCode.WebApi.Host.Tests.Domain.Currencies.Exceptions;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.DeleteCurrency
{
    public class DeleteCurrencyCommandHandler : CommandHandlerBase<DeleteCurrencyCommand>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ISession _session;

        public DeleteCurrencyCommandHandler(ICurrencyRepository currencyRepository, ISession session)
        {
            _currencyRepository = currencyRepository;
            _session = session;
        }

        public override void Execute(DeleteCurrencyCommand command)
        {
            var deleted = _currencyRepository.Delete(command.Id);
            if (!deleted)
            {
                throw new CurrencyNotFoundException();
            }
        }
    }
}
