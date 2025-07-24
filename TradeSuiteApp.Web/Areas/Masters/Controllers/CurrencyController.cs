using BusinessLayer;
using BusinessObject;
using Microsoft.AspNet.SignalR.Hubs;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class CurrencyController : Controller
    {
        // GET: Masters/Currency
        private ICurrencyContract iCurrencyContract;
        public CurrencyController()
        {
            iCurrencyContract = new CurrencyBL();
        }
        public ActionResult Index()
        {
            List<CurrencyModel> currencyList = new List<CurrencyModel>();
            currencyList = iCurrencyContract.GetCurrencyList().Select(a => new CurrencyModel()
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                Description = a.Description,
                CountryName = a.CountryName,
            }).ToList();
            return View(currencyList);
        }
        public ActionResult Details(int id)
        {
            //try
            //{
            var obj = iCurrencyContract.GetCurrencyDetails(id);
            CurrencyModel currencyModel = new CurrencyModel();
            currencyModel.Id = obj.Id;
            currencyModel.Code = obj.Code;
            currencyModel.Name = obj.Name;
            currencyModel.Description = obj.Description;
            currencyModel.CountryName = obj.CountryName;
            currencyModel.DecimalPlaces = obj.DecimalPlaces;
            currencyModel.MinimumCurrency = obj.MinimumCurrency;
            currencyModel.MinimumCurrencyCode = obj.MinimumCurrencyCode;
            return View(currencyModel);
            //}
            //catch (Exception e)
            //{
            //    return RedirectToAction("Index");
            //}
        }
        public ActionResult Create()
        {


            return View();
        }

        // POST: Masters/Country/Create
        [HttpPost]
        public ActionResult Save(CurrencyModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CurrencyBO countryBO = new CurrencyBO()
                    {
                        Code = model.Code,
                        Name = model.Name,
                        Description = model.Description,
                        CountryID = model.CountryID,
                        DecimalPlaces= model.DecimalPlaces,
                        MinimumCurrency=model.MinimumCurrency,
                        MinimumCurrencyCode = model.MinimumCurrencyCode

                    };
                    iCurrencyContract.CreateCurrency(countryBO);
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

        public ActionResult Edit(int id)
        {
            try
            {
                var obj = iCurrencyContract.GetCurrencyDetails(id);
                CurrencyModel currencyModel = new CurrencyModel();
                currencyModel.Id = obj.Id;
                currencyModel.Code = obj.Code;
                currencyModel.Name = obj.Name;
                currencyModel.Description = obj.Description;
                currencyModel.CountryName = obj.CountryName;
                currencyModel.CountryID = obj.CountryID;
                currencyModel.DecimalPlaces = obj.DecimalPlaces;
                currencyModel.MinimumCurrency = obj.MinimumCurrency;
                currencyModel.MinimumCurrencyCode = obj.MinimumCurrencyCode;
                return View(currencyModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Masters/Country/Edit/5
        [HttpPost]
        public ActionResult Edit(CurrencyModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CurrencyBO countryBO = new CurrencyBO()
                    {
                        Id = (int)model.Id,
                        Code = model.Code,
                        Name = model.Name,
                        Description = model.Description,
                        CountryID = model.CountryID,
                        DecimalPlaces = model.DecimalPlaces,
                        MinimumCurrency=model.MinimumCurrency,
                        MinimumCurrencyCode=model.MinimumCurrencyCode
                };
                    iCurrencyContract.EditCurrency(countryBO);
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
        public JsonResult GetBaseCurrenciesList(DatatableModel Datatable)

        {
            try
            {
                string Code = Datatable.Columns[2].Search.Value;
                string Name = Datatable.Columns[3].Search.Value;
                string CountryName = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = iCurrencyContract.GetCurrencySearchList(Code, Name, CountryName, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetConversionCurrenciesList(DatatableModel Datatable)

        {
            try
            {
                string Code = Datatable.Columns[2].Search.Value;
                string Name = Datatable.Columns[3].Search.Value;
                string CountryName = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = iCurrencyContract.GetCurrencySearchList(Code, Name, CountryName, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCurrenciesList(DatatableModel Datatable)

        {
            try
            {
                string Code = Datatable.Columns[2].Search.Value;
                string Name = Datatable.Columns[3].Search.Value;
                string CountryName = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = iCurrencyContract.GetCurrencySearchList2(Code, Name, CountryName, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetCurrenciesAutoCompleteList(string CurrencyName)
        {

            DatatableResultBO resultBO = iCurrencyContract.GetCurrencySearchList2("", CurrencyName, "", "Name", "ASC", 1, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);

        }
    }
}