using TauCode.Cqrs.Queries;

namespace TauCode.WebApi.Host.Validation
{
    public interface IValidatingQueryRunner : IQueryRunner
    {
        void Validate<TQuery>(TQuery query) where TQuery : IQuery;
    }
}
