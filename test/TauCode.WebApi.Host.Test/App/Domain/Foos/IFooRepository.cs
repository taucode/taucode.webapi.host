using System.Collections.Generic;

namespace TauCode.WebApi.Host.Test.FooManagement.Domain.Foos
{
    public interface IFooRepository
    {
        Foo GetById(FooId id);
        IList<Foo> GetAll();
        void Save(Foo foo);
        void Delete(FooId id);
    }
}
