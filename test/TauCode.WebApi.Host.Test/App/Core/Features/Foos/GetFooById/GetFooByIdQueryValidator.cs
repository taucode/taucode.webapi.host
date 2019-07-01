using FluentValidation;
using System;
using TauCode.WebApi.Host.Test.FooManagement.Domain.Foos;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.GetFooById
{
    public class GetFooByIdQueryValidator : AbstractValidator<GetFooByIdQuery>
    {
        public GetFooByIdQueryValidator()
        {
            this.RuleFor(x => x.Id)
                .NotEqual(new FooId(Guid.Empty));
        }
    }
}
