using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using PresentationContractLayer;
using BusinessLayer;

namespace TradeSuiteApp.Web.ActionFilters
{
    public class LogActionFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            string Message = filterContext.Exception.Message;
            string StackTrace = filterContext.Exception.StackTrace;
            string InnerException = filterContext.Exception.InnerException?.ToString() ?? "";

            string Controller = filterContext.RouteData.Values["controller"].ToString();
            string Action = filterContext.RouteData.Values["action"].ToString();
            string Area = filterContext.RouteData.DataTokens["area"]?.ToString() ?? "";
            string IDString = filterContext.RouteData.Values["id"]?.ToString() ?? "0";
            int ID = Convert.ToInt32(IDString);
            var Result = new ViewResult();
           
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                Result = new ViewResult
                {
                    ViewName = "ErrorPlain",
                    MasterName = "~/Views/Shared/_LayoutEmpty.cshtml"
                };
            }
            else if (HttpContext.Current.Session["EmployeeName"] == null)
            {
                Result = new ViewResult
                {
                    ViewName = "Error",
                    MasterName = "~/Views/Shared/_Layout.cshtml"
                };
            }
            else
            {
                Result = new ViewResult
                {
                    ViewName = "Error",
                };
            }
            Result.ViewBag.Message = Message;
            Result.ViewBag.StackTrace = StackTrace;
            Result.ViewBag.InnerException = InnerException;
            ILocationContract locationBL = new LocationBL();
            Result.ViewBag.VBLocations = new SelectList(locationBL.GetLocationListByUser(Convert.ToInt16(filterContext.HttpContext.Session["UserID"])), "ID", "Name");
            Result.ViewBag.VBCurrentLocationID = filterContext.HttpContext.Session["CurrentLocationID"];


            IGeneralContract generalBL = new GeneralBL();
            generalBL.LogError(Area, Controller, Action, ID, Message, StackTrace, InnerException);

            filterContext.Result = Result;
            filterContext.ExceptionHandled = true;

        }

        Stopwatch stopWatch;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            IGeneralContract generalBL = new GeneralBL();
            string Controller = filterContext.RouteData.Values["controller"].ToString();
            string Action = filterContext.RouteData.Values["action"].ToString();
            string Area = filterContext.RouteData.DataTokens["area"]?.ToString() ?? "";
            string IDString = filterContext.RouteData.Values["id"]?.ToString() ?? "0";

            stopWatch.Stop();
            generalBL.LogPerformance(Area, Controller, Action, IDString, Convert.ToInt32(stopWatch.ElapsedMilliseconds));
        }

        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);

            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}