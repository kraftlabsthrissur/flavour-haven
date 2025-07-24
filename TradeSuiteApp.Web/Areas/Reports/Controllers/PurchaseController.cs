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
using TradeSuiteApp.Web.Areas.Reports.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class PurchaseController : BaseReportController
    {
        private ReportsEntities dbEntity;
        private IDropdownContract _dropdown;
        private IDepartmentContract departmentBL;
        private IUserContract userBL;
        private ItemContract itemBL;
        private ILocationContract locationBL;
        private IReportContract reportBL;
        private ITreasuryContract treasuryBL;
        private ILocalPurchaseInvoiceContract LocalPurchaseInvoiceBL;
        private IMilkPurchase milkPurchaseBL;
        private IStatusContract statusBL;
        private IAddressContract addressBL;
        private IInterCompanyPurchaseInvoiceContract interCompanyBL;
        private IPurchaseOrder purchaseOrderBL;
        private IServicePurchaseOrderContract servicePurchaseOrderBL;
        private IPurchaseInvoice purchaseInvoiceBL;
        private IPurchaseReturnOrderContract purchaseReturnBL;
        private IGoodsReceiptNoteContract goodsReceiptNoteBL;

        private SelectList AtoZRange = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());

        private DateTime StartDate, EndDate, DateAsOn;
        private DateTime? POFromDate, POToDate, InvoiceDateFrom, InvoiceDateTo, GRNFromDate, GRNToDate, TransDateFrom, TransDateTo;
        private int purchaseRetunNoFromId, purchaseRetunNoTOId, SupplierID, ItemID, GRNNOFromID, GRNNOToID, QCNOFromID, QCNOToID;

        public PurchaseController(IDropdownContract dropdown)
        {
            _dropdown = dropdown;
            userBL = new UserBL();
            itemBL = new ItemBL();
            locationBL = new LocationBL();
            reportBL = new ReportBL();
            departmentBL = new DepartmentBL();
            statusBL = new StatusBL();
            dbEntity = new ReportsEntities();
            AtoZRange = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());

            ViewBag.FinStartDate = GeneralBO.FinStartDate;
            ViewBag.CurrentDate = General.FormatDate(DateTime.Now);
            treasuryBL = new TreasuryBL();
            LocalPurchaseInvoiceBL = new LocalPurchaseInvoiceBL();
            milkPurchaseBL = new MilkPurchaseBL();
            interCompanyBL = new InterCompanyPurchaseInvoiceBL();
            addressBL = new AddressBL();
            purchaseOrderBL = new PurchaseOrderBL();
            servicePurchaseOrderBL = new ServicePurchaseOrderBL();
            purchaseInvoiceBL = new PurchaseInvoiceBL();
            purchaseReturnBL = new PurchaseReturnOrderBL();
            goodsReceiptNoteBL = new GoodsReceiptNoteBL();
        }
        // GET: Reports/Purchase/PurchaseRequisition  
        [HttpGet]
        public ActionResult PurchaseRequisition()
        {
            ReportViewModel rep = new ReportViewModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.FinStartDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            rep.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            rep.StatusList = new SelectList(statusBL.GetStatusList("PurchaseRequisitionReport"), "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            rep.PRDateFrom = General.ToDateTime(GeneralBO.FinStartDate);
            rep.PRDateTo = Convert.ToDateTime(DateTime.Now);
            rep.ToDepartmentFromRangeList = AtoZRange;
            rep.ToDepartmentToRangeList = AtoZRange;
            rep.FromItemCategoryRangeList = AtoZRange;
            rep.ToItemCategoryRangeList = AtoZRange;
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.UserFromRangeList = AtoZRange;
            rep.UserToRangeList = AtoZRange;
            return View(rep);
        }
        public JsonResult GetCategory(string Type)
        {
            ReportViewModel rep = new ReportViewModel();
            if (Type == "Stock")
            {
                rep.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            }
            else
            {
                rep.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
            }
            return Json(new { Status = "success", data = rep.ItemCategoryList }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetToDepartmentRange(string from_range)
        {
            ReportViewModel rep = new ReportViewModel();
            if (from_range == "")
            {
                rep.ToDepartmentToRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.ToDepartmentToRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = rep.ToDepartmentToRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserRange(string from_range)
        {
            ReportViewModel rep = new ReportViewModel();
            if (from_range == "")
            {
                rep.UserToRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.UserToRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = rep.UserToRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDocumentRange(string from_range)
        {
            SelectList DocumentRangeList;
            if (from_range == "")
            {
                DocumentRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                DocumentRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = DocumentRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemRange(string from_range)
        {
            ReportViewModel rep = new ReportViewModel();
            if (from_range == "")
            {
               
                rep.FromItemNameRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.FromItemNameRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = rep.FromItemNameRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MISGetItemRange(string from_range)
        {
            MISViewModel rep = new MISViewModel();
            if (from_range == "")
            {
                rep.FromItemNameRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.FromItemNameRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = rep.FromItemNameRangeList }, JsonRequestBehavior.AllowGet);
        }

        private string GetFilterValues(List<string> PNames, List<string> PValues)
        {
            var names = "";
            var i = 0;
            foreach (string name in PValues)
            {
                if (name != null)
                {
                    var value = PNames[i];
                    names += " " + value + " : " + name + " ";
                }
                i++;
            }
            return names;
        }
        [HttpPost]
        public ActionResult PurchaseRequisition(ReportViewModel model)
        {
            model.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            //if (model.PRDateFrom != null)
            //    PRFromDate = General.ToDateTime(model.PRDateFrom);
            //if (model.PRDateTo != null)
            //    PRToDate = General.ToDateTime(model.PRDateTo);
            if (model.ToDepartmentID != null)//priority is for supplier id so item range is set to null
            {
                model.ToDepartmentFromRange = null;
                model.ToDepartmentToRange = null;
            }
            if (model.ItemID != null)
            {
                model.FromItemNameRange = null;
                model.ToItemNameRange = null;
            }
            if (model.UserID != null)
            {
                model.FromUserRange = null;
                model.ToUserRange = null;
            }
            if (model.ItemCategoryID != null)
            {
                model.FromItemCategoryRange = null;
                model.ToItemCategoryRange = null;
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Purchase Requisition " + model.Type + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            var PurchaseRequsition = dbEntity.SpRptPurchaseRequisition(
                                                StartDate,
                                                EndDate,
                                                model.Type,
                                                model.PRNOFromID,
                                                model.PRNOToID,
                                                model.ToDepartmentFromRange,
                                                model.ToDepartmentToRange,
                                                model.ToDepartmentID,
                                                model.FromItemCategoryRange,
                                                model.ToItemCategoryRange,
                                                model.ItemCategoryID,
                                                model.FromItemNameRange,
                                                model.ToItemNameRange,
                                                model.FromUserRange,
                                                model.ToUserRange,
                                                model.ItemID,
                                                model.UserID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();
            if (model.Type == "Stock")
            {
                if (model.Summary == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseRequisitionSummaryForStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseRequisitionSummaryForStockItemsDataSet", PurchaseRequsition));
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseRequisitionDetailForStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseRequisitionStockItemsDataSet", PurchaseRequsition));
                }
            }
            else
            {
                if (model.Summary == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseRequisitionSummaryForNonStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseRequisitionSummaryForStockItemsDataSet", PurchaseRequsition));
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseRequisitionDetailForNonStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseRequisitionStockItemsDataSet", PurchaseRequsition));
                }

            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, GSTNoParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public JsonResult GetItemsForAutoComplete(string Hint = "", string Areas = "")
        {
            List<ItemBO> itemList = new List<ItemBO>();
            if (Areas == "Stock")
            {
                itemList = itemBL.GetStockableItemsForAutoComplete(Hint).Select(a => new ItemBO()
                {
                    ID = a.ID,
                    Name = a.Name,
                }).ToList();
            }
            else if (Areas == "All")
            {
                itemList = itemBL.GetAllItemsForAutoComplete(Hint).Select(a => new ItemBO()
                {
                    ID = a.ID,
                    Name = a.Name,
                }).ToList();
            }
            else
            {
                itemList = itemBL.GetServiceItems(Hint).Select(a => new ItemBO()
                {
                    ID = a.ID,
                    Name = a.Name,
                }).ToList();
            }

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStockItemsForAutoComplete(string Hint = "")
        {
            List<ItemBO> itemList = new List<ItemBO>();
            itemList = itemBL.GetStockableItemsForAutoComplete(Hint).Select(a => new ItemBO()
            {
                ID = a.ID,
                Name = a.Name,
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceItemsForAutoComplete(string Hint = "")
        {
            List<ItemBO> itemList = new List<ItemBO>();
            itemList = itemBL.GetServiceItems(Hint).Select(a => new ItemBO()
            {
                ID = a.ID,
                Name = a.Name,
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemCategory(string Type)
        {
            List<ItemCategoryBO> StockItemCategories = _dropdown.GetItemCategoryList();
            List<ItemCategoryBO> ServiceItemCategories = _dropdown.GetItemCategoryForService();
            List<ItemCategoryBO> AllItemCategories = StockItemCategories.Concat(ServiceItemCategories).ToList();
            switch (Type)
            {
                case "Stock": return Json(new { Status = "success", data = StockItemCategories }, JsonRequestBehavior.AllowGet);
                case "Service": return Json(new { Status = "success", data = ServiceItemCategories }, JsonRequestBehavior.AllowGet);
                default: return Json(new { Status = "success", data = AllItemCategories }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetItemsByCategory(int? CategoryID)
        {
            try
            {
                List<ItemBO> Items = _dropdown.GetItemList("", (int)CategoryID, 0);
                return Json(new { Status = "success", data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        // GET: Reports/Purchase/PurchaseOrder
        public ActionResult PurchaseOrder()
        {
            ReportViewModel rep = new ReportViewModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            rep.StatusList = new SelectList(statusBL.GetStatusList("PurchaseOrderReport"), "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            rep.FromSupplierRangeList = AtoZRange;
            rep.ToSupplierRangeList = AtoZRange;
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.PODateFrom = GeneralBO.FinStartDate;
            rep.PODateTo = General.FormatDate(DateTime.Now);
            return View(rep);
        }

        public JsonResult GetRange(string from_range)
        {
            ReportViewModel rep = new ReportViewModel();
            if (from_range == "")
            {
                rep.FromSupplierRangeList = AtoZRange;
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.FromSupplierRangeList = AtoZRange;
            }
            return Json(new { Status = "success", data = rep.FromSupplierRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemCategoryRange(string from_range)
        {
            ReportViewModel rep = new ReportViewModel();
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

        [HttpPost]
        public ActionResult PurchaseOrder(ReportViewModel model)
        {
            var POType = "";
            model.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            model.StatusList = new SelectList(
                                 new List<SelectListItem>
                                 {
                                                new SelectListItem { Text = "All", Value = "All"},
                                                new SelectListItem { Text = "Processed", Value = "Processed"},
                                                new SelectListItem { Text = "Partial", Value ="Partial"},
                                                new SelectListItem { Text = "Pending", Value = "Pending"},
                                 }, "Value", "Text");
            if (model.FromDate != null)
            {
                StartDate = General.ToDateTime(model.FromDate);
                POFromDate = General.ToDateTime(model.FromDate);
            }
            if (model.ToDate != null)
            {
                EndDate = General.ToDateTime(model.ToDate);
                POToDate = General.ToDateTime(model.ToDate);
            }
            //if(model.PRDateFrom!=null)
            //{
            //    StartDate = General.ToDateTime(model.FromDate);
            //}
            //if(model.PRDateTo!=null)
            //{
            //    EndDate = General.ToDateTime(model.ToDate);
            //}
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Purchase Order" + " " + model.Type + " Items " + model.Summary + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            if (model.ItemID != null)//priority is for item id so item range is set to null
            {
                model.FromItemNameRange = null;
                model.ToItemNameRange = null;
            }
            if (model.SupplierID != null)//priority is for supplier id so item range is set to null
            {
                model.FromSupplierRange = null;
                model.ToSupplierRange = null;
            }
            if (model.PONOFromID == 0)//null value is assigned to ID's inorder to skip null value assign in sp
                model.PONOFromID = null;
            if (model.PONOToID == 0)
                model.PONOToID = null;
            if (model.PRNOFromID == 0)
                model.PRNOFromID = null;
            if (model.PRNOToID == 0)
                model.PRNOToID = null;
            if (model.Type == "Stock")
            {
                POType = "Stock";
                if (model.Summary == "Summary")
                {
                    var PurchaseOrder = dbEntity.SpRptPurchaseOrder(
                        StartDate,
                        EndDate,
                        POType,
                        model.PONOFromID,
                        model.PONOToID,
                        model.PRNOFromID,
                        model.PRNOToID,
                        model.FromSupplierRange,
                        model.ToSupplierRange,
                        model.SupplierID,
                        model.ItemCategoryID,
                        model.FromItemNameRange,
                        model.ToItemNameRange,
                        model.ItemID,
                        model.PRDateFrom,
                        model.PRDateTo,
                        POFromDate,
                        EndDate,
                        model.UserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.DisplayName = "PurchaseOrderSummary";
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseOrderSummaryForStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderSummaryForStockItemDataSet", PurchaseOrder));

                }

                else if (model.Summary == "Detail")
                {
                    if (model.PONOFromID != null)
                    {
                        //if PO no is selected then report focus on PO no insted of dates selected so date is set to default values
                        StartDate = General.ToDateTime(GeneralBO.FinStartDate);
                        EndDate = DateTime.Today;
                    }
                    ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
                    if (model.ReportType == "Supplier Wise")
                    {
                        var PurchaseOrder = dbEntity.SpRptPurchaseOrder(
                            POFromDate,
                            POToDate,
                            POType,
                            model.PONOFromID,
                            model.PONOToID,
                            model.PRNOFromID,
                            model.PRNOToID,
                            model.FromSupplierRange,
                            model.ToSupplierRange,
                            model.SupplierID,
                            model.ItemCategoryID,
                            model.FromItemNameRange,
                            model.ToItemNameRange,
                            model.ItemID,
                            model.PRDateFrom,
                            model.PRDateTo,
                            POFromDate,
                            POToDate,
                            model.UserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseOrderDetailSupplierWiseStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderForStockItemsDataSet", PurchaseOrder));

                    }
                    else if (model.ReportType == "Item Wise")
                    {
                        var PurchaseOrder = dbEntity.SpRptPurchaseOrder(
                            StartDate,
                            EndDate,
                            POType,
                            model.PONOFromID,
                            model.PONOToID,
                            model.PRNOFromID,
                            model.PRNOToID,
                            model.FromSupplierRange,
                            model.ToSupplierRange,
                            model.SupplierID,
                            model.ItemCategoryID,
                            model.FromItemNameRange,
                            model.ToItemNameRange,
                            model.ItemID,
                            model.PRDateFrom,
                            model.PRDateTo,
                            POFromDate, POToDate,
                            model.UserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseOrderDetailItemWiseStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderDetailItemWiseDataSet", PurchaseOrder));

                    }
                }
            }
            else if (model.Type == "Service")
            {
                POType = "Service";
                if (model.Summary == "Summary")
                {
                    var PurchaseOrder = dbEntity.SpRptPurchaseOrder(
                        StartDate,
                        EndDate,
                        POType,
                        model.PONOFromID,
                        model.PONOToID,
                        model.PRNOFromID,
                        model.PRNOToID,
                        model.FromSupplierRange,
                        model.ToSupplierRange,
                        model.SupplierID,
                        model.ItemCategoryID,
                        model.FromItemNameRange,
                        model.ToItemNameRange,
                        model.ItemID,
                        model.PRDateFrom,
                        model.PRDateTo,
                        POFromDate,
                        POToDate,
                        model.UserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.@ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseOrderSummaryForNonStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderSummaryForStockItemDataSet", PurchaseOrder));


                }
                else if (model.Summary == "Detail")
                {
                    if (model.PONOFromID != null)
                    {
                        //if PO no is selected then report focus on PO no insted of dates selected so date is set to default values
                        //StartDate = General.ToDateTime(GeneralBO.FinStartDate);
                        //POFromDate = General.ToDateTime(GeneralBO.FinStartDate);
                        //EndDate = DateTime.Today;
                        //POToDate = DateTime.Today;
                        StartDate = General.ToDateTime(GeneralBO.FinStartDate);
                        EndDate = DateTime.Today;
                    }
                    if (model.ReportType == "Supplier Wise")
                    {
                        var PurchaseOrder = dbEntity.SpRptPurchaseOrder(
                            POFromDate,
                            POToDate,
                            POType,
                            model.PONOFromID,
                            model.PONOToID,
                            model.PRNOFromID,
                            model.PRNOToID,
                            model.FromSupplierRange,
                            model.ToSupplierRange,
                            model.SupplierID,
                            model.ItemCategoryID,
                            model.FromItemNameRange,
                            model.ToItemNameRange,
                            model.ItemID,
                            model.PRDateFrom,
                            model.PRDateTo,
                            POFromDate,
                            POToDate,
                           model.UserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseOrderDetailSupplierWiseNonStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderForStockItemsDataSet", PurchaseOrder));

                    }
                    else if (model.ReportType == "Item Wise")
                    {
                        var PurchaseOrder = dbEntity.SpRptPurchaseOrder(
                            StartDate,
                            EndDate,
                            POType,
                            model.PONOFromID,
                            model.PONOToID,
                            model.PRNOFromID,
                            model.PRNOToID, model.
                            FromSupplierRange,
                            model.ToSupplierRange,
                            model.SupplierID,
                            model.ItemCategoryID,
                            model.FromItemNameRange,
                            model.ToItemNameRange,
                            model.ItemID,
                            model.PRDateFrom,
                            model.PRDateTo,
                            POFromDate, POToDate,
                            model.UserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseOrderDetailItemWiseNonStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderDetailItemWiseDataSet", PurchaseOrder));

                    }
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //GET: Reports/PurchaseInvoice
        public ActionResult PurchaseInvoice()
        {
            ReportViewModel rep = new ReportViewModel();
            rep.FromDate = General.FormatDate(General.FirstDayOfMonth);
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.FromItemCategoryRangeList = AtoZRange;
            rep.ToItemCategoryRangeList = AtoZRange;
            rep.FromSupplierRangeList = AtoZRange;
            rep.ToSupplierRangeList = AtoZRange;
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            rep.StatusList = new SelectList(statusBL.GetStatusList("PurchaseInvoiceReport"), "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        //public ActionResult GetItemCategoryRange(string from_range)
        //{
        //    char range = Convert.ToChar(from_range);
        //    ReportViewModel rep = new ReportViewModel();
        //    rep.ToItemCategoryRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
        //    return Json(new { Status = "success", data = rep.ToItemCategoryRangeList }, JsonRequestBehavior.AllowGet);
        //}

        private SelectList GetStatusList(string Type)
        {
            if (Type == "PurchaseRequisition")
            {
                new SelectList(
                                  new List<SelectListItem>
                                  {
                                                new SelectListItem { Text = "All", Value = "All"},
                                                new SelectListItem { Text = "Processed", Value = "Processed"},
                                                new SelectListItem { Text = "Partial", Value ="Partial"},
                                                new SelectListItem { Text = "Pending", Value = "Pending"},
                                  }, "Value", "Text");
            }
            else if (Type == "PurchaseInvoice")
            {
                return new SelectList(
                                     new List<SelectListItem>
                                     {
                                                   new SelectListItem { Text = "All", Value = "All"},
                                                   new SelectListItem { Text = "Approved", Value = "Approved"},
                                                   new SelectListItem { Text = "Booked", Value = "Booked"},
                                                   new SelectListItem { Text = "Processed", Value = "Processed"},
                                                   new SelectListItem { Text = "Partial", Value ="Partial"},
                                                   new SelectListItem { Text = "Pending", Value = "Pending"},

                                      }, "Value", "Text");
            }
            return new SelectList(new List<SelectListItem>());
        }
        // Post: Reports/PurchaseInvoice
        [HttpPost]
        public ActionResult PurchaseInvoice(ReportViewModel model)
        {
            model.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            model.StatusList = GetStatusList("PurchaseInvoice");
            if (model.FromDate != null)
            {
                StartDate = General.ToDateTime(model.FromDate);
                InvoiceDateFrom = General.ToDateTime(model.FromDate);
            }
            if (model.ToDate != null)
            {
                EndDate = General.ToDateTime(model.ToDate);
                InvoiceDateTo = General.ToDateTime(model.ToDate);
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Purchase Invoice" + " " + model.Summary + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            if (model.ItemID != null)
            {
                model.FromItemNameRange = null;
                model.ToItemNameRange = null;
            }
            if (model.SupplierID != null)
            {
                model.FromSupplierRange = null;
                model.ToSupplierRange = null;
            }
            if (model.ItemCategoryID != null)
            {
                model.FromItemCategoryRange = null;
                model.ToItemCategoryRange = null;
            }
            if (model.PONOFromID == 0)
                model.PONOFromID = null;
            if (model.PONOToID == 0)
                model.PONOToID = null;
            if (model.QCNOFromID == 0)
                model.QCNOFromID = null;
            if (model.QCNOToID == 0)
                model.QCNOToID = null;
            if (model.GRNNOFromID == 0)
                model.GRNNOFromID = null;
            if (model.GRNNOToID == 0)
                model.GRNNOToID = null;
            if (model.InvoiceNOFromID == 0)
                model.InvoiceNOFromID = null;
            if (model.InvoiceNOToID == 0)
                model.InvoiceNOToID = null;
            if (model.SRNNOFromID == 0)
                model.SRNNOFromID = null;
            if (model.SRNNOToID == 0)
                model.SRNNOToID = null;
            if (model.Type == "Stock")
            {
                if (model.Summary == "Summary")
                {
                    var stock = dbEntity.SpRptPurchaseInvoiceSupplierWiseSummary(
                                           InvoiceDateFrom,
                                           InvoiceDateTo,
                                           model.Type,
                                           model.FromSupplierRange,
                                           model.ToSupplierRange,
                                           model.SupplierID,
                                           model.InvoiceNOFromID,
                                           model.InvoiceNOToID,
                                           model.SupplierInvoiceNO,
                                           model.InvoiceStatus,
                                           model.IsOverruled,
                                           GeneralBO.CreatedUserID,
                                           GeneralBO.FinYear,
                                           GeneralBO.LocationID,
                                           GeneralBO.ApplicationID).ToList();
                    ReportNameParam = new ReportParameter("ReportName", "Purchase Invoice" + " " + model.Summary);
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceSummaryForStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceWithoutItemWiseSummaryDataSet", stock));
                }

                else /*if (model.Summary == "Summary" && model.ReportType != "without-item-wise")*/
                {
                    var stock = dbEntity.SpRptPurchaseInvoice(
                                                InvoiceDateFrom,
                                                InvoiceDateTo,
                                                model.Type,
                                                model.FromItemCategoryRange,
                                                model.ToItemCategoryRange,
                                                model.ItemCategoryID,
                                                model.FromItemNameRange,
                                                model.ToItemNameRange,
                                                model.ItemID,
                                                model.FromSupplierRange,
                                                model.ToSupplierRange,
                                                model.SupplierID,
                                                model.PONOFromID,
                                                model.PONOToID,
                                                model.QCNOFromID,
                                                model.QCNOToID,
                                                model.GRNNOFromID,
                                                model.GRNNOToID,
                                                model.InvoiceNOFromID,
                                                model.InvoiceNOToID,
                                                model.SupplierInvoiceNO,
                                                model.InvoiceStatus,
                                                GeneralBO.CreatedUserID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();

                    if (model.ReportType == "item-wise")
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceItemWiseSummaryForStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceItemWiseSummaryStockItemsDataSet", stock));
                    }

                    else
                    {
                        ReportNameParam = new ReportParameter("ReportName", "Purchase Invoice" + " " + model.Summary + " Billwise ");
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceDetailsForStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceItemWiseSummaryStockItemsDataSet", stock));
                        //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceDetailsStockItemsDataSet", stock));
                    }
                }
            }
            else
            {
                var service = dbEntity.SpRptPurchaseInvoice(
                                InvoiceDateFrom,
                                InvoiceDateTo,
                                model.Type,
                                model.FromItemCategoryRange,
                                model.ToItemCategoryRange,
                                model.ItemCategoryID,
                                model.FromItemNameRange,
                                model.ToItemNameRange,
                                model.ItemID,
                                model.FromSupplierRange,
                                model.ToSupplierRange,
                                model.SupplierID,
                                model.PONOFromID,
                                model.PONOToID,
                                model.QCNOFromID,
                                model.QCNOToID,
                                model.SRNNOFromID,
                                model.SRNNOToID,
                                model.InvoiceNOFromID,
                                model.InvoiceNOToID,
                                model.SupplierInvoiceNO,
                                model.InvoiceStatus,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID).ToList();
                if (model.Summary == "Summary")
                {
                    if (model.ReportType == "without-item-wise")
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceSummaryForNonStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceWithoutItemWiseSummaryForNonStockItemsDataSet", service));
                    }
                    else if (model.ReportType == "item-wise")
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceItemWiseSummaryForNonStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceItemWiseSummaryForNonStockItemsDataSet", service));
                    }
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceDetailsForNonStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceDetailsStockItemsDataSet", service));
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        //GET: Reports/GRN
        public ActionResult GRN()
        {
            ReportViewModel rep = new ReportViewModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            List<ItemCategoryBO> StockItemCategories = _dropdown.GetItemCategoryList();
            rep.ItemCategoryList = new SelectList(StockItemCategories, "ID", "Name");
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            rep.FromDate = GeneralBO.FinStartDate;
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.UserFromRangeList = AtoZRange;
            rep.UserToRangeList = AtoZRange;
            rep.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            rep.PODateFrom = GeneralBO.FinStartDate;
            rep.PODateTo = General.FormatDate(DateTime.Now);
            rep.PRDateFrom = General.ToDateTime(GeneralBO.FinStartDate);
            rep.PRDateTo = General.ToDateTime(General.FormatDate(DateTime.Now));
            rep.StatusList = new SelectList(
                                new List<SelectListItem>
                                {
                                                new SelectListItem { Text = "All", Value = "All"},
                                                new SelectListItem { Text = "Completed", Value = "Completed"},
                                                new SelectListItem { Text = "Partial", Value ="Partial"},
                                                new SelectListItem { Text = "Not Completed", Value = "Not Completed"},

                                }, "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult GRN(ReportViewModel model)
        {
            model.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.PODateFrom != null)
                POFromDate = General.ToDateTime(model.PODateFrom);
            if (model.PODateTo != null)
                POToDate = General.ToDateTime(model.PODateTo);
            if (model.PONOFromID == 0)
                model.PONOFromID = null;
            if (model.PONOToID == 0)
                model.PONOToID = null;
            if (model.GRNNOFromID == 0)
                model.GRNNOFromID = null;
            if (model.GRNNOToID == 0)
                model.GRNNOToID = null;
            if (model.SRNNOFromID == 0)
                model.SRNNOFromID = null;
            if (model.SRNNOToID == 0)
                model.SRNNOToID = null;
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Goods Receipt Note" + " " + model.Type + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            model.StatusList = new SelectList(
                                               new List<SelectListItem>
                                               {
                                                new SelectListItem { Text = "All", Value = "All"},
                                                new SelectListItem { Text = "Completed", Value = "Completed"},
                                                new SelectListItem { Text = "Partial", Value ="Partial"},
                                                new SelectListItem { Text = "Not Completed", Value = "Not Completed"},
                                               }, "Value", "Text");
            var Grn = dbEntity.SpRptGRNAndSRN(
                                StartDate,
                                EndDate,
                                model.Type,
                                model.GRNNOFromID,
                                model.GRNNOToID,
                                model.ItemCategoryID,
                                model.FromItemNameRange,
                                model.ToItemNameRange,
                                model.PONOFromID,
                                model.PONOToID,
                                POFromDate,
                                POToDate,
                                model.ItemID,
                                model.SupplierID,
                                model.SupplierInvoiceNO,
                                model.FromUserRange,
                                model.ToUserRange,
                                model.UserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID).ToList();
            if (model.Type == "Stock")
            {
                if (model.Summary == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/GRNSummaryForStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GRNDetailStockItemDataSet", Grn));
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/GRNDetailForStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GRNStockitemsDataSet", Grn));
                }
            }
            else
            {
                var Srn = dbEntity.SpRptGRNAndSRN(
                               StartDate,
                               EndDate,
                               model.Type,
                               model.SRNNOFromID,
                               model.SRNNOToID,
                               model.ItemCategoryID,
                               model.FromItemNameRange,
                               model.ToItemNameRange,
                               model.PONOFromID,
                               model.PONOToID,
                               POFromDate,
                               POToDate,
                               model.ItemID,
                               model.SupplierID,
                               model.SupplierInvoiceNO,
                               model.FromUserRange,
                               model.ToUserRange,
                               model.UserID,
                               GeneralBO.FinYear,
                               GeneralBO.LocationID,
                               GeneralBO.ApplicationID).ToList();
                if (model.Summary == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/SRNSummaryForNonStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SRNDetailNonStockItemsDataSet", Srn));
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/SRNDetailsForNonStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SRNDetailNonStockItemsDataSet", Srn));
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }
        [HttpGet]
        //GET: Reports/QualityCheck
        public ActionResult QualityCheck()
        {
            ReportViewModel rep = new ReportViewModel();
            rep.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            rep.StatusList = new SelectList(statusBL.GetStatusList("QualityCheckReport"), "Value", "Text");

            rep.ItemTypeList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "All", Value = "All"},
                                                new SelectListItem { Text = "Purchased", Value = "Purchased"},
                                                new SelectListItem { Text = "Produced", Value ="Produced"},
                                    }, "Value", "Text");
            //rep.FromLoginNameRangeList = AtoZRange
            //rep.ToLoginNameRangeList = AtoZRange
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.UserFromRangeList = AtoZRange;
            rep.UserToRangeList = AtoZRange;
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        //POST : Reports/QualityCheck
        public ActionResult QualityCheck(ReportViewModel model)
        {
            //model.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            //model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            //model.FromLoginNameRangeList = AtoZRange;
            //model.ToLoginNameRangeList = AtoZRange;
            if (model.FromDate != null)
            {
                StartDate = General.ToDateTime(model.FromDate);
            }
            if (model.ToDate != null)
            {
                EndDate = General.ToDateTime(model.ToDate);
            }
            if (model.PODateFrom != null)
            {
                POFromDate = General.ToDateTime(model.PODateFrom);
            }
            if (model.PODateTo != null)
            {
                POToDate = General.ToDateTime(model.PODateTo);
            }
            if (model.GRNFromDate != null)
            {
                GRNFromDate = General.ToDateTime(model.GRNFromDate);
            }
            if (model.GRNToDate != null)
            {
                GRNToDate = General.ToDateTime(model.GRNToDate);
            }
            if (model.ItemID != null)
            {
                model.FromItemNameRange = null;
                model.ToItemNameRange = null;
            }
            if (model.PONOFromID == 0)
                model.PONOFromID = null;
            if (model.PONOToID == 0)
                model.PONOToID = null;
            if (model.QCNOFromID == 0)
                model.QCNOFromID = null;
            if (model.QCNOToID == 0)
                model.QCNOToID = null;
            if (model.GRNNOFromID == 0)
                model.GRNNOFromID = null;
            if (model.GRNNOToID == 0)
                model.GRNNOToID = null;
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Quality Assurance For Stock Items" + " " + model.Type + " " + model.Summary + " " + model.QcType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            var QualityCheck = dbEntity.SpRptPurchaseItemQCDetails(
                                            StartDate,
                                            EndDate,
                                            model.ItemType,
                                            model.Status,
                                            model.QCNOFromID,
                                            model.QCNOToID,
                                            model.FromItemNameRange,
                                            model.ToItemNameRange,
                                            model.ItemID,
                                            model.PONOFromID,
                                            model.PONOToID,
                                            POFromDate,
                                            POToDate,
                                            GRNFromDate,
                                            GRNToDate,
                                            model.GRNNOFromID,
                                            model.GRNNOToID,
                                            model.FromUserRange,
                                            model.ToUserRange,
                                            model.UserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
            if (model.Summary == "Summary")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/QcAllSummaryRpt.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AyurwareQCDataSet", QualityCheck));
            }
            else if (model.Summary == "Detail")
            {
                if (model.QcType == "QCAssurance")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/QualityApprovedGRNForStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AyurwareQCDataSet", QualityCheck));
                }
                else if (model.QcType == "QCTest")
                {
                    var QualityCheckTest = dbEntity.SpRptQCTestForStockItems(
                                              StartDate,
                                              EndDate,
                                              model.ItemType,
                                              model.Status,
                                              model.QCNOFromID,
                                              model.QCNOToID,
                                              model.FromItemNameRange,
                                              model.ToItemNameRange,
                                              model.ItemID,
                                              model.PONOFromID,
                                              model.PONOToID,
                                              POFromDate,
                                              POToDate,
                                              GRNFromDate,
                                              GRNToDate,
                                              model.GRNNOFromID,
                                              model.GRNNOToID,
                                              model.FromUserRange,
                                              model.ToUserRange,
                                              model.UserID,
                                              GeneralBO.FinYear,
                                              GeneralBO.LocationID,
                                              GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/QualityTestForStockItemDetails.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("QualityTestDetailForStockItemsDataSet", QualityCheckTest));
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        // GET: Reports/Purchase/MilkPurchaseReport
        public ActionResult MilkPurchase()
        {
            ReportViewModel rep = new ReportViewModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        // POST: Reports/Purchase/MilkPurchaseReport
        [HttpPost]
        public ActionResult MilkPurchase(ReportViewModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Milk Purchase" + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            var MilkPurchase = dbEntity.SpRptMilkPurchase(
                                            StartDate,
                                            EndDate,
                                            model.SupplierID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
            if (model.Summary == "Summary")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MilkPurchaseSummary.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MilkPurchaseDataSet", MilkPurchase));
            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MilkPurchase.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MilkPurchaseDataSet", MilkPurchase));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //GET: Reports/Purchase/PurchaseReturn
        public ActionResult PurchaseReturn()
        {
            ReportViewModel rep = new ReportViewModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.UserID = GeneralBO.CreatedUserID;
            rep.FromSupplierRangeList = AtoZRange;
            rep.ToSupplierRangeList = AtoZRange;
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        //POST: Reports/Purchase/PurchaseReturn
        [HttpPost]
        public ActionResult PurchaseReturn(ReportViewModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.purchaseRetunNoFromId != null)
                purchaseRetunNoFromId = (int)model.purchaseRetunNoFromId;
            if (model.purchaseRetunNoTOId != null)
                purchaseRetunNoTOId = (int)model.purchaseRetunNoTOId;
            if (model.SupplierID != null)
                SupplierID = (int)model.SupplierID;
            if (model.ItemID != null)
                ItemID = (int)model.ItemID;
            if (model.GRNNOFromID != null)
                GRNNOFromID = (int)model.GRNNOFromID;
            if (model.GRNNOToID != null)
                GRNNOToID = (int)model.GRNNOToID;
            if (model.QCNOFromID != null)
                QCNOFromID = (int)model.QCNOFromID;
            if (model.QCNOToID != null)
                QCNOToID = (int)model.QCNOToID;
            if (model.SupplierID != null)//priority is for supplier id so supplier range is set to null
            {
                model.FromSupplierRange = null;
                model.ToSupplierRange = null;
            }
            if (model.ItemID != null)
            {
                model.FromItemNameRange = null;
                model.ToItemNameRange = null;
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Purchase Retrun" + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            var PurchaseReturn = dbEntity.SpRptPurchaseReturn(
                                            StartDate,
                                            EndDate,
                                            model.purchaseRetunNoFromId,
                                            model.purchaseRetunNoTOId,
                                            model.SupplierID,
                                            model.ItemID,
                                            model.GRNNOFromID,
                                            model.GRNNOToID,
                                            model.QCNOFromID,
                                            model.QCNOToID,
                                            model.FromSupplierRange,
                                            model.ToSupplierRange,
                                            model.FromItemNameRange,
                                            model.ToItemNameRange,
                                            GeneralBO.CreatedUserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID
                                            ).ToList();
            if (model.Summary == "Summary")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseReturnSummaryForStockItems.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseReturnDataSet", PurchaseReturn));
            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseReturnDetailForStockItems.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseReturnDataSet", PurchaseReturn));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public ActionResult MISReport()
        {
            MISViewModel rep = new MISViewModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(General.FirstDayOfMonth);
            //rep.FromInvoiceDate = GeneralBO.FinStartDate;
            //rep.ToInvoiceDate = General.FormatDate(DateTime.Now);
            rep.FromTransDate = GeneralBO.FinStartDate;
            rep.ToTransDate = General.FormatDate(DateTime.Now);
            rep.DateAsOn = General.FormatDate(DateTime.Now);
            rep.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            rep.OutstandingRangeList = new SelectList(
                                 new List<SelectListItem>
                                 {
                                                new SelectListItem { Text = "Greater than", Value = "Greaterthan"},
                                                new SelectListItem { Text = "Less than", Value = "Lessthan"},
                                                new SelectListItem { Text = "Equal to", Value ="Equalto"},

                                 }, "Value", "Text");
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.SupplierRangeList = AtoZRange;
            rep.FromTransTypeRangeList = AtoZRange;
            rep.FromItemAccountsCategoryList = AtoZRange;
            rep.AccountNameFromList = AtoZRange;
            rep.StatusList = new SelectList(statusBL.GetStatusList("PurchaseMISReport"), "Value", "Text");
            //rep.AgeingBucketList = new SelectList(
            //                        new List<SelectListItem>
            //                        {
            //                            new SelectListItem { Text = "AB1", Value = "AB1"},
            //                            new SelectListItem { Text = "AB2", Value = "AB2"},
            //                            new SelectListItem { Text = "AB3", Value = "AB3"},
            //                            new SelectListItem { Text = "AB4", Value = "AB4"},
            //                            new SelectListItem { Text = "AB5", Value = "AB5"},
            //                        }, "Value", "Text");
            return View(rep);
        }

        [HttpPost]
        public ActionResult MISReport(MISViewModel model)
        {
            MISViewModel rep = new MISViewModel();
            if (model.DateAsOn != null)
                DateAsOn = General.ToDateTime(model.DateAsOn);
            if (model.FromInvoiceDate != null)
                InvoiceDateFrom = General.ToDateTime(model.FromInvoiceDate);
            if (model.ToInvoiceDate != null)
                InvoiceDateTo = General.ToDateTime(model.ToInvoiceDate);
            if (model.FromTransDate != null)
                TransDateFrom = General.ToDateTime(model.FromTransDate);
            if (model.ToTransDate != null)
                TransDateTo = General.ToDateTime(model.ToTransDate);
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.SupplierID != null)//priority is for supplier id so supplier range is set to null
            {
                model.FromSupplierRange = null;
                model.ToSupplierRange = null;
            }
            if (model.SupplierInvoiceNOID != null)
            {
                model.FromDocumentRange = null;
                model.ToDocumentRange = null;
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

            if (model.TransTypeID != null)//priority is for supplier id so item range is set to null
            {
                model.TransTypeFromRange = null;
                model.TransTypeToRange = null;
            }

            if (model.AccountNameID != null)//priority is for supplier id so item range is set to null
            {
                model.AccountNameFromRange = null;
                model.AccountNameToRange = null;
            }
            //if (model.AccountGroupID == 0)
            //    model.AccountGroupID = null;

            //if (model.AccountNameID == 0)
            //    model.AccountNameID = null;

            //if (model.AccountCodeFromID == 0)
            //    model.AccountCodeFromID = null;

            //if (model.AccountCodeToID == 0)
            //    model.AccountCodeToID = null;

            //if (model.EmployeeID == 0)
            if (model.ReportType == "Supplier-Ledger")
            {
                FromDateParam = new ReportParameter("FromDate", model.StartDate.ToShortDateString());
                ToDateParam = new ReportParameter("ToDate", model.EndDate.ToShortDateString());
            }
            else
            {
                FromDateParam = new ReportParameter("FromDate", model.TransDateFrom.ToShortDateString());
                ToDateParam = new ReportParameter("ToDate", model.TransDateTo.ToShortDateString());
            }
            ReportNameParam = new ReportParameter("ReportName", model.ReportType + " Report ");
            FilterParam = new ReportParameter("Filters", model.Filters);
            if (model.ReportType == "Supplier-Invoice")
            {
                var MISReport = dbEntity.SpRptSupplierInvoicesBalancePayableYesHistory(
                                                model.TransDateFrom,
                                                model.TransDateTo,
                                                InvoiceDateFrom,
                                                InvoiceDateTo,
                                                model.FromSupplierRange,
                                                model.ToSupplierRange,
                                                model.SupplierID,
                                                model.FromDocumentRange,
                                                model.ToDocumentRange,
                                                model.ItemType,
                                                model.SupplierInvoiceNO,
                                                model.InvoiceNOFromID,
                                                model.InvoiceNOToID,
                                                model.OutstandingDays,
                                                model.BalanceType,
                                                model.InvoiceStatus,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();
                //MISReport = MISReport.Where(a => a.TotalInvoiceValue <= a.PaidAmount).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/SupplierInvoicesHistoryBalancePayableYes.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierInvoicesHistoryBalancePayableYesDataSet", MISReport));
            }
            if (model.ReportType == "Supplier-Invoice" && model.BalanceType == "No")
            {
                var MISReport = dbEntity.SpRptSupplierInvoicesBalancePayableNoHistory(
                                                model.TransDateFrom,
                                                model.TransDateTo,
                                                InvoiceDateFrom,
                                                InvoiceDateTo,
                                                model.FromSupplierRange,
                                                model.ToSupplierRange,
                                                model.SupplierID,
                                                model.FromDocumentRange,
                                                model.ToDocumentRange,
                                                model.ItemType,
                                                model.SupplierInvoiceNO,
                                                model.InvoiceNOFromID,
                                                model.InvoiceNOToID,
                                                model.OutstandingDays,
                                                model.BalanceType,
                                                model.InvoiceStatus,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();
                //MISReport = MISReport.Where(a => a.TotalInvoiceValue <= a.PaidAmount).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/SupplierInvoicesHistoryBalancePayableNo.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierInvoicesHistoryBalancePayableNoDataSet", MISReport));
            }



            if (model.ReportType == "Purchase-History")
            {
                var MISReport = dbEntity.SpRptPurchasesHistory(
                                                    model.TransDateFrom,
                                                    model.TransDateTo,
                                                    InvoiceDateFrom,
                                                    InvoiceDateTo,
                                                    model.FromSupplierRange,
                                                    model.ToSupplierRange,
                                                    model.SupplierID,
                                                    model.FromDocumentRange,
                                                    model.ToDocumentRange,
                                                    model.ItemType,
                                                    model.SupplierInvoiceNOID,
                                                    model.InvoiceNOFromID,
                                                    model.InvoiceNOToID,
                                                    model.FromItemNameRange,
                                                    model.ToItemNameRange,
                                                    model.ItemID,
                                                    model.FromItemCategoryRange,
                                                    model.ToItemCategoryRange,
                                                    model.ItemCategoryID,
                                                    GeneralBO.FinYear,
                                                    GeneralBO.LocationID,
                                                    GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/PurchaseHistoryReport.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseHistoryDataSet", MISReport));
            }
            if (model.ReportType == "SLA")
            {
                var MISReport = dbEntity.SpSLADetails(
                                                    model.TransDateFrom,
                                                    model.TransDateTo,
                                                    model.FromTransTypeRange,
                                                    model.ToTransTypeRange,
                                                    model.TransType,
                                                    model.KeyValue,
                                                    model.FromItemAccountsCategory,
                                                    model.ToItemAccountsCategory,
                                                    model.ItemAccountCategory,
                                                    model.Status,
                                                    GeneralBO.FinYear,
                                                    GeneralBO.LocationID,
                                                    GeneralBO.ApplicationID
                                                    ).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/SLA.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SLADataSet", MISReport));
            }
            if (model.ReportType == "Supplier-Ledger")
            {
                var MISReport = new object();
                if (model.ReportDataType == "Summary")
                {
                    MISReport = dbEntity.SpRptSupplierSubLedgerSummary(
                                                    model.StartDate,
                                                    model.EndDate,
                                                    model.SupplierID,
                                                    GeneralBO.FinYear,
                                                    GeneralBO.LocationID,
                                                    GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/SupplierSubLedgerReportSummary.rdlc";
                }
                else
                {
                    MISReport = dbEntity.SpRptSupplierSubLedger(
                                                    model.StartDate,
                                                    model.EndDate,
                                                    model.SupplierID,
                                                    GeneralBO.FinYear,
                                                    GeneralBO.LocationID,
                                                    GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/SupplierSubLedgerReport.rdlc";
                }
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierSubLedgerReportDataSet", MISReport));
            }
            if (model.ReportType == "General-Ledger")
            {
                var MISReport = dbEntity.SpRptAccountLedger(
                                                    model.ReportType,
                                                    model.AccountGroupID,
                                                    model.AccountNameID,
                                                    model.StartDate,
                                                    model.EndDate,
                                                    model.DocumentType,
                                                    model.DocumentNo,
                                                    model.TransactionType,
                                                    model.AccountCodeFromID,
                                                    model.AccountCodeToID,
                                                    model.AccountNameFromRange,
                                                    model.AccountNameToRange,
                                                    model.LocationID,
                                                    model.DepartmentID,
                                                    model.EmployeeID,
                                                    model.InterCompanyID,
                                                    model.ProjectID,
                                                    GeneralBO.CreatedUserID,
                                                    GeneralBO.FinYear,
                                                    GeneralBO.LocationID,
                                                    GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/GeneralLedgerForAcNoRmPurchases.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AccountLedgerDataSet", MISReport));
            }
            if (model.ReportType == "Supplier-Ageing")
            {
                var MISReport = dbEntity.SpGetSupplierAgeing(
                                                           DateAsOn,
                                                           InvoiceDateFrom,
                                                           InvoiceDateTo,
                                                           model.TransDateFrom,
                                                           model.TransDateTo,
                                                           model.FromSupplierRange,
                                                           model.ToSupplierRange,
                                                           model.SupplierID,
                                                           model.SupplierInvoiceNO,
                                                           model.InvoiceNOFromID,
                                                           model.InvoiceNOToID,
                                                           model.OutstandingDays,
                                                           model.BalancePayableOnly,
                                                           model.AgeingBucketID,
                                                           GeneralBO.FinYear,
                                                           GeneralBO.LocationID,
                                                           GeneralBO.ApplicationID
                                                           ).ToList();
                if (model.AgeingBucketID > 0)
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/SupplierAgeingAB" + model.AgeingBucketID + ".rdlc";
                }
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SupplierAgeingAB1DataSet", MISReport));

            }
            if (model.ReportType == "Overruled-Items")
            {
                var MISReport = dbEntity.SpGetOverruledItems(
                                                           model.TransDateFrom,
                                                           model.TransDateTo,
                                                           GeneralBO.FinYear,
                                                           GeneralBO.LocationID,
                                                           GeneralBO.ApplicationID
                                                           ).ToList();
                if (model.AgeingBucketID > 0)
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/OverruledItems.rdlc";
                }
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("OverruledItemsDataSet", MISReport));

            }
            if (model.ReportType == "ItemListFreeOfferWise")
            {
                var MISReport = dbEntity.spGetItemListByOffer(
                                                           model.TransDateFrom,
                                                           model.TransDateTo,
                                                           GeneralBO.FinYear,
                                                           GeneralBO.LocationID,
                                                           GeneralBO.ApplicationID
                                                           ).ToList();
                if (model.AgeingBucketID > 0)
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/ItemListByOffer.rdlc";
                }
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemListByOfferDataSet", MISReport));

            }
            if (model.ReportType == "ItemListByNonOffer")
            {
                var MISReport = dbEntity.spGetItemListByNonOffer(
                                                           model.TransDateFrom,
                                                           model.TransDateTo,
                                                           GeneralBO.FinYear,
                                                           GeneralBO.LocationID,
                                                           GeneralBO.ApplicationID
                                                           ).ToList();
                if (model.AgeingBucketID > 0)
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/MISReports/ItemListByNonOffer.rdlc";
                }
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemListByNonOfferDataSet", MISReport));

            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        public ActionResult PurchaseOrderStatus()
        {
            PurchaseOrderStatusReportModel rep = new PurchaseOrderStatusReportModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.StatusList = new SelectList(statusBL.GetStatusList("PurchaseOrderStatusReport"), "Value", "Text");
            rep.LocationID = GeneralBO.LocationID;
            rep.ItemLocationID = GeneralBO.LocationID;
            //rep.UserID = GeneralBO.CreatedUserID;
            //rep.FromSupplierRangeList = AtoZRange;
            //rep.ToSupplierRangeList = AtoZRange;
            //rep.FromItemNameRangeList = AtoZRange;
            //rep.ToItemNameRangeList = AtoZRange;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult PurchaseOrderStatus(PurchaseOrderStatusReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Purchase Order Status Report.");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var POStatus = dbEntity.SpRptPurchaseOrderStatus(
                    model.FromDate,
                    model.ToDate,
                    model.PONOFromID,
                    model.PONOToID,
                    model.ItemLocationID,
                    model.SupplierID,
                    model.ItemID,
                    model.Status,
                    model.UserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrderStatus");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderStatusDataSet", POStatus));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpPost]
        public JsonResult LocalPurchaseInvoicePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.LocalPurchaseInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var LocalPurchaseInvoice = LocalPurchaseInvoiceBL.GetLocalPurchaseOrder((int)Id);
            List<SpGetLocalPurchaseOrder_Result> localpurchase = new List<SpGetLocalPurchaseOrder_Result>();
            SpGetLocalPurchaseOrder_Result localpurchaseinvoice = new SpGetLocalPurchaseOrder_Result()
            {
                PurchaseOrderNo = LocalPurchaseInvoice.PurchaseOrderNo,
                PurchaseOrderDate = (DateTime)LocalPurchaseInvoice.PurchaseOrderDate,
                ID = LocalPurchaseInvoice.ID,
                SupplierReferenceNo = LocalPurchaseInvoice.SupplierReference,
                NetAmt = (decimal)LocalPurchaseInvoice.NetAmount,
                SGSTAmt = (decimal)LocalPurchaseInvoice.GSTAmount,
                IsDraft = LocalPurchaseInvoice.IsDraft
            };
            localpurchase.Add(localpurchaseinvoice);
            var LocalPurchaseInvoiceTrans = LocalPurchaseInvoiceBL.GetLocalPurchaseOrderItems((int)Id).Select(a => new SpGetLocalPurchaseOrderItems_Result()
            {
                ItemID = (int)a.ItemID,
                Unit = a.Unit,
                UnitID = (int)a.UnitID,
                Quantity = (decimal)a.QtyOrdered,
                Rate = (decimal)a.Rate,
                Remarks = a.Remarks,
                Amount = a.Amount,
                CGSTAMT = a.CGSTAmt,
                CGSTPercent = a.CGSTPercent,
                SGSTAmt = a.SGSTAmt,
                SGSTPercent = a.SGSTPercent,
                NetAmount = (decimal)a.NetAmount,
                ItemName = a.Name,
                HSNCode = a.HSNCode,
                GSTPercen = (decimal)a.GSTPercentage,
                GSTAMT = (decimal)a.IGSTAmt
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("LocalPurchaseInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LocalPurchaseInvoicePrintPdfDataSet", localpurchase));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LocalPurchaseInvoiceTransPrintPdfDataSet", LocalPurchaseInvoiceTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "LocalPurchaseInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/LocalPurchaseInvoice/"), FileName);
            string URL = "/Outputs/LocalPurchaseInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MilkPurchasePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.MilkPurchaseReceipt);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var MilkPurchase = milkPurchaseBL.GetAllMilkPurchase((int)Id).ToList();
            var MilkPurchaseTrans = milkPurchaseBL.GetAllMilkPurchaseTrans(Id).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("MilkPurchasePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MilkPurchasePrintPdfDataSet", MilkPurchase));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MilkPurchaseTransPrintPdfDataSet", MilkPurchaseTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "MilkPurchasePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/MilkPurchase/"), FileName);
            string URL = "/Outputs/MilkPurchase/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InterCompanyPurchaseInvoicePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.InterCompanyPurchaseInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var InterCompanyPurchaseInvoice = interCompanyBL.GetPurchaseInvoiceDetails(Id);
            List<SpGetInterCompanyPurchaseInvoiceDetail_Result> interCompanyPurchaseInvoices = new List<SpGetInterCompanyPurchaseInvoiceDetail_Result>();
            SpGetInterCompanyPurchaseInvoiceDetail_Result interCompanyPurchaseInvoice = new SpGetInterCompanyPurchaseInvoiceDetail_Result()
            {
                SupplierID = (int)InterCompanyPurchaseInvoice.SupplierID,
                PurchaseNo = InterCompanyPurchaseInvoice.PurchaseNo,
                PurchaseDate = (DateTime)InterCompanyPurchaseInvoice.PurchaseOrderDate,
                GrossAmount = (decimal)InterCompanyPurchaseInvoice.GrossAmount,
                SGSTAmount = (decimal)InterCompanyPurchaseInvoice.SGSTAmount,
                CGSTAmount = (decimal)InterCompanyPurchaseInvoice.CGSTAmount,
                IGSTAmount = (decimal)InterCompanyPurchaseInvoice.IGSTAmount,
                Discount = (decimal)InterCompanyPurchaseInvoice.Discount,
                CashDiscountEnabled = (bool)InterCompanyPurchaseInvoice.CashDiscountEnabled,
                CashDiscount = (decimal)InterCompanyPurchaseInvoice.CashDiscount,
                TurnOverDiscount = (decimal)InterCompanyPurchaseInvoice.TurnoverDiscount,
                AdditionalDiscount = (decimal)InterCompanyPurchaseInvoice.AdditionalDiscount,
                TaxableAmt = (decimal)InterCompanyPurchaseInvoice.TaxableAmount,
                RoundOFf = (decimal)InterCompanyPurchaseInvoice.RoundOff,
                InvoiceDate = (DateTime)InterCompanyPurchaseInvoice.InvoiceDate,
                InvoiceNo = InterCompanyPurchaseInvoice.InvoiceNo,
                SupplierName = InterCompanyPurchaseInvoice.SupplierName,
                TradeDiscount = (decimal)InterCompanyPurchaseInvoice.TradeDiscount,
                NetAmount = (decimal)InterCompanyPurchaseInvoice.NetAmount,
                PaymentMode = InterCompanyPurchaseInvoice.PaymentMode,
                OutstandingAmount = (decimal)InterCompanyPurchaseInvoice.OutstandingAmount
            };
            interCompanyPurchaseInvoices.Add(interCompanyPurchaseInvoice);
            var InterCompanyPurchaseInvoiceTrans = interCompanyBL.GetPurchaseInvoiceTrans(Id).Select(a => new SpGetInterCompanyPurchaseInvoiceTrans_Result()
            {
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceRate = (decimal)a.InvoiceRate,
                ItemName = a.ItemName,
                Unit = a.Unit,
                CGSTPercent = (decimal)a.CGSTPercent,
                IGSTPercent = (decimal)a.IGSTPercent,
                SGSTPercent = (decimal)a.SGSTPercent,
                InvoiceGSTPercent = (decimal)a.InvoiceGSTPercent,
                UnitID = (int)a.UnitID,
                BasicPrice = (decimal)a.BasicPrice,
                GrossAmount = (decimal)a.GrossAmount,
                DiscountAmount = (decimal)a.DiscountAmount,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                AdditionalDiscount = (decimal)a.AdditionalDiscount,
                TaxableAmount = (decimal)a.TaxableAmount,
                GSTAmount = (decimal)a.GSTAmount,
                TurnOverDiscount = (decimal)a.TurnoverDiscount,
                CashDiscount = (decimal)a.CashDiscount,
                NetAmount = (decimal)a.NetAmount,
                Batch = a.Batch,
                ExpiryDate = a.ExpiryDate,
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", InterCompanyPurchaseInvoice.SupplierID, "").ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("InterCompany", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("InterCompanyPurchaseInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("InterCompanyPurchaseInvoicePrintPdfDataSet", interCompanyPurchaseInvoices));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("InterCompanyPurchaseInvoiceTransPrintPdfDataSet", InterCompanyPurchaseInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "InterCompanyPurchaseInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/InterCompanyPurchaseInvoice/"), FileName);
            string URL = "/Outputs/InterCompanyPurchaseInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PurchaseOrderPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseOrder);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseOrder = purchaseOrderBL.GetPurchaseOrder(Id);
            List<PurchaseOrderReportModel> purchaseOrders = new List<PurchaseOrderReportModel>();
            PurchaseOrderReportModel purchaseOrder = new PurchaseOrderReportModel()
            {
                Shipment=PurchaseOrder.Shipment,
                SuppQuotNo = PurchaseOrder.SuppQuotNo,
                CountryName = PurchaseOrder.CountryName,
                Email = PurchaseOrder.Email,
                MobileNo = PurchaseOrder.MobileNo,
                CGSTAmt = PurchaseOrder.CGSTAmt,
                DeliveryWithin = PurchaseOrder.DeliveryWithin,
                IGSTAmt = PurchaseOrder.IGSTAmt,
                NetAmt = (decimal)PurchaseOrder.NetAmt,
                SGSTAmt = PurchaseOrder.SGSTAmt,
                PurchaseOrderDate = PurchaseOrder.PurchaseOrderDate,
                PurchaseOrderNo = PurchaseOrder.PurchaseOrderNo,
                SupplierID = PurchaseOrder.SupplierID,
                AddressLine1 = PurchaseOrder.AddressLine1,
                AddressLine2 = PurchaseOrder.AddressLine2,
                AddressLine3 = PurchaseOrder.AddressLine3,
                BAddressLine1 = PurchaseOrder.BAddressLine1,
                BAddressLine2 = PurchaseOrder.BAddressLine2,
                BAddressLine3 = PurchaseOrder.BAddressLine3,
                CurrencyName = PurchaseOrder.CurrencyName,
                SupplierName = PurchaseOrder.SupplierName,
                PaymentMode = PurchaseOrder.PaymentMode,
                PaymentWithin = (int)PurchaseOrder.PaymentWithin,
                TermsOfPrice = PurchaseOrder.TermsOfPrice,
                BillingLocation = PurchaseOrder.BillingLocation,
                SupplierReferenceNo = PurchaseOrder.SupplierReferenceNo,
                Remarks = PurchaseOrder.Remarks,
                CurrencyCode=PurchaseOrder.CurrencyCode,
                OrderType = PurchaseOrder.OrderType,
              //  CurrencyCode = PurchaseOrder.CurrencyCode,
                Discount = PurchaseOrder.Discount,
                SuppDocAmount = PurchaseOrder.SuppDocAmount,
                SuppShipAmount = PurchaseOrder.SuppShipAmount,
                VATAmount = PurchaseOrder.VATAmount,
                SuppOtherCharge = PurchaseOrder.SuppOtherCharge,
                GrossAmount = PurchaseOrder.GrossAmount,
                AmountInWords = PurchaseOrder.AmountInWords,
                MinimumCurrency = PurchaseOrder.MinimumCurrency,
                DecimalPlaces = PurchaseOrder.DecimalPlaces



            };
            purchaseOrders.Add(purchaseOrder);
            var PurchaseOrderTrans = purchaseOrderBL.GetPurchaseOrderItems(Id).Select(a => new SpGetPurchaseOrderTransDetails_Result()
            {
                Amount = a.Amount,
                CGSTAmt = a.CGSTAmt,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                ItemName = a.Name,
                QtyMet = a.QtyMet,
                Quantity = a.Quantity,
                NetAmount = (decimal)a.NetAmount,
                //Rate = a.Rate,
                //Unit = a.Unit,
                SecondaryRate = a.SecondaryRate ?? 0,
                SecondaryQty = a.SecondaryQty ?? 0,
                SecondaryUnit = a.SecondaryUnit,
                Remarks = a.Remarks,
                Code = a.ItemCode,
                PartsNumber = a.PartsNumber,
                Make = a.Make
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseOrder.SupplierID, "").ToList();
            if (PurchaseOrder.IsDraft == true && PurchaseOrder.IsSuspended == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrderDetailPagePrintPdf");
            }
            else
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermaingePrintPdf");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderPrintPdfDataSet", purchaseOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderTransPrintPdfDataSet", PurchaseOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseOrdermaingePrintPdf" + Id + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseOrder/"), FileName);
            string URL = "/Outputs/PurchaseOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PurchaseOrderItemCodePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseOrder);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseOrder = purchaseOrderBL.GetPurchaseOrder(Id);
            List<PurchaseOrderReportModel> purchaseOrders = new List<PurchaseOrderReportModel>();
            PurchaseOrderReportModel purchaseOrder = new PurchaseOrderReportModel()
            {
                Shipment = PurchaseOrder.Shipment,
                SuppQuotNo =PurchaseOrder.SuppQuotNo,
                CountryName = PurchaseOrder.CountryName,
                Email = PurchaseOrder.Email,
                MobileNo = PurchaseOrder.MobileNo,
                CGSTAmt = PurchaseOrder.CGSTAmt,
                DeliveryWithin = PurchaseOrder.DeliveryWithin,
                IGSTAmt = PurchaseOrder.IGSTAmt,
                NetAmt = (decimal)PurchaseOrder.NetAmt,
                SGSTAmt = PurchaseOrder.SGSTAmt,
                PurchaseOrderDate = PurchaseOrder.PurchaseOrderDate,
                PurchaseOrderNo = PurchaseOrder.PurchaseOrderNo,
                SupplierID = PurchaseOrder.SupplierID,
                AddressLine1 = PurchaseOrder.AddressLine1,
                AddressLine2 = PurchaseOrder.AddressLine2,
                AddressLine3 = PurchaseOrder.AddressLine3,
                BAddressLine1 = PurchaseOrder.BAddressLine1,
                BAddressLine2 = PurchaseOrder.BAddressLine2,
                BAddressLine3 = PurchaseOrder.BAddressLine3,
                CurrencyName = PurchaseOrder.CurrencyName,
                SupplierName = PurchaseOrder.SupplierName,
                PaymentMode = PurchaseOrder.PaymentMode,
                PaymentWithin = (int)PurchaseOrder.PaymentWithin,
                TermsOfPrice = PurchaseOrder.TermsOfPrice,
                BillingLocation = PurchaseOrder.BillingLocation,
                SupplierReferenceNo = PurchaseOrder.SupplierReferenceNo,
                OrderType = PurchaseOrder.OrderType,
                Remarks = PurchaseOrder.Remarks,
                CurrencyCode=PurchaseOrder.CurrencyCode,
                Discount= PurchaseOrder.Discount,
                SuppDocAmount= PurchaseOrder.SuppDocAmount,
                SuppShipAmount = PurchaseOrder.SuppShipAmount,
                VATAmount= PurchaseOrder.VATAmount,
                SuppOtherCharge= PurchaseOrder.SuppOtherCharge,
                GrossAmount = PurchaseOrder.GrossAmount,
                AmountInWords = PurchaseOrder.AmountInWords,
                MinimumCurrency = PurchaseOrder.MinimumCurrency,
                DecimalPlaces= PurchaseOrder.DecimalPlaces,


            };
            purchaseOrders.Add(purchaseOrder);
            var PurchaseOrderTrans = purchaseOrderBL.GetPurchaseOrderItems(Id).Select(a => new SpGetPurchaseOrderTransDetails_Result()
            {
                Amount = a.Amount,
                CGSTAmt = a.CGSTAmt,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                ItemName = a.Name,
                QtyMet = a.QtyMet,
                Quantity = a.Quantity,
                NetAmount = (decimal)a.NetAmount,
                //Rate = a.Rate,
                //Unit = a.Unit,
                SecondaryRate = a.SecondaryRate ?? 0,
                SecondaryQty = a.SecondaryQty ?? 0,
                SecondaryUnit = a.SecondaryUnit,
                Remarks = a.Remarks,
                Code = a.ItemCode,
                PartsNumber = a.PartsNumber,
                Make = a.Make
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseOrder.SupplierID, "").ToList();
            if (PurchaseOrder.IsDraft == true && PurchaseOrder.IsSuspended == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermainItemCode");
            }
            else
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermainItemCode");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderPrintPdfDataSet", purchaseOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderTransPrintPdfDataSet", PurchaseOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseOrderPrint" + Id + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseOrder/"), FileName);
            string URL = "/Outputs/PurchaseOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PurchaseOrderPartNoPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseOrder);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseOrder = purchaseOrderBL.GetPurchaseOrder(Id);
            List<PurchaseOrderReportModel> purchaseOrders = new List<PurchaseOrderReportModel>();
            PurchaseOrderReportModel purchaseOrder = new PurchaseOrderReportModel()
            {
                Shipment = PurchaseOrder.Shipment,
                SuppQuotNo = PurchaseOrder.SuppQuotNo,
                CountryName = PurchaseOrder.CountryName,
                Email = PurchaseOrder.Email,
                MobileNo = PurchaseOrder.MobileNo,
                CGSTAmt = PurchaseOrder.CGSTAmt,
                DeliveryWithin = PurchaseOrder.DeliveryWithin,
                IGSTAmt = PurchaseOrder.IGSTAmt,
                NetAmt = (decimal)PurchaseOrder.NetAmt,
                SGSTAmt = PurchaseOrder.SGSTAmt,
                PurchaseOrderDate = PurchaseOrder.PurchaseOrderDate,
                PurchaseOrderNo = PurchaseOrder.PurchaseOrderNo,
                SupplierID = PurchaseOrder.SupplierID,
                AddressLine1 = PurchaseOrder.AddressLine1,
                AddressLine2 = PurchaseOrder.AddressLine2,
                AddressLine3 = PurchaseOrder.AddressLine3,
                BAddressLine1 = PurchaseOrder.BAddressLine1,
                BAddressLine2 = PurchaseOrder.BAddressLine2,
                BAddressLine3 = PurchaseOrder.BAddressLine3,
                CurrencyName = PurchaseOrder.CurrencyName,
                SupplierName = PurchaseOrder.SupplierName,
                PaymentMode = PurchaseOrder.PaymentMode,
                PaymentWithin = (int)PurchaseOrder.PaymentWithin,
                TermsOfPrice = PurchaseOrder.TermsOfPrice,
                BillingLocation = PurchaseOrder.BillingLocation,
                SupplierReferenceNo = PurchaseOrder.SupplierReferenceNo,
                Remarks = PurchaseOrder.Remarks,
                CurrencyCode=PurchaseOrder.CurrencyCode,
                OrderType = PurchaseOrder.OrderType,
                Discount = PurchaseOrder.Discount,
                SuppDocAmount = PurchaseOrder.SuppDocAmount,
                SuppShipAmount = PurchaseOrder.SuppShipAmount,
                VATAmount = PurchaseOrder.VATAmount,
                SuppOtherCharge = PurchaseOrder.SuppOtherCharge,
                GrossAmount=PurchaseOrder.GrossAmount,
                AmountInWords = PurchaseOrder.AmountInWords,
                MinimumCurrency = PurchaseOrder.MinimumCurrency,
                DecimalPlaces = PurchaseOrder.DecimalPlaces,
            };
            purchaseOrders.Add(purchaseOrder);
            var PurchaseOrderTrans = purchaseOrderBL.GetPurchaseOrderItems(Id).Select(a => new SpGetPurchaseOrderTransDetails_Result()
            {
                Amount = a.Amount,
                CGSTAmt = a.CGSTAmt,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                ItemName = a.Name,
                QtyMet = a.QtyMet,
                Quantity = a.Quantity,
                NetAmount = (decimal)a.NetAmount,
                //Rate = a.Rate,
                //Unit = a.Unit,
                SecondaryRate = a.SecondaryRate ?? 0,
                SecondaryQty = a.SecondaryQty ?? 0,
                SecondaryUnit = a.SecondaryUnit,
                Remarks = a.Remarks,
                Code = a.ItemCode,
                PartsNumber = a.PartsNumber,
                Make = a.Make
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseOrder.SupplierID, "").ToList();
            if (PurchaseOrder.IsDraft == true && PurchaseOrder.IsSuspended == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermainPartsNo");
            }
            else
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermainPartsNo");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderPrintPdfDataSet", purchaseOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderTransPrintPdfDataSet", PurchaseOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseOrdermainPartsNo" + Id + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseOrder/"), FileName);
            string URL = "/Outputs/PurchaseOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PurchaseOrderExportIemCodePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseOrder);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseOrder = purchaseOrderBL.GetPurchaseOrder(Id);
            List<PurchaseOrderReportModel> purchaseOrders = new List<PurchaseOrderReportModel>();
            PurchaseOrderReportModel purchaseOrder = new PurchaseOrderReportModel()
            {
                Shipment = PurchaseOrder.Shipment,
                SuppQuotNo = PurchaseOrder.SuppQuotNo,
                CountryName = PurchaseOrder.CountryName,
                Email = PurchaseOrder.Email,
                MobileNo = PurchaseOrder.MobileNo,
                CGSTAmt = PurchaseOrder.CGSTAmt,
                DeliveryWithin = PurchaseOrder.DeliveryWithin,
                IGSTAmt = PurchaseOrder.IGSTAmt,
                NetAmt = (decimal)PurchaseOrder.NetAmt,
                SGSTAmt = PurchaseOrder.SGSTAmt,
                PurchaseOrderDate = PurchaseOrder.PurchaseOrderDate,
                PurchaseOrderNo = PurchaseOrder.PurchaseOrderNo,
                SupplierID = PurchaseOrder.SupplierID,
                AddressLine1 = PurchaseOrder.AddressLine1,
                AddressLine2 = PurchaseOrder.AddressLine2,
                AddressLine3 = PurchaseOrder.AddressLine3,
                BAddressLine1 = PurchaseOrder.BAddressLine1,
                BAddressLine2 = PurchaseOrder.BAddressLine2,
                BAddressLine3 = PurchaseOrder.BAddressLine3,
                CurrencyName = PurchaseOrder.CurrencyName,
                SupplierName = PurchaseOrder.SupplierName,
                PaymentMode = PurchaseOrder.PaymentMode,
                PaymentWithin = (int)PurchaseOrder.PaymentWithin,
                TermsOfPrice = PurchaseOrder.TermsOfPrice,
                BillingLocation = PurchaseOrder.BillingLocation,
                SupplierReferenceNo = PurchaseOrder.SupplierReferenceNo,
                Remarks = PurchaseOrder.Remarks,
                CurrencyCode =PurchaseOrder.CurrencyCode,
                OrderType = PurchaseOrder.OrderType,
                Discount = PurchaseOrder.Discount,
                SuppDocAmount = PurchaseOrder.SuppDocAmount,
                SuppShipAmount = PurchaseOrder.SuppShipAmount,
                VATAmount = PurchaseOrder.VATAmount,
                SuppOtherCharge = PurchaseOrder.SuppOtherCharge,
                GrossAmount = PurchaseOrder.GrossAmount,
                AmountInWords = PurchaseOrder.AmountInWords,
                MinimumCurrency = PurchaseOrder.MinimumCurrency,
                DecimalPlaces = PurchaseOrder.DecimalPlaces,
            };
            purchaseOrders.Add(purchaseOrder);
            var PurchaseOrderTrans = purchaseOrderBL.GetPurchaseOrderItems(Id).Select(a =>new SpGetPurchaseOrderTransDetails_Result()
            {
                Amount = a.Amount,
                CGSTAmt = a.CGSTAmt,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                ItemName = a.Name,
                QtyMet = a.QtyMet,
                Quantity = a.Quantity,
                NetAmount = (decimal)a.NetAmount,
                //Rate = a.Rate,
                //Unit = a.Unit,
                SecondaryRate = a.SecondaryRate ?? 0,
                SecondaryQty = a.SecondaryQty ?? 0,
                SecondaryUnit = a.SecondaryUnit,
                Remarks = a.Remarks,
                ItemCode = a.ItemCode,
                PartsNumber = a.PartsNumber,
                Make = a.Make
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseOrder.SupplierID, "").ToList();
            if (PurchaseOrder.IsDraft == true && PurchaseOrder.IsSuspended == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermainExportItemCode");
            }
            else
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermainExportItemCode");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, MobileNoParam,ReportLogoPathParam, ReportfooterPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderPrintPdfDataSet", purchaseOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderTransPrintPdfDataSet", PurchaseOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseOrdermainExportItemCode" + Id + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseOrder/"), FileName);
            string URL = "/Outputs/PurchaseOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PurchaseOrderExportPartNoPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseOrder);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseOrder = purchaseOrderBL.GetPurchaseOrder(Id);
            List<PurchaseOrderReportModel> purchaseOrders = new List<PurchaseOrderReportModel>();
            PurchaseOrderReportModel purchaseOrder = new PurchaseOrderReportModel()
            {
                Shipment = PurchaseOrder.Shipment,
                SuppQuotNo = PurchaseOrder.SuppQuotNo,
                CountryName = PurchaseOrder.CountryName,
                Email = PurchaseOrder.Email,
                MobileNo = PurchaseOrder.MobileNo,
                CGSTAmt = PurchaseOrder.CGSTAmt,
                DeliveryWithin = PurchaseOrder.DeliveryWithin,
                IGSTAmt = PurchaseOrder.IGSTAmt,
                NetAmt = (decimal)PurchaseOrder.NetAmt,
                SGSTAmt = PurchaseOrder.SGSTAmt,
                PurchaseOrderDate = PurchaseOrder.PurchaseOrderDate,
                PurchaseOrderNo = PurchaseOrder.PurchaseOrderNo,
                SupplierID = PurchaseOrder.SupplierID,
                AddressLine1 = PurchaseOrder.AddressLine1,
                AddressLine2 = PurchaseOrder.AddressLine2,
                AddressLine3 = PurchaseOrder.AddressLine3,
                BAddressLine1 = PurchaseOrder.BAddressLine1,
                BAddressLine2 = PurchaseOrder.BAddressLine2,
                BAddressLine3 = PurchaseOrder.BAddressLine3,
                CurrencyName = PurchaseOrder.CurrencyName,
                SupplierName = PurchaseOrder.SupplierName,
                PaymentMode = PurchaseOrder.PaymentMode,
                PaymentWithin = (int)PurchaseOrder.PaymentWithin,
                TermsOfPrice = PurchaseOrder.TermsOfPrice,
                BillingLocation = PurchaseOrder.BillingLocation,
                SupplierReferenceNo = PurchaseOrder.SupplierReferenceNo,
                Remarks = PurchaseOrder.Remarks,
                CurrencyCode = PurchaseOrder.CurrencyCode,
                OrderType = PurchaseOrder.OrderType,
                Discount = PurchaseOrder.Discount,
                SuppDocAmount = PurchaseOrder.SuppDocAmount,
                SuppShipAmount = PurchaseOrder.SuppShipAmount,
                VATAmount = PurchaseOrder.VATAmount,
                SuppOtherCharge = PurchaseOrder.SuppOtherCharge,
                GrossAmount = PurchaseOrder.GrossAmount,
                AmountInWords = PurchaseOrder.AmountInWords,
                MinimumCurrency = PurchaseOrder.MinimumCurrency,
                DecimalPlaces = PurchaseOrder.DecimalPlaces,
            };
            purchaseOrders.Add(purchaseOrder);
            var PurchaseOrderTrans = purchaseOrderBL.GetPurchaseOrderItems(Id).Select(a => new SpGetPurchaseOrderTransDetails_Result()
            {
                Amount = a.Amount,
                CGSTAmt = a.CGSTAmt,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                ItemName = a.Name,
                QtyMet = a.QtyMet,
                Quantity = a.Quantity,
                NetAmount = (decimal)a.NetAmount,
                //Rate = a.Rate,
                //Unit = a.Unit,
                SecondaryRate = a.SecondaryRate ?? 0,
                SecondaryQty = a.SecondaryQty ?? 0,
                SecondaryUnit = a.SecondaryUnit,
                Remarks = a.Remarks,
                Code = a.ItemCode,
                PartsNumber = a.PartsNumber,
                Make = a.Make
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseOrder.SupplierID, "").ToList();
            if (PurchaseOrder.IsDraft == true && PurchaseOrder.IsSuspended == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermainExportPartsNo");
            }
            else
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermainExportPartsNo");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);
            ReportfooterPathParam = new ReportParameter("ReportfooterPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportfooterPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, MobileNoParam, ReportLogoPathParam,ReportfooterPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderPrintPdfDataSet", purchaseOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderTransPrintPdfDataSet", PurchaseOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseOrdermainExportPartsNo" + Id + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseOrder/"), FileName);
            string URL = "/Outputs/PurchaseOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GoodsReceiptNotePrint(int Id)
        {
            var GRM = goodsReceiptNoteBL.GetGRNDetail(Id).FirstOrDefault();
            List<SpGetGRNDetail_Result> SpGetGRNDetailReport = new List<SpGetGRNDetail_Result>();
            SpGetGRNDetail_Result SpGetGRNDetail = new SpGetGRNDetail_Result()
            {

                Email = GRM.Email,
                MobileNo = GRM.MobileNo,
                AddressLine1 = GRM.AddressLine1,
                AddressLine2 = GRM.AddressLine2,
                AddressLine3 = GRM.AddressLine3,
                BAddressLine1 = GRM.BAddressLine1,
                BAddressLine2 = GRM.BAddressLine2,
                BAddressLine3 = GRM.BAddressLine3,
                CountryName = GRM.CountryName,
                Remarks = GRM.Remarks,
                CurrencyName = GRM.CurrencyName,
                NetAmount = (decimal)GRM.NetAmount,
            };
            SpGetGRNDetailReport.Add(SpGetGRNDetail);
            var GetGRNTransDetails = goodsReceiptNoteBL.GetGRNItems(Id).Select(m => new SpGetGRNTransDetails_Result()
            {
                ItemCode = m.ItemCode,
                ItemName = m.ItemName,
                PartsNumber = m.PartsNumber,
                Remark = m.Remark,
                Model = m.Model,
                PurchaseOrderQty = m.PurchaseOrderQty,
                ReceivedQty = m.ReceivedQty,
                Unit = m.Unit,
                TaxableAmount = m.TaxableAmount,

                Remarks = m.Remarks,
                SecondaryUnit = m.SecondaryUnit,
                SecondaryRate = m.SecondaryRate,
                SecondaryUnitSize = m.SecondaryUnitSize,

            }).ToList();

            Warning[] warnings;
            string[] streamIds;
            string contentType, encoding, extension;

            // Make sure report path is correct
            reportViewer.LocalReport.ReportPath = GetReportPath("GRMPrintPdfReport");

            // Clear previous data sources and add new ones
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptNotePrintDataSet", SpGetGRNDetailReport));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodsReceiptNoteTransPrintDataSet", GetGRNTransDetails));
            // Render the report to a PDF byte array
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);

            if (warnings != null && warnings.Length > 0)
            {
                foreach (var warning in warnings)
                {
                    Console.WriteLine(warning.Message);
                }
            }
            string fileName = "GoodsReceiptNotePrint" + Id + ".pdf";
            string filePath = Path.Combine(Server.MapPath("~/Outputs/GoodsReceiptNote/"), fileName);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            string url = "/Outputs/GoodsReceiptNote/" + fileName;
            return Json(new { Status = "success", URL = url }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult PurchaseOrderPrint(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseOrder);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseOrder = purchaseOrderBL.GetPurchaseOrder(Id);
            List<PurchaseOrderReportModel> purchaseOrders = new List<PurchaseOrderReportModel>();
            PurchaseOrderReportModel purchaseOrder = new PurchaseOrderReportModel()
            {
                Shipment = PurchaseOrder.Shipment,
                SuppQuotNo = PurchaseOrder.SuppQuotNo,
                CGSTAmt = PurchaseOrder.CGSTAmt,
                CountryName = PurchaseOrder.CountryName,
                Email = PurchaseOrder.Email,
                MobileNo = PurchaseOrder.MobileNo,
                AddressLine1 = PurchaseOrder.AddressLine1,
                AddressLine2 = PurchaseOrder.AddressLine2,
                AddressLine3 = PurchaseOrder.AddressLine3,
                BAddressLine1 = PurchaseOrder.BAddressLine1,
                BAddressLine2 = PurchaseOrder.BAddressLine2,
                BAddressLine3 = PurchaseOrder.BAddressLine3,
                DeliveryWithin = PurchaseOrder.DeliveryWithin,
                IGSTAmt = PurchaseOrder.IGSTAmt,
                NetAmt = (decimal)PurchaseOrder.NetAmt,
                SGSTAmt = PurchaseOrder.SGSTAmt,
                PurchaseOrderDate = PurchaseOrder.PurchaseOrderDate,
                PurchaseOrderNo = PurchaseOrder.PurchaseOrderNo,
                SupplierID = PurchaseOrder.SupplierID,
                SupplierName = PurchaseOrder.SupplierName,
                PaymentMode = PurchaseOrder.PaymentMode,
                PaymentWithin = (int)PurchaseOrder.PaymentWithin,
                TermsOfPrice = PurchaseOrder.TermsOfPrice,
                BillingLocation = PurchaseOrder.BillingLocation,
                SupplierReferenceNo = PurchaseOrder.SupplierReferenceNo,
                Remarks = PurchaseOrder.Remarks,
                CurrencyName = PurchaseOrder.CurrencyName,
                CurrencyCode = PurchaseOrder.CurrencyCode,
                OrderType = PurchaseOrder.OrderType,
                Discount = PurchaseOrder.Discount,
                SuppDocAmount = PurchaseOrder.SuppDocAmount,
                SuppShipAmount = PurchaseOrder.SuppShipAmount,
                VATAmount = PurchaseOrder.VATAmount,
                SuppOtherCharge = PurchaseOrder.SuppOtherCharge,
                GrossAmount = PurchaseOrder.GrossAmount,
                AmountInWords = PurchaseOrder.AmountInWords,
                MinimumCurrency = PurchaseOrder.MinimumCurrency,
                DecimalPlaces = PurchaseOrder.DecimalPlaces,
            };
            purchaseOrders.Add(purchaseOrder);
            var PurchaseOrderTrans = purchaseOrderBL.GetPurchaseOrderItems(Id).Select(a => new SpGetPurchaseOrderTransDetails_Result()
            {
                Amount = a.Amount,
                CGSTAmt = a.CGSTAmt,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                ItemName = a.Name,
                QtyMet = a.QtyMet,
                //Quantity = a.Quantity,
                //Rate = a.Rate,
                //Unit = a.Unit,
                SecondaryRate = a.SecondaryRate ?? 0,
                SecondaryQty = a.SecondaryQty ?? 0,
                SecondaryUnit = a.SecondaryUnit,
                NetAmount = (decimal)a.NetAmount,
                Code = a.ItemCode,
                PartsNumber = a.PartsNumber,
                Remarks = a.Remarks,
                Make= a.Make,
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseOrder.SupplierID, "").ToList();
            if (PurchaseOrder.IsDraft == true && PurchaseOrder.IsSuspended == false)
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermaingePrintPdf");
            }
            else
            {
                reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrdermaingePrintPdf");
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderPrintPdfDataSet", purchaseOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderTransPrintPdfDataSet", PurchaseOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseOrderPrint" + Id + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseOrder/"), FileName);
            string URL = "/Outputs/PurchaseOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }





        public JsonResult ServicePurchaseOrderPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ServicePurchaseOrder);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ServicePurchaseOrder = servicePurchaseOrderBL.GetPurchaseOrder(Id);
            List<SpGetPurchaseOrderForService_Result> servicepurchaseOrders = new List<SpGetPurchaseOrderForService_Result>();
            SpGetPurchaseOrderForService_Result servicepurchaseOrder = new SpGetPurchaseOrderForService_Result()
            {
                CGSTAmt = ServicePurchaseOrder.CGSTAmt,
                DeliveryWithin = ServicePurchaseOrder.DeliveryWithin,
                IGSTAmt = ServicePurchaseOrder.IGSTAmt,
                NetAmt = (decimal)ServicePurchaseOrder.NetAmt,
                SGSTAmt = ServicePurchaseOrder.SGSTAmt,
                PurchaseOrderDate = ServicePurchaseOrder.PurchaseOrderDate,
                PurchaseOrderNo = ServicePurchaseOrder.PurchaseOrderNo,
                SupplierID = ServicePurchaseOrder.SupplierID,
                Supplier = ServicePurchaseOrder.SupplierName,
                PaymentMode = ServicePurchaseOrder.PaymentMode,
                PaymentWithIn = (int)ServicePurchaseOrder.PaymentWithin,
                TermsOfPrice = ServicePurchaseOrder.TermsOfPrice,
                BillingAddress = ServicePurchaseOrder.BillingLocation,
                SupplierReferenceNo = ServicePurchaseOrder.SupplierReferenceNo
            };
            servicepurchaseOrders.Add(servicepurchaseOrder);
            var ServicePurchaseOrderTrans = servicePurchaseOrderBL.GetPurchaseOrderTransDetails(Id).Select(a => new SpGetPurchaseOrderTransDetailsForService_Result()
            {
                Amount = a.Amount,
                CGSTAmt = a.CGSTAmt,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                ItemName = a.Name,
                QtyMet = a.QtyMet,
                Quantity = a.Quantity,
                NetAmount = (decimal)a.NetAmount,
                Rate = a.Rate,
                Remarks = a.Remarks,
                Unit = a.Unit,
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", ServicePurchaseOrder.SupplierID, "").ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("ServicePurchaseOrderPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ServicePurchaseOrderPrintPdfDataSet", servicepurchaseOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ServicePurchaseOrderTransPrintPdfDataSet", ServicePurchaseOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ServicePurchaseOrderPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ServicePurchaseOrder/"), FileName);
            string URL = "/Outputs/ServicePurchaseOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PurchaseReturnPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseReturn);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseReturn = purchaseReturnBL.GetPurchaseReturnDetail(Id).Select(a => new SpGetPurchaseReturnOrderDetail_Result()
            {
                ID = a.Id,
                RETURNNO = a.ReturnNo,
                Date = (DateTime)a.ReturnDate,
                suppliername = a.SupplierName,
                SupplierID = (int)a.SupplierID,
                FreightAmount = (decimal)a.Freight,
                PackingCahrge = (decimal)a.PackingCharges,
                OtherCharge = (decimal)a.OtherCharges,
                NetAmount = (decimal)a.NetAmount,
                IsDraft = (bool)a.IsDraft,
                Discount = (decimal)a.Discount,
                StateID = (int)a.StateID,
                IGSTAmount = (decimal)a.IGSTAmount,
                SGSTAmount = (decimal)a.SGSTAmount,
                CGSTAmount = (decimal)a.CGSTAmount,
                GSTNo = a.GSTNo,
                State = a.State,
                Addresses1 = a.Addresses1,
                Addresses2 = a.Addresses2
            }).ToList();
            var PurchaseReturnTrans = purchaseReturnBL.GetPurchaseReturnTransList(Id).Select(a => new SpGetPurchaseReturnOrderTrans_Result()
            {
                ItemName = a.ItemName,
                PurchaseInvoiceID = a.PurchaseInvoiceID,
                PurchaseNo = a.PurchaseNo,
                unit = a.Unit,
                UnitID = (int)a.UnitID,
                ItemID = a.ItemID,
                AcceptedQty = (decimal)a.AcceptedQty,
                QTY = a.ReturnQty,
                Rate = a.Rate,
                SGSTAmount = a.SGSTAmt,
                SGSTPercent = a.SGSTPercent,
                IGSTAmount = a.IGSTAmt,
                IGSTPercent = a.IGSTPercent,
                CGSTAmount = a.CGSTAmt,
                CGSTPercent = a.CGSTPercent,
                Amount = a.Amount,
                Remarks = a.Remarks,
                Stock = (decimal)a.Stock,
                ConvertedQuantity = (decimal)a.ConvertedQty,
                ConvertedStock = (decimal)a.ConvertedStock,
                PrimaryUnitID = a.PrimaryUnitID,
                PurchaseUnitID = (int)a.PurchaseUnitID,
                PurchaseUnit = a.PurchaseUnit,
                PrimaryUnit = a.PrimaryUnit,
                InvoiceQty = (decimal)a.InvoiceQty,
                InvoiceTransID = (int)a.InvoiceTransID,
                InvoiceNo = a.InvoiceNo,
                GSTPercent = a.GSTPercentage,
                GSTAmount = a.GSTAmount,
                GSTID = a.GSTID,
                Discount = a.Discount
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseReturnPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseReturnPrintPdfDataSet", PurchaseReturn));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseReturnTransPrintPdfDataSet", PurchaseReturnTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseReturnPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseReturn/"), FileName);
            string URL = "/Outputs/PurchaseReturn/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTranTypeRange(string from_range)
        {
            MISViewModel rep = new MISViewModel();
            if (from_range == "")
            {
                rep.FromTransTypeRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.FromTransTypeRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = rep.FromTransTypeRangeList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemAccountCategoryRange(string from_range)
        {
            MISViewModel rep = new MISViewModel();
            if (from_range == "")
            {
                rep.FromItemAccountsCategoryList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.FromItemAccountsCategoryList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = rep.FromItemAccountsCategoryList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountNameRange(string from_range)
        {
            MISViewModel rep = new MISViewModel();
            if (from_range == "")
            {
                rep.AccountNameFromList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                rep.AccountNameFromList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = rep.AccountNameFromList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutoComplete(string Term = "", string Table = "")
        {
            return Json(reportBL.GetAutoComplete(Term, Table).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getProductList(string Areas, int SupplierId, string term = "", string ItemCategoryID = "", string PurchaseCategoryID = "")
        {
            List<PurchaseOrderItemBO> _outItems = new List<PurchaseOrderItemBO>();
            var ItemCategoryIDInt = (ItemCategoryID != "") ? Convert.ToInt32(ItemCategoryID) : 0;
            var PurchaseCategoryIDInt = (PurchaseCategoryID != "") ? Convert.ToInt32(PurchaseCategoryID) : 0;

            _outItems = _dropdown.GetPurchaseOrderItems(term, SupplierId, ItemCategoryIDInt, PurchaseCategoryIDInt);
            return Json(_outItems, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CostingAndProfitability()
        {
            ProfitReportModel model = new ProfitReportModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult CostingAndProfitability(ProfitReportModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Costing And Profitability Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var Production = new object();
            //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/CostingAndProfitability.rdlc";
            string XMLParams = XMLHelper.ParseXML(model);
            if (model.Summary == "Summary")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/CostingAndProfitabilitySummary.rdlc";
                var Profit = dbEntity.SpRptCostingAndProfitability(
                                        XMLParams
                                        ).ToList();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CostingAndProfitabilityDataSet", Profit));
            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/CostingAndProfitability.rdlc";
                var Profit = dbEntity.SpRptCostingAndProfitability(
                                        XMLParams
                                        ).ToList();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CostingAndProfitabilityDataSet", Profit));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult CostingAndProfitabilityV2()
        {
            ProfitReportModel model = new ProfitReportModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult CostingAndProfitabilityV2(ProfitReportModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Costing And Profitability Report");
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var Production = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/CostingAndProfitabilityV2.rdlc";
            string XMLParams = XMLHelper.ParseXML(model);
            var Profit = dbEntity.SpRptCostingAndProfitabilityNewMethod(
                                    XMLParams
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CostingAndProfitabilityV2DataSet", Profit));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
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
            ReportNameParam = new ReportParameter("ReportName", "Profitability On The Basis Of Purchase");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);

            var Production = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/ProfitabilityReport.rdlc";
            string XMLParams = XMLHelper.ParseXML(model);
            var Profit = dbEntity.SpRptProfitabilityOnTheBasisOfPurchase(
                                    XMLParams
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseProfitabilityDataSet", Profit));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //public JsonResult QRCodePrintPdf(int ID)
        //{
        //    //DateTime FromDate = General.ToDateTime(Date);
        //    ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ConsultationFeeReceipt);
        //    Warning[] warnings;
        //    string[] streamIds;
        //    string contentType;
        //    string mimeType = string.Empty;
        //    string encoding = string.Empty;
        //    string extension = string.Empty;
        //    var AppointmentSchedule = appointmentScheduleBL.GetAppointmentScheduleDetailsForPrint((int)ID).Select(a => new SpGetAppointmentScheduleDetailsForPrint_Result()
        //    {
        //        Time = a.Time,
        //        ValidFromDate = a.FromDate,
        //        ValidToDate = a.ToDate,
        //        PatientName = a.Patient,
        //        Rate = a.Rate,
        //        ValidDate = a.CreatedDate,
        //        ItemName = a.ItemName,
        //        SalesOrderNo = a.TransNo,
        //        DoctorName = a.DoctorName,
        //        TokenNo = a.TokenNo
        //    }).ToList();
        //    reportViewer.LocalReport.ReportPath = GetReportPath("AppointmentBookingPrintPdf");
        //    ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
        //    reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam });
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AppointmentBookingPrintPdfDataSet", AppointmentSchedule));
        //    //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
        //    byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
        //    // Open generated PDF.
        //    string FileName = "AppointmentBookingPrintPdf.pdf";
        //    string FilePath = Path.Combine(Server.MapPath("~/Outputs/AppointmentBooking/"), FileName);
        //    string URL = "/Outputs/AppointmentBooking/" + FileName;

        //    using (FileStream fs = new FileStream(FilePath, FileMode.Create))
        //    {
        //        fs.Write(bytes, 0, bytes.Length);
        //    }
        //    return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public JsonResult PurchaseInvoicePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseInvoice = purchaseInvoiceBL.GetPurchaseInvoiceDetails(Id);
            List<SpGetPurchaseInvoiceDetailsByID_Result> purchaseInvoice = new List<SpGetPurchaseInvoiceDetailsByID_Result>();
            SpGetPurchaseInvoiceDetailsByID_Result purchaseinvoice = new SpGetPurchaseInvoiceDetailsByID_Result()
            {
                suppliercurrencyCode = PurchaseInvoice.suppliercurrencyCode,
                SuuplierCurrencyconverion =PurchaseInvoice.SuuplierCurrencyconverion,
                shipmentmode=PurchaseInvoice.shipmentmode,
                SecondaryQty = PurchaseInvoice.SecondaryQty,
                Id = PurchaseInvoice.Id,
                AddressLine1 = PurchaseInvoice.AddressLine1,
                AddressLine2 = PurchaseInvoice.AddressLine2,
                AddressLine3 = PurchaseInvoice.AddressLine3,
                PurchaseNo = PurchaseInvoice.PurchaseNo,
                PurchaseDate = PurchaseInvoice.PurchaseDate,
                InvoiceNo = PurchaseInvoice.InvoiceNo,
                InvoiceDate = PurchaseInvoice.InvoiceDate,
                SupplierName = PurchaseInvoice.SupplierName,
                LocalSupplier = PurchaseInvoice.LocalSupplierName,
                NetAmount = PurchaseInvoice.NetAmount,
                InvoiceTotal = (decimal)PurchaseInvoice.InvoiceTotal,
                GrossAmount = PurchaseInvoice.GrossAmount,
               // LocalMiscCharge=purchaseInvoice.LocalMiscCharge,
                SuppDocAmount= PurchaseInvoice.SuppDocAmount,
                SuppShipAmount=PurchaseInvoice.SuppShipAmount,
                LocalOtherCharges=PurchaseInvoice.LocalOtherCharges,
                LocalCustomsDuty=PurchaseInvoice.LocalCustomsDuty,
                LocalMiscCharge=PurchaseInvoice.LocalMiscCharge,
                LocalFreight=PurchaseInvoice.LocalFreight,
                CurrencyCode=PurchaseInvoice.CurrencyCode,
                

                SGSTAmount = PurchaseInvoice.SGST,
                CGSTAmount = PurchaseInvoice.CGST,
                IGSTAmount = PurchaseInvoice.IGST,
                Discount = PurchaseInvoice.Discount,
                CurrencyExchangeRate = PurchaseInvoice.CurrencyExchangeRate,
                FreightAmount = PurchaseInvoice.FreightAmount,
                CurrencyName = PurchaseInvoice.CurrencyName,
                PackingForwarding=PurchaseInvoice.PackingForwarding,
                SupplierOtherCharges=PurchaseInvoice.SupplierOtherCharges,

               // FreightAmount = PurchaseInvoice.FreightAmount,
               // PackingCharges = PurchaseInvoice.PackingCharges,
                OtherCharges = PurchaseInvoice.OtherCharges,
                //TaxOnFreight = PurchaseInvoice.TaxOnFreight,
                //TaxOnPackingCharges = PurchaseInvoice.TaxOnPackingCharges,
                //TDSOnFreightPercentage = PurchaseInvoice.TDSOnFreight,
                //LessTDS = PurchaseInvoice.LessTDS,
                AmountPayable = PurchaseInvoice.AmountPayable,
                TotalDifference = (decimal)PurchaseInvoice.TotalDifference,
                OtherDeductions = (decimal)PurchaseInvoice.OtherDeductions,
                PurchaseOrderDate = PurchaseInvoice.PurchaseOrderDate,
                SupplierCode = PurchaseInvoice.SupplierCode,
                //TDSCode = PurchaseInvoice.TDSCode,
                SupplierLocation = PurchaseInvoice.SupplierLocation,
                GSTNo = PurchaseInvoice.GSTNo,
                Remarks = PurchaseInvoice.Remarks,
                //GRNDate = (DateTime)PurchaseInvoice.GRNDate,
                IsDraft = PurchaseInvoice.IsDraft,
                IsCancelled = PurchaseInvoice.IsCancelled,
                IsGSTRegistered = PurchaseInvoice.IsGSTRegistered,
                PItype= PurchaseInvoice.PItype,
                WayBillNo = PurchaseInvoice.WayBillNo,
                Freight = PurchaseInvoice.Freight,
                VatRegNo = PurchaseInvoice.VatRegNo,
                OtherChargesVATAmount = PurchaseInvoice.OtherChargesVATAmount
            };
            purchaseInvoice.Add(purchaseinvoice);
            var PurchaseInvoiceTrans = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails(Id);
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var ShippingAddress = addressBL.GetShippingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("PurchaseInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseInvoicePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
           // ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoicePrintPdfDataSet", purchaseInvoice));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceTransPrintPdfDataSet", PurchaseInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdfDataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseInvoicePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseInvoice/"), FileName);
            string URL = "/Outputs/PurchaseInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult PurchaseInvoiceIemCodePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseInvoice = purchaseInvoiceBL.GetPurchaseInvoiceDetails(Id); 
            List<SpGetPurchaseInvoiceDetailsByID_Result> purchaseInvoice = new List<SpGetPurchaseInvoiceDetailsByID_Result>();
            SpGetPurchaseInvoiceDetailsByID_Result purchaseinvoice = new SpGetPurchaseInvoiceDetailsByID_Result()
            {
                suppliercurrencyCode=PurchaseInvoice.suppliercurrencyCode,
                SuuplierCurrencyconverion = PurchaseInvoice.SuuplierCurrencyconverion,
                shipmentmode = PurchaseInvoice.shipmentmode,
                SecondaryQty = PurchaseInvoice.SecondaryQty,
                Id = PurchaseInvoice.Id,
                AddressLine1 = PurchaseInvoice.AddressLine1,
                AddressLine2 = PurchaseInvoice.AddressLine2,
                AddressLine3 = PurchaseInvoice.AddressLine3,
                PurchaseNo = PurchaseInvoice.PurchaseNo,
                PurchaseDate = PurchaseInvoice.PurchaseDate,
                InvoiceNo = PurchaseInvoice.InvoiceNo,
                InvoiceDate = PurchaseInvoice.InvoiceDate,
                SupplierName = PurchaseInvoice.SupplierName,
                LocalSupplier = PurchaseInvoice.LocalSupplierName,
                NetAmount = PurchaseInvoice.NetAmount,
                InvoiceTotal = (decimal)PurchaseInvoice.InvoiceTotal,
                GrossAmount = PurchaseInvoice.GrossAmount,
                // LocalMiscCharge=purchaseInvoice.LocalMiscCharge,
                SuppDocAmount = PurchaseInvoice.SuppDocAmount,
                SuppShipAmount = PurchaseInvoice.SuppShipAmount,
                LocalOtherCharges = PurchaseInvoice.LocalOtherCharges,
                LocalCustomsDuty = PurchaseInvoice.LocalCustomsDuty,
                LocalMiscCharge = PurchaseInvoice.LocalMiscCharge,
                LocalFreight = PurchaseInvoice.LocalFreight,


                SGSTAmount = PurchaseInvoice.SGST,
                CGSTAmount = PurchaseInvoice.CGST,
                IGSTAmount = PurchaseInvoice.IGST,
                Discount = PurchaseInvoice.Discount,
                CurrencyExchangeRate = PurchaseInvoice.CurrencyExchangeRate,
                FreightAmount = PurchaseInvoice.FreightAmount,
                CurrencyName = PurchaseInvoice.CurrencyName,
                PackingForwarding = PurchaseInvoice.PackingForwarding,
                SupplierOtherCharges = PurchaseInvoice.SupplierOtherCharges,

                // FreightAmount = PurchaseInvoice.FreightAmount,
                // PackingCharges = PurchaseInvoice.PackingCharges,
                OtherCharges = PurchaseInvoice.OtherCharges,
                //TaxOnFreight = PurchaseInvoice.TaxOnFreight,
                //TaxOnPackingCharges = PurchaseInvoice.TaxOnPackingCharges,
                //TDSOnFreightPercentage = PurchaseInvoice.TDSOnFreight,
                //LessTDS = PurchaseInvoice.LessTDS,
                AmountPayable = PurchaseInvoice.AmountPayable,
                TotalDifference = (decimal)PurchaseInvoice.TotalDifference,
                OtherDeductions = (decimal)PurchaseInvoice.OtherDeductions,
                PurchaseOrderDate = PurchaseInvoice.PurchaseOrderDate,
                SupplierCode = PurchaseInvoice.SupplierCode,
                //TDSCode = PurchaseInvoice.TDSCode,
                SupplierLocation = PurchaseInvoice.SupplierLocation,
                GSTNo = PurchaseInvoice.GSTNo,
                Remarks = PurchaseInvoice.Remarks,
                //GRNDate = (DateTime)PurchaseInvoice.GRNDate,
                IsDraft = PurchaseInvoice.IsDraft,
                IsCancelled = PurchaseInvoice.IsCancelled,
                IsGSTRegistered = PurchaseInvoice.IsGSTRegistered,
                CurrencyCode=PurchaseInvoice.CurrencyCode,
                PItype = PurchaseInvoice.PItype,
                WayBillNo = PurchaseInvoice.WayBillNo,
                Freight = PurchaseInvoice.Freight,
                VatRegNo = PurchaseInvoice.VatRegNo,
                OtherChargesVATAmount = PurchaseInvoice.OtherChargesVATAmount
            };
            purchaseInvoice.Add(purchaseinvoice);
            var PurchaseInvoiceTrans = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails(Id);
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var ShippingAddress = addressBL.GetShippingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("PurchaseInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseInvoiceItemCodePrint");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            // ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoicePrintPdfDataSet", purchaseInvoice));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceTransPrintPdfDataSet", PurchaseInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdfDataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseInvoiceItemCodePrint.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseInvoice/"), FileName);
            string URL = "/Outputs/PurchaseInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PurchaseInvoicePartNoPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseInvoice = purchaseInvoiceBL.GetPurchaseInvoiceDetails(Id);
            List<SpGetPurchaseInvoiceDetailsByID_Result> purchaseInvoice = new List<SpGetPurchaseInvoiceDetailsByID_Result>();
            SpGetPurchaseInvoiceDetailsByID_Result purchaseinvoice = new SpGetPurchaseInvoiceDetailsByID_Result()
            {
                suppliercurrencyCode = PurchaseInvoice.suppliercurrencyCode,
                SuuplierCurrencyconverion = PurchaseInvoice.SuuplierCurrencyconverion,
                shipmentmode = PurchaseInvoice.shipmentmode,
                SecondaryQty = PurchaseInvoice.SecondaryQty,
                Id = PurchaseInvoice.Id,
                AddressLine1 = PurchaseInvoice.AddressLine1,
                AddressLine2 = PurchaseInvoice.AddressLine2,
                AddressLine3 = PurchaseInvoice.AddressLine3,
                PurchaseNo = PurchaseInvoice.PurchaseNo,
                PurchaseDate = PurchaseInvoice.PurchaseDate,
                InvoiceNo = PurchaseInvoice.InvoiceNo,
                InvoiceDate = PurchaseInvoice.InvoiceDate,
                SupplierName = PurchaseInvoice.SupplierName,
                LocalSupplier = PurchaseInvoice.LocalSupplierName,
                NetAmount = PurchaseInvoice.NetAmount,
                InvoiceTotal = (decimal)PurchaseInvoice.InvoiceTotal,
                GrossAmount = PurchaseInvoice.GrossAmount,
                // LocalMiscCharge=purchaseInvoice.LocalMiscCharge,
                SuppDocAmount = PurchaseInvoice.SuppDocAmount,
                SuppShipAmount = PurchaseInvoice.SuppShipAmount,
                LocalOtherCharges = PurchaseInvoice.LocalOtherCharges,
                LocalCustomsDuty = PurchaseInvoice.LocalCustomsDuty,
                LocalMiscCharge = PurchaseInvoice.LocalMiscCharge,
                LocalFreight = PurchaseInvoice.LocalFreight,


                SGSTAmount = PurchaseInvoice.SGST,
                CGSTAmount = PurchaseInvoice.CGST,
                IGSTAmount = PurchaseInvoice.IGST,
                Discount = PurchaseInvoice.Discount,
                CurrencyExchangeRate = PurchaseInvoice.CurrencyExchangeRate,
                FreightAmount = PurchaseInvoice.FreightAmount,
                CurrencyName = PurchaseInvoice.CurrencyName,
                PackingForwarding = PurchaseInvoice.PackingForwarding,
                SupplierOtherCharges = PurchaseInvoice.SupplierOtherCharges,

                // FreightAmount = PurchaseInvoice.FreightAmount,
                // PackingCharges = PurchaseInvoice.PackingCharges,
                OtherCharges = PurchaseInvoice.OtherCharges,
                //TaxOnFreight = PurchaseInvoice.TaxOnFreight,
                //TaxOnPackingCharges = PurchaseInvoice.TaxOnPackingCharges,
                //TDSOnFreightPercentage = PurchaseInvoice.TDSOnFreight,
                //LessTDS = PurchaseInvoice.LessTDS,
                AmountPayable = PurchaseInvoice.AmountPayable,
                TotalDifference = (decimal)PurchaseInvoice.TotalDifference,
                OtherDeductions = (decimal)PurchaseInvoice.OtherDeductions,
                PurchaseOrderDate = PurchaseInvoice.PurchaseOrderDate,
                SupplierCode = PurchaseInvoice.SupplierCode,
                //TDSCode = PurchaseInvoice.TDSCode,
                SupplierLocation = PurchaseInvoice.SupplierLocation,
                GSTNo = PurchaseInvoice.GSTNo,
                Remarks = PurchaseInvoice.Remarks,
                //GRNDate = (DateTime)PurchaseInvoice.GRNDate,
                IsDraft = PurchaseInvoice.IsDraft,
                IsCancelled = PurchaseInvoice.IsCancelled,
                IsGSTRegistered = PurchaseInvoice.IsGSTRegistered,
                        CurrencyCode = PurchaseInvoice.CurrencyCode,
                PItype = PurchaseInvoice.PItype,
                WayBillNo = PurchaseInvoice.WayBillNo,
                Freight = PurchaseInvoice.Freight,
                VatRegNo = PurchaseInvoice.VatRegNo,
                OtherChargesVATAmount = PurchaseInvoice.OtherChargesVATAmount
            };
            purchaseInvoice.Add(purchaseinvoice);
            var PurchaseInvoiceTrans = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails(Id);
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var ShippingAddress = addressBL.GetShippingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("PurchaseInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseInvoicePartsNo");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            // ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoicePrintPdfDataSet", purchaseInvoice));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceTransPrintPdfDataSet", PurchaseInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdfDataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseInvoicePartsNo.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseInvoice/"), FileName);
            string URL = "/Outputs/PurchaseInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult PurchaseInvoiceExportIemCodePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseInvoice = purchaseInvoiceBL.GetPurchaseInvoiceDetails(Id);
            List<SpGetPurchaseInvoiceDetailsByID_Result> purchaseInvoice = new List<SpGetPurchaseInvoiceDetailsByID_Result>();
            SpGetPurchaseInvoiceDetailsByID_Result purchaseinvoice = new SpGetPurchaseInvoiceDetailsByID_Result()
            {
                suppliercurrencyCode = PurchaseInvoice.suppliercurrencyCode,
                SuuplierCurrencyconverion = PurchaseInvoice.SuuplierCurrencyconverion,
                shipmentmode = PurchaseInvoice.shipmentmode,
                SecondaryQty = PurchaseInvoice.SecondaryQty,
                Id = PurchaseInvoice.Id,
                AddressLine1 = PurchaseInvoice.AddressLine1,
                AddressLine2 = PurchaseInvoice.AddressLine2,
                AddressLine3 = PurchaseInvoice.AddressLine3,
                PurchaseNo = PurchaseInvoice.PurchaseNo,
                PurchaseDate = PurchaseInvoice.PurchaseDate,
                InvoiceNo = PurchaseInvoice.InvoiceNo,
                InvoiceDate = PurchaseInvoice.InvoiceDate,
                SupplierName = PurchaseInvoice.SupplierName,
                LocalSupplier = PurchaseInvoice.LocalSupplierName,
                NetAmount = PurchaseInvoice.NetAmount,
                InvoiceTotal = (decimal)PurchaseInvoice.InvoiceTotal,
                GrossAmount = PurchaseInvoice.GrossAmount,
                // LocalMiscCharge=purchaseInvoice.LocalMiscCharge,
                SuppDocAmount = PurchaseInvoice.SuppDocAmount,
                SuppShipAmount = PurchaseInvoice.SuppShipAmount,
                LocalOtherCharges = PurchaseInvoice.LocalOtherCharges,
                LocalCustomsDuty = PurchaseInvoice.LocalCustomsDuty,
                LocalMiscCharge = PurchaseInvoice.LocalMiscCharge,
                LocalFreight = PurchaseInvoice.LocalFreight,
                CurrencyCode = PurchaseInvoice.CurrencyCode,


                SGSTAmount = PurchaseInvoice.SGST,
                CGSTAmount = PurchaseInvoice.CGST,
                IGSTAmount = PurchaseInvoice.IGST,
                Discount = PurchaseInvoice.Discount,
                CurrencyExchangeRate = PurchaseInvoice.CurrencyExchangeRate,
                FreightAmount = PurchaseInvoice.FreightAmount,
                CurrencyName = PurchaseInvoice.CurrencyName,
                PackingForwarding = PurchaseInvoice.PackingForwarding,
                SupplierOtherCharges = PurchaseInvoice.SupplierOtherCharges,

                // FreightAmount = PurchaseInvoice.FreightAmount,
                // PackingCharges = PurchaseInvoice.PackingCharges,
                OtherCharges = PurchaseInvoice.OtherCharges,
                //TaxOnFreight = PurchaseInvoice.TaxOnFreight,
                //TaxOnPackingCharges = PurchaseInvoice.TaxOnPackingCharges,
                //TDSOnFreightPercentage = PurchaseInvoice.TDSOnFreight,
                //LessTDS = PurchaseInvoice.LessTDS,
                AmountPayable = PurchaseInvoice.AmountPayable,
                TotalDifference = (decimal)PurchaseInvoice.TotalDifference,
                OtherDeductions = (decimal)PurchaseInvoice.OtherDeductions,
                PurchaseOrderDate = PurchaseInvoice.PurchaseOrderDate,
                SupplierCode = PurchaseInvoice.SupplierCode,
                //TDSCode = PurchaseInvoice.TDSCode,
                SupplierLocation = PurchaseInvoice.SupplierLocation,
                GSTNo = PurchaseInvoice.GSTNo,
                Remarks = PurchaseInvoice.Remarks,
                //GRNDate = (DateTime)PurchaseInvoice.GRNDate,
                IsDraft = PurchaseInvoice.IsDraft,
                IsCancelled = PurchaseInvoice.IsCancelled,
                IsGSTRegistered = PurchaseInvoice.IsGSTRegistered,
                PItype = PurchaseInvoice.PItype,
                WayBillNo = PurchaseInvoice.WayBillNo,
                Freight = PurchaseInvoice.Freight,
                VatRegNo = PurchaseInvoice.VatRegNo,
                OtherChargesVATAmount = PurchaseInvoice.OtherChargesVATAmount
            };
            purchaseInvoice.Add(purchaseinvoice);
            var PurchaseInvoiceTrans = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails(Id);
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var ShippingAddress = addressBL.GetShippingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("PurchaseInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseInvoiceExportWithItemcode");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoicePrintPdfDataSet", purchaseInvoice));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceTransPrintPdfDataSet", PurchaseInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdfDataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseInvoiceExportWithItemcode.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseInvoice/"), FileName);
            string URL = "/Outputs/PurchaseInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PurchaseInvoiceExportPartNoPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseInvoice);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseInvoice = purchaseInvoiceBL.GetPurchaseInvoiceDetails(Id);
            List<SpGetPurchaseInvoiceDetailsByID_Result> purchaseInvoice = new List<SpGetPurchaseInvoiceDetailsByID_Result>();
            SpGetPurchaseInvoiceDetailsByID_Result purchaseinvoice = new SpGetPurchaseInvoiceDetailsByID_Result()
            {
                suppliercurrencyCode = PurchaseInvoice.suppliercurrencyCode,
                SuuplierCurrencyconverion = PurchaseInvoice.SuuplierCurrencyconverion,
                shipmentmode = PurchaseInvoice.shipmentmode,
                SecondaryQty = PurchaseInvoice.SecondaryQty,
                Id = PurchaseInvoice.Id,
                AddressLine1 = PurchaseInvoice.AddressLine1,
                AddressLine2 = PurchaseInvoice.AddressLine2,
                AddressLine3 = PurchaseInvoice.AddressLine3,
                PurchaseNo = PurchaseInvoice.PurchaseNo,
                PurchaseDate = PurchaseInvoice.PurchaseDate,
                InvoiceNo = PurchaseInvoice.InvoiceNo,
                InvoiceDate = PurchaseInvoice.InvoiceDate,
                SupplierName = PurchaseInvoice.SupplierName,
                LocalSupplier = PurchaseInvoice.LocalSupplierName,
                NetAmount = PurchaseInvoice.NetAmount,
                InvoiceTotal = (decimal)PurchaseInvoice.InvoiceTotal,
                GrossAmount = PurchaseInvoice.GrossAmount,
                // LocalMiscCharge=purchaseInvoice.LocalMiscCharge,
                SuppDocAmount = PurchaseInvoice.SuppDocAmount,
                SuppShipAmount = PurchaseInvoice.SuppShipAmount,
                LocalOtherCharges = PurchaseInvoice.LocalOtherCharges,
                LocalCustomsDuty = PurchaseInvoice.LocalCustomsDuty,
                LocalMiscCharge = PurchaseInvoice.LocalMiscCharge,
                LocalFreight = PurchaseInvoice.LocalFreight,
                CurrencyCode = PurchaseInvoice.CurrencyCode,


                SGSTAmount = PurchaseInvoice.SGST,
                CGSTAmount = PurchaseInvoice.CGST,
                IGSTAmount = PurchaseInvoice.IGST,
                Discount = PurchaseInvoice.Discount,
                CurrencyExchangeRate = PurchaseInvoice.CurrencyExchangeRate,
                FreightAmount = PurchaseInvoice.FreightAmount,
                CurrencyName = PurchaseInvoice.CurrencyName,
                PackingForwarding = PurchaseInvoice.PackingForwarding,
                SupplierOtherCharges = PurchaseInvoice.SupplierOtherCharges,

                // FreightAmount = PurchaseInvoice.FreightAmount,
                // PackingCharges = PurchaseInvoice.PackingCharges,
                OtherCharges = PurchaseInvoice.OtherCharges,
                //TaxOnFreight = PurchaseInvoice.TaxOnFreight,
                //TaxOnPackingCharges = PurchaseInvoice.TaxOnPackingCharges,
                //TDSOnFreightPercentage = PurchaseInvoice.TDSOnFreight,
                //LessTDS = PurchaseInvoice.LessTDS,
                AmountPayable = PurchaseInvoice.AmountPayable,
                TotalDifference = (decimal)PurchaseInvoice.TotalDifference,
                OtherDeductions = (decimal)PurchaseInvoice.OtherDeductions,
                PurchaseOrderDate = PurchaseInvoice.PurchaseOrderDate,
                SupplierCode = PurchaseInvoice.SupplierCode,
                //TDSCode = PurchaseInvoice.TDSCode,
                SupplierLocation = PurchaseInvoice.SupplierLocation,
                GSTNo = PurchaseInvoice.GSTNo,
                Remarks = PurchaseInvoice.Remarks,
                //GRNDate = (DateTime)PurchaseInvoice.GRNDate,
                IsDraft = PurchaseInvoice.IsDraft,
                IsCancelled = PurchaseInvoice.IsCancelled,
                IsGSTRegistered = PurchaseInvoice.IsGSTRegistered,
                PItype = PurchaseInvoice.PItype,
                WayBillNo = PurchaseInvoice.WayBillNo,
                Freight = PurchaseInvoice.Freight,
                VatRegNo = PurchaseInvoice.VatRegNo,
                OtherChargesVATAmount = PurchaseInvoice.OtherChargesVATAmount
            };
            purchaseInvoice.Add(purchaseinvoice);
            var PurchaseInvoiceTrans = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails(Id);
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var ShippingAddress = addressBL.GetShippingAddress("Supplier", PurchaseInvoice.SupplierID, "").ToList();
            var SalesInvoiceGST = dbEntity.SpRptSalesInvoicePrintGSTDetails("PurchaseInvoice", Id,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseInvoiceExportWithParsNo");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, ReportLogoPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoicePrintPdfDataSet", purchaseInvoice));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceTransPrintPdfDataSet", PurchaseInvoiceTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceShippingAddressPrintPdfDataSet", ShippingAddress));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceGSTPrintPdfDataSet", SalesInvoiceGST));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseInvoiceExportWithParsNo.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseInvoice/"), FileName);
            string URL = "/Outputs/PurchaseInvoice/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GRNPrintPDF(int ID)
        //{
        //    //DateTime FromDate = General.ToDateTime(Date);
        //    ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.GoodsReceiptNote);
        //    Warning[] warnings;
        //    string[] streamIds;
        //    string contentType;
        //    string mimeType = string.Empty;
        //    string encoding = string.Empty;
        //    string extension = string.Empty;
        //    var GRNPrintPDF = goodsReceiptNoteBL.GetGRNPrintPDF((int)ID).Select(a => new SpGetGoodsReceiptNotePrintPDF_Result()
        //    {
        //        ItemID = a.ItemID,
        //        Item = a.Item,
        //        UnitID = (int)a.UnitID,
        //        Unit = a.Unit,
        //        Batch = a.Batch,
        //        ExpiryDate = a.ExpiryDate,
        //        PurchaseOrderQty = (decimal)a.PurchaseOrderQty,
        //        PurchaseRate = (decimal)a.PurchaseRate,
        //        ReceivedQty = a.ReceivedQty,
        //        DiscPercent = (decimal)a.DiscountPercent,
        //        DiscountAmount = (decimal)a.DiscountAmount,
        //        CGSTPercent = a.CGSTPercent,
        //        SGSTPercent = a.SGSTPercent,
        //        CGSTAmt = a.CGSTAmt,
        //        SGSTAmt = a.SGSTAmt,
        //        LooseQty = (decimal)a.LooseQty,
        //        MRP = (decimal)a.MRP,
        //        GrnNo = a.GrnNo,
        //        SupplierName = a.SupplierName,
        //        GSTNo = a.GSTNo,
        //        SupplierLocation = a.SupplierLocation,
        //        NetAmount = a.NetAmount,
        //        Date = (DateTime)a.Date

        //    }).ToList();
        //    reportViewer.LocalReport.ReportPath = GetReportPath("GRNPrintPDF");
        //    ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
        //    reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam, GSTNoParam });
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GRNPrintPDFDataSet", GRNPrintPDF));
        //    //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
        //    byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
        //    // Open generated PDF.
        //    string FileName = "GRNPrintPDF.pdf";
        //    string FilePath = Path.Combine(Server.MapPath("~/Outputs/GRNPrintPDF/"), FileName);
        //    string URL = "/Outputs/GRNPrintPDF/" + FileName;

        //    using (FileStream fs = new FileStream(FilePath, FileMode.Create))
        //    {
        //        fs.Write(bytes, 0, bytes.Length);
        //    }
        //    return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        //}
        public JsonResult GRNPrintPDF(int ID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.GoodsReceiptNote);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var GRNPrintPDF = goodsReceiptNoteBL.GetGRNDetail((int)ID).Select(a => new SpGetGRNDetail_Result()
            {
                ReceiptDate = a.ReceiptDate,
                Supplier = a.SupplierName,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                BAddressLine1 = a.BAddressLine1,
                BAddressLine2 = a.BAddressLine2,
              
                BAddressLine3 = a.BAddressLine3,
                CountryName = a.CountryName,
                Email = a.Email,
                CurrencyName = a.CurrencyName,
                LocalOtherCharges = a.LocalOtherCharges,
                NetAmount = (decimal)a.NetAmount,
                Remarks = a.Remarks,
                GrnNo=a.Code,
                SupplierCode=a.SupplierCode,
                GrossAmount=a.GrossAmt,
                DiscountAmount=a.DiscountAmt,
                LocalCustomsDuty=a.LocalCustomsDuty,
                LocalFreight=a.LocalFreight,
                LocalMiscCharge=a.LocalMiscCharge,
                currencycode=a.currencycode,
                CurrencyCodeL=a.CurrencyCodeL,
                SuuplierCurrencyconverion=a.SuuplierCurrencyconverion,
                SuppDocAmount=a.SuppDocAmount,
                SuppFreight=a.SuppFreight,
                SuppShipAmount=a.SuppShipAmount,
                SuppOtherCharge=a.SuppOtherCharges,
                PackingForwarding=a.PackingForwarding


                // LocalOtherCharges = a.LocalOtherCharges



            }).ToList();

            var GRNPrint = goodsReceiptNoteBL.GetGRNItems((int)ID).Select(a => new SpGetGRNTransDetails_Result()
            {
                ItemCode = a.ItemCode,
                ItemName = a.ItemName,
                PartsNumber = a.PartsNumber,
                ReceivedQty = a.ReceivedQty,
                PurchaseOrderQty = a.PurchaseOrderQty,
                Unit = a.Unit,
                PurchaseRate = a.PurchaseRate,
                NetPurchasePrice = a.NetAmount,
                TaxableAmount = a.TaxableAmount,
                Make = a.Make,
                PurchaseOrderNo=a.PurchaseOrderNo,
                GrossAmount=a.GrossAmount
            }).ToList();

            reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePrintPdfNew");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
          //reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam, GSTNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodreciptNotPrintpdf1DataSet", GRNPrintPDF));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodReceiptTransPrintpdf1DataSet", GRNPrint));

            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "GoodsReceiptNotePrintPdfNew.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/GRNPrintPDF/"), FileName);
            string URL = "/Outputs/GRNPrintPDF/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GRNPrintExportPDF(int ID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.GoodsReceiptNote);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var GRNPrintPDF = goodsReceiptNoteBL.GetGRNDetail((int)ID).Select(a => new SpGetGRNDetail_Result()
            {
                ReceiptDate = a.ReceiptDate,
                Supplier = a.SupplierName,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                BAddressLine1 = a.BAddressLine1,
                BAddressLine2 = a.BAddressLine2,

                BAddressLine3 = a.BAddressLine3,
                CountryName = a.CountryName,
                Email = a.Email,
                CurrencyName = a.CurrencyName,
                LocalOtherCharges = a.LocalOtherCharges,
                NetAmount = (decimal)a.NetAmount,
                Remarks = a.Remarks,
                GrnNo = a.Code,
                SupplierCode = a.SupplierCode,
                GrossAmount = a.GrossAmt,
                DiscountAmount = a.DiscountAmt,
                LocalCustomsDuty = a.LocalCustomsDuty,
                LocalFreight = a.LocalFreight,
                LocalMiscCharge = a.LocalMiscCharge,
                currencycode = a.currencycode,
                CurrencyCodeL = a.CurrencyCodeL,
                SuuplierCurrencyconverion = a.SuuplierCurrencyconverion,
                SuppDocAmount = a.SuppDocAmount,
                SuppFreight = a.SuppFreight,
                SuppShipAmount = a.SuppShipAmount,
                SuppOtherCharge = a.SuppOtherCharges,
                PackingForwarding = a.PackingForwarding


                // LocalOtherCharges = a.LocalOtherCharges



            }).ToList();

            var GRNPrint = goodsReceiptNoteBL.GetGRNItems((int)ID).Select(a => new SpGetGRNTransDetails_Result()
            {
                ItemCode = a.ItemCode,
                ItemName = a.ItemName,
                PartsNumber = a.PartsNumber,
                ReceivedQty = a.ReceivedQty,
                PurchaseOrderQty = a.PurchaseOrderQty,
                Unit = a.Unit,
                PurchaseRate = a.PurchaseRate,
                NetPurchasePrice = a.NetAmount,
                TaxableAmount = a.TaxableAmount,
                Make = a.Make,
                PurchaseOrderNo = a.PurchaseOrderNo,
                GrossAmount = a.GrossAmount
            }).ToList();

            reportViewer.LocalReport.ReportPath = GetReportPath("GoodsReceiptNotePrintPdfNewExport");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            ReportLogoPathParam = new ReportParameter("ReportLogoPath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ReportLogoPath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { ReportLogoPathParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodreciptNotPrintpdf1DataSet", GRNPrintPDF));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GoodReceiptTransPrintpdf1DataSet", GRNPrint));

            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "GoodsReceiptNotePrintPdfNewExport.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/GRNPrintPDF/"), FileName);
            string URL = "/Outputs/GRNPrintPDF/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult PurchaseInvoiceV3()
        {
            ReportViewModel rep = new ReportViewModel();
            rep.FromDate = General.FormatDate(General.FirstDayOfMonth);
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.FromItemCategoryRangeList = AtoZRange;
            rep.ToItemCategoryRangeList = AtoZRange;
            rep.FromSupplierRangeList = AtoZRange;
            rep.ToSupplierRangeList = AtoZRange;
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            rep.StatusList = new SelectList(statusBL.GetStatusList("PurchaseInvoiceReport"), "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult PurchaseInvoiceV3(ReportViewModel model)
        {
            model.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryList(), "ID", "Name");
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.UsersList = new SelectList(userBL.GetUserList(), "ID", "Name");
            model.StatusList = GetStatusList("PurchaseInvoice");
            if (model.FromDate != null)
            {
                StartDate = General.ToDateTime(model.FromDate);
                InvoiceDateFrom = General.ToDateTime(model.FromDate);
            }
            if (model.ToDate != null)
            {
                EndDate = General.ToDateTime(model.ToDate);
                InvoiceDateTo = General.ToDateTime(model.ToDate);
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Purchase Invoice" + " " + model.Summary + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            if (model.ItemID != null)
            {
                model.FromItemNameRange = null;
                model.ToItemNameRange = null;
            }
            if (model.SupplierID != null)
            {
                model.FromSupplierRange = null;
                model.ToSupplierRange = null;
            }
            if (model.ItemCategoryID != null)
            {
                model.FromItemCategoryRange = null;
                model.ToItemCategoryRange = null;
            }
            if (model.PONOFromID == 0)
                model.PONOFromID = null;
            if (model.PONOToID == 0)
                model.PONOToID = null;
            if (model.QCNOFromID == 0)
                model.QCNOFromID = null;
            if (model.QCNOToID == 0)
                model.QCNOToID = null;
            if (model.GRNNOFromID == 0)
                model.GRNNOFromID = null;
            if (model.GRNNOToID == 0)
                model.GRNNOToID = null;
            if (model.InvoiceNOFromID == 0)
                model.InvoiceNOFromID = null;
            if (model.InvoiceNOToID == 0)
                model.InvoiceNOToID = null;
            if (model.SRNNOFromID == 0)
                model.SRNNOFromID = null;
            if (model.SRNNOToID == 0)
                model.SRNNOToID = null;
            if (model.Type == "Stock")
            {
                if (model.Summary == "Summary")
                {
                    var stock = dbEntity.SpRptPurchaseInvoiceSupplierWiseSummaryV3(
                                           InvoiceDateFrom,
                                           InvoiceDateTo,
                                           model.Type,
                                           model.FromSupplierRange,
                                           model.ToSupplierRange,
                                           model.SupplierID,
                                           model.InvoiceNOFromID,
                                           model.InvoiceNOToID,
                                           model.SupplierInvoiceNO,
                                           model.InvoiceStatus,
                                           model.IsOverruled,
                                           GeneralBO.CreatedUserID,
                                           GeneralBO.FinYear,
                                           GeneralBO.LocationID,
                                           GeneralBO.ApplicationID).ToList();
                    ReportNameParam = new ReportParameter("ReportName", "Purchase Invoice" + " " + model.Summary);
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceSummaryForStockItemsV3.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceWithoutItemWiseSummaryV3DataSet", stock));
                }

                else /*if (model.Summary == "Summary" && model.ReportType != "without-item-wise")*/
                {
                    var stock = dbEntity.SpRptPurchaseInvoiceV3(
                                                InvoiceDateFrom,
                                                InvoiceDateTo,
                                                model.Type,
                                                model.FromItemCategoryRange,
                                                model.ToItemCategoryRange,
                                                model.ItemCategoryID,
                                                model.FromItemNameRange,
                                                model.ToItemNameRange,
                                                model.ItemID,
                                                model.FromSupplierRange,
                                                model.ToSupplierRange,
                                                model.SupplierID,
                                                model.PONOFromID,
                                                model.PONOToID,
                                                model.QCNOFromID,
                                                model.QCNOToID,
                                                model.GRNNOFromID,
                                                model.GRNNOToID,
                                                model.InvoiceNOFromID,
                                                model.InvoiceNOToID,
                                                model.SupplierInvoiceNO,
                                                model.InvoiceStatus,
                                                GeneralBO.CreatedUserID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();

                    if (model.ReportType == "item-wise")
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceItemWiseSummaryForStockItemsV3.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceItemWiseSummaryStockItemsV3DataSet", stock));
                    }

                    else
                    {
                        ReportNameParam = new ReportParameter("ReportName", "Purchase Invoice" + " " + model.Summary + " Billwise ");
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceDetailsForStockItemsV3.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceDetailsStockItemsV3DataSet", stock));
                    }
                }
            }
            else
            {
                var service = dbEntity.SpRptPurchaseInvoice(
                                InvoiceDateFrom,
                                InvoiceDateTo,
                                model.Type,
                                model.FromItemCategoryRange,
                                model.ToItemCategoryRange,
                                model.ItemCategoryID,
                                model.FromItemNameRange,
                                model.ToItemNameRange,
                                model.ItemID,
                                model.FromSupplierRange,
                                model.ToSupplierRange,
                                model.SupplierID,
                                model.PONOFromID,
                                model.PONOToID,
                                model.QCNOFromID,
                                model.QCNOToID,
                                model.SRNNOFromID,
                                model.SRNNOToID,
                                model.InvoiceNOFromID,
                                model.InvoiceNOToID,
                                model.SupplierInvoiceNO,
                                model.InvoiceStatus,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID).ToList();
                if (model.Summary == "Summary")
                {
                    if (model.ReportType == "without-item-wise")
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceSummaryForNonStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceWithoutItemWiseSummaryForNonStockItemsDataSet", service));
                    }
                    else if (model.ReportType == "item-wise")
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceItemWiseSummaryForNonStockItems.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceItemWiseSummaryForNonStockItemsDataSet", service));
                    }
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/PurchaseInvoiceDetailsForNonStockItems.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseInvoiceDetailsStockItemsDataSet", service));
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

    }


}