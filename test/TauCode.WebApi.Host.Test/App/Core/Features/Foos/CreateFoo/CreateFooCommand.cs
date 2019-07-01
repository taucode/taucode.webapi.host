using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.CreateFoo
{
    public class CreateFooCommand : Command<FooId>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
