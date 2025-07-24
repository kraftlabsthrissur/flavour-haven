using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class BinMasterController : Controller
    {
        private IBinContract binBL;
        private ItemContract itemBL;
        private IUnitContract unitBL;
        private IDiscountCategoryContract discountCategoryBL;
        private IGSTContract GstBL;
        private IWareHouseContract wareHouseBL;


        public BinMasterController()
        {
            binBL = new BinBL();
            itemBL = new ItemBL();
            unitBL = new UnitBL();
            discountCategoryBL = new DiscountCategoryBL();
            GstBL = new GSTBL();
            wareHouseBL = new WarehouseBL();

        }


        //public ActionResult Index()
        //{
        //    return View();
        //}



        // GET: Masters/Bin/Create
        public ActionResult Create()
        {
            BinModel binModel = new BinModel();
            binModel.WareHouseList = new SelectList(wareHouseBL.GetWareHouseList(), "ID", "Name");

            //batchModel.ManufacturingDate = General.FormatDate(DateTime.Now);
            return View(binModel);
        }

        [HttpPost]
        public ActionResult Save(BinModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BinBO binBO = new BinBO()
                    {

                        BinCode = model.BinCode,
                        WareHouseID = model.WareHouseID





                    };
                    binBL.CreateBin(binBO);
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

        public ActionResult Index()
        {
            List<BinModel> BinList = new List<BinModel>();
            BinList = binBL.GetBinList().Select(a => new BinModel()
            {
                ID = a.ID,
                BinCode = a.BinCode,
                WarehouseName = a.WareHouseName,

            }).ToList();
            return View(BinList);
        }
        public ActionResult Details(int id)
        {
            //try
            //{
            var obj = binBL.GetBinDetailsMaster(id);

            BinModel binModel = new BinModel();
            binModel.ID = obj.ID;
            binModel.BinCode = obj.BinCode;
            binModel.WarehouseName = obj.WareHouseName;

            return View(binModel);
            //}
            //catch (Exception e)
            //{
            //    return RedirectToAction("Index");
            //}
        }
        public ActionResult Edit(int id)
        {
            try
            {
                var obj = binBL.GetBinDetailsMaster(id);
                BinModel BinModel = new BinModel();
                BinModel.ID = obj.ID;
                BinModel.BinCode = obj.BinCode;
                BinModel.WareHouseID = obj.WareHouseID;
               BinModel.WareHouseList= new SelectList(wareHouseBL.GetWareHouseList(), "ID", "Name");


                return View(BinModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult Edit(BinModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BinBO BinBO = new BinBO()
                    {
                        ID = (int)model.ID,
                        BinCode=model.BinCode,
                        WareHouseID=model.WareHouseID

                    };
                    binBL.EditBinDetails(BinBO);
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

    }
}