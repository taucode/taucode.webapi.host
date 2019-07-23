using Autofac;

namespace TauCode.WebApi.Host
{
    public interface IAutofacStartup
    {
        IContainer ApplicationContainer { get; }
    }
}
