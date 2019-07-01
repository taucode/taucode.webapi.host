using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.DeleteFoo
{
    public class DeleteFooCommandHandler : ICommandHandler<DeleteFooCommand>
    {
        private readonly IFooRepository _fooRepository;
        public DeleteFooCommandHandler(IFooRepository fooRepository)
        {
            _fooRepository = fooRepository;
        }
        public void Execute(DeleteFooCommand command)
        {
            _fooRepository.Delete(command.Id);
        }
    }
}
