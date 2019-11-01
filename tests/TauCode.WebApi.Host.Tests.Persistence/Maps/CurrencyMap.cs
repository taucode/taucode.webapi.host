using FluentNHibernate.Mapping;
using TauCode.WebApi.Host.Tests.Domain.Currencies;

namespace TauCode.WebApi.Host.Tests.Persistence.Maps
{
    public class CurrencyMap : ClassMap<Currency>
    {
        public CurrencyMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.Code);
            this.Map(x => x.Name);
            this.Map(x => x.IsAvailableToUsers);
        }
    }
}
