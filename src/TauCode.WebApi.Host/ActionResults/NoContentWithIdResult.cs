using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TauCode.WebApi.Host.ActionResults
{
    public class NoContentWithIdResult : StatusCodeResult
    {
        private readonly string _id;
        private readonly string _route;

        public NoContentWithIdResult(string id = null, string route = null)
            : base(StatusCodes.Status204NoContent)
        {
            _id = id;
            _route = route;
        }

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);

            if (_id != null)
            {
                context.HttpContext.Response.Headers.Add(DtoHelper.InstanceIdHeaderName, _id);
            }

            if (_route != null)
            {
                context.HttpContext.Response.Headers.Add(DtoHelper.RouteHeaderName, _route);
            }
        }
    }
}
