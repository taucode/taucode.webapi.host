using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace TauCode.WebApi.Host.Tests.App
{
    public class Startup : AppStartupBase
    {
        protected override Assembly GetValidatorsAssembly() => typeof(Startup).Assembly;

        protected override void ConfigureContainerBuilder()
        {
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}
