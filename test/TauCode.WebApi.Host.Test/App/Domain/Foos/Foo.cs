using System;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos.Exceptions;

namespace TauCode.WebApi.Host.Test.FooManagement.Domain.Foos
{
    public class Foo
    {
        private Foo()
        {
        }
        public Foo(string code, string name)
        {
            if (code == "wat")
            {
                throw new FooException("Foo cannot be wat!");
            }

            this.Id = new FooId();
            this.Code = code ?? throw new ArgumentNullException(nameof(code), "Code must be set");
            this.ChangeName(name);
        }
        public FooId Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public void ChangeName(string name)
        {
            if (name == "Wat")
            {
                throw new FooException("Foo cannot have name 'Wat'!");
            }

            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
