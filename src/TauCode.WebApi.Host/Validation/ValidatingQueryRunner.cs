using FluentValidation;
using System;
using TauCode.Cqrs.Queries;

namespace TauCode.WebApi.Host.Validation
{
    public class ValidatingQueryRunner : QueryRunner, IValidatingQueryRunner
    {
        protected IQueryValidatorSource QueryValidatorSource { get; }

        public ValidatingQueryRunner(
            IQueryHandlerFactory queryHandlerFactory,
            IQueryValidatorSource queryValidatorSource)
            : base(queryHandlerFactory)
        {
            this.QueryValidatorSource =
                queryValidatorSource
                ??
                throw new ArgumentNullException(nameof(queryValidatorSource));
        }

        public void Validate<TQuery>(TQuery query) where TQuery : IQuery
        {
            var validator = this.QueryValidatorSource.GetValidator<TQuery>();
            if (validator != null)
            {
                var validationResult = validator.Validate(query);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException("The query is invalid.", validationResult.Errors);
                }
            }
        }

        protected override void OnBeforeExecuteHandler<TQuery>(IQueryHandler<TQuery> handler, TQuery query)
        {
            this.Validate(query);
        }
    }
}