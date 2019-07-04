using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById
{
    public class GetFooByIdQuery : Query<GetFooByIdQueryResult>
    {
        public FooId Id { get; set; }
    }
}
