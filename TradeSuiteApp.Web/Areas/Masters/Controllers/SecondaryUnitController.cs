using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessObject;
using PresentationContractLayer;
using BusinessLayer;
using System.Web.UI.WebControls;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class SecondaryUnitController : Controller
    {
        private IUnitContract unitBL;

        public SecondaryUnitController()
        {

            unitBL = new UnitBL();

        }
        // GET: Masters/Unit
        public ActionResult Index()
        {
            List<SecondaryUnitModel> unitList = new List<SecondaryUnitModel>();

            unitList = unitBL.GetSecondaryUnitList().Select(a => new SecondaryUnitModel()
            {
                ID = a.ID,
                Name = a.Name,
                UnitName = a.Unit,
                UnitGroupName = a.UnitGroup,
            }).ToList();
            return View(unitList);
        }


        // GET: Masters/State/Details/5
        public ActionResult Details(int id)
        {
            var obj = unitBL.GetSecondaryUnitDetails(id);

            SecondaryUnitModel unitModel = new SecondaryUnitModel();

            unitModel.ID = obj.ID;
            unitModel.Name = obj.Name;
            unitModel.UnitName = obj.UnitGroup;
            unitModel.UnitGroupName = obj.UnitGroup;
            unitModel.PackSize = obj.PackSize;

            return View(unitModel);
        }

        //GET: Masters/Unit/Create
        public ActionResult Create()
        {
            SecondaryUnitModel Unit = new SecondaryUnitModel();
            Unit.UnitList = new SelectList(unitBL.GetUnitList(), "ID", "Name");
            Unit.UnitGroupList = new SelectList(unitBL.GetUnitGroupList(), "ID", "Name");
            var Group = unitBL.GetUnitGroupList().OrderBy(x => x.ID).FirstOrDefault();
            if (Group != null)
            {
                Unit.UnitGroupID = Group.ID;
            }
            return View(Unit);
        }

        // POST: Masters/Unit/Create
        [HttpPost]
        public ActionResult Create(SecondaryUnitModel model)
        {

            try
            {
                SecondaryUnitBO secondaryUnitBO = new SecondaryUnitBO()
                {
                    Name = model.Name,
                    UnitID = model.UnitID,
                    UnitGroupID = model.UnitGroupID,
                    PackSize = model.PackSize
                };
                unitBL.CreateSecondaryUnit(secondaryUnitBO);
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
            var obj = unitBL.GetSecondaryUnitDetails(id);
            SecondaryUnitModel unitModel = new SecondaryUnitModel();
            unitModel.UnitList = new SelectList(unitBL.GetUnitList(), "ID", "Name", obj.UnitID);
            unitModel.UnitGroupList = new SelectList(unitBL.GetUnitGroupList(), "ID", "Name", obj.UnitGroupID);
            var Group = unitBL.GetUnitGroupList().OrderBy(x => x.ID).FirstOrDefault();
            if (Group != null)
            {
                unitModel.UnitGroupID = Group.ID;
            }
            unitModel.ID = obj.ID;
            unitModel.Name = obj.Name;
            //unitModel.UnitGroupID = obj.UnitGroupID;
            unitModel.UnitID = obj.UnitID;
            unitModel.UnitName = obj.UnitGroup;
            unitModel.UnitGroupName = obj.UnitGroup;
            unitModel.PackSize = obj.PackSize;
            return View(unitModel);

        }

        // POST: Masters/State/Edit/5
        [HttpPost]
        public ActionResult Edit(SecondaryUnitModel model)
        {

            try
            {
                SecondaryUnitBO unitBO = new SecondaryUnitBO()
                {
                    ID = (int)model.ID,
                    Name = model.Name,
                    UnitID = model.UnitID,
                    UnitGroupID = model.UnitGroupID,
                    PackSize = model.PackSize
                };
                unitBL.UpdateSecondaryUnit(unitBO);
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