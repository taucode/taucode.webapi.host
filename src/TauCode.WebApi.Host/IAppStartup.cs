using Autofac;
using Microsoft.AspNetCore.Hosting;

namespace TauCode.WebApi.Host
{
    public interface IAppStartup : IStartup
    {
        ContainerBuilder GetContainerBuilder();

        IContainer GetContainer();
    }
}
