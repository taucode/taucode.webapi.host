using System;

namespace TauCode.WebApi.Host.Test.FooManagement.Domain.Foos.Exceptions
{
    [Serializable]
    public class FooException : Exception
    {
        public FooException()
        {
        }

        public FooException(string message)
            : base(message)
        {
        }
    }
}