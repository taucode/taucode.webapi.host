using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using TauCode.WebApi.Host.Results;

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
            return new ConflictErrorResult(ex);
        }

        public static IActionResult CreatedOk(this ControllerBase controller, string id, string url)
        {
            return new CreatedOkResult(id, url);
        }

        public static IActionResult CreatedOk<T>(this ControllerBase controller, string id, string url, T content)
        {
            return new CreatedOkResult<T>(id, url, content);
        }

        public static IActionResult UpdatedOk(this ControllerBase controller, string id, string url)
        {
            return new UpdatedOkResult(id, url);
        }

        public static IActionResult UpdatedOk<T>(this ControllerBase controller, string id, string url, T content)
        {
            return new UpdatedOkResult<T>(id, url, content);
        }

        public static IActionResult DeletedOk(this ControllerBase controller, string id)
        {
            return new DeletedOkResult(id);
        }

        public static IActionResult NotFoundError(this ControllerBase controller, Exception ex)
        {
            return new NotFoundErrorResult(ex);
        }

        public static IActionResult ValidationError(this ControllerBase controller, ValidationResult validationResult)
        {
            return new ValidationErrorResult(validationResult);
        }

        public static IActionResult ValidationError(this ControllerBase controller, ValidationException validationException)
        {
            return new ValidationErrorResult(validationException);
        }
    }
}
