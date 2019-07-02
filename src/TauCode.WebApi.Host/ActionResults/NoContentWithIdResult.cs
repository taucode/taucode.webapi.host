using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TauCode.WebApi.Host.ActionResults
{
    public class NoContentWithIdResult : StatusCodeResult
    {
        private readonly HttpRequest _request;
        private readonly string _id;
        private readonly string _route;

        public NoContentWithIdResult(HttpRequest request, string id = null, string route = null)
            : base(StatusCodes.Status204NoContent)
        {
            _request = request;
            _id = id;
            _route = route;
        }

        public override void ExecuteResult(ActionContext context)
        {
            throw new NotImplementedException();
        }

        //public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    var response = _request.CreateResponse(HttpStatusCode.NoContent);
        //    if (_id != null)
        //    {
        //        response.Headers.Add(DtoHelper.InstanceIdHeaderName, _id);
        //    }

        //    if (_route != null)
        //    {
        //        response.Headers.Add(DtoHelper.RouteHeaderName, _route);
        //    }

        //    return Task.FromResult(response);
        //}
    }
}
