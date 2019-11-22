using FluentValidation;
using System;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Host.Validation
{
    public class ValidatingCommandDispatcher : CommandDispatcher, IValidatingCommandDispatcher
    {
        protected ICommandValidatorSource CommandValidatorSource { get; }

        public ValidatingCommandDispatcher(
            ICommandHandlerFactory commandHandlerFactory,
            ICommandValidatorSource commandValidatorSource)
            : base(commandHandlerFactory)
        {
            this.CommandValidatorSource =
                commandValidatorSource
                ??
                throw new ArgumentNullException(nameof(commandValidatorSource));
        }

        public void Validate<TCommand>(TCommand command) where TCommand : ICommand
        {
            var validator = this.CommandValidatorSource.GetValidator<TCommand>();
            if (validator != null)
            {
                var validationResult = validator.Validate(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException("The command is invalid.", validationResult.Errors);
                }
            }
        }

        protected override void OnBeforeExecuteHandler<TCommand>(ICommandHandler<TCommand> handler, TCommand command)
        {
            this.Validate(command);
        }
    }
}