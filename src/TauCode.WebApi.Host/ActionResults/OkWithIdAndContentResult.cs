//using System.Collections.Generic;
//using System.Web.Http;
//using System.Web.Http.Results;

//namespace TauCode.WebApi.Host.ActionResults
//{
//    public class OkWithIdAndContentResult<T> : WithIdAndContentResultBase<T>
//    {
//        public OkWithIdAndContentResult(ApiController controller, string routeName, IDictionary<string, object> routeValues, string idPropertyName = null) 
//            : base(controller, routeName, routeValues, idPropertyName)
//        {
//        }

//        protected override IHttpActionResult CreateInnerResponse(T content)
//        {
//            var innerResult = new OkNegotiatedContentResult<T>(content, this.Controller);

//            return innerResult;
//        }
//    }
//}
