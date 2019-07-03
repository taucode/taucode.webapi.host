using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TauCode.WebApi.Host.ActionResults
{
    public class ValidationErrorResult : ContentResult
    {
        private readonly ValidationErrorResponseDto _validationError;

        public ValidationErrorResult(ValidationErrorResponseDto validationError)
            : this()
        {
            _validationError = validationError ?? throw new ArgumentNullException(nameof(validationError));
        }

        public ValidationErrorResult(
            string code = null,
            string message = null,
            IDictionary<string, ValidationFailureDto> failures = null)
            : this()
        {
            _validationError = ValidationErrorResponseDto.Standard;
            if (code != null)
            {
                _validationError.Code = code;
            }

            if (message != null)
            {
                _validationError.Message = message;
            }

            if (failures != null)
            {
                _validationError.Failures = failures;
            }
        }

        public ValidationErrorResult()
        {
            this.StatusCode = StatusCodes.Status400BadRequest;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            this.Content = JsonConvert.SerializeObject(_validationError);
            await base.ExecuteResultAsync(context);
        }
    }
}
