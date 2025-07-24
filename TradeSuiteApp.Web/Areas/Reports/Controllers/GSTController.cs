using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Reports.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class GSTController : BaseReportController
    {
        private ICategoryContract categoryBL;
        private ItemContract itemBL;
        private ReportsEntities dbEntity;
        private IGSTContract igstBL;
        private ILocationContract locationBL;
        private SelectList AtoZRange = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
        private SelectList number = new SelectList(Enumerable.Range(1, 9 - 1 + 1).Select(c => (int)c).ToList());
        private DateTime StartDate, EndDate;
        private DateTime? InvoiceDateFrom, InvoiceDateTo, GRNFromDate, GRNToDate, QCDateFrom, QCDateTo;
       
        public GSTController()
        {
            categoryBL = new CategoryBL();
            itemBL = new ItemBL();
            dbEntity = new ReportsEntities();           
            igstBL = new GSTBL();
            locationBL = new LocationBL();


            ViewBag.FinStartDate = GeneralBO.FinStartDate;
            ViewBag.CurrentDate = General.FormatDate(DateTime.Now);

        }

        // GET: Reports/GST
        public ActionResult Index()
        {
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/GST/GSTItemwiseSummary.rdlc";
            // reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GSTItemwise", dbEntity.SpGetPurchaseOrderDetails(0, GeneralBO.FinYear, GeneralBO.LocationID)));
            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        public JsonResult GetSupplierSubCategoryToRange(string from_range)
        {
            GSTViewModel rep = new GSTViewModel();
            if (from_range == "")
            {
                rep.ToSupplierTaxSubCategoryRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.ToSupplierTaxSubCategoryRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }

            return Json(new { Status = "success", data = rep.ToSupplierTaxSubCategoryRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSupplierToRange(string from_range)
        {
            GSTViewModel rep = new GSTViewModel();
            if (from_range == "")
            {
                rep.ToSupplierRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.ToSupplierRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }

            return Json(new { Status = "success", data = rep.ToSupplierRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategoryToRange(string from_range)
        {
            GSTViewModel rep = new GSTViewModel();
            if (from_range == "")
            {
                rep.ToItemCategoryRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.ToItemCategoryRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }

            return Json(new { Status = "success", data = rep.ToItemCategoryRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemToRange(string from_range)
        {
            GSTViewModel rep = new GSTViewModel();
            if (from_range == "")
            {
                rep.ToItemNameRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.ToItemNameRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }

            return Json(new { Status = "success", data = rep.ToItemNameRangeList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGstToRate(decimal FromGst)
        {
            List<GSTBO> GSTRate = igstBL.GetGstList().Where(x => x.IGSTPercentage >= FromGst).ToList();
            return Json(new { Status = "success", data = GSTRate }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        //GET: Reports/GST/GSTReports
        public ActionResult GST()
        {
            GSTViewModel rep = new GSTViewModel();
            rep.FromSupplierTaxSubCategoryRangeList = AtoZRange;
            rep.ToSupplierTaxSubCategoryRangeList = AtoZRange;
            rep.FromSupplierRangeList = AtoZRange;
            rep.ToSupplierRangeList = AtoZRange;
            //rep.FromItemCategoryRangeList = AtoZRange;
            //rep.ToItemCategoryRangeList = AtoZRange;
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.FromGSTRateRangeList = AtoZRange;
            rep.FromSupplierGSTNNoRangeList = number;
            rep.ToSupplierGSTNNoRangeList = number;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.ToGSTRateRangeList = new SelectList(
                                                 igstBL.GetGstList(), "IGSTPercentage", "IGSTPercentage");
            rep.FromGSTRateRangeList = new SelectList(
                                                  igstBL.GetGstList(), "IGSTPercentage", "IGSTPercentage");

            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            rep.ItemCategoryList = new SelectList(categoryBL.GetAllItemCategoryList(), "ID", "Name");
            rep.ItemLocationID = GeneralBO.LocationID;
            rep.InvoiceDateFrom = GeneralBO.FinStartDate;
            rep.InvoiceDateTo = General.FormatDate(DateTime.Now);
            rep.QCDateFrom = null;// GeneralBO.FinStartDate;
            rep.QCDateTo = null; //General.FormatDate(DateTime.Now);
            rep.GRNDateFrom = null; //GeneralBO.FinStartDate;
            rep.GRNDateTo = null;// General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        //POST:Reports/GST/GSTReports
        public ActionResult GST(GSTViewModel model)
        {
            model.FromSupplierTaxSubCategoryRangeList = AtoZRange;
            model.ToSupplierTaxSubCategoryRangeList = AtoZRange;
            model.FromSupplierRangeList = AtoZRange;
            model.ToSupplierRangeList = AtoZRange;
            //model.FromItemCategoryRangeList = AtoZRange;
            //model.ToItemCategoryRangeList = AtoZRange;
            model.FromItemNameRangeList = AtoZRange;
            model.ToItemNameRangeList = AtoZRange;
            model.FromGSTRateRangeList = AtoZRange;
            model.FromSupplierGSTNNoRangeList = number;
            model.ToSupplierGSTNNoRangeList = number;
            model.ToGSTRateRangeList = new SelectList(
                                               igstBL.GetGstList(), "IGSTPercentage", "IGSTPercentage");
            model.FromGSTRateRangeList = new SelectList(
                                                  igstBL.GetGstList(), "IGSTPercentage", "IGSTPercentage");
            model.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            if (model.InvoiceDateFrom != null)
                StartDate = General.ToDateTime(model.InvoiceDateFrom);
            if (model.InvoiceDateTo != null)
                EndDate = General.ToDateTime(model.InvoiceDateTo);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName","GST Report" + " " + model.Type + " " + model.IGST);
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            if (model.InvoiceDateFrom != null)
            {
                InvoiceDateFrom = General.ToDateTime(model.InvoiceDateFrom);
            }
            if (model.InvoiceDateTo != null)
            {
                InvoiceDateTo = General.ToDateTime(model.InvoiceDateTo);
            }
            if (model.GRNDateFrom != null)
            {
                GRNFromDate = General.ToDateTime(model.GRNDateFrom);
            }
            if (model.GRNDateTo != null)
            {
                GRNToDate = General.ToDateTime(model.GRNDateTo);
            }
            if (model.QCDateFrom != null)
            {
                QCDateFrom = General.ToDateTime(model.QCDateFrom);
            }
            if (model.QCDateTo != null)
            {
                QCDateTo = General.ToDateTime(model.QCDateTo);
            }
            if (model.ItemID != null)
            {
                model.FromItemNameRange = null;
                model.ToItemNameRange = null;
            }
            if (model.ItemCategoryID != null)
            {
                model.FromItemCategoryRange = null;
                model.ToItemCategoryRange = null;
            }
            if (model.SupplierID != null)//priority is for supplier id so item range is set to null
            {
                model.FromSupplierRange = null;
                model.ToSupplierRange = null;
            }
            if (model.Type == "Item-Wise")
            {
                var GSTItemwise = dbEntity.SpRptGSTItemwiseSummary(
                            InvoiceDateFrom,
                            InvoiceDateTo, 
                            model.ItemLocationID,
                            model.SupplierTaxSubCategoryID,
                            model.SupplierID,
                            model.ItemCategoryID,
                            model.ItemID,
                            model.FromGSTRateRange,
                            model.ToGSTRateRange,
                            model.SupplierGSTNoID,
                            model.IGST,
                            model.UserID,
                            GeneralBO.FinYear,
                            GeneralBO.ApplicationID,
                            GeneralBO.LocationID
                        ).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/GST/GSTItemwiseSummary.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GSTDataSet", GSTItemwise));
            }
            if (model.Type == "Invoice-Wise")
            {
                var GSTInvoicewise = dbEntity.SpRptGSTInvoicewiseSummary(
                       InvoiceDateFrom,
                       InvoiceDateTo,
                       model.ItemLocationID,
                       model.SupplierTaxSubCategoryID,
                       model.SupplierID,
                       model.FromGSTRateRange,
                       model.ToGSTRateRange,
                       model.SupplierGSTNoID,
                       model.IGST,
                       model.UserID,
                       GeneralBO.FinYear,
                       GeneralBO.ApplicationID,
                       GeneralBO.LocationID
                    ).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/GST/GSTInvoicewiseSummary.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GSTInvoiceDataSet", GSTInvoicewise));
            }
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
    }
}
