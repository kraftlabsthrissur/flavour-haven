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
    public class TaxtypeController : Controller
    {
        // GET: Masters/Currency
        private ITaxTypeContract iTaxTypeContract;
        public TaxtypeController()
        {
            iTaxTypeContract = new TaxTypeBL();
        }
        public ActionResult Index()
        {
            List<TaxtypeModel> taxtypelist = new List<TaxtypeModel>();
            taxtypelist = iTaxTypeContract.GetTaxTypeList().Select(a => new TaxtypeModel()
            {
                Id = a.ID,
               
                Name = a.Name,
                LocationName = a.LocationName,
                Description =a.Description
                
            }).ToList();
            return View(taxtypelist);
        }
        //public ActionResult Details(int id)
        //{
        //    //try
        //    //{
        //    var obj = iCurrencyContract.GetCurrencyDetails(id);
        //    CurrencyModel currencyModel = new CurrencyModel();
        //    currencyModel.Id = obj.Id;
        //    currencyModel.Code = obj.Code;
        //    currencyModel.Name = obj.Name;
        //    currencyModel.Description = obj.Description;
        //    currencyModel.CountryName = obj.CountryName;
        //    return View(currencyModel);
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    return RedirectToAction("Index");
        //    //}
        //}
        public ActionResult Create()
        {


            return View();
        }

        //POST: Masters/Country/Create
       [HttpPost]
        public ActionResult Save(TaxtypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TaxTypeBO locationbo = new TaxTypeBO()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        //LocationName = model.LocationName
                    };
                    iTaxTypeContract.CreateTaxtype(locationbo);
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
                var obj = iTaxTypeContract.GetTaxtypeDetails(id);
                TaxtypeModel TaxtypeModel = new TaxtypeModel();
                TaxtypeModel.Id = obj.ID;
                TaxtypeModel.Name = obj.Name;
                TaxtypeModel.Description = obj.Description;
                TaxtypeModel.LocationName = obj.LocationName;
                TaxtypeModel.LocationID = obj.LocationID;
                return View(TaxtypeModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        //// POST: Masters/Country/Edit/5
        [HttpPost]
        public ActionResult Edit(TaxtypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TaxTypeBO locationbo = new TaxTypeBO()
                    {
                        ID = (int)model.Id,
                      
                        Name = model.Name,
                        Description = model.Description,
                        LocationID = model.LocationID
                    };
                    iTaxTypeContract.EditTaxtype(locationbo);
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
        //public JsonResult GetBaseCurrenciesList(DatatableModel Datatable)

        //{
        //    try
        //    {
        //        string Code = Datatable.Columns[2].Search.Value;
        //        string Name = Datatable.Columns[3].Search.Value;
        //        string CountryName = Datatable.Columns[4].Search.Value;
        //        string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
        //        string SortOrder = Datatable.Order[0].Dir;
        //        int Offset = Datatable.Start;
        //        int Limit = Datatable.Length;

        //        DatatableResultBO resultBO = iCurrencyContract.GetCurrencySearchList(Code, Name, CountryName, SortField, SortOrder, Offset, Limit);
        //        var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        var res = new List<object>();
        //        return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public JsonResult GetConversionCurrenciesList(DatatableModel Datatable)

        //{
        //    try
        //    {
        //        string Code = Datatable.Columns[2].Search.Value;
        //        string Name = Datatable.Columns[3].Search.Value;
        //        string CountryName = Datatable.Columns[4].Search.Value;
        //        string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
        //        string SortOrder = Datatable.Order[0].Dir;
        //        int Offset = Datatable.Start;
        //        int Limit = Datatable.Length;

        //        DatatableResultBO resultBO = iCurrencyContract.GetCurrencySearchList(Code, Name, CountryName, SortField, SortOrder, Offset, Limit);
        //        var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        var res = new List<object>();
        //        return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public JsonResult GetCurrenciesList(DatatableModel Datatable)

        //{
        //    try
        //    {
        //        string Code = Datatable.Columns[2].Search.Value;
        //        string Name = Datatable.Columns[3].Search.Value;
        //        string CountryName = Datatable.Columns[4].Search.Value;
        //        string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
        //        string SortOrder = Datatable.Order[0].Dir;
        //        int Offset = Datatable.Start;
        //        int Limit = Datatable.Length;

        //        DatatableResultBO resultBO = iCurrencyContract.GetCurrencySearchList2(Code, Name, CountryName, SortField, SortOrder, Offset, Limit);
        //        var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        var res = new List<object>();
        //        return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //[HttpPost]
        //public JsonResult GetCurrenciesAutoCompleteList(string CurrencyName)
        //{

        //    DatatableResultBO resultBO = iCurrencyContract.GetCurrencySearchList2("", CurrencyName, "", "Name", "ASC", 1, 20);
        //    return Json(resultBO.data, JsonRequestBehavior.AllowGet);

        //}
    }
}