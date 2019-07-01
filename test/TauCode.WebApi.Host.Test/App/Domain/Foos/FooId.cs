using System;
using TauCode.Domain.Identities;

namespace TauCode.WebApi.Host.Test.App.Domain.Foos
{
    [Serializable]
    public class FooId : IdBase
    {
        public FooId()
        {
        }
        public FooId(Guid id)
            : base(id)
        {
        }
        public FooId(string id)
            : base(id)
        {
        }
    }
}
