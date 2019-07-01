using System;

namespace TauCode.WebApi.Host.Test.App.Domain.Foos.Exceptions
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