using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host.ActionResults
{
    public class BusinessLogicErrorResult : StatusCodeResult
    {
        //private readonly HttpRequestMessage _request;
        //private readonly string _code;
        //private readonly string _message;

        //public BusinessLogicErrorResult(HttpRequestMessage request, string code, string message)
        //{
        //    _request = request ?? throw new ArgumentNullException(nameof(request));
        //    _code = code;
        //    _message = message;
        //}

        //public BusinessLogicErrorResult(HttpRequestMessage request, string message)
        //    : this(request, DtoHelper.BusinessLogicErrorCode, message)
        //{
        //}

        //public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    var errorResponse = new ErrorResponseDto(_code, _message);

        //    var response = _request.CreateResponse(HttpStatusCode.Conflict, errorResponse);
        //    response.Headers.Add(DtoHelper.SubReasonHeaderName, DtoHelper.BusinessLogicErrorSubReason);
        //    return Task.FromResult(response);
        //}

        public BusinessLogicErrorResult()
            : base(StatusCodes.Status409Conflict)
        {
        }

        public override void ExecuteResult(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
