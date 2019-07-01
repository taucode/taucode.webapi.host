using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.GetFooById
{
    public class GetFooByIdQuery : Query<GetFooByIdQueryResult>
    {
        public FooId Id { get; set; }
    }
}
