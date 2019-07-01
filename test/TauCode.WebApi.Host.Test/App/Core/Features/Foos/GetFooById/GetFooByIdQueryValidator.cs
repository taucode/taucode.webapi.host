using System;
using TauCode.WebApi.Host.Test.App.Domain.Foos;

namespace TauCode.WebApi.Host.Test.App.Core.Features.Foos.GetFooById
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
