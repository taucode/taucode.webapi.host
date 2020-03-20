using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace TauCode.WebApi.Host
{
    public interface IStartupHelper
    {
        void Init(IServiceCollection services);

        /// <summary>
        /// Don't build this container manually; use <see cref="Accomplish"/> instead.
        /// </summary>
        /// <returns>Current instance of <see cref="ContainerBuilder"/></returns>
        ContainerBuilder GetContainerBuilder();

        /// <summary>
        /// Finalizes helper by building its container.
        /// </summary>
        void Accomplish();

        IContainer GetContainer();
    }
}
