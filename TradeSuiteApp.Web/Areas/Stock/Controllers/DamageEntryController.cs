using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class DamageEntryController : Controller
    {
        private IDamageEntryContract damageEntryBL;
        private ICategoryContract categoryBL;
        private IWareHouseContract warehouseBL;
        private IGeneralContract generalBL;
        private IDamageTypeContract DamageTypeBL;

        public DamageEntryController()
        {
            categoryBL = new CategoryBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            damageEntryBL = new DamageEntryBL();
            DamageTypeBL = new DamageTypeBL();
        }

        // GET: Stock/DamageEntry
        public ActionResult Index()
        {
            List<DamageEntryModel> damageEntry = damageEntryBL.GetDamageEntryList().Select(a => new DamageEntryModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date, "view"),
                Warehouse = a.Warehouse,
                DamageType = a.DamageType,
                Status = a.IsDraft ? "draft" : "",
            }).ToList();
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };

            return View(damageEntry);
        }

        public ActionResult Create()
        {
            DamageEntryModel DamageEntry = new DamageEntryModel();
            DamageEntry.TransNo = generalBL.GetSerialNo("DamageEntry", "Code");
            DamageEntry.Date = General.FormatDate(DateTime.Now);
            DamageEntry.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            DamageEntry.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            DamageEntry.Items = new List<DamageEntryItem>();
            DamageEntry.DamageTypeList = new SelectList(DamageTypeBL.GetDamageTypeList(), "ID", "Name");
            return View(DamageEntry);
        }

        [HttpPost]
        public JsonResult GetDamageEntryItems(int WarehouseID, int ItemCategoryID = 0, int ItemID = 0)
        {
            try
            {
                List<DamageEntryItemBO> damageentrylist = damageEntryBL.GetDamageEntryItems(WarehouseID, ItemCategoryID, ItemID);
                damageentrylist = damageentrylist.Select(a =>
                {
                    a.ExpiryDateString = a.ExpiryDate == null ? "" : General.FormatDate((DateTime)a.ExpiryDate);
                    return a;
                }).ToList();
                return Json(new { Status = "success", Data = damageentrylist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetDropdownVal()
        {
            List<DamageTypeBO> DamageType = new List<DamageTypeBO>()
            {
                new DamageTypeBO()
                {
                    ID = 0,
                    Name = "Select"
                }
            };
            DamageType.AddRange(DamageTypeBL.GetDamageTypeList());
            return Json(new { Status = "success", data = DamageType }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(DamageEntryModel damageEntryModel)
        {
            try
            {
                DamageEntryBO damageEntry = new DamageEntryBO()
                {
                    IsDraft = damageEntryModel.IsDraft,
                    Date = General.ToDateTime(damageEntryModel.Date),
                    TransNo = damageEntryModel.TransNo,
                    WarehouseID = damageEntryModel.WarehouseID,
                    ID = damageEntryModel.ID

                };

                List<DamageEntryItemBO> items = new List<DamageEntryItemBO>();
                DamageEntryItemBO damageEntryBO;
                foreach (var item in damageEntryModel.Items)
                {
                    damageEntryBO = new DamageEntryItemBO()
                    {
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        WarehouseID = item.WarehouseID,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        CurrentQty = item.CurrentQty,
                        ExpiryDate = General.ToDateTime(item.ExpiryDate),
                        DamageQty = item.DamageQty,
                        DamageTypeID = item.DamageTypeID,
                        Remarks = item.Remarks
                    };
                    items.Add(damageEntryBO);

                }
                damageEntryBL.Save(damageEntry, items);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetBatchesByItemIDForDamageEntry(int WarehouseID, int ItemID = 0)
        {
            try
            {
                List<DamageEntryItemBO> damageentrylist = damageEntryBL.GetBatchesByItemIDForDamageEntry(WarehouseID, ItemID);
                damageentrylist = damageentrylist.Select(a =>
                {
                    a.ExpiryDateString = a.ExpiryDate == null ? "" : General.FormatDate((DateTime)a.ExpiryDate);
                    return a;
                }).ToList();
                return Json(new { Status = "success", Data = damageentrylist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int id)
        {
            DamageEntryModel damageEntry = damageEntryBL.GetDamageEntryDetail(id).Select(a => new DamageEntryModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date, "view"),
                Warehouse = a.Warehouse,
                IsDraft = a.IsDraft
            }).First();
            damageEntry.Items = damageEntryBL.GetDamageEntryTrans(id).Select(a => new DamageEntryItem()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
                Batch = a.Batch,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                CurrentQty = a.CurrentQty,
                DamageQty = a.DamageQty,
                WarehouseID = a.WarehouseID,
                DamageTypeID = a.DamageTypeID,
                DamageType = a.DamageType,
                Remarks = a.Remarks,
                ExpiryDate = General.FormatDate(a.ExpiryDate, "view")

            }).ToList();
            return View(damageEntry);
        }

        public ActionResult Edit(int id)
        {
            DamageEntryModel damageEntry = damageEntryBL.GetDamageEntryDetail(id).Select(a => new DamageEntryModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(DateTime.Now),
                Warehouse = a.Warehouse,
                WarehouseID = a.WarehouseID
            }).First();
            damageEntry.Items = damageEntryBL.GetDamageEntryTrans(id).Select(a => new DamageEntryItem()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
                Batch = a.Batch,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                CurrentQty = a.CurrentQty,
                DamageQty = a.DamageQty,
                WarehouseID = a.WarehouseID,
                DamageTypeID = a.DamageTypeID,
                DamageType = a.DamageType,
                Remarks = a.Remarks,
                ExpiryDate = General.FormatDate(a.ExpiryDate)
            }).ToList();
            damageEntry.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            damageEntry.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            damageEntry.DamageTypeList = new SelectList(DamageTypeBL.GetDamageTypeList(), "ID", "Name");

            return View(damageEntry);
        }
    }
}