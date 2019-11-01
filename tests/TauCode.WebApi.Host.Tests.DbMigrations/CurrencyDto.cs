using System;

namespace TauCode.WebApi.Host.Tests.DbMigrations
{
    public class CurrencyDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool InitialIsAvailableToUsers { get; set; }
        public decimal InitialRate { get; set; }
    }
}
