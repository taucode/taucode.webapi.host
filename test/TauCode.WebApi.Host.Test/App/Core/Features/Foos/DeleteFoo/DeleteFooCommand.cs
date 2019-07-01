using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.DeleteFoo
{
    public class DeleteFooCommand : ICommand
    {
        public FooId Id { get; set; }
    }
}
