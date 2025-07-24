using BusinessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessObject;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class WarehouseController : Controller
    {
        private ItemContract itemBL;
        private IWareHouseContract warehouseBL;
        private ILocationContract locationBL;
        private ICategoryContract categoryBL;

        public WarehouseController()
        {
            warehouseBL = new WarehouseBL();
            itemBL = new ItemBL();
            locationBL = new LocationBL();
            categoryBL = new CategoryBL();
        }

        // GET: Masters/Warehouse
        public ActionResult Index()
        {
            List<WareHouseModel> warehouselist = warehouseBL.GetWareHouseList().Select(a => new WareHouseModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Place = a.Place,
                ItemTypeID = a.ItemTypeID,
                ItemTypeName = a.ItemTypeName
            }).ToList();

            return View(warehouselist);
        }

        // GET: Masters/Warehouse/Details/5
        public ActionResult Details(int id)
        {

            var obj = warehouseBL.GetWareHouseDetails(id);

            WareHouseModel wareHouseModel = new WareHouseModel();

            wareHouseModel.ID = obj.ID;
            wareHouseModel.Code = obj.Code;
            wareHouseModel.Name = obj.Name;
            wareHouseModel.Remarks = obj.Remarks;
            wareHouseModel.Place = obj.Place;
            wareHouseModel.ItemTypeName = obj.ItemTypeName;
            wareHouseModel.LocationName = obj.LocationName;
            return View(wareHouseModel);
        }

        // GET: Masters/Warehouse/Create
        public ActionResult Create()
        {
            WareHouseModel wareHouseModel = new WareHouseModel();
            wareHouseModel.ItemTypeGroup = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            wareHouseModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            return View(wareHouseModel);
        }

        // POST: Masters/Warehouse/Create
        [HttpPost]
        public ActionResult Create(WareHouseModel model)
        {
            if (ModelState.IsValid)
            {
                var warehouseretvalue = 0;
                try
                {
                    WareHouseBO warehouseBO = new WareHouseBO()
                    {
                        Code = model.Code,
                        Name = model.Name,
                        Place = model.Place,
                        ItemTypeID = model.ItemTypeID,
                        LocationID = model.LocationID,
                        Remarks = model.Remarks
                    };
                    warehouseretvalue = warehouseBL.CreateWarehouse(warehouseBO);
                    if (warehouseretvalue > 0)
                    {
                        return Json(new { Status = "success", data = model, message = "WareHouse created successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Status = "failure", message = "Warehouse Already Exists" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    var res = new List<object>();
                    return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Masters/Warehouse/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                WareHouseModel wareHouseModel = new WareHouseModel();
                //wareHouseModel.ItemTypeGroup = new SelectList(itemBL.GetItemTypeList(), "ID", "Name");
                wareHouseModel.ItemTypeGroup = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
                wareHouseModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
                var obj = warehouseBL.GetWareHouseDetails(id);
                wareHouseModel.ID = obj.ID;
                wareHouseModel.Code = obj.Code;
                wareHouseModel.Name = obj.Name;
                wareHouseModel.Remarks = obj.Remarks;
                wareHouseModel.Place = obj.Place;
                wareHouseModel.ItemTypeID = obj.ItemTypeID;
                wareHouseModel.LocationID = obj.LocationID;
                return View(wareHouseModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

        }

        // POST: Masters/Warehouse/Edit/5
        [HttpPost]
        public ActionResult Edit(WareHouseModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WareHouseBO WareHouseBO = new WareHouseBO()
                    {
                        ID = (int)model.ID,
                        Code = model.Code,
                        Name = model.Name,
                        Place = model.Place,
                        Remarks = model.Remarks,
                        ItemTypeID = model.ItemTypeID,
                        LocationID = model.LocationID
                    };
                    warehouseBL.EditWareHouse(WareHouseBO);
                    return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    var res = new List<object>();
                    return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult GetWareHousesByLocation(int LocationID)
        {
            try
            {
                List<WareHouseBO> warehouses = warehouseBL.GetWareHousesByLocation(LocationID);
                return Json(new { Status = "success", data = warehouses }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(int id)
        {
            return null;
        }
        public JsonResult GetNursingStationAutoComplete(string Hint)
        {
            DatatableResultBO resultBO = warehouseBL.GetNursingStationAutoComplete(Hint);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
    }
}
