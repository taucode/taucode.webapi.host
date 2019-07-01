//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;

//namespace TauCode.WebApi.Host.ActionResults
//{
//    public class NoContentWithIdResult : IHttpActionResult
//    {
//        private readonly HttpRequestMessage _request;
//        private readonly string _id;
//        private readonly string _route;

//        public NoContentWithIdResult(HttpRequestMessage request, string id = null, string route = null)
//        {
//            _request = request;
//            _id = id;
//            _route = route;
//        }

//        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
//        {
//            var response = _request.CreateResponse(HttpStatusCode.NoContent);
//            if (_id != null)
//            {
//                response.Headers.Add(DtoHelper.InstanceIdHeaderName, _id);
//            }

//            if (_route != null)
//            {
//                response.Headers.Add(DtoHelper.RouteHeaderName, _route);
//            }

//            return Task.FromResult(response);
//        }
//    }
//}
