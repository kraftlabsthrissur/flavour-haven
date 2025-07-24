using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers
{
    public class PreProcessIssueController : Controller
    {
        private IPreprocessIssue _preprocessIssue;
        private IGeneralContract generalBL;

        #region Constructor
        public PreProcessIssueController(IPreprocessIssue preprocessIssue)
        {
            _preprocessIssue = preprocessIssue;
            generalBL = new GeneralBL();
        }
        #endregion

        // GET: Manufacturing/PreProcessIssue
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Manufacturing/PreProcessIssue/Details/5
        public ActionResult Details(int id)
        {
            var preProcessViewModel = _preprocessIssue.GetPreProcessIssue(id).MapToModel();
            return View(preProcessViewModel);
        }

        // GET: Manufacturing/PreProcessIssue/Create
        public ActionResult Create()
        {
            PreProcessIssueViewModel preProcessIssueViewModel = new PreProcessIssueViewModel();
            preProcessIssueViewModel.IssueNo = generalBL.GetSerialNo("PurificationIssue", "Code");
            preProcessIssueViewModel.IssueDate = DateTime.Now;
            preProcessIssueViewModel.Items = new List<PreProcessIssueItemModel>();
            //      preProcessIssueViewModel.Items = _preprocessIssue.GetPreProcessIssueItems(_locationID, _applicationID).MapToModelList();
            return View(preProcessIssueViewModel);
        }

        [HttpPost]
        public ActionResult Save(PreProcessIssueViewModel preProcessIssueViewModel)
        {
            var result = new List<object>();
            try
            {
                if (preProcessIssueViewModel.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PreprocessIssueBO Temp = _preprocessIssue.GetPreProcessIssue(preProcessIssueViewModel.ID);
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var preProcessIssueBO = preProcessIssueViewModel.MapToBo();
                _preprocessIssue.Save(preProcessIssueBO);
                return Json(new { Status = "success", Message = "Purification issue saved successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (OutofStockException e)
            {
                result.Add(new { ErrorMessage = "Item out of stock" });
                generalBL.LogError("Manufacturing", "PreProcessIssue", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = "Failed to Save, Please try again" });
                generalBL.LogError("Manufacturing", "PreProcessIssue", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                
            }
        }

        // GET: Manufacturing/PreProcessIssue/Edit/5
        public ActionResult Edit(int id)
        {
            var preProcessViewModel = _preprocessIssue.GetPreProcessIssue(id).MapToModel();
            if (preProcessViewModel.IsDraft)
                return View(preProcessViewModel);
            else
                return RedirectToAction("Details", new { id = id });
        }
        public ActionResult Cancel(int ID, string Table)
        {
            generalBL.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAsDraft(PreProcessIssueViewModel preProcessIssueViewModel)
        {
            return Save(preProcessIssueViewModel);
        }

        public JsonResult GetPreProcessIssueList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string CreatedUserHint = Datatable.Columns[3].Search.Value;
                string ItemNameHint = Datatable.Columns[4].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = _preprocessIssue.GetPreProcessIssueList(Type, TransNoHint, TransDateHint, CreatedUserHint, ItemNameHint, SortField, SortOrder, Offset, Limit);
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
