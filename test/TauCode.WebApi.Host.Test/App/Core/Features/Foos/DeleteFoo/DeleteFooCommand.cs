using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.DeleteFoo
{
    public class DeleteFooCommand : ICommand
    {
        public FooId Id { get; set; }
    }
}
