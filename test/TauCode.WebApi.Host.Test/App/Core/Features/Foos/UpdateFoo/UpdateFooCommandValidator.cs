using FluentValidation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TauCode.Utils.Extensions;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.UpdateFoo
{
    public class UpdateFooCommandValidator : AbstractValidator<UpdateFooCommand>, IParameterValidator
    {
        public UpdateFooCommandValidator()
        {
            this.RuleFor(x => x.Id)
                .NotNull();

            this.RuleFor(x => x.Name)
                .NotEmpty();

            this.RuleFor(x => x)
                .Must(DummyMustNotBeGocha)
                .WithErrorCode("NoGochaValidator")
                .WithMessage("'Dummy' should not be 'Gocha'.");
        }

        private static bool DummyMustNotBeGocha(UpdateFooCommand command)
        {
            return command.Dummy != "Gocha";
        }

        protected override bool PreValidate(ValidationContext<UpdateFooCommand> context, ValidationResult result)
        {
            context.InstanceToValidate.Id = this.RuntimeId;
            return base.PreValidate(context, result);
        }

        public IDictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        private FooId RuntimeId => this.Parameters.GetOrDefault("id") as FooId;
    }
}
