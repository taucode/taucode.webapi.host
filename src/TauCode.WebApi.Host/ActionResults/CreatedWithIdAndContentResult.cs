//using System.Collections.Generic;

//namespace TauCode.WebApi.Host.ActionResults
//{
//    public class CreatedWithIdAndContentResult<T> : WithIdAndContentResultBase<T>
//    {
//        public CreatedWithIdAndContentResult(ApiController controller, string routeName, IDictionary<string, object> routeValues, string idPropertyName = null)
//            : base(controller, routeName, routeValues, idPropertyName)
//        {
//        }

//        protected override IHttpActionResult CreateInnerResponse(T content)
//        {
//            var innerResult = new CreatedAtRouteNegotiatedContentResult<T>(
//                this.RouteName,
//                this.RouteValues,
//                content,
//                this.Controller);

//            return innerResult;
//        }
//    }
//}
