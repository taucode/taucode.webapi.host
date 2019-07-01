using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById
{
    public class GetFooByIdQueryResult
    {
        public FooId Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
