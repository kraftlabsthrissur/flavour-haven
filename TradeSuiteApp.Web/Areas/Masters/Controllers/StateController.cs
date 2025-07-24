using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;
using PresentationContractLayer;
using BusinessLayer;
using DataAccessLayer.DBContext;
using BusinessObject;


namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class StateController : Controller
    {

        #region Private Declarations

        private IStateContract iStateContract;
        ICountryContract countryBL;

        #endregion

        #region Constructors

        public StateController()
        {

            iStateContract = new StateBL();
            countryBL = new CountryBL();
        }
        #endregion

        // GET: Masters/State
        public ActionResult Index()
        {
            List<StateModel> stateList = new List<StateModel>();

            stateList = iStateContract.GetStateList().Select(a => new StateModel()
            {
                ID = a.ID,
                Name = a.Name,
                GstState = a.GstState
            }).ToList();
            return View(stateList);
        }

        // GET: Masters/State/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var obj = iStateContract.GetStateDetails(id);

                StateModel stateModel = new StateModel();

                stateModel.ID = obj.ID;
                stateModel.GstState = obj.GstState;
                stateModel.Name = obj.Name;
                stateModel.CountryID = (int)obj.CountryID;
                stateModel.Country = obj.Country;

                return View(stateModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Masters/State/Create
        public ActionResult Create()
        {
            StateModel state = new StateModel();
            state.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");

            return View(state);
        }

        // POST: Masters/State/Create
        [HttpPost]
        public ActionResult Create(StateModel model)
        {
            try
            {
                StateBO stateBO = new StateBO()
                {
                    Name = model.Name,
                    GstState = model.GstState,
                    CountryID = model.CountryID

                };
                iStateContract.CreateState(stateBO);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        // GET: Masters/State/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var obj = iStateContract.GetStateDetails(id);
                StateModel stateModel = new StateModel();
                stateModel.ID = obj.ID;
                stateModel.GstState = obj.GstState;
                stateModel.Name = obj.Name;
                stateModel.CountryID = (int)obj.CountryID;
                stateModel.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
                return View(stateModel);
            }

            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Masters/State/Edit/5
        [HttpPost]
        public ActionResult Edit(StateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    StateBO stateBO = new StateBO()
                    {
                        ID = (int)model.ID,
                        Name = model.Name,
                        GstState = model.GstState,
                        CountryID = model.CountryID
                    };
                    iStateContract.EditState(stateBO);
                    return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {

                    return Json(new { Status = "failure", data = model }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetStateCountryWise(int CountryID)
        {
            List<StateBO> District = iStateContract.GetStateListCountryWise(CountryID).ToList();
            return Json(new { Status = "success", data = District }, JsonRequestBehavior.AllowGet);
        }
    }
}