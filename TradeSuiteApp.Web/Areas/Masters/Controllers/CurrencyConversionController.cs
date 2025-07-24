using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class CurrencyConversionController : Controller
    {
        // GET: Masters/CurrencyConversion
        private ICurrencyConversionContract icurrencyConversionContract;
        public CurrencyConversionController()
        {
            icurrencyConversionContract = new CurrencyConversionBL();
        }
        public ActionResult Index()
        {
            List<CurrencyConversionModel> currencyConversionModelList = new List<CurrencyConversionModel>();
            currencyConversionModelList = icurrencyConversionContract.GetCurrencyConversionList().Select(a => new CurrencyConversionModel()
            {
                Id = a.Id,
                BaseCurrencyCode = a.BaseCurrencyCode,
                ConversionCurrencyCode = a.ConversionCurrencyCode,
                ExchangeRate = a.ExchangeRate,
                InverseExchangeRate = a.InverseExchangeRate,
                Description = a.Description,
                FromDate = General.FormatDate(a.FromDate, "view"),
                IsActive = a.IsActive,
            }).ToList();
            return View(currencyConversionModelList);
        }
        public ActionResult Details(int id)
        {
            //try
            //{
            var obj = icurrencyConversionContract.GetCurrencyConversionDetails(id);
            CurrencyConversionModel currencyConversionModel = new CurrencyConversionModel();
            currencyConversionModel.Id = obj.Id;
            currencyConversionModel.BaseCurrencyCode = obj.BaseCurrencyCode;
            currencyConversionModel.ConversionCurrencyCode = obj.ConversionCurrencyCode;
            currencyConversionModel.ExchangeRate = obj.ExchangeRate;
            currencyConversionModel.InverseExchangeRate = obj.InverseExchangeRate;
            currencyConversionModel.Description = obj.Description;
            currencyConversionModel.FromDate = General.FormatDate(obj.FromDate, "view");
            return View(currencyConversionModel);
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
        public ActionResult Save(CurrencyConversionModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CurrencyConversionBO countryBO = new CurrencyConversionBO()
                    {
                        BaseCurrencyCode = model.BaseCurrencyCode,
                        BaseCurrencyID = model.BaseCurrencyID,
                        ConversionCurrencyCode = model.ConversionCurrencyCode,
                        ConversionCurrencyID = model.ConversionCurrencyID,
                        ExchangeRate = model.ExchangeRate,
                        InverseExchangeRate = model.InverseExchangeRate,
                        Description = model.Description,
                        FromDate = General.ToDateTime(model.FromDate)
                    };
                    string ErrorMesage = icurrencyConversionContract.CreateCurrencyConversion(countryBO);
                    if (string.IsNullOrEmpty(ErrorMesage))
                        return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                    else
                    {
                        var res = new List<object>();
                        return Json(new { Status = "failure", data = res, Message = ErrorMesage }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetDebitCurrencyDetails(int BaseCurrencyID, int ConversionCurrencyID)
        {

            CurrencyConversionModel itemList = icurrencyConversionContract.GetCurrencyDetails(BaseCurrencyID, ConversionCurrencyID).Select(a => new CurrencyConversionModel
            {
                BaseCurrencyCode = a.BaseCurrencyCode,
                BaseCurrencyID = a.BaseCurrencyID,
                ConversionCurrencyCode = a.ConversionCurrencyCode,
                ConversionCurrencyID = a.ConversionCurrencyID,
                ExchangeRate = a.ExchangeRate,
                InverseExchangeRate = a.InverseExchangeRate
            }).FirstOrDefault();
            return Json(new { Status = "success", data = itemList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrencyDetails(int BaseCurrencyID, int ConversionCurrencyID)
        {

            CurrencyConversionModel itemList = icurrencyConversionContract.GetCurrencyDetails(BaseCurrencyID, ConversionCurrencyID).Select(a => new CurrencyConversionModel
               {
                   BaseCurrencyCode = a.BaseCurrencyCode,
                   BaseCurrencyID = a.BaseCurrencyID,
                   ConversionCurrencyCode = a.ConversionCurrencyCode,
                   ConversionCurrencyID = a.ConversionCurrencyID,
                   ExchangeRate = a.ExchangeRate,
                   InverseExchangeRate = a.InverseExchangeRate
               }).FirstOrDefault();
            return Json(new { Status = "success", data = itemList }, JsonRequestBehavior.AllowGet);
        }

    }
}