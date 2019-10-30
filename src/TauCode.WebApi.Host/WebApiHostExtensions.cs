﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host
{
    public static class WebApiHostExtensions
    {
        internal static ErrorDto ToErrorDto(this Exception exception, string code = null)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var errorDto = new ErrorDto(code ?? exception.GetType().FullName, exception.Message);
            return errorDto;
        }

        public static IActionResult ConflictError(this ControllerBase controller, Exception ex)
        {
            controller.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);
            var error = ex.ToErrorDto();
            return controller.Conflict(error);
        }

        public static IActionResult DeletedNoContent(this ControllerBase controller, string id)
        {
            controller.Response.Headers.Add(DtoHelper.DeletedInstanceIdHeaderName, id);
            return controller.NoContent();
        }

        public static IActionResult NotFoundError(this ControllerBase controller, Exception ex)
        {
            controller.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ErrorPayloadType);
            var error = ex.ToErrorDto();
            return controller.NotFound(error);
        }

        public static IActionResult ValidationError(this ControllerBase controller, ValidationResult validationResult)
        {
            controller.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ValidationErrorPayloadType);
            var error = WebApiHostHelper.CreateValidationErrorDto(validationResult);
            return controller.BadRequest(error);
        }

        public static IActionResult ValidationError(this ControllerBase controller, ValidationException validationException)
        {
            controller.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ValidationErrorPayloadType);
            var error = WebApiHostHelper.CreateValidationErrorDto(validationException);
            return controller.BadRequest(error);
        }
    }
}
