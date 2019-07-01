using NHibernate;
using System.Linq;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.GetAllFoos
{
    public class GetAllFoosQueryHandler : IQueryHandler<GetAllFoosQuery>
    {
        private readonly ISession _session;

        public GetAllFoosQueryHandler(ISession session)
        {
            _session = session;
        }

        public void Execute(GetAllFoosQuery query)
        {
            var foos = _session
                .Query<Foo>()
                .OrderBy(x => x.Code)
                .ToList();

            var queryResult = new GetAllFoosQueryResult
            {
                Items = foos
                    .Select(x => new GetAllFoosQueryResult.FooDto
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                    })
                    .ToList(),
            };

            query.SetResult(queryResult);
        }
    }
}
