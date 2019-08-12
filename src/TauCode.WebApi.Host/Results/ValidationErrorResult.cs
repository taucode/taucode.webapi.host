﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TauCode.WebApi.Host.Results
{
    public class ValidationErrorResult : BadRequestObjectResult
    {
        public ValidationErrorResult(ValidationResult validationResult)
            : base(WebApiHostHelper.CreateValidationErrorDto(validationResult))
        {
            // todo: check if validationResult is not null
        }

        public ValidationErrorResult(ValidationException validationException)
            : base(WebApiHostHelper.CreateValidationErrorDto(validationException))
        {
            // todo: check if validationResult is not null
        }

        public ValidationErrorDto Error => (ValidationErrorDto)this.Value;

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ValidationErrorPayloadType);
            return base.ExecuteResultAsync(context);
        }
    }
}