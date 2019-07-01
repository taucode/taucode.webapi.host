using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.UpdateFoo
{
    public class UpdateFooCommand : ICommand
    {
        public FooId Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Must not be "gocha"
        /// </summary>
        public string Dummy { get; set; }
    }
}
