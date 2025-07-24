using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TradeSuiteApp.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Login",
              url: "login",
              defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Language",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"en" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, lang = "en" }
            );

            
        }
    }
}
