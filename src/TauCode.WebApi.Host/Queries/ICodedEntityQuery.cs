using TauCode.Cqrs.Queries;
using TauCode.Domain.Identities;

namespace TauCode.WebApi.Host.Queries
{
    public interface ICodedEntityQuery : IQuery
    {
        IdBase GetId();
        string GetCode();
        string GetCodePropertyName();
    }
}
