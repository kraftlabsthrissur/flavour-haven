using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using System;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class ItemController : BaseReportController
    {
        private ReportsEntities dbEntity;
        private IReportContract reportBL;

        public ItemController()
        {
            ViewBag.FinStartDate = GeneralBO.FinStartDate;
            ViewBag.CurrentDate = General.FormatDate(DateTime.Now);
            
            dbEntity = new ReportsEntities();
            reportBL = new ReportBL();
        
        }

        [HttpPost]
        public ActionResult Print(int ID)
        {
            try
            {
                FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
                ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
                ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.Item);


                Warning[] warnings;
                string[] streamIds;
                string contentType;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;


                var Item = dbEntity.SpRptItem(
                        ID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).ToList();


                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Item/ItemPrint.rdlc";
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemDataSet", Item));
                byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
                //Open generated PDF.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.ContentType = contentType;
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
                ViewBag.ReportViewer = reportViewer;
                return View();
            }
            catch (Exception e)
            {
                return View();

            }
        }

    }
}


