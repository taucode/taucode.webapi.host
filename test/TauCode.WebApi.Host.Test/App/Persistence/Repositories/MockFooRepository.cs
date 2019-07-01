using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Persistence.Repositories
{
    public class MockFooRepository : IFooRepository
    {
        private readonly Dictionary<FooId, Foo> _foos;

        public MockFooRepository()
        {
            _foos = new Dictionary<FooId, Foo>();
        }

        public Foo GetById(FooId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Id must be set");
            }

            return _foos.GetOrDefault(id);
        }

        public IList<Foo> GetAll()
        {
            return _foos.Values.ToList();
        }

        public void Save(Foo foo)
        {
            if (foo == null)
            {
                throw new ArgumentNullException(nameof(foo), "Foo must be set");
            }

            _foos[foo.Id] = foo;
        }

        public void Delete(FooId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Id must be set");
            }

            _foos.Remove(id);
        }

        public void Clear()
        {
            _foos.Clear();
        }
    }
}
