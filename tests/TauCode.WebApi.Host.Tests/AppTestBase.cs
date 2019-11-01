using Autofac;
using Microsoft.AspNetCore.TestHost;
using NHibernate;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using TauCode.Db.Utils.Building;
using TauCode.Db.Utils.Building.SQLite;
using TauCode.Db.Utils.Building.SqlServer;
using TauCode.Db.Utils.Crud;
using TauCode.Db.Utils.Crud.SQLite;
using TauCode.Db.Utils.Crud.SqlServer;
using TauCode.Db.Utils.Inspection;
using TauCode.Db.Utils.Inspection.SQLite;
using TauCode.Db.Utils.Inspection.SqlServer;
using TauCode.Db.Utils.Serialization;
using TauCode.WebApi.Host.Tests.App;
using TauCode.WebApi.Host.Tests.DbMigrations;

namespace TauCode.WebApi.Host.Tests
{
    [TestFixture]
    public abstract class AppTestBase
    {
        protected IDbConnection Connection { get; private set; }
        protected string ConnectionString { get; private set; }
        protected IDbInspector DbInspector { get; private set; }
        protected IScriptBuilder ScriptBuilder { get; private set; }
        protected ICruder Cruder { get; private set; }

        protected IDataSerializer DataSerializer { get; private set; }

        protected TestFactory Factory { get; private set; }

        protected HttpClient HttpClient { get; private set; }
        protected IContainer Container { get; private set; }

        protected ILifetimeScope SetupLifetimeScope { get; private set; }
        protected ILifetimeScope TestLifetimeScope { get; private set; }
        protected ILifetimeScope AssertLifetimeScope { get; private set; }

        protected ISession SetupSession { get; private set; }
        protected ISession TestSession { get; private set; }
        protected ISession AssertSession { get; private set; }

        protected TargetDbType GetTargetDbType() => TargetDbType.SQLite;

        protected virtual IDbInspector CreateDbInspector(IDbConnection connection)
        {
            var dbType = this.GetTargetDbType();
            switch (dbType)
            {
                case TargetDbType.SqlServer:
                    return new SqlServerInspector(connection);

                case TargetDbType.SQLite:
                    return new SQLiteInspector(connection);

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual IDbConnection CreateDbConnection(string connectionString) =>
            new SQLiteConnection(connectionString);

        protected virtual IScriptBuilder CreateScriptBuilder()
        {
            var dbType = this.GetTargetDbType();
            switch (dbType)
            {
                case TargetDbType.SqlServer:
                    return new SqlServerScriptBuilder();

                case TargetDbType.SQLite:
                    return new SQLiteScriptBuilder();

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual ICruder CreateCruder()
        {
            var dbType = this.GetTargetDbType();
            switch (dbType)
            {
                case TargetDbType.SqlServer:
                    return new SqlServerCruder();

                case TargetDbType.SQLite:
                    return new SQLiteCruder();

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual Assembly GetMigrationAssembly() => typeof(M0_Baseline).Assembly;

        protected virtual void PurgeDb()
        {
            this.DbInspector.ClearDb();
            var tables = this.DbInspector.GetOrderedTableMolds(false);

            using (var command = this.Connection.CreateCommand())
            {
                foreach (var table in tables)
                {
                    var sql = this.ScriptBuilder.BuildDropTableSql(table.Name);
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        protected virtual void Migrate()
        {
            var migrator = new Migrator(this.ConnectionString, this.GetTargetDbType(), this.GetMigrationAssembly());
            migrator.Migrate();
        }

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
            this.Container = startup.Container;
            this.ConnectionString = startup.ConnectionString;

            this.Connection = this.CreateDbConnection(this.ConnectionString);
            this.Connection.Open();
            this.DbInspector = this.CreateDbInspector(this.Connection);
            this.ScriptBuilder = this.CreateScriptBuilder();
            this.Cruder = this.CreateCruder();
            //this.DataSerializer = this.CreateDataSerializer();
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            if (this.Connection != null)
            {
                this.Connection.Dispose();
                this.Connection = null;
            }

            this.HttpClient.Dispose();
            this.Factory.Dispose();

            this.HttpClient = null;
            this.Factory = null;
        }

        [SetUp]
        public void SetUpBase()
        {
            this.PurgeDb();
            this.Migrate();

            // autofac stuff
            this.SetupLifetimeScope = this.Container.BeginLifetimeScope();
            this.TestLifetimeScope = this.Container.BeginLifetimeScope();
            this.AssertLifetimeScope = this.Container.BeginLifetimeScope();

            // nhibernate stuff
            this.SetupSession = this.SetupLifetimeScope.Resolve<ISession>();
            this.TestSession = this.TestLifetimeScope.Resolve<ISession>();
            this.AssertSession = this.AssertLifetimeScope.Resolve<ISession>();
        }

        [TearDown]
        public void TearDownBase()
        {
            this.SetupSession.Dispose();
            this.TestSession.Dispose();
            this.AssertSession.Dispose();

            this.SetupLifetimeScope.Dispose();
            this.TestLifetimeScope.Dispose();
            this.AssertLifetimeScope.Dispose();
        }
    }
}
