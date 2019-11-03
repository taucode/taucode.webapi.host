using Autofac;
using Autofac.Core;
using FluentValidation;
using System;
using System.Linq;
using System.Reflection;
using TauCode.Cqrs.Autofac;
using TauCode.Cqrs.Commands;
using TauCode.Cqrs.Queries;
using TauCode.WebApi.Host.Validation;

namespace TauCode.WebApi.Host
{
    public static class AppStartupExtensions
    {
        public static IAppStartup AddCqrs(this IAppStartup appStartup, Assembly cqrsAssembly, Type commandHandlerDecoratorType)
        {
            // command dispatching
            appStartup.ContainerBuilder
                .RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            appStartup.ContainerBuilder
                .RegisterType<ValidatingCommandDispatcher>()
                .As<IValidatingCommandDispatcher>()
                .InstancePerLifetimeScope();

            appStartup.ContainerBuilder
                .RegisterType<AutofacCommandHandlerFactory>()
                .As<ICommandHandlerFactory>()
                .InstancePerLifetimeScope();

            // register API ICommandHandler decorator
            appStartup.ContainerBuilder.RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
                .As(t => t.GetInterfaces()
                    .Where(x => x.IsClosedTypeOf(typeof(ICommandHandler<>)))
                    .Select(x => new KeyedService("commandHandler", x)))
                .InstancePerLifetimeScope();

            appStartup.ContainerBuilder.RegisterGenericDecorator(
                commandHandlerDecoratorType,
                typeof(ICommandHandler<>),
                "commandHandler");

            // command validator source
            appStartup.ContainerBuilder
                .RegisterInstance(new CommandValidatorSource(cqrsAssembly))
                .As<ICommandValidatorSource>()
                .SingleInstance();

            // validators
            appStartup.ContainerBuilder
                .RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(AbstractValidator<>)))
                .AsSelf()
                .InstancePerLifetimeScope();

            // query handling
            appStartup.ContainerBuilder
                .RegisterType<QueryRunner>()
                .As<IQueryRunner>()
                .InstancePerLifetimeScope();

            appStartup.ContainerBuilder
                .RegisterType<ValidatingQueryRunner>()
                .As<IValidatingQueryRunner>()
                .InstancePerLifetimeScope();

            appStartup.ContainerBuilder
                .RegisterType<AutofacQueryHandlerFactory>()
                .As<IQueryHandlerFactory>()
                .InstancePerLifetimeScope();

            appStartup.ContainerBuilder
                .RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            // query validator source
            appStartup.ContainerBuilder
                .RegisterInstance(new QueryValidatorSource(cqrsAssembly))
                .As<IQueryValidatorSource>()
                .SingleInstance();


            return appStartup;
        }
    }
}
