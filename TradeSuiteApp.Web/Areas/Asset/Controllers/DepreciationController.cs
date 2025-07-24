using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Asset.Models;
using TradeSuiteApp.Web.Utils;
using TradeSuiteApp.Web.Models;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;

namespace TradeSuiteApp.Web.Areas.Asset.Controllers
{
    public class DepreciationController : Controller
    {
        IAssetContract assetBL;
        IDepreciationContract depreciationBL;
        public DepreciationController()
        {
            assetBL = new AssetBL();
            depreciationBL = new DepreciationBL();
        }
        // GET: Asset/Depreciation
        public ActionResult Index()
        {
            DepreciationFilterModel depreciation = new DepreciationFilterModel();
            depreciation.TransDateFromStr = GeneralBO.FinStartDate;
            depreciation.TransDateToStr = General.FormatDate(DateTime.Now);
            return View(depreciation);
        }
        public JsonResult GetCapitalList(DatatableModel Datatable)
        {
            try
            {
                DepreciationFilterBO depreciation = new DepreciationFilterBO();
                depreciation.FromCompanyDepreciationRate = Convert.ToDecimal(Datatable.GetValueFromKey("FromCompanyDepreciationRate", Datatable.Params));
                depreciation.ToCompanyDepreciationRate = Convert.ToDecimal(Datatable.GetValueFromKey("ToCompanyDepreciationRate", Datatable.Params));
                depreciation.TransDateFrom = General.ToDateTime(Datatable.GetValueFromKey("TransDateFrom", Datatable.Params));
                depreciation.TransDateTo = General.ToDateTime(Datatable.GetValueFromKey("TransDateTo", Datatable.Params));
                depreciation.FromIncomeTaxDepreciationRate = Convert.ToDecimal(Datatable.GetValueFromKey("FromIncomeTaxDepreciationRate", Datatable.Params));
                depreciation.ToIncomeTaxDepreciationRate = Convert.ToDecimal(Datatable.GetValueFromKey("ToIncomeTaxDepreciationRate", Datatable.Params));
                DatatableResultBO resultBO = assetBL.GetCapitalList("Capital", depreciation, Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)

            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CalculateDepreciation()
        {
            int success = 0;
            try
            {

                success = depreciationBL.CalculateDepreciation();
                Index();
                return Json(new { Status = "success", data = "Successfully depreciation calculated" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { Status = "failure", data = "Some error occured while calculating" }, JsonRequestBehavior.AllowGet);

            }
        }
        // GET: Asset/Depreciation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Asset/Depreciation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asset/Depreciation/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Asset/Depreciation/Edit/5
        public ActionResult Edit(int id)
        {
            DepreciationModel deprecaition = new DepreciationModel();
            deprecaition.DepreciationTypeList = new SelectList(
                                        new List<SelectListItem>
                                        {
                                               new SelectListItem { Text = "CompanyDepreciationAct", Value = "CompanyDepreciationAct"},
                                               new SelectListItem { Text = "IncomeTaxDepreciationAct", Value ="IncomeTaxDepreciationAct"},
                                        }, "Value", "Text");
            return View(deprecaition);
        }
    }
}
