using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class AdvanceRequestController : Controller
    {
        #region Private members
        private IDropdownContract _dropdown;
        private IAdvanceRequestContract advanceRequestBL;
        private IGeneralContract generalBL;
        private IFileContract fileBL;
        #endregion

        #region Constructor
        public AdvanceRequestController(IDropdownContract IDropdown)
        {
            advanceRequestBL = new AdvanceRequestBL();
            _dropdown = IDropdown;
            generalBL = new GeneralBL();
            fileBL = new FileBL();
        }
        #endregion

        #region Public methods

        // GET: Accounts/AdvanceRequest
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "processed" };
            return View();
        }
        //GET:Accounts/AdvanceRequest/Create
        [HttpGet]
        public ActionResult Create()
        {
            AdvanceRequestModel AdvanceRequest = new AdvanceRequestModel();
            AdvanceRequest.AdvanceRequestNo = generalBL.GetSerialNo("AdvanceRequest", "Code");
            AdvanceRequest.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
            AdvanceRequest.AdvanceCategoryList = new SelectList(
                                              new List<SelectListItem>
                                              {
                                                new SelectListItem { Text = "Official", Value = "Advances Staff Official"},
                                                new SelectListItem { Text = "Personal", Value = "Advances Staff Personal"},
                                              }, "Value", "Text");

            AdvanceRequest.AdvanceRequestDate = General.FormatDate(DateTime.Now);
            return View(AdvanceRequest);
        }

        [HttpPost]
        public ActionResult Create(AdvanceRequestModel model, List<AdvanceRequestTransModel> trans)
        {
            try
            {
                // TODO: Add insert logic here
                AdvanceRequestBO advanceRequestBO = new AdvanceRequestBO();

                advanceRequestBO.AdvanceRequestNo = model.AdvanceRequestNo;
                advanceRequestBO.AdvanceRequestDate = General.ToDateTime(model.AdvanceRequestDate);
                advanceRequestBO.Amount = model.Amount;
                advanceRequestBO.SelectedQuotationID = model.SelectedQuotationID;
                var ItemList = new List<AdvanceRequestTransBO>();
                AdvanceRequestTransBO advanceRequestTransBO;
                foreach (var itm in trans)
                {
                    advanceRequestTransBO = new AdvanceRequestTransBO();
                    advanceRequestTransBO.EmployeeID = itm.EmployeeID;
                    advanceRequestTransBO.ItemID = itm.ItemID;
                    advanceRequestTransBO.Amount = itm.Amount;
                    advanceRequestTransBO.Remarks = itm.Remarks;
                    advanceRequestTransBO.IsOfficial = itm.IsOfficial;

                    advanceRequestTransBO.ExpectedDate = General.ToDateTime(itm.ExpectedDate);

                    ItemList.Add(advanceRequestTransBO);
                }

                var outId = advanceRequestBL.SaveAdvanceRequest(advanceRequestBO, ItemList);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Accounts", "Advancerequest", "Create", (int)model.ID, e);
                return Json("fail", JsonRequestBehavior.AllowGet);
            }

        }

        public PartialViewResult Summary(int ID)
        {
            return null;
        }

        public JsonResult GetAdvanceRequestListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string AdvanceRequestNo = Datatable.Columns[1].Search.Value;
                string AdvanceRequestDate = Datatable.Columns[2].Search.Value;
                string EmployeeName = Datatable.Columns[3].Search.Value;
                string Amount = Datatable.Columns[4].Search.Value;


                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = advanceRequestBL.GetAdvanceRequestListForDataTable(Type,AdvanceRequestNo, AdvanceRequestDate, EmployeeName, Amount, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        //GET:Accounts/AdvanceRequest/Details
        public ActionResult Details(int id)
        {

            try
            {
                AdvanceRequestModel AdvanceRequest = advanceRequestBL.GetAdvanceRequestList((int)id).Select(a => new AdvanceRequestModel()
                {
                    ID = a.ID,
                    AdvanceRequestNo = a.AdvanceRequestNo,
                    AdvanceRequestDate = General.FormatDate(a.AdvanceRequestDate, "view"),
                    Amount = a.Amount,
                    Status = a.IsProcessed ? "processed" : "",
                    IsSuspend=a.IsSuspend,
                    SelectedQuotationID=a.SelectedQuotationID

                }).FirstOrDefault();
                AdvanceRequest.Item = advanceRequestBL.GetAdvanceRequesTrans((int)id).Select(a => new AdvanceRequestTransModel()
                {
                    ItemName = a.ItemName,
                    EmployeeName = a.EmployeeName,
                    Amount = a.Amount,
                    Remarks = a.Remarks,
                    ExpectedDate = General.FormatDate(a.ExpectedDate, "view"),
                    Category = a.Category

                }).ToList();
                AdvanceRequest.SelectedQuotation = fileBL.GetAttachments(AdvanceRequest.SelectedQuotationID.ToString());
                //if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.Contains("Approval"))
                //{
                //    ViewBag.CloseURL = "/Approval";
                //}
                return View(AdvanceRequest);
            }
            catch (Exception e)
            {
                generalBL.LogError("Accounts", "AdvanceRequest", "Details", id, e);
                return RedirectToAction("index");
            }
        }

        public ActionResult Suspend(int ID)
        {
            var output = advanceRequestBL.Suspend(ID);
            return Json(new { Status = "success", Data = output }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}