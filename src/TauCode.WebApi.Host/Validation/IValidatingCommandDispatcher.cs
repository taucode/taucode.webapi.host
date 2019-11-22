using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Host.Validation
{
    public interface IValidatingCommandDispatcher : ICommandDispatcher
    {
        void Validate<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
