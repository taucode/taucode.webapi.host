using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NHibernate;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;

namespace TauCode.WebApi.Host.Tests.App
{
    [TestFixture]
    public abstract class AppTestBase
    {
        protected TestFactory Factory { get; private set; }

        protected HttpClient HttpClient { get; private set; }
        protected IContainer Container { get; private set; }

        protected ILifetimeScope SetupLifetimeScope { get; private set; }
        protected ILifetimeScope TestLifetimeScope { get; private set; }
        protected ILifetimeScope AssertLifetimeScope { get; private set; }

        protected ISession SetupSession { get; private set; }
        protected ISession TestSession { get; private set; }
        protected ISession AssertSession { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            //this.Factory = this.CreateFactory();
            this.Factory = new TestFactory();
            //this.HttpClient = this.CreateClient();
            this.HttpClient = this.Factory
                .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(@"tests\TauCode.Apps.Testsush"))
                .CreateClient();

            //throw new NotImplementedException();

            var testServer = this.Factory.Factories.Single().Server;

            //var startup = (TestStartup)testServer.Host.Services.GetService(typeof(IStartup));
            //this.Container = startup.Container;

            throw new NotImplementedException();

            //var autofacStartup = (IAutofacStartup)testServer.Host.Services.GetService(typeof(IAutofacStartup));
            //this.Container = autofacStartup.ApplicationContainer;
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            this.HttpClient.Dispose();
            this.Factory.Dispose();

            this.HttpClient = null;
            this.Factory = null;
        }
    }
}
