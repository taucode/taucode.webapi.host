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
        protected virtual Configuration CreateConfiguration()
        {
            var filePath = FileExtensions.CreateTempFilePath(extension:".sqlite");
            File.WriteAllBytes(filePath, new byte[] { });

            var connectionString = $@"Data Source={filePath};Version=3;";

            var nhibernateConfiguration = new Configuration();
            nhibernateConfiguration.Properties.Add("connection.connection_string", connectionString);
            nhibernateConfiguration.Properties.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            nhibernateConfiguration.Properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            nhibernateConfiguration.Properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");

            return nhibernateConfiguration;
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
