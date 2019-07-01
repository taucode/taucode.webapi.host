//using System;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using TauCode.WebApi.Dto;

//namespace TauCode.WebApi.Host.ActionResults
//{
//    public class ForbiddenErrorResult : IHttpActionResult
//    {
//        private readonly HttpRequestMessage _request;
//        private readonly string _code;
//        private readonly string _message;

//        public ForbiddenErrorResult(HttpRequestMessage request, string code, string message)
//        {
//            _request = request ?? throw new ArgumentNullException(nameof(request));
//            _code = code;
//            _message = message;
//        }

//        public ForbiddenErrorResult(HttpRequestMessage request, string message)
//            : this(request, DtoHelper.ForbiddenErrorCode, message)
//        {
//        }

//        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
//        {
//            var errorResponse = new ErrorResponseDto(_code, _message);

//            var response = _request.CreateResponse(HttpStatusCode.Forbidden, errorResponse);
//            response.Headers.Add(DtoHelper.SubReasonHeaderName, DtoHelper.ForbiddenErrorSubReason);
//            return Task.FromResult(response);
//        }
//    }
//}
