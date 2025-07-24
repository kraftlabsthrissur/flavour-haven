using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessObject;
using PresentationContractLayer;
using BusinessLayer;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class UnitController : Controller
    {
        #region Private Declarations

        private IUnitContract unitBL;

        #endregion

        #region Constructors

        public UnitController()
        {

            unitBL = new UnitBL();

        }
        #endregion
        // GET: Masters/Unit
        public ActionResult Index()
        {
            List<UnitModel> unitList = new List<UnitModel>();

            unitList = unitBL.GetUnitList().Select(a => new UnitModel()
            {
                ID = a.ID,
                Name = a.Name,
                QOM = a.QOM,
                UOM = a.UOM,
                CF = a.CF
            }).ToList();
            return View(unitList);
        }


        // GET: Masters/State/Details/5
        public ActionResult Details(int id)
        {
            var obj = unitBL.GetUnitDetails(id);

            UnitModel unitModel = new UnitModel();

            unitModel.ID = obj.ID;
            unitModel.Name = obj.Name;
            unitModel.QOM = obj.QOM;
            unitModel.UOM = obj.UOM;
            unitModel.CF = obj.CF;
            unitModel.PackSize = obj.PackSize;

            return View(unitModel);
        }

        //GET: Masters/Unit/Create
        public ActionResult Create()
        {
            UnitModel Unit = new UnitModel();
            Unit.QOM = 1;
            return View(Unit);
        }

        // POST: Masters/Unit/Create
        [HttpPost]
        public ActionResult Create(UnitModel model)
        {

            try
            {
                UnitBO unitBO = new UnitBO()
                {
                    Name = model.Name,
                    QOM = model.QOM,
                    UOM = model.UOM,
                    CF = model.CF,
                    PackSize=model.PackSize
                };
                unitBL.CreateUnit(unitBO);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        // GET: Masters/Unit/Edit/5
        public ActionResult Edit(int id)
        {
            var obj = unitBL.GetUnitDetails(id);
            UnitModel unitModel = new UnitModel();
            unitModel.ID = obj.ID;
            unitModel.Name = obj.Name;
            unitModel.QOM = obj.QOM;
            unitModel.UOM = obj.UOM;
            unitModel.CF = obj.CF;
            unitModel.PackSize = obj.PackSize;
            return View(unitModel);

        }

        // POST: Masters/State/Edit/5
        [HttpPost]
        public ActionResult Edit(UnitModel model)
        {

            try
            {
                UnitBO unitBO = new UnitBO()
                {
                    ID = (int)model.ID,
                    Name = model.Name,
                    QOM = model.QOM,
                    UOM = model.UOM,
                    CF = model.CF,
                    PackSize=model.PackSize
                };
                unitBL.EditUnit(unitBO);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(int id)
        {
            return null;
        }
        public JsonResult GetUnitsList()
        {
            List<UnitBO> UnitList = unitBL.GetUnitsList().ToList();
            return Json(new { Status = "success", data = UnitList }, JsonRequestBehavior.AllowGet);
        }
    }
}