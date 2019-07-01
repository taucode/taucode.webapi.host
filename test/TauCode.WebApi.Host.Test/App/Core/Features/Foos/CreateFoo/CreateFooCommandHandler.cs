using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.CreateFoo
{
    public class CreateFooCommandHandler : ICommandHandler<CreateFooCommand>
    {
        private readonly IFooRepository _fooRepository;

        public CreateFooCommandHandler(IFooRepository fooRepository)
        {
            _fooRepository = fooRepository;
        }

        public void Execute(CreateFooCommand command)
        {
            if (command.Code == "ira")
            {
                throw new ForbiddenFooException("nope:)");
            }

            var foo = new Foo(command.Code, command.Name);
            _fooRepository.Save(foo);
            command.SetResult(foo.Id);
        }
    }
}
