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
    public class AssetController : Controller
    {
        private SelectList temp;
        private IAssetContract assetBL;
        private IGeneralContract generalBL;

        public AssetController()
        {
            assetBL = new AssetBL();
            generalBL = new GeneralBL();
            temp = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
        }
        // GET: Asset/Asset
        public ActionResult Index()
        {
            AssetFilterModel asset = new AssetFilterModel();
            asset.SupplierNameRangeList = temp;
            asset.AssetNameRangeList = temp;
            asset.AccountCategoryRangeList = temp;
            asset.TransDateFromStr = GeneralBO.FinStartDate;
            asset.TransDateToStr = General.FormatDate(DateTime.Now);
            asset.AccountCategoryList = new SelectList(assetBL.GetAccountCategoryList(), "ID", "Name");

            return View(asset);
        }
        public JsonResult GetPendingList(DatatableModel Datatable)
        {
            try
            {
                AssetFilterBO asset = new AssetFilterBO();

                asset.TransNoFromID = Convert.ToInt32(Datatable.GetValueFromKey("TransNoFromID", Datatable.Params));
                asset.TransNoToID = Convert.ToInt32(Datatable.GetValueFromKey("TransNoToID", Datatable.Params));
                asset.TransDateFrom = General.ToDateTime(Datatable.GetValueFromKey("TransDateFrom", Datatable.Params));
                asset.TransDateTo = General.ToDateTime(Datatable.GetValueFromKey("TransDateTo", Datatable.Params));
                asset.ReceiptNoFromID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptNoFromID", Datatable.Params));
                asset.ReceiptNoToID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptNoToID", Datatable.Params));
                asset.AssetNameFrom = Convert.ToString(Datatable.GetValueFromKey("AssetNameFrom", Datatable.Params));
                asset.AssetNameTo = Convert.ToString(Datatable.GetValueFromKey("AssetNameTo", Datatable.Params));
                asset.AssetName = Convert.ToString(Datatable.GetValueFromKey("AssetName", Datatable.Params));
                asset.AccountCategoryFrom = Convert.ToString(Datatable.GetValueFromKey("AccountCategoryFrom", Datatable.Params));
                asset.AccountCategoryTo = Convert.ToString(Datatable.GetValueFromKey("AccountCategoryTo", Datatable.Params));
                asset.AccountCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("AccountCategoryID", Datatable.Params));
                asset.SupplierNameFrom = Convert.ToString(Datatable.GetValueFromKey("SupplierNameFrom", Datatable.Params));
                asset.SupplierNameTo = Convert.ToString(Datatable.GetValueFromKey("SupplierNameTo", Datatable.Params));
                asset.SupplierID = Convert.ToInt32(Datatable.GetValueFromKey("SupplierID", Datatable.Params));
                if (asset.AccountCategoryID != 0)
                {
                    asset.AccountCategoryFrom = asset.AccountCategoryTo = null;
                }
                DatatableResultBO resultBO = assetBL.GetAssetList("Pending", asset, Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)

            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetRevenueList(DatatableModel Datatable)
        {
            try
            {
                AssetFilterBO asset = new AssetFilterBO();

                asset.TransNoFromID = Convert.ToInt32(Datatable.GetValueFromKey("TransNoFromID", Datatable.Params));
                asset.TransNoToID = Convert.ToInt32(Datatable.GetValueFromKey("TransNoToID", Datatable.Params));
                asset.TransDateFrom = General.ToDateTime(Datatable.GetValueFromKey("TransDateFrom", Datatable.Params));
                asset.TransDateTo = General.ToDateTime(Datatable.GetValueFromKey("TransDateTo", Datatable.Params));
                asset.ReceiptNoFromID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptNoFromID", Datatable.Params));
                asset.ReceiptNoToID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptNoToID", Datatable.Params));
                asset.AssetNameFrom = Convert.ToString(Datatable.GetValueFromKey("AssetNameFrom", Datatable.Params));
                asset.AssetNameTo = Convert.ToString(Datatable.GetValueFromKey("AssetNameTo", Datatable.Params));
                asset.AssetName = Convert.ToString(Datatable.GetValueFromKey("AssetName", Datatable.Params));
                asset.AccountCategoryFrom = Convert.ToString(Datatable.GetValueFromKey("AccountCategoryFrom", Datatable.Params));
                asset.AccountCategoryTo = Convert.ToString(Datatable.GetValueFromKey("AccountCategoryTo", Datatable.Params));
                asset.AccountCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("AccountCategoryID", Datatable.Params));
                asset.SupplierNameFrom = Convert.ToString(Datatable.GetValueFromKey("SupplierNameFrom", Datatable.Params));
                asset.SupplierNameTo = Convert.ToString(Datatable.GetValueFromKey("SupplierNameTo", Datatable.Params));
                asset.SupplierID = Convert.ToInt32(Datatable.GetValueFromKey("SupplierID", Datatable.Params));
                DatatableResultBO resultBO = assetBL.GetAssetList("Revenue", asset, Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)

            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCapitalList(DatatableModel Datatable)
        {
            try

            {
                AssetFilterBO asset = new AssetFilterBO();
                asset.TransNoFromID = Convert.ToInt32(Datatable.GetValueFromKey("TransNoFromID", Datatable.Params));
                asset.TransNoToID = Convert.ToInt32(Datatable.GetValueFromKey("TransNoToID", Datatable.Params));
                asset.TransDateFrom = General.ToDateTime(Datatable.GetValueFromKey("TransDateFrom", Datatable.Params));
                asset.TransDateTo = General.ToDateTime(Datatable.GetValueFromKey("TransDateTo", Datatable.Params));
                asset.ReceiptNoFromID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptNoFromID", Datatable.Params));
                asset.ReceiptNoToID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptNoToID", Datatable.Params));
                asset.AssetNameFrom = Convert.ToString(Datatable.GetValueFromKey("AssetNameFrom", Datatable.Params));
                asset.AssetNameTo = Convert.ToString(Datatable.GetValueFromKey("AssetNameTo", Datatable.Params));
                asset.AssetName = Convert.ToString(Datatable.GetValueFromKey("AssetName", Datatable.Params));
                asset.AccountCategoryFrom = Convert.ToString(Datatable.GetValueFromKey("AccountCategoryFrom", Datatable.Params));
                asset.AccountCategoryTo = Convert.ToString(Datatable.GetValueFromKey("AccountCategoryTo", Datatable.Params));
                asset.AccountCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("AccountCategoryID", Datatable.Params));
                asset.SupplierNameFrom = Convert.ToString(Datatable.GetValueFromKey("SupplierNameFrom", Datatable.Params));
                asset.SupplierNameTo = Convert.ToString(Datatable.GetValueFromKey("SupplierNameTo", Datatable.Params));
                asset.SupplierID = Convert.ToInt32(Datatable.GetValueFromKey("SupplierID", Datatable.Params));
                DatatableResultBO resultBO = assetBL.GetAssetList("Capital", asset, Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)

            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAssetList(DatatableModel Datatable)
        {
            try

            {
                AssetFilterBO asset = new AssetFilterBO();
                asset.TransNoFromID = Convert.ToInt32(Datatable.GetValueFromKey("TransNoFromID", Datatable.Params));
                asset.TransNoToID = Convert.ToInt32(Datatable.GetValueFromKey("TransNoToID", Datatable.Params));
                asset.TransDateFrom = General.ToDateTime(Datatable.GetValueFromKey("TransDateFrom", Datatable.Params));
                asset.TransDateTo = General.ToDateTime(Datatable.GetValueFromKey("TransDateTo", Datatable.Params));
                asset.ReceiptNoFromID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptNoFromID", Datatable.Params));
                asset.ReceiptNoToID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptNoToID", Datatable.Params));
                asset.AssetNameFrom = Convert.ToString(Datatable.GetValueFromKey("AssetNameFrom", Datatable.Params));
                asset.AssetNameTo = Convert.ToString(Datatable.GetValueFromKey("AssetNameTo", Datatable.Params));
                asset.AssetName = Convert.ToString(Datatable.GetValueFromKey("AssetName", Datatable.Params));
                asset.AccountCategoryFrom = Convert.ToString(Datatable.GetValueFromKey("AccountCategoryFrom", Datatable.Params));
                asset.AccountCategoryTo = Convert.ToString(Datatable.GetValueFromKey("AccountCategoryTo", Datatable.Params));
                asset.AccountCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("AccountCategoryID", Datatable.Params));
                asset.SupplierNameFrom = Convert.ToString(Datatable.GetValueFromKey("SupplierNameFrom", Datatable.Params));
                asset.SupplierNameTo = Convert.ToString(Datatable.GetValueFromKey("SupplierNameTo", Datatable.Params));
                asset.SupplierID = Convert.ToInt32(Datatable.GetValueFromKey("SupplierID", Datatable.Params));
                DatatableResultBO resultBO = assetBL.GetAssetList("Capital", asset, Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)

            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAssetRange(string from_range)
        {
            char range = Convert.ToChar(from_range);
            AssetFilterModel asset = new AssetFilterModel();
            asset.AssetNameRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            return Json(new { Status = "success", data = asset.AssetNameRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategoryRange(string from_range)
        {
            char range = Convert.ToChar(from_range);
            AssetFilterModel asset = new AssetFilterModel();
            asset.AccountCategoryRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            return Json(new { Status = "success", data = asset.AccountCategoryRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSupplierRange(string from_range)
        {
            char range = Convert.ToChar(from_range);
            AssetFilterModel asset = new AssetFilterModel();
            asset.SupplierNameRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            return Json(new { Status = "success", data = asset.SupplierNameRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssetNumber()
        {
            
            var AssetNumber = generalBL.GetSerialNo("Capital", "code");

            return Json(new { Status = "success", data = AssetNumber }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssetUniqueNumber(string Hint)
        {

            var count = assetBL.GetAssetUniqueNoCount(Hint);

            return Json(new { Status = "success", data = count }, JsonRequestBehavior.AllowGet);
        }
        // GET: Asset/Asset/Details/5
        public ActionResult Details(int id)
        {

            AssetModel asset = assetBL.GetAsset(id).Select(a => new AssetModel()
            {
                ID = id,
                TransNo = a.TransNo,
                TransDateStr = General.FormatDate((DateTime)a.TransDate, "view"),
                ItemName = a.ItemName,
                AssetName = a.AssetName,
                AssetCode = a.AssetCode,
                AssetUniqueNo = a.AssetUniqueNo,
                SupplierName = a.SupplierName,
                ItemAccountCategory = a.ItemAccountCategory,
                IsRepairable = a.IsRepairable,
                CompanyDepreciationRate = a.CompanyDepreciationRate,
                IncomeTaxDepreciationRate = a.IncomeTaxDepreciationRate,
                AssetValue = a.AssetValue,
                AdditionToAssetNo = a.AdditionToAssetNo,
                ResidualValue = a.ResidualValue,
                DepreciationStartDateStr = General.FormatDate((DateTime)a.DepreciationStartDate, "view"),
                DepreciationEndDateStr = General.FormatDate((DateTime)a.DepreciationEndDate, "view"),
                LifeInYears = a.LifeInYears,
                Department = a.Department,
                Location = a.Location,
                Employee = a.Employee,
                Project = a.Project,
                Remark = a.Remark,
                Status = a.Status,
                IsCapital = a.IsCapital,
                Qty = a.Qty
            }).FirstOrDefault();
            asset.StatusList = new SelectList(
                                                new List<SelectListItem>
                                                {
                                               new SelectListItem { Text = "Capital", Value = "Capital"},
                                               new SelectListItem { Text = "Revenue", Value ="Revenue"},
                                                }, "Value", "Text");

            return View(asset);
        }



        // GET: Asset/Asset/Edit/5
        public ActionResult Edit(int id)
        {

            AssetModel asset = assetBL.GetAsset(id).Select(a => new AssetModel()
            {
                ID = id,
                TransNo = a.TransNo,
                ItemName = a.ItemName,
                SupplierName = a.SupplierName,
                AssetUniqueNo = a.AssetUniqueNo,
                ItemAccountCategory = a.ItemAccountCategory,
                IsRepairable = a.IsRepairable,
                CompanyDepreciationRate = a.CompanyDepreciationRate,
                IncomeTaxDepreciationRate = a.IncomeTaxDepreciationRate,
                AssetValue = a.AssetValue,
                AdditionToAssetNo = a.AdditionToAssetNo,
                ResidualValue = a.ResidualValue,
                DepreciationStartDateStr = a.DepreciationStartDate.ToString(),
                Department = a.Department,
                Location = a.Location,
                Employee = a.Employee,
                Project = a.Project,
                Remark = a.Remark,
                Status = a.Status,
                IsCapital = false,
                Qty = a.Qty
            }).First();
            asset.StatusList = new SelectList(
                                          new List<SelectListItem>
                                          {
                                               new SelectListItem { Text = "Capital", Value = "Capital"},
                                               new SelectListItem { Text = "Revenue", Value ="Revenue"},
                                          }, "Value", "Text");

            asset.TransDateStr = General.FormatDate(DateTime.Now);
            asset.StatusChangeDateStr = General.FormatDate(DateTime.Now);
            return View(asset);
        }

        [HttpPost]
        public ActionResult Save(AssetModel model)
        {
            try
            {
                // TODO: Add insert logic here
                AssetBO asset = new AssetBO();


                asset.ID = model.ID;
                asset.TransDate = model.TransDate;
                asset.AssetName = model.AssetName;
                asset.IsRepairable = model.IsRepairable;
                asset.CompanyDepreciationRate = model.CompanyDepreciationRate;
                asset.IncomeTaxDepreciationRate = model.IncomeTaxDepreciationRate;
                asset.LifeInYears = model.LifeInYears;
                asset.ResidualValue = model.ResidualValue;
                asset.Status = model.Status;
                asset.IsDraft = model.IsDraft;
                asset.AdditionToAssetNo = model.AdditionToAssetNo;
                asset.StatusChangeDate = model.StatusChangeDate;
                asset.Remark = model.Remark;
                asset.AssetUniqueNo = model.AssetUniqueNo;
                asset.DepreciationStartDate = asset.StatusChangeDate;
                decimal x = (decimal)asset.LifeInYears;
                int nbYear = (int)(asset.LifeInYears);
                var y = x - Math.Truncate(x);
                int nbMonth = Convert.ToInt16(y * 12);
                asset.DepreciationEndDate = ((DateTime)asset.StatusChangeDate).AddYears(nbYear);
                asset.DepreciationEndDate = ((DateTime)asset.DepreciationEndDate).AddMonths(nbMonth);

                if (assetBL.Save(asset))
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


        [HttpPost]
        public ActionResult ChangeStatus(AssetModel model)
        {
            try
            {
                // TODO: Add insert logic here
                AssetBO asset = new AssetBO();
                asset.ID = model.ID;
                asset.Status = model.Status;
                model.TransDateStr = General.FormatDate(DateTime.Now);
                asset.TransDate = model.TransDate;
                asset.AssetCode = model.AssetCode;
                asset.IsCapital = model.IsCapital;
                decimal x = (decimal)model.LifeInYears;
                int nbYear = (int)(model.LifeInYears);
                var y = x - Math.Truncate(x);
                int nbMonth = Convert.ToInt16(y * 12);
                asset.DepreciationEndDate = ((DateTime)asset.TransDate).AddYears(nbYear);
                asset.DepreciationEndDate = ((DateTime)asset.DepreciationEndDate).AddMonths(nbMonth);

                if (assetBL.ChangeStatus(asset))
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
