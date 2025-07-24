using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;


namespace WebApplication1.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //config.EnableCors();   
        }
    }
}