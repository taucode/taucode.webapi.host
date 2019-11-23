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

            _container = _containerBuilder.Build();
            _containerBuilder = null; // no more registrations

            return new AutofacServiceProvider(_container);
        }

        public abstract void Configure(IApplicationBuilder app);

        #endregion

        #region IAppStartup Members

        public ContainerBuilder GetContainerBuilder()
        {
            if (_containerBuilder == null)
            {
                throw new InvalidOperationException("ContainerBuilder cannot be used at this moment.");
            }

            return _containerBuilder;
        }

        public IContainer GetContainer()
        {
            if (_container == null)
            {
                throw new InvalidOperationException("'Container cannot be used at this moment.");
            }

            return _container;
        }

        #endregion
    }
}
