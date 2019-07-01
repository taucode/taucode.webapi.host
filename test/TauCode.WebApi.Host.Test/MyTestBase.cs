using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net.Http;

namespace TauCode.WebApi.Host.Test
{
    [TestFixture]
    public abstract class MyTestBase
    {
        private MyFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new MyFactory();
            _client = _factory
                .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot("test/TauCode.WebApi.Host.Test"))
                .CreateClient();
        }

        [Test]
        public void Wat()
        {
            var response = _client.GetAsync("api/foo").Result;
            var result = response.Content.ReadAsStringAsync().Result;

            var k = 3;
        }
    }
}
