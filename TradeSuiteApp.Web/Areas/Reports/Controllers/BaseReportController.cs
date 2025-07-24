using BusinessObject;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class BaseReportController : Controller
    {

        string AppName;
        string RDLCPath;
        protected ReportViewer reportViewer;

        protected ReportParameter CompanyNameParam;
        protected ReportParameter Address1Param;
        protected ReportParameter Address2Param;
        protected ReportParameter Address3Param;
        protected ReportParameter Address4Param;
        protected ReportParameter Address5Param;
        protected ReportParameter FromDateParam;
        protected ReportParameter ToDateParam;
        protected ReportParameter ReportNameParam;
        protected ReportParameter ImagePathParam;
        protected ReportParameter UserParam;
        protected ReportParameter MonthParam;
        protected ReportParameter YearParam;
        protected ReportParameter LocationParam;
        protected ReportParameter TreatmentGroupParam;
        protected ReportParameter FilterParam;
        protected ReportParameter DateAsOnParam;
        protected ReportParameter GSTNoParam;
        protected ReportParameter CINNoParam;
        protected ReportParameter PINParam;
        protected ReportParameter LandLine1Param;
        protected ReportParameter MobileNoParam;
        protected ReportParameter ReportLogoPathParam;
        protected ReportParameter ReportfooterPathParam;

        public BaseReportController()
        {
            reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.AsyncRendering = false;
            reportViewer.ZoomMode = ZoomMode.FullPage;
            reportViewer.BackColor = System.Drawing.Color.White;
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.KeepSessionAlive = true;

            string a = reportViewer.ResolveClientUrl("test");
            AppName = ConfigurationManager.AppSettings["AppName"];
            RDLCPath = "Areas\\Reports\\RDLC\\";

            CompanyNameParam = new ReportParameter("CompanyName", GeneralBO.CompanyName);
            Address1Param = new ReportParameter("Address1", GeneralBO.Address1);
            Address2Param = new ReportParameter("Address2", GeneralBO.Address2);
            Address3Param = new ReportParameter("Address3", GeneralBO.Address3);
            Address4Param = new ReportParameter("Address4", GeneralBO.Address4);
            Address5Param = new ReportParameter("Address5", GeneralBO.Address5);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            GSTNoParam = new ReportParameter("GSTNo", GeneralBO.GSTNo);
            CINNoParam = new ReportParameter("CINNo", GeneralBO.CINNo);
            PINParam = new ReportParameter("PIN", GeneralBO.PIN);
            LandLine1Param = new ReportParameter("LandLine1", GeneralBO.LandLine1);
            MobileNoParam = new ReportParameter("MobileNo", GeneralBO.MobileNo);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath ", GeneralBO.ReportfooterPath);
        }

        protected string GetReportPath(string RDLCName)
        {
            System.Web.Routing.RouteData RouteData = Request.RequestContext.RouteData;
            string Controller = RouteData.Values["controller"].ToString();

            string ReportPath = Request.MapPath(Request.ApplicationPath) + RDLCPath + "_" + AppName + "\\" + Controller + "\\" + RDLCName + ".rdlc";

            if (!System.IO.File.Exists(ReportPath))
            {
                ReportPath = Request.MapPath(Request.ApplicationPath) + RDLCPath + Controller + "\\" + RDLCName + ".rdlc";
            }

            return ReportPath;
        }

        protected void ExportReport(ReportViewer reportViewer, string Type)
        {
            Warning[] warnings;
            string reportName = "Report";
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            byte[] bytes = reportViewer.LocalReport.Render(Type, null, out contentType, out encoding, out extension, out streamIds, out warnings);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            if (reportViewer.LocalReport.DisplayName != "") {
                reportName = reportViewer.LocalReport.DisplayName;
            }
            Response.AddHeader("content-disposition", "attachment; filename=" + reportName + "." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
    }
}