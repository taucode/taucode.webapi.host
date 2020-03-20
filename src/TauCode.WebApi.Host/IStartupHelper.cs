using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace TauCode.WebApi.Host
{
    public interface IStartupHelper
    {
        void Init(IServiceCollection services);

        // todo: to be used only by extensions.
        ContainerBuilder GetContainerBuilder();

        void Accomplish();

        IContainer GetContainer();
    }
}
