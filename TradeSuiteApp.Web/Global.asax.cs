using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly List<object> _sessions = new List<object>();
        private static readonly object padlock = new object();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnityConfig.RegisterComponents();

            Database.SetInitializer<ApplicationDbContext>(null);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());
        }

        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 60;  //In minutes
            lock (padlock)
            {
                _sessions.Add(Session.SessionID);
            }
        }       

        public static List<object> Sessions
        {
            get
            {
                return _sessions;
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            lock (padlock)
            {
                _sessions.Remove(Session.SessionID);
            }
        }


    }

    public class CSharpRazorViewEngine : RazorViewEngine
    {
        public CSharpRazorViewEngine()
        {
            string AppName;

            AppName = ConfigurationManager.AppSettings["AppName"];

            AreaViewLocationFormats = new[]
             {
             "~/Areas/{2}/Views/_" + AppName + "/{1}/{0}.cshtml",
             "~/Areas/{2}/Views/{1}/{0}.cshtml",
             "~/Areas/{2}/Views/Shared/{0}.cshtml"
             };
            AreaMasterLocationFormats = new[]
             {
             "~/Areas/{2}/Views/_" + AppName + "/{1}/{0}.cshtml",
             "~/Areas/{2}/Views/{1}/{0}.cshtml",
             "~/Areas/{2}/Views/Shared/{0}.cshtml"
             };
            AreaPartialViewLocationFormats = new[]
             {
             "~/Areas/{2}/Views/_" + AppName + "/{1}/{0}.cshtml",
             "~/Areas/{2}/Views/{1}/{0}.cshtml",
             "~/Areas/{2}/Views/Shared/{0}.cshtml"
             };
            ViewLocationFormats = new[]
             {
             "~/Views/_" + AppName + "/{1}/{0}.cshtml",
             "~/Views/_" + AppName + "/Shared/{0}.cshtml",
             "~/Views/{1}/{0}.cshtml",
             "~/Views/Shared/{0}.cshtml"
             };
            MasterLocationFormats = new[]
             {
             "~/Views/_" + AppName + "/{1}/{0}.cshtml",
             "~/Views/_" + AppName + "/Shared/{0}.cshtml",
             "~/Views/{1}/{0}.cshtml",
             "~/Views/Shared/{0}.cshtml"
             };
            PartialViewLocationFormats = new[]
             {
             "~/Views/_" + AppName + "/{1}/{0}.cshtml",
             "~/Views/_" + AppName + "/Shared/{0}.cshtml",
             "~/Views/{1}/{0}.cshtml",
             "~/Views/Shared/{0}.cshtml"
             };
        }
    }
}
