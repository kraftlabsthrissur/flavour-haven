using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class ServiceItemIssueController : Controller
    {
        private IGeneralContract generalBL;
        private IWareHouseContract warehouseBL;
        private ILocationContract locationBL;
        private IBatchTypeContract batchTypeBL;
        private IBatchContract batchBL;
        private ICategoryContract categoryBL;
        private IAddressContract addressBL;
        private IDropdownContract _dropdown;
        private IServiceItemIssueContract serviceItemIssueBL;
        public ServiceItemIssueController(IDropdownContract tempDropdown)
        {
            warehouseBL = new WarehouseBL();
            locationBL = new LocationBL();
            batchTypeBL = new BatchTypeBL();
            batchBL = new BatchBL();
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            addressBL = new AddressBL();
            this._dropdown = tempDropdown;
            serviceItemIssueBL = new ServiceItemIssueBL();
        }
        // GET: Stock/ServiceItemIssue
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            StockIssueViewModel StockIssue = new StockIssueViewModel();

            StockIssue.IssueNo = generalBL.GetSerialNo("ServiceItemIssue", "IntraLocation");
            StockIssue.Date = General.FormatDate(DateTime.Now);
            StockIssue.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
            StockIssue.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            StockIssue.ReceiptLocationID = GeneralBO.LocationID;
            StockIssue.IssueLocationID = GeneralBO.LocationID;
            StockIssue.PremiseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            StockIssue.ReceiptPremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(GeneralBO.LocationID), "ID", "Name");
            StockIssue.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            StockIssue.ReceiptLocationList = locationBL.GetTransferableLocationList().Select(a => new LocationModel()
            {
                ID = a.ID,
                Name = a.Name,
                StateID = a.StateID
            }).ToList();
            StockIssue.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            StockIssue.LocationStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            StockIssue.Items = new List<StockIssueItem>();
            StockIssue.TradeDiscountPercent = serviceItemIssueBL.GetTradeDiscountPercent();
            ViewBag.Statuses = new List<string>() { "isk", "osk", "export" };
            return View(StockIssue);
        }

        public JsonResult GetSerialNo(string IssueType)
        {
            try
            {
                string IssueNo = generalBL.GetSerialNo("ServiceItemIssue", IssueType);
                return Json(new { Status = "success", IssueNo = IssueNo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Save(StockIssueViewModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    StockIssueBO Temp = serviceItemIssueBL.GetServiceItemIssueDetail(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                // TODO: Add insert logic here
                StockIssueBO issue = new StockIssueBO();
                issue.ID = model.ID;
                issue.IssueNo = model.IssueNo;
                issue.Date = General.ToDateTime(model.Date);

                issue.IssueLocationID = model.IssueLocationID;
                issue.IssuePremiseID = model.IssuePremiseID;
                issue.ReceiptLocationID = model.ReceiptLocationID;
                issue.ReceiptPremiseID = model.ReceiptPremiseID;

                issue.GrossAmount = model.GrossAmount;
                issue.TradeDiscount = model.TradeDiscount;
                issue.TaxableAmount = model.TaxableAmount;
                issue.SGSTAmount = model.SGSTAmount;
                issue.CGSTAmount = model.CGSTAmount;
                issue.IGSTAmount = model.IGSTAmount;
                issue.RoundOff = model.RoundOff;
                issue.NetAmount = model.NetAmount;
                issue.IssueType = model.IssueType;
                issue.IsDraft = model.IsDraft;
                issue.IsService = true;
                issue.IsCancelled = false;
                issue.RequestNo = model.RequestNo;

                List<StockIssueItemBO> ItemList = new List<StockIssueItemBO>();
                StockIssueItemBO stockIssueItemBO;
                foreach (var itm in model.Items)
                {
                    stockIssueItemBO = new StockIssueItemBO();
                    stockIssueItemBO.ItemID = itm.ItemID;
                    stockIssueItemBO.Name = itm.Name;
                    stockIssueItemBO.IssueDate = General.ToDateTime(model.Date);
                    stockIssueItemBO.IssueQty = itm.IssueQty;
                    stockIssueItemBO.Rate = itm.Rate;
                    stockIssueItemBO.BasicPrice = itm.BasicPrice;
                    stockIssueItemBO.GrossAmount = itm.GrossAmount;
                    stockIssueItemBO.TradeDiscountPercentage = itm.TradeDiscountPercentage;
                    stockIssueItemBO.TradeDiscount = itm.TradeDiscount;
                    stockIssueItemBO.TaxableAmount = itm.TaxableAmount;
                    stockIssueItemBO.SGSTPercentage = itm.SGSTPercentage;
                    stockIssueItemBO.CGSTPercentage = itm.CGSTPercentage;
                    stockIssueItemBO.IGSTPercentage = itm.IGSTPercentage;
                    stockIssueItemBO.SGSTAmount = itm.SGSTAmount;
                    stockIssueItemBO.CGSTAmount = itm.CGSTAmount;
                    stockIssueItemBO.IGSTAmount = itm.IGSTAmount;
                    stockIssueItemBO.NetAmount = itm.NetAmount;
                    stockIssueItemBO.UnitID = itm.UnitID;
                    ItemList.Add(stockIssueItemBO);
                }

                if (serviceItemIssueBL.Save(issue, ItemList))
                {
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage =" Failed to save Stock issue." });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "ServiceItemIssue", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetServiceItemIssueList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string IssueLocationHint = Datatable.Columns[3].Search.Value;
                string IssuePremiseHint = Datatable.Columns[4].Search.Value;
                string ReceiptLocationHint = Datatable.Columns[5].Search.Value;
                string ReceiptPremiseHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = serviceItemIssueBL.GetServiceItemIssueList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                bool IsExist = serviceItemIssueBL.IsServiceOrStockIssue((int)id, "Service");
                if (IsExist == false)
                {
                    return View("PageNotFound");
                }
                else
                {
                    try
                    {
                        StockIssueViewModel stockIssue = serviceItemIssueBL.GetServiceItemIssueDetail((int)id).Select(a => new StockIssueViewModel()
                        {
                            ID = a.ID,
                            IssueNo = a.IssueNo,
                            Date = General.FormatDate(a.Date),
                            IssuePremiseName = a.IssuePremiseName,
                            ReceiptPremiseName = a.ReceiptPremiseName,
                            IssueLocationName = a.IssueLocationName,
                            ReceiptLocationName = a.ReceiptLocationName,
                            IssueLocationID = a.IssueLocationID,
                            IssuePremiseID = a.IssuePremiseID,
                            ReceiptLocationID = a.ReceiptLocationID,
                            ReceiptPremiseID = a.ReceiptPremiseID,
                            GrossAmount = a.GrossAmount,
                            TradeDiscount = a.TradeDiscount,
                            TaxableAmount = a.TaxableAmount,
                            CGSTAmount = a.CGSTAmount,
                            SGSTAmount = a.SGSTAmount,
                            IGSTAmount = a.IGSTAmount,
                            RoundOff = a.RoundOff,
                            NetAmount = a.NetAmount,
                            IsDraft = a.IsDraft,
                            RequestNo = a.RequestNo,
                            IsCancelled = a.IsCancelled,
                            LocationStateID = addressBL.GetShippingAddress("Location", a.IssueLocationID, "").FirstOrDefault().StateID,

                        }).First();
                        if(!stockIssue.IsDraft || stockIssue.IsCancelled)
                        {
                            return RedirectToAction("Index");
                        }
                        stockIssue.Items = serviceItemIssueBL.GetServiceItemIssueTrans((int)id).Select(a => new StockIssueItem()
                        {
                            Name = a.Name,
                            Unit = a.Unit,
                            BatchType = a.BatchType,
                            IssueQty = a.IssueQty,
                            BatchName = a.BatchName,
                            RequestedQty = a.RequestedQty,
                            IssueDate = General.FormatDate(a.IssueDate),
                            ItemID = a.ItemID,
                            BatchID = a.BatchID,
                            BatchTypeID = a.BatchTypeID,
                            Stock = a.Stock,
                            Rate = a.Rate,
                            BasicPrice = a.BasicPrice,
                            GrossAmount = a.GrossAmount,
                            TradeDiscountPercentage = a.TradeDiscountPercentage,
                            TradeDiscount = a.TradeDiscount,
                            TaxableAmount = a.TaxableAmount,
                            CGSTPercentage = a.CGSTPercentage,
                            SGSTPercentage = a.SGSTPercentage,
                            IGSTPercentage = a.IGSTPercentage,
                            CGSTAmount = a.CGSTAmount,
                            SGSTAmount = a.SGSTAmount,
                            IGSTAmount = a.IGSTAmount,
                            NetAmount = a.NetAmount,
                            StockRequestTransID = a.StockRequestTransID,
                            StockRequestID = a.StockRequestID,
                            StockRequisitionNo = a.StockRequisitionNo,
                            UnitID = a.UnitID
                        }).ToList();
                        stockIssue.UnitList = new SelectList(
                                             new List<SelectListItem>
                                             {
                                                new SelectListItem { Text = "", Value = "0"}

                                             }, "Value", "Text");
                        stockIssue.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
                        stockIssue.PremiseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                        stockIssue.ReceiptPremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(stockIssue.ReceiptLocationID), "ID", "Name");
                        stockIssue.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
                        stockIssue.ReceiptLocationList = locationBL.GetTransferableLocationList().Select(a => new LocationModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            StateID = a.StateID
                        }).ToList();
                        stockIssue.UnProcessedSRList = new List<StockRequestViewModel>();
                        ViewBag.Statuses = new List<string>() { "isk", "osk", "export" };
                        return View(stockIssue);
                    }
                    catch (Exception e)
                    {
                        return View(e);
                    }
                }
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                bool IsExist = serviceItemIssueBL.IsServiceOrStockIssue((int)id, "Service");
                if (IsExist == false)
                {
                    return View("PageNotFound");
                }
                else
                {
                    try
                    {
                        StockIssueViewModel stockIssue = serviceItemIssueBL.GetServiceItemIssueDetail((int)id).Select(a => new StockIssueViewModel()
                        {
                            ID = a.ID,
                            IssueNo = a.IssueNo,
                            Date = General.FormatDate(a.Date, "view"),
                            IssuePremiseName = a.IssuePremiseName,
                            ReceiptPremiseName = a.ReceiptPremiseName,
                            IssueLocationName = a.IssueLocationName,
                            ReceiptLocationName = a.ReceiptLocationName,
                            IssueLocationID = a.IssueLocationID,
                            IssuePremiseID = a.IssuePremiseID,
                            ReceiptLocationID = a.ReceiptLocationID,
                            ReceiptPremiseID = a.ReceiptPremiseID,
                            GrossAmount = a.GrossAmount,
                            TradeDiscount = a.TradeDiscount,
                            TaxableAmount = a.TaxableAmount,
                            CGSTAmount = a.CGSTAmount,
                            SGSTAmount = a.SGSTAmount,
                            IGSTAmount = a.IGSTAmount,
                            RoundOff = a.RoundOff,
                            NetAmount = a.NetAmount,
                            IsDraft = a.IsDraft,
                            RequestNo = a.RequestNo,
                            IsCancelled = a.IsCancelled,
                            LocationStateID = addressBL.GetShippingAddress("Location", a.IssueLocationID, "").FirstOrDefault().StateID,
                            ReceiptLocationList = locationBL.GetTransferableLocationList().Select(b => new LocationModel()
                            {
                                ID = b.ID,
                                Name = b.Name,
                                StateID = b.StateID
                            }).ToList()
                        }).First();
                        stockIssue.Items = serviceItemIssueBL.GetServiceItemIssueTrans((int)id).Select(a => new StockIssueItem()
                        {
                            Name = a.Name,
                            Unit = a.Unit,
                            BatchType = a.BatchType,
                            IssueQty = a.IssueQty,
                            BatchName = a.BatchName,
                            RequestedQty = a.RequestedQty,
                            IssueDate = General.FormatDate(a.IssueDate, "view"),
                            Rate = a.Rate,
                            BasicPrice = a.BasicPrice,
                            GrossAmount = a.GrossAmount,
                            TradeDiscountPercentage = a.TradeDiscountPercentage,
                            TradeDiscount = a.TradeDiscount,
                            TaxableAmount = a.TaxableAmount,
                            CGSTPercentage = a.CGSTPercentage,
                            SGSTPercentage = a.SGSTPercentage,
                            IGSTPercentage = a.IGSTPercentage,
                            CGSTAmount = a.CGSTAmount,
                            SGSTAmount = a.SGSTAmount,
                            IGSTAmount = a.IGSTAmount,
                            NetAmount = a.NetAmount,
                        }).ToList();
                        return View(stockIssue);
                    }
                    catch (Exception e)
                    {
                        return View(e);
                    }
                }
            }
        }

        public JsonResult GetUnProcessedServiceItemIssueList(int ReceiptLocationID, int ReceiptPremiseID = 0, int IssueLocationID = 0, int IssuePremiseID = 0)
        {
            List<StockIssueViewModel> StockIssue = new List<StockIssueViewModel>();

            StockIssue = serviceItemIssueBL.GetUnProcessedServiceItemIssueList(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID).Select(m => new StockIssueViewModel()
            {
                IssueNo = m.IssueNo,
                Date = General.FormatDate(m.Date, "view"),
                IssueLocationName = m.IssueLocationName,
                IssuePremiseName = m.IssuePremiseName,
                ReceiptLocationName = m.ReceiptLocationName,
                ReceiptPremiseName = m.ReceiptPremiseName,
                IssueLocationID = m.IssueLocationID,
                IssuePremiseID = m.IssuePremiseID,
                ReceiptLocationID = m.ReceiptLocationID,
                ReceiptPremiseID = m.ReceiptPremiseID,
                ID = m.ID,
                ProductionGroup = m.ProductionGroup,
                NetAmount = m.NetAmount
            }).ToList();
            return Json(StockIssue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ServiceIssuePrintPdf(int Id)
        {
            return null;
        }

        public ActionResult SaveAsDraft(StockIssueViewModel model)
        {
            return Save(model);
        }
    }
}