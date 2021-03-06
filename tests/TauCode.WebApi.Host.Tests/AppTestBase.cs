﻿using Autofac;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using TauCode.WebApi.Host.Tests.App;

namespace TauCode.WebApi.Host.Tests
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

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en-US");


            this.Factory = new TestFactory();

            this.HttpClient = this.Factory
                .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(@"tests\TauCode.WebApi.Host.Tests"))
                .CreateClient();

            var testServer = this.Factory.Factories.Single().Server;

            var startup = (Startup)testServer.Host.Services.GetService(typeof(IAppStartup));
            this.Container = startup.GetContainer();
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            this.HttpClient.Dispose();
            this.Factory.Dispose();

            this.HttpClient = null;
            this.Factory = null;
        }

        [SetUp]
        public void SetUpBase()
        {
            // autofac stuff
            this.SetupLifetimeScope = this.Container.BeginLifetimeScope();
            this.TestLifetimeScope = this.Container.BeginLifetimeScope();
            this.AssertLifetimeScope = this.Container.BeginLifetimeScope();
        }

        [TearDown]
        public void TearDownBase()
        {
            this.SetupLifetimeScope.Dispose();
            this.TestLifetimeScope.Dispose();
            this.AssertLifetimeScope.Dispose();
        }
    }
}
