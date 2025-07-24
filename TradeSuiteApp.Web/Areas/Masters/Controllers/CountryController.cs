using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class CountryController : Controller
    {
        #region Private Declarations

        private ICountryContract iCountryContract;

        #endregion

        #region Constructors

        public CountryController()
        {
            iCountryContract = new CountryBL();
        }
        #endregion
        // GET: Masters/Country
        public ActionResult Index()
        {
            List<CountryModel> countryList = new List<CountryModel>();
            countryList = iCountryContract.GetCountryList().Select(a => new CountryModel()
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                IsActive = a.IsActive,
                IsIntraCountry = a.IsIntraCountry,
            }).ToList();
            return View(countryList);
        }

        // GET: Masters/Country/Details/5
        public ActionResult Details(int id)
        {
            //try
            //{
            var obj = iCountryContract.GetCountryDetails(id);

            CountryModel countryModel = new CountryModel();

            countryModel.Id = obj.Id;
            countryModel.Code = obj.Code;
            countryModel.Name = obj.Name;
            countryModel.IsActive = obj.IsActive;
            countryModel.IsIntraCountry = obj.IsIntraCountry;
            return View(countryModel);
            //}
            //catch (Exception e)
            //{
            //    return RedirectToAction("Index");
            //}
        }

        // GET: Masters/Country/Create
        public ActionResult Create()
        {


            return View();
        }

        // POST: Masters/Country/Create
        [HttpPost]
        public ActionResult Save(CountryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CountryBO countryBO = new CountryBO()
                    {
                        Code = model.Code,
                        Name = model.Name,
                        IsActive = model.IsActive,
                        IsIntraCountry = model.IsIntraCountry
                    };
                    iCountryContract.CreateCountry(countryBO);
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

        // GET: Masters/Country/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var obj = iCountryContract.GetCountryDetails(id);
                CountryModel countryModel = new CountryModel();
                countryModel.Id = obj.Id;
                countryModel.Code = obj.Code;
                countryModel.Name = obj.Name;
                countryModel.IsActive = obj.IsActive;
                countryModel.IsIntraCountry = obj.IsIntraCountry;
                return View(countryModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Masters/Country/Edit/5
        [HttpPost]
        public ActionResult Edit(CountryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CountryBO countryBO = new CountryBO()
                    {
                        Id = (int)model.Id,
                        Code = model.Code,
                        Name = model.Name,
                        IsActive = model.IsActive,
                        IsIntraCountry = model.IsIntraCountry,
                    };
                    iCountryContract.EditCountry(countryBO);
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

        public JsonResult GetCountriesList(DatatableModel Datatable)

        {
            try
            {
                string Code = Datatable.Columns[2].Search.Value;
                string Name = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = iCountryContract.GetCountrySearchList(Code, Name, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
