using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.UpdateFoo
{
    public class UpdateFooCommandHandler : ICommandHandler<UpdateFooCommand>
    {
        private readonly IFooRepository _fooRepository;

        public UpdateFooCommandHandler(IFooRepository fooRepository)
        {
            _fooRepository = fooRepository;
        }

        public void Execute(UpdateFooCommand command)
        {
            if (command.Name == "Ira")
            {
                throw new ForbiddenFooException("stop!:)");
            }

            var foo = _fooRepository.GetById(command.Id);
            foo.ChangeName(command.Name);
            _fooRepository.Save(foo);
        }
    }
}
