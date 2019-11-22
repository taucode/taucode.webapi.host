using FluentValidation;
using System;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Host.Validation
{
    public interface ICommandValidatorSource
    {
        Type[] GetCommandTypes();
        IValidator<TCommand> GetValidator<TCommand>() where TCommand : ICommand;
    }
}
