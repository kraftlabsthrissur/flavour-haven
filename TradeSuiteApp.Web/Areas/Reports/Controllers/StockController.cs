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
    public class StockController : BaseReportController
    {
        private ReportsEntities dbEntity;
        private IDropdownContract _dropdown;
        private ICategoryContract categoryBL;
        private IReportContract reportBL;
        private ILocationContract locationBL;
        private IPremisesContract premisesBL;
        private IBatchTypeContract batchtypeBL;
        private ItemContract itemBL;
        private IWareHouseContract warehouseBL;
        private IStockIssueContract stockIssueBL;
        private IStockRequestContract stockRequestBL;
        private IServiceItemIssueContract serviceItemIssueBL;
        private IStatusContract statusBL;
        private IStockAdjustmentContract stockAdjustmentBL;

        private SelectList AtoZRange = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
        private DateTime StartDate, EndDate, FromDeliveredDate, ToDeliveredDate, StockAsAt;

        private DateTime? RequestDateFrom, RequestDateTo, DeliveredDateFrom, DeliveredDateTo, GRNDateFrom, GRNDateTo;

        public StockController(IDropdownContract dropdown)
        {
            _dropdown = dropdown;
            reportBL = new ReportBL();
            locationBL = new LocationBL();
            premisesBL = new PremisesBL();
            batchtypeBL = new BatchTypeBL();
            itemBL = new ItemBL();
            warehouseBL = new WarehouseBL();
            categoryBL = new CategoryBL();
            stockIssueBL = new StockIssueBL();
            stockRequestBL = new StockRequestBL();
            serviceItemIssueBL = new ServiceItemIssueBL();
            statusBL = new StatusBL();
            stockAdjustmentBL = new StockAdjustmentBL();
            dbEntity = new ReportsEntities();

            ViewBag.FinStartDate = GeneralBO.FinStartDate;
            ViewBag.CurrentDate = General.FormatDate(DateTime.Now);

        }

        public JsonResult GetAutoComplete(string Term = "", string Table = "")
        {
            return Json(reportBL.GetAutoComplete(Term, Table).ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Reports/StockTransfer
        [HttpGet]
        public ActionResult StockTransfer()
        {
            StockModel rep = new StockModel();
            //rep.FromDate = GeneralBO.FinStartDate;
            //rep.ToDate = General.FormatDate(DateTime.Now);
            rep.LocationID = GeneralBO.LocationID;
            rep.DeliveredDateFrom = GeneralBO.FinStartDate;
            rep.DeliveredDateTo = General.FormatDate(DateTime.Now);
            rep.LocationToID = GeneralBO.LocationID;

            rep.PremiseList = new SelectList(premisesBL.GetPremisesList(rep.LocationID), "ID", "Name");

            rep.LocationHeadList = locationBL.GetTransferableLocationList().Select(a => new LocationHead()
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                LocationType = a.LocationType,
                LocationHeadName = a.LocationName,
                LocationHeadID = (int)a.LocationHeadID
            }).ToList();
            //rep.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            rep.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.FromItemCategoryRangeList = AtoZRange;
            rep.ToItemCategoryRangeList = AtoZRange;
            rep.BatchTypeList = new SelectList(batchtypeBL.GetBatchTypeList(), "ID", "Name");
            rep.TransactionTypeList = new SelectList(
                                 new List<SelectListItem>
                                 {
                                       new SelectListItem { Text = "All", Value = "All"},
                                       new SelectListItem { Text = "StockIn", Value = "StockIn"},
                                       new SelectListItem { Text = "StockOut", Value ="StockOut"},

                                 }, "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        public JsonResult GetItemsForAutoComplete(string Hint = "", string Areas = "")
        {
            List<ItemBO> itemList = new List<ItemBO>();
            itemList = itemBL.GetStockableItemsForAutoComplete(Hint).Select(a => new ItemBO()
            {
                ID = a.ID,
                Name = a.Name,

            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemRange(string from_range)
        {
            StockModel rep = new StockModel();
            SelectList ItemNameRangeList;
            if (from_range == "")
            {
                ItemNameRangeList = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
            }
            else
            {
                char range = Convert.ToChar(from_range);
                ItemNameRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            }
            return Json(new { Status = "success", data = ItemNameRangeList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemCategoryRange(string from_range)
        {
            StockModel rep = new StockModel();
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
        public ActionResult StockTransfer(StockModel model)
        {
            if (model.FromDate != null)
                RequestDateFrom = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                RequestDateTo = General.ToDateTime(model.ToDate);

            if (model.DeliveredDateFrom != null)
                FromDeliveredDate = General.ToDateTime(model.DeliveredDateFrom);
            if (model.DeliveredDateTo != null)
                ToDeliveredDate = General.ToDateTime(model.DeliveredDateTo);

            FromDateParam = new ReportParameter("FromDate", FromDeliveredDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", ToDeliveredDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", model.StockType + " " + model.ReportType + " " + "Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            if (model.ItemCategoryID != 0)
            {
                model.ItemCategoryFromRange = null;
                model.ItemCategoryToRange = null;
            }
            if (model.ItemID != 0)//priority is for item id so item range is set to null
            {
                model.ItemNameFromRange = null;
                model.ItemNameToRange = null;
            }

            if (model.StockType == "StockTransfer")
            {
                if (model.ReportType == "Summary")
                {
                    var StockTransfer = dbEntity.SpRptStockTransferSummary(
                    RequestDateFrom,
                    RequestDateTo,
                    model.RequestNoFromID,
                    model.RequestNoToID,
                    FromDeliveredDate,
                    ToDeliveredDate,
                    model.IssueNoFromID,
                    model.IssueNoToID,
                    model.LocationFromID,
                    model.LocationToID,
                    model.PremisesFromID,
                    model.PremisesToID,
                    model.ItemCategoryID,
                    model.ItemCodeFromID,
                    model.ItemCodeToID,
                    model.ItemID,
                    model.ItemNameFromRange,
                    model.ItemNameToRange,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockTransferFormSummary");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockTransferSummaryDataSet", StockTransfer));
                }
                else if (model.ReportType == "Detail")
                {
                    var StockTransfer = dbEntity.SpRptStockTransfer(
                        RequestDateFrom,
                        RequestDateTo,
                        model.RequestNoFromID,
                        model.RequestNoToID,
                        FromDeliveredDate,
                        ToDeliveredDate,
                        model.IssueNoFromID,
                        model.IssueNoToID,
                        model.LocationFromID,
                        model.LocationToID,
                        model.PremisesFromID,
                        model.PremisesToID,
                        model.ItemCategoryID,
                        model.ItemCodeFromID,
                        model.ItemCodeToID,
                        model.ItemID,
                        model.ItemNameFromRange,
                        model.ItemNameToRange,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockTransferFormDetail");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockTransferDetailDataSet", StockTransfer));
                }
            }
            else if (model.StockType == "StockTransferByItem")
            {
                if (model.ReportType == "Summary")
                {
                    var StockTransfer = dbEntity.SpRptStockTransferByItem(RequestDateFrom, RequestDateTo, FromDeliveredDate, ToDeliveredDate,
                        model.TransactionType, model.RequestNoFromID, model.RequestNoToID, model.IssueNoFromID, model.IssueNoToID,
                        model.LocationFromID, model.LocationToID,
                        model.PremisesFromID, model.PremisesToID, model.ItemCategoryFromRange, model.ItemCategoryToRange,
                        model.ItemCategoryID, model.ItemCodeFromID, model.ItemCodeToID, model.ItemNameFromRange, model.ItemNameToRange,
                        model.ItemID, model.BatchTypeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockTransferByItemSummary");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockTransferByItemDataSet", StockTransfer));
                    //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.StockTransferByItemSummary);
                }
                else if (model.ReportType == "Detail")
                {
                    var StockTransfer = dbEntity.SpRptStockTransferByItemBatchNoWise(RequestDateFrom, RequestDateTo, FromDeliveredDate, ToDeliveredDate,
                       model.TransactionType, model.RequestNoFromID, model.RequestNoToID, model.IssueNoFromID, model.IssueNoToID,
                       model.LocationFromID, model.LocationToID,
                       model.PremisesFromID, model.PremisesToID, model.ItemCategoryFromRange, model.ItemCategoryToRange,
                       model.ItemCategoryID, model.ItemCodeFromID, model.ItemCodeToID, model.ItemNameFromRange, model.ItemNameToRange,
                       model.ItemID, model.BatchTypeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockTransferByItemDetail");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockTransferByItemBatchNoDataSet", StockTransfer));
                    //ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.StockTransferByItemDetail);
                }
            }

            else if (model.StockType == "StockTransferByLocation")
            {
                if (model.ReportType == "Summary")
                {
                    var StockTransfer = dbEntity.SpRptStockTransferByItem(RequestDateFrom, RequestDateTo, FromDeliveredDate, ToDeliveredDate,
                        model.TransactionType, model.RequestNoFromID, model.RequestNoToID, model.IssueNoFromID, model.IssueNoToID,
                        model.LocationFromID, model.LocationToID,
                        model.PremisesFromID, model.PremisesToID, model.ItemCategoryFromRange, model.ItemCategoryToRange,
                        model.ItemCategoryID, model.ItemCodeFromID, model.ItemCodeToID, model.ItemNameFromRange, model.ItemNameToRange,
                        model.ItemID, model.BatchTypeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockTransferByLocationSummary");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockTransferByItemDataSet", StockTransfer));
                    //ReportNameParam = new ReportParameter("ReportName", model.Filters);
                }
                else if (model.ReportType == "Detail")
                {
                    var StockTransfer = dbEntity.SpRptStockTransferByItemBatchNoWise(RequestDateFrom, RequestDateTo, FromDeliveredDate, ToDeliveredDate,
                        model.TransactionType, model.RequestNoToID, model.RequestNoToID, model.IssueNoFromID, model.IssueNoToID,
                        model.LocationFromID, model.LocationToID,
                        model.PremisesFromID, model.PremisesToID, model.ItemCategoryFromRange, model.ItemCategoryToRange,
                        model.ItemCategoryID, model.ItemCodeFromID, model.ItemCodeToID, model.ItemNameFromRange, model.ItemNameToRange,
                        model.ItemID, model.BatchTypeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockTransferByLocationDetail");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockTransferByItemBatchNoDataSet", StockTransfer));
                    //ReportNameParam = new ReportParameter("ReportName", model.Filters);
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult StockLedgerByDate()
        {
            StockModel rep = new StockModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            rep.LocationToID = GeneralBO.LocationID;
            rep.LocationFromID = GeneralBO.LocationID;
            rep.PremiseList = new SelectList(premisesBL.GetPremisesList(rep.LocationToID), "ID", "Name");
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.FromItemCategoryRangeList = AtoZRange;
            rep.ToItemCategoryRangeList = AtoZRange;
            rep.BatchTypeList = new SelectList(batchtypeBL.GetBatchTypeList(), "ID", "Name");
            rep.ValueList = new SelectList(
                  new List<SelectListItem>
                  {
                                      new SelectListItem { Text = "MRP", Value = "MRP"},
                                      new SelectListItem { Text = "Gross Price", Value ="Gross Price"},
                                      new SelectListItem { Text = "Stock Price", Value = "Stock Price"},
                  }, "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult StockLedgerByDate(StockModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Stock Ledger" + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            var StockLedger = new object();
            StockLedger = dbEntity.SpRptStockLedger(
                                        EndDate,
                                        model.LocationFromID,
                                        model.PremisesFromID,
                                        model.ItemCategoryFromRange,
                                        model.ItemCategoryToRange,
                                        model.ItemCategoryID,
                                        model.ItemCodeFromID,
                                        model.ItemCodeToID,
                                        model.ItemNameFromRange,
                                        model.ItemNameToRange,
                                        model.ItemID,
                                        model.BatchTypeID,
                                        model.ValueType
                                        ).ToList();



            reportViewer.LocalReport.ReportPath = GetReportPath("StockLedger" + model.Type + model.ReportType);
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockLedgerDataSet", StockLedger));

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult ItemWiseStock()
        {
            ItemMovementModel rep = new ItemMovementModel();
            rep.GRNDateFrom = GeneralBO.FinStartDate;
            rep.GRNDateTo = General.FormatDate(DateTime.Now);
            rep.FromItemCategoryRangeList = AtoZRange;
            rep.ToItemCategoryRangeList = AtoZRange;
            rep.FromItemNameRangeList = AtoZRange;
            rep.ToItemNameRangeList = AtoZRange;
            rep.LocationID = GeneralBO.LocationID;
            rep.LocationFromID = GeneralBO.LocationID;
            rep.LocationToID = GeneralBO.LocationID;
            rep.LocationHeadList = locationBL.GetTransferableLocationList().Select(a => new LocationHead()
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                LocationType = a.LocationType,
                LocationHeadName = a.LocationName,
                LocationHeadID = (int)a.LocationHeadID
            }).ToList();
            //rep.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            rep.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            //rep.ItemCategoryID = 222;
            rep.TransactionTypeList = new SelectList(_dropdown.GetTransactionTypeList(), "ID", "Name");
            rep.BatchTypeList = new SelectList(batchtypeBL.GetBatchTypeList(), "ID", "Name");
            rep.PremiseList = new SelectList(premisesBL.GetPremisesList(rep.LocationID), "ID", "Name");
            rep.StatusList = new SelectList(statusBL.GetStatusList("ItemWiseStockReport"), "Value", "Text");

            rep.ItemMovementTransactionTypeList = new SelectList(
                           new List<SelectListItem>
                           {
                                      new SelectListItem { Text = "All", Value = "All"},
                                      new SelectListItem { Text = "Purchases", Value = "Purchases"},
                                      new SelectListItem { Text = "Stock Issue", Value ="Stock Issue"},
                                      new SelectListItem { Text = "Stock Receipt", Value ="Stock Receipt"},
                                      new SelectListItem { Text = "Production Issue", Value ="Production Issue"},
                                      new SelectListItem { Text = "Production Receipt", Value = "Production Receipt"},
                                      new SelectListItem { Text = "Sales", Value = "Sales"},
                                      new SelectListItem { Text = "Purchase Return", Value = "Purchase Return"},
                                      new SelectListItem { Text = "Sales Return", Value = "Sales Return"},
                                      new SelectListItem { Text = "Stock Adjustment", Value = "Stock Adjustment"},
                           }, "Value", "Text");

            rep.ValueList = new SelectList(
                   new List<SelectListItem>
                   {
                                      new SelectListItem { Text = "MRP", Value = "MRP"},
                                      new SelectListItem { Text = "Gross Price", Value ="Gross Price"},
                                      new SelectListItem { Text = "Net Price", Value = "Net Price"},
                                      new SelectListItem { Text = "Stock Price", Value = "Stock Price"},
                   }, "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult ItemWiseStock(ItemMovementModel model)
        {
            if (model.RequestDateFrom != "" && model.RequestDateFrom != null)
            {
                StartDate = General.ToDateTime(model.RequestDateFrom);
            }
            if (model.RequestDateTo != "" && model.RequestDateTo != null)
            {
                EndDate = General.ToDateTime(model.RequestDateTo);
            }

            //////////////////
            //if (model.RequestDateFrom != null)
            //{
            //    RequestDateFrom = General.ToDateTime(model.RequestDateFrom);
            //    StartDate = General.ToDateTime(model.RequestDateFrom);
            //}
            //else
            //{
            //    StartDate = General.ToDateTime(GeneralBO.FinStartDate);
            //}
            //if (model.RequestDateTo != null)
            //{
            //    RequestDateTo = General.ToDateTime(model.RequestDateTo);
            //    EndDate = General.ToDateTime(model.RequestDateTo);
            //}
            //else
            //{
            //    EndDate = DateTime.Now;
            //}

            if (model.GRNDateFrom != null)
                GRNDateFrom = General.ToDateTime(model.GRNDateFrom);
            StartDate = General.ToDateTime(model.GRNDateFrom);
            if (model.GRNDateTo != null)
                GRNDateTo = General.ToDateTime(model.GRNDateTo);
            EndDate = General.ToDateTime(model.GRNDateTo);
            if (model.DeliveredDateFrom != null)
            {
                DeliveredDateFrom = General.ToDateTime(model.DeliveredDateFrom);
            }
            if (model.DeliveredDateTo != null)
            {
                DeliveredDateTo = General.ToDateTime(model.DeliveredDateTo);
            }

            if (model.purchaseOrderNOFromID == 0)
                model.purchaseOrderNOFromID = null;
            if (model.PurchaseOrderNoToID == 0)
                model.PurchaseOrderNoToID = null;
            if (model.GRNNoFromID == 0)
                model.GRNNoFromID = null;
            if (model.GRNNoToID == 0)
                model.GRNNoToID = null;
            if (model.RequestNoFromID == 0)
                model.RequestNoFromID = null;
            if (model.RequestNoToID == 0)
                model.RequestNoToID = null;
            if (model.LocationFromID == 0)
                model.LocationFromID = null;
            if (model.PremisesFromID == 0)
                model.PremisesFromID = null;
            if (model.PremisesToID == 0)
                model.PremisesToID = null;
            if (model.ItemCategoryID == 0)
                model.ItemCategoryID = null;
            if (model.BatchTypeID == 0)
                model.BatchTypeID = null;
            if (model.ItemCodeFromID == 0)
                model.ItemCodeFromID = null;
            if (model.ItemCodeToID == 0)
                model.ItemCodeToID = null;
            if (model.ItemNameID == 0)
                model.ItemNameID = null;
            if (model.ItemCategoryID != 0)
            {
                model.FromItemCategoryRange = null;
                model.ToItemCategoryRange = null;
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", model.ReportType + " " + "Report");
            FilterParam = new ReportParameter("Filters", model.Filters);

            if (model.ReportType == "Item Movement")
            {
                var ItemMovement = dbEntity.SpRptItemMovement(
                                            RequestDateFrom,
                                            RequestDateTo,
                                            DeliveredDateFrom,
                                            DeliveredDateTo,
                                            model.RequestNoFromID,
                                            model.RequestNoToID,
                                            model.LocationFromID,
                                            model.LocationToID,
                                            model.PremisesFromID,
                                            model.PremisesToID,
                                            model.FromItemCategoryRange,
                                            model.ToItemCategoryRange,
                                            model.ItemCategoryID,
                                            model.ItemCodeFromID,
                                            model.ItemCodeToID,
                                            model.FromItemNameRange,
                                            model.ToItemNameRange,
                                            model.ItemID,
                                            model.BatchTypeID,
                                            model.ItemMovementTransactionType,
                                            model.ValueType,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID);
                if (model.RateValue == "Yes")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ItemMovement.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemMovementDataSet", ItemMovement));
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ItemMovementRateValue.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemMovementDataSet", ItemMovement));
                }
            }
            if (model.ReportType == "ItemwiseInput")
            {
                var ItemwiseProductionInput = dbEntity.SpRptItemwiseProductionInput(
                                                            model.RequestNoFromID,
                                                            model.RequestNoToID,
                                                            RequestDateFrom,
                                                            RequestDateTo,
                                                            DeliveredDateFrom,
                                                            DeliveredDateTo,
                                                            model.LocationFromID,
                                                            model.LocationToID,
                                                            model.PremisesFromID,
                                                            model.PremisesToID,
                                                            model.FromItemCategoryRange,
                                                            model.ToItemCategoryRange,
                                                            model.ItemCategoryID,
                                                            model.ItemCodeFromID,
                                                            model.ItemCodeToID,
                                                            model.FromItemNameRange,
                                                            model.ToItemNameRange,
                                                            model.ItemNameID,
                                                            model.BatchTypeID,
                                                            model.StatusFrom,
                                                            model.StatusTo,
                                                            GeneralBO.FinYear,
                                                            GeneralBO.LocationID,
                                                            GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ItemwiseProductionInput.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemwiseProductionInputDataSet", ItemwiseProductionInput));
            }
            if (model.ReportType == "ItemwiseOutput")
            {
                var ItemwiseProductionOutput = dbEntity.SpRptItemwiseProductionOutput(
                                                            model.RequestNoFromID,
                                                            model.RequestNoToID,
                                                            RequestDateFrom,
                                                            RequestDateTo,
                                                            DeliveredDateFrom,
                                                            DeliveredDateTo,
                                                            model.LocationFromID,
                                                            model.LocationToID,
                                                            model.PremisesFromID,
                                                            model.PremisesToID,
                                                            model.FromItemCategoryRange,
                                                            model.ToItemCategoryRange,
                                                            model.ItemCategoryID,
                                                            model.ItemCodeFromID,
                                                            model.ItemCodeToID,
                                                            model.FromItemNameRange,
                                                            model.ToItemNameRange,
                                                            model.ItemNameID,
                                                            model.BatchTypeID,
                                                            model.StatusFrom,
                                                            model.StatusTo,
                                                            GeneralBO.FinYear,
                                                            GeneralBO.@LocationID,
                                                            GeneralBO.@ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ItemwiseProductionOutput.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemwiseProductionOutputDataSet", ItemwiseProductionOutput));
            }
            if (model.ReportType == "Item Status")
            {
                var status = dbEntity.SpItemStatus(
                     model.RequestNoFromID, model.RequestNoToID, RequestDateFrom, RequestDateTo, DeliveredDateFrom, DeliveredDateTo,
                    model.LocationFromID, model.LocationToID, model.PremisesFromID, model.PremisesToID, model.FromItemCategoryRange,
                    model.ToItemCategoryRange, model.ItemCategoryID, model.ItemCodeFromID, model.ItemCodeToID, model.FromItemNameRange,
                    model.ToItemNameRange, model.ItemNameID, model.BatchTypeID, model.StatusFrom, model.ValueType, model.BatchID, GeneralBO.FinYear, GeneralBO.@LocationID,
                    GeneralBO.@ApplicationID).ToList();
                if (model.RateValue == "Yes")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ItemStatus.rdlc";
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ItemStatusRateValue.rdlc";
                }
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemStatusDataSet", status));
            }
            if (model.ReportType == "ItemwisePurchase")
            {
                //if (model.GRNDateFrom != null)
                //    RequestDateFrom = General.ToDateTime(model.GRNDateFrom);
                //if (model.GRNDateTo != null)
                //    RequestDateTo = General.ToDateTime(model.GRNDateTo);
                var PurchaseDetails = dbEntity.SpItemwisePurchaseReport(
                                                    model.purchaseOrderNOFromID,
                                                    model.PurchaseOrderNoToID,
                                                    model.GRNNoFromID,
                                                    model.GRNNoToID,
                                                    GRNDateFrom,
                                                    GRNDateTo,
                                                    model.LocationFromID,
                                                    model.LocationToID,
                                                    model.PremisesFromID,
                                                    model.PremisesToID,
                                                    model.FromItemCategoryRange,
                                                    model.ToItemCategoryRange,
                                                    model.ItemCategoryID,
                                                    model.ItemCodeFromID,
                                                    model.ItemCodeToID,
                                                    model.FromItemNameRange,
                                                    model.ToItemNameRange,
                                                    model.ItemNameID,
                                                    GeneralBO.FinYear,
                                                    GeneralBO.LocationID,
                                                    GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ItemwisePurchase.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ItemwisePurchaseDataSet", PurchaseDetails));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }

        [HttpGet]
        public ActionResult StockAgeing()
        {
            StockAgeing rep = new StockAgeing();
            rep.StockAsAt = General.FormatDate(DateTime.Now);
            rep.LocationFromList = AtoZRange;
            rep.LocationToList = AtoZRange;
            rep.PremisesFromList = AtoZRange;
            rep.PremisesToList = AtoZRange;
            rep.categoryFromList = AtoZRange;
            rep.categoryToList = AtoZRange;
            rep.ItemNameFromList = AtoZRange;
            rep.ItemNameToList = AtoZRange;
            rep.BatchTypeFromList = AtoZRange;
            rep.BatchTypeToList = AtoZRange;
            rep.LocationID = GeneralBO.LocationID;
            rep.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            rep.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            rep.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            rep.BatchTypeList = new SelectList(batchtypeBL.GetBatchTypeList(), "ID", "Name");
            rep.PremisesList = new SelectList(premisesBL.GetPremisesList(rep.LocationID), "ID", "Name");
            //rep.AgeingBucketList = new SelectList(
            //                       new List<SelectListItem>
            //                       {
            //                                    new SelectListItem { Text = "AB1", Value = "AB1"},
            //                                    new SelectListItem { Text = "AB2", Value = "AB2"},
            //                                    new SelectListItem { Text = "AB3", Value = "AB3"},
            //                                    new SelectListItem { Text = "AB4", Value = "AB4"},
            //                                    new SelectListItem { Text = "AB5", Value = "AB5"},

            //                       }, "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            rep.ValueList = new SelectList(
                  new List<SelectListItem>
                  {
                                      new SelectListItem { Text = "MRP", Value = "MRP"},
                                      new SelectListItem { Text = "Gross Price", Value ="Gross Price"},
                                      new SelectListItem { Text = "Net Price", Value = "Net Price"},
                                      new SelectListItem { Text = "Stock Price", Value = "Stock Price"},
                  }, "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult StockAgeing(StockAgeing model)
        {
            if (model.StockAsAt != null)
                StockAsAt = General.ToDateTime(model.StockAsAt);
            if (model.PremiseId == 0)
                model.PremiseId = null;
            if (model.ItemCategoryID == 0)
                model.ItemCategoryID = null;
            if (model.ItemCodeFromID == 0)
                model.ItemCodeFromID = null;
            if (model.ItemCodeToID == 0)
                model.ItemCodeToID = null;
            if (model.ItemNameID == 0)
                model.ItemNameID = null;
            if (model.BatchTypeID == 0)
                model.BatchTypeID = null;


            FromDateParam = new ReportParameter("FromDate", StockAsAt.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", StockAsAt.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Stock Ageing" + " " + model.ReportType + " " + "Report");
            FilterParam = new ReportParameter("Filters", model.Filters);

            var StockAgeing = dbEntity.SpStockAgeing(
                     StockAsAt,
                     model.LocationID,
                     model.PremiseId,
                     model.ItemCategoryID,
                     model.SalesCategoryID,
                     model.ItemCodeFromID,
                     model.ItemCodeToID,
                     model.ItemNameFrom,
                     model.ItemNameTo,
                     model.ItemNameID,
                     model.BatchTypeID,
                     model.BatchID,
                     model.AgeingBucketID,
                     model.ValueType,
                     model.StockAgeingType,
                     GeneralBO.FinYear,
                     GeneralBO.ApplicationID).ToList();
            if (model.AgeingBucketID > 0)
            {
                if (model.ReportType == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/StockAgeingAB" + model.AgeingBucketID + "Summary.rdlc";
                }
                else
                {
                    if (model.Batch == "Yes")
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/StockAgeingAB" + model.AgeingBucketID + ".rdlc";
                    }
                    else
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/StockAgeingWithoutBatchAB" + model.AgeingBucketID + ".rdlc";
                    }
                }
            }
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAgeingAB" + model.AgeingBucketID + "DataSet", StockAgeing));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");


        }
        [HttpPost]
        public void PickList(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Stock.IssuePickList);

            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var issuepicklist = dbEntity.SpStockIssuePickList(Id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/IssuePickList.rdlc";

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("IssuePickListDataSet", issuepicklist));

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

        public void GetRequisitionTemplate(string Category, int WarehouseID)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var Items = dbEntity.SpGetRequisitionTemplateItems(Category, WarehouseID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/RequisitionTemplate.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("RequisitionTemplateItemsDataSet", Items));
            byte[] bytes = reportViewer.LocalReport.Render("Excel", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AddHeader("content-disposition", "attachment; filename= RequisitionTemplate" + "." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        public void GetIssueTemplate(string Category, int WarehouseID)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var Items = dbEntity.SpGetIssueTemplateItems(Category, WarehouseID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/IssueTemplate.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("IssueTemplateDataSet", Items));
            byte[] bytes = reportViewer.LocalReport.Render("Excel", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AddHeader("content-disposition", "attachment; filename= IssueTemplate" + "." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        [HttpGet]
        public ActionResult StockExpiry()
        {
            StockExpiryReportModel model = new StockExpiryReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            model.LocationList = new SelectList(locationBL.GetLocationListByUser(model.UserID), "ID", "Name");
            model.PremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(model.LocationID), "ID", "Name");
            model.BatchTypeList = new SelectList(batchtypeBL.GetBatchTypeList(), "ID", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult StockExpiry(StockExpiryReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Stock Expiry");
            FilterParam = new ReportParameter("Filters", model.Filters);

            var StockExpiry = dbEntity.SpRptStockExpiry(
                                        model.ToDate,
                                        model.LocationID,
                                        model.PremiseID,
                                        model.ItemCodeFromID,
                                        model.ItemID,
                                        model.BatchTypeID,
                                        model.UserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                        ).ToList();
            reportViewer.LocalReport.DisplayName = "StockExpiry";
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/StockExpiry.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockExpiryDataSet", StockExpiry));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }

        [HttpGet]
        public ActionResult ShortTransfer()
        {
            StockTransferShortageReportModel model = new StockTransferShortageReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            model.LocationID = GeneralBO.LocationID;
            model.FromLocationID = GeneralBO.LocationID;
            model.ToLocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            model.LocationHeadList = locationBL.GetTransferableLocationList().Select(a => new LocationHead()
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                LocationType = a.LocationType,
                LocationHeadName = a.LocationName,
                LocationHeadID = (int)a.LocationHeadID
            }).ToList();
            //model.LocationList = new SelectList(locationBL.GetLocationListByUser(model.UserID), "ID", "Name");
            return View(model);

        }

        [HttpPost]
        public ActionResult ShortTransfer(StockTransferShortageReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Short Transfer");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var ShortTransfer = dbEntity.SpRptShortageInTransfer(
                model.FromDate,
                model.ToDate,
                model.FromLocationID,
                model.ToLocationID,
                model.ItemID,
                model.SalesCategoryID,
                model.UserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ShortTransfer.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ShortTransferDataSet", ShortTransfer));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam, UserParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult StockTransferGST()
        {
            List<CategoryBO> ItemCategoryList = categoryBL.GetItemCategoryForSales();
            StockTransferGSTReportModel model = new StockTransferGSTReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationID = GeneralBO.LocationID;
            model.FromLocationID = GeneralBO.LocationID;
            model.ToLocationID = GeneralBO.LocationID;
            model.ItemCategoryList = new SelectList(ItemCategoryList, "ID", "Name");
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            model.LocationHeadList = locationBL.GetTransferableLocationList().Select(a => new LocationHead()
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                LocationType = a.LocationType,
                LocationHeadName = a.LocationName,
                LocationHeadID = (int)a.LocationHeadID
            }).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult StockTransferGST(StockTransferGSTReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Stock Transfer GST" + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            if (model.ReportType == "issue")
            {
                // model.FromLocationID = GeneralBO.LocationID;

                var IssueGST = dbEntity.SpRptStockTransferIssueGST(
                    model.FromDate,
                    model.ToDate,
                    model.IssueNoFromID,
                    model.IssueNoToID,
                    model.FromLocationID,
                    model.ToLocationID,
                    model.ItemCategoryID,
                    model.SalesCategoryID,
                    model.ItemID,
                    model.BatchID,
                    GeneralBO.CreatedUserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/StockTransferIssueGST.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockTransferIssueGSTDataSet", IssueGST));
            }
            else
            {
                var ReceiptGST = dbEntity.SpRptStockTransferReceiptGST(
                    model.FromDate,
                    model.ToDate,
                    model.ReceiptNoFromID,
                    model.ReceiptNoToID,
                    model.FromLocationID,
                    model.ToLocationID,
                    model.ItemCategoryID,
                    model.SalesCategoryID,
                    model.ItemID,
                    model.BatchID,
                    GeneralBO.CreatedUserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/StockTransferReceiptGST.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockTransferReceiptGSTDataSet", ReceiptGST));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult StockValuation()
        {
            StockValuationReportModel model = new StockValuationReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationID = GeneralBO.LocationID;
            model.ItemLocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.PremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(model.LocationID), "ID", "Name");
            //model.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            model.ItemCategoryList = new SelectList(
                  new List<SelectListItem>
                  {
                                      new SelectListItem { Text = "Select", Value = "0"},
                  }, "Value", "Text");
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            model.BatchTypeList = new SelectList(batchtypeBL.GetBatchTypeList(), "ID", "Name");
            model.ValueList = new SelectList(
                   new List<SelectListItem>
                   {
                                      new SelectListItem { Text = "MRP", Value = "MRP"},
                                      new SelectListItem { Text = "Gross Price", Value ="Gross Price"},
                   }, "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public ActionResult StockValuation(StockValuationReportModel model)
        {

            // FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Stock Valuation" + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);

            var StockValuation = dbEntity.SpRptStockValuation(
                model.ToDate,
                model.ItemLocationID,
                model.PremiseID,
                model.ItemCategoryID,
                model.SalesCategoryID,
                model.ItemCodeFromID,
                model.ItemID,
                model.BatchID,
                model.BatchTypeID,
                model.ValueType,
                model.IsQtyZero,
                model.Itemtype,
                model.UserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                ).ToList();

            reportViewer.LocalReport.ReportPath = GetReportPath("StockValuation" + model.ReportType);
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockValuationDataSet", StockValuation));

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult StockLedger()
        {
            StockLedgerReportModel model = new StockLedgerReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            model.LocationList = new SelectList(locationBL.GetLocationListByUser(model.UserID), "ID", "Name");
            model.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            model.PremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(model.LocationID), "ID", "Name");
            model.ValueList = new SelectList(
                    new List<SelectListItem>
                    {
                                      new SelectListItem { Text = "MRP", Value = "MRP"},
                                      new SelectListItem { Text = "Gross Price", Value ="Basic Price"},
                    }, "Value", "Text");
            return View(model);
        }
        [HttpPost]
        public ActionResult StockLedger(StockLedgerReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Stock Ledger" + " " + model.NMode + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            if (model.NMode == "ItemWise")
            {
                if (model.NItemType == "ItemSummary")
                {
                    var StockLedger = dbEntity.SpStockLedgerItemWiseSummary(
                   model.FromDate,
                   model.ToDate,
                   model.ItemID,
                   null,
                   null,
                   model.LocationID
                   ).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockLedgerItemWiseSummary");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockLedgerItemWiseSummaryDataSet", StockLedger));
                }
                else
                {
                    var StockLedger = dbEntity.SpStockLedgerItemWiseDetails(
                   model.FromDate,
                   model.ToDate,
                   model.ItemID,
                   null,
                   null,
                   model.LocationID
                   ).ToList();
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockLedgerItemWiseDetail");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockLedgerItemWiseDetailDataSet", StockLedger));
                }

            }
            else
            {
                if (model.ReportType == "Detail")
                {
                    string Temp = "";
                    var StockLedger = dbEntity.SpRptStockLedgerNew(
                    model.FromDate,
                    model.ToDate,
                    model.LocationID,
                    model.ItemCategoryID,
                    model.SalesCategoryID,
                    model.ItemID,
                    model.BatchID,
                    model.PremiseID,
                    model.ValueType,
                    GeneralBO.FinYear,
                    GeneralBO.ApplicationID
                    ).ToList();
                    var StockLedgerArranged = new List<SpRptStockLedgerNew_Result>();
                    SpRptStockLedgerNew_Result item;
                    for (int i = 0; i < StockLedger.Count(); i++)
                    {
                        item = StockLedger[i];
                        if (Temp != item.Location + "-" + item.Premises + "-" + item.ItemCode + "-" + item.BatchType)
                        {
                            Temp = item.Location + "-" + item.Premises + "-" + item.ItemCode + "-" + item.BatchType;
                        }
                        else
                        {
                            item.OpeningQty = StockLedger[i - 1].ClosingQty + item.OpeningQty;
                            item.OpeningKgs = StockLedger[i - 1].ClosingKgs + item.OpeningKgs;
                            item.OpeningValue = StockLedger[i - 1].ClosingValue + item.OpeningValue;
                        }

                        item.ClosingQty = (decimal)item.OpeningQty + (decimal)item.ReceiptQty - (decimal)item.SalesQty - (decimal)item.StockIssueQty;
                        item.ClosingKgs = (decimal)item.OpeningKgs + (decimal)item.ReceiptKgs - (decimal)item.SalesKgs - (decimal)item.StockIssueKgs;
                        item.ClosingValue = (decimal)item.OpeningValue + (decimal)item.ReceiptValue - (decimal)item.SalesValue - (decimal)item.StockIssueValue;

                        StockLedgerArranged.Add(item);
                    }

                    reportViewer.LocalReport.ReportPath = GetReportPath("StockLedgerDetailed");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockLedgerDetailNewDataSet", StockLedger));
                }
                else
                {
                    if (model.Type == "Micro")
                    {
                        string Temp = "";
                        var StockLedger = dbEntity.SpRptStockLedgerSummary(
                        model.FromDate,
                        model.ToDate,
                        model.LocationID,
                        model.ItemCategoryID,
                        model.SalesCategoryID,
                        model.ItemID,
                        model.BatchID,
                        model.PremiseID,
                        model.ValueType,
                        GeneralBO.FinYear,
                        GeneralBO.ApplicationID
                        ).ToList();
                        var StockLedgerArranged = new List<SpRptStockLedgerSummary_Result>();
                        SpRptStockLedgerSummary_Result item;
                        for (int i = 0; i < StockLedger.Count(); i++)
                        {
                            item = StockLedger[i];
                            if (Temp != item.Location + "-" + item.Premises + "-" + item.ItemCode + "-" + item.BatchType)
                            {
                                Temp = item.Location + "-" + item.Premises + "-" + item.ItemCode + "-" + item.BatchType;
                            }
                            else
                            {
                                item.OpeningQty = StockLedger[i - 1].ClosingQty + item.OpeningQty;
                            }
                            item.ClosingQty = (decimal)item.OpeningQty + (decimal)item.ReceiptQty - (decimal)item.SalesQty - (decimal)item.StockTransferQty;
                            StockLedgerArranged.Add(item);
                        }
                        reportViewer.LocalReport.ReportPath = GetReportPath("StockLedgerMicro");
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockLedgerMicroDataSet", StockLedger));
                    }
                    else if (model.Type == "WithoutBatch")
                    {
                        var StockLedger = dbEntity.SpRptStockLedgerNew(
                        model.FromDate,
                        model.ToDate,
                        model.LocationID,
                        model.ItemCategoryID,
                        model.SalesCategoryID,
                        model.ItemID,
                        model.BatchID,
                        model.PremiseID,
                        model.ValueType,
                        GeneralBO.FinYear,
                        GeneralBO.ApplicationID
                        ).ToList();
                        reportViewer.LocalReport.ReportPath = GetReportPath("StockLedgerWithoutBatchSummary");
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockLedgerSummaryNewDataSet", StockLedger));
                    }
                    else
                    {
                        var StockLedger = dbEntity.SpRptStockLedgerNew(
                        model.FromDate,
                        model.ToDate,
                        model.LocationID,
                        model.ItemCategoryID,
                        model.SalesCategoryID,
                        model.ItemID,
                        model.BatchID,
                        model.PremiseID,
                        model.ValueType,
                        GeneralBO.FinYear,
                        GeneralBO.ApplicationID
                        ).ToList();
                        reportViewer.LocalReport.ReportPath = GetReportPath("StockLedgerSummary");
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockLedgerSummaryNewDataSet", StockLedger));
                    }
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult StockAdjustment()
        {
            StockAdjustmentReportModel model = new StockAdjustmentReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationID = GeneralBO.LocationID;
            model.ItemLocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            model.LocationList = new SelectList(locationBL.GetLocationListByUser(model.UserID), "ID", "Name");
            model.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(model);
        }

        [HttpPost]
        public ActionResult StockAdjustment(StockAdjustmentReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Stock Adjustment Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var StockAdjustment = new object();
            StockAdjustment = dbEntity.SpRptStockAdjustment(
                                        model.FromDate,
                                        model.ToDate,
                                        model.ReportType,
                                        model.ItemLocationID,
                                        model.ItemCategoryID,
                                        model.SalesCategoryID,
                                        GeneralBO.CreatedUserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();
            if (model.Mode == "BillWise")
            {
                if (model.ReportType == "All")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustment");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentDataSet", StockAdjustment));
                }
                if (model.ReportType == "ExcessStock")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustmentExcess");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentDataSet", StockAdjustment));
                }
                if (model.ReportType == "ShortStock")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustmentShort");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentDataSet", StockAdjustment));
                }
            }
            else
            {
                if (model.ReportType == "All")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustmentItemWise");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentDataSet", StockAdjustment));
                }
                if (model.ReportType == "ExcessStock")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustmentExcessItemWise");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentDataSet", StockAdjustment));
                }
                if (model.ReportType == "ShortStock")
                {
                    reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustmentShortItemWise");
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentDataSet", StockAdjustment));
                }
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult StockAdjustmentPending()
        {
            StockAdjustmentReportModel model = new StockAdjustmentReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationID = GeneralBO.LocationID;
            model.ItemLocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            model.LocationList = new SelectList(locationBL.GetLocationListByUser(model.UserID), "ID", "Name");
            model.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(model);
        }
        [HttpPost]
        public ActionResult StockAdjustmentPending(StockAdjustmentReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Stock Adjustment Pending Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var StockAdjustment = new object();
            StockAdjustment = dbEntity.SpRptStockAdjustmentPending(
                                        model.FromDate,
                                        model.ToDate,
                                        model.ItemLocationID,
                                        GeneralBO.CreatedUserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();

            reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustmentPending");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentPendingDataSet", StockAdjustment));
            
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }


        [HttpGet]
        public ActionResult StockAdjustmentToBeScheduled()
        {
            StockAdjustmentReportModel model = new StockAdjustmentReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationID = GeneralBO.LocationID;
            model.ItemLocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            model.LocationList = new SelectList(locationBL.GetLocationListByUser(model.UserID), "ID", "Name");
            model.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(model);
        }
        [HttpPost]
        public ActionResult StockAdjustmentToBeScheduled(StockAdjustmentReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Stock Adjustment To Be Scheduled");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var StockAdjustment = new object();
            StockAdjustment = dbEntity.SpGetStockCheckingDetails(
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID).ToList();

            reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustmentToBeScheduled");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentToBeScheduledDataSet", StockAdjustment));

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }


        [HttpPost]
        public JsonResult StockRequestPrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.StockTransferRequest);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var StockRequest = stockRequestBL.GetStockRequestDetail(Id).Select(a => new spGetStockRequisitionDetail_Result
            {
                RequestNo = a.RequestNo,
                CreatedDate = (DateTime)a.Date,
                IssuePremises = a.IssuePremiseName,
                ReceiptPremises = a.ReceiptPremiseName,
                IssueLocation = a.IssueLocationName,
                ReceiptLocation = a.ReceiptLocationName,
                ProductionGroupName = a.ProductionGroup,
                BatchNo = a.Batch
            }).ToList();
            var StockRequestTrans = stockRequestBL.GetStockRequestTrans(Id).Select(a => new SpGetStockRequisitionTrans_Result
            {
                ItemName = a.Name,
                Unit = a.Unit,
                RequiredQty = (decimal)a.RequiredQty,
                Remarks = a.Remarks,
                SuggestedQty = (decimal)a.SuggestedQty,
                MalayalamName = a.MalayalamName
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("StockRequestPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockRequestPrintPdfDataSet", StockRequest));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockRequestTransPrintPdfDataSet", StockRequestTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "StockRequestPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/StockRequest/"), FileName);
            string URL = "/Outputs/StockRequest/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult StockIssuePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.StockTransferIssue);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var StockIssue = stockIssueBL.GetStockIssueDetail((int)Id).Select(a => new spGetStockIssueDetail_Result()
            {
                ID = a.ID,
                IssueNo = a.IssueNo,
                IssueDate = (DateTime)a.Date,
                IssuePremises = a.IssuePremiseName,
                ReceiptPremises = a.ReceiptPremiseName,
                IssueLocation = a.IssueLocationName,
                ReceiptLocation = a.ReceiptLocationName,
                ProductionGroupName = a.ProductionGroup,
                BatchNo = a.Batch,
                Remark = a.Remark
            }).ToList();
            var StockIssueTrans = stockIssueBL.GetIssueTrans((int)Id).Select(a => new SpGetStockIssueTrans_Result()
            {
                ItemName = a.Name,
                Unit = a.Unit,
                IssueQty = (decimal)a.IssueQty,
                RequestedQty = (decimal)a.RequestedQty,
                IssueDate = (DateTime)a.IssueDate,
                MalayalamName = a.MalayalamName,
                BatchName = a.BatchName,
                Rate = (decimal)a.Rate
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("StockIssuePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockIssuePrintPdfDataSet", StockIssue));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockIssueTransPrintPdfDataSet", StockIssueTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "StockIssuePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/StockIssue/"), FileName);
            string URL = "/Outputs/StockIssue/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ServiceIssuePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ServiceIssue);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ServiceIssue = serviceItemIssueBL.GetServiceItemIssueDetail((int)Id).Select(a => new spGetStockIssueDetail_Result()
            {
                ID = a.ID,
                IssueNo = a.IssueNo,
                IssueDate = (DateTime)a.Date,
                IssuePremises = a.IssuePremiseName,
                ReceiptPremises = a.ReceiptPremiseName,
                IssueLocation = a.IssueLocationName,
                ReceiptLocation = a.ReceiptLocationName,
                ProductionGroupName = a.ProductionGroup,
                BatchNo = a.Batch
            }).ToList();
            var ServiceIssueTrans = serviceItemIssueBL.GetServiceItemIssueTrans((int)Id).Select(a => new SpGetStockIssueTrans_Result()
            {
                ItemName = a.Name,
                Unit = a.Unit,
                IssueQty = (decimal)a.IssueQty,
                RequestedQty = (decimal)a.RequestedQty,
                IssueDate = (DateTime)a.IssueDate,
                MalayalamName = a.MalayalamName
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("ServiceIssuePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockIssuePrintPdfDataSet", ServiceIssue));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockIssueTransPrintPdfDataSet", ServiceIssueTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "ServiceIssuePrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ServiceIssue/"), FileName);
            string URL = "/Outputs/ServiceIssue/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Costing()
        {
            CostingReportModel model = new CostingReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationID = GeneralBO.LocationID;
            model.UserID = GeneralBO.CreatedUserID;
            model.LocationList = new SelectList(locationBL.GetLocationListByUser(model.UserID), "ID", "Name");
            model.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(0), "ID", "Name");
            model.CostCategoryList = new SelectList(categoryBL.GetCostCategoryList(), "ID", "Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult Costing(CostingReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Costing" + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);
            if (model.ReportType == "Summary")
            {
                var Costing = dbEntity.SpRptCostingSummary(
                model.FromDate,
                model.ToDate,
                model.ItemID,
                model.LocationID,
                GeneralBO.FinYear,
                GeneralBO.ApplicationID
                ).ToList();
                reportViewer.LocalReport.ReportPath = GetReportPath("CostingSummary");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CostingSummaryDataSet", Costing));
            }

            else
            {
                //var Costing = dbEntity.SpRptCostingDetail(
                //model.FromDate,
                //model.ToDate,
                //model.LocationID,
                //model.ItemCategoryID,
                //model.SalesCategoryID,
                //model.ItemID,
                //model.BatchID,
                //model.PremiseID,
                //GeneralBO.FinYear,
                //GeneralBO.ApplicationID
                //).ToList();
                //reportViewer.LocalReport.ReportPath = GetReportPath("CostingDetail");
                //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CostingDetailDataSet", Costing));
            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public JsonResult StockAdjustmentScheduledItemsPrintPdf(DateTime FromDate, DateTime ToDate)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.StockAdjustmentScheduledItems);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            FromDateParam = new ReportParameter("FromDateParam", General.FormatDate(FromDate));
            ToDateParam = new ReportParameter("ToDateParam", General.FormatDate(ToDate));
            var StockAdjustment = "";
            //var StockAdjustment = stockAdjustmentBL.GetScheduledStockItems((DateTime)FromDate, (DateTime)ToDate).Select(a => new SpGetStockAdjustmentScheduledItemsForPrint_Result()
            //{
            //    ItemName = a.ItemName,
            //    WareHouseName = a.WareHouse
            //}).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("StockAdjustmentScheduledItemPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam, FromDateParam, ToDateParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockAdjustmentScheduledItemDataSet", StockAdjustment));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "StockAdjustmentScheduledItemPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/StockAdjustment/"), FileName);
            string URL = "/Outputs/StockAdjustment/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ExpiringAndExpiredItems()
        {
            StockReportModel model = new StockReportModel();
            //model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            //model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult ExpiringAndExpiredItems(StockReportModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Expiring And Expired Items");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateString);
            var Production = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/ExpiringAndExpiredItems.rdlc";
            string XMLParams = XMLHelper.ParseXML(model);
            var Stock = dbEntity.SpRptGetExpiringAndExpiredItems(
                                    XMLParams
                                    ).ToList(); 
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ExpiringAndExpiredItemsDataSet", Stock));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult StockValueWithGST()
        {
            StockReportModel model = new StockReportModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult StockValueWithGST(StockReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Stock Value With GST Report");
            var StockValueWithGST = dbEntity.SpRptCostValueByStock(
                                        model.FromDate,
                                        model.ToDate,
                                        model.ItemID,
                                        model.LocationID,
                                        GeneralBO.LocationID,
                                        GeneralBO.CreatedUserID
                                        ).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Stock/StockValueWithGST.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StockValueWithGSTDataSet", StockValueWithGST));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }

    }
}