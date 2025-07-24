using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers
{
    public class PreProcessReceiptController : Controller
    {
        private IPreprocessIssueReceipt _preprocessIssueReceipt;
        private IGeneralContract generalBL;
        #region Constructor
        public PreProcessReceiptController(IPreprocessIssueReceipt preprocessIssueReceipt)
        {
            _preprocessIssueReceipt = preprocessIssueReceipt;
            generalBL = new GeneralBL();

        }
        #endregion

        // GET: Manufacturing/PreProcessReceipt
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "cancelled" };
            return View();
        }

        // GET: Manufacturing/PreProcessReceipt/Details/5
        public ActionResult Details(int id)
        {
            var materialPurificationReceipt = _preprocessIssueReceipt.GetMaterialPurificationReceipt(id).MapToModel();
            return View(materialPurificationReceipt);
        }

        // GET: Manufacturing/PreProcessReceipt/Create
        public ActionResult Create()
        {
            PreProcessReceiptViewModel preProcessReceiptViewModel = new PreProcessReceiptViewModel();
            preProcessReceiptViewModel.TransNo = generalBL.GetSerialNo("PurificationReceipt", "Code");
            preProcessReceiptViewModel.TransDate = DateTime.Now;
            preProcessReceiptViewModel.PreProcessReceiptPurificationItemModelList = new List<PreProcessReceiptPurificationItemModel>();
            return View(preProcessReceiptViewModel);
        }

        public PartialViewResult GetUnProcessedMaterialPrurificationIssueItemListView()
        {
            var unProcessedMaterials = _preprocessIssueReceipt.GetUnProcessedMaterialPurificationIssueItemList().MapToModelList();
            return PartialView("_unProcessedMaterialPurification", unProcessedMaterials);
        }

        /// <summary>
        /// GEt unprocessed material purification issue item list
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUnProcessedMaterialPurificationIssueItemListAutoComplete(string search)
        {
            var unProcessedMaterials = _preprocessIssueReceipt.GetUnProcessedMaterialPurificationIssueItemList(search).MapToModelList();

            return Json(unProcessedMaterials, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Save(PreProcessReceiptViewModel preProcessReceiptViewModel)
        {
            var result = new List<object>();
            try
            {
                if (preProcessReceiptViewModel.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PreprocessReceiptBO Temp = _preprocessIssueReceipt.GetMaterialPurificationReceipt(preProcessReceiptViewModel.ID);
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var preProcessReceiptID = 0;
                if (preProcessReceiptViewModel != null)
                {
                    var preProcessReceiptBO = preProcessReceiptViewModel.MapToBo();

                    preProcessReceiptID = _preprocessIssueReceipt.Save(preProcessReceiptBO);
                }
                return Json(new { Result = preProcessReceiptID, Message = preProcessReceiptID > 0 ? "Purification receipt saved successfully" : "Please try again" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = "Failed to Save, Please try again" });
                generalBL.LogError("Manufacturing", "PreProcessReceipt", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);

            }
        }


        // GET: Manufacturing/PreProcessReceipt/Edit/5
        public ActionResult Edit(int id)
        {
            var materialPurificationReceipt = _preprocessIssueReceipt.GetMaterialPurificationReceipt(id).MapToModel();
            if (materialPurificationReceipt.IsDraft)
                return View(materialPurificationReceipt);
            else return RedirectToAction("Index");
        }
        public ActionResult Cancel(int ID, string Table)
        {
            _preprocessIssueReceipt.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAsDraft(PreProcessReceiptViewModel preProcessReceiptViewModel)
        {
            return Save(preProcessReceiptViewModel);
        }

        public JsonResult GetPreProcessReceiptList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string IssueItemHint = Datatable.Columns[2].Search.Value;
                string UnitHint = Datatable.Columns[3].Search.Value;
                string IssueQtyHint = Datatable.Columns[4].Search.Value;
                string ReceiptItemHint = Datatable.Columns[5].Search.Value;
                string ReceiptUnitHint = Datatable.Columns[6].Search.Value;
                string ReceiptQtyHint = Datatable.Columns[7].Search.Value;
                string ActivityHint = Datatable.Columns[8].Search.Value;
                string QuantityLossHint = Datatable.Columns[9].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = _preprocessIssueReceipt.GetPreProcessReceiptList(Type, TransNoHint, IssueItemHint, UnitHint, IssueQtyHint, ReceiptItemHint, ReceiptUnitHint, ReceiptQtyHint, ActivityHint, QuantityLossHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
