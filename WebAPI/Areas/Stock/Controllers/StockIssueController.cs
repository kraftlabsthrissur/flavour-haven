using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Areas.Masters.Models;
using WebAPI.Areas.Stock.Models;
using WebAPI.Utils;

namespace WebAPI.Areas.Stock.Controllers
{
    [Authorize]
    public class StockIssueController : Controller
    {
        
        private IWareHouseContract warehouseBL;
        private IStockIssueContract stockIssueBL;
        private IGeneralContract generalBL;
        private ICategoryContract categoryBL;
        private IBatchContract batchBL;

        public StockIssueController()
        {
            warehouseBL = new WarehouseBL();
            stockIssueBL = new StockIssueBL();
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            batchBL = new BatchBL();

        }

        [HttpGet]
        public JsonResult GetWarehouseList()
        {
            List<WareHouseModel> warehouselist = new List<WareHouseModel>();

            warehouselist = warehouseBL.GetWareHouses().Select(a => new WareHouseModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Place = a.Place

            }).ToList();
            return Json(new { Status = "Success", Data = warehouselist }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStockIssueBatchDetailsByQRCodeBatchNo(string BatchNo,int FromWarehouseID)
        {
            generalBL.LogError("StockIssue", "StockIssueAPI", "GetStockIssueBatchDetailsByQRCodeBatchNo", 0, BatchNo, "", "");
            try
            {
                var obj = batchBL.GetStockIssueItemDetailsByQRCodeBatchNo(BatchNo, FromWarehouseID);
                generalBL.LogError("StockIssue", "StockIssueAPI", "GetStockIssueBatchDetailsByQRCodeBatchNo", obj.ID, BatchNo, "", "");
                BatchModel batchModel = new BatchModel();
                batchModel.ID = obj.ID;
                batchModel.BatchNo = obj.BatchNo;
                batchModel.CustomBatchNo = obj.CustomBatchNo;
                batchModel.ItemName = obj.Name;
                batchModel.ExpDate = General.FormatDate(obj.ExpiryDate, "view");
                batchModel.Unit = obj.Unit;
                batchModel.UnitID = obj.UnitID;
                batchModel.Stock = obj.Stock;
                batchModel.ItemID = obj.ItemID;
                batchModel.GSTPercentage = obj.GSTPercentage;
                batchModel.CessPercentage = obj.CessPercentage;
                batchModel.RetailLooseRate = obj.RetailLooseRate;
                batchModel.BusinessCategory = obj.BusinessCategory;
                batchModel.BusinessCategoryID = obj.BusinessCategoryID;
                return Json(new { Status = "Success", Data = batchModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("StockIssue", "StockIssue", "GetStockIssueBatchDetailsByQRCodeBatchNo", 0, BatchNo, "", "");
                generalBL.LogError("Stock", "StockIssue", "GetStockIssueBatchDetailsByQRCodeBatchNo", 0, e);
                return Json(new { Status = "failure", Data = e }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Save(StockIssueModel model)
        {
            var result = new List<object>();
            try
            {
                // TODO: Add insert logic here
                StockIssueBO issue = new StockIssueBO();
                issue.ID = model.ID;
                issue.IssueNo = model.IssueNo;
                issue.Date = General.ToDateTime(model.Date);
                issue.RequestNo = model.RequestNo ?? "";
                issue.IssueLocationID = model.IssueLocationID;
                issue.IssuePremiseID = model.IssuePremiseID;
                issue.ReceiptLocationID = GeneralBO.LocationID;
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
                if (model.PackingDetails!=null)
                { 
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
    }
}