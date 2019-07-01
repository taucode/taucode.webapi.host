//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Results;
//using System.Web.Http.Routing;
//using TauCode.WebApi.Dto;

//namespace TauCode.WebApi.Host.ActionResults
//{
//    public abstract class WithIdAndContentResultBase<T> : IHttpActionResult
//    {
//        protected ApiController Controller { get; private set; }
//        protected string RouteName { get; private set; }
//        protected IDictionary<string, object> RouteValues { get; private set; }
//        protected string IdPropertyName { get; private set; }
//        protected UrlHelper UrlHelper { get; private set; }

//        protected WithIdAndContentResultBase(ApiController controller, string routeName, IDictionary<string, object> routeValues, string idPropertyName = null)
//        {
//            this.Controller = controller ?? throw new ArgumentNullException(nameof(controller));
//            this.RouteName = routeName ?? throw new ArgumentNullException(nameof(routeName));

//            this.RouteValues = routeValues ?? throw new ArgumentNullException(nameof(routeValues));
//            if (this.RouteValues.Count == 0)
//            {
//                throw new InvalidOperationException("'routeValues' object doesn't contain any entries");
//            }

//            this.IdPropertyName = idPropertyName;
//            this.UrlHelper = new UrlHelper(controller.Request);
//        }

//        protected abstract IHttpActionResult CreateInnerResponse(T content);

//        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
//        {
//            // instance id
//            object instanceId;
//            if (this.IdPropertyName == null)
//            {
//                if (this.RouteValues.Count != 1)
//                {
//                    throw new InvalidOperationException("'routeValues' object contains more than one entry; provide exact non-null 'idPropertyName'");
//                }
//                else
//                {
//                    instanceId = this.RouteValues.Single().Value;
//                }
//            }
//            else
//            {
//                this.RouteValues.TryGetValue(this.IdPropertyName, out instanceId);
//                if (instanceId == null)
//                {
//                    throw new InvalidOperationException("'routeValues' does not contain non-null instance id");
//                }
//            }

//            // content
//            var actionResult = InternalRouteHelper.InvokeRoute(this.Controller.Request, this.RouteName, this.RouteValues);
//            T content;

//            if (actionResult is OkNegotiatedContentResult<T> okResult)
//            {
//                content = okResult.Content;
//            }
//            else if (actionResult is NotFoundResult)
//            {
//                throw new InvalidOperationException($"Route '{this.RouteName}' returned NotFound result");
//            }
//            else
//            {
//                throw new InvalidOperationException($"Route '{this.RouteName}' returned unexpected result (neither OK nor NotFound)");
//            }

//            var innerResult = this.CreateInnerResponse(content);

//            var innerResponse = await innerResult.ExecuteAsync(cancellationToken);

//            // header: route
//            var route = this.UrlHelper.Route(this.RouteName, this.RouteValues);
//            if (route.StartsWith("/"))
//            {
//                route = route.Substring(1);
//            }
//            innerResponse.Headers.Add(DtoHelper.RouteHeaderName, route);

//            // header: instance id
//            innerResponse.Headers.Add(DtoHelper.InstanceIdHeaderName, instanceId.ToString());

//            return innerResponse;
//        }
//    }
//}
