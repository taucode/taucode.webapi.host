//using System;
//using System.Collections.Generic;
//using System.Net;

//namespace TauCode.WebApi.Host
//{
//    public static class WebApiHostExtensions
//    {
//        #region Business Logic Error

//        public static IHttpActionResult BusinessLogicError(this ApiController controller, string code, string message)
//        {
//            return new BusinessLogicErrorResult(controller.Request, code, message);
//        }

//        public static IHttpActionResult BusinessLogicError(this ApiController controller, string message)
//        {
//            return new BusinessLogicErrorResult(controller.Request, message);
//        }

//        public static IHttpActionResult BusinessLogicError(this ApiController controller, string code, Exception exception)
//        {
//            return BusinessLogicError(controller, code, exception.Message);
//        }

//        public static IHttpActionResult BusinessLogicError(this ApiController controller, Exception exception)
//        {
//            return BusinessLogicError(controller, exception.Message);
//        }

//        #endregion

//        #region Forbidden Error

//        public static IHttpActionResult ForbiddenError(this ApiController controller, string code, string message)
//        {
//            return new ForbiddenErrorResult(controller.Request, code, message);
//        }

//        public static IHttpActionResult ForbiddenError(this ApiController controller, string message)
//        {
//            return new ForbiddenErrorResult(controller.Request, message);
//        }

//        public static IHttpActionResult ForbiddenError(this ApiController controller, string code, Exception exception)
//        {
//            return ForbiddenError(controller, code, exception.Message);
//        }

//        public static IHttpActionResult ForbiddenError(this ApiController controller, Exception exception)
//        {
//            return ForbiddenError(controller, exception.Message);
//        }

//        #endregion

//        #region No Content

//        public static IHttpActionResult NoContent(this ApiController controller)
//        {
//            var response = new StatusCodeResult(HttpStatusCode.NoContent, controller);
//            return response;
//        }

//        #endregion

//        #region No Content With Id

//        public static IHttpActionResult NoContentWithId(this ApiController controller, string id = null, string route = null)
//        {
//            return new NoContentWithIdResult(controller.Request, id, route);
//        }

//        #endregion

//        #region Created With Content

//        /// <summary>
//        /// Creates a 201 Created result with Location header and content from the given route.
//        /// </summary>
//        /// <param name="controller">The current API controller</param>
//        /// <param name="routeName">The name of the route where the created resource can be found</param>
//        /// <param name="routeValues">The route data used to generate the route url</param>
//        /// <returns>An action result containing the Location header linking to the given route, and the content from the given route</returns>
//        /// <typeparam name="T">The type of the returned content</typeparam>
//        public static IHttpActionResult CreatedAtRouteWithContent<T>(this ApiController controller, string routeName, object routeValues)
//        {
//            return controller.CreatedAtRouteWithContent<T>(routeName, new HttpRouteValueDictionary(routeValues));
//        }

//        /// <summary>
//        /// Creates a 201 Created result with Location header and content from the given route.
//        /// </summary>
//        /// <param name="controller">The current API controller</param>
//        /// <param name="routeName">The name of the route where the created resource can be found</param>
//        /// <param name="routeValues">The route data used to generate the route url</param>
//        /// <returns>An action result containing the Location header linking to the given route, and the content from the given route</returns>
//        /// <exception cref="NotSupportedException">The result type of the given route is not supported</exception>
//        /// <typeparam name="T">The type of the returned content</typeparam>
//        public static IHttpActionResult CreatedAtRouteWithContent<T>(this ApiController controller, string routeName, IDictionary<string, object> routeValues)
//        {
//            var content = GetResourceData<T>(controller, routeName, routeValues);

//            return new CreatedAtRouteNegotiatedContentResult<T>(routeName, routeValues, content, controller);
//        }

//        public static IHttpActionResult CreatedWithIdAndContent<T>(
//            this ApiController controller,
//            string routeName,
//            object routeValues,
//            string idPropertyName = null)
//        {
//            return controller.CreatedWithIdAndContent<T>(
//                routeName,
//                new HttpRouteValueDictionary(routeValues),
//                idPropertyName);
//        }

//        public static IHttpActionResult CreatedWithIdAndContent<T>(
//            this ApiController controller,
//            string routeName,
//            IDictionary<string, object> routeValues,
//            string idPropertyName = null)
//        {
//            return new CreatedWithIdAndContentResult<T>(controller, routeName, routeValues, idPropertyName);
//        }

//        #endregion

//        #region OK With Content

//        /// <summary>
//        /// Creates a 200 Ok result with the content from the given route.
//        /// </summary>
//        /// <param name="controller">The current API controller</param>
//        /// <param name="routeName">The name of the route where the content should be gotten from</param>
//        /// <param name="routeValues">The route data used to generate the url for the route</param>
//        /// <returns>The result of the action behind the given route</returns>
//        /// <typeparam name="T">The type of the returned content</typeparam>
//        public static IHttpActionResult OkWithContentFromRoute<T>(this ApiController controller, string routeName, object routeValues)
//        {
//            return controller.OkWithContentFromRoute<T>(routeName, new HttpRouteValueDictionary(routeValues));
//        }

//        /// <summary>
//        /// Creates a 200 Ok result with the content from the given route.
//        /// </summary>
//        /// <param name="controller">The current API controller</param>
//        /// <param name="routeName">The name of the route where the content should be gotten from</param>
//        /// <param name="routeValues">The route data used to generate the url for the route</param>
//        /// <returns>The result of the action behind the given route</returns>
//        /// <typeparam name="T">The type of the returned content</typeparam>
//        public static IHttpActionResult OkWithContentFromRoute<T>(this ApiController controller, string routeName, IDictionary<string, object> routeValues)
//        {
//            var content = GetResourceData<T>(controller, routeName, routeValues);

//            return new OkNegotiatedContentResult<T>(content, controller);
//        }

//        public static IHttpActionResult OkWithIdAndContent<T>(
//            this ApiController controller,
//            string routeName,
//            object routeValues,
//            string idPropertyName = null)
//        {
//            return controller.OkWithIdAndContent<T>(
//                routeName,
//                new HttpRouteValueDictionary(routeValues),
//                idPropertyName);
//        }

//        public static IHttpActionResult OkWithIdAndContent<T>(
//            this ApiController controller,
//            string routeName,
//            IDictionary<string, object> routeValues,
//            string idPropertyName = null)
//        {
//            return new OkWithIdAndContentResult<T>(controller, routeName, routeValues, idPropertyName);
//        }

//        #endregion

//        #region Validation Error

//        public static ValidationErrorResult ValidationError(
//            this ApiController controller,
//            string code = null,
//            string message = null,
//            IDictionary<string, ValidationFailureDto> failures = null)
//        {
//            return new ValidationErrorResult(controller.Request, code, message, failures);
//        }

//        public static ValidationErrorResult ValidationError(
//            this ApiController controller,
//            ValidationErrorResponseDto validationError)
//        {
//            return new ValidationErrorResult(controller.Request, validationError);
//        }

//        public static ValidationErrorResult ValidationError(
//            this ApiController controller,
//            ValidationException ex)
//        {
//            if (ex == null)
//            {
//                throw new ArgumentNullException(nameof(ex));
//            }

//            var failures = new Dictionary<string, ValidationFailureDto>();
//            foreach (var error in ex.Errors)
//            {
//                if (failures.ContainsKey(error.PropertyName))
//                {
//                    continue;
//                }

//                failures.Add(error.PropertyName, new ValidationFailureDto(error.ErrorCode, error.ErrorMessage));
//            }

//            return new ValidationErrorResult(controller.Request, message: ex.Message, failures: failures);
//        }

//        #endregion

//        #region Private

//        private static T GetResourceData<T>(ApiController controller, string routeName, IDictionary<string, object> routeValues)
//        {
//            // Invoke the the action given by the route name
//            var result = InternalRouteHelper.InvokeRoute(controller.Request, routeName, routeValues);

//            // Convert the result into an IHttpActionResult if possible, and return the result content.
//            var httpActionResult = result as IHttpActionResult;
//            if (httpActionResult != null)
//            {
//                if (httpActionResult.GetType().IsGenericType && httpActionResult.GetType().GetGenericTypeDefinition() == typeof(OkNegotiatedContentResult<>))
//                {
//                    var content = httpActionResult.GetType().GetProperty("Content").GetValue(httpActionResult);

//                    return (T)content;
//                }

//                if (httpActionResult.GetType() == typeof(NotFoundResult))
//                {
//                    throw new Exception("The given route unexpectedly returned a not found result");
//                }
//            }

//            // If the result type could not be converted into an IHttpActionResult, 
//            // throw an exception informing that the given route has an unsupported return type.
//            throw new NotSupportedException("Action result type is not supported");
//        }

//        #endregion
//    }
//}
