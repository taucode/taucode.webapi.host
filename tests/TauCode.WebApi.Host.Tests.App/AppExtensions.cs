using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;
using Inflector;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Reflection;
using TauCode.Domain.NHibernate.Conventions;

namespace TauCode.WebApi.Host.Tests.App
{
    public static class AppExtensions
    {
        public static ISessionFactory BuildSessionFactory(Configuration configuration, Assembly mappingsAssembly)
        {
            return Fluently.Configure(configuration)
                .Mappings(m => m.FluentMappings.AddFromAssembly(mappingsAssembly)
                    .Conventions.Add(ForeignKey.Format((p, t) =>
                    {
                        if (p == null) return t.Name.Underscore() + "_id";

                        return p.Name.Underscore() + "_id";
                    }))
                    .Conventions.Add(LazyLoad.Never())
                    .Conventions.Add(Table.Is(x => x.TableName.Underscore().ToUpper()))
                    .Conventions.Add(ConventionBuilder.Property.Always(x => x.Column(x.Property.Name.Underscore())))
                    .Conventions.Add(new IdUserTypeConvention())
                )
                .BuildSessionFactory();
        }

        public static void AddNHibernate(this IAppStartup startup, Configuration configuration, Assembly mappingsAssembly)
        {
            startup.ContainerBuilder.Register(c => BuildSessionFactory(configuration, mappingsAssembly))
                .As<ISessionFactory>()
                .SingleInstance();

            startup.ContainerBuilder.Register(c => c.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>()
                .InstancePerLifetimeScope();
        }

        private static bool IsRepository(Type type)
        {
            return type.IsClass && type.Name.EndsWith("Repository");
        }

        public static void AddPersistence(this IAppStartup startup, Assembly persistenceAssembly)
        {
            startup.ContainerBuilder.RegisterAssemblyTypes(persistenceAssembly)
                .Where(IsRepository)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
