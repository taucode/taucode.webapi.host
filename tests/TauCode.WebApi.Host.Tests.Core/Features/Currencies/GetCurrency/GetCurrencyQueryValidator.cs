using TauCode.Validation;
using TauCode.WebApi.Host.Validation;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.GetCurrency
{
    public class GetCurrencyQueryValidator : CodedEntityValidator<GetCurrencyQuery, CurrencyCodeValidator>
    {
    }
}
