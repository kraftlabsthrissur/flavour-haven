using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Areas.Stock.Controllers;
using WebAPI.Areas.Stock.Models;
using WebAPI.Areas.Masters.Models;
using WebAPI.Utils;
using System.Web.Script.Serialization;

namespace WebAPI.Areas.Stock.Controllers
{
    public class StockAPIController : Controller
    {
        private IStockAdjustmentContract stockAdjustmentBL;
        private IGeneralContract generalBL;
        private ItemContract itemBL;

        public StockAPIController()
        {
            stockAdjustmentBL = new StockAdjustmentBL();
            generalBL = new GeneralBL();
            itemBL = new ItemBL();
        }

        [HttpGet]
        public JsonResult GetStockAdjustmentItemListForAPI(string FromDate, string ToDate, int ItemID = 0)
        {
            try
            {
                //generalBL.LogError("Stock", "StockAPI", FromDate, ItemID, ToDate, "", "");
                List<StockAPIModel> stockadjustmentlist = new List<StockAPIModel>();
                stockadjustmentlist = stockAdjustmentBL.GetStockAdjustmentItemsForAlopathyAPI(General.ToDateTime(FromDate), General.ToDateTime(ToDate), ItemID).Select(a => new StockAPIModel()
                {
                    ItemID = (int)a.ItemID,
                    ItemName = a.ItemName,
                    //Batch = a.Batch,
                    UnitName = a.UnitName,
                    UnitID = (int)a.UnitID,
                    CurrentQty = (decimal)a.CurrentQty,
                    PhysicalQty = (decimal)a.CurrentQty,
                    Status = a.Status,
                }).ToList();
                return Json(new { Status = "Success", Data = stockadjustmentlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //generalBL.LogError("Stock", "StockAPI", "GetStockAdjustmentItemListForAPI", 0, e);
                //return Json(new { Status = "failure", Data = e }, JsonRequestBehavior.AllowGet);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveStockAdjustment(string ItemCode = "",string Batch = "",decimal PhysicalQty = 0)
        {
            string QRCode = Batch + ':' + ItemCode;
            ItemCode = itemBL.GetItemCode(QRCode);
            //Batch = itemBL.GetBatchNo(QRCode);
            //Selec i.code AS ItemCode , b.batchNo from Item I in inner join Batch B on B.ItemID = I.ID where B.Qrcode = @QRCode
            try
            {
                StockAdjustmentBO stockAdjustment = new StockAdjustmentBO();
                stockAdjustment.ItemCode = ItemCode;
                stockAdjustment.Batch = Batch;
                stockAdjustment.PhysicalQty = PhysicalQty;
                string json = new JavaScriptSerializer().Serialize(stockAdjustment);
                generalBL.LogError("Stock", "StockAPI", "Save", 0, json, "", "");
                var ID = stockAdjustmentBL.SaveStockAdjustmentForAPI(stockAdjustment);
                return Json(new { Status = "Success", Data = ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Stock", "StockAPI", "Save", 0, e);
                return Json(new { Status = "failure", Data = e }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveStockAdjustments(string QRCode, decimal PhysicalQty = 0)
        {
            //string json = new JavaScriptSerializer().Serialize(model);
            //generalBL.LogError("Stock", "StockAPI", "Save", 0, json, "", "");
            //string QRCode = ItemCode + ':' + Batch;
            string ItemCode = ""; string Batch = "";
            ItemCode = itemBL.GetItemCode(QRCode);
            Batch = itemBL.GetBatchNo(QRCode);
            //var IsPending = stockAdjustmentBL.GetIsStockCheckingDone(ItemCode, Batch);
            var IsPending = Convert.ToInt32(stockAdjustmentBL.GetIsStockCheckingDone(ItemCode, Batch).IsPending);
            //ItemCode = Itey
            //Selec i.code AS ItemCode , b.batchNo from Item I in inner join Batch B on B.ItemID = I.ID where B.Qrcode = @QRCode
            if (IsPending==1)
            {
                try
                {
                    StockAdjustmentBO stockAdjustment = new StockAdjustmentBO();
                    stockAdjustment.ItemCode = ItemCode;
                    stockAdjustment.Batch = Batch;
                    stockAdjustment.PhysicalQty = PhysicalQty;
                    string json = new JavaScriptSerializer().Serialize(stockAdjustment);
                    generalBL.LogError("Stock", "StockAPI", "Save", 0, json, "", "");
                    var ID = stockAdjustmentBL.SaveStockAdjustmentForAPI(stockAdjustment);
                    if (ID > 0)
                        return Json(new { Status = "Success", Data = ID }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { Status = "failure", Data = ID }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    //generalBL.LogError("Stock", "StockAPI", "Save", 0, e);
                    //return Json(new { Status = "failure", Data = e }, JsonRequestBehavior.AllowGet);
                    return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else if(IsPending==2)
            {
                return Json(new { Status = "This Batch is Not Scheduled",Data=IsPending}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "This Batch is Already Scanned", Data=IsPending }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
