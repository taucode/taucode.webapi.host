//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;

//namespace TauCode.WebApi.Host.ActionResults
//{
//    public class ValidationErrorResult : IHttpActionResult
//    {
//        private readonly HttpRequestMessage _request;
//        private readonly ValidationErrorResponseDto _validationError;

//        public ValidationErrorResult(HttpRequestMessage request, ValidationErrorResponseDto validationError)
//        {
//            _request = request ?? throw new ArgumentNullException(nameof(request));
//            _validationError = validationError ?? throw new ArgumentNullException(nameof(validationError));
//        }

//        public ValidationErrorResult(
//            HttpRequestMessage request,
//            string code = null,
//            string message = null,
//            IDictionary<string, ValidationFailureDto> failures = null)
//        {
//            _request = request ?? throw new ArgumentNullException(nameof(request));
//            _validationError = ValidationErrorResponseDto.Standard;
//            if (code != null)
//            {
//                _validationError.Code = code;
//            }

//            if (message != null)
//            {
//                _validationError.Message = message;
//            }

//            if (failures != null)
//            {
//                _validationError.Failures = failures;
//            }
//        }

//        public ValidationErrorResponseDto ValidationError => _validationError;

//        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
//        {
//            var response = _request.CreateResponse(HttpStatusCode.BadRequest, _validationError);
//            response.Headers.Add(DtoHelper.SubReasonHeaderName, DtoHelper.ValidationErrorSubReason);
//            return Task.FromResult(response);
//        }
//    }
//}
