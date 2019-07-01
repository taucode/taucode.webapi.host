using System;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos
{
    [Serializable]
    public class ForbiddenFooException : Exception
    {
        public ForbiddenFooException()
        {
        }

        public ForbiddenFooException(string message)
            : base(message)
        {
        }
    }
}
