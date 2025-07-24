using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Asset.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Asset.Controllers
{
    public class AssetCorrectionController : Controller
    {
        private SelectList temp;
        private IAssetCorrectionContract assetCorrrectionBL;
        private IDropdownContract _dropdown;
        private IGeneralContract generalBL;

        public AssetCorrectionController(IDropdownContract dropdown)
        {
            assetCorrrectionBL = new AssetCorrectionBL();
            _dropdown = dropdown;
            generalBL = new GeneralBL();
            temp = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());

        }
        // GET: Asset/AssetCorrection
        public ActionResult Index()
        {
            AssetCorrectionFilterModel correction = new AssetCorrectionFilterModel();
            correction.AssetNameRangeList = temp;
            return View(correction);
        }
        public JsonResult GetCapitalListForCorrection(DatatableModel Datatable)
        {
            try
            {
                AssetCorrectionFilterBO correction = new AssetCorrectionFilterBO();
                correction.AssetCodeFrom = Convert.ToString(Datatable.GetValueFromKey("AssetCodeFrom", Datatable.Params));
                correction.AssetCodeTo = Convert.ToString(Datatable.GetValueFromKey("AssetCodeTo", Datatable.Params));
                correction.AssetNameFrom = Convert.ToString(Datatable.GetValueFromKey("AssetNameFrom", Datatable.Params));
                correction.AssetNameTo = Convert.ToString(Datatable.GetValueFromKey("AssetNameTo", Datatable.Params));
                correction.AssetName = Convert.ToString(Datatable.GetValueFromKey("AssetName", Datatable.Params));
                DatatableResultBO resultBO = assetCorrrectionBL.GetCapitalListForCorrection(correction, Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)

            {
                var res = new List<object>();                
                return Json(new { Status = "failure", data = res }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Asset/AssetCorrection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Asset/AssetCorrection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asset/AssetCorrection/Create
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

        // GET: Asset/AssetCorrection/Edit/5
        public ActionResult Edit(int id)
        {
            AssetCorrectionModel correction = new AssetCorrectionModel();

            correction = assetCorrrectionBL.GetAssetForCorrection(id).Select(a => new AssetCorrectionModel()
            {
                ID = id,
                ItemName = a.ItemName,
                AssetUniqueNo = a.AssetUniqueNo,
                ItemAccountCategory = a.ItemAccountCategory,
                AssetCode = a.AssetCode,
                AssetName = a.AssetName,
                DebitACCode = a.DebitACCode,
                DebitACName = a.DebitACName,
                CreditACCode = a.CreditACCode,
                CreditACName = a.CreditACName,
                AssetValue = a.AssetValue,
                DebitACID = a.DebitACID,
                CreditACID = a.CreditACID
            }).FirstOrDefault();
            correction.CorrectionDateStr = General.FormatDate(DateTime.Now);
            correction.CorrectionTransNumber = generalBL.GetSerialNo("AssetCorrection", "Code");
            return View(correction);
        }
        public ActionResult Save(AssetCorrectionModel model)
        {
            try
            {
                // TODO: Add insert logic here
                AssetCorrectionBO correction = new AssetCorrectionBO();
                correction.ID = model.ID;
                correction.CorrectionDate = model.CorrectionDate;
                correction.AmountValue = model.AmountValue;
                correction.DebitACID = model.DebitACID;
                correction.CreditACID = model.CreditACID;
                correction.IsAdditionDuringYear = model.IsAdditionDuringYear;
                correction.IsDepreciation = model.IsDepreciation;
                correction.Remark = model.Remark;

                if (assetCorrrectionBL.Save(correction))
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
