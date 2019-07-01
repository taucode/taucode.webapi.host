﻿using FluentValidation;

namespace TauCode.WebApi.Host.Test.FooManagement.Core.Features.Foos.CreateFoo
{
    public class CreateFooCommandValidator : AbstractValidator<CreateFooCommand>
    {
        public CreateFooCommandValidator()
        {
            this.RuleFor(x => x.Code).Length(3);
        }
    }
}
