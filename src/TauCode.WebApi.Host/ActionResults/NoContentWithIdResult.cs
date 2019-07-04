//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace TauCode.WebApi.Host.ActionResults
//{
//    public class NoContentWithIdResult : StatusCodeResult
//    {
//        private readonly string _id;
//        private readonly string _instanceLocation;

//        public NoContentWithIdResult(string id = null, string instanceLocation = null)
//            : base(StatusCodes.Status204NoContent)
//        {
//            _id = id;
//            _instanceLocation = instanceLocation;
//        }

//        public override void ExecuteResult(ActionContext context)
//        {
//            base.ExecuteResult(context);

//            if (_id != null)
//            {
//                context.HttpContext.Response.Headers.Add(DtoHelper.InstanceIdHeaderName, _id);
//            }

//            if (_instanceLocation != null)
//            {
//                context.HttpContext.Response.Headers.Add("X-Instance-Location", _instanceLocation);
//            }
//        }
//    }
//}
