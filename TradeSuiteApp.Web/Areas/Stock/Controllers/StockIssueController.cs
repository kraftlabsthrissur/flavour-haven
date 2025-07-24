using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class StockIssueController : Controller
    {
        private IStockRequestContract stockRequestBL;
        private IWareHouseContract warehouseBL;
        private ILocationContract locationBL;
        private IBatchTypeContract batchTypeBL;
        private IBatchContract batchBL;
        private IStockIssueContract stockIssueBL;
        private IGeneralContract generalBL;
        private ICategoryContract categoryBL;
        private IAddressContract addressBL;
        private IServiceItemIssueContract serviceItemIssueBL;

        public StockIssueController()
        {
            stockRequestBL = new StockRequestBL();
            warehouseBL = new WarehouseBL();
            locationBL = new LocationBL();
            batchTypeBL = new BatchTypeBL();
            batchBL = new BatchBL();
            stockIssueBL = new StockIssueBL();
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            addressBL = new AddressBL();
            serviceItemIssueBL = new ServiceItemIssueBL();
        }
        // GET: Stock/StockIssue
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Stock/StockIssue/Details/5
        public ActionResult Details(int? id)
        {

            bool IsExist = serviceItemIssueBL.IsServiceOrStockIssue((int)id, "Stock");
            if (IsExist == false)
            {
                return View("PageNotFound");
            }

            StockIssueViewModel stockIssue = stockIssueBL.GetStockIssueDetail((int)id).Select(a => new StockIssueViewModel()
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
                Remark = a.Remark,
                LocationStateID = addressBL.GetShippingAddress("Location", a.IssueLocationID, "").FirstOrDefault().StateID,
                ReceiptLocationList = locationBL.GetTransferableLocationList().Select(b => new LocationModel()
                {
                    ID = b.ID,
                    Name = b.Name,
                    StateID = b.StateID
                }).ToList()
            }).First();

            stockIssue.PackingDetails = stockIssueBL.GetPackingDetails((int)id, "StockIssue").Select(b => new StockIssuePackingDetailsModel()
            {
                PackUnit = b.PackUnit,
                PackSize = b.PackSize,
                PackUnitID = b.PackUnitID,
                Quantity = b.Quantity
            }).ToList();


            stockIssue.Items = stockIssueBL.GetIssueTrans((int)id).Select(a => new StockIssueItem()
            {
                Code = a.Code,
                Name = a.Name,
                PartsNumber = a.PartsNo,
                Model = a.Make + "/" + a.Model,
                Unit = a.Unit,
                BatchType = a.BatchType,
                IssueQty = a.IssueQty,
                SecondaryIssueQty = a.SecondaryIssueQty,
                SecondaryUnitSize = a.SecondaryUnitSize,
                SecondaryQty = a.SecondaryQty,
                SecondaryUnit = a.SecondaryUnit,
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
                Remark = a.Remark
            }).ToList();
            return View(stockIssue);
        }

        // GET: Stock/StockIssue/Create
        public ActionResult Create()
        {
            StockIssueViewModel StockIssue = new StockIssueViewModel();
            List<WareHouseBO> StockRequestIssuePremisesList = warehouseBL.GetWareHousesForStockRequestIssue(Convert.ToInt32(generalBL.GetConfig("DefaultStockRequestIssueStore", GeneralBO.CreatedUserID)));
            //List<WareHouseBO> StockRequestReceiptPremisesList = warehouseBL.GetWareHousesForStockRequestReceipt(Convert.ToInt32(generalBL.GetConfig("DefaultStockRequestReceiptStore")));
            List<WareHouseBO> StockRequestReceiptPremisesList = warehouseBL.GetWareHouses();

            StockIssue.IssueNo = generalBL.GetSerialNo("StockIssue", "IntraLocation");
            StockIssue.Date = General.FormatDate(DateTime.Now);
            StockIssue.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            StockIssue.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            StockIssue.ReceiptLocationID = GeneralBO.LocationID;
            StockIssue.IssueLocationID = GeneralBO.LocationID;
            //StockIssue.PremiseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            //StockIssue.ReceiptPremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(GeneralBO.LocationID), "ID", "Name");
            StockIssue.PremiseList = new SelectList(StockRequestIssuePremisesList, "ID", "Name");
            StockIssue.ReceiptPremiseList = new SelectList(StockRequestReceiptPremisesList, "ID", "Name");
            StockIssue.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            StockIssue.ReceiptLocationList = locationBL.GetTransferableLocationList().Select(a => new LocationModel()
            {
                ID = a.ID,
                Name = a.Name,
                StateID = a.StateID
            }).ToList();
            StockIssue.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            var address = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            StockIssue.LocationStateID = address != null ? address.StateID : 0;
            StockIssue.Items = new List<StockIssueItem>();
            StockIssue.PackingDetails = new List<StockIssuePackingDetailsModel>();
            ViewBag.Statuses = new List<string>() { "isk", "osk", "export" };
            return View(StockIssue);
        }

        public JsonResult GetSerialNo(string IssueType)
        {
            try
            {
                string IssueNo = generalBL.GetSerialNo("StockIssue", IssueType);
                return Json(new { Status = "success", IssueNo = IssueNo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Stock", "StockIssue", "GetSerialNo", 0, e);
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
                bool IsExist = serviceItemIssueBL.IsServiceOrStockIssue((int)id, "Stock");
                if (IsExist == false)
                {
                    return View("PageNotFound");
                }
                else
                {
                    StockIssueViewModel stockIssue = stockIssueBL.GetStockIssueDetail((int)id).Select(a => new StockIssueViewModel()
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
                        Remark = a.Remark
                    }).First();
                    stockIssue.Items = stockIssueBL.GetIssueTrans((int)id).Select(a => new StockIssueItem()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        PartsNumber = a.PartsNo,
                        Model = a.Make + "/" + a.Model,
                        Unit = a.Unit,
                        BatchType = a.BatchType,
                        IssueQty = a.IssueQty,
                        BatchName = a.BatchName,
                        RequestedQty = a.RequestedQty,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryQty = a.SecondaryQty,
                        SecondaryIssueQty = a.SecondaryIssueQty,
                        SecondaryUnitSize = a.SecondaryUnitSize,
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
                        UnitID = a.UnitID,
                        PackSize = a.PackSize,
                        PrimaryUnit = a.PrimaryUnit,
                        PrimaryUnitID = a.PrimaryUnitID,
                        Remark = a.Remark
                    }).ToList();
                    stockIssue.UnitList = new SelectList(
                                         new List<SelectListItem>
                                         {
                                                new SelectListItem { Text = "", Value = "0"}

                                         }, "Value", "Text");
                    stockIssue.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
                    stockIssue.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
                    stockIssue.PremiseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                    stockIssue.ReceiptPremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(stockIssue.ReceiptLocationID), "ID", "Name");
                    stockIssue.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
                    stockIssue.ReceiptLocationList = locationBL.GetTransferableLocationList().Select(a => new LocationModel()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        StateID = a.StateID
                    }).ToList();
                    stockIssue.PackingDetails = stockIssueBL.GetPackingDetails((int)id, "StockIssue").Select(b => new StockIssuePackingDetailsModel()
                    {
                        PackUnit = b.PackUnit,
                        PackSize = b.PackSize,
                        PackUnitID = b.PackUnitID,
                        Quantity = b.Quantity
                    }).ToList();
                    stockIssue.UnProcessedSRList = new List<StockRequestViewModel>();
                    ViewBag.Statuses = new List<string>() { "isk", "osk", "export" };
                    return View(stockIssue);
                }
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
                    StockIssueBO Temp = stockIssueBL.GetStockIssueDetail(model.ID).FirstOrDefault();
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
                issue.RequestNo = model.RequestNo ?? "";

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
                issue.IsService = false;
                issue.IsCancelled = false;
                issue.Remark = model.Remark;
                List<StockIssueItemBO> ItemList = new List<StockIssueItemBO>();
                StockIssueItemBO stockIssueItemBO;
                foreach (var itm in model.Items)
                {
                    stockIssueItemBO = new StockIssueItemBO();
                    stockIssueItemBO.ItemID = itm.ItemID;
                    stockIssueItemBO.Name = itm.Name;
                    stockIssueItemBO.BatchName = itm.BatchName;
                    stockIssueItemBO.BatchID = itm.BatchID;
                    stockIssueItemBO.IssueDate = General.ToDateTime(model.Date);
                    stockIssueItemBO.BatchTypeID = itm.BatchTypeID;
                    stockIssueItemBO.IssueQty = itm.IssueQty;
                    stockIssueItemBO.RequestedQty = itm.RequestedQty;
                    stockIssueItemBO.SecondaryIssueQty = itm.SecondaryIssueQty;
                    stockIssueItemBO.SecondaryQty = itm.SecondaryQty;
                    stockIssueItemBO.SecondaryUnit = itm.SecondaryUnit;
                    stockIssueItemBO.SecondaryUnitSize = itm.SecondaryUnitSize;
                    stockIssueItemBO.StockRequestTransID = itm.StockRequestTransID;
                    stockIssueItemBO.StockRequestID = itm.StockRequestID;
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
                    stockIssueItemBO.Remark = itm.Remark;
                    ItemList.Add(stockIssueItemBO);
                }
                List<StockIssuePackingDetailsBO> PackingList = new List<StockIssuePackingDetailsBO>();

                StockIssuePackingDetailsBO stockIssuePackingBO;
                foreach (var itm in model.PackingDetails)
                {
                    stockIssuePackingBO = new StockIssuePackingDetailsBO();
                    stockIssuePackingBO.PackSize = itm.PackSize;
                    stockIssuePackingBO.PackUnit = itm.PackUnit;
                    stockIssuePackingBO.Quantity = itm.Quantity;
                    stockIssuePackingBO.PackUnitID = itm.PackUnitID;
                    PackingList.Add(stockIssuePackingBO);
                }

                if (stockIssueBL.SaveStockIssue(issue, ItemList, PackingList))
                {
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Failed To Save" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                generalBL.LogError("Stock", "StockIssue", "Save", model.ID, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockIssueList(int ReceiptLocationID, int ReceiptPremiseID = 0, int IssueLocationID = 0, int IssuePremiseID = 0)
        {
            List<StockIssueViewModel> StockIssue = new List<StockIssueViewModel>();

            StockIssue = stockIssueBL.GetUnProcessedSIList(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID).Select(m => new StockIssueViewModel()
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
                Batch = m.Batch,
                ProductionGroup = m.ProductionGroup,
                NetAmount = m.NetAmount
            }).ToList();

            return Json(StockIssue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStockIssueForReceipt(int[] StockIssueIDS)
        {

            List<StockIssueItemBO> Items = new List<StockIssueItemBO>();

            if (StockIssueIDS.Length > 0)
            {
                foreach (var StockIssueID in StockIssueIDS)
                {
                    var list = stockIssueBL.GetUnProcessedSITransList(StockIssueID);

                    if (list != null)
                    {
                        Items.AddRange(list);
                    }
                }
            }
            return Json(Items, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetBatchwiseItemWithSUnit(int ItemID, int BatchTypeID, decimal Qty, int WarehouseID, int UnitID, decimal SecondaryQty, string SecondaryUnit, decimal SecondaryUnitSize)
        {
            try
            {
                List<StockIssueItemBO> Items = stockIssueBL.GetBatchwiseItem(ItemID, BatchTypeID, Qty, WarehouseID, UnitID);
                foreach (var item in Items)
                {
                    item.SecondaryUnit = string.IsNullOrEmpty(SecondaryUnit) && SecondaryUnitSize == 1 ? item.PrimaryUnit : SecondaryUnit;
                    item.SecondaryQty = SecondaryQty;
                    item.SecondaryIssueQty = item.IssueQty / SecondaryUnitSize;
                    item.SecondaryUnitSize = SecondaryUnitSize;
                }
                return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Stock", "StockIssue", "GetBatchwiseItem", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetBatchwiseItem(int ItemID, int BatchTypeID, decimal Qty, int WarehouseID, int UnitID)
        {
            try
            {
                List<StockIssueItemBO> Items = stockIssueBL.GetBatchwiseItem(ItemID, BatchTypeID, Qty, WarehouseID, UnitID);

                return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Stock", "StockIssue", "GetBatchwiseItem", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Cancel(int ID, string Table)
        {
            stockIssueBL.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIssueList(DatatableModel Datatable)
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

                DatatableResultBO resultBO = stockIssueBL.GetStockIssueList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Stock", "StockIssue", "GetIssueList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(StockIssueViewModel model)
        {
            return Save(model);
        }

        public JsonResult ReadExcel(string Path)
        {
            try
            {
                List<StockIssueItemBO> ItemList = stockIssueBL.ReadExcel(Path);
                return Json(new { Status = "success", Data = ItemList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Stock", "StockIssue", "ReadExcel", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemsToGrid(List<StockIssueItem> model, int IssuePremiseID)
        {
            try
            {
                List<StockIssueItemBO> stockissueitems = new List<StockIssueItemBO>();
                StockIssueItemBO stockissueitem;
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        stockissueitem = new StockIssueItemBO()
                        {
                            ItemID = item.ItemID,
                            UnitID = item.UnitID,
                            BatchTypeID = item.BatchTypeID,
                            IssueQty = item.IssueQty,
                            PrimaryUnitID = item.PrimaryUnitID

                        };
                        stockissueitems.Add(stockissueitem);
                    }

                    var items = stockIssueBL.GetItemsToGrid(stockissueitems, IssuePremiseID);
                    return Json(new { Status = "success", Data = items }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Status = "success", Message = "Invalid issue quantity" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                generalBL.LogError("Stock", "StockIssue", "GetItemsToGrid", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + stockIssueBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIssueNoAutoCompleteForReport(string CodeHint, string FromDate, string ToDate)
        {
            List<StockIssueViewModel> CodeList = new List<StockIssueViewModel>();
            CodeList = stockIssueBL.GetIssueNoAutoCompleteForReport(CodeHint, General.ToDateTime(FromDate), General.ToDateTime(ToDate)).Select(a => new StockIssueViewModel()
            {
                ID = a.ID,
                IssueNo = a.IssueNo,
            }).ToList();

            return Json(CodeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StockIssuePrintPdf(int Id)
        {
            return null;
        }
    }
}
