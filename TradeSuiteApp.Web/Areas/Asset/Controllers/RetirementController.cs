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
    public class RetirementController : Controller
    {
        private SelectList temp;
        private IRetirementContract retirementBL;
        private IGeneralContract generalBL;

        public RetirementController()
        {
            retirementBL = new RetirementBL();
            generalBL = new GeneralBL();
            temp = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
        }
        // GET: Asset/Retirement
        public ActionResult Index()
        {
            RetirementFilterModel retirement = new RetirementFilterModel();
            retirement.AssetNameRangeList = temp;
            retirement.CapitalisationDateFromStr = GeneralBO.FinStartDate;
            retirement.CapitalisationDateToStr = General.FormatDate(DateTime.Now);
            return View(retirement);
        }
        public JsonResult GetAssetForRetirementList(DatatableModel Datatable)
        {
            try
            {
                RetirementFilterBO retirement = new RetirementFilterBO();
                retirement.CapitalisationDateFrom = General.ToDateTime(Datatable.GetValueFromKey("CapitalisationDateFrom", Datatable.Params));
                retirement.CapitalisationDateTo = General.ToDateTime(Datatable.GetValueFromKey("CapitalisationDateTo", Datatable.Params));
                retirement.AssetNameFrom = Convert.ToString(Datatable.GetValueFromKey("AssetNameFrom", Datatable.Params));
                retirement.AssetNameTo = Convert.ToString(Datatable.GetValueFromKey("AssetNameTo", Datatable.Params));
                retirement.AssetName = Convert.ToString(Datatable.GetValueFromKey("AssetName", Datatable.Params));
                retirement.AssetCodeFrom = Convert.ToString(Datatable.GetValueFromKey("AssetCodeFrom", Datatable.Params));
                retirement.AssetCodeTo = Convert.ToString(Datatable.GetValueFromKey("AssetCodeTo", Datatable.Params));
                DatatableResultBO resultBO = retirementBL.GetAssetForRetirementList(retirement, Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)

            {
                var res = new List<object>();               
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Asset/Retirement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Asset/Retirement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asset/Retirement/Create
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

        // GET: Asset/Retirement/Edit/5
        public ActionResult Edit(int id)
        {
            RetirementModel retirement = retirementBL.GetAssetForRetirement(id).Select(a => new RetirementModel()
            {
                ID = id,
                CapitalisationDateStr = General.FormatDate((DateTime)a.CapitalisationDate, "view"),
                ItemName = a.ItemName,
                AssetName = a.AssetName,
                AssetCode = a.AssetCode,
                AssetUniqueNo = a.AssetUniqueNo,
                ItemAccountCategory = a.ItemAccountCategory,
                ResidualValue = a.ResidualValue,
                Location = a.Location,
                ClosingWDV = a.ClosingWDV,
                ClosingGrossBlockValue = a.ClosingGrossBlockValue,
                ClosingAccumulatedDepreciation = a.ClosingAccumulatedDepreciation,
                AssetQty = a.AssetQty

            }).FirstOrDefault();
            retirement.DateStr = General.FormatDate(DateTime.Now);
            retirement.EndDateStr = General.FormatDate(DateTime.Now);
            retirement.RetirementTransNo = generalBL.GetSerialNo("AssetRetirement", "Code");
            return View(retirement);
        }
        [HttpPost]
        public ActionResult Save(RetirementModel model)
        {
            try
            {
                // TODO: Add insert logic here
                AssetRetirementBO retirement = new AssetRetirementBO();
                retirement.ID = model.ID;
                retirement.EndDate = model.EndDate;
                retirement.Date = model.Date;
                retirement.Status = model.Status;
                retirement.ClosingAccumulatedDepreciation = model.ClosingAccumulatedDepreciation;
                retirement.ClosingGrossBlockValue = model.ClosingGrossBlockValue;
                retirement.ClosingWDV = model.ClosingWDV;
                retirement.SaleQty = model.SaleQty;
                retirement.SaleValue = model.SaleValue;
                if (retirementBL.Save(retirement))
                {
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "fail" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
