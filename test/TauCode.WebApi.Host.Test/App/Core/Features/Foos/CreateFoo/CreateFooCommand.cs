using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.CreateFoo
{
    public class CreateFooCommand : Command<FooId>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
