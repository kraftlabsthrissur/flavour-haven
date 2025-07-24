using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Utils;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Models;
using DataAccessLayer.DBContext;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class StockRequestController : Controller
    {

        private IStockRequestContract stockRequestBL;
        private IWareHouseContract warehouseBL;
        private ILocationContract locationBL;
        private IBatchTypeContract BatchBL;
        private IGeneralContract generalBL;
        private ICategoryContract categoryBL;
        private IAddressContract addressBL;

        public StockRequestController()
        {
            stockRequestBL = new StockRequestBL();
            warehouseBL = new WarehouseBL();
            locationBL = new LocationBL();
            BatchBL = new BatchTypeBL();
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            addressBL = new AddressBL();
        }

        // GET: Stock/StockRequest
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled", "suspended" };
            return View();
        }

        // GET: Stock/StockRequest/Details/5
        public ActionResult Details(int id)
        {
            StockRequestViewModel StockRequest = stockRequestBL.GetStockRequestDetail(id).Select(m => new StockRequestViewModel()
            {
                ID = m.ID,
                RequestNo = m.RequestNo,
                Date = General.FormatDate(m.Date, "view"),
                IssuePremiseName = m.IssuePremiseName,
                ReceiptPremiseName = m.ReceiptPremiseName,
                ReceiptLocationName = m.ReceiptLocationName,
                IssueLocationName = m.IssueLocationName,
                IsDraft = m.IsDraft,
                IsCancelled = m.IsCancelled,
                IsSuspended = m.IsSuspended
            }).First();
            List<StockRequestItemBO> stockrequestItemsBO = stockRequestBL.GetStockRequestTrans((int)id);
            StockRequest.Items = stockRequestBL.GetStockRequestTrans(id).Select(m => new StockRequestItem()
            {
                Name = m.Name,
                Unit = m.Unit,
                RequiredQty = m.RequiredQty,
                SecondaryUnit = m.SecondaryUnit,
                SecondaryQty = m.SecondaryQty,
                RequiredDate = m.RequiredDate == null ? "" : General.FormatDate((DateTime)m.RequiredDate, "view"),
                Remarks = m.Remarks,
                BatchName = m.BatchName,
                BatchTypeID = m.BatchTypeID,
                Stock = m.Stock,
                AverageSales = m.AverageSales,
                SalesCategory = m.SalesCategory,
                SuggestedQty = m.SuggestedQty
            }).ToList();
            return View(StockRequest);
        }
        private List<SecondaryUnitBO> SecondaryUnitList(string Unit, string SecondaryUnits)
        {
            List<SecondaryUnitBO> secondaryUnits = new List<SecondaryUnitBO>();
            SecondaryUnitBO primaryUnitBO = new SecondaryUnitBO();
            primaryUnitBO.Name = Unit;
            primaryUnitBO.PackSize = 1;
            secondaryUnits.Add(primaryUnitBO);
            string[] SecondaryUnitsArray = SecondaryUnits.Split(',');
            for (int i = 0; i < SecondaryUnitsArray.Length; i++)
            {
                var SecondaryUnitItem = SecondaryUnitsArray[i].Split('|'); ;
                if (SecondaryUnitItem.Length > 1)
                {
                    SecondaryUnitBO secondaryUnitBO = new SecondaryUnitBO();
                    var text = SecondaryUnitItem[0];
                    var value = SecondaryUnitItem[1];
                    secondaryUnitBO.Name = text;
                    secondaryUnitBO.PackSize = Convert.ToDecimal(value);
                    secondaryUnits.Add(secondaryUnitBO);
                }
            }
            return secondaryUnits;
        }

        // GET: Stock/StockRequest/Create
        public ActionResult Create()
        {
            StockRequestViewModel stockRequest = new StockRequestViewModel();
            List<WareHouseBO> PremisesList = warehouseBL.GetWareHouses();
            List<WareHouseBO> StockRequestIssuePremisesList = warehouseBL.GetWareHousesForStockRequestIssue(Convert.ToInt32(generalBL.GetConfig("DefaultStockRequestIssueStore", GeneralBO.CreatedUserID)));
            //List<WareHouseBO> StockRequestIssuePremisesList = warehouseBL.GetWareHouses();
            //List<WareHouseBO> StockRequestReceiptPremisesList = warehouseBL.GetWareHousesForStockRequestReceipt(Convert.ToInt32(generalBL.GetConfig("DefaultStockRequestReceiptStore", GeneralBO.CreatedUserID)));
            List<WareHouseBO> StockRequestReceiptPremisesList = warehouseBL.GetWareHouses();
            stockRequest.RequestNo = generalBL.GetSerialNo("StockRequsitionIntra", "Code");

            stockRequest.Date = General.FormatDate(DateTime.Now);

            if (Convert.ToInt32(locationBL.IsBranchLocation(GeneralBO.LocationID)) == 1)
            {
                stockRequest.ItemCategoryID = Convert.ToInt32(generalBL.GetConfig("DefaultCategoryForBranch"));
            }
            stockRequest.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");

            stockRequest.BatchTypeList = new SelectList(BatchBL.GetBatchTypeList(), "ID", "Name");
            stockRequest.ReceiptLocationID = GeneralBO.LocationID;
            stockRequest.IssueLocationID = locationBL.GetHeadLocation(GeneralBO.LocationID).ID;

            stockRequest.PremiseList = new SelectList(StockRequestReceiptPremisesList, "ID", "Name");
            if (PremisesList.Count() == 1)
            {
                stockRequest.ReceiptPremiseID = StockRequestReceiptPremisesList.FirstOrDefault() != null ? StockRequestReceiptPremisesList.FirstOrDefault().ID : 0;
            }
            stockRequest.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            PremisesList = warehouseBL.GetWareHousesByLocation(stockRequest.IssueLocationID);
            stockRequest.IssuePremiseList = new SelectList(StockRequestIssuePremisesList, "ID", "Name");
            if (PremisesList.Count() == 1)
            {
                stockRequest.IssuePremiseID = StockRequestIssuePremisesList.FirstOrDefault().ID;
            }
            else if (stockRequest.IssueLocationID != GeneralBO.LocationID)
            {
                stockRequest.IssuePremiseID = PremisesList.FirstOrDefault().ID;
            }
            var Address = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            int LocationStateID = Address != null ? Address.StateID : 0;
            stockRequest.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            stockRequest.DefaultBatchTypeID = 1;
            if (LocationStateID != 32)
            {
                stockRequest.DefaultBatchTypeID = 2;
            }
            stockRequest.Items = new List<StockRequestItem>();
            return View(stockRequest);
        }

        [HttpPost]
        public JsonResult GetStockRequisitionList(int IssueLocationID, int IssuePremiseID = 0, int ReceiptLocationID = 0, int ReceiptPremiseID = 0)
        {
            List<StockRequestViewModel> StockRequests = new List<StockRequestViewModel>();
            try
            {
                StockRequests = stockRequestBL.GetUnProcessedSRList(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID).Select(m => new StockRequestViewModel()
                {
                    RequestNo = m.RequestNo,
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
                    Batch = m.Batch
                }).ToList();
                return Json(new { Status = "success", Data = StockRequests }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetRequisitionList(DatatableModel Datatable)
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

                DatatableResultBO resultBO = stockRequestBL.GetStockRequisitionList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockRequestItems(int[] StockRequisitionIDs)
        {
            try
            {
                List<StockRequestItemBO> UnProcesedSRTransList = stockRequestBL.GetUnProcessedSRTransList(StockRequisitionIDs);

                return Json(new { Status = "success", Data = UnProcesedSRTransList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Stock/StockRequest/Create
        [HttpPost]
        public ActionResult Save(StockRequestViewModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    StockRequestBO Temp = stockRequestBL.GetStockRequestDetail(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft || Temp.IsCancelled || Temp.IsSuspended)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                // TODO: Add insert logic here
                StockRequestBO StockRequest = new StockRequestBO();
                StockRequest.IsCancelled = false;
                StockRequest.RequestNo = model.RequestNo;
                StockRequest.Date = General.ToDateTime(model.Date);
                StockRequest.IssuePremiseID = model.IssuePremiseID;
                StockRequest.ReceiptPremiseID = model.ReceiptPremiseID;
                StockRequest.ReceiptLocationID = model.ReceiptLocationID;
                StockRequest.IssueLocationID = model.IssueLocationID;
                StockRequest.IsDraft = model.IsDraft;
                StockRequest.ID = model.ID;
                var ItemList = new List<StockRequestItemBO>();
                StockRequestItemBO stockRequestItemBO;
                foreach (var item in model.Items)
                {
                    stockRequestItemBO = new StockRequestItemBO();
                    stockRequestItemBO.ItemID = item.ItemID;
                    stockRequestItemBO.RequiredQty = item.RequiredQty;
                    stockRequestItemBO.SecondaryUnit = item.SecondaryUnit;
                    stockRequestItemBO.SecondaryUnitSize = item.SecondaryUnitSize;
                    stockRequestItemBO.SecondaryQty = item.SecondaryQty;
                    stockRequestItemBO.BatchTypeID = item.BatchTypeID;
                    stockRequestItemBO.Remarks = item.Remarks;
                    stockRequestItemBO.Stock = item.Stock;
                    stockRequestItemBO.AverageSales = item.AverageSales;
                    stockRequestItemBO.UnitID = item.UnitID;
                    stockRequestItemBO.SuggestedQty = item.SuggestedQty;
                    if (item.RequiredDate != null)
                    {
                        stockRequestItemBO.RequiredDate = General.ToDateTime(item.RequiredDate);
                    }

                    ItemList.Add(stockRequestItemBO);
                }
                stockRequestBL.SaveStockRequest(StockRequest, ItemList);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "StockRequest", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Stock/StockRequest/Edit/5
        public ActionResult Edit(int id)
        {
            StockRequestViewModel StockRequest = stockRequestBL.GetStockRequestDetail(id).Select(m => new StockRequestViewModel()
            {
                ID = m.ID,
                RequestNo = m.RequestNo,
                Date = General.FormatDate(m.Date),
                IssuePremiseName = m.IssuePremiseName,
                ReceiptPremiseName = m.ReceiptPremiseName,
                ReceiptLocationID = m.ReceiptLocationID,
                ReceiptPremiseID = m.ReceiptPremiseID,
                IssueLocationID = m.IssueLocationID,
                IssuePremiseID = m.IssuePremiseID,
                ReceiptLocationName = m.ReceiptLocationName,
                IssueLocationName = m.IssueLocationName,
                IsDraft = m.IsDraft,
                IsCancelled = m.IsCancelled,
                IsSuspended = m.IsSuspended,
            }).First();
            if (!StockRequest.IsDraft || StockRequest.IsCancelled || StockRequest.IsSuspended)
            {
                return RedirectToAction("Index");
            }
            if (Convert.ToInt32(locationBL.IsBranchLocation(GeneralBO.LocationID)) == 1)
            {
                StockRequest.ItemCategoryID = Convert.ToInt32(generalBL.GetConfig("DefaultCategoryForBranch"));
            }

            StockRequest.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");

            StockRequest.BatchTypeList = new SelectList(BatchBL.GetBatchTypeList(), "ID", "Name");
            StockRequest.PremiseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            StockRequest.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            StockRequest.IssuePremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(StockRequest.IssueLocationID), "ID", "Name");

            List<StockRequestItemBO> stockrequestItemsBO = stockRequestBL.GetStockRequestTrans((int)id);
            int LocationStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            StockRequest.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");

            StockRequest.DefaultBatchTypeID = 1;
            if (LocationStateID != 32)
            {
                StockRequest.DefaultBatchTypeID = 2;
            }
            StockRequest.Items = stockRequestBL.GetStockRequestTrans(id).Select(m => new StockRequestItem()
            {
                Name = m.Name,
                Code = m.Code,
                PartsNumber = m.PartsNo,
                Model = m.Make + " / " + m.Model,
                Unit = m.Unit,
                RequiredQty = m.RequiredQty,
                SecondaryUnit = m.SecondaryUnit,
                SecondaryQty = m.SecondaryQty,
                SecondaryUnitList = SecondaryUnitList(m.Unit, m.SecondaryUnits),
                RequiredDate = m.RequiredDate == null ? "" : General.FormatDate((DateTime)m.RequiredDate),
                Remarks = m.Remarks,
                BatchName = m.BatchName,
                ItemID = m.ItemID,
                BatchTypeID = m.BatchTypeID,
                Stock = m.Stock,
                AverageSales = m.AverageSales,
                UnitID = m.UnitID,
                SalesCategory = m.SalesCategory,
                SuggestedQty = m.SuggestedQty
            }).ToList();
            StockRequest.IsClone = false;
            return View(StockRequest);
        }

        public ActionResult Clone(int id)
        {
            StockRequestViewModel StockRequest = stockRequestBL.GetStockRequestDetail(id).Select(m => new StockRequestViewModel()
            {
                ID = 0,
                IssuePremiseName = m.IssuePremiseName,
                ReceiptPremiseName = m.ReceiptPremiseName,
                ReceiptLocationID = m.ReceiptLocationID,
                ReceiptPremiseID = m.ReceiptPremiseID,
                IssueLocationID = m.IssueLocationID,
                IssuePremiseID = m.IssuePremiseID,
                ReceiptLocationName = m.ReceiptLocationName,
                IssueLocationName = m.IssueLocationName
            }).First();
            StockRequest.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            StockRequest.RequestNo = generalBL.GetSerialNo("StockRequsitionIntra", "Code");
            StockRequest.Date = General.FormatDate(DateTime.Now);
            StockRequest.BatchTypeList = new SelectList(BatchBL.GetBatchTypeList(), "ID", "Name");
            StockRequest.PremiseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            StockRequest.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            StockRequest.IssuePremiseList = new SelectList(warehouseBL.GetWareHousesByLocation(StockRequest.IssueLocationID), "ID", "Name");
            List<StockRequestItemBO> stockrequestItemsBO = stockRequestBL.GetStockRequestTrans((int)id);
            int LocationStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            StockRequest.DefaultBatchTypeID = 1;
            StockRequest.UnitList = new SelectList(
                                      new List<SelectListItem>
                                      {
                                                new SelectListItem { Text = "", Value = "0"}

                                      }, "Value", "Text");
            if (LocationStateID != 32)
            {
                StockRequest.DefaultBatchTypeID = 2;
            }
            StockRequest.Items = stockRequestBL.GetStockRequestTrans(id).Select(m => new StockRequestItem()
            {
                Name = m.Name,
                Unit = m.Unit,
                RequiredQty = m.RequiredQty,
                RequiredDate = m.RequiredDate == null ? "" : General.FormatDate((DateTime)m.RequiredDate),
                Remarks = m.Remarks,
                BatchName = m.BatchName,
                ItemID = m.ItemID,
                BatchTypeID = m.BatchTypeID,
                Stock = m.Stock,
                AverageSales = m.AverageSales,
                UnitID = m.UnitID
            }).ToList();
            StockRequest.IsClone = true;
            return View(StockRequest);
        }

        public ActionResult Suspend(int ID, String Table)
        {
            var output = stockRequestBL.Suspend(ID, Table);
            return Json(new { Status = "success", Data = output }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cancel(int ID, string Table)
        {
            stockRequestBL.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadExcel(string Path)
        {
            try
            {
                List<StockRequestItemBO> ItemList = stockRequestBL.ReadExcel(Path);
                return Json(new { Status = "success", Data = ItemList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult StockRequestPrintPdf(int Id)
        {
            return null;
        }

        public ActionResult SaveAsDraft(StockRequestViewModel model)
        {
            return Save(model);
        }

    }
}
