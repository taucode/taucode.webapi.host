using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using TauCode.Cqrs.Autofac;
using TauCode.Cqrs.Commands;
using TauCode.Cqrs.Queries;

namespace TauCode.WebApi.Host.Tests.App
{
    public class Startup : IStartupHelperHost
    {
        private IStartupHelper _startupHelper;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var cqrsAssembly = typeof(Startup).Assembly;

            services.AddControllers(options => options.Filters.Add(new ValidationFilterAttribute(cqrsAssembly)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Demo Server RESTful Service",
                        Version = "v1"
                    });
                c.CustomSchemaIds(x => x.FullName);
                c.EnableAnnotations();
            });


            var startupHelper = this.GetStartupHelper();
            startupHelper.Init(services);

            var containerBuilder = _startupHelper.GetContainerBuilder();

            // command dispatching
            containerBuilder
                .RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<AutofacCommandHandlerFactory>()
                .As<ICommandHandlerFactory>()
                .InstancePerLifetimeScope();

            // register API ICommandHandler decorator
            containerBuilder
                .RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            // validators
            containerBuilder
                .RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(AbstractValidator<>)))
                .AsSelf()
                .InstancePerLifetimeScope();

            // query handling
            containerBuilder
                .RegisterType<QueryRunner>()
                .As<IQueryRunner>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<AutofacQueryHandlerFactory>()
                .As<IQueryHandlerFactory>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            // finalize
            _startupHelper.Accomplish();
            var container = _startupHelper.GetContainer();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Server RESTful Service"); });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public virtual IStartupHelper GetStartupHelper()
        {
            return _startupHelper ??= new StartupHelper();
        }
    }
}
