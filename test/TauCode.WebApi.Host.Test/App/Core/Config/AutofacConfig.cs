using Autofac;
using FluentValidation;
using Moq;
using NHibernate;
using System.Linq;
using TauCode.Cqrs.Autofac;
using TauCode.Cqrs.Commands;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Test.App.Domain.Foos;
using TauCode.WebApi.Host.Test.App.Persistence.Repositories;

namespace TauCode.WebApi.Host.Test.App.Core.Config
{
    public static class AutofacConfig
    {
        public static void Configure(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<AutofacCommandHandlerFactory>()
                .As<ICommandHandlerFactory>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<QueryRunner>()
                .As<IQueryRunner>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<AutofacQueryHandlerFactory>()
                .As<IQueryHandlerFactory>()
                .InstancePerLifetimeScope();

            // register query handlers
            containerBuilder.RegisterAssemblyTypes(typeof(AutofacConfig).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            // register command handler validators
            containerBuilder.RegisterAssemblyTypes(typeof(AutofacConfig).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();


            // register command handlers
            containerBuilder.RegisterAssemblyTypes(typeof(AutofacConfig).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            // register repo
            containerBuilder
                .RegisterType<MockFooRepository>()
                .As<IFooRepository>()
                .SingleInstance();

            containerBuilder
                .Register(c => CreateSessionFromRepo(c.Resolve<IFooRepository>()))
                .As<ISession>()
                .InstancePerLifetimeScope();
        }

        private static ISession CreateSessionFromRepo(IFooRepository fooRepository)
        {
            var mock = new Mock<ISession>();

            mock
                .Setup(x => x.Query<Foo>())
                .Returns(() => 
                    fooRepository
                    .GetAll()
                    .AsQueryable());

            return mock.Object;
        }
    }
}
