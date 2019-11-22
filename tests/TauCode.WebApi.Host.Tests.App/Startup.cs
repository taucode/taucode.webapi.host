using Microsoft.AspNetCore.Builder;
using NHibernate.Cfg;
using System.IO;
using System.Reflection;
using TauCode.Cqrs.NHibernate;
using TauCode.Utils.Extensions;
using TauCode.WebApi.Host.Tests.Core;
using TauCode.WebApi.Host.Tests.Persistence;

namespace TauCode.WebApi.Host.Tests.App
{
    public class Startup : AppStartupBase
    {
        public string ConnectionString { get; private set; }

        protected virtual Configuration CreateConfiguration()
        {
            var filePath = FileExtensions.CreateTempFilePath(extension:".sqlite");
            File.WriteAllBytes(filePath, new byte[] { });

            this.ConnectionString = $@"Data Source={filePath};Version=3;";

            var configuration = new Configuration();
            configuration.Properties.Add("connection.connection_string", this.ConnectionString);
            configuration.Properties.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            configuration.Properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.Properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");

            return configuration;
        }

        protected override Assembly GetValidatorsAssembly() => typeof(CoreBeacon).Assembly;

        protected override void ConfigureContainerBuilder()
        {
            this.AddCqrs(typeof(CoreBeacon).Assembly, typeof(TransactionalCommandHandlerDecorator<>));

            var configuration = this.CreateConfiguration();

            this.AddNHibernate(configuration, typeof(PersistenceBeacon).Assembly);
            this.AddPersistence(typeof(PersistenceBeacon).Assembly);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}
