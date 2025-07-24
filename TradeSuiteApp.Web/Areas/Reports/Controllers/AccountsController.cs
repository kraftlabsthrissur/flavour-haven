using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Reports.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class AccountsController : BaseReportController
    {
        private ReportsEntities rdb;
        private IPaymentTypeContract paymentTypeBL;
        private IPaymentModeContract paymentModeBL;
        private IEmployeeContract employeeBL;
        private IReportContract reportBL;
        private IPaymentVoucher paymentVoucherBL;
        private IDepartmentContract departmentBL;
        private ILocationContract locationBL;
        private ITreasuryContract treauryBL;
        private IPremisesContract premisesBL;
        private SelectList AtoZRange = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
        private DateTime StartDate, EndDate;
        private IProjectContract projectBL;
        private IInterCompanyContract interCompanyBL;
        private IAdvancePayment iAdvancePayment;
        private ICustomerCreditNoteContract creditnoteBL;
        private ICustomerDebitNoteContract debitnoteBL;
        private ISupplierCreditNoteContract suppliercreditnoteBL;
        private ISupplierDebitNoteContract supplierdebitnoteBL;
        private IFundTransferContract fundtranferBL;
        IReceiptVoucher receiptVoucherBL;
        private IStatusContract statusBL;
        private IBankExpensesContract bankexpensesBL;
        private IFundTransferReceiptContract fundreceiptBL;
        private IPaymentReturnVoucherContract paymentReturnVoucherBL;
        private ICustomerReturnVoucherContract customerReturnVoucherBL;
        private IDebitNoteContract debitnoteBLv3;
        private IAddressContract addressBL;
        private DateTime? RefDocDate;

        public AccountsController()
        {
            employeeBL = new EmployeeBL();
            departmentBL = new DepartmentBL();
            paymentTypeBL = new PaymentTypeBL();
            treauryBL = new TreasuryBL();
            premisesBL = new PremisesBL();
            projectBL = new ProjectBL();
            interCompanyBL = new InterCompanyBL();
            fundtranferBL = new FundTransferBL();
            statusBL = new StatusBL();
            bankexpensesBL = new BankExpensesBL();
            fundreceiptBL = new FundTransferReceiptBL();

            rdb = new ReportsEntities();
            reportBL = new ReportBL();
            paymentVoucherBL = new PaymentVoucherBL();
            paymentModeBL = new PaymentModeBL();
            locationBL = new LocationBL();
            iAdvancePayment = new AdvancePaymentBL();
            receiptVoucherBL = new ReceiptVoucherBL();
            creditnoteBL = new CustomerCreditNoteBL();
            debitnoteBL = new CustomerDebitNoteBL();
            suppliercreditnoteBL = new SupplierCreditNoteBL();
            supplierdebitnoteBL = new SupplierDebitNoteBL();
            paymentReturnVoucherBL = new PaymentReturnVoucherBL();
            customerReturnVoucherBL = new CustomerReturnVoucherBL();
            debitnoteBLv3 = new DebitNoteBL();
            addressBL = new AddressBL();
            ViewBag.FinStartDate = GeneralBO.FinStartDate;
            ViewBag.CurrentDate = General.FormatDate(DateTime.Now);

        }
        // GET: Reports/AccountLedger
        public ActionResult Index()
        {
            return View();
        }

        // below codes are created by lini on 20/04/2018
        // GET: Reports/AccountsLedger/AccountsLedgerReport
        [HttpGet]
        public ActionResult GeneralLedger()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            rep.FromAccountNameRangeList = AtoZRange;
            rep.ToAccountNameRangeList = AtoZRange;
            rep.LocationID = GeneralBO.LocationID;
            rep.ItemLocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            rep.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            rep.FromEmployeeNoRangeList = AtoZRange;
            rep.ToEmployeeNoRangeList = AtoZRange;
            rep.FromInterCompanyRangeList = AtoZRange;
            rep.ToInterCompanyRangeList = AtoZRange;
            rep.ProjectList = new SelectList(projectBL.GetProjectList(), "ID", "Name");
            rep.EmployeeList = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            rep.InterCompanyList = new SelectList(interCompanyBL.GetInterCompanyList(), "ID", "Name");
            return View(rep);
        }

        [HttpPost]
        // GET: Reports/AccountsLedger/AccountsLedgerReport
        public ActionResult GeneralLedger(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.AccountCreditID != null)//priority is for supplier id so item range is set to null
            {
                model.AccountNameFromRange = null;
                model.AccountNameToRange = null;
            }
            var AccountLedger = rdb.SpRptAccountLedger(
                model.ReportType,
                model.AccountGroupID,
                model.AccountNameID,
                StartDate,
                EndDate,
                model.DocType,
                model.DocumentNo,
                model.TransType,
                model.AccountCodeFromID,
                model.AccountCodeToID,
                model.AccountNameFromRange,
                model.AccountNameToRange,
                model.ItemLocationID,
                model.DepartmentID,
                model.EmployeeID,
                model.InterCompanyID,
                model.ProjectID,
                GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                ).ToList();
            if (model.ReportType == "DocumentNumberwise")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/Ledger/GeneralLedgerReport.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AccountLedgerDataSet", AccountLedger));
            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/Ledger/GeneralLedgerAccountCodeWiseReport.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AccountLedgerDataSet", AccountLedger));
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "General Ledger");
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        //created by Lini and Anju on28/4/2018
        [HttpGet]
        public ActionResult ItemSubLedger()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.PremisesList = new SelectList(premisesBL.GetPremisesList(rep.LocationID), "ID", "Name");
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        // GET: Reports/Accounts/ItemSubLedger
        public ActionResult ItemSubLedger(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            //  model.FromDate = General.FormatDate(DateTime.Now);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            model.PremisesList = new SelectList(premisesBL.GetPremisesList(model.LocationID), "ID", "Name");
            var pre = 0;
            model.PremisesID = pre;
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Item Sub Ledger");
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var ItemSubLedger = rdb.SpRptItemSubLedger(
                                        StartDate,
                                        EndDate,
                                        model.ItemID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/Ledger/ItemSubLedgerReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemSubLedgerDataSet", ItemSubLedger));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //created by Lini and Anju on28/4/2018
        [HttpGet]
        public ActionResult SupplierSubLedger()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        // GET: Reports/Accounts/SupplierSubLedger
        public ActionResult SupplierSubLedger(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            //  model.FromDate = General.FormatDate(DateTime.Now);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SupplierSubLedgerReport);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);

            var SupplierSubLedger = rdb.SpRptSupplierSubLedger(
                                                StartDate,
                                                EndDate,
                                                model.SupplierID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/SupplierSubLedgerReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierSubLedgerReportDataSet", SupplierSubLedger));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        // GET: Reports/Accounts/PaymentVoucher
        [HttpGet]
        public ActionResult PaymentVoucher()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = General.FormatDate(General.FirstDayOfMonth);
            rep.ToDate = General.FormatDateTime(DateTime.Now);
            rep.FromSupplierRangeList = AtoZRange;
            rep.ToSupplierRangeList = AtoZRange;
            rep.LocationID = GeneralBO.LocationID;
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.UserName = GeneralBO.EmployeeName;
            ViewBag.ReportViewer = reportViewer;

            return View(rep);
        }


        [HttpPost]
        public ActionResult PaymentVoucher(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.SupplierID != null)//priority is for supplier id so item range is set to null
            {
                model.FromSupplierRange = null;
                model.ToSupplierRange = null;
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Payment Voucher Report");
            FilterParam = new ReportParameter("Filters", model.Filters);

            var PaymentVoucher = rdb.SpRptPaymentVoucherSummary(
                                                StartDate,
                                                EndDate,
                                                model.LocationID,
                                                model.FromSupplierRange,
                                                model.ToSupplierRange,
                                                model.SupplierID,
                                                model.BankDetails,
                                                GeneralBO.CreatedUserID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/PaymentVoucherReportSummary.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PaymentVoucherDataSet", PaymentVoucher));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        // GET: Reports/Accounts/AdvancePayment
        [HttpGet]
        public ActionResult AdvancePayment()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            // rep.Categories = GetCategories();
            ViewBag.ReportViewer = reportViewer;
            rep.FromSupplierRangeList = AtoZRange;
            rep.ToSupplierRangeList = AtoZRange;
            rep.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", rep.PaymentTypeID);
            return View(rep);
        }
        public JsonResult GetSupplierCatRange(string from_range)
        {
            AccountReportModel rep = new AccountReportModel();
            if (from_range == "")
            {
                rep.ToSupplierCatRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.ToSupplierCatRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = rep.ToSupplierCatRangeList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdvancePayment(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Advance Payment " + model.ReportType + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var AdvancePayment = rdb.SpRptAdvancePayment(
                                        StartDate,
                                        EndDate,
                                        model.ReportType,
                                        model.AdvancePaymentNoFromID,
                                        model.AdvancePaymentNoToID,
                                        model.SupplierID,
                                        model.EmployeeID,
                                        model.FromSupplierRange,
                                        model.ToSupplierRange,
                                        model.PaymentTypeID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
            if (model.Summary == "Summary")
            {
                if (model.ReportType == "All")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/AdvancePaymentReportSummaryAll.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvancePaymentDataSet", AdvancePayment));
                }
                else if (model.ReportType == "Supplier")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/AdvancePaymentReportSummarySupplierWise.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvancePaymentDataSet", AdvancePayment));
                }
                else if (model.ReportType == "Employee")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/AdvancePaymentReportSummaryEmployeeWise.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvancePaymentDataSet", AdvancePayment));
                }
            }
            else
            {
                if (model.ReportType == "All")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/AdvancePaymentReportDetailAll.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvancePaymentDataSet", AdvancePayment));
                }
                else if (model.ReportType == "Supplier")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/AdvancePaymentReportDetailSupplierWise.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvancePaymentDataSet", AdvancePayment));
                }
                else if (model.ReportType == "Employee")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/AdvancePaymentReportDetailEmployeeWise.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvancePaymentDataSet", AdvancePayment));
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        // GET: Reports/Accounts/PaymentVoucher
        [HttpGet]
        public ActionResult AdvanceRequest()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.EmployeeList = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.Location = GeneralBO.LocationID;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        // POST: Reports/Accounts/PaymentVoucher
        [HttpPost]
        public ActionResult AdvanceRequest(AccountReportModel model)
        {
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.EmployeeList = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Advance Request " + model.Summary);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            FilterParam = new ReportParameter("Filters", model.Filters);
            var AdvanceRequest = rdb.SpRptAdvanceRequest(
                    StartDate,
                    EndDate,
                    model.EmployeeID,
                    model.Location,
                    GeneralBO.CreatedUserID,
                    GeneralBO.FinYear,
                    model.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/AdvanceRequestAll" + model.Summary + "Rpt.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvanceRequestAll" + model.Summary + "DataSet", AdvanceRequest));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        [HttpGet]
        public ActionResult AdvancePaymentReturn()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.FromSupplierRangeList = AtoZRange;
            rep.ToSupplierRangeList = AtoZRange;
            rep.FromEmployeeNoRangeList = AtoZRange;
            rep.ToEmployeeNoRangeList = AtoZRange;
            rep.StatusList = new SelectList(statusBL.GetStatusList("AdvancePaymentReturnReport"), "Value", "Text");
            rep.Categories = GetCategories();
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult AdvancePaymentReturn(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Advance Payment Return " + model.ReportType + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var AdvanceReturn = new object();
            if (model.Summary == "Summary")
            {
                AdvanceReturn = rdb.SpRptAdvanceReturnSummary(
                                        StartDate,
                                        EndDate,
                                        model.ReportType,
                                        model.SupplierID,
                                        model.EmployeeID,
                                        model.FromSupplierRange,
                                        model.ToSupplierRange,
                                        model.EmployeeNoFromRange,
                                        model.EmployeeNoToRange,
                                        model.AdvancePaymentNoFromID,
                                        model.AdvancePaymentNoToID,
                                        model.AdvReturnVchNoFromID,
                                        model.AdvReturnVchNoToID,
                                        model.Status,
                                        GeneralBO.FinYear,
                                        GeneralBO.@LocationID,
                                        GeneralBO.@ApplicationID).ToList();
            }
            else
            {
                AdvanceReturn = rdb.SpRptAdvanceReturn(
                                        StartDate,
                                        EndDate,
                                        model.ReportType,
                                        model.SupplierID,
                                        model.EmployeeID,
                                        model.FromSupplierRange,
                                        model.ToSupplierRange,
                                        model.EmployeeNoFromRange,
                                        model.EmployeeNoToRange,
                                        model.AdvancePaymentNoFromID,
                                        model.AdvancePaymentNoToID,
                                        model.AdvReturnVchNoFromID,
                                        model.AdvReturnVchNoToID,
                                        model.Status,
                                        GeneralBO.FinYear,
                                        GeneralBO.@LocationID,
                                        GeneralBO.@ApplicationID).ToList();
            }

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/AdvanceReturn" + model.Summary + ".rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvanceReturnDataSet", AdvanceReturn));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        private List<SelectListItem> GetCategories()
        {
            return new List<SelectListItem>() { new SelectListItem() { Text = "Supplier", Value = "Supplier" }, new SelectListItem() { Text = "Employee", Value = "Employee" } };
        }
        public JsonResult GetAutoComplete(string Term = "", string Table = "")
        {
            List<AccountReportModel> CodeList = new List<AccountReportModel>();
            CodeList = reportBL.GetAutoComplete(Term, Table).Select(a => new AccountReportModel()
            {

                ID = a.ID,
                Code = a.Code

            }).ToList();
            return Json(CodeList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CashBankLedger()
        {
            CashOrBankModel rep = new CashOrBankModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.Location = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.Username = GeneralBO.EmployeeName;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            rep.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult CashBankLedger(CashOrBankModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Cash/Bank Ledger");
            FilterParam = new ReportParameter("Filters", model.Filters);

            var CashBank = rdb.SpRptCashBankLedger(
                StartDate,
                EndDate,
                model.BankID,
                model.DocumentNo,
                model.AccountCodeFromID,
                model.AccountNameID,
                model.Location,
                model.PaymentModeID,
                GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/CashOrBankLedger.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CashOrBankLedgerDataSet", CashBank));

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //created by Lini  on 03/09/2018
        [HttpGet]
        public ActionResult DebitAndCreditNote()
        {
            DebitAndCreditNoteModel rep = new DebitAndCreditNoteModel();
            rep.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            rep.ToDateString = General.FormatDate(DateTime.Now);
            //rep.FromLocationRangeList = AtoZRange;
            //rep.ToLocationRangeList = AtoZRange;
            //rep.FromDepartmentRangeList = AtoZRange;
            //rep.ToDepartmentRangeList = AtoZRange;
            //rep.EmployeeFromRangeList = AtoZRange;
            //rep.EmployeeToRangeList = AtoZRange;
            //rep.LocationID = GeneralBO.LocationID;
            //rep.ItemLocationID = GeneralBO.LocationID;
            //rep.UserID = GeneralBO.CreatedUserID;
            //rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            //rep.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            //rep.ProjectList = new SelectList(projectBL.GetProjectList(), "ID", "Name");
            //rep.InterCompanyList = new SelectList(interCompanyBL.GetInterCompanyList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        // GET: Reports/Accounts/SupplierSubLedger
        public ActionResult DebitAndCreditNote(DebitAndCreditNoteModel model)
        {
            //if (model.FromDate != null)
            //    StartDate = General.ToDateTime(model.FromDate);
            //if (model.ToDate != null)
            //    EndDate = General.ToDateTime(model.ToDate);
            //if (model.RefDocDate != null)
            //    RefDocDate = General.ToDateTime(model.RefDocDate);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate",model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName",  model.Type + " " +"Report " );
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var Note = new object();
            string XMLParams = XMLHelper.ParseXML(model);

            Note = rdb.SpRptDebitAndCreditNote(
                XMLParams).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/" + model.Type + ".rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DebitAndCeditNoteDataSet", Note));
            
            //var PartyType = model.party + model.Type;
            //if (model.Summary == "Summary")
            //{
            //    Note = rdb.SpRptDebitAndCreditNoteSummary(
            //                StartDate,
            //                EndDate,
            //                PartyType,
            //                model.DebitNoteNoFromID,
            //                model.DebitNoteNoToID,
            //                model.SupplierID,
            //                model.CustomerCodeFromID,
            //                model.CustomerCodeToID,
            //                model.CustomerID,
            //                model.RefInvoiceNo,
            //                RefDocDate,
            //                model.Location,
            //                GeneralBO.CreatedUserID,
            //                GeneralBO.FinYear,
            //                GeneralBO.LocationID,
            //                GeneralBO.ApplicationID);
            //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/" + model.party + model.Type + model.Summary + ".rdlc";
            //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(model.party + model.Type + "DataSet", Note));
            //}
            //else
            //{
            //    Note = rdb.SpRptDebitAndCreditNoteDetail(
            //                StartDate,
            //                EndDate,
            //                model.party + model.Type,
            //                model.DebitNoteNoFromID,
            //                model.DebitNoteNoToID,
            //                model.SupplierID,
            //                model.CustomerCodeFromID,
            //                model.CustomerCodeToID,
            //                model.CustomerID,
            //                model.RefInvoiceNo,
            //                RefDocDate,
            //                model.Location,
            //                GeneralBO.CreatedUserID,
            //                GeneralBO.FinYear,
            //                GeneralBO.LocationID,
            //                GeneralBO.ApplicationID);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        [HttpGet]
        public ActionResult JournalVoucher()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.FromDocTypeRangeList = AtoZRange;
            rep.ToDocTypeRangeList = AtoZRange;
            rep.FromTransTypeRangeList = AtoZRange;
            rep.ToTransTypeRangeList = AtoZRange;
            rep.FromInterCompanyRangeList = AtoZRange;
            rep.ToInterCompanyRangeList = AtoZRange;
            rep.FromAccountNameRangeList = AtoZRange;
            rep.ToAccountNameRangeList = AtoZRange;
            rep.LocationID = GeneralBO.LocationID;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(GeneralBO.CreatedUserID), "ID", "Name");
            rep.ProjectList = new SelectList(projectBL.GetProjectList(), "ID", "Name");
            rep.EmployeeList = new SelectList(employeeBL.GetEmployeeList(), "EmployeeID", "Name");
            rep.InterCompanyList = new SelectList(interCompanyBL.GetInterCompanyList(), "ID", "Name");
            rep.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            rep.FromEmployeeNoRangeList = AtoZRange;
            rep.ToEmployeeNoRangeList = AtoZRange;
            rep.FromInterCompanyRangeList = AtoZRange;
            rep.ToInterCompanyRangeList = AtoZRange;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult JournalVoucher(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.DocType != null)
            {
                model.DocTypeFromRange = null;
                model.DocTypeToRange = null;
            }
            if (model.TransTypeID != null)
            {
                model.TransTypeFromRange = null;
                model.TransTypeToRange = null;
            }
            if (model.AccountCreditID != null)
            {
                model.AccountNameFromRange = null;
                model.AccountNameToRange = null;
            }
            var JournalVoucher = rdb.SpJounalVoucher(
                                        StartDate,
                                        EndDate,
                                        model.DocumentNoFromID,
                                        model.DocumentNoToID,
                                        model.AccountCodeFromID,
                                        model.AccountCodeToID,
                                        model.AccountNameFromRange,
                                        model.AccountNameToRange,
                                        model.AccountNameID,
                                        model.DepartmentID,
                                        model.EmployeeNoFromRange,
                                        model.EmployeeNoToRange,
                                        model.EmployeeID,
                                        model.InterCompanyFromRange,
                                        model.InterCompanyToRange,
                                        model.InterCompanyID,
                                        model.ProjectID,
                                        model.SLAStatusID,
                                        model.LocationID,
                                        GeneralBO.CreatedUserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/Ledger/JournalVoucher.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("JournalVoucherDataSet", JournalVoucher));
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Journal Voucher");
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            this.reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult FundTransfer()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.FromBankNameList = new SelectList(treauryBL.GetBank(rep.LocationID), "ID", "BankName");
            rep.ToBankNameList = new SelectList(treauryBL.GetBank(rep.LocationID), "ID", "BankName");
            return View(rep);
        }
        [HttpPost]
        public ActionResult FundTransfer(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Contra Voucher Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            var StockTransfer = rdb.SpRptFundTransfer(
                                    model.FromBankNameID,
                                    model.ToBankNameID,
                                    model.FromBankAccountNo,
                                    model.ToBankAccountNo,
                                    model.FromTransNoID,
                                    model.ToTransNoID,
                                    StartDate,
                                    EndDate,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/FundTransferDetail.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FundTransferDataSet", StockTransfer));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult ChequeDeposit()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.FromBankNameList = new SelectList(treauryBL.GetBank(), "ID", "BankName");
            rep.ToBankNameList = new SelectList(treauryBL.GetBank(), "ID", "BankName");

            rep.StatusList = new SelectList(statusBL.GetStatusList("ChequeDepositReport"), "Value", "Text");

            return View(rep);
        }

        [HttpPost]
        public ActionResult ChequeDeposit(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Cheque Status Report");
            FilterParam = new ReportParameter("Filters", model.Filters);

            var Cheque = rdb.SpRptChequeDeposited(
                                        StartDate,
                                        EndDate,
                                        model.BankID,
                                        model.BankAccountNoID,
                                        model.InstrumentNoID,
                                        model.Status,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/ChequeDepositReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ChequeDepositDataSet", Cheque));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult DayBook()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            return View(rep);
        }
        public ActionResult DayBook(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.AccountNameID == 0)
                model.AccountNameID = null;
            if (model.TransCodeID == 0)
                model.TransCodeID = null;

            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ChequeDepositedReport);
            var DayBook = rdb.SpRptDayBook(StartDate, EndDate, model.AccountNameID, model.TransCodeID, GeneralBO.FinYear,
                GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/DayBookReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DayBookDataSet", DayBook));
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.DayBook);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult TrialBalance()
        {
            AccountReportModel rep = new AccountReportModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            return View(rep);
        }
        [HttpPost]
        public ActionResult TrialBalance(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Trial Balance Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            var TrialBalance = new object();
            if (model.ReportDataType == "Summary")
            {
                TrialBalance = rdb.SpRptTrialBalanceThreeColumn(
                                            EndDate,
                                            model.AccountCodeFromID,
                                            model.AccountNameID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("TrialBalanceReport");
            }
            else
            {
                TrialBalance = rdb.SpRptTrialBalanceThreeColumnDetail(
                                            EndDate,
                                            model.AccountCodeFromID,
                                            model.AccountNameID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("TrialBalanceReportDetail");
            }

            //reportViewer.LocalReport.ReportPath = GetReportPath("TrialBalanceReport");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TrialBalanceDataSet", TrialBalance));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpPost]
        public ActionResult TrialBalanceStr(AccountReportModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.AccountNameID == 0)
                model.AccountNameID = null;
            if (model.TransCodeID == 0)
                model.TransCodeID = null;

            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ChequeDepositedReport);
            var TrialBalance = rdb.SpRptTrialBalance(EndDate, model.AccountNameID, GeneralBO.FinYear,
                GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            //var TrialBalance = rdb.SpRptTrialBalanceThreeColumn(EndDate, model.AccountNameID, GeneralBO.FinYear,
            //    GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("TrialBalanceReport");//Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/TrialBalanceReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TrialBalanceDataSet", TrialBalance));
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TrialBalance);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpPost]
        public ActionResult CustomerCreditNotePrint(int Id)
        {
            try
            {
                FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
                ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
                ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.CreditNote);
                Warning[] warnings;
                string[] streamIds;
                string contentType;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                var Sales = rdb.SpCustomerCreditNotePrint(
                        Id,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/CustomerCreditNotePrint.rdlc";
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustCreditNotePrintDataSet", Sales));
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
                return View("~/Areas/Reports/Views/ReportViewer.cshtml");
            }
            catch (Exception e)
            {
                return View();

            }
        }

        [HttpPost]
        public ActionResult CustomerDebitNotePrint(int Id)
        {
            try
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
                var Sales = rdb.SpCustomerDebitNotePrint(
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
                return View("~/Areas/Reports/Views/ReportViewer.cshtml");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult ReceiptVoucher()
        {
            ReceiptVoucherReportModel rep = new ReceiptVoucherReportModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDateTime(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.ItemLocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.UserName = GeneralBO.EmployeeName;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(rep.UserID), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult ReceiptVoucher(ReceiptVoucherReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Receipt Voucher");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var ReceiptVoucher = rdb.SpReceiptVoucher(
                                                model.FromDate,
                                                model.ToDate,
                                                model.CustomerCodeFromID,
                                                model.CustomerID,
                                                model.ItemLocationID,
                                                GeneralBO.CreatedUserID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID
                                                ).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Accounts/ReceiptVoucher.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReceiptVoucherDataSet", ReceiptVoucher));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpPost]
        public JsonResult AdvancePaymentPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.AdvancePaymentVoucher);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var AdvancePayment = iAdvancePayment.GetAdvancePaymentDetails((int)Id).Select(a => new SpGetAdvancePayment_Result()
            {
                ID = a.ID,
                AdvanceNo = a.AdvancePaymentNo,
                AdvanceDate = (DateTime)a.AdvancePaymentDate,
                Category = a.Category,
                Name = a.SelectedName,
                Amount = (decimal)a.Amt,
                Mode = a.ModeOfPaymentName,
                BankName = a.BankDetail,
                ReferenceNo = a.ReferenceNo,
                NetAmount = (decimal)a.NetAmount,
                SupplierOrEmployeeBankName = a.SupplierOrEmployeeBankName,
                SupplierOrEmployeeBankACNo = a.SupplierOrEmployeeBankACNo,
                SupplierOrEmployeeIFSCNo = a.SupplierOrEmployeeIFSCNo
            }).ToList();
            var AdvancePaymentTrans = iAdvancePayment.GetAdvancePaymentTransDetails((int)Id).Select(a => new SpGetAdvancePaymentTrans_Result()
            {
                PODate = (DateTime)a.PurchaseOrderDate,
                Amount = (decimal)a.Amount,
                TDSAmount = (decimal)a.TDSAmount,
                Remarks = a.Remarks,
                TransNo = a.TransNo,
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("AdvancePaymentPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvancePaymentPrintPdfDataSet", AdvancePayment));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AdvancePaymentTransPrintPdfDataSet", AdvancePaymentTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "AdvancePaymentPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/AdvancePayment/"), FileName);
            string URL = "/Outputs/AdvancePayment/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PaymentVoucherPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PaymentVoucher);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PaymentVoucher = paymentVoucherBL.GetPaymentVoucherDetailV3((int)Id).Select(a => new SpGetPaymentVoucherDetailV3_Result()
            {
                VoucherNo = a.VoucherNo,
                VoucherDate = (DateTime)a.VoucherDate,
                AccountName = a.AccountHead,
                BankName = a.BankName,
                ReferenceNo = a.ReferenceNumber,
                Remark = a.Remark,
                PaymentTypeName = a.PaymentTypeName,
                IsDraft = a.IsDraft,
                ReconciledDate=a.ReconciledDate,
                MinimumCurrencyCode = a.MinimumCurrencyCode,
                AmountInWords=a.AmountInWords,
                SuuplierCurrencyconverion=a.SuuplierCurrencyconverion,
                supplierCurrencycode=a.supplierCurrencycode,
                currencycode=a.currencycode,
                LocalCurrencyID = (int)a.LocalCurrencyID,
                LocalCurrencyCode = a.LocalCurrencyCode,
                CurrencyExchangeRate = (decimal)a.CurrencyExchangeRate,
                LocalNetAmt = (decimal)a.LocalVoucherAmt,
                //SupplierBankName = a.SupplierBankName,
                //SupplierBankACNo = a.SupplierBankACNo,
                //SupplierIFSCNo = a.SupplierIFSCNo
            }).ToList();
            var PaymentVoucherTrans = paymentVoucherBL.GetPaymentVoucherTransV3((int)Id).Select(a => new SpGetPaymentVoucherTransV3_Result()
            {
                DocumentAmount= (decimal)a.DocumentAmount,
                AmountToBePaid= (decimal)a.AmountToBePayed,
                PaidAmount = (decimal)a.PaidAmount,
                DocumentNo = a.InvoiceNo,
                SettledDate=a.Date,
                Narration=a.Narration
                
            }).ToList();

            
            reportViewer.LocalReport.ReportPath = GetReportPath("PaymentVoucherPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PaymentVoucherPrintPdfDataSet", PaymentVoucher));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PaymentVoucherTransPrintPdfDataSet", PaymentVoucherTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "PaymentVoucherPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PaymentVoucher/"), FileName);
            string URL = "/Outputs/PaymentVoucher/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReceiptVoucherPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ReceiptVoucher);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ReceiptVoucher = receiptVoucherBL.GetReceiptDetailsV3((int)Id);
            List<SpGetReceiptVoucherDetailV3_Result> receipts = new List<SpGetReceiptVoucherDetailV3_Result>();
            SpGetReceiptVoucherDetailV3_Result receipt = new SpGetReceiptVoucherDetailV3_Result()
            {
                ID = ReceiptVoucher.ID,
                VoucherNo = ReceiptVoucher.ReceiptNo,
                VoucherDate = ReceiptVoucher.ReceiptDate,
                ReceiptAmount = ReceiptVoucher.ReceiptAmount,
                AccountHead = ReceiptVoucher.AccountHead,
                BankName = ReceiptVoucher.BankName,
                Mode = ReceiptVoucher.PaymentTypeName,
                Remarks = ReceiptVoucher.Remarks,
                ReferenceNo = ReceiptVoucher.BankReferanceNumber,
                currencycode= ReceiptVoucher.currencycode,
                supplierCurrencycode= ReceiptVoucher.supplierCurrencycode,
                SuuplierCurrencyconverion= ReceiptVoucher.SuuplierCurrencyconverion,
                AmountInWords = ReceiptVoucher.AmountInWords

            };
            receipts.Add(receipt);
            var ReceiptVoucherTrans = receiptVoucherBL.GetReceiptTransV3((int)Id).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("ReceiptVoucherPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReceiptVoucherPrintPdfDataSet", receipts));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReceiptVoucherTransPrintPdfDataSet", ReceiptVoucherTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "ReceiptVoucherPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ReceiptVoucher/"), FileName);
            string URL = "/Outputs/ReceiptVoucher/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CustomerCreditNotePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.CustomerCreditNote);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var CustomerCreditNote = creditnoteBL.GetCreditNoteDetails(Id).ToList();
            var CustomerCreditNoteTrans = creditnoteBL.GetCreditNoteTransDetails(Id).Select(a => new SpGetCustomerCreditNoteTransDetails_Result()
            {
                Item = a.Item,
                HSNCode = a.HSNCode,
                Unit = a.Unit,
                Quantity = (decimal)a.Qty,
                Rate = (decimal)a.Rate,
                Amount = (decimal)a.Amount,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                CGSTAmt = (decimal)a.CGSTAmt,
                SGSTAmt = (decimal)a.SGSTAmt,
                IGSTAmt = (decimal)a.IGSTAmt,
                Remarks = a.Remarks,
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("CustomerCreditNotePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerCreditNotePrintPdfDataSet", CustomerCreditNote));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerCreditNoteTransPrintPdfDataSet", CustomerCreditNoteTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "CustomerCreditNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/CustomerCreditNote/"), FileName);
            string URL = "/Outputs/CustomerCreditNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CustomerDebitNotePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.CustomerDebitNote);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var CustomerDebitNote = debitnoteBL.GetDebitNoteDetail(Id).ToList();
            var CustomerDebitNoteTrans = debitnoteBL.GetDebitNoteDetailTrans(Id).Select(a => new SpGetCustomerDebitNoteTransDetail_Result()
            {
                Item = a.Item,
                HSNCode = a.HSNCode,
                Unit = a.Unit,
                Quantity = (decimal)a.Qty,
                Rate = (decimal)a.Rate,
                Amount = (decimal)a.Amount,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                CGSTAmt = (decimal)a.CGSTAmt,
                SGSTAmt = (decimal)a.SGSTAmt,
                IGSTAmt = (decimal)a.IGSTAmt,
                Remarks = a.Remarks,
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("CustomerDebitNotePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerDebitNotePrintPdfDataSet", CustomerDebitNote));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerDebitNoteTransPrintPdfDataSet", CustomerDebitNoteTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "CustomerDebitNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/CustomerDebitNote/"), FileName);
            string URL = "/Outputs/CustomerDebitNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SupplierCreditNotePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SupplierCreditNote);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SupplierCreditNote = suppliercreditnoteBL.GetCreditNoteDetail(Id).ToList();
            var SupplierCreditNoteTrans = suppliercreditnoteBL.GetCreditNoteDetailTrans(Id).Select(a => new SpGetSupplierCreditNoteTransDetail_Result()
            {
                Item = a.Item,
                HSNCode = a.HSNCode,
                Unit = a.Unit,
                Quantity = (decimal)a.Qty,
                Rate = (decimal)a.Rate,
                Amount = (decimal)a.Amount,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                CGSTAmt = (decimal)a.CGSTAmt,
                SGSTAmt = (decimal)a.SGSTAmt,
                IGSTAmt = (decimal)a.IGSTAmt,
                Remarks = a.Remarks,
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("SupplierCreditNotePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierCreditNotePrintPdfDataSet", SupplierCreditNote));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierCreditNoteTransPrintPdfDataSet", SupplierCreditNoteTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SupplierCreditNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SupplierCreditNote/"), FileName);
            string URL = "/Outputs/SupplierCreditNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SupplierDebitNotePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.SupplierDebitNote);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var SupplierDebitNote = supplierdebitnoteBL.GetDebitNoteDetail(Id).ToList();
            var SupplierDebitNoteTrans = supplierdebitnoteBL.GetDebitNoteTransDetail(Id).Select(a => new SpGetSupplierDebitNoteTransDetails_Result()
            {
                Item = a.Item,
                HSNCode = a.HSNCode,
                Unit = a.Unit,
                Quantity = (decimal)a.Qty,
                Rate = (decimal)a.Rate,
                Amount = (decimal)a.Amount,
                DiscountAmount = (decimal)a.DiscountAmount,
                TaxableAmount = (decimal)a.TaxableAmount,
                CGSTAmt = (decimal)a.CGSTAmt,
                SGSTAmt = (decimal)a.SGSTAmt,
                IGSTAmt = (decimal)a.IGSTAmt,
                Remarks = a.Remarks,
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("SupplierDebitNotePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierDebitNotePrintPdfDataSet", SupplierDebitNote));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierDebitNoteTransPrintPdfDataSet", SupplierDebitNoteTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "SupplierDebitNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/SupplierDebitNote/"), FileName);
            string URL = "/Outputs/SupplierDebitNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FundTransferPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.FundTransfer);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var FundTransfer = fundtranferBL.GetFundTransferDetails(Id).ToList();
            var FundTransferTrans = fundtranferBL.GetFundTransferTransDetails(Id).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("FundTransferPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FundTransferPrintPdfDataSet", FundTransfer));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FundTransferTransPrintPdfDataSet", FundTransferTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "FundTransferPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/FundTransfer/"), FileName);
            string URL = "/Outputs/FundTransfer/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FundTransferReceiptPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.FundTransfer);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var FundTransferReceipt = fundreceiptBL.GetFundTransferReceiptByID(Id).ToList();
            var FundTransferReceiptTrans = fundreceiptBL.GetFundTransferReceiptTransByID(Id).Select(a => new SpGetFundTransferReceiptTransByID_Result()
            {
                FromLocation = a.FromLocationName,
                ToLocation = a.ToLocationName,
                PaymentMode = a.Payment,
                Amount = (decimal)a.Amount,
                FromBankName = a.FromBankName,
                ToBankName = a.ToBankName,
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("FundTransferReceiptPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FundTransferReceiptPrintPdfDataSet", FundTransferReceipt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FundTransferReceiptTransPrintPdfDataSet", FundTransferReceiptTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "FundTransferReceiptPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/FundTransferReceipt/"), FileName);
            string URL = "/Outputs/FundTransferReceipt/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BankExpensesPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.BankExpenses);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var BankExpenses = bankexpensesBL.GetBankExpensesDetails(Id).Select(a => new SpGetBankExpensesDetails_Result()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = (DateTime)a.Date,
                BankID = (int)a.BankID,
                BankName = a.Bank,
                TotalAmount = a.TotalAmount
            }).ToList();
            var BankExpensesTrans = bankexpensesBL.GetBankExpensesTransDetails(Id).Select(a => new SpGetBankExpensesTransDetails_Result()
            {
                TransactionNo = a.TransactionNumber,
                TransactionDate = (DateTime)a.TransactionDate,
                ItemID = a.ItemID,
                ModeOfPaymentID = a.ModeOfPaymentID,
                ModeOfPayment = a.ModeOfPayment,
                Amount = (decimal)a.Amount,
                ItemName = a.ItemName,
                Remarks = a.Remarks,
                ReferenceNo = a.ReferenceNo
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("BankExpensesPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("BankExpensesPrintPdfDataSet", BankExpenses));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("BankExpensesTransPrintPdfDataSet", BankExpensesTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "BankExpensesPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/BankExpenses/"), FileName);
            string URL = "/Outputs/BankExpenses/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PaymentReturnVoucherPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PaymentReturnVoucher);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PaymentReturnVoucher = paymentReturnVoucherBL.GetpaymentReturnVoucherDetails(Id).ToList();
            var PaymentReturnVoucherTrans = paymentReturnVoucherBL.GetPaymentReturnTransDetails(Id).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PaymentReturnVoucherPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PaymentReturnVoucherPrintPdfDataSet", PaymentReturnVoucher));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PaymentReturnVoucherTransPrintPdfDataSet", PaymentReturnVoucherTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "PaymentReturnVoucherPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PaymentReturnVoucher/"), FileName);
            string URL = "/Outputs/PaymentReturnVoucher/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CustomerReturnVoucherPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.CustomerReturnVoucher);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var CustomerReturnVoucher = customerReturnVoucherBL.GetCustomerReturnDetails(Id).ToList();
            var CustomerReturnVoucherTrans = customerReturnVoucherBL.GetCustomerReturnTransDetails(Id).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("CustomerReturnVoucherPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerReturnVoucherPrintPdfDataSet", CustomerReturnVoucher));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerReturnVoucherTransPrintPdfDataSet", CustomerReturnVoucherTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "CustomerReturnVoucherPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/CustomerReturnVoucher/"), FileName);
            string URL = "/Outputs/CustomerReturnVoucher/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRange(string from_range)
        {
            AccountReportModel rep = new AccountReportModel();
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

        //added on 21-oct-2019
        public ActionResult TDSReport()
        {
            TDSReportModel rep = new TDSReportModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.A2ZRangeList = AtoZRange;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.LocationID = GeneralBO.LocationID;
            rep.LocationList = new SelectList(locationBL.GetLocationListByUser(GeneralBO.CreatedUserID), "ID", "Name");
            return View(rep);
        }
        [HttpPost]
        public ActionResult TDSReport(TDSReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "TDS Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);

            var TDS = new object();
            reportViewer.LocalReport.ReportPath = GetReportPath("TDS");
            TDS = rdb.SpRptTDS(
                model.FromDate,
                model.ToDate,
                model.LocationID,
                model.FromSupplierRange,
                model.ToSupplierRange,
                model.SupplierID,
                model.TransactionNo,
                model.TDSCodeID,
                model.PanNoID,
                GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TDSDataSet", TDS));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam
    });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public ActionResult TrialBalanceV3()
        {
            TrialBalanceModel rep = new TrialBalanceModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            return View(rep);
        }
        [HttpPost]
        public ActionResult TrialBalanceV3(TrialBalanceModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "TrialBalance Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);

            string XMLParams = XMLHelper.ParseXML(model);
            var TB = new object();
            if (model.TBType == "ThreeColumn")
            {
                TB = rdb.SpRptThreeColumnTrialBalance(
                    XMLParams
                    ).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("ThreeColumnTrialBalanceTally");
                //                reportViewer.LocalReport.ReportPath = GetReportPath("ThreeColumnTrialBalanceV3");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TrialBalanceThreeColumnV3DataSet", TB));
            }
            else
            {
                TB = rdb.SpRptTwoColumnTrialBalanceV3(
                    XMLParams
                    ).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("TwoColumnTrialBalanceV3");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TrialBalanceTwoColumnV3DataSet", TB));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public ActionResult AccountLedgerV3()
        {
            GeneralLedgerModel rep = new GeneralLedgerModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            return View(rep);
        }
        [HttpPost]
        public ActionResult AccountLedgerV3(GeneralLedgerModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            //ReportNameParam = new ReportParameter("ReportName", "General Ledger Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);

            string XMLParams = XMLHelper.ParseXML(model);

            var GL = new object();

            if (model.ReportType == "AccountGroup")
            {
                GL = rdb.SpRptGroupLedger(
                    XMLParams
                    ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Group Ledger Report");
                reportViewer.LocalReport.ReportPath = GetReportPath("GroupLedger");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GroupLedgerDataSet", GL));
            }
            else 
            {
                GL = rdb.SpRptGeneralLedgerV3(
                    XMLParams
                    ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Ledger Report");
                reportViewer.LocalReport.ReportPath = GetReportPath("GeneralLedgerV3");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GeneralLedgerV3DataSet", GL));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param,
                Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public JsonResult DebitNotePrintPdf(int ID)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Debit Note");
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var DebitNote = debitnoteBLv3.GetDebitNote((int)ID);
            List<SpGetDebitNote_Result> debitNote = new List<SpGetDebitNote_Result>();
            SpGetDebitNote_Result debitnote = new SpGetDebitNote_Result()
            {
                ID = DebitNote.ID,
                TransNo = DebitNote.TransNo,
                TransDate = DebitNote.TransDate,
                CreditAccountID = DebitNote.CreditAccountID,
                DebitAccountID = DebitNote.DebitAccountID,
                CreditAccount = DebitNote.CreditAccount,
                DebitAccount = DebitNote.DebitAccount,
                Remarks = DebitNote.Remarks,
                Amount = DebitNote.Amount,
                IsInclusive = DebitNote.IsInclusive,
                GSTCategoryID = DebitNote.GSTCategoryID,
                TaxableAmount = DebitNote.TaxableAmount,
                GSTAmount = DebitNote.GSTAmount,
                TotalAmount = DebitNote.TotalAmount,
                GSTPercent = DebitNote.GSTPercent,
                SupplierID=DebitNote.SupplierID
            };
            debitNote.Add(debitnote);
            var ShippingAddress = addressBL.GetShippingAddress("Supplier", DebitNote.SupplierID, "").ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("DebitNotePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DebitNotePrintPdfDataSet", debitNote));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierAddressDataSet", ShippingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "DebitNotePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/DebitNote/"), FileName);
            string URL = "/Outputs/DebitNote/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
    }
}