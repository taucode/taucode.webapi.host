using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net.Http;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test
{
    [TestFixture]
    public abstract class MyTestBase
    {
        private MyFactory _factory;
        protected HttpClient Client { get; private set; }

        protected IFooRepository Repository { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            _factory = new MyFactory();
            this.Client = _factory
                .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot("test/TauCode.WebApi.Host.Test"))
                .CreateClient();
        }
    }
}
