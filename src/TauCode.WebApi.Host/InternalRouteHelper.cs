//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;

//namespace TauCode.WebApi.Host
//{
//    public static class InternalRouteHelper
//    {
//        //public static string FormatSiblingRoute(this ControllerBase controller, string routeName, object routeValues)
//        //{
//        //    throw new NotImplementedException();
//        //    //IActionSelector ddeas;

//        //    //var ddaaaoo = controller.Request;

//        //    //var routeValuesDictionary = new RouteValueDictionary(routeValues);
//        //    //routeValuesDictionary.Add(HttpRoute.HttpRouteKey, true);
//        //    //var vp = controller.Request.GetConfiguration().Routes.GetVirtualPath(new HttpRequestMessage(), routeName, routeValuesDictionary);

//        //    //return vp.VirtualPath.Substring(1); // skip leading '/'
//        //}

//        public static object InvokeRoute(HttpRequestMessage request, string routeName, IDictionary<string, object> routeValues)
//        {
//            throw new NotImplementedException();
//            //// Add a special route value, so that GetVirtualPath will not return null
//            //routeValues.Add(HttpRoute.HttpRouteKey, true);

//            //// Find the action related to the given route
//            //var vp = request.GetConfiguration().Routes.GetVirtualPath(new HttpRequestMessage(), routeName, routeValues);
//            //var action = (ReflectedHttpActionDescriptor)((HttpActionDescriptor[])vp.Route.DataTokens["actions"]).FirstOrDefault();

//            //// Resolve the controller and method information
//            //var actionController = request.GetDependencyScope().GetService(action.ControllerDescriptor.ControllerType);
//            //var actionMethod = action.MethodInfo;

//            //// Convert the action parameters into method parameters
//            //var actionParameters = action.GetParameters();
//            //var methodParameters = new object[actionParameters.Count];
//            //foreach (ReflectedHttpParameterDescriptor httpParameterDescriptor in actionParameters)
//            //{
//            //    var paramValue = Type.Missing;
//            //    routeValues.TryGetValue(httpParameterDescriptor.ParameterName, out paramValue);

//            //    methodParameters[httpParameterDescriptor.ParameterInfo.Position] = paramValue;
//            //}

//            //// Invoke the action and return the result;
//            //return actionMethod.Invoke(actionController, methodParameters);
//        }
//    }
//}
