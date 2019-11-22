using System.Collections.Generic;
using FluentValidation;
using TauCode.Utils.Extensions;
using TauCode.Validation;
using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Core.Features.Currencies.UpdateCurrency
{
    public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>, IParameterValidator
    {
        public UpdateCurrencyCommandValidator()
        {
            this.RuleFor(x => this.GetCurrencyId())
                .NotNull()
                .WithName("Id");

            this.RuleFor(x => x.Code)
                .NotEmpty()
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Code)
                        .CurrencyCode();
                });

            this.RuleFor(x => x.Name)
                .NotEmpty()
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Name)
                        .FullName(1, 50);
                });
        }

        private CurrencyId GetCurrencyId()
        {
            return this.Parameters.GetOrDefault("id") as CurrencyId;
        }

        public IDictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}
