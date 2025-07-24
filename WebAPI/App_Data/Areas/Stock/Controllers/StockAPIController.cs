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

        public StockAPIController()
        {
            stockAdjustmentBL = new StockAdjustmentBL();
            generalBL = new GeneralBL();
        }

        [HttpGet]
        public JsonResult GetStockAdjustmentItemListForAPI(string FromDate, string ToDate, int ItemID = 0)
        {
            try
            {
                generalBL.LogError("Stock", "StockAPI", FromDate, ItemID, ToDate, "", "");
                List<StockAPIModel> stockadjustmentlist = new List<StockAPIModel>();
                stockadjustmentlist = stockAdjustmentBL.GetStockAdjustmentItemsForAlopathyAPI(General.ToDateTime(FromDate), General.ToDateTime(ToDate), ItemID).Select(a => new StockAPIModel()
                {
                    ItemID = (int)a.ItemID,
                    ItemName = a.ItemName,
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
                generalBL.LogError("Stock", "StockAPI", "GetStockAdjustmentItemListForAPI", 0, e);
                return Json(new { Status = "failure", Data = e }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveStockAdjustment(string ItemCode = "",string Batch = "",decimal PhysicalQty = 0)
        {
            //string json = new JavaScriptSerializer().Serialize(model);
            //generalBL.LogError("Stock", "StockAPI", "Save", 0, json, "", "");
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
    }
}
