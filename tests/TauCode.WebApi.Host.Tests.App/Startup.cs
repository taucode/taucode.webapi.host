using Microsoft.AspNetCore.Builder;
using System.Reflection;
using TauCode.Cqrs.NHibernate;
using TauCode.WebApi.Host.Tests.Core;

namespace TauCode.WebApi.Host.Tests.App
{
    public class Startup : AppStartupBase
    {
        protected override Assembly GetValidatorsAssembly() => typeof(CoreBeacon).Assembly;

        protected override void ConfigureContainerBuilder()
        {
            this.AddCqrs(typeof(CoreBeacon).Assembly, typeof(TransactionalCommandHandlerDecorator<>)); // todo: what if null? add another AppSimple!
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}
