using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TauCode.WebApi.Host
{
    public abstract class StartupHelperBase : IStartupHelper
    {
        private ContainerBuilder _containerBuilder;
        private IContainer _container;

        protected StartupHelperBase()
        {   
        }

        public void Init(IServiceCollection services)
        {
            if (_containerBuilder != null)
            {
                throw new NotImplementedException();
            }

            _containerBuilder = new ContainerBuilder();
            _containerBuilder.Populate(services);

            _containerBuilder
                .RegisterInstance(this)
                .As<IStartupHelper>()
                .SingleInstance();
        }

        public ContainerBuilder GetContainerBuilder()
        {
            if (_container != null)
            {
                throw new NotImplementedException();
            }

            return _containerBuilder;
        }

        public void Accomplish()
        {
            if (_containerBuilder == null)
            {
                throw new NotImplementedException();
            }

            if (_container != null)
            {
                throw new NotImplementedException();
            }

            _container = _containerBuilder.Build();
        }

        public IContainer GetContainer()
        {
            if (_container == null)
            {
                throw new NotImplementedException();
            }

            return _container;
        }
    }
}
