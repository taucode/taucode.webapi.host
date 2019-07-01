using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TauCode.WebApi.Host.Test.TestApp;

namespace TauCode.WebApi.Host.Test
{
    public class MyFactory : WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = WebHost
                .CreateDefaultBuilder()
                .UseStartup<Startup>();

            return builder;
        }
    }
}
