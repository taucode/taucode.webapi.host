using NHibernate;
using System;
using System.Linq;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById
{
    public class GetFooByIdQueryHandler : IQueryHandler<GetFooByIdQuery>
    {
        private readonly ISession _session;

        public GetFooByIdQueryHandler(ISession session)
        {
            _session = session;
        }
        public void Execute(GetFooByIdQuery query)
        {
            var foo = _session
                .Query<Foo>()
                .SingleOrDefault(x => x.Id == query.Id);

            if (foo == null)
            {
                throw new Exception();
            }

            var queryResult = new GetFooByIdQueryResult
            {
                Id = foo.Id,
                Code = foo.Code,
                Name = foo.Name,
            };
            query.SetResult(queryResult);
        }
    }
}
