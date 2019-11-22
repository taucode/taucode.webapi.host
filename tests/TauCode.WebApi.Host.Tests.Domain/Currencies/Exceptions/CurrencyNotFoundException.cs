using System;

namespace TauCode.WebApi.Host.Tests.Domain.Currencies.Exceptions
{
    [Serializable]
    public class CurrencyNotFoundException : Exception
    {
        public CurrencyNotFoundException()
            : this("Currency not found.")
        {
        }

        public CurrencyNotFoundException(string message)
            : base(message)
        {
        }
    }
}
