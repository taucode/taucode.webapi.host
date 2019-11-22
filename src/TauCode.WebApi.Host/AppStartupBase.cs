using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace TauCode.WebApi.Host
{
    public abstract class AppStartupBase : IAppStartup
    {
        #region Fields

        private ContainerBuilder _containerBuilder;
        private IContainer _container;

        #endregion

        #region Polymorph

        protected abstract Assembly GetValidatorsAssembly();

        protected virtual void ConfigureServicesImpl(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new ValidationFilterAttribute(this.GetValidatorsAssembly()));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        protected abstract void ConfigureContainerBuilder();

        #endregion

        #region IStartup Members

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.ConfigureServicesImpl(services);

            _containerBuilder = new ContainerBuilder();
            _containerBuilder.Populate(services);

            this.ConfigureContainerBuilder();

            // add self as a service.
            _containerBuilder
                .RegisterInstance(this)
                .As<IAppStartup>()
                .SingleInstance();

            this.Container = _containerBuilder.Build();
            _containerBuilder = null; // no more registrations

            return new AutofacServiceProvider(this.Container);
        }

        public abstract void Configure(IApplicationBuilder app);

        #endregion

        #region IAppStartup Members

        public ContainerBuilder ContainerBuilder
        {
            get
            {
                if (_containerBuilder == null)
                {
                    throw new InvalidOperationException($"'{nameof(ContainerBuilder)}' cannot be used at this moment.");
                }

                return _containerBuilder;
            }
            private set => _containerBuilder = value;
        }

        public IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    throw new InvalidOperationException($"'{nameof(Container)}' cannot be used at this moment.");
                }

                return _container;
            }
            private set => _container = value;
        }

        #endregion
    }
}
