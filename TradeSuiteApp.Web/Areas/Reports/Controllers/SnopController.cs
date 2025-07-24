using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using System.Linq;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class SnopController : BaseReportController
    {
        private SnopEntities dbEntity;

        public SnopController()
        {
            dbEntity = new SnopEntities();
        }

        // GET: Reports/Snop
        public ActionResult SalesForecast(int SalesForecastID)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var Items = dbEntity.SpGetSalesForecastItems(SalesForecastID,"","","","","","",0,20000, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Snop/SalesForecast.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SnopSalesDataSet", Items));
            byte[] bytes = reportViewer.LocalReport.Render("Excel", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AddHeader("content-disposition", "attachment; filename= SalesForecast" + "." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            ViewBag.ReportViewer = reportViewer;
            return View();
        }
    }
}