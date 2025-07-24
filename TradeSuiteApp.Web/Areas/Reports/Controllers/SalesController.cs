using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using iTextSharp.text.pdf;
using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.WebForms;
using Org.BouncyCastle.Ocsp;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Xml.Linq;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Reports.Models;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Utils;
using static System.Net.Mime.MediaTypeNames;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class SalesController : BaseReportController
    {
        private ReportsEntities dbEntity;
        private IReportContract reportBL;
        private ILocationContract locationBL;
        private ICategoryContract categoryBL;
        private ISupplierContract supplierBL;
        private IEmployeeContract employeeBL;
        private ICustomerContract customerBL;
        private IBatchTypeContract batchtypeBL;
        private IGSTContract igstBL;
        private IBatchTypeContract batchTypeBL;
        private IStateContract stateBL;
        private ISalesOrder salesOrderBL;
        private IProformaInvoice proformaInvoiceBL;
        private ISalesInvoice salesInvoiceBL;
        private IAddressContract addressBL;
        private IStatusContract statusBL;
        private IServiceSalesInvoiceContract servicesalesInvoiceBL;
        private IAgeingBucketContract ageingBucketBL;
        private ISubmasterContract submasterBL;
        private ICounterSalesContract counterSalesBL;
        private IPaymentModeContract paymentModeBL;
        private IStockAdjustmentContract stockAdjustmentBL;
        private ICounterSalesReturn countersalesReturnBL;
        private ISalesGoodsReceiptNote SalesGoodsReceiptNoteBL;
        private ISalesReturn salesReturnBL;

        private DateTime StartDate, EndDate, ChequeDate, ReceiptDateFrom, ReceiptDateTo, ReportAsAt;
        private SelectList AtoZRange = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
        private SelectList Number = new SelectList(Enumerable.Range(1, 9 - 1 + 1).Select(c => (int)c).ToList());


        public SalesController()
        {
            reportBL = new ReportBL();
            locationBL = new LocationBL();
            categoryBL = new CategoryBL();
            supplierBL = new SupplierBL();
            employeeBL = new EmployeeBL();
            customerBL = new CustomerBL();
            batchtypeBL = new BatchTypeBL();
            igstBL = new GSTBL();
            batchTypeBL = new BatchTypeBL();
            stateBL = new StateBL();
            salesOrderBL = new SalesOrderBL();
            proformaInvoiceBL = new ProformaInvoiceBL();
            salesInvoiceBL = new SalesInvoiceBL();
            addressBL = new AddressBL();
            statusBL = new StatusBL();
            servicesalesInvoiceBL = new ServiceSalesInvoiceBL();
            ageingBucketBL = new AgeingBucketBL();
            submasterBL = new SubmasterBL();
            counterSalesBL = new CounterSalesBL();
            paymentModeBL = new PaymentModeBL();
            SalesGoodsReceiptNoteBL = new SalesGoodsReceiptNoteBL();
            dbEntity = new ReportsEntities();
            stockAdjustmentBL = new StockAdjustmentBL();
            ViewBag.FinStartDate = GeneralBO.FinStartDate;
            ViewBag.CurrentDate = General.FormatDate(DateTime.Now);
            countersalesReturnBL = new CounterSalesReturnBL();
            salesReturnBL = new SalesReturnBL();
        }

        // GET: Reports/Sales
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAutoComplete(string Term = "", string Table = "")
        {
            return Json(reportBL.GetAutoComplete(Term, Table).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerRange(string from_range)
        {
            char range = Convert.ToChar(from_range);
            SalesModel rep = new SalesModel();
            rep.ToCustomerRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            return Json(new { Status = "success", data = rep.ToCustomerRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemCategoryRange(string from_range)
        {
            char range = Convert.ToChar(from_range);
            SalesModel rep = new SalesModel();
            rep.ToCategoryRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            return Json(new { Status = "success", data = rep.ToCategoryRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemNameRange(string from_range)
        {
            char range = Convert.ToChar(from_range);
            SalesModel rep = new SalesModel();
            rep.ToItemRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            return Json(new { Status = "success", data = rep.ToItemRangeList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGstToRate(decimal FromGst)
        {
            List<GSTBO> GSTRate = igstBL.GetGstList().Where(x => x.IGSTPercentage >= FromGst).ToList();
            return Json(new { Status = "success", data = GSTRate }, JsonRequestBehavior.AllowGet);
        }

        // GET: Reports/Sales
        [HttpGet]
        public ActionResult SalesReports()
        {
            SalesModel rep = new SalesModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.FromCustomerRangeList = AtoZRange;
            rep.ToCustomerRangeList = AtoZRange;
            rep.LocationID = GeneralBO.LocationID;
            rep.ItemLocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.Username = GeneralBO.EmployeeName;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            rep.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            rep.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            rep.BatchTypeList = new SelectList(batchtypeBL.GetBatchTypeList(), "ID", "Name");
            rep.SalesIncentiveCategoryList = new SelectList(categoryBL.GetSalesIncentiveCategoryList(0), "ID", "Name");
            rep.ReceiptDateFrom = GeneralBO.FinStartDate;
            rep.ReceiptDateTo = General.FormatDate(DateTime.Now);
            rep.FromItemRangeList = AtoZRange;
            rep.ToItemRangeList = AtoZRange;
            rep.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            rep.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            return View(rep);
        }
        // GET: Reports/Sales
        [HttpPost]
        public ActionResult SalesReports(SalesModel model)
        {
            //if (model.FromDate != null)
            //    StartDate = General.ToDateTime(model.FromDatefor);
            //if (model.ToDate != null)
            //    EndDate = General.ToDateTime(model.ToDate);
            if (model.ReceiptDateFrom != null)
                ReceiptDateFrom = General.ToDateTime(model.ReceiptDateFrom);
            if (model.ReceiptDateTo != null)
                ReceiptDateTo = General.ToDateTime(model.ReceiptDateTo);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            FilterParam = new ReportParameter("Filters", model.Filters);

            var Sales = new object();
            if (model.SalesType == "SalesByBranch")
            {
                if (model.ReportType == "Summary")
                {
                    Sales = dbEntity.SpRptSalesByBranchSummary(
                    model.FromDate,
                    model.ToDate,
                    model.ItemID,
                    model.ItemCategoryID,
                    model.SalesCategoryID,
                    model.CustomerCategoryID,
                    model.ItemLocationID,
                    model.BatchTypeID,
                    model.ItemType,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByBranchSummary");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByBranchDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Branch Summary Report");
                }
                else if (model.ReportType == "Micro")
                {
                    Sales = dbEntity.SpRptSalesByBranchSummary(
                    StartDate,
                    EndDate,
                    model.ItemID,
                    model.ItemCategoryID,
                    model.SalesCategoryID,
                    model.CustomerCategoryID,
                    model.ItemLocationID,
                    model.BatchTypeID,
                    model.ItemType,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByBranchMicro");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByBranchDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Branch Micro Report");

                }
                else
                {
                    Sales = dbEntity.SpRptSalesByBranch(
                    StartDate,
                    EndDate,
                    model.ItemID,
                    model.ItemCategoryID,
                    model.SalesCategoryID,
                    model.CustomerCategoryID,
                    model.ItemLocationID,
                    model.ItemType,
                    model.BatchTypeID,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByBranchDetail");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByBranchDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Branch Detail Report");
                }
            }
            else if (model.SalesType == "SalesByCustomer")
            {

                if (model.ReportType == "Summary")
                {
                    Sales = dbEntity.SpRptSalesByCustomerSummary(
                                        StartDate,
                                        EndDate,
                                        model.InvoiceNOFromID,
                                        model.InvoiceNOToID,
                                        model.CustomerCodeFromID,
                                        model.CustomerCodeToID,
                                        model.FromCustomerRange,
                                        model.ToCustomerRange,
                                        model.CustomerID,
                                        model.CustomerCategoryID,
                                        model.ItemCodeFromID,
                                        model.ItemCodeToID,
                                        model.ItemFromRange,
                                        model.ItemToRange,
                                        model.ItemID,
                                        model.SalesCategoryID,
                                        model.ItemCategoryID,
                                        model.BatchTypeID,
                                        model.ItemLocationID,
                                        model.ItemType,
                                        model.UserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByCustomerSummary");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByCustomerDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Customer Summary Report");
                }
                else if (model.ReportType == "Micro")
                {
                    Sales = dbEntity.SpRptSalesByCustomer(
                                        StartDate,
                                        EndDate,
                                        model.InvoiceNOFromID,
                                        model.InvoiceNOToID,
                                        model.CustomerCodeFromID,
                                        model.CustomerCodeToID,
                                        model.FromCustomerRange,
                                        model.ToCustomerRange,
                                        model.CustomerID,
                                        model.CustomerCategoryID,
                                        model.ItemCodeFromID,
                                        model.ItemCodeToID,
                                        model.ItemFromRange,
                                        model.ItemToRange,
                                        model.ItemID,
                                        model.SalesCategoryID,
                                        model.ItemCategoryID,
                                        model.BatchTypeID,
                                        model.ItemLocationID,
                                        model.ItemType,
                                        model.UserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByCustomerMicro");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByCustomerDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Customer Micro Report");

                }

                else
                {
                    Sales = dbEntity.SpRptSalesByCustomer(
                                        StartDate,
                                        EndDate,
                                        model.InvoiceNOFromID,
                                        model.InvoiceNOToID,
                                        model.CustomerCodeFromID,
                                        model.CustomerCodeToID,
                                        model.FromCustomerRange,
                                        model.ToCustomerRange,
                                        model.CustomerID,
                                        model.CustomerCategoryID,
                                        model.ItemCodeFromID,
                                        model.ItemCodeToID,
                                        model.ItemFromRange,
                                        model.ItemToRange,
                                        model.ItemID,
                                        model.SalesCategoryID,
                                        model.ItemCategoryID,
                                        model.BatchTypeID,
                                        model.ItemLocationID,
                                        model.ItemType,
                                        model.UserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByCustomerDetail");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByCustomerDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Customer Detail Report");
                }
            }
            else if (model.SalesType == "SalesByItem")
            {
                Sales = dbEntity.SpRptSalesByItem(
                                        StartDate,
                                        EndDate,
                                        model.SalesCategoryID,
                                        model.ItemCategoryID,
                                        model.ItemLocationID,
                                        model.ItemCodeFromID,
                                        model.ItemCodeToID,
                                        model.ItemFromRange,
                                        model.ItemToRange,
                                        model.ItemID,
                                        model.BatchTypeID,
                                        model.ItemType,
                                        model.UserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
                if (model.ReportType == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByItemSummary");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByItemDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Item Summary Report");
                }
                else if (model.ReportType == "Micro")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByItemMicro");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByItemDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Item Micro Report");
                }

                else
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByItemDetail");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByItemDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By Item Detail Report");
                }
            }
            else if (model.SalesType == "SalesByFSO")
            {
                Sales = dbEntity.SpRptSalesByFSO(
                                    StartDate,
                                    EndDate,
                                    model.FSOID,
                                    model.SalesIncentiveCategoryID,
                                    model.InvoiceNOFromID,
                                    model.InvoiceNOToID,
                                    model.CustomerCodeFromID,
                                    model.CustomerCodeToID,
                                    model.FromCustomerRange,
                                    model.ToCustomerRange,
                                    model.CustomerID,
                                    model.CustomerCategoryID,
                                    model.SalesCategoryID,
                                    model.ItemLocationID,
                                    model.BatchTypeID,
                                    model.ItemType,
                                    model.UserID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID).ToList();
                if (model.ReportType == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByFSOSummary");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByFSOSummaryDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By FSO Summary Report");
                }
                else if (model.ReportType == "Micro")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByFSOMicro");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByFSODetailDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By FSO Micro Report");
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesByFSODetail");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByFSODetailDataSet", Sales));
                    ReportNameParam = new ReportParameter("ReportName", "Sales By FSO Detail Report");
                }
                reportViewer.LocalReport.DisplayName = "SalesByFSO";
            }
            else if (model.SalesType == "invoice-status")
            {
                Sales = dbEntity.SpInvoiceStatus(
                                            model.InvoiceNOFromID,
                                            model.InvoiceNOToID,
                                            StartDate,
                                            EndDate,
                                            model.ReceiptNoFromID,
                                            model.ReceiptNoToID,
                                            ReceiptDateFrom,
                                            ReceiptDateTo,
                                            model.CustomerCodeFromID,
                                            model.CustomerCodeToID,
                                            model.FromCustomerRange,
                                            model.ToCustomerRange,
                                            model.CustomerID,
                                            model.ItemLocationID,
                                            model.ItemType,
                                            model.UserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("InvoiceStatus");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceStatusDataSet", Sales));
                reportViewer.LocalReport.DisplayName = "InvoiceStatus";
                ReportNameParam = new ReportParameter("ReportName", "Invoice Status Report");

            }
            else
            {
                Sales = dbEntity.SpReceiptVoucher(
                                            ReceiptDateFrom,
                                            ReceiptDateTo,
                                            model.CustomerCodeFromID,
                                            model.CustomerID,
                                            model.ItemLocationID,
                                            model.UserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID
                                            ).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("ReceiptVoucher");
                reportViewer.LocalReport.DisplayName = "ReceiptVoucher";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReceiptVoucherDataSet", Sales));
                ReportNameParam = new ReportParameter("ReportName", "Receipt Voucher Report");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });

            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        // GET: Reports/Sales
        public ActionResult SalesOrder()
        {
            List<CategoryBO> ItemCategoryList = categoryBL.GetItemCategoryForSales();
            List<CategoryBO> SalesCategoryList = categoryBL.GetSalesCategoryList(222);
            SalesOrderReportModel rep = new SalesOrderReportModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.A2ZRangeList = AtoZRange;
            rep.ItemCategoryList = new SelectList(ItemCategoryList, "ID", "Name");
            rep.SalesCategoryList = new SelectList(SalesCategoryList, "ID", "Name");
            rep.LocationID = GeneralBO.LocationID;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(GeneralBO.CreatedUserID), "ID", "Name");

            rep.StatusList = new SelectList(statusBL.GetStatusList("SalesOrderReport"), "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult SalesOrder(SalesOrderReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);

            ReportNameParam = new ReportParameter("ReportName", "Sales Order " + model.ItemType + " Items " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var SalesOrder = new object();
            reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrder" + model.ReportType);

            if (model.ReportType == "summary")
            {
                SalesOrder = dbEntity.SpRptSalesOrderSummary(
                                model.FromDate,
                                model.ToDate,
                                model.ItemCategoryID,
                                model.LocationID,
                                model.CustomerFromID,
                                model.CustomerToID,
                                model.FromCustomerRange,
                                model.ToCustomerRange,
                                model.CustomerID,
                                model.SalesOrderFromID,
                                model.SalesOrderToID,
                                model.Status,
                                model.ItemType,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.DisplayName = "SalesOrderSummary";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderSummaryDataSet", SalesOrder));
            }
            else
            {
                SalesOrder = dbEntity.SpRptSalesOrderDetail(
                                        model.FromDate,
                                        model.ToDate,
                                        model.SalesOrderFromID,
                                        model.SalesOrderToID,
                                        model.CustomerFromID,
                                        model.CustomerToID,
                                        model.FromCustomerRange,
                                        model.ToCustomerRange,
                                        model.CustomerID,
                                        model.LocationID,
                                        model.ItemCategoryID,
                                        model.ItemFromID,
                                        model.ItemToID,
                                        model.ItemID,
                                        model.Status,
                                        model.ItemType,
                                        GeneralBO.CreatedUserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.DisplayName = "SalesOrderDetail";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderDetailDataSet", SalesOrder));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam, });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //  GET: Reports/Sales
        public ActionResult MISReports()
        {
            SalesModel rep = new SalesModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.ReportAsAt = General.FormatDate(DateTime.Now);
            rep.FromCustomerRangeList = AtoZRange;
            rep.ToCustomerRangeList = AtoZRange;
            rep.FromCategoryRangeList = AtoZRange;
            rep.ToCategoryRangeList = AtoZRange;
            rep.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryForSales(), "ID", "Name");
            rep.FromItemRangeList = AtoZRange;
            rep.ToItemRangeList = AtoZRange;
            rep.LocationID = GeneralBO.LocationID;
            rep.ItemLocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.Username = GeneralBO.EmployeeName;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            //rep.AgeingBucketsList = new SelectList(
            //                        new List<SelectListItem>
            //                        {
            //                            new SelectListItem { Text = "AB1", Value = "AB1"},
            //                            new SelectListItem { Text = "AB2", Value = "AB2"},
            //                            new SelectListItem { Text = "AB3", Value = "AB3"},
            //                            new SelectListItem { Text = "AB4", Value = "AB4"},
            //                            new SelectListItem { Text = "AB5", Value = "AB5"},
            //                        }, "Value", "Text");
            //rep.AgeingBucketsList = new SelectList(ageingBucketBL.GetAgeingBucketList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult MISReports(SalesModel model)
        {
            SalesModel rep = new SalesModel();
            if (model.ReportAsAt != null)
                ReportAsAt = General.ToDateTime(model.ReportAsAt);
            //if (model.FromDate != null)
            //    StartDate = General.ToDateTime(model.FromDate);
            //if (model.ToDate != null)
            //    EndDate = General.ToDateTime(model.ToDate);
            //if (model.InvoiceDateFrom != null)
            //    StartDate = General.ToDateTime(model.InvoiceDateFrom);
            //if (model.InvoiceDateTo != null)
            //    EndDate = General.ToDateTime(model.InvoiceDateTo);
            if (model.InvoiceNOFromID == 0)
                model.InvoiceNOFromID = null;
            if (model.InvoiceNOToID == 0)
                model.InvoiceNOToID = null;
            if (model.CustomerCodeFromID == 0)
                model.CustomerCodeFromID = null;
            if (model.CustomerCodeToID == 0)
                model.CustomerCodeToID = null;
            if (model.ItemCodeFromID == 0)
                model.ItemCodeFromID = null;
            if (model.ItemCodeToID == 0)
                model.ItemCodeToID = null;
            if (model.ItemID == 0)
                model.ItemID = null;
            if (model.CustomerID != null)//priority is for item id so item range is set to null
            {
                model.FromCustomerRange = null;
                model.ToCustomerRange = null;
            }
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            DateAsOnParam = new ReportParameter("DateAsOn", ReportAsAt.ToShortDateString());
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);

            if (model.SalesType == "CustomerAgeing")
            {
                var CustomerAgeing = dbEntity.SpCustomerAgeing(
                                                ReportAsAt,
                                                model.ItemCodeFromID,
                                                model.ItemCodeToID,
                                                model.ItemFromRange,
                                                model.ItemToRange,
                                                model.ItemID,
                                                model.ItemLocationID,
                                                model.CustomerCodeFromID,
                                                model.CustomerCodeToID,
                                                model.FromCustomerRange,
                                                model.ToCustomerRange,
                                                model.CustomerID,
                                                model.InvoiceNOFromID,
                                                model.InvoiceNOToID,
                                                model.FromDate,
                                                model.ToDate,
                                                model.AgeingBucketID,
                                                model.UserID,
                                                GeneralBO.LocationID).ToList();
                if (model.AgeingBucketID > 0)
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("CustomerAgeing" + model.ReportType + "AB" + model.AgeingBucketID);
                }

                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerAgeing" + model.ReportType + "DataSet", CustomerAgeing));
                ReportNameParam = new ReportParameter("ReportName", "Customer Ageing" + " " + model.ReportType);
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, FilterParam, DateAsOnParam });
                ViewBag.ReportViewer = reportViewer;
                return View("~/Areas/Reports/Views/ReportViewer.cshtml");
            }
            else if (model.SalesType == "SalesAndMarginByBranch")
            {
                var Sales = dbEntity.SpRptSalesAndMarginByBranch(
                                            StartDate,
                                            EndDate,
                                            model.InvoiceNOFromID,
                                            model.InvoiceNOToID,
                                            model.FromCustomerRange,
                                            model.ToCustomerRange,
                                            model.CustomerCodeFromID,
                                            model.CustomerCodeToID,
                                            model.CustomerID,
                                            model.ItemLocationID,
                                            model.FromCategoryRange,
                                            model.ToCategoryRange,
                                            model.ItemCategoryID,
                                            model.ItemCodeFromID,
                                            model.ItemCodeToID,
                                            model.ItemFromRange,
                                            model.ItemToRange,
                                            model.ItemID,
                                            model.UserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/" + model.SalesMarginType + model.ReportType + ".rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesAndMarginByBranchDataSet", Sales));
                if (model.SalesMarginType == "SalesAndMarginByDate")
                {
                    if (model.ReportType == "Summary")
                    {
                        ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesAndMarginByDateSummary);
                    }
                    else
                    {
                        ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesAndMarginByDateDetail);
                    }
                }
                else if (model.SalesMarginType == "SalesAndMarginByBranch")
                {
                    if (model.ReportType == "Summary")
                    {
                        ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesAndMarginByBranchSummary);
                    }
                    else
                    {
                        ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesAndMarginByBranchDetail);
                    }
                }
                else if (model.SalesMarginType == "SalesAndMarginByCustomer")
                {
                    if (model.ReportType == "Summary")
                    {
                        ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesAndMarginByCustomerSummary);
                    }
                    else
                    {
                        ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesAndMarginByCustomerDetail);
                    }
                }
                else
                {
                    if (model.ReportType == "Summary")
                    {
                        ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesAndMarginByItemSummary);
                    }
                    else
                    {
                        ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesAndMarginByItemDetail);
                    }
                }
            }
            else if (model.SalesType == "CustomerHistory")
            {
                var Sales = dbEntity.SpCustomerHistory(
                                                StartDate,
                                                EndDate,
                                                model.CustomerCodeFromID,
                                                model.CustomerCodeToID,
                                                model.CustomerID,
                                                model.UserID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();

                var CustomerHistoryArranged = new List<SpCustomerHistory_Result>();
                SpCustomerHistory_Result item;

                for (int i = 0; i < Sales.Count(); i++)
                {
                    item = Sales[i];
                    if (i == 0)
                    {
                        CustomerHistoryArranged.Add(item);
                        continue;
                    }

                    //item.Debit = Sales[i - 1].CumulativeValue;
                    item.CumulativeValue = Sales[i - 1].CumulativeValue + (decimal)item.Debit - (decimal)item.Credit;

                    CustomerHistoryArranged.Add(item);
                }
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/CustomerHistory.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerHistoryDataSet", CustomerHistoryArranged));
                ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.CustomerHistory);
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        // GET: Reports/Sales
        [HttpGet]
        public ActionResult ChequeStatus()
        {
            SalesModel rep = new SalesModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.ReceiptDateFrom = GeneralBO.FinStartDate;
            rep.ReceiptDateTo = General.FormatDate(DateTime.Now);
            rep.ChequeDate = GeneralBO.FinStartDate;
            rep.LocationID = GeneralBO.LocationID;
            rep.ItemLocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            rep.ChequeStatusList = new SelectList(statusBL.GetStatusList("ChequeStatusReport"), "Value", "Text");

            return View(rep);
        }
        [HttpPost]
        public ActionResult ChequeStatus(SalesModel model)
        {
            if (model.ReceiptDateFrom != null)
                StartDate = General.ToDateTime(model.ReceiptDateFrom);
            if (model.ReceiptDateTo != null)
                EndDate = General.ToDateTime(model.ReceiptDateTo);
            if (model.ChequeDate != null)
                ChequeDate = General.ToDateTime(model.ChequeDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", model.ReportName + " report " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            if (model.ReportName == "cheque")
            {
                var ChequeStatus = dbEntity.SpChequeReport(
                                                  StartDate,
                                                  EndDate,
                                                  model.CustomerID,
                                                  model.ChequeStatus,
                                                  model.ItemLocationID,
                                                  model.UserID,
                                                  GeneralBO.FinYear,
                                                  GeneralBO.LocationID,
                                                  GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("ChequeReport");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ChequeReportDataSet", ChequeStatus));
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            }
            if (model.ReportName == "collection")
            {
                var ChequeStatus = dbEntity.SpCollectionReport(
                                                  StartDate,
                                                  EndDate,
                                                  model.CustomerID,
                                                  model.ChequeStatus,
                                                  model.ItemLocationID,
                                                  model.ChequeNo,
                                                  ChequeDate,
                                                  model.UserID,
                                                  GeneralBO.FinYear,
                                                  GeneralBO.LocationID,
                                                  GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("CollectionReport" + model.ReportType);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CollectionReportDataSet", ChequeStatus));
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            }
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }

        [HttpPost]
        public ActionResult PickList(int Id, string[] Preferences)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string contentType;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesOrderPickList);
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);

                var orderpicklist = dbEntity.SpSalesOrderPickList(Id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/SalesOrderPickList.rdlc";
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPickListDataSet", orderpicklist));

                byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);

                //string SalesOrderNo = orderpicklist.FirstOrDefault().SalesOrderNo;
                //string FileName = SalesOrderNo + ".pdf";
                //string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
                //string URL = "/Outputs/SalesOrder/" + FileName;

                //using (FileStream fs = new FileStream(FilePath, FileMode.Create))
                //{
                //    fs.Write(bytes, 0, bytes.Length);
                //}
                //return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
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
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CategoryWiseSalesReport()
        {
            SalesModel rep = new SalesModel();
            rep.InvoiceDateFrom = GeneralBO.FinStartDate;
            rep.InvoiceDateTo = General.FormatDate(DateTime.Now);
            return View(rep);
        }
        [HttpPost]
        public ActionResult CategoryWiseSalesReport(SalesModel model)
        {
            if (model.InvoiceDateFrom != null)
                StartDate = General.ToDateTime(model.InvoiceDateFrom);
            if (model.InvoiceDateTo != null)
                EndDate = General.ToDateTime(model.InvoiceDateTo);
            if (model.SalesInvoiceID == 0)
                model.SalesInvoiceID = null;
            if (model.SalesCategoryID == 0)
                model.SalesCategoryID = null;

            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesOrderSummary);
            var SalesInvoice = new object();
            SalesInvoice = dbEntity.SpRptSalesByCategory(
                                        //StartDate,
                                        //EndDate,
                                        model.SalesInvoiceID,
                                        model.SalesCategoryID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/SalesByCategoryReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesByCategoryDataSet", SalesInvoice));
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesByCategory);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }
        [HttpPost]
        public ActionResult Print(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var Sales = dbEntity.SpRptSalesInvoicePrint(
                    Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/SalesInvoiceBillPrint.rdlc";
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillPrintDataSet", Sales));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
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

        public ActionResult PrintInvoice(int SalesInvoiceID, string[] Preferences)
        {

            try
            {
                ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);

                ReportParameter ReportType;

                Warning[] warnings;
                string[] streamIds;
                string contentType;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                string SalesOrderNo;
                string FileName;
                string FilePath;
                string URL;


                var orderpicklist = dbEntity.SpSalesOrderPickList(SalesInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/SalesOrderPickList.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPickListDataSet", orderpicklist));
                List<byte[]> byteArrays = new List<byte[]>();

                SalesOrderNo = orderpicklist.FirstOrDefault().SalesOrderNo;
                FileName = SalesOrderNo + ".pdf";
                FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
                URL = "/Outputs/SalesOrder/" + FileName;

                using (FileStream fs = new FileStream(FilePath, FileMode.Append))
                {
                    foreach (var Preference in Preferences)
                    {
                        ReportType = new ReportParameter("ReportType", Preference);
                        reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
                        byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);


                        fs.Write(bytes, 0, bytes.Length);

                    }

                    //byteArrays.Add(bytes);
                }

                return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }



        }


        [HttpGet]
        public ActionResult CustomerSubLedger()
        {
            SalesModel rep = new SalesModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.ItemLocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.Username = GeneralBO.EmployeeName;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            return View(rep);
        }
        [HttpPost]
        public ActionResult CustomerSubLedger(SalesModel model)
        {
            //if (model.FromDate != null)
            //    StartDate = General.ToDateTime(model.FromDate);
            //if (model.ToDate != null)
            //    EndDate = General.ToDateTime(model.ToDate);

            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Customer Sub Ledger Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            var CustomerSubLedger = new object();
            if (model.ReportType == "summary")
            {
                CustomerSubLedger = dbEntity.SpRptCustomerSubLedgerSummary(
                                        model.FromDate,
                                        model.ToDate,
                                        model.CustomerID,
                                        model.ItemLocationID,
                                        model.UserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/CustomerSubLedgerSummary.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerSubLedgerDataSetSummary", CustomerSubLedger));
            }
            else
            {
                CustomerSubLedger = dbEntity.SpRptCustomerSubLedger(
                                        StartDate,
                                        EndDate,
                                        model.CustomerID,
                                        model.ItemLocationID,
                                        model.UserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/CustomerSubLedger.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerSubLedgerDataSet", CustomerSubLedger));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpPost]
        public void PickListProformaInvoice(int Id)
        {

            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoicePickList);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoicePickList = dbEntity.SpProformaInvoicePickList(Id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/ProformaPickList.rdlc";
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoicePickList);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePickListDataSet", ProformaInvoicePickList));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);

            // Open generated PDF.
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }

        [HttpPost]
        public void SalesReturnPrint(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var Sales = dbEntity.SpSalesReturnPrint(
                    Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/SalesReturnPrint.rdlc";
            ReportNameParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesReturnPrintDataSet", Sales));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }




        [HttpGet]
        public ActionResult SalesReturn()
        {
            SalesModel rep = new SalesModel();
            rep.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            rep.ToDateString = General.FormatDate(DateTime.Now);
            //rep.FromDate = GeneralBO.FinStartDate;
            //rep.ToDate = General.FormatDate(DateTime.Now);
            rep.DoctorList = new SelectList(employeeBL.GetDoctor(), "ID", "Name");
            rep.LocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;

            rep.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            return View(rep);
        }
        [HttpPost]
        public ActionResult SalesReturn(SalesModel model)
        {
            //if (model.FromDate != null)
            //    StartDate = General.ToDateTime(model.FromDate);
            //if (model.ToDate != null)
            //    EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "SalesReturn" + model.ReportType + " Report");
            FilterParam = new ReportParameter("Filters", "CounterSales"); //model.Filters
            //string XMLParams = XMLHelper.ParseXML(model);
            //if (model.ReportType == "DaywiseSales")
            {

                var SalesReturn = dbEntity.SpSalesReturnReport(
                   model.FromDate,
                   model.ToDate,
                   GeneralBO.FinYear,
                   GeneralBO.LocationID,
                   GeneralBO.ApplicationID).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Sales Return Report");
                //FilterParam = new ReportParameter("Filters", "daywisesales");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/SalesReturnReport.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesReturnsDataSet", SalesReturn));

            }


            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }




        public ActionResult GetItemForPriceList()
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var Sales = dbEntity.SpGetItemForPriceList().ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/PriceList.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GetItemForPriceListDataSet", Sales));
            byte[] bytes = reportViewer.LocalReport.Render("Excel", null, out contentType, out encoding, out extension, out streamIds, out warnings);
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

        public void GetItemForTurnOverDiscount(int CustomerLocationID, string Month, string FromDate, string ToDate)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string fromdate = General.ToDateTime(FromDate).ToString("dd/MMM/yyyy");
            string todate = General.ToDateTime(ToDate).ToString("dd/MMM/yyyy");
            FromDateParam = new ReportParameter("FromDate", fromdate);
            ToDateParam = new ReportParameter("ToDate", todate);
            MonthParam = new ReportParameter("Month", Month);

            var Sales = dbEntity.SpGetLocationWiseCustomerList(CustomerLocationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/TurnOverDiscount.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TurnOverDiscountDataSet", Sales));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, MonthParam });
            byte[] bytes = reportViewer.LocalReport.Render("Excel", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        [HttpGet]
        public ActionResult ShortDelivery()
        {
            SalesModel rep = new SalesModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.Locations = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            rep.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(222), "ID", "Name");
            return View(rep);
        }

        [HttpPost]
        public ActionResult ShortDelivery(SalesModel model)
        {
            //if (model.FromDate != null)
            //    StartDate = General.ToDateTime(model.FromDate);
            //if (model.ToDate != null)
            //    EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Delivery Shortage Report ");
            FilterParam = new ReportParameter("Filters", model.Filters);
            var shortDelivery = dbEntity.SpRptShortDelivery(
                StartDate,
                EndDate,
                model.Locations,
                model.ItemID,
                model.SalesCategoryID,
                model.ItemCategoryID,
                GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                ).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/ShortDelivery.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ShortDeliveryDataSet", shortDelivery));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        [HttpGet]
        public ActionResult CounterSales()
        {
            SalesModel rep = new SalesModel();
            rep.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            rep.ToDateString = General.FormatDate(DateTime.Now);
            //rep.FromDate = GeneralBO.FinStartDate;
            //rep.ToDate = General.FormatDate(DateTime.Now);
            rep.DoctorList = new SelectList(employeeBL.GetDoctor(), "ID", "Name");
            rep.LocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            return View(rep);
        }
        [HttpPost]
        public ActionResult CounterSales(SalesModel model)
        {
            //if (model.FromDate != null)
            //    StartDate = General.ToDateTime(model.FromDate);
            //if (model.ToDate != null)
            //    EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Counter Sales " + model.ReportType + " Report");
            FilterParam = new ReportParameter("Filters", "CounterSales"); //model.Filters
            string XMLParams = XMLHelper.ParseXML(model);
            if (model.ReportType == "DaywiseSales")
            {

                var DaywiseSales = dbEntity.SpRptDaywiseSales(
                XMLParams
                ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Daywise Sales Report");
                //FilterParam = new ReportParameter("Filters", "daywisesales");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/DaywiseSalesReport.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DaywiseSalesDataSet", DaywiseSales));

            }
            else if (model.ReportType == "ScheduleX")
            {

                var ScheduleHSales = dbEntity.SpGetScheduleHMedicineSale(
                XMLParams
                ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "ScheduleX Medicines Sales Report");
                //FilterParam = new ReportParameter("Filters", "daywisesales");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/ScheduleH.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ScheduleHDataSet", ScheduleHSales));

            }
            else
            {
                if (model.ReportType == "Summary")
                {
                    var CounterSalesSummary = dbEntity.SpRptCounterSalesSummary(
                    model.FromDate,
                    model.ToDate,
                    model.DoctorID,
                    model.PatientID,
                    model.PaymentModeID,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                    ).ToList();

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/CounterSalesSummary.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesSummaryDataSet", CounterSalesSummary));
                }
                else
                {
                    var CounterSales = dbEntity.SpRptCounterSales(
                    model.FromDate,
                    model.ToDate,
                    model.DoctorID,
                    model.PatientID,
                    model.PaymentModeID,
                    model.ItemID,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                    ).ToList();
                    if (model.ReportType == "Micro")
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/CounterSalesMicro.rdlc";
                    }
                    else
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/CounterSales.rdlc";
                    }
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesDataSet", CounterSales));
                }

            }

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }


        [HttpGet]
        public ActionResult GSTReport()
        {
            List<CategoryBO> ItemCategoryList = categoryBL.GetItemCategoryForSales();
            SalesGSTModel rep = new SalesGSTModel();
            rep.InvoiceDateFrom = General.FormatDate(General.FirstDayOfMonth);
            rep.InvoiceDateTo = General.FormatDate(DateTime.Now);
            rep.UserID = GeneralBO.CreatedUserID;
            rep.FromCustomerRangeList = AtoZRange;
            rep.ToCustomerRangeList = AtoZRange;
            rep.FromItemRangeList = AtoZRange;
            rep.ToItemRangeList = AtoZRange;
            //rep.FromGSTRateRangeList = AtoZRange;
            rep.CustomerTaxCategoryList = new SelectList(categoryBL.GetCustomerTaxCategoryList(), "ID", "Name");
            rep.ItemCategoryList = new SelectList(ItemCategoryList, "ID", "Name");
            rep.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            rep.ToGSTRateRangeList = new SelectList(
                                               igstBL.GetGstList(), "IGSTPercentage", "IGSTPercentage");
            rep.FromGSTRateRangeList = new SelectList(
                                                 igstBL.GetGstList(), "IGSTPercentage", "IGSTPercentage");
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            rep.Locations = GeneralBO.LocationID;

            return View(rep);
        }

        [HttpPost]
        public ActionResult GSTReport(SalesGSTModel model)
        {
            if (model.InvoiceDateFrom != null)
                StartDate = General.ToDateTime(model.InvoiceDateFrom);
            if (model.InvoiceDateTo != null)
                EndDate = General.ToDateTime(model.InvoiceDateTo);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", model.ReportType + " " + model.IGST + " Report " + model.ReportDataType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            if (model.ReportType == "Sales Output GST" && model.ReportDataType == "Detail")
            {
                var SalesGST = dbEntity.SpRptSalesOutputGSTReport(
                    StartDate,
                    EndDate,
                    model.Locations,
                    model.CustomerTaxCategoryID,
                    model.FromCustomerRange,
                    model.ToCustomerRange,
                    model.CustomerID,
                    model.ItemCategoryID,
                    model.SalesCategoryID,
                    model.ItemFromRange,
                    model.ItemToRange,
                    model.ItemID,
                    model.InvoiceNoFromID,
                    model.InvoiceNoToID,
                    model.CustomerGSTNoID,
                    model.FromGSTRateRange,
                    model.ToGSTRateRange,
                    model.IGST,
                    model.ItemType,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                    ).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOutputGSTReport");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOutputGSTReportDataSet", SalesGST));
            }
            if (model.ReportType == "Sales Output GST" && model.ReportDataType == "Summary")
            {
                var SalesGST = dbEntity.SpRptSalesOutputGSTReportSummary(
                    StartDate,
                    EndDate,
                    model.Locations,
                    model.CustomerTaxCategoryID,
                    model.FromCustomerRange,
                    model.ToCustomerRange,
                    model.CustomerID,
                    model.InvoiceNoFromID,
                    model.InvoiceNoToID,
                    model.CustomerGSTNoID,
                    model.FromGSTRateRange,
                    model.ToGSTRateRange,
                    model.IGST,
                    model.ItemType,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                    ).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOutputGSTReportSummary");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOutputGSTSummaryDataSet", SalesGST));
            }
            if (model.ReportType == "Sales Return GST" && model.ReportDataType == "Detail")
            {
                var SalesGST = dbEntity.SpRptSalesReturnGSTReport(
                    StartDate,
                    EndDate,
                    model.Locations,
                    model.CustomerTaxCategoryID,
                    model.FromCustomerRange,
                    model.ToCustomerRange,
                    model.CustomerID,
                    model.ItemCategoryID,
                    model.SalesCategoryID,
                    model.ItemFromRange,
                    model.ItemToRange,
                    model.ItemID,
                    model.InvoiceNoFromID,
                    model.InvoiceNoToID,
                    model.CustomerGSTNoID,
                    model.GSTRateFrom,
                    model.GSTRateTo,
                    model.TransactionType,
                    model.IGST,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.ApplicationID,
                    GeneralBO.LocationID
                    ).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesReturnGSTReport");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesReturnGSTDataSet", SalesGST));
            }
            if (model.ReportType == "Sales Return GST" && model.ReportDataType == "Summary")
            {
                var SalesGST = dbEntity.SpRptSalesReturnGSTSummary(
                    StartDate,
                    EndDate,
                    model.Locations,
                    model.CustomerTaxCategoryID,
                    model.FromCustomerRange,
                    model.ToCustomerRange,
                    model.CustomerID,
                    model.InvoiceNoFromID,
                    model.InvoiceNoToID,
                    model.CustomerGSTNoID,
                    model.GSTRateFrom,
                    model.GSTRateTo,
                    model.TransactionType,
                    model.IGST,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.ApplicationID,
                    GeneralBO.LocationID
                    ).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesReturnGSTReportSummary");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesReturnGSTSummaryDataSet", SalesGST));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public void GetItemForSalesBudget(int BranchID)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var Sales = dbEntity.SpGetItemForSalesBudget(BranchID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/SalesBudget.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GetItemForSalesBudgetDataSet", Sales));
            byte[] bytes = reportViewer.LocalReport.Render("Excel", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        //[HttpPost]
        //public JsonResult TransportPermit(int StockIssueNoFrom,int StockIssueNoTo,int SalesInvoiceNoFrom,int SalesInvoiceNoTo,string ReportType)
        //{

        //    var Items = dbEntity.SpGetItemForTransportPermit(StockIssueNoFrom, StockIssueNoTo, SalesInvoiceNoFrom, SalesInvoiceNoTo, ReportType).ToList();
        //    return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public ActionResult TransportPermit(int ID)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var permit = dbEntity.SpRptTransportPermit(
                                    ID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/TransportPermit.rdlc";
            FromDateParam = new ReportParameter("Date", DateTime.Now.ToShortDateString());

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, FromDateParam });

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TransportPermitDataSet", permit));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            var transportpermit = dbEntity.SpRptTransportPermit(
                                            ID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).FirstOrDefault().TransNo;
            string FileName = transportpermit + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/TransportPermit/"), FileName);
            string URL = "/Outputs/TransportPermit/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SalesInvoice()
        {
            SalesInvoiceReportModel rep = new SalesInvoiceReportModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.ItemLocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.ItemType = rep.ItemAutoType;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            rep.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            rep.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            rep.ItemCategoryList = new SelectList(categoryBL.GetItemCategories(rep.ItemType), "ID", "Name");
            return View(rep);
        }

        [HttpPost]
        public ActionResult SalesInvoice(SalesInvoiceReportModel model)
        {

            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Sales Invoice " + model.ReportType + " Report.");
            FilterParam = new ReportParameter("Filters", model.Filters);


            var SalesInvoice = dbEntity.SpRptSalesInvoice(
                    model.FromDate,
                    model.ToDate,
                    model.ItemLocationID,
                    model.BatchTypeID,
                    model.CustomerID,
                    model.InvoiceNoFromID,
                    model.InvoiceNoToID,
                    model.StateID,
                    model.ItemType,
                    model.PaymentType,
                    model.ItemCategoryID,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            if (model.ReportType == "Detail")
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesInvoice" + model.ReportType);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceDataSet", SalesInvoice));
            }
            else
            {

                reportViewer.LocalReport.ReportPath = GetReportPath("SalesInvoice" + model.ReportType);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceDataSet", SalesInvoice));
                
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            reportViewer.LocalReport.DisplayName = "SalesInvoice" + model.ReportType;
            reportViewer.LocalReport.ReleaseSandboxAppDomain();
            if (model.ExportType == "PDF" || model.ExportType == "Excel")
            {
                ExportReport(reportViewer, model.ExportType);
                return null;
            }
            else
            {
                ViewBag.ReportViewer = reportViewer;
                return View("~/Areas/Reports/Views/ReportViewer.cshtml");
            }

        }

        [HttpPost]
        public JsonResult SalesOrderPrintPdf(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());

            // ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesOrder = salesOrderBL.GetSalesOrder(Id);
            List<SpSGetSalesOrder_Result> salesOrders = new List<SpSGetSalesOrder_Result>();
            SpSGetSalesOrder_Result salesOrder = new SpSGetSalesOrder_Result()
            {
                QuotationExpiry = SalesOrder.QuotationExpiry,
                EnquiryDate = SalesOrder.EnquiryDate,
                SalesOrderNo = SalesOrder.SONo,
                OrderDate = (DateTime)SalesOrder.SODate,
                CustomerID = (int)SalesOrder.CustomerID,
                Customer = SalesOrder.CustomerName,
                DespatchDate = (DateTime)SalesOrder.DespatchDate,
                GrossAmt = (decimal)SalesOrder.GrossAmount,
                TaxableAmt = (decimal)SalesOrder.TaxableAmount,
                RoundOff = (decimal)SalesOrder.RoundOff,
                NetAmt = (decimal)SalesOrder.NetAmount,
                IsDraft = SalesOrder.IsDraft,
                QuotationNo = SalesOrder.QuotationNo,
                CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                Remarks = SalesOrder.Remarks,
                AadhaarNo = SalesOrder.AadhaarNo,
                MinimumCurrency = SalesOrder.MinimumCurrency,
                AmountInWords = SalesOrder.AmountInWords,
                //  Remarks = SalesOrder.Remarks
                DecimalPlaces = SalesOrder.DecimalPlaces






            };
            salesOrders.Add(salesOrder);
            var CustomerDetail = customerBL.GetCustomerDetails((int)salesOrder.CustomerID);
            List<SpGetCustomerByID_Result> CustomerDetails = new List<SpGetCustomerByID_Result>();
            SpGetCustomerByID_Result customerDetail = new SpGetCustomerByID_Result()
            {
                Code = CustomerDetail.Code,
                Name = CustomerDetail.Name,
                ExpiryDate = CustomerDetail.ExpiryDate,
                Currency = CustomerDetail.Currency,
                PaymentTypeName = CustomerDetail.PaymentTypeName,
            };
            CustomerDetails.Add(customerDetail);
            var SalesOrderTrans = salesOrderBL.GetSalesOrderItems(Id).Select(a => new SpGetSalesOrderItems_Result()
            {
                Unit = a.Unit,
                ItemName = a.Name,
                ItemCode = a.Code,
                FullOrLoose = a.FullOrLoose,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                MRP = (decimal)a.MRP,
                ExchangeRate = (decimal)a.ExchangeRate,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                CessAmount = a.CessAmount,
                NetAmt = (decimal)a.NetAmount,
                IGSTPercentage = (decimal)a.GSTPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                Category = a.Category,
                Make = a.Make,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                VATAmount = a.VATAmount,
                DecimalPlaces=a.DecimalPlaces
                //Model=a.Model,
                //Remarks=a.Remarks
            }).ToList();
            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithItemName");
            }
            else if (!SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithPartsNo");
            }
            else if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithItemName");
            }
            else if (!SalesOrder.PrintWithItemCode && salesOrder.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithPartsNo");
            }
            //SalesOrderQuotationPrintPdfWithItemName
            /// reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintPdf");
            /// 
            if (salesOrder.IsDraft == true)
            {
                ReportNameParam = new ReportParameter("ReportName", "QUOTATION");
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            }

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPrintPdfDataSet", salesOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderTransPrintPdfDataSet", SalesOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderCustomerDetailsPrintPdfDataSet", CustomerDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
            string URL = "/Outputs/SalesOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }




        [HttpPost]
        public JsonResult SalesOrderdetailpage(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());

            ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesOrder = salesOrderBL.GetSalesOrder(Id);
            List<SpSGetSalesOrder_Result> salesOrders = new List<SpSGetSalesOrder_Result>();
            SpSGetSalesOrder_Result salesOrder = new SpSGetSalesOrder_Result()
            {
                QuotationExpiry = SalesOrder.QuotationExpiry,
                EnquiryDate = SalesOrder.EnquiryDate,
                SalesOrderNo = SalesOrder.SONo,
                OrderDate = (DateTime)SalesOrder.SODate,
                CustomerID = (int)SalesOrder.CustomerID,
                Customer = SalesOrder.CustomerName,
                DespatchDate = (DateTime)SalesOrder.DespatchDate,
                GrossAmt = (decimal)SalesOrder.GrossAmount,
                TaxableAmt = (decimal)SalesOrder.TaxableAmount,
                RoundOff = (decimal)SalesOrder.RoundOff,
                NetAmt = (decimal)SalesOrder.NetAmount,
                IsDraft = SalesOrder.IsDraft,
                BillingAddress=SalesOrder.BillingAddress,
                QuotationNo = SalesOrder.QuotationNo,



                CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                Remarks = SalesOrder.Remarks,
                CurrencyCode = SalesOrder.CurrencyCode,
                AadhaarNo = SalesOrder.AadhaarNo,
                MinimumCurrency = SalesOrder.MinimumCurrency,
                AmountInWords = SalesOrder.AmountInWords,



            };
            salesOrders.Add(salesOrder);
            var CustomerDetail = customerBL.GetCustomerDetails((int)salesOrder.CustomerID);
            List<SpGetCustomerByID_Result> CustomerDetails = new List<SpGetCustomerByID_Result>();
            SpGetCustomerByID_Result customerDetail = new SpGetCustomerByID_Result()
            {
                Code = CustomerDetail.Code,
                Name = CustomerDetail.Name,
                ExpiryDate = CustomerDetail.ExpiryDate,
                Currency = CustomerDetail.Currency,
                PaymentTypeName = CustomerDetail.PaymentTypeName,
            };
            CustomerDetails.Add(customerDetail);
            var SalesOrderTrans = salesOrderBL.GetSalesOrderItems(Id).Select(a => new SpGetSalesOrderItems_Result()
            {
                Unit = a.Unit,
                ItemName = SalesOrder.PrintWithItemCode ? a.Name : a.PartsNumber,
                ItemCode = a.Code,
                FullOrLoose = a.FullOrLoose,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                MRP = (decimal)a.MRP,
                ExchangeRate = (decimal)a.ExchangeRate,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                CessAmount = a.CessAmount,
                NetAmt = (decimal)a.NetAmount,
                IGSTPercentage = (decimal)a.GSTPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                Category = a.Category,
                Make = a.Make,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                VATAmount = a.VATAmount
                //Model = a.Model,
                //Remarks=a.Remarks              
            }).ToList();
            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithItemName");
            }
            else if (!SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithPartsNo");
            }
            else if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithItemName");
            }
            else if (!SalesOrder.PrintWithItemCode && salesOrder.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithPartsNo");
            }
            //SalesOrderQuotationPrintPdfWithItemName
            //reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintPdf");
            if (salesOrder.IsDraft == true)
            {
                ReportNameParam = new ReportParameter("ReportName", "QUOTATION");
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPrintPdfDataSet", salesOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderTransPrintPdfDataSet", SalesOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderCustomerDetailsPrintPdfDataSet", CustomerDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
            string URL = "/Outputs/SalesOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }







        [HttpPost]
        public JsonResult SalesOrderWithItemCode(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesOrder = salesOrderBL.GetSalesOrder(Id);
            List<SpSGetSalesOrder_Result> salesOrders = new List<SpSGetSalesOrder_Result>();
            SpSGetSalesOrder_Result salesOrder = new SpSGetSalesOrder_Result()
            {
                QuotationExpiry = SalesOrder.QuotationExpiry,
                SalesOrderNo = SalesOrder.SONo,
                OrderDate = (DateTime)SalesOrder.SODate,
                CustomerID = (int)SalesOrder.CustomerID,
                Customer = SalesOrder.CustomerName,
                DespatchDate = (DateTime)SalesOrder.DespatchDate,
                GrossAmt = (decimal)SalesOrder.GrossAmount,
                TaxableAmt = (decimal)SalesOrder.TaxableAmount,
                RoundOff = (decimal)SalesOrder.RoundOff,
                NetAmt = (decimal)SalesOrder.NetAmount,
                IsDraft = SalesOrder.IsDraft,
                QuotationNo = SalesOrder.QuotationNo,
                CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                EnquiryDate = SalesOrder.EnquiryDate,
                Remarks = SalesOrder.Remarks,
                CurrencyCode = SalesOrder.CurrencyCode,
                AadhaarNo = SalesOrder.AadhaarNo,
                MinimumCurrency = SalesOrder.MinimumCurrency,
                AmountInWords = SalesOrder.AmountInWords,
                DecimalPlaces=(int)SalesOrder.DecimalPlaces

            };
            salesOrders.Add(salesOrder);
            var CustomerDetail = customerBL.GetCustomerDetails((int)salesOrder.CustomerID);
            List<SpGetCustomerByID_Result> CustomerDetails = new List<SpGetCustomerByID_Result>();
            SpGetCustomerByID_Result customerDetail = new SpGetCustomerByID_Result()
            {
                Code = CustomerDetail.Code,
                Name = CustomerDetail.Name,
                ExpiryDate = CustomerDetail.ExpiryDate,
                Currency = CustomerDetail.Currency,
                PaymentTypeName = CustomerDetail.PaymentTypeName,
            };
            CustomerDetails.Add(customerDetail);
            var SalesOrderTrans = salesOrderBL.GetSalesOrderItems(Id).Select(a => new SpGetSalesOrderItems_Result()
            {
                Unit = a.Unit,
                ItemName = SalesOrder.PrintWithItemCode ? a.Name : a.PartsNumber,
                ItemCode = a.Code,
                FullOrLoose = a.FullOrLoose,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                MRP = (decimal)a.MRP,
                ExchangeRate = (decimal)a.ExchangeRate,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                CessAmount = a.CessAmount,
                NetAmt = (decimal)a.NetAmount,
                IGSTPercentage = (decimal)a.GSTPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                Category = a.Category,
                Make = a.Make,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                Model = a.Model,
                VATAmount = a.VATAmount,
                DecimalPlaces = (int)SalesOrder.DecimalPlaces

                //Remarks=a.Remarks              
            }).ToList();
            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithItemName");
            }


            //SalesOrderQuotationPrintPdfWithItemName
            reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithItemName");
            if (salesOrder.IsDraft == true)
            {
                ReportNameParam = new ReportParameter("ReportName", "QUOTATION");
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPrintPdfDataSet", salesOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderTransPrintPdfDataSet", SalesOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderCustomerDetailsPrintPdfDataSet", CustomerDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
            string URL = "/Outputs/SalesOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public JsonResult WithPartNo(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesOrder = salesOrderBL.GetSalesOrder(Id);
            List<SpSGetSalesOrder_Result> salesOrders = new List<SpSGetSalesOrder_Result>();
            SpSGetSalesOrder_Result salesOrder = new SpSGetSalesOrder_Result()
            {
                SalesOrderNo = SalesOrder.SONo,
                OrderDate = (DateTime)SalesOrder.SODate,
                CustomerID = (int)SalesOrder.CustomerID,
                Customer = SalesOrder.CustomerName,
                DespatchDate = (DateTime)SalesOrder.DespatchDate,
                GrossAmt = (decimal)SalesOrder.GrossAmount,
                TaxableAmt = (decimal)SalesOrder.TaxableAmount,
                RoundOff = (decimal)SalesOrder.RoundOff,
                NetAmt = (decimal)SalesOrder.NetAmount,
                IsDraft = SalesOrder.IsDraft,
                QuotationNo = SalesOrder.QuotationNo,
                QuotationExpiry = SalesOrder.QuotationExpiry,
                CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                EnquiryDate = SalesOrder.EnquiryDate,
                Remarks = SalesOrder.Remarks,
                CurrencyCode = SalesOrder.CurrencyCode,
                AadhaarNo = SalesOrder.AadhaarNo,
                MinimumCurrency = SalesOrder.MinimumCurrency,
                AmountInWords = SalesOrder.AmountInWords,
                DecimalPlaces = (int)SalesOrder.DecimalPlaces



            };
            salesOrders.Add(salesOrder);
            var CustomerDetail = customerBL.GetCustomerDetails((int)salesOrder.CustomerID);
            List<SpGetCustomerByID_Result> CustomerDetails = new List<SpGetCustomerByID_Result>();
            SpGetCustomerByID_Result customerDetail = new SpGetCustomerByID_Result()
            {
                Code = CustomerDetail.Code,
                Name = CustomerDetail.Name,
                ExpiryDate = CustomerDetail.ExpiryDate,
                Currency = CustomerDetail.Currency,
                PaymentTypeName = CustomerDetail.PaymentTypeName,
            };
            CustomerDetails.Add(customerDetail);
            var SalesOrderTrans = salesOrderBL.GetSalesOrderItems(Id).Select(a => new SpGetSalesOrderItems_Result()
            {
                Unit = a.Unit,
                ItemName = a.Name,
                ItemCode = a.Code,
                FullOrLoose = a.FullOrLoose,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                MRP = (decimal)a.MRP,
                ExchangeRate = (decimal)a.ExchangeRate,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                CessAmount = a.CessAmount,
                NetAmt = (decimal)a.NetAmount,
                IGSTPercentage = (decimal)a.GSTPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                Category = a.Category,
                Make = a.Make,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                Model = a.Model,
                VATAmount = a.VATAmount,
                DecimalPlaces = (int)a.DecimalPlaces

                //Remarks=a.Remarks              
            }).ToList();
            //if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            //{
            //    reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithPartsNo");
            //}

            //SalesOrderQuotationPrintPdfWithItemName
            reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuotationPrintPdfWithPartsNo");
            if (salesOrder.IsDraft == true)
            {
                ReportNameParam = new ReportParameter("ReportName", "QUOTATION");
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPrintPdfDataSet", salesOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderTransPrintPdfDataSet", SalesOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderCustomerDetailsPrintPdfDataSet", CustomerDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
            string URL = "/Outputs/SalesOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }



        //EXPORT
        [HttpPost]
        public JsonResult SalesOrderExportPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesOrder = salesOrderBL.GetSalesOrder(Id);
            List<SpSGetSalesOrder_Result> salesOrders = new List<SpSGetSalesOrder_Result>();
            SpSGetSalesOrder_Result salesOrder = new SpSGetSalesOrder_Result()
            {
                QuotationExpiry = SalesOrder.QuotationExpiry,
                EnquiryDate = SalesOrder.EnquiryDate,
                SalesOrderNo = SalesOrder.SONo,
                OrderDate = (DateTime)SalesOrder.SODate,
                CustomerID = (int)SalesOrder.CustomerID,
                Customer = SalesOrder.CustomerName,
                DespatchDate = (DateTime)SalesOrder.DespatchDate,
                GrossAmt = (decimal)SalesOrder.GrossAmount,
                TaxableAmt = (decimal)SalesOrder.TaxableAmount,
                RoundOff = (decimal)SalesOrder.RoundOff,
                NetAmt = (decimal)SalesOrder.NetAmount,
                IsDraft = SalesOrder.IsDraft,
                QuotationNo = SalesOrder.QuotationNo,
                CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                CurrencyCode = SalesOrder.CurrencyCode,
                AadhaarNo = SalesOrder.AadhaarNo,
                MinimumCurrency = SalesOrder.MinimumCurrency,
                AmountInWords = SalesOrder.AmountInWords,
                Remarks = SalesOrder.Remarks,
                DecimalPlaces=SalesOrder.DecimalPlaces

            };
            salesOrders.Add(salesOrder);
            var CustomerDetail = customerBL.GetCustomerDetails((int)salesOrder.CustomerID);
            List<SpGetCustomerByID_Result> CustomerDetails = new List<SpGetCustomerByID_Result>();
            SpGetCustomerByID_Result customerDetail = new SpGetCustomerByID_Result()
            {
                Code = CustomerDetail.Code,
                Name = CustomerDetail.Name,
                ExpiryDate = CustomerDetail.ExpiryDate,
                Currency = CustomerDetail.Currency,
                PaymentTypeName = CustomerDetail.PaymentTypeName,
            };
            CustomerDetails.Add(customerDetail);
            var SalesOrderTrans = salesOrderBL.GetSalesOrderItems(Id).Select(a => new SpGetSalesOrderItems_Result()
            {
                Unit = a.Unit,
                ItemName = a.Name,
                ItemCode = a.Code,
                FullOrLoose = a.FullOrLoose,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                MRP = (decimal)a.MRP,
                ExchangeRate = (decimal)a.ExchangeRate,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                CessAmount = a.CessAmount,
                NetAmt = (decimal)a.NetAmount,
                IGSTPercentage = (decimal)a.GSTPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                Category = a.Category,
                Make = a.Make,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                VATAmount = a.VATAmount,
                DecimalPlaces=a.DecimalPlaces
                //Remarks = a.Remarks
            }).ToList();


            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintQuoteExportWithCode");
            }
            else if (!SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuoteExportWithPartno");
            }
            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintQuoteExportWithCode");
            }
            else if (!SalesOrder.PrintWithItemCode && salesOrder.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderExportPdfWithPartsNo");
            }
            //else

            //    reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintPdf");

            if (salesOrder.IsDraft == true)
            {
                ReportNameParam = new ReportParameter("ReportName", "QUOTATION");
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportfooterPathParam });
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPrintPdfDataSet", salesOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderTransPrintPdfDataSet", SalesOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderCustomerDetailsPrintPdfDataSet", CustomerDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
            string URL = "/Outputs/SalesOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public JsonResult SalesOrderExportDetial(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesOrder = salesOrderBL.GetSalesOrder(Id);
            List<SpSGetSalesOrder_Result> salesOrders = new List<SpSGetSalesOrder_Result>();
            SpSGetSalesOrder_Result salesOrder = new SpSGetSalesOrder_Result()
            {
                QuotationExpiry = SalesOrder.QuotationExpiry,
                EnquiryDate = SalesOrder.EnquiryDate,
                SalesOrderNo = SalesOrder.SONo,
                OrderDate = (DateTime)SalesOrder.SODate,
                CustomerID = (int)SalesOrder.CustomerID,
                Customer = SalesOrder.CustomerName,
                DespatchDate = (DateTime)SalesOrder.DespatchDate,
                GrossAmt = (decimal)SalesOrder.GrossAmount,
                TaxableAmt = (decimal)SalesOrder.TaxableAmount,
                RoundOff = (decimal)SalesOrder.RoundOff,
                NetAmt = (decimal)SalesOrder.NetAmount,
                IsDraft = SalesOrder.IsDraft,
                QuotationNo = SalesOrder.QuotationNo,
                CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                CurrencyCode = SalesOrder.CurrencyCode,
                AadhaarNo = SalesOrder.AadhaarNo,
                MinimumCurrency = SalesOrder.MinimumCurrency,
                AmountInWords = SalesOrder.AmountInWords,
                Remarks = SalesOrder.Remarks,
                DecimalPlaces= SalesOrder.DecimalPlaces

            };
            salesOrders.Add(salesOrder);
            var CustomerDetail = customerBL.GetCustomerDetails((int)salesOrder.CustomerID);
            List<SpGetCustomerByID_Result> CustomerDetails = new List<SpGetCustomerByID_Result>();
            SpGetCustomerByID_Result customerDetail = new SpGetCustomerByID_Result()
            {
                Code = CustomerDetail.Code,
                Name = CustomerDetail.Name,
                ExpiryDate = CustomerDetail.ExpiryDate,
                Currency = CustomerDetail.Currency,
                PaymentTypeName = CustomerDetail.PaymentTypeName,
            };
            CustomerDetails.Add(customerDetail);
            var SalesOrderTrans = salesOrderBL.GetSalesOrderItems(Id).Select(a => new SpGetSalesOrderItems_Result()
            {
                ItemCode = a.Code,
                Unit = a.Unit,
                ItemName = SalesOrder.PrintWithItemCode ? a.Name : a.PartsNumber,
                FullOrLoose = a.FullOrLoose,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                MRP = (decimal)a.MRP,
                ExchangeRate = (decimal)a.ExchangeRate,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                CessAmount = a.CessAmount,
                NetAmt = (decimal)a.NetAmount,
                IGSTPercentage = (decimal)a.GSTPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                Category = a.Category,
                Make = a.Make,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                VATAmount = a.VATAmount,
                DecimalPlaces=a.DecimalPlaces
                //Remarks = a.Remarks
            }).ToList();


            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintQuoteExportWithCode");
            }
            else if (!SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuoteExportWithPartno");
            }
            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintQuoteExportWithCode");
            }
            else if (!SalesOrder.PrintWithItemCode && salesOrder.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderExportPdfWithPartsNo");
            }

            //reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintPdf");

            if (salesOrder.IsDraft == true)
            {
                ReportNameParam = new ReportParameter("ReportName", "QUOTATION");
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam, ReportfooterPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPrintPdfDataSet", salesOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderTransPrintPdfDataSet", SalesOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderCustomerDetailsPrintPdfDataSet", CustomerDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
            string URL = "/Outputs/SalesOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }








        //PartNO Export
        [HttpPost]
        public JsonResult SalesOrderExportPartNo(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesOrder = salesOrderBL.GetSalesOrder(Id);
            List<SpSGetSalesOrder_Result> salesOrders = new List<SpSGetSalesOrder_Result>();
            SpSGetSalesOrder_Result salesOrder = new SpSGetSalesOrder_Result()
            {
                SalesOrderNo = SalesOrder.SONo,
                OrderDate = (DateTime)SalesOrder.SODate,
                CustomerID = (int)SalesOrder.CustomerID,
                Customer = SalesOrder.CustomerName,
                DespatchDate = (DateTime)SalesOrder.DespatchDate,
                GrossAmt = (decimal)SalesOrder.GrossAmount,
                TaxableAmt = (decimal)SalesOrder.TaxableAmount,
                RoundOff = (decimal)SalesOrder.RoundOff,
                NetAmt = (decimal)SalesOrder.NetAmount,
                IsDraft = SalesOrder.IsDraft,
                QuotationNo = SalesOrder.QuotationNo,
                QuotationExpiry = SalesOrder.QuotationExpiry,
                CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                EnquiryDate = SalesOrder.EnquiryDate,
                CurrencyCode = SalesOrder.CurrencyCode,
                AadhaarNo = SalesOrder.AadhaarNo,
                MinimumCurrency = SalesOrder.MinimumCurrency,
                AmountInWords = SalesOrder.AmountInWords,
                Remarks = SalesOrder.Remarks,
                DecimalPlaces =SalesOrder.DecimalPlaces


            };
            salesOrders.Add(salesOrder);
            var CustomerDetail = customerBL.GetCustomerDetails((int)salesOrder.CustomerID);
            List<SpGetCustomerByID_Result> CustomerDetails = new List<SpGetCustomerByID_Result>();
            SpGetCustomerByID_Result customerDetail = new SpGetCustomerByID_Result()
            {
                Code = CustomerDetail.Code,
                Name = CustomerDetail.Name,
                ExpiryDate = CustomerDetail.ExpiryDate,
                Currency = CustomerDetail.Currency,
                PaymentTypeName = CustomerDetail.PaymentTypeName,
            };
            CustomerDetails.Add(customerDetail);
            var SalesOrderTrans = salesOrderBL.GetSalesOrderItems(Id).Select(a => new SpGetSalesOrderItems_Result()
            {
                Unit = a.Unit,
                ItemName = a.Name,
                ItemCode = a.Code,
                FullOrLoose = a.FullOrLoose,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                MRP = (decimal)a.MRP,
                ExchangeRate = (decimal)a.ExchangeRate,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                CessAmount = a.CessAmount,
                NetAmt = (decimal)a.NetAmount,
                IGSTPercentage = (decimal)a.GSTPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                Category = a.Category,
                Make = a.Make,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                VATAmount = a.VATAmount,
                DecimalPlaces=a.DecimalPlaces
                //Remarks = a.Remarks
            }).ToList();


            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuoteExportWithPartno");
            }


            reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderQuoteExportWithPartno");
            if (salesOrder.IsDraft == true)
            {
                ReportNameParam = new ReportParameter("ReportName", "QUOTATION");
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam, ReportfooterPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPrintPdfDataSet", salesOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderTransPrintPdfDataSet", SalesOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderCustomerDetailsPrintPdfDataSet", CustomerDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
            string URL = "/Outputs/SalesOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }


        //ItemCode Export
        [HttpPost]
        public JsonResult SalesOrderExportWithItemCode(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesOrder = salesOrderBL.GetSalesOrder(Id);
            List<SpSGetSalesOrder_Result> salesOrders = new List<SpSGetSalesOrder_Result>();
            SpSGetSalesOrder_Result salesOrder = new SpSGetSalesOrder_Result()
            {
                SalesOrderNo = SalesOrder.SONo,
                OrderDate = (DateTime)SalesOrder.SODate,
                CustomerID = (int)SalesOrder.CustomerID,
                Customer = SalesOrder.CustomerName,
                DespatchDate = (DateTime)SalesOrder.DespatchDate,
                GrossAmt = (decimal)SalesOrder.GrossAmount,
                TaxableAmt = (decimal)SalesOrder.TaxableAmount,
                RoundOff = (decimal)SalesOrder.RoundOff,
                NetAmt = (decimal)SalesOrder.NetAmount,
                IsDraft = SalesOrder.IsDraft,
                QuotationNo = SalesOrder.QuotationNo,
                QuotationExpiry = SalesOrder.QuotationExpiry,
                CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                EnquiryDate = SalesOrder.EnquiryDate,
                CurrencyCode = SalesOrder.CurrencyCode,
                AadhaarNo = SalesOrder.AadhaarNo,
                MinimumCurrency = SalesOrder.MinimumCurrency,
                AmountInWords = SalesOrder.AmountInWords,
                Remarks = SalesOrder.Remarks,
                DecimalPlaces= SalesOrder.DecimalPlaces

            };
            salesOrders.Add(salesOrder);
            var CustomerDetail = customerBL.GetCustomerDetails((int)salesOrder.CustomerID);
            List<SpGetCustomerByID_Result> CustomerDetails = new List<SpGetCustomerByID_Result>();
            SpGetCustomerByID_Result customerDetail = new SpGetCustomerByID_Result()
            {
                Code = CustomerDetail.Code,
                Name = CustomerDetail.Name,
                ExpiryDate = CustomerDetail.ExpiryDate,
                Currency = CustomerDetail.Currency,
                PaymentTypeName = CustomerDetail.PaymentTypeName,
            };
            CustomerDetails.Add(customerDetail);
            var SalesOrderTrans = salesOrderBL.GetSalesOrderItems(Id).Select(a => new SpGetSalesOrderItems_Result()
            {
                Unit = a.Unit,
                ItemName = a.Name,
                ItemCode = a.Code,
                FullOrLoose = a.FullOrLoose,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                MRP = (decimal)a.MRP,
                ExchangeRate = (decimal)a.ExchangeRate,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                CessAmount = a.CessAmount,
                NetAmt = (decimal)a.NetAmount,
                IGSTPercentage = (decimal)a.GSTPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                Category = a.Category,
                Make = a.Make,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                VATAmount = a.VATAmount,
                DecimalPlaces=a.DecimalPlaces
                //Remarks = a.Remarks
            }).ToList();


            if (SalesOrder.PrintWithItemCode && salesOrder.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintQuoteExportWithCode");
            }


            reportViewer.LocalReport.ReportPath = GetReportPath("SalesOrderPrintQuoteExportWithCode");

            if (salesOrder.IsDraft == true)
            {
                ReportNameParam = new ReportParameter("ReportName", "QUOTATION");
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "SALES ORDER");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam, ReportfooterPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderPrintPdfDataSet", salesOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderTransPrintPdfDataSet", SalesOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesOrderCustomerDetailsPrintPdfDataSet", CustomerDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesOrder/"), FileName);
            string URL = "/Outputs/SalesOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public JsonResult ProformaInvoicePrintPdf(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoice = proformaInvoiceBL.GetProformaInvoice(Id);
            List<SpGetProformaInvoice_Result> proformaInvoices = new List<SpGetProformaInvoice_Result>();
            SpGetProformaInvoice_Result proformaInvoice = new SpGetProformaInvoice_Result()
            {
                TransNo = ProformaInvoice.TransNo,
                InvoiceDate = (DateTime)ProformaInvoice.TransDate,
                CustomerName = ProformaInvoice.CustomerName,

            };
            proformaInvoices.Add(proformaInvoice);
            var ProformaInvoiceTrans = proformaInvoiceBL.GetProformaInvoiceItems(Id).Select(a => new ProformaInvoiceItemsModel()
            {
                BatchNo = a.BatchName,
                BatchTypeName = a.BatchTypeName,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                MRP = (decimal)a.MRP,
                ItemName = a.Name,
                ItemCode = a.Code,
                PartsNumber = a.PartsNumber,
                UnitName = a.Unit,
                Category = a.Category,
                PackSize = (decimal)a.PackSize,
                SecondaryUnit = a.SecondaryUnit,
                MalayalamName = a.MalayalamName,
                Make = a.Model,
                DeliveryTerm = a.DeliveryTerm,

            }).ToList();
            if (ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceDraftPrintPdfWithItemName");
            }
            else if (!ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceDraftPrintPdfWithPartNo");
            }
            else if (ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithItemName");
            }
            else if (!ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithPartNo");
            }

            //reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePrintPdfDataSet", proformaInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoiceTransPrintPdfDataSet", ProformaInvoiceTrans));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ProformaInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProformaInvoice/"), FileName);
            string URL = "/Outputs/ProformaInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {

                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ProformaInvoiceDetail(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoice = proformaInvoiceBL.GetProformaInvoice(Id);
            List<SpGetProformaInvoice_Result> proformaInvoices = new List<SpGetProformaInvoice_Result>();
            SpGetProformaInvoice_Result proformaInvoice = new SpGetProformaInvoice_Result()
            {
                TransNo = ProformaInvoice.TransNo,
                InvoiceDate = (DateTime)ProformaInvoice.TransDate,
                CustomerName = ProformaInvoice.CustomerName,

            };
            proformaInvoices.Add(proformaInvoice);
            var ProformaInvoiceTrans = proformaInvoiceBL.GetProformaInvoiceItems(Id).Select(a => new ProformaInvoiceItemsModel()
            {
                PartsNumber = a.PartsNumber,
                ItemCode = a.Code,
                BatchNo = a.BatchName,
                BatchTypeName = a.BatchTypeName,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                MRP = (decimal)a.MRP,
                ItemName = a.ItemName,
                //ItemCode = ProformaInvoice.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.Unit,
                Category = a.Category,
                PackSize = (decimal)a.PackSize,
                SecondaryUnit = a.SecondaryUnit,
                MalayalamName = a.MalayalamName,
                DeliveryTerm = a.DeliveryTerm,
                Model = a.Model,
                SalesorderNO = a.SalesOrderNo,
                SalesOrderNo = a.SalesOrderNo,
                Orderdate = a.OrderDate,
                OrderDate = (DateTime)a.OrderDate,
                CurrencyName = a.CurrencyName
            }).ToList();
            if (ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceDraftPrintPdfWithItemName");
            }
            else if (!ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceDraftPrintPdfWithPartNo");
            }
            else if (ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithItemName");
            }
            else if (!ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithPartNo");
            }


            //reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePrintPdfDataSet", proformaInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoiceTransPrintPdfDataSet", ProformaInvoiceTrans));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ProformaInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProformaInvoice/"), FileName);
            string URL = "/Outputs/ProformaInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {

                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }







        [HttpPost]
        public JsonResult ProformaInvoiceExportPrintPdf(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoice = proformaInvoiceBL.GetProformaInvoice(Id);
            List<SpGetProformaInvoice_Result> proformaInvoices = new List<SpGetProformaInvoice_Result>();
            SpGetProformaInvoice_Result proformaInvoice = new SpGetProformaInvoice_Result()
            {
                TransNo = ProformaInvoice.TransNo,
                InvoiceDate = (DateTime)ProformaInvoice.TransDate,
                CustomerName = ProformaInvoice.CustomerName,

            };
            proformaInvoices.Add(proformaInvoice);
            var ProformaInvoiceTrans = proformaInvoiceBL.GetProformaInvoiceItems(Id).Select(a => new ProformaInvoiceItemsModel()
            {
                BatchNo = a.BatchName,
                ItemCode = a.Code,
                BatchTypeName = a.BatchTypeName,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                MRP = (decimal)a.MRP,
                ItemName = ProformaInvoice.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.Unit,
                Category = a.Category,
                PackSize = (decimal)a.PackSize,
                SecondaryUnit = a.SecondaryUnit,
                PartsNumber = a.PartsNumber,
                MalayalamName = a.MalayalamName,
                DeliveryTerm = a.DeliveryTerm,
                SalesOrderNo = a.SalesOrderNo,
                OrderDate = a.Orderdate,
                Make = a.Model,
                CurrencyName = a.CurrencyName
            }).ToList();
            if (ProformaInvoice.PrintWithItemCode)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceDraftExportWithItemcode");
            }
            else if (!ProformaInvoice.PrintWithItemCode)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithPartNo");
            }
            else if (ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithItemcode");
            }
            else if (!ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithPartNo");
            }

            //reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePrintPdfDataSet", proformaInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoiceTransPrintPdfDataSet", ProformaInvoiceTrans));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ProformaInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProformaInvoice/"), FileName);
            string URL = "/Outputs/ProformaInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {

                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ProformaInvoiceExportDetail(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoice = proformaInvoiceBL.GetProformaInvoice(Id);
            List<SpGetProformaInvoice_Result> proformaInvoices = new List<SpGetProformaInvoice_Result>();
            SpGetProformaInvoice_Result proformaInvoice = new SpGetProformaInvoice_Result()
            {
                TransNo = ProformaInvoice.TransNo,
                InvoiceDate = (DateTime)ProformaInvoice.TransDate,
                CustomerName = ProformaInvoice.CustomerName,

            };
            proformaInvoices.Add(proformaInvoice);
            var ProformaInvoiceTrans = proformaInvoiceBL.GetProformaInvoiceItems(Id).Select(a => new ProformaInvoiceItemsModel()
            {
                Make = a.Model,
                PartsNumber = a.PartsNumber,
                ItemCode = a.Code,
                BatchNo = a.BatchName,
                BatchTypeName = a.BatchTypeName,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                MRP = (decimal)a.MRP,
                ItemName = ProformaInvoice.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.Unit,
                Category = a.Category,
                PackSize = (decimal)a.PackSize,
                SecondaryUnit = a.SecondaryUnit,
                DeliveryTerm = a.DeliveryTerm,
                MalayalamName = a.MalayalamName,
                CurrencyName = a.CurrencyName,
                SalesorderNO = a.SalesOrderNo,
                SalesOrderNo = a.SalesOrderNo,
                Orderdate = a.OrderDate,
                OrderDate = a.Orderdate

            }).ToList();
            if (ProformaInvoice.PrintWithItemCode)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceDraftExportWithItemcode");
            }
            else if (!ProformaInvoice.PrintWithItemCode)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithPartNo");
            }
            else if (ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithItemcode");
            }
            else if (!ProformaInvoice.PrintWithItemCode && ProformaInvoice.IsDraft == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithPartNo");
            }

            //reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePrintPdfDataSet", proformaInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoiceTransPrintPdfDataSet", ProformaInvoiceTrans));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ProformaInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProformaInvoice/"), FileName);
            string URL = "/Outputs/ProformaInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {

                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ProformaInvoiceExportItemCode(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoice = proformaInvoiceBL.GetProformaInvoice(Id);
            List<SpGetProformaInvoice_Result> proformaInvoices = new List<SpGetProformaInvoice_Result>();
            SpGetProformaInvoice_Result proformaInvoice = new SpGetProformaInvoice_Result()
            {
                TransNo = ProformaInvoice.TransNo,
                InvoiceDate = (DateTime)ProformaInvoice.TransDate,
                CustomerName = ProformaInvoice.CustomerName,

            };
            proformaInvoices.Add(proformaInvoice);
            var ProformaInvoiceTrans = proformaInvoiceBL.GetProformaInvoiceItems(Id).Select(a => new ProformaInvoiceItemsModel()
            {
                Make = a.Model,
                PartsNumber = a.PartsNumber,
                ItemCode = a.Code,
                DeliveryTerm = a.DeliveryTerm,
                BatchNo = a.BatchName,
                BatchTypeName = a.BatchTypeName,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                MRP = (decimal)a.MRP,
                ItemName = ProformaInvoice.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.Unit,
                Category = a.Category,
                PackSize = (decimal)a.PackSize,
                SecondaryUnit = a.SecondaryUnit,
                MalayalamName = a.MalayalamName

            }).ToList();
            if (ProformaInvoice.PrintWithItemCode)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithItemcode");
            }
            //else if (!ProformaInvoice.PrintWithItemCode)
            //{
            //    reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithPartNo");
            //}
            //else

            reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithItemcode");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePrintPdfDataSet", proformaInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoiceTransPrintPdfDataSet", ProformaInvoiceTrans));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ProformaInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProformaInvoice/"), FileName);
            string URL = "/Outputs/ProformaInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {

                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult ProformaInvoiceitemcode(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoice = proformaInvoiceBL.GetProformaInvoice(Id);
            List<SpGetProformaInvoice_Result> proformaInvoices = new List<SpGetProformaInvoice_Result>();
            SpGetProformaInvoice_Result proformaInvoice = new SpGetProformaInvoice_Result()
            {
                TransNo = ProformaInvoice.TransNo,
                InvoiceDate = (DateTime)ProformaInvoice.TransDate,
                CustomerName = ProformaInvoice.CustomerName,

            };
            proformaInvoices.Add(proformaInvoice);
            var ProformaInvoiceTrans = proformaInvoiceBL.GetProformaInvoiceItems(Id).Select(a => new SpGetProformaInvoiceItems_Result()
            {
                BatchNo = a.BatchName,
                BatchTypeName = a.BatchTypeName,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                MRP = (decimal)a.MRP,
                ItemCode = ProformaInvoice.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.Unit,
                ItemName = a.ItemName,
                DeliveryTerm = a.DeliveryTerm,
                Category = a.Category,
                PackSize = (decimal)a.PackSize,
                SecondaryUnit = a.SecondaryUnit,
                MalayalamName = a.MalayalamName,
                SalesOrderNo = a.SalesOrderNo,
                OrderDate = a.Orderdate,
                Model = a.Model,
                CurrencyName = a.CurrencyName



            }).ToList();
            if (ProformaInvoice.PrintWithItemCode)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithItemName");
            }
            //else if (!ProformaInvoice.PrintWithItemName)
            //{
            //    reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithPartNo");
            //}
            //else

            reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithItemName");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePrintPdfDataSet", proformaInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoiceTransPrintPdfDataSet", ProformaInvoiceTrans));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ProformaInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProformaInvoice/"), FileName);
            string URL = "/Outputs/ProformaInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {

                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ProformaInvoiceExportPartNo(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoice = proformaInvoiceBL.GetProformaInvoice(Id);
            List<SpGetProformaInvoice_Result> proformaInvoices = new List<SpGetProformaInvoice_Result>();
            SpGetProformaInvoice_Result proformaInvoice = new SpGetProformaInvoice_Result()
            {
                TransNo = ProformaInvoice.TransNo,
                InvoiceDate = (DateTime)ProformaInvoice.TransDate,
                CustomerName = ProformaInvoice.CustomerName,

            };
            proformaInvoices.Add(proformaInvoice);
            var ProformaInvoiceTrans = proformaInvoiceBL.GetProformaInvoiceItems(Id).Select(a => new SpGetProformaInvoiceItems_Result()
            {
                ItemName = a.ItemName,
                SalesOrderNo = a.SalesOrderNo,
                OrderDate = (DateTime)a.OrderDate,
                CurrencyName = a.CurrencyName,
                Model = a.Model,
                DeliveryTerm = a.DeliveryTerm,
                BatchNo = a.BatchName,
                BatchTypeName = a.BatchTypeName,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                MRP = (decimal)a.MRP,
                PartsNumber = ProformaInvoice.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.Unit,
                Category = a.Category,
                PackSize = (decimal)a.PackSize,
                SecondaryUnit = a.SecondaryUnit,
                MalayalamName = a.MalayalamName

            }).ToList();
            if (ProformaInvoice.PrintWithItemCode)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithPartNo");
            }
            //else if (!ProformaInvoice.PrintWithItemName)
            //{
            //    reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithPartNo");
            //}
            //else

            reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoiceExportWithPartNo");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePrintPdfDataSet", proformaInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoiceTransPrintPdfDataSet", ProformaInvoiceTrans));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ProformaInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProformaInvoice/"), FileName);
            string URL = "/Outputs/ProformaInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {

                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ProformaInvoiceipartno(int Id)
        {

            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProformaInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProformaInvoice = proformaInvoiceBL.GetProformaInvoice(Id);
            List<SpGetProformaInvoice_Result> proformaInvoices = new List<SpGetProformaInvoice_Result>();
            SpGetProformaInvoice_Result proformaInvoice = new SpGetProformaInvoice_Result()
            {
                TransNo = ProformaInvoice.TransNo,
                InvoiceDate = (DateTime)ProformaInvoice.TransDate,
                CustomerName = ProformaInvoice.CustomerName,

            };
            proformaInvoices.Add(proformaInvoice);
            var ProformaInvoiceTrans = proformaInvoiceBL.GetProformaInvoiceItems(Id).Select(a => new SpGetProformaInvoiceItems_Result()
            {
                BatchNo = a.BatchName,
                BatchTypeName = a.BatchTypeName,
                DeliveryTerm = a.DeliveryTerm,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                MRP = (decimal)a.MRP,
                ItemName = a.ItemName,
                PartsNumber = ProformaInvoice.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.Unit,
                Category = a.Category,
                PackSize = (decimal)a.PackSize,
                SecondaryUnit = a.SecondaryUnit,
                MalayalamName = a.MalayalamName,
                SalesOrderNo = a.SalesOrderNo,
                OrderDate = a.Orderdate,
                Model = a.Model,
                CurrencyName = a.CurrencyName

            }).ToList();
            if (ProformaInvoice.PrintWithItemCode)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithPartNo");
            }
            //else if (!ProformaInvoice.PrintWithItemName)
            //{
            //    reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithPartNo");
            //}
            //else

            reportViewer.LocalReport.ReportPath = GetReportPath("ProformaInvoicePrintPdfWithPartNo");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoicePrintPdfDataSet", proformaInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProformaInvoiceTransPrintPdfDataSet", ProformaInvoiceTrans));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ProformaInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProformaInvoice/"), FileName);
            string URL = "/Outputs/ProformaInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {

                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult SalesInvoicePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "SALES INVOICE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesInvoice = salesInvoiceBL.GetSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetSalesInvoice_Result> salesInvoices = new List<SpGetSalesInvoice_Result>();
            SpGetSalesInvoice_Result salesInvoice = new SpGetSalesInvoice_Result()
            {
                TransNo = SalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)SalesInvoice.InvoiceDate,
                GrossAmt = (decimal)SalesInvoice.GrossAmount,
                DiscountAmt = (decimal)SalesInvoice.DiscountAmount,
                TurnoverDiscount = (decimal)SalesInvoice.TurnoverDiscount,
                FreightAmount = (decimal)SalesInvoice.FreightAmount,
                AdditionalDiscount = (decimal)SalesInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)SalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)SalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)SalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)SalesInvoice.IGSTAmount,
                RoundOff = (decimal)SalesInvoice.RoundOff,
                NetAmt = (decimal)SalesInvoice.NetAmount,
                CustomerID = (int)SalesInvoice.CustomerID,
                CustomerName = SalesInvoice.CustomerName,
                StateID = (int)SalesInvoice.StateID,
                BillingAddressID = (int)SalesInvoice.BillingAddressID,
                ShippingAddressID = (int)SalesInvoice.ShippingAddressID,
                BillingAddress = SalesInvoice.BillingAddress,
                ShippingAddress = SalesInvoice.ShippingAddress,
                CashDiscount = (decimal)SalesInvoice.CashDiscount,
                CessAmount = (decimal)SalesInvoice.CessAmount,
                CashDiscountPercentage = (decimal)SalesInvoice.CashDiscountPercentage,
                OutstandingAmount = (decimal)SalesInvoice.OutstandingAmount,
                VehicleNo = SalesInvoice.VehicleNo,
                CustomerGSTNo = SalesInvoice.CustomerGSTNo,
                IsCancelled = SalesInvoice.IsCancelled,
                IsDraft = SalesInvoice.IsDraft,
                CurrencyCode = SalesInvoice.CurrencyCode,
                // CustomerPONo=SalesInvoice.CustomerPONo,
                //  CustomerPODate= SalesInvoice.CustomerPODate,
                DONO = SalesInvoice.DONO,
                ReceiptDate = (DateTime)SalesInvoice.ReceiptDate,
                VatRegNo = SalesInvoice.VatRegNo,
                CustomerPODate = SalesInvoice.CustomerPODate,
                CustomerPONo = SalesInvoice.CustomerPONo,
                AmountInWords = SalesInvoice.AmountInWords,
                AadhaarNo = SalesInvoice.AadhaarNo,
                MinimumCurrency = SalesInvoice.MinimumCurrency,
                Remarks = SalesInvoice.Remarks,
                OtherCharges = SalesInvoice.OtherCharges,
                OtherChargesVATAmount = (decimal)SalesInvoice.OtherChargesVATAmount,

            };
            salesInvoices.Add(salesInvoice);
            var SalesInvoiceTrans = salesInvoiceBL.GetSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                packnumber = a.packnumber,
                dndate = a.dndate,
                ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                BatchID = (int)a.BatchID,
                BatchTypeID = (int)a.BatchTypeID,
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                BatchName = a.BatchName,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                Code = a.Code,
                ItemName = a.Name,
                PartsNumber = a.PartsNumber,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                ExpiryDate = a.ExpiryDate,
                PackSize = (decimal)a.PackSize,
                BatchType = a.BatchTypeName,
                CurrencyName = a.CurrencyName,
                PurchaseOrderNo = a.PurchaseOrderNo,
                PurchaseOrderDate = a.PurchaseOrderDate,
                Remarks = a.Remarks,
                Make = a.Make,
                PrintWithItemCode = a.PrintWithItemCode,
                DeliveryTerm = a.DeliveryTerm,
                VATAmount=a.VATAmount
               
            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(salesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(salesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

            string reportPath;
            //bool useItemName = SalesInvoiceTrans.Any(a => a.ItemName != a.PartsNumber);
            bool useItemName = SalesInvoiceTrans.Any(a => a.PrintWithItemCode);

            string FileName;
            if (useItemName)
            {
                reportPath = GetReportPath("SalesInvoicePrintPdfwithItemname");
            }
            else
            {
                reportPath = GetReportPath("SalesInvoicePrintPdfwithPONo");
            }

            reportViewer.LocalReport.ReportPath = reportPath;
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoicePrintPdf2DataSet", salesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceTransPrintPdf2DataSet", SalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceAmountDetailsPrintPdfDataSet", SalesInvoiceAmountDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdf2DataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);




            if (useItemName)
            {
                FileName = "SalesInvoicePrintPdfwithItemname.pdf";
            }
            else
            {
                FileName = "SalesInvoicePrintPdfwithPONo.pdf";
            }
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            string URL = "/Outputs/SalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
            //string FileName = "SalesInvoicePrintPdf.pdf";
            //string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            //string URL = "/Outputs/SalesInvoice/" + FileName;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            //return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public JsonResult SalesInvoiceDetail(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "SALES INVOICE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesInvoice = salesInvoiceBL.GetSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetSalesInvoice_Result> salesInvoices = new List<SpGetSalesInvoice_Result>();
            SpGetSalesInvoice_Result salesInvoice = new SpGetSalesInvoice_Result()
            {
                TransNo = SalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)SalesInvoice.InvoiceDate,
                GrossAmt = (decimal)SalesInvoice.GrossAmount,
                DiscountAmt = (decimal)SalesInvoice.DiscountAmount,
                TurnoverDiscount = (decimal)SalesInvoice.TurnoverDiscount,
                FreightAmount = (decimal)SalesInvoice.FreightAmount,
                AdditionalDiscount = (decimal)SalesInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)SalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)SalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)SalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)SalesInvoice.IGSTAmount,
                RoundOff = (decimal)SalesInvoice.RoundOff,
                NetAmt = (decimal)SalesInvoice.NetAmount,
                CustomerID = (int)SalesInvoice.CustomerID,
                CustomerName = SalesInvoice.CustomerName,
                StateID = (int)SalesInvoice.StateID,
                BillingAddressID = (int)SalesInvoice.BillingAddressID,
                ShippingAddressID = (int)SalesInvoice.ShippingAddressID,
                BillingAddress = SalesInvoice.BillingAddress,
                ShippingAddress = SalesInvoice.ShippingAddress,
                CashDiscount = (decimal)SalesInvoice.CashDiscount,
                CessAmount = (decimal)SalesInvoice.CessAmount,
                CashDiscountPercentage = (decimal)SalesInvoice.CashDiscountPercentage,
                OutstandingAmount = (decimal)SalesInvoice.OutstandingAmount,
                VehicleNo = SalesInvoice.VehicleNo,
                CustomerGSTNo = SalesInvoice.CustomerGSTNo,
                IsCancelled = SalesInvoice.IsCancelled,
                IsDraft = SalesInvoice.IsDraft,
                CurrencyCode = SalesInvoice.CurrencyCode,
                DONO = SalesInvoice.DONO,
                ReceiptDate = (DateTime)SalesInvoice.ReceiptDate,
                VatRegNo = SalesInvoice.VatRegNo,
                CustomerPODate = SalesInvoice.CustomerPODate,
                CustomerPONo = SalesInvoice.CustomerPONo,
                AmountInWords = SalesInvoice.AmountInWords,
                AadhaarNo = SalesInvoice.AadhaarNo,
                MinimumCurrency = SalesInvoice.MinimumCurrency,
                Remarks = SalesInvoice.Remarks,
                VATAmount=SalesInvoice.VATAmount,
                DecimalPlaces=(int)SalesInvoice.DecimalPlaces,
                OtherCharges = SalesInvoice.OtherCharges,
                OtherChargesVATAmount = (decimal)SalesInvoice.OtherChargesVATAmount,

            };
            salesInvoices.Add(salesInvoice);
            var SalesInvoiceTrans = salesInvoiceBL.GetSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                packnumber = a.packnumber,
                dndate = a.dndate,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                BatchID = (int)a.BatchID,
                BatchTypeID = (int)a.BatchTypeID,
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                BatchName = a.BatchName,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                Code = a.Code,
                ItemName = a.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                ExpiryDate = a.ExpiryDate,
                PackSize = (decimal)a.PackSize,
                BatchType = a.BatchTypeName,
                CurrencyName = a.CurrencyName,
                PurchaseOrderNo = a.PurchaseOrderNo,
                PurchaseOrderDate = a.PurchaseOrderDate,
                Remarks = a.Remarks,
                Make = a.Make,
                PrintWithItemCode = a.PrintWithItemCode,
                VATAmount=(decimal)a.VATAmount,
                DecimalPlaces=(int)a.DecimalPlaces

            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(salesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(salesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

            string reportPath;
            //bool useItemName = SalesInvoiceTrans.Any(a => a.ItemName != a.PartsNumber);
            bool useItemName = SalesInvoiceTrans.Any(a => a.PrintWithItemCode);

            string FileName;
            if (useItemName)
            {
                reportPath = GetReportPath("SalesInvoicePrintPdfwithItemname");
            }
            else
            {
                reportPath = GetReportPath("SalesInvoicePrintPdfwithPONo");
            }

            reportViewer.LocalReport.ReportPath = reportPath;
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, UserParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoicePrintPdf2DataSet", salesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceTransPrintPdf2DataSet", SalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceAmountDetailsPrintPdfDataSet", SalesInvoiceAmountDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdf2DataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);




            if (useItemName)
            {
                FileName = "SalesInvoicePrintPdfwithItemname.pdf";
            }
            else
            {
                FileName = "SalesInvoicePrintPdfwithPONo.pdf";
            }
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            string URL = "/Outputs/SalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
            //string FileName = "SalesInvoicePrintPdf.pdf";
            //string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            //string URL = "/Outputs/SalesInvoice/" + FileName;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            //return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult SalesInvoiceWithCode(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "CREDIT INVOICE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesInvoice = salesInvoiceBL.GetSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetSalesInvoice_Result> salesInvoices = new List<SpGetSalesInvoice_Result>();
            SpGetSalesInvoice_Result salesInvoice = new SpGetSalesInvoice_Result()
            {
                TransNo = SalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)SalesInvoice.InvoiceDate,
                GrossAmt = (decimal)SalesInvoice.GrossAmount,
                DiscountAmt = (decimal)SalesInvoice.DiscountAmount,
                TurnoverDiscount = (decimal)SalesInvoice.TurnoverDiscount,
                FreightAmount = (decimal)SalesInvoice.FreightAmount,
                AdditionalDiscount = (decimal)SalesInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)SalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)SalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)SalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)SalesInvoice.IGSTAmount,
                RoundOff = (decimal)SalesInvoice.RoundOff,
                NetAmt = (decimal)SalesInvoice.NetAmount,
                CustomerID = (int)SalesInvoice.CustomerID,
                CustomerName = SalesInvoice.CustomerName,
                StateID = (int)SalesInvoice.StateID,
                BillingAddressID = (int)SalesInvoice.BillingAddressID,
                ShippingAddressID = (int)SalesInvoice.ShippingAddressID,
                BillingAddress = SalesInvoice.BillingAddress,
                ShippingAddress = SalesInvoice.ShippingAddress,
                CashDiscount = (decimal)SalesInvoice.CashDiscount,
                CessAmount = (decimal)SalesInvoice.CessAmount,
                CashDiscountPercentage = (decimal)SalesInvoice.CashDiscountPercentage,
                OutstandingAmount = (decimal)SalesInvoice.OutstandingAmount,
                VehicleNo = SalesInvoice.VehicleNo,
                CustomerGSTNo = SalesInvoice.CustomerGSTNo,
                IsCancelled = SalesInvoice.IsCancelled,
                IsDraft = SalesInvoice.IsDraft,
                CurrencyCode = SalesInvoice.CurrencyCode,
                DONO = SalesInvoice.DONO,
                ReceiptDate = (DateTime)SalesInvoice.ReceiptDate,
                VatRegNo = SalesInvoice.VatRegNo,
                CustomerPODate = SalesInvoice.CustomerPODate,
                CustomerPONo = SalesInvoice.CustomerPONo,
                AmountInWords = SalesInvoice.AmountInWords,
                AadhaarNo = SalesInvoice.AadhaarNo,
                MinimumCurrency = SalesInvoice.MinimumCurrency,
                Remarks = SalesInvoice.Remarks,
                DecimalPlaces=SalesInvoice.DecimalPlaces,
                OtherCharges = SalesInvoice.OtherCharges,
                OtherChargesVATAmount = (decimal)SalesInvoice.OtherChargesVATAmount,
            };
            salesInvoices.Add(salesInvoice);
            var SalesInvoiceTrans = salesInvoiceBL.GetSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                packnumber = a.packnumber,
                dndate = a.dndate,
                ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                BatchID = (int)a.BatchID,
                BatchTypeID = (int)a.BatchTypeID,
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                BatchName = a.BatchName,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                Code = a.Code,
                ItemName = a.Name,
                PartsNumber = a.PartsNumber,
                DeliveryTerm = a.DeliveryTerm,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                ExpiryDate = a.ExpiryDate,
                PackSize = (decimal)a.PackSize,
                BatchType = a.BatchTypeName,
                CurrencyName = a.CurrencyName,
                PurchaseOrderNo = a.PurchaseOrderNo,
                PurchaseOrderDate = a.PurchaseOrderDate,
                Remarks = a.Remarks,
                Make = a.Make,
                PrintWithItemCode = a.PrintWithItemCode,
                VATAmount=a.VATAmount,
                DecimalPlaces=a.DecimalPlaces

            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(salesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(salesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

            string reportPath;
            //bool useItemName = SalesInvoiceTrans.Any(a => a.ItemName != a.PartsNumber);
            bool useItemName = SalesInvoiceTrans.Any(a => a.PrintWithItemCode);

            string FileName;
            if (useItemName)
            {
                reportPath = GetReportPath("SalesInvoicePrintPdfwithItemname");
            }

            reportViewer.LocalReport.ReportPath = GetReportPath("SalesInvoicePrintPdfwithItemname");
            //reportViewer.LocalReport.ReportPath = ("SalesInvoicePrintPdfwithItemname");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, UserParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoicePrintPdf2DataSet", salesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceTransPrintPdf2DataSet", SalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceAmountDetailsPrintPdfDataSet", SalesInvoiceAmountDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdf2DataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);




            if (useItemName)
            {
                FileName = "SalesInvoicePrintPdfwithItemname.pdf";
            }
            else
            {
                FileName = "SalesInvoicePrintPdfwithPONo.pdf";
            }
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            string URL = "/Outputs/SalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
            //string FileName = "SalesInvoicePrintPdf.pdf";
            //string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            //string URL = "/Outputs/SalesInvoice/" + FileName;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            //return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult SalesInvoicePartNo(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "CREDIT INVOICE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesInvoice = salesInvoiceBL.GetSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetSalesInvoice_Result> salesInvoices = new List<SpGetSalesInvoice_Result>();
            SpGetSalesInvoice_Result salesInvoice = new SpGetSalesInvoice_Result()
            {
                TransNo = SalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)SalesInvoice.InvoiceDate,
                GrossAmt = (decimal)SalesInvoice.GrossAmount,
                DiscountAmt = (decimal)SalesInvoice.DiscountAmount,
                TurnoverDiscount = (decimal)SalesInvoice.TurnoverDiscount,
                FreightAmount = (decimal)SalesInvoice.FreightAmount,
                AdditionalDiscount = (decimal)SalesInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)SalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)SalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)SalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)SalesInvoice.IGSTAmount,
                RoundOff = (decimal)SalesInvoice.RoundOff,
                NetAmt = (decimal)SalesInvoice.NetAmount,
                CustomerID = (int)SalesInvoice.CustomerID,
                CustomerName = SalesInvoice.CustomerName,
                StateID = (int)SalesInvoice.StateID,
                BillingAddressID = (int)SalesInvoice.BillingAddressID,
                ShippingAddressID = (int)SalesInvoice.ShippingAddressID,
                BillingAddress = SalesInvoice.BillingAddress,
                ShippingAddress = SalesInvoice.ShippingAddress,
                CashDiscount = (decimal)SalesInvoice.CashDiscount,
                CessAmount = (decimal)SalesInvoice.CessAmount,
                CashDiscountPercentage = (decimal)SalesInvoice.CashDiscountPercentage,
                OutstandingAmount = (decimal)SalesInvoice.OutstandingAmount,
                VehicleNo = SalesInvoice.VehicleNo,
                CustomerGSTNo = SalesInvoice.CustomerGSTNo,
                IsCancelled = SalesInvoice.IsCancelled,
                IsDraft = SalesInvoice.IsDraft,
                RefferenceNo = SalesInvoice.RefferenceNo,
                CurrencyCode = SalesInvoice.CurrencyCode,
                DONO = SalesInvoice.DONO,
                ReceiptDate = (DateTime)SalesInvoice.ReceiptDate,
                VatRegNo = SalesInvoice.VatRegNo,
                CustomerPODate = SalesInvoice.CustomerPODate,
                CustomerPONo = SalesInvoice.CustomerPONo,
                AmountInWords = SalesInvoice.AmountInWords,
                AadhaarNo = SalesInvoice.AadhaarNo,
                MinimumCurrency = SalesInvoice.MinimumCurrency,
                Remarks = SalesInvoice.Remarks,
                DecimalPlaces=(int)SalesInvoice.DecimalPlaces,
                OtherCharges = SalesInvoice.OtherCharges,
                OtherChargesVATAmount = (decimal)SalesInvoice.OtherChargesVATAmount,

            };
            salesInvoices.Add(salesInvoice);
            var SalesInvoiceTrans = salesInvoiceBL.GetSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                packnumber = a.packnumber,
                dndate = a.dndate,
                DeliveryTerm = a.DeliveryTerm,
                ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                BatchID = (int)a.BatchID,
                BatchTypeID = (int)a.BatchTypeID,
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                BatchName = a.BatchName,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                Code = a.Code,
                ItemName = a.Name,
                PartsNumber = a.PartsNumber,
                Make = a.Make,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                ExpiryDate = a.ExpiryDate,
                PackSize = (decimal)a.PackSize,
                BatchType = a.BatchTypeName,
                CurrencyName = a.CurrencyName,
                PurchaseOrderNo = a.PurchaseOrderNo,
                PurchaseOrderDate = a.PurchaseOrderDate,
                Remarks = a.Remarks,
                PrintWithItemCode = a.PrintWithItemCode,
               VATAmount = a.VATAmount,
               DecimalPlaces=(int)a.DecimalPlaces
            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(salesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(salesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

            string reportPath;
            //bool useItemName = SalesInvoiceTrans.Any(a => a.ItemName != a.PartsNumber);
            bool useItemName = SalesInvoiceTrans.Any(a => a.PrintWithItemCode);

            string FileName;
            if (useItemName)
            {
                reportPath = GetReportPath("SalesInvoicePrintPdfwithPONo");
            }

            reportViewer.LocalReport.ReportPath = GetReportPath("SalesInvoicePrintPdfwithPONo");
            //reportViewer.LocalReport.ReportPath = ("SalesInvoicePrintPdfwithItemname");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, UserParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoicePrintPdf2DataSet", salesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceTransPrintPdf2DataSet", SalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceAmountDetailsPrintPdfDataSet", SalesInvoiceAmountDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdf2DataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);




            if (useItemName)
            {
                FileName = "SalesInvoicePrintPdfwithItemname.pdf";
            }
            else
            {
                FileName = "SalesInvoicePrintPdfwithPONo.pdf";
            }
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            string URL = "/Outputs/SalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
            //string FileName = "SalesInvoicePrintPdf.pdf";
            //string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            //string URL = "/Outputs/SalesInvoice/" + FileName;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            //return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SalesInvoiceExportitemcode(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "CREDIT INVOICE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesInvoice = salesInvoiceBL.GetSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetSalesInvoice_Result> salesInvoices = new List<SpGetSalesInvoice_Result>();
            SpGetSalesInvoice_Result salesInvoice = new SpGetSalesInvoice_Result()
            {
                TransNo = SalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)SalesInvoice.InvoiceDate,
                GrossAmt = (decimal)SalesInvoice.GrossAmount,
                DiscountAmt = (decimal)SalesInvoice.DiscountAmount,
                TurnoverDiscount = (decimal)SalesInvoice.TurnoverDiscount,
                FreightAmount = (decimal)SalesInvoice.FreightAmount,
                AdditionalDiscount = (decimal)SalesInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)SalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)SalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)SalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)SalesInvoice.IGSTAmount,
                RoundOff = (decimal)SalesInvoice.RoundOff,
                NetAmt = (decimal)SalesInvoice.NetAmount,
                CustomerID = (int)SalesInvoice.CustomerID,
                CustomerName = SalesInvoice.CustomerName,
                StateID = (int)SalesInvoice.StateID,
                BillingAddressID = (int)SalesInvoice.BillingAddressID,
                ShippingAddressID = (int)SalesInvoice.ShippingAddressID,
                BillingAddress = SalesInvoice.BillingAddress,
                ShippingAddress = SalesInvoice.ShippingAddress,
                CashDiscount = (decimal)SalesInvoice.CashDiscount,
                CessAmount = (decimal)SalesInvoice.CessAmount,
                CashDiscountPercentage = (decimal)SalesInvoice.CashDiscountPercentage,
                OutstandingAmount = (decimal)SalesInvoice.OutstandingAmount,
                VehicleNo = SalesInvoice.VehicleNo,
                CustomerGSTNo = SalesInvoice.CustomerGSTNo,
                IsCancelled = SalesInvoice.IsCancelled,
                IsDraft = SalesInvoice.IsDraft,
                //Reportlogopath= SalesInvoice.Reportlogopath
                CurrencyCode = SalesInvoice.CurrencyCode,
                DONO = SalesInvoice.DONO,
                ReceiptDate = (DateTime)SalesInvoice.ReceiptDate,
                VatRegNo = SalesInvoice.VatRegNo,
                CustomerPODate = SalesInvoice.CustomerPODate,
                CustomerPONo = SalesInvoice.CustomerPONo,
                AmountInWords = SalesInvoice.AmountInWords,
                AadhaarNo = SalesInvoice.AadhaarNo,
                MinimumCurrency = SalesInvoice.MinimumCurrency,
                Remarks = SalesInvoice.Remarks,
                DecimalPlaces=(int)SalesInvoice.DecimalPlaces,
                OtherCharges = SalesInvoice.OtherCharges,
                OtherChargesVATAmount = (decimal)SalesInvoice.OtherChargesVATAmount,
            };
            salesInvoices.Add(salesInvoice);
            var SalesInvoiceTrans = salesInvoiceBL.GetSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                BatchID = (int)a.BatchID,
                BatchTypeID = (int)a.BatchTypeID,
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                ItemName = a.Name,
                BatchName = a.BatchName,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                Code = a.Code,
                // ItemName = a.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                ExpiryDate = a.ExpiryDate,
                PackSize = (decimal)a.PackSize,
                BatchType = a.BatchTypeName,
                CurrencyName = a.CurrencyName,
                PurchaseOrderNo = a.PurchaseOrderNo,
                PurchaseOrderDate = a.PurchaseOrderDate,
                Remarks = a.Remarks,
                Make = a.Make,
                PrintWithItemCode = a.PrintWithItemCode,
                packnumber = a.packnumber,
                dndate = a.dndate,
                VATAmount=a.VATAmount,
                DecimalPlaces=(int)a.DecimalPlaces,

            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(salesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(salesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

            string reportPath;
            //bool useItemName = SalesInvoiceTrans.Any(a => a.ItemName != a.PartsNumber);
            bool useItemName = SalesInvoiceTrans.Any(a => a.PrintWithItemCode);

            string FileName;
            if (useItemName)
            {
                reportPath = GetReportPath("SalesInvoiceExportPrintPdfwithItem");
            }

            reportViewer.LocalReport.ReportPath = GetReportPath("SalesInvoiceExportPrintPdfwithItem");
            //reportViewer.LocalReport.ReportPath = ("SalesInvoicePrintPdfwithItemname");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam, ReportfooterPathParam, UserParam });

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoicePrintPdf3DataSet", salesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceTransPrintPdf2DataSet", SalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceAmountDetailsPrintPdfDataSet", SalesInvoiceAmountDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdf2DataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);




            if (useItemName)
            {
                FileName = "SalesInvoiceExportPrintPdfwithItem.pdf";
            }
            else
            {
                FileName = "SalesInvoiceExportPrintPdfwithPONo.pdf";
            }
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            string URL = "/Outputs/SalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
            //string FileName = "SalesInvoicePrintPdf.pdf";
            //string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            //string URL = "/Outputs/SalesInvoice/" + FileName;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            //return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalesInvoiceExportpartno(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "CREDIT INVOICE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesInvoice = salesInvoiceBL.GetSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetSalesInvoice_Result> salesInvoices = new List<SpGetSalesInvoice_Result>();
            SpGetSalesInvoice_Result salesInvoice = new SpGetSalesInvoice_Result()
            {
                TransNo = SalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)SalesInvoice.InvoiceDate,
                GrossAmt = (decimal)SalesInvoice.GrossAmount,
                DiscountAmt = (decimal)SalesInvoice.DiscountAmount,
                TurnoverDiscount = (decimal)SalesInvoice.TurnoverDiscount,
                FreightAmount = (decimal)SalesInvoice.FreightAmount,
                AdditionalDiscount = (decimal)SalesInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)SalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)SalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)SalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)SalesInvoice.IGSTAmount,
                RoundOff = (decimal)SalesInvoice.RoundOff,
                NetAmt = (decimal)SalesInvoice.NetAmount,
                CustomerID = (int)SalesInvoice.CustomerID,
                CustomerName = SalesInvoice.CustomerName,
                StateID = (int)SalesInvoice.StateID,
                BillingAddressID = (int)SalesInvoice.BillingAddressID,
                ShippingAddressID = (int)SalesInvoice.ShippingAddressID,
                BillingAddress = SalesInvoice.BillingAddress,
                ShippingAddress = SalesInvoice.ShippingAddress,
                CashDiscount = (decimal)SalesInvoice.CashDiscount,
                CessAmount = (decimal)SalesInvoice.CessAmount,
                CashDiscountPercentage = (decimal)SalesInvoice.CashDiscountPercentage,
                OutstandingAmount = (decimal)SalesInvoice.OutstandingAmount,
                VehicleNo = SalesInvoice.VehicleNo,
                CustomerGSTNo = SalesInvoice.CustomerGSTNo,
                IsCancelled = SalesInvoice.IsCancelled,
                IsDraft = SalesInvoice.IsDraft,
                CurrencyCode = SalesInvoice.CurrencyCode,
                DONO = SalesInvoice.DONO,
                ReceiptDate = (DateTime)SalesInvoice.ReceiptDate,
                VatRegNo = SalesInvoice.VatRegNo,
                CustomerPODate = SalesInvoice.CustomerPODate,
                CustomerPONo = SalesInvoice.CustomerPONo,
                AmountInWords = SalesInvoice.AmountInWords,
                AadhaarNo = SalesInvoice.AadhaarNo,
                MinimumCurrency = SalesInvoice.MinimumCurrency,
                Remarks = SalesInvoice.Remarks,
                DecimalPlaces=SalesInvoice.DecimalPlaces,
                OtherCharges = SalesInvoice.OtherCharges,
                OtherChargesVATAmount = (decimal)SalesInvoice.OtherChargesVATAmount,
            };
            salesInvoices.Add(salesInvoice);
            var SalesInvoiceTrans = salesInvoiceBL.GetSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                packnumber = a.packnumber,
                dndate = a.dndate,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                BatchID = (int)a.BatchID,
                BatchTypeID = (int)a.BatchTypeID,
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                BatchName = a.BatchName,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                Code = a.Code,
                ItemName = a.Name,

                //  ItemName = a.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                ExpiryDate = a.ExpiryDate,
                PackSize = (decimal)a.PackSize,
                BatchType = a.BatchTypeName,
                CurrencyName = a.CurrencyName,
                PurchaseOrderNo = a.PurchaseOrderNo,
                PurchaseOrderDate = a.PurchaseOrderDate,
                Remarks = a.Remarks,
                Make = a.Make,
                PrintWithItemCode = a.PrintWithItemCode,
                VATAmount=a.VATAmount,
                DecimalPlaces=(int)a.DecimalPlaces


            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(salesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(salesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

            string reportPath;
            //bool useItemName = SalesInvoiceTrans.Any(a => a.ItemName != a.PartsNumber);
            bool useItemName = SalesInvoiceTrans.Any(a => a.PrintWithItemCode);

            string FileName;
            if (useItemName)
            {
                reportPath = GetReportPath("SalesInvoiceExportPrintPdfwithPONo");
            }

            reportViewer.LocalReport.ReportPath = GetReportPath("SalesInvoiceExportPrintPdfwithPONo");
            //reportViewer.LocalReport.ReportPath = ("SalesInvoicePrintPdfwithItemname");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam, ReportfooterPathParam , UserParam });


            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoicePrintPdf2DataSet", salesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceTransPrintPdf2DataSet", SalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceAmountDetailsPrintPdfDataSet", SalesInvoiceAmountDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdf2DataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);




            if (useItemName)
            {
                FileName = "SalesInvoiceExportPrintPdfwithItem.pdf";
            }
            else
            {
                FileName = "SalesInvoiceExportPrintPdfwithPONo.pdf";
            }
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            string URL = "/Outputs/SalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
            //string FileName = "SalesInvoicePrintPdf.pdf";
            //string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            //string URL = "/Outputs/SalesInvoice/" + FileName;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            //return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        public JsonResult SalesInvoiceExportPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "CREDIT INVOICE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesInvoice = salesInvoiceBL.GetSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetSalesInvoice_Result> salesInvoices = new List<SpGetSalesInvoice_Result>();
            SpGetSalesInvoice_Result salesInvoice = new SpGetSalesInvoice_Result()
            {

                TransNo = SalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)SalesInvoice.InvoiceDate,
                GrossAmt = (decimal)SalesInvoice.GrossAmount,
                DiscountAmt = (decimal)SalesInvoice.DiscountAmount,
                TurnoverDiscount = (decimal)SalesInvoice.TurnoverDiscount,
                FreightAmount = (decimal)SalesInvoice.FreightAmount,
                AdditionalDiscount = (decimal)SalesInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)SalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)SalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)SalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)SalesInvoice.IGSTAmount,
                RoundOff = (decimal)SalesInvoice.RoundOff,
                NetAmt = (decimal)SalesInvoice.NetAmount,
                CustomerID = (int)SalesInvoice.CustomerID,
                CustomerName = SalesInvoice.CustomerName,
                StateID = (int)SalesInvoice.StateID,
                BillingAddressID = (int)SalesInvoice.BillingAddressID,
                ShippingAddressID = (int)SalesInvoice.ShippingAddressID,
                BillingAddress = SalesInvoice.BillingAddress,
                ShippingAddress = SalesInvoice.ShippingAddress,
                CashDiscount = (decimal)SalesInvoice.CashDiscount,
                CessAmount = (decimal)SalesInvoice.CessAmount,
                CashDiscountPercentage = (decimal)SalesInvoice.CashDiscountPercentage,
                OutstandingAmount = (decimal)SalesInvoice.OutstandingAmount,
                VehicleNo = SalesInvoice.VehicleNo,
                CustomerGSTNo = SalesInvoice.CustomerGSTNo,
                IsCancelled = SalesInvoice.IsCancelled,
                IsDraft = SalesInvoice.IsDraft,
                CurrencyCode = SalesInvoice.CurrencyCode,
                DONO = SalesInvoice.DONO,
                ReceiptDate = (DateTime)SalesInvoice.ReceiptDate,
                VatRegNo = SalesInvoice.VatRegNo,
                CustomerPODate = SalesInvoice.CustomerPODate,
                CustomerPONo = SalesInvoice.CustomerPONo,
                AmountInWords = SalesInvoice.AmountInWords,
                AadhaarNo = SalesInvoice.AadhaarNo,
                MinimumCurrency = SalesInvoice.MinimumCurrency,
                Remarks = SalesInvoice.Remarks,
                DecimalPlaces = SalesInvoice.DecimalPlaces,
                OtherCharges = SalesInvoice.OtherCharges,
                OtherChargesVATAmount = (decimal)SalesInvoice.OtherChargesVATAmount,
            };
            salesInvoices.Add(salesInvoice);
            var SalesInvoiceTrans = salesInvoiceBL.GetSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                DeliveryTerm = a.DeliveryTerm,
                packnumber = a.packnumber,
                dndate = a.dndate,
                ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                BatchID = (int)a.BatchID,
                BatchTypeID = (int)a.BatchTypeID,
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                BatchName = a.BatchName,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                Code = a.Code,
                ItemName = a.Name,
                PartsNumber = a.PartsNumber,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                ExpiryDate = a.ExpiryDate,
                PackSize = (decimal)a.PackSize,
                BatchType = a.BatchTypeName,
                CurrencyName = a.CurrencyName,
                PurchaseOrderNo = a.PurchaseOrderNo,
                PurchaseOrderDate = a.PurchaseOrderDate,
                Remarks = a.Remarks,
                Make = a.Make,
                PrintWithItemCode = a.PrintWithItemCode,
                VATAmount=a.VATAmount

            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(salesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(salesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();


            string reportPath;
            //bool useItemName = SalesInvoiceTrans.Any(a => a.ItemName != a.PartsNumber);
            bool useItemName = SalesInvoiceTrans.Any(a => a.PrintWithItemCode);

            string FileName;
            if (useItemName)
            {
                reportPath = GetReportPath("SalesInvoiceExportPrintPdfwithItem");
            }
            else
            {
                reportPath = GetReportPath("SalesInvoiceExportPrintPdfwithPONo ");
            }
            reportViewer.LocalReport.ReportPath = reportPath;
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam, ReportfooterPathParam });

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoicePrintPdf2DataSet", salesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceTransPrintPdf2DataSet", SalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceAmountDetailsPrintPdfDataSet", SalesInvoiceAmountDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdf2DataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);




            if (useItemName)
            {
                FileName = "SalesInvoiceExportPrintPdfwithItem.pdf";
            }
            else
            {
                FileName = "SalesInvoiceExportPrintPdfwithPONo .pdf";
            }
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            string URL = "/Outputs/SalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SalesInvoiceExportDetail(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "SALES INVOICE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SalesInvoice = salesInvoiceBL.GetSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetSalesInvoice_Result> salesInvoices = new List<SpGetSalesInvoice_Result>();
            SpGetSalesInvoice_Result salesInvoice = new SpGetSalesInvoice_Result()
            {
                TransNo = SalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)SalesInvoice.InvoiceDate,
                GrossAmt = (decimal)SalesInvoice.GrossAmount,
                DiscountAmt = (decimal)SalesInvoice.DiscountAmount,
                TurnoverDiscount = (decimal)SalesInvoice.TurnoverDiscount,
                FreightAmount = (decimal)SalesInvoice.FreightAmount,
                AdditionalDiscount = (decimal)SalesInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)SalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)SalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)SalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)SalesInvoice.IGSTAmount,
                RoundOff = (decimal)SalesInvoice.RoundOff,
                NetAmt = (decimal)SalesInvoice.NetAmount,
                CustomerID = (int)SalesInvoice.CustomerID,
                CustomerName = SalesInvoice.CustomerName,
                StateID = (int)SalesInvoice.StateID,
                BillingAddressID = (int)SalesInvoice.BillingAddressID,
                ShippingAddressID = (int)SalesInvoice.ShippingAddressID,
                BillingAddress = SalesInvoice.BillingAddress,
                ShippingAddress = SalesInvoice.ShippingAddress,
                CashDiscount = (decimal)SalesInvoice.CashDiscount,
                CessAmount = (decimal)SalesInvoice.CessAmount,
                CashDiscountPercentage = (decimal)SalesInvoice.CashDiscountPercentage,
                OutstandingAmount = (decimal)SalesInvoice.OutstandingAmount,
                VehicleNo = SalesInvoice.VehicleNo,
                CustomerGSTNo = SalesInvoice.CustomerGSTNo,
                IsCancelled = SalesInvoice.IsCancelled,
                IsDraft = SalesInvoice.IsDraft,
                CurrencyCode = SalesInvoice.CurrencyCode,
                DONO = SalesInvoice.DONO,
                ReceiptDate = (DateTime)SalesInvoice.ReceiptDate,
                VatRegNo = SalesInvoice.VatRegNo,
                CustomerPODate = SalesInvoice.CustomerPODate,
                CustomerPONo = SalesInvoice.CustomerPONo,
                AmountInWords = SalesInvoice.AmountInWords,
                AadhaarNo = SalesInvoice.AadhaarNo,
                MinimumCurrency = SalesInvoice.MinimumCurrency,
                Remarks = SalesInvoice.Remarks,
                VATAmount=SalesInvoice.VATAmount,
                DecimalPlaces= SalesInvoice.DecimalPlaces,
                OtherCharges = SalesInvoice.OtherCharges,
                OtherChargesVATAmount = (decimal)SalesInvoice.OtherChargesVATAmount,
            };
            salesInvoices.Add(salesInvoice);
            var SalesInvoiceTrans = salesInvoiceBL.GetSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                packnumber = a.packnumber,
                dndate = a.dndate,
                DeliveryTerm = a.DeliveryTerm,
                PartsNumber = a.PartsNumber,
                ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                BatchID = (int)a.BatchID,
                BatchTypeID = (int)a.BatchTypeID,
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                OfferQty = (decimal)a.OfferQty,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                BatchName = a.BatchName,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                Code = a.Code,
                ItemName = a.Name,
                //  ItemName = a.PrintWithItemCode ? a.Name : a.PartsNumber,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                ExpiryDate = a.ExpiryDate,
                PackSize = (decimal)a.PackSize,
                BatchType = a.BatchTypeName,
                CurrencyName = a.CurrencyName,
                PurchaseOrderNo = a.PurchaseOrderNo,
                PurchaseOrderDate = a.PurchaseOrderDate,
                Remarks = a.Remarks,
                Make = a.Make,
                PrintWithItemCode = a.PrintWithItemCode,
                VATAmount=(decimal)a.VATAmount,
                DecimalPlaces=(int)a.DecimalPlaces


            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(salesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(salesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();


            string reportPath;
            //bool useItemName = SalesInvoiceTrans.Any(a => a.ItemName != a.PartsNumber);
            bool useItemName = SalesInvoiceTrans.Any(a => a.PrintWithItemCode);

            string FileName;
            if (useItemName)
            {
                reportPath = GetReportPath("SalesInvoiceExportPrintPdfwithItem");
            }
            else
            {
                reportPath = GetReportPath("SalesInvoiceExportPrintPdfwithPONo");
            }
            reportViewer.LocalReport.ReportPath = reportPath;
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam, ReportfooterPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoicePrintPdf2DataSet", salesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceTransPrintPdf2DataSet", SalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceAmountDetailsPrintPdfDataSet", SalesInvoiceAmountDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdf2DataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);




            if (useItemName)
            {
                FileName = "SalesInvoiceExportPrintPdfwithItem.pdf";
            }
            else
            {
                FileName = "SalesInvoiceExportPrintPdfwithPONo .pdf";
            }
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesInvoice/"), FileName);
            string URL = "/Outputs/SalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }







        [HttpPost]
        public JsonResult ServiceSalesInvoicePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ServiceTaxInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ServiceSalesInvoice = servicesalesInvoiceBL.GetServiceSalesInvoice(Id, GeneralBO.LocationID);
            List<SpGetServiceSalesInvoice_Result> servicesalesInvoices = new List<SpGetServiceSalesInvoice_Result>();
            SpGetServiceSalesInvoice_Result servicesalesInvoice = new SpGetServiceSalesInvoice_Result()
            {
                TransNo = ServiceSalesInvoice.InvoiceNo,
                InvoiceDate = (DateTime)ServiceSalesInvoice.InvoiceDate,
                CustomerID = (int)ServiceSalesInvoice.CustomerID,
                GrossAmt = (decimal)ServiceSalesInvoice.GrossAmount,
                DiscountAmt = (decimal)ServiceSalesInvoice.DiscountAmount,
                TaxableAmt = (decimal)ServiceSalesInvoice.TaxableAmount,
                SGSTAmt = (decimal)ServiceSalesInvoice.SGSTAmount,
                CGSTAmt = (decimal)ServiceSalesInvoice.CGSTAmount,
                IGSTAmt = (decimal)ServiceSalesInvoice.IGSTAmount,
                RoundOff = (decimal)ServiceSalesInvoice.RoundOff,
                NetAmt = (decimal)ServiceSalesInvoice.NetAmount,
                CustomerName = ServiceSalesInvoice.CustomerName,
                StateID = (int)ServiceSalesInvoice.StateID,
                BillingAddressID = (int)ServiceSalesInvoice.BillingAddressID,
                ShippingAddressID = (int)ServiceSalesInvoice.ShippingAddressID,
                BillingAddress = ServiceSalesInvoice.BillingAddress,
                ShippingAddress = ServiceSalesInvoice.ShippingAddress,
                CessAmount = (decimal)ServiceSalesInvoice.CessAmount,
                CustomerGSTNo = ServiceSalesInvoice.CustomerGSTNo
            };
            servicesalesInvoices.Add(servicesalesInvoice);
            var ServiceSalesInvoiceTrans = servicesalesInvoiceBL.GetServiceSalesInvoiceItems(Id, GeneralBO.LocationID).Select(a => new SpGetSalesInvoiceItems_Result()
            {
                ItemID = (int)a.ItemID,
                Quantity = (decimal)a.Qty,
                InvoiceQty = (decimal)a.InvoiceQty,
                MRP = (decimal)a.MRP,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TurnoverDiscount = (decimal)a.TurnoverDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                SGSTAmt = (decimal)a.SGST,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                NetAmt = (decimal)a.NetAmount,
                WareHouseID = (int)a.StoreID,
                ItemName = a.Name,
                UnitName = a.UnitName,
                Rate = (decimal)a.Rate,
                CashDiscount = (decimal)a.CashDiscount,
                CessAmount = (decimal)a.CessAmount,
                CessPercentage = (decimal)a.CessPercentage,
                VATAmount=(decimal)a.VATAmount,
            }).ToList();
            var SalesInvoiceAmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(Id, GeneralBO.LocationID).ToList();
            var BillingAddress = addressBL.GetAddresses(servicesalesInvoice.BillingAddressID).ToList();
            var ShippingAddress = addressBL.GetAddresses(servicesalesInvoice.ShippingAddressID).ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("SalesInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("ServiceSalesInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ServiceSalesInvoicePrintPdfDataSet", servicesalesInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ServiceSalesInvoiceTransPrintPdfDataSet", ServiceSalesInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdfDataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ServiceSalesInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ServiceSalesInvoice/"), FileName);
            string URL = "/Outputs/ServiceSalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult EmployeeFreeMedicine()
        //{
        //    EmployeeFreeMedicineModel rep = new EmployeeFreeMedicineModel();
        //    rep.FromDateString = GeneralBO.FinStartDate;
        //    rep.ToDateString = General.FormatDate(DateTime.Now);
        //    return View(rep);
        //}
        //[HttpPost]
        //public ActionResult EmployeeFreeMedicine(EmployeeFreeMedicineModel model)
        //{
        //    try
        //    {

        //        FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
        //        ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
        //        ReportNameParam = new ReportParameter("ReportName", "Sales Invoice " + model.ReportType + " Report.");
        //        FilterParam = new ReportParameter("Filters", model.Filters);
        //        var EmployeeFreeMedicine = dbEntity.SpRptGetEmployeeFreeMedicineDetails(
        //            model.EmployeeID,
        //            model.FromDate,
        //            model.ToDate
        //            ).ToList();
        //        reportViewer.LocalReport.ReportPath = GetReportPath("EmployeeFreeMedicine");
        //        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("EmployeeFreeMedicineDataSet", EmployeeFreeMedicine));
        //        ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
        //        reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
        //        ViewBag.ReportViewer = reportViewer;
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        //}

        //Added by Anish 23Oct2019
        //[HttpGet]
        //public ActionResult SalesReturnDetails()
        //{
        //    SalesReturnDetailsModel rep = new SalesReturnDetailsModel();
        //    rep.FromDateString = GeneralBO.FinStartDate;
        //    rep.ToDateString = General.FormatDate(DateTime.Now);
        //    return View(rep);
        //}
        //[HttpPost]
        //public ActionResult SalesReturnDetails(SalesReturnDetailsModel model)
        //{
        //    try
        //    {

        //        FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
        //        ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
        //        ReportNameParam = new ReportParameter("ReportName", "Sales Invoice " + model.ReportType + " Report.");
        //        FilterParam = new ReportParameter("Filters", model.Filters);
        //        var SalesReturnDetails = dbEntity.spRptGetSalesReturnDetails(
        //            model.CustomerID,
        //            model.FromDate,
        //            model.ToDate
        //            ).ToList();
        //        reportViewer.LocalReport.ReportPath = GetReportPath("SalesReturnDetails");
        //        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesReturnDetailsDataSet", SalesReturnDetails));
        //        ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
        //        reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
        //        ViewBag.ReportViewer = reportViewer;
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        //}


        [HttpGet]
        public ActionResult IncentiveReport()
        {
            List<CategoryBO> ItemCategoryList = categoryBL.GetItemCategoryForSales();
            IncentiveReportModel rep = new IncentiveReportModel();
            rep.UserID = GeneralBO.CreatedUserID;
            rep.DurationList = new SelectList(submasterBL.GetDurationList(), "ID", "Name");
            rep.TimePeriodList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Select", Value ="0", }
                                                 }, "Value", "Text");
            rep.PartyList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "FSO", Value ="FSO", },
                                                 new SelectListItem { Text = "Managers", Value ="Managers", },
                                                 new SelectListItem { Text = "Branches", Value ="Branches", },
                                                 }, "Value", "Text");
            return View(rep);
        }

        [HttpPost]
        public ActionResult IncentiveReport(IncentiveReportModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Incentive Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var Incentive = dbEntity.SpRptIncentiveReport(
                model.PartyType,
                model.DurationID,
                model.TimePeriodID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                ).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/IncentiveReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("IncentiveDataSet", Incentive));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { ReportNameParam, FilterParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, CompanyNameParam, ImagePathParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpPost]
        public JsonResult CounterSalesInvoicePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "CASH SALE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var countersalesinvoice = counterSalesBL.GetCounterSalesDetail(Id).Select(a => new SpGetCounterSalesDetail_Result()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                DiscountAmt = a.DiscountAmt,
                Transdate = (DateTime)a.TransDate,
                Warehouse = a.WarehouseName,
                PartyName = a.PartyName,
                MobileNumber = a.MobileNumber,
                CivilID = a.CivilID,
                PrintWithItemName = a.PrintWithItemCode,
                NetAmountTotal = (decimal)a.NetAmount,
                IsDraft = (bool)a.IsDraft,
                IsCancelled = (bool)a.IsCancelled,
                DoctorID = (int)a.DoctorID,
                DoctorName = a.DoctorName,
                PatientID = (int)a.PatientID,
                PackingPrice = (int)a.PackingPrice,
                RoundOff = (decimal)a.RoundOff,
                CGSTAmount = (decimal)a.CGSTAmount,
                SGSTAmount = (decimal)a.SGSTAmount,
                IGSTAmount = (decimal)a.IGSTAmount,
                PaymentModeID = (int)a.PaymentModeID,
                TotalAmountReceived = (decimal)a.TotalAmountReceived,
                BalanceToBePaid = (decimal)a.BalanceToBePaid,
                CessAmount = (decimal)a.CessAmount,
                EmployeeID = (int)a.EmployeeID,
                SalesTypeID = (int)a.TypeID,
                SalesType = a.Type,
                Employee = a.EmployeeName,
                PatientName = a.PatientName,
                BalAmount = (decimal)a.BalanceAmount,
                EmployeeCode = a.EmployeeCode,
                TaxableAmt = (decimal)a.TaxableAmt,
                GrossAmount = (decimal)a.GrossAmount,
                BankID = a.BankID,
                CashSalesName = a.CashSalesName,
                OutstandingAmount = (decimal)a.OutstandingAmount,
                Remarks = a.Remarks,
                currencyCode=a.currencyCode,
                AmountInWords=a.AmountInWords,
                MinimumCurrency=a.MinimumCurrency,
                TotalVATAmount=a.TotalVATAmount,
                DecimalPlaces=(int)a.DecimalPlaces
            }).ToList();
            var countersalesinvoiceTrans = counterSalesBL.GetCounterSalesListDetails(Id).Select(m => new SpGetCounterSalesTrans_Result()
            {
                CounterSalesID = (int)m.CounterSalesID,
                FullOrLoose = m.FullOrLoose,
                ItemID = (int)m.ItemID,
                ItemName = m.Name,
                BatchID = (int)m.BatchID,
                Quantity = (decimal)m.Quantity,
                Rate = (decimal)m.Rate,
                MRP = (decimal)m.MRP,
                SecondaryQty = m.SecondaryQty,
                SecondaryOfferQty = m.SecondaryOfferQty,
                SecondaryUnitSize = m.SecondaryUnitSize,
                SecondaryRate = m.SecondaryRate,
                SecondaryUnit = m.SecondaryUnit,
                GrossAmount = (decimal)m.GrossAmount,
                SGSTAmount = (decimal)m.SGSTAmount,
                CGSTAmount = (decimal)m.CGSTAmount,
                IGSTAmount = (decimal)m.IGSTAmount,
                NetAmount = (decimal)m.NetAmount,
                IGSTPercent = (decimal)m.IGSTPercentage,
                SGSTPercent = (decimal)m.SGSTPercentage,
                CGSTPercent = (decimal)m.CGSTPercentage,
                Code = m.Code,
                BatchNo = m.BatchNo,
                BatchTypeID = (int)m.BatchTypeID,
                WarehouseID = (int)m.WareHouseID,
                ExpiryDate = (DateTime)m.ExpiryDate,
                Unit = m.Unit,
                Stock = m.Stock,
                TaxableAmount = (decimal)m.TaxableAmount,
                UnitID = (int)m.UnitID,
                CessAmount = (decimal)m.CessAmount,
                CessPercentage = (decimal)m.CessPercentage,
                BasicPrice = (decimal)m.BasicPrice,
                HSNCode = m.HSNCode,
                MinSalesQty = (decimal)m.MinimumSalesQty,
                MaxSalesQty = (decimal)m.MaximumSalesQty,
                GSTPercent = m.GSTPercentage,
                Model=m.Model,
                Make=m.Make,
                VATAmount=m.VATAmount,
                DecimalPlaces=m.DecimalPlaces
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("CounterSalesInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesInvoicePrintPdfDataSet", countersalesinvoice));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesInvoiceTransPrintPdfDataSet", countersalesinvoiceTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "CounterSalesInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/CounterSalesInvoice/"), FileName);
            string URL = "/Outputs/CounterSalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult CounterSalesPartNo(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "CASH SALE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var countersalesinvoice = counterSalesBL.GetCounterSalesDetail(Id).Select(a => new SpGetCounterSalesDetail_Result()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                DiscountAmt = a.DiscountAmt,

                Transdate = (DateTime)a.TransDate,
                Warehouse = a.WarehouseName,
                PartyName = a.PartyName,
                MobileNumber = a.MobileNumber,
                CivilID = a.CivilID,
                PrintWithItemName = a.PrintWithItemCode,
                NetAmountTotal = (decimal)a.NetAmount,
                IsDraft = (bool)a.IsDraft,
                IsCancelled = (bool)a.IsCancelled,
                DoctorID = (int)a.DoctorID,
                DoctorName = a.DoctorName,
                PatientID = (int)a.PatientID,
                PackingPrice = (int)a.PackingPrice,
                RoundOff = (decimal)a.RoundOff,
                CGSTAmount = (decimal)a.CGSTAmount,
                SGSTAmount = (decimal)a.SGSTAmount,
                IGSTAmount = (decimal)a.IGSTAmount,
                PaymentModeID = (int)a.PaymentModeID,
                TotalAmountReceived = (decimal)a.TotalAmountReceived,
                BalanceToBePaid = (decimal)a.BalanceToBePaid,
                CessAmount = (decimal)a.CessAmount,
                EmployeeID = (int)a.EmployeeID,
                SalesTypeID = (int)a.TypeID,
                SalesType = a.Type,
                Employee = a.EmployeeName,
                PatientName = a.PatientName,
                BalAmount = (decimal)a.BalanceAmount,
                EmployeeCode = a.EmployeeCode,
                TaxableAmt = (decimal)a.TaxableAmt,
                GrossAmount = (decimal)a.GrossAmount,
                BankID = a.BankID,
                CashSalesName = a.CashSalesName,
                OutstandingAmount = (decimal)a.OutstandingAmount,
                Remarks = a.Remarks,
                currencyCode = a.currencyCode,
                AmountInWords = a.AmountInWords,
                MinimumCurrency = a.MinimumCurrency,
                TotalVATAmount=a.TotalVATAmount,
                DecimalPlaces=a.DecimalPlaces
            }).ToList();
            var countersalesinvoiceTrans = counterSalesBL.GetCounterSalesListDetails(Id).Select(m => new SpGetCounterSalesTrans_Result()
            {
                PartsNumber = m.PartsNumber,
                CounterSalesID = (int)m.CounterSalesID,
                FullOrLoose = m.FullOrLoose,
                ItemID = (int)m.ItemID,
                ItemName = m.Name,
                BatchID = (int)m.BatchID,
                Quantity = (decimal)m.Quantity,
                Rate = (decimal)m.Rate,
                MRP = (decimal)m.MRP,
                SecondaryQty = m.SecondaryQty,
                SecondaryOfferQty = m.SecondaryOfferQty,
                SecondaryUnitSize = m.SecondaryUnitSize,
                SecondaryRate = m.SecondaryRate,
                SecondaryUnit = m.SecondaryUnit,
                GrossAmount = (decimal)m.GrossAmount,
                SGSTAmount = (decimal)m.SGSTAmount,
                CGSTAmount = (decimal)m.CGSTAmount,
                IGSTAmount = (decimal)m.IGSTAmount,
                NetAmount = (decimal)m.NetAmount,
                IGSTPercent = (decimal)m.IGSTPercentage,
                SGSTPercent = (decimal)m.SGSTPercentage,
                CGSTPercent = (decimal)m.CGSTPercentage,
                Code = m.Code,
                BatchNo = m.BatchNo,
                BatchTypeID = (int)m.BatchTypeID,
                WarehouseID = (int)m.WareHouseID,
                ExpiryDate = (DateTime)m.ExpiryDate,
                Unit = m.Unit,
                Stock = m.Stock,
                TaxableAmount = (decimal)m.TaxableAmount,
                UnitID = (int)m.UnitID,
                CessAmount = (decimal)m.CessAmount,
                CessPercentage = (decimal)m.CessPercentage,
                BasicPrice = (decimal)m.BasicPrice,
                HSNCode = m.HSNCode,
                MinSalesQty = (decimal)m.MinimumSalesQty,
                MaxSalesQty = (decimal)m.MaximumSalesQty,
                GSTPercent = m.GSTPercentage,
                Make=m.Make,
                VATAmount=m.VATAmount,
                DecimalPlaces=(int)m.DecimalPlaces,
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("CounterSalesInvoicePartNoPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesInvoicePrintPdfDataSet", countersalesinvoice));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesInvoiceTransPrintPdfDataSet", countersalesinvoiceTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "CounterSalesInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/CounterSalesInvoice/"), FileName);
            string URL = "/Outputs/CounterSalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult CounterSalesInvoicePrintPdfV5(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SalesInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var countersalesinvoice = counterSalesBL.GetCounterSalesDetail(Id).Select(a => new SpGetCounterSalesDetail_Result()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Transdate = (DateTime)a.TransDate,
                Warehouse = a.WarehouseName,
                PartyName = a.PartyName,
                NetAmountTotal = (decimal)a.NetAmount,
                IsDraft = (bool)a.IsDraft,
                IsCancelled = (bool)a.IsCancelled,
                DoctorID = (int)a.DoctorID,
                DoctorName = a.DoctorName,
                PatientID = (int)a.PatientID,
                PackingPrice = (int)a.PackingPrice,
                RoundOff = (decimal)a.RoundOff,
                CGSTAmount = (decimal)a.CGSTAmount,
                SGSTAmount = (decimal)a.SGSTAmount,
                IGSTAmount = (decimal)a.IGSTAmount,
                PaymentModeID = (int)a.PaymentModeID,
                TotalAmountReceived = (decimal)a.TotalAmountReceived,
                BalanceToBePaid = (decimal)a.BalanceToBePaid,
                CessAmount = (decimal)a.CessAmount,
                EmployeeID = (int)a.EmployeeID,
                SalesTypeID = (int)a.TypeID,
                SalesType = a.Type,
                Employee = a.EmployeeName,
                PatientName = a.PatientName,
                BalAmount = (decimal)a.BalanceAmount,
                EmployeeCode = a.EmployeeCode,
                TaxableAmt = (decimal)a.TaxableAmt,
                GrossAmount = (decimal)a.GrossAmount,
                BankID = a.BankID,
                CashSalesName = a.CashSalesName,
                OutstandingAmount = (decimal)a.OutstandingAmount,
                DiscountCategory = a.DiscountCategory,
                DiscountAmt = (decimal)a.DiscountAmt,

            }).ToList();
            var countersalesinvoiceTrans = counterSalesBL.GetCounterSalesListDetails(Id).Select(m => new SpGetCounterSalesTrans_Result()
            {
                CounterSalesID = (int)m.CounterSalesID,
                FullOrLoose = m.FullOrLoose,
                ItemID = (int)m.ItemID,
                ItemName = m.Name,
                BatchID = (int)m.BatchID,
                Quantity = (decimal)m.Quantity,
                Rate = (decimal)m.Rate,
                MRP = (decimal)m.MRP,
                GrossAmount = (decimal)m.GrossAmount,
                SGSTAmount = (decimal)m.SGSTAmount,
                CGSTAmount = (decimal)m.CGSTAmount,
                IGSTAmount = (decimal)m.IGSTAmount,
                NetAmount = (decimal)m.NetAmount,
                IGSTPercent = (decimal)m.IGSTPercentage,
                SGSTPercent = (decimal)m.SGSTPercentage,
                CGSTPercent = (decimal)m.CGSTPercentage,
                Code = m.Code,
                BatchNo = m.BatchNo,
                BatchTypeID = (int)m.BatchTypeID,
                WarehouseID = (int)m.WareHouseID,
                ExpiryDate = (DateTime)m.ExpiryDate,
                Unit = m.Unit,
                Stock = m.Stock,
                TaxableAmount = (decimal)m.TaxableAmount,
                UnitID = (int)m.UnitID,
                CessAmount = (decimal)m.CessAmount,
                CessPercentage = (decimal)m.CessPercentage,
                BasicPrice = (decimal)m.BasicPrice,
                HSNCode = m.HSNCode,
                MinSalesQty = (decimal)m.MinimumSalesQty,
                MaxSalesQty = (decimal)m.MaximumSalesQty,
                GSTPercent = m.GSTPercentage,
                TotalMRP = m.TotalMRP
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("CounterSalesInvoicePrintPdfV5");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesInvoicePrintPdfDataSet", countersalesinvoice));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesInvoiceTransPrintPdfDataSet", countersalesinvoiceTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "CounterSalesInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/CounterSalesInvoice/"), FileName);
            string URL = "/Outputs/CounterSalesInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ProfitabilityReport()
        {
            CounterSaleReportModel model = new CounterSaleReportModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult ProfitabilityReport(CounterSaleReportModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Profitability Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var Production = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/ProfitabilityReport.rdlc";
            string XMLParams = XMLHelper.ParseXML(model);
            var Profit = dbEntity.SpRptGetProfitOfSales(
                                    XMLParams
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProfitabilityReportDataSet", Profit));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        [HttpGet]
        public ActionResult FastMovingItemsReport()
        {
            CounterSaleReportModel model = new CounterSaleReportModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.FinYear = GeneralBO.FinYear;
            return View(model);
        }
        [HttpPost]
        public ActionResult FastMovingItemsReport(CounterSaleReportModel model)
        {

            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var Production = new object();
            if (model.Type == "NonItem")
            {
                ReportNameParam = new ReportParameter("ReportName", "Non Moving Items Report");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/NonMovingItemsReport.rdlc";
                string XMLParams = XMLHelper.ParseXML(model);
                var Items = dbEntity.SpRptGetNonMovableItems(
                                        XMLParams
                                        ).ToList();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("NonMovingItemsReportDataSet", Items));
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
                ViewBag.ReportViewer = reportViewer;
            }
            if (model.Type == "InventoryLevel")
            {
                ReportNameParam = new ReportParameter("ReportName", "Inventory Level Report");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/ArrivingInventoryLevel.rdlc";
                string XMLParams = XMLHelper.ParseXML(model);
                var Items = dbEntity.SpRptArrivingInventoryLevels(
                                        XMLParams
                                        ).ToList();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("InventoryLevelDataSet", Items));
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
                ViewBag.ReportViewer = reportViewer;
            }
            if (model.Mode == "Detail" && model.Type != "NonItem" && model.Type != "InventoryLevel")
            {
                ReportNameParam = new ReportParameter("ReportName", "Fast Moving Items Month Wise");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/FastMovingItemsMonthlyWise.rdlc";
                string XMLParams = XMLHelper.ParseXML(model);
                var Items = dbEntity.SpRptFastMovingItemsMonthlyWise(
                                        XMLParams
                                        ).ToList();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FastMovingItemsMonthlyDataSet", Items));
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
                ViewBag.ReportViewer = reportViewer;
            }
            if (model.Type == "Value" || model.Mode == "Quantity" && model.Type != "NonItem" && model.Type != "InventoryLevel")
            {
                ReportNameParam = new ReportParameter("ReportName", "Fast Moving Items Report");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/FastMovingItemsReport.rdlc";
                string XMLParams = XMLHelper.ParseXML(model);
                var Items = dbEntity.SpRptGetFastMovingItems(
                                        XMLParams
                                        ).ToList();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FastMovingItemsReportDataSet", Items));
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
                ViewBag.ReportViewer = reportViewer;
            }
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        [HttpGet]
        public ActionResult EmployeeDailyReport()
        {
            CounterSaleReportModel model = new CounterSaleReportModel();
            //model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult EmployeeDailyReport(CounterSaleReportModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Employee Daily Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", "01-Apr-2020");
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var Production = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/EmployeeDailyReport.rdlc";
            string XMLParams = XMLHelper.ParseXML(model);
            var Employee = dbEntity.SpRptGetEmployeeDailySalesDetails(
                                    XMLParams
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("EmployeeDailyReportDataSet", Employee));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public JsonResult SignOutPrintPdf(string Type)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.EmployeeDailyReport);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SignOut = counterSalesBL.GetCounterSalesSignOutPrint(Type).Select(a => new SpGetCounterSalesSignOutPrint_Result()
            {
                //StartingBillNo = a.StartingBillNo,
                //EndingBillNo = a.EndingBillNo,
                //Count = a.Count,
                //NetAmount = a.NetAmount,
                //TotalCash = a.TotalCash,
                //Card = a.CashOnCard,
                //Employee = a.EmployeeName,
                Date = a.TransDate
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("SignOutSalesPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SignOutCounterSalesPrintDataSet", SignOut));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "SignOutSalesPrint.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SignOutSalesPrint/"), FileName);
            string URL = "/Outputs/SignOutSalesPrint/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult SalesGSTRReports()
        {
            GSTRModel rep = new GSTRModel();
            rep.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            rep.ToDateString = General.FormatDate(DateTime.Now);
            //rep.FilterLocationID = GeneralBO.LocationID;
            rep.LocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.GSTPercentageList = new SelectList(
                                     igstBL.GetGstList(), "IGSTPercentage", "IGSTPercentage");
            rep.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            return View(rep);
        }
        [HttpPost]
        public ActionResult SalesGSTRReports(GSTRModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Sales GSTR" + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            model.LocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;

            string XMLParams = XMLHelper.ParseXML(model);

            if (model.ReportType == "HSN")
            {
                var HSN = dbEntity.SpRptSalesGSTRHSN(
                                    XMLParams).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesGSTRHSN");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesGSTRHSNDataSet", HSN));
            }
            else if (model.ReportType == "SalesReturn")
            {
                var SalesReturn = dbEntity.SpRptSalesReturnGSTR(
                                    XMLParams).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("SalesGSTRReturn");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesGSTRReturnDataSet", SalesReturn));
            }
            else if (model.ReportType == "OutputGSTTaxPercentagewise")
            {
                var GSTOutputSummary = dbEntity.SpRptGSTOutputSummary(
                                XMLParams).ToList();
                ReportNameParam = new ReportParameter("ReportName", "GST Output Summary Report");
                reportViewer.LocalReport.ReportPath = GetReportPath("GSTOutputSummary");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GSTOutputSummaryDataSet", GSTOutputSummary));
            }
            else
            {
                var TOD = dbEntity.SpRptSalesGSTR(
                                    XMLParams).ToList();
                if (model.ReportType == "B2B")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesGSTRB2B");
                }
                else if (model.ReportType == "B2C")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesGSTRB2C");
                }
                else if (model.ReportType == "Export")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("SalesGSTRExport");
                }
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesGSTRB2BExportB2CDataSet", TOD));
            }

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }

        [HttpGet]
        public ActionResult GeneralReports()
        {
            GeneralReportsModel model = new GeneralReportsModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult GeneralReports(GeneralReportsModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            FilterParam = new ReportParameter("Filters", "General Reports");
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            model.LocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            string XMLParams = XMLHelper.ParseXML(model);

            if (model.ReportType == "DailyReport")
            {
                ReportNameParam = new ReportParameter("ReportName", "Daily Report");
                var DailyReport = dbEntity.SpRptDailyReport(
                        XMLParams).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/DailyReport.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DailyReportDataSet", DailyReport));
            }
            else if (model.ReportType == "Income&Expenses")
            {
                var IncomeandExpense = dbEntity.SpRptIncomeandExpense(
                        XMLParams).ToList();
                if (model.ItemType == "Income")
                {
                    ReportNameParam = new ReportParameter("ReportName", "Income");
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/IncomeReport.rdlc";
                }
                if (model.ItemType == "Expense")
                {
                    ReportNameParam = new ReportParameter("ReportName", "Expense");
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/ExpenseReport.rdlc";
                }
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("IncomeandExpenseDataSet", IncomeandExpense));
            }
            else if (model.ReportType == "PatientList")
            {
                ReportNameParam = new ReportParameter("ReportName", "Patient List");
                var PatientList = dbEntity.SpRptPatientList(
                        XMLParams).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/PatientList.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PatientListDataSet", PatientList));
            }
            else if (model.ReportType == "PrivilegeCardDetail")
            {
                ReportNameParam = new ReportParameter("ReportName", "Privilege Card Detail");
                var PrivilegeCard = dbEntity.SpRptPrivillegeCardSaleDetails(
                        XMLParams).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/PrivilegeCardSale.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PrivilegeCardSaleDataSet", PrivilegeCard));
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "PayMent For Doctors");
                var PaymentForDoctors = dbEntity.SpRptPaymentForDoctors(
                        XMLParams).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/PaymentForDoctors.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PaymentForDoctorsDataSet", PaymentForDoctors));
            }

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }

        //        [HttpPost]
        //        public JsonResult GoodsReceiptNotePrintPdf(int Id)
        //        {
        //            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
        //            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
        //            ReportNameParam = new ReportParameter("ReportName", "DELIVERY NOTE");
        //            Warning[] warnings;
        //            string[] streamIds;
        //            string contentType;
        //            string mimeType = string.Empty;
        //            string encoding = string.Empty;
        //            string extension = string.Empty;
        //            //var IdString = Id.ToString;
        //            string idString = Id.ToString();

        //            //string idString = Id.ToString();
        //            var Goodreceiptitemsdetails = SalesGoodsReceiptNoteBL.GetGoodReceiptNotesItems(idString).Select(a => new SpGetGoodsReceiptCounterSalesItems_Result ()
        //            {
        //                ItemCode = a.Itemcode,
        //                ItemName = a.ItemName,
        //                Remark = a.Remarks,
        //                Quantity = a.Quantity,
        //                PartsNumber = a.PartsNumber,
        //                Make = a.Make
        //            }).ToList();

        //var GoodsReceipt = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes1(idString).Select(a => new SpGetGoodsReceiptSalesOrderItems_Result()
        //            {
        //                Qty = (int)a.Qty,
        //                Unit = a.Unit
        //            }).ToList();

        //            if (Goodreceiptitemsdetails.PrintWithItemCode == true)
        //            {
        //                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNote");
        //            }
        //            else if (!Goodreceiptitemsdetails.PrintWithItemCode == true)
        //            {
        //                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePOPrintPdf");
        //            }
        //            else
        //                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePrintPdf");
        //            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
        //            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
        //            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptCounterSalesItemsDataSet", GoodReceiptNot));
        //            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptDetailDataSet", GoodReceiptNot2));
        //            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptItemDetail1DataSet", GoodReceiptNot3));
        //            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptSalesOrderItems4DataSet", GoodsReceipt));

        //            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
        //            string FileName = "GoodsReceiptNotePrintPdf.pdf";
        //            string FilePath = Path.Combine(Server.MapPath("~/Outputs/GoodsReceiptNote/"), FileName);
        //            string URL = "/Outputs/GoodsReceiptNote/" + FileName;
        //            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
        //            {
        //                fs.Write(bytes, 0, bytes.Length);
        //            }
        //            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        //        }


        [HttpPost]
        public JsonResult GoodsReceiptNotePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "DELIVERY NOTE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            //var IdString = Id.ToString;
            string idString = Id.ToString();
            var GoodReceiptNote = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes(idString);
            var GoodReceiptNot = SalesGoodsReceiptNoteBL.GetGoodReceiptNotesItems(idString).Select(a => new SpGetGoodsReceiptCounterSalesItems_Result()
            {
                ItemCode = a.Itemcode,
                ItemName = a.ItemName,
                Remark = a.Remarks,
                Quantity = a.Quantity,
                PartsNumber = a.PartsNumber,
                Make = a.Make,
                Unit = a.Unit,
                Location = a.Location
                //FullOrLoose=a.FullOrLoose

            }).ToList();

            // GoodReceiptNot.Add(GoodReceiptNotes);
            var GoodReceiptNote1 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            List<SpGetGoodsReceiptDetail_Result> GoodReceiptNot2 = new List<SpGetGoodsReceiptDetail_Result>();
            SpGetGoodsReceiptDetail_Result GoodReceiptNotes3 = new SpGetGoodsReceiptDetail_Result()
            {
                CustomerName = GoodReceiptNote1.CustomerName,
                TransNo = GoodReceiptNote1.TransNo,
                ReceiptDate = GoodReceiptNote1.TransDate,
                Remarks = GoodReceiptNote1.Remarks,
                // SONO= GoodReceiptNote1.SONO



            };
            GoodReceiptNot2.Add(GoodReceiptNotes3);
            //var GoodReceiptNote2 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            //List<SpGetGoodsReceiptItemDetail_Result> GoodReceiptNot3 = new List<SpGetGoodsReceiptItemDetail_Result>();
            //SpGetGoodsReceiptItemDetail_Result GoodReceiptNotes4 = new SpGetGoodsReceiptItemDetail_Result()
            //{
            //    SalesOrderNo = GoodReceiptNote2.SalesOrderNos,

            //     SONO= GoodReceiptNote2.SONO,
            //   // GoodReceiptNote2.TransNo

            //    //Name = GoodReceiptNote.Name,
            //};



            //GoodReceiptNot3.Add(GoodReceiptNotes4);
            var GoodReceiptNot3 = SalesGoodsReceiptNoteBL.GetGoodReceiptNoteItem(Id).Select(a => new SpGetGoodsReceiptItemDetail_Result()
            {
                SONO = a.SONO,
                SalesOrderNo = a.SalesInvoiceNo,

                //FullOrLoose=a.FullOrLoose

            }).ToList();


            //string idString = Id.ToString();
            var GoodsReceipt = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes1(idString).Select(a => new SpGetGoodsReceiptSalesOrderItems_Result()
            {
                Qty = (int)a.Qty,
                Unit = a.Unit,
                OrderDate = a.OrderDate


            }).ToList();

            if (GoodReceiptNote.PrintWithItemCode == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNote");
            }
            else if (!GoodReceiptNote.PrintWithItemCode == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePOPrintPdf");
            }
            else
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptCounterSalesItemsDataSet", GoodReceiptNot));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptDetailDataSet", GoodReceiptNot2));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptItemDetail1DataSet", GoodReceiptNot3));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptSalesOrderItems4DataSet", GoodsReceipt));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "GoodsReceiptNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/GoodsReceiptNote/"), FileName);
            string URL = "/Outputs/GoodsReceiptNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult GoodsReceiptNoteDetail(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "DELIVERY NOTE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            //var IdString = Id.ToString;
            string idString = Id.ToString();
            var GoodReceiptNote = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes(idString);
            var GoodReceiptNot = SalesGoodsReceiptNoteBL.GetGoodReceiptNotesItems(idString).Select(a => new SpGetGoodsReceiptCounterSalesItems_Result()
            {
                ItemCode = a.Itemcode,
                ItemName = a.ItemName,
                Remark = a.Remarks,
                Quantity = a.Quantity,
                PartsNumber = a.PartsNumber,
                Make = a.Make,
                Unit = a.Unit,
                Location = a.Location
                //FullOrLoose = a.FullOrLoose
            }).ToList();
            // GoodReceiptNot.Add(GoodReceiptNotes);
            var GoodReceiptNote1 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            List<SpGetGoodsReceiptDetail_Result> GoodReceiptNot2 = new List<SpGetGoodsReceiptDetail_Result>();
            SpGetGoodsReceiptDetail_Result GoodReceiptNotes3 = new SpGetGoodsReceiptDetail_Result()
            {
                CustomerName = GoodReceiptNote1.CustomerName,
                TransNo = GoodReceiptNote1.TransNo,
                ReceiptDate = GoodReceiptNote1.TransDate,
                Remarks = GoodReceiptNote1.Remarks


            };
            //GoodReceiptNot2.Add(GoodReceiptNotes3);
            //var GoodReceiptNote2 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            //List<SpGetGoodsReceiptItemDetail_Result> GoodReceiptNot3 = new List<SpGetGoodsReceiptItemDetail_Result>();
            //SpGetGoodsReceiptItemDetail_Result GoodReceiptNotes4 = new SpGetGoodsReceiptItemDetail_Result()
            //{
            //    SalesOrderNo = GoodReceiptNote2.SalesOrderNos,
            //    //Name = GoodReceiptNote.Name,
            //    SONO = GoodReceiptNote2.SONO,

            //};
            //GoodReceiptNot3.Add(GoodReceiptNotes4);
            var GoodReceiptNot3 = SalesGoodsReceiptNoteBL.GetGoodReceiptNoteItem(Id).Select(a => new SpGetGoodsReceiptItemDetail_Result()
            {
                SONO = a.SONO,
                SalesOrderNo = a.SalesInvoiceNo,

                //FullOrLoose=a.FullOrLoose

            }).ToList();

            //string idString = Id.ToString();
            var GoodsReceipt = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes1(idString).Select(a => new SpGetGoodsReceiptSalesOrderItems_Result()
            {
                Qty = (int)a.Qty,
                Unit = a.Unit,
                OrderDate = a.OrderDate
            }).ToList();

            if (GoodReceiptNote.PrintWithItemCode == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNote");
            }
            else if (!GoodReceiptNote.PrintWithItemCode == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePOPrintPdf");
            }
            else
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptCounterSalesItemsDataSet", GoodReceiptNot));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptDetailDataSet", GoodReceiptNot2));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptItemDetail1DataSet", GoodReceiptNot3));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptSalesOrderItems4DataSet", GoodsReceipt));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "GoodsReceiptNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/GoodsReceiptNote/"), FileName);
            string URL = "/Outputs/GoodsReceiptNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GoodsReceiptNoteitemcode(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "DELIVERY NOTE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            //var IdString = Id.ToString;
            string idString = Id.ToString();
            var GoodReceiptNote = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes(idString);

            var GoodReceiptNot = SalesGoodsReceiptNoteBL.GetGoodReceiptNotesItems(idString).Select(a => new SpGetGoodsReceiptCounterSalesItems_Result()
            {
                ItemCode = a.Itemcode,
                ItemName = a.ItemName,
                Remark = a.Remarks,
                Quantity = a.Quantity,
                PartsNumber = a.PartsNumber,
                Make = a.Make,
                Unit = a.Unit,
                Location = a.Location
                //FullOrLoose = a.FullOrLoose
            }).ToList();
            var GoodReceiptNote1 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            List<SpGetGoodsReceiptDetail_Result> GoodReceiptNot2 = new List<SpGetGoodsReceiptDetail_Result>();
            SpGetGoodsReceiptDetail_Result GoodReceiptNotes3 = new SpGetGoodsReceiptDetail_Result()
            {
                CustomerName = GoodReceiptNote1.CustomerName,
                TransNo = GoodReceiptNote1.TransNo,
                ReceiptDate = GoodReceiptNote1.TransDate,
                Remarks = GoodReceiptNote1.Remarks


            };
            //GoodReceiptNot2.Add(GoodReceiptNotes3);
            //var GoodReceiptNote2 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            //List<SpGetGoodsReceiptItemDetail_Result> GoodReceiptNot3 = new List<SpGetGoodsReceiptItemDetail_Result>();
            //SpGetGoodsReceiptItemDetail_Result GoodReceiptNotes4 = new SpGetGoodsReceiptItemDetail_Result()
            //{
            //    SalesOrderNo = GoodReceiptNote2.SalesOrderNos,
            //    //Name = GoodReceiptNote.Name,
            //    SONO = GoodReceiptNote2.SONO,

            //};
            //GoodReceiptNot3.Add(GoodReceiptNotes4);
            var GoodReceiptNot3 = SalesGoodsReceiptNoteBL.GetGoodReceiptNoteItem(Id).Select(a => new SpGetGoodsReceiptItemDetail_Result()
            {
                SONO = a.SONO,
                SalesOrderNo = a.SalesInvoiceNo,

                //FullOrLoose=a.FullOrLoose

            }).ToList();

            //string idString = Id.ToString();
            var GoodsReceipt = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes1(idString).Select(a => new SpGetGoodsReceiptSalesOrderItems_Result()
            {
                Qty = (int)a.Qty,
                Unit = a.Unit,
                OrderDate = a.OrderDate
            }).ToList();

            if (GoodReceiptNote.PrintWithItemCode == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNote");
            }
            //else if (!GoodReceiptNote.PrintWithItemCode == true)
            //{
            //    reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePOPrintPdf ");
            //}
            //else
            reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNote");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptCounterSalesItemsDataSet", GoodReceiptNot));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptDetailDataSet", GoodReceiptNot2));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptItemDetail1DataSet", GoodReceiptNot3));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptSalesOrderItems4DataSet", GoodsReceipt));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "GoodsReceiptNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/GoodsReceiptNote/"), FileName);
            string URL = "/Outputs/GoodsReceiptNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GoodsReceiptNoteExportPrintPdf(int Id)
        {
            //FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            //ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", "DELIVERY NOTE");
            //Warning[] warnings;
            //string[] streamIds;
            //string contentType;
            //string mimeType = string.Empty;
            //string encoding = string.Empty;
            //string extension = string.Empty;
            ////var IdString = Id.ToString;
            //string idString = Id.ToString();
            //var GoodReceiptNote = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes(idString);
            //var GoodReceiptNot = SalesGoodsReceiptNoteBL.GetGoodReceiptNotesItems(idString).Select(a => new SpGetGoodsReceiptCounterSalesItems_Result()
            //{
            //    ItemCode = a.Itemcode,
            //    ItemName = a.ItemName,
            //    Remark = a.Remarks,
            //    Quantity = a.Quantity,
            //    PartsNumber = a.PartsNumber,
            //    Make = a.Make,
            //    Unit = a.Unit,
            //    Location = a.Location
            //    // FullOrLoose = a.FullOrLoose
            //}).ToList();
            ////List<SpGetGoodsReceiptCounterSalesItems_Result> GoodReceiptNot = new List<SpGetGoodsReceiptCounterSalesItems_Result>();
            ////SpGetGoodsReceiptCounterSalesItems_Result GoodReceiptNotes = new SpGetGoodsReceiptCounterSalesItems_Result()
            ////{
            ////    ItemCode = GoodReceiptNote.Itemcode,
            ////    ItemName = GoodReceiptNote.ItemName,
            ////    Remark = GoodReceiptNote.Remarks,
            ////    Quantity = GoodReceiptNote.Quantity,
            ////    PartsNumber = GoodReceiptNote.PartsNumber,

            ////};
            ////GoodReceiptNot.Add(GoodReceiptNotes);
            //var GoodReceiptNote1 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            //List<SpGetGoodsReceiptDetail_Result> GoodReceiptNot2 = new List<SpGetGoodsReceiptDetail_Result>();
            //SpGetGoodsReceiptDetail_Result GoodReceiptNotes3 = new SpGetGoodsReceiptDetail_Result()
            //{
            //    CustomerName = GoodReceiptNote1.CustomerName,
            //    TransNo = GoodReceiptNote1.TransNo,
            //    ReceiptDate = GoodReceiptNote1.TransDate,
            //    Remarks = GoodReceiptNote1.Remarks


            //};
            //GoodReceiptNot2.Add(GoodReceiptNotes3);
            ////var GoodReceiptNote2 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            ////List<SpGetGoodsReceiptItemDetail_Result> GoodReceiptNot3 = new List<SpGetGoodsReceiptItemDetail_Result>();
            ////SpGetGoodsReceiptItemDetail_Result GoodReceiptNotes4 = new SpGetGoodsReceiptItemDetail_Result()
            ////{
            ////    SalesOrderNo = GoodReceiptNote2.SalesOrderNos,
            ////    //Name = GoodReceiptNote.Name,
            ////    SONO = GoodReceiptNote2.SONO,

            ////};
            ////GoodReceiptNot3.Add(GoodReceiptNotes4);
            //var GoodReceiptNot3 = SalesGoodsReceiptNoteBL.GetGoodReceiptNoteItem(Id).Select(a => new SpGetGoodsReceiptItemDetail_Result()
            //{
            //    SONO = a.SONO,
            //    SalesOrderNo = a.SalesInvoiceNo,

            //    //FullOrLoose=a.FullOrLoose

            //}).ToList();

            ////string idString = Id.ToString();
            //var GoodsReceipt = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes1(idString).Select(a => new SpGetGoodsReceiptSalesOrderItems_Result()
            //{
            //    Qty = (int)a.Qty,
            //    Unit = a.Unit,
            //    OrderDate = a.OrderDate
            //}).ToList();

            //if (GoodReceiptNote.PrintWithItemCode == true)
            //{
            //    reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNoteExportPrint");
            //}
            //else if (!GoodReceiptNote.PrintWithItemCode == true)
            //{
            //    reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNoteExportPo");
            //}
            //else
            //    reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePrintPdf");
            //ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            //ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            //ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam, ReportfooterPathParam });
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptCounterSalesItemsDataSet", GoodReceiptNot));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptDetailDataSet", GoodReceiptNot2));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptItemDetail1DataSet", GoodReceiptNot3));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptSalesOrderItems4DataSet", GoodsReceipt));

            //byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "GoodsReceiptNotePrintPdf.pdf";
            //string FilePath = Path.Combine(Server.MapPath("~/Outputs/GoodsReceiptNote/"), FileName);
            string URL = "/Outputs/GoodsReceiptNote/" + FileName;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult GoodsReceiptNoteExportDetail(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "DELIVERY NOTE");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            //var IdString = Id.ToString;
            string idString = Id.ToString();
            var GoodReceiptNote = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes(idString);
            //List<SpGetGoodsReceiptCounterSalesItems_Result> GoodReceiptNot = new List<SpGetGoodsReceiptCounterSalesItems_Result>();
            //SpGetGoodsReceiptCounterSalesItems_Result GoodReceiptNotes = new SpGetGoodsReceiptCounterSalesItems_Result()
            //{
            //    ItemCode = GoodReceiptNote.Itemcode,
            //    ItemName = GoodReceiptNote.ItemName,
            //    Remark = GoodReceiptNote.Remarks,
            //    Quantity = GoodReceiptNote.Quantity,
            //    PartsNumber = GoodReceiptNote.PartsNumber,

            //};
            //GoodReceiptNot.Add(GoodReceiptNotes);
            var GoodReceiptNot = SalesGoodsReceiptNoteBL.GetGoodReceiptNotesItems(idString).Select(a => new SpGetGoodsReceiptCounterSalesItems_Result()
            {
                ItemCode = a.Itemcode,
                ItemName = a.ItemName,
                Remark = a.Remarks,
                Quantity = a.Quantity,
                PartsNumber = a.PartsNumber,
                Make = a.Make,
                Unit = a.Unit,
                Location = a.Location
                // FullOrLoose = a.FullOrLoose
            }).ToList();
            var GoodReceiptNote1 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            List<SpGetGoodsReceiptDetail_Result> GoodReceiptNot2 = new List<SpGetGoodsReceiptDetail_Result>();
            SpGetGoodsReceiptDetail_Result GoodReceiptNotes3 = new SpGetGoodsReceiptDetail_Result()
            {
                CustomerName = GoodReceiptNote1.CustomerName,
                TransNo = GoodReceiptNote1.TransNo,
                ReceiptDate = GoodReceiptNote1.TransDate,
                Remarks = GoodReceiptNote1.Remarks


            };
            //GoodReceiptNot2.Add(GoodReceiptNotes3);
            //var GoodReceiptNote2 = SalesGoodsReceiptNoteBL.GetGoodReceiptNote(Id);
            //List<SpGetGoodsReceiptItemDetail_Result> GoodReceiptNot3 = new List<SpGetGoodsReceiptItemDetail_Result>();
            //SpGetGoodsReceiptItemDetail_Result GoodReceiptNotes4 = new SpGetGoodsReceiptItemDetail_Result()
            //{
            //    SalesOrderNo = GoodReceiptNote2.SalesOrderNos,
            //    //Name = GoodReceiptNote.Name,
            //    SONO = GoodReceiptNote2.SONO,

            //};
            //GoodReceiptNot3.Add(GoodReceiptNotes4);
            //string idString = Id.ToString();
            var GoodReceiptNot3 = SalesGoodsReceiptNoteBL.GetGoodReceiptNoteItem(Id).Select(a => new SpGetGoodsReceiptItemDetail_Result()
            {
                SONO = a.SONO,
                SalesOrderNo = a.SalesInvoiceNo,

                //FullOrLoose=a.FullOrLoose

            }).ToList();

            var GoodsReceipt = SalesGoodsReceiptNoteBL.GetGoodReceiptNotes1(idString).Select(a => new SpGetGoodsReceiptSalesOrderItems_Result()
            {
                Qty = (int)a.Qty,
                Unit = a.Unit,
                OrderDate = a.OrderDate
            }).ToList();

            if (GoodReceiptNote.PrintWithItemCode == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNoteExportPrint ");
            }
            else if (!GoodReceiptNote.PrintWithItemCode == true)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNoteExportPo");
            }
            else
                reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptCounterSalesItemsDataSet", GoodReceiptNot));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptDetailDataSet", GoodReceiptNot2));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptItemDetail1DataSet", GoodReceiptNot3));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptSalesOrderItems4DataSet", GoodsReceipt));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "GoodsReceiptNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/GoodsReceiptNote/"), FileName);
            string URL = "/Outputs/GoodsReceiptNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult CounterSalesReturnPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "CASH SALE RETURN");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var countersalesreturn = countersalesReturnBL.GetCounterSalesReturn(Id).Select(a => new SpGetCounterSalesReturnDetail_Result()
            {
                ID = a.ID,
                TransNo = a.ReturnNo,
                TransDate = (DateTime)a.ReturnDate,
                NetAmount = (decimal)a.NetAmount,
                IsDraft = (bool)a.IsDraft,
                SGSTAmount = (decimal)a.SGSTAmount,
                CGSTAmount = (decimal)a.CGSTAmount,
                IGSTAmount = (decimal)a.IGSTAmount,
                RoundOff = (decimal)a.RoundOff,
                BankID = a.BankID,
                BankName = a.BankName,
                PaymentModeID = (int)a.PaymentModeID,
                PaymentMode = a.PaymentMode,
                InvoiceID = (int)a.InvoiceID,
                PartyName = a.PartyName,
                InvoiceNo = a.InvoiceNo,
                Reason = a.Reason,
                BillDiscount = (decimal)a.BillDiscount,
                AmountInWords = a.AmountInWords,
                DecimalPlaces = a.DecimalPlaces,
                currencyCode = a.currencyCode,
                VATAmount = (decimal)a.VATAmount,
            }).ToList();
            var countersalesreturnTrans = countersalesReturnBL.GetCounterSalesReturnTrans(Id).Select(m => new SpGetCounterSalesReturnTrans_Result()
            {
                FullOrLoose = m.FullOrLoose,
                ItemID = (int)m.ItemID,
                ItemName = m.Name,
                BatchID = (int)m.BatchID,
                Quantity = (decimal)m.Quantity,
                Rate = (decimal)m.Rate,
                MRP = (decimal)m.MRP,
                GrossAmount = (decimal)m.GrossAmount,
                SGSTAmount = (decimal)m.SGSTAmount,
                CGSTAmount = (decimal)m.CGSTAmount,
                IGSTAmount = (decimal)m.IGSTAmount,
                NetAmount = (decimal)m.NetAmount,
                IGSTPercent = (decimal)m.IGSTPercentage,
                SGSTPercent = (decimal)m.SGSTPercentage,
                CGSTPercent = (decimal)m.CGSTPercentage,
                Code = m.Code,
                BatchNo = m.BatchNo,
                BatchTypeID = (int)m.BatchTypeID,
                WarehouseID = (int)m.WareHouseID,
                ExpiryDate = (DateTime)m.ExpiryDate,
                Unit = m.Unit,
                TaxableAmount = (decimal)m.TaxableAmount,
                ReturnQty = (decimal)m.ReturnQty,
                CounterSalesTransID = (int)m.CounterSalesTransID,
                UnitID = (int)m.UnitID,
                SalesUnitID = (int)m.SalesUnitID,
                PrimaryUnitID = (int)m.PrimaryUnitID,
                LooseRate = (decimal)m.LoosePrice,
                FullRate = (decimal)m.FullPrice,
                ConvertedQuantity = (decimal)m.ConvertedQuantity,
                CounterSalesQuantity = (decimal)m.CounterSalesQty,
                CounterSalesTransUnitID = (int)m.CounterSalesTransUnitID,
                SalesUnit = m.SalesUnitName,
                PrimaryUnit = m.PrimaryUnit,
                CessPercentage = m.CessPercentage,
                CessAmount = m.CessAmount,
                SecondaryRate = m.SecondaryRate,
                SecondaryUnitSize = m.SecondaryUnitSize,
                SecondaryReturnQty = m.SecondaryReturnQty,
                SecondaryUnit = m.SecondaryUnit,
                VATPercentage = (decimal)m.VATPercentage,
                VATAmount = (decimal)m.VATAmount,
                DiscountPercentage = (decimal)m.DiscountPercentage,
                DiscountAmount = (decimal)m.DiscountAmount,
                Make =m.Make,
                DecimalPlaces=(int)m.DecimalPlaces
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("CounterSalesReturnPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesReturnPrintPdfDataSet", countersalesreturn));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CounterSalesReturnTransPrintPdfDataSet", countersalesreturnTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "CounterSalesReturnPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/CounterSalesReturn/"), FileName);
            string URL = "/Outputs/CounterSalesReturn/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SalesReturnPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TaxInvoice);
            ReportNameParam = new ReportParameter("ReportName", "SALES RETURN");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var salesreturn = salesReturnBL.GetSalesReturn(Id).Select(a => new SpGetSalesReturnDetail_Result()
            {
                ID = a.ID,
                Code = a.SRNo,
                CurrencyCode = a.CurrencyCode,
                TranDate = (DateTime)a.SRDate,
                CustomerName = a.CustomerName.TrimStart(),
                NetAmount = (decimal)a.NetAmount,
                IsDraft = (bool)a.IsDraft,
                Cancelled = a.IsCancelled,
                CustomerID = (int)a.CustomerID,
                GrossAmount = (decimal)a.GrossAmount,
                SGSTAmount = (decimal)a.SGSTAmount,
                CGSTAmount = (decimal)a.CGSTAmount,
                IGSTAmount = (decimal)a.IGSTAmount,
                RoundOff = (decimal)a.RoundOff,
                SalesInvoiceID = (int)a.SalesInvoiceID,
                InvoiceNo = a.InvoiceNo,
                IsNewInvoice = (bool)a.IsNewInvoice,
                AmountInWords = a.AmountInWords,
                DecimalPlaces = a.DecimalPlaces,
            }).ToList();
            var salesreturnTrans = salesReturnBL.GetSalesReturnTrans(Id).Select(a => new SpGetSalesReturnTrans_Result()
            {
                ItemID = a.ItemID,
                ItemName = a.Name,
                MRP = (decimal)a.MRP,
                SecondaryMRP = a.SecondaryMRP,
                SecondaryQty = a.SecondaryQty,
                SecondaryUnit = a.SecondaryUnit,
                SecondaryOfferQty = a.SecondaryOfferQty,
                SecondaryUnitSize = a.SecondaryUnitSize,
                BasicPrice = (decimal)a.BasicPrice,
                Qty = a.SaleQty,
                OfferQty = a.OfferQty,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountAmount = (decimal)a.DiscountAmount,
                GrossAmount = (decimal)a.GrossAmount,
                CGSTAmt = (decimal)a.CGST,
                IGSTAmt = (decimal)a.IGST,
                SGSTAmt = (decimal)a.SGST,
                IGSTPercentage = (decimal)a.IGSTPercentage,
                CGSTPercentage = (decimal)a.CGSTPercentage,
                SGSTPercentage = (decimal)a.SGSTPercentage,
                NetAmt = (decimal)a.NetAmount,
                Unit = a.Unit,
                UnitID = (int)a.UnitID,
                ReturnQty = (decimal)a.Qty,
                ItemCode = a.Code,
                BatchNo = a.BatchName,
                BatchID = (int)a.BatchID,
                ReturnOfferQty = (decimal)a.OfferReturnQty,
                TransNo = a.TransNo,
                SalesInvoiceTransID = (int)a.SalesInvoiceTransID,
                SalesUnitID = (int)a.SalesUnitID,
                PrimaryUnitID = (int)a.PrimaryUnitID,
                LooseRate = (decimal)a.LoosePrice,
                FullRate = (decimal)a.FullPrice,
                ConvertedQuantity = (decimal)a.ConvertedQuantity,
                InvoiceQuantity = (decimal)a.SalesInvoiceQty,
                CounterSalesTransUnitID = (int)a.SalesTransUnitID,
                SalesUnit = a.SalesUnit,
                PrimaryUnit = a.PrimaryUnit,
                LogicCodeID = (int)a.LogicCodeID,
                LogicCode = a.LogicCode,
                LogicName = a.LogicName,
                ConvertedOfferQuantity = (decimal)a.ConvertedOfferQuantity,
                InvoiceOfferQuantity = (decimal)a.SalesOfferQty,
                BatchtypeID = (int)a.BatchTypeID,
                VATPercentage = (decimal)a.VATPercentage,
                VATAmount = (decimal)a.VATAmount,
               // Make = a.Make
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("SalesReturnPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesReturnPrintPdfDataSet", salesreturn));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesReturnTransPrintPdfDataSet", salesreturnTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SalesReturnPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SalesReturn/"), FileName);
            string URL = "/Outputs/SalesReturn/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

    }
}
