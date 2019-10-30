using FluentValidation;
using System;
using TauCode.Cqrs.Queries;

namespace TauCode.WebApi.Host.Validation
{
    public interface IQueryValidatorSource
    {
        Type[] GetQueryTypes();
        IValidator<TQuery> GetValidator<TQuery>() where TQuery : IQuery;
    }
}
