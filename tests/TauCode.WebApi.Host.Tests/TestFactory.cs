using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TauCode.WebApi.Host.Tests
{
    public class TestFactory : WebApplicationFactory<TestStartup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost
                .CreateDefaultBuilder()
                .UseStartup<TestStartup>();
        }
    }
}
