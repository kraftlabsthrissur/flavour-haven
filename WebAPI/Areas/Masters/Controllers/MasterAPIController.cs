using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Areas.Masters.Controllers;
using WebAPI.Areas.Masters.Models;
using WebAPI.Areas.Sales.Models;
using WebAPI.Utils;

namespace WebAPI.Areas.Masters.Controllers
{
    [Authorize]
    public class MasterAPIController : Controller
    {
        private ItemContract itemBL;
        private IBatchContract batchBL;
        private IGeneralContract generalBL;
        private IUnitContract unitBL;
        private ICategoryContract categoryBL;

        public MasterAPIController()
        {
            itemBL = new ItemBL();
            batchBL = new BatchBL();
            generalBL = new GeneralBL();
            unitBL = new UnitBL();
            categoryBL = new CategoryBL();
        }

        [HttpGet]

        public JsonResult GetItemListForAPI(int Offset,int Limit)
        {
            List<ItemModel> itemList = new List<ItemModel>();

            itemList = itemBL.GetItemListForAPI(Offset,Limit).Select(a => new ItemModel()
            {
                ItemID = a.ItemID,
                ItemCode = a.ItemCode,
                ItemName = a.ItemName,
                SalesCategoryID = a.SalesCategoryID,
                SalesCategory = a.SalesCategory,
                PackSize = a.PackSize,
                FullUnitID = a.FullUnitID,
                LooseUnitID = a.LooseUnitID,
                FullUnit = a.FullUnit,
                LooseUnit = a.LooseUnit,
                PurchaseUnitID = a.PurchaseUnitID,
                PurchaseUnit = a.PurchaseUnit,
                IsEnabled = a.IsEnabled,
                GSTPercentage = (decimal)a.GSTPercentage,
                ManufacturerID = a.ManufacturerID,
                Manufacturer = a.Manufacturer,
            }).ToList();
            return Json(new { Status = "Success", Data = itemList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnitListForAPI(int ItemID = 0)
        {
            List<UnitModel> unitList = new List<UnitModel>();

            unitList = unitBL.GetUnitListForAPI(ItemID).Select(a => new UnitModel()
            {
                UnitID = a.UnitID,
                Unit = a.Unit,
                PackSize = a.PackSize,
                Description = a.Description
            }).ToList();
            return Json(new { Status = "Success", Data = unitList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBatchListForAPI(int ItemID = 0)
        {
            List<BatchModel> batchList = new List<BatchModel>();

            batchList = batchBL.GetBatchListForAPI(ItemID).Select(a => new BatchModel()
            {
                BatchID = a.BatchID,
                BatchNo = a.BatchNo,
                ItemID = a.ItemID,
                ExpDate = General.FormatDate(a.ExpiryDate),
                FullSellingPrice = a.FullSellingPrice,
                LooseSellingPrice = a.LooseSellingPrice,
                FullPurchasePrice = a.FullPurchasePrice,
                LoosePurchasePrice = a.LoosePurchasePrice,
                IsSuspended = a.IsSuspended,
            }).ToList();
            return Json(new { Status = "Success", Data = batchList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCategoryListForAPI()
        {
            List<CategoryModel> categoryList = new List<CategoryModel>();

            categoryList = categoryBL.GetCategoryList().Select(a => new CategoryModel()
            {
                CategoryID = a.ID,
                CategoryName =a.Name,
            }).ToList();
            return Json(new { Status = "Success", Data = categoryList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStockListForAPI(int ItemID = 0)
        {
            List<StockModel> stockList = new List<StockModel>();

            stockList = itemBL.GetStockListForAPI(ItemID).Select(a => new StockModel()
            {
                id = a.id,
                itemid = a.itemid,
                batchid = a.batchid,
                MRP = a.MRP,
                SalesUnitID = a.SalesUnitID,
                GSTPercentage = a.GSTPercentage,
                BatchNo = a.BatchNo,
                ExpiryDate = General.FormatDate(a.ExpiryDate),
                warehouseid = a.warehouseid,
                transactiontype = a.transactiontype,
                issue = a.issue,
                Receipt = a.Receipt,
                value = a.value,
            }).ToList();
            return Json(new { Status = "Success", Data = stockList }, JsonRequestBehavior.AllowGet);
        }
    }
}
