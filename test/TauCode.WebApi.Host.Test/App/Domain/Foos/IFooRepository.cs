using System.Collections.Generic;

namespace TauCode.WebApi.Host.Test.App.Domain.Foos
{
    public interface IFooRepository
    {
        Foo GetById(FooId id);
        IList<Foo> GetAll();
        void Save(Foo foo);
        void Delete(FooId id);
    }
}
