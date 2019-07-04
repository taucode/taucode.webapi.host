using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TauCode.WebApi.Host.Test.App.AppHost;

namespace TauCode.WebApi.Host.Test
{
    public class MyFactory : WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = WebHost
                .CreateDefaultBuilder()
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>();

            return builder;
        }
    }
}
