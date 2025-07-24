using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class DistrictController : Controller
    {
        #region private Declarations

        private IDistrictContract iDistrictContract;

        #endregion

        #region Constructors

        public DistrictController()
        {

            iDistrictContract = new DistrictBL();

        }
        #endregion

        // GET: Masters/District
        public ActionResult Index()
        {
            List<DistrictModel> districtList = new List<DistrictModel>();

            districtList = iDistrictContract.GetDistrictList(0).Select(a => new DistrictModel()
            {
                ID = a.ID,
                Name = a.Name,
                StateID = a.StateID,
                StateName=a.StateName,
                PIN = a.PIN,
                OfficeName=a.OfficeName,
                Taluk=a.Taluk
            }).ToList();
            return View(districtList);           
        }
        // GET: Masters/District/Create
        public ActionResult Create()
        {
            DistrictModel districtModel = new DistrictModel();
            districtModel.States = new SelectList(iDistrictContract.GetStateName(), "ID", "Name"); /*obj.GroupName;*/
            return View(districtModel);
        }

        // POST: Masters/District/Create
        [HttpPost]
        public ActionResult Create(DistrictModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    DistrictBO districtBO = new DistrictBO()
                    {
                        Name = model.Name,
                        StateID = (int)model.StateID,
                        StateName = model.StateName,
                        OfficeName=model.OfficeName,
                        PIN=model.PIN,
                        Taluk=model.Taluk
                    };
                    iDistrictContract.CreateDistrict(districtBO);
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

        // GET: Masters/District/Details/5
        public ActionResult Details(int id)
        {
            var obj = iDistrictContract.GetDistrictDetails(id);
            DistrictModel districtModel = new DistrictModel();
            districtModel.ID = obj.ID;
            districtModel.Name = obj.Name;
            districtModel.StateID = obj.StateID;
            districtModel.StateName = obj.StateName;
            districtModel.OfficeName = obj.OfficeName;
            districtModel.PIN = obj.PIN;
            districtModel.Taluk = obj.Taluk;
            return View(districtModel);
        }

        // GET: Masters/District/Edit/5
        public ActionResult Edit(int id)
        {
            var obj = iDistrictContract.GetDistrictDetails(id);
            DistrictModel districtModel = new DistrictModel();
            districtModel.ID = obj.ID;
            districtModel.Name = obj.Name;
            districtModel.StateID = obj.StateID;
            districtModel.States = new SelectList(iDistrictContract.GetStateName(), "ID", "Name");
            districtModel.OfficeName = obj.OfficeName;
            districtModel.PIN = obj.PIN;
            districtModel.Taluk = obj.Taluk;
            return View(districtModel);
        }

        //POST: Masters/Unit/Edit/5
        [HttpPost]
        public ActionResult Edit(DistrictModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DistrictBO districtBO = new DistrictBO()
                    {
                        ID = (int)model.ID,
                        Name = model.Name,
                        StateID = (int)model.StateID,
                        StateName = model.StateName,
                        OfficeName = model.OfficeName,
                        PIN = model.PIN,
                        Taluk = model.Taluk
                    };
                    iDistrictContract.EditDistrict(districtBO);
                    return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    var res = new List<object>();
                                        return Json(new { Status = "failure", data = res }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        //code below by prama on 6-6-18
        public JsonResult GetDistrict(int StateID)
        {
            List<DistrictBO> District = iDistrictContract.GetDistrictList(StateID).ToList();
            return Json(new { Status = "success", data = District }, JsonRequestBehavior.AllowGet);
        }
    }
}