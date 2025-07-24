//file created by prama on 6-6-18
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
    public class PlacesController : Controller
    {
        IStateContract stateBL;
        IDistrictContract districtBL;
        ICountryContract countryBL;
        IPlacesContract placesBL;
        public PlacesController()
        {

            stateBL = new StateBL();
            districtBL = new DistrictBL();
            countryBL = new CountryBL();
            placesBL = new PlacesBL();

        }

        // GET: Masters/Places
        public ActionResult Index()
        {
            List<PlacesModel> List = new List<PlacesModel>();

            List = placesBL.GetPlaces(0).Select(a => new PlacesModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return View(List);
        }

        // GET: Masters/Places/Details/5
        public ActionResult Details(int id)
        {
            var obj = placesBL.GetPlacesDetails(id);
            PlacesModel Model = new PlacesModel();
            Model.ID = obj.ID;
            Model.Code = obj.Code;
            Model.Name = obj.Name;
            Model.Address = obj.Address;
            Model.District = obj.District;
            Model.State = obj.State;
            Model.Country = obj.Country;
            return View(Model);
        }
        // GET: Masters/Places/Create
        public ActionResult Create()
        {
            PlacesModel placeModel = new PlacesModel();
            placeModel.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            placeModel.DisitrictList = new SelectList(districtBL.GetDistrictList(0), "ID", "Name");
            placeModel.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            return View(placeModel);
        }

        // POST: Masters/Places/Create
        [HttpPost]
        public ActionResult Save(PlacesModel model)
        {
            try
            {
                PlacesBO places = new PlacesBO()
                {
                    ID = (int)model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    Address = model.Address,
                    DistrictID = model.DistrictID,
                    StateID = model.StateID,
                    CountryID = model.CountryID
                };
                var outId = placesBL.SavePlaces(places);
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Masters/Places/Edit/5
        public ActionResult Edit(int id)
        {
            var obj = placesBL.GetPlacesDetails(id);
            PlacesModel placeModel = new PlacesModel();
            placeModel.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            placeModel.DisitrictList = new SelectList(districtBL.GetDistrictList(0), "ID", "Name");
            placeModel.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            placeModel.ID = obj.ID;
            placeModel.Code = obj.Code;
            placeModel.Name = obj.Name;
            placeModel.Address = obj.Address;
            placeModel.DistrictID = obj.DistrictID;
            placeModel.StateID = obj.StateID;
            placeModel.CountryID = obj.CountryID;

            return View(placeModel);
        }

        // POST: Masters/Places/Edit/5
        [HttpPost]
        public ActionResult Update(PlacesModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PlacesBO placesBO = new PlacesBO()
                    {
                        ID = (int)model.ID,
                        Code = model.Code,
                        Name = model.Name,
                        Address = model.Address,
                        DistrictID = model.DistrictID,
                        StateID = model.StateID,
                        CountryID = model.CountryID
                    };
                    placesBL.UpdatePlaces(placesBO);
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
