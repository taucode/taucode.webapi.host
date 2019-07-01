using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace TauCode.WebApi.Host.ActionResults
{
    // todo: get rid of comments
    public class ValidationErrorResult : StatusCodeResult
    {
        private readonly HttpRequest _request;
        private readonly ValidationErrorResponseDto _validationError;

        public ValidationErrorResult(HttpRequest request, ValidationErrorResponseDto validationError)
        : this()
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
            _validationError = validationError ?? throw new ArgumentNullException(nameof(validationError));
        }

        public ValidationErrorResult(
            HttpRequest request,
            string code = null,
            string message = null,
            IDictionary<string, ValidationFailureDto> failures = null)
            : this()
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
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

        //public ValidationErrorResponseDto ValidationError => _validationError;

        //public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    var response = _request.CreateResponse(HttpStatusCode.BadRequest, _validationError);
        //    response.Headers.Add(DtoHelper.SubReasonHeaderName, DtoHelper.ValidationErrorSubReason);
        //    return Task.FromResult(response);
        //}

        private ValidationErrorResult()
            : base(StatusCodes.Status400BadRequest)
        {
        }

        public override void ExecuteResult(ActionContext context)
        {
            throw new NotImplementedException();
            //base.ExecuteResult(context);
        }
    }
}
