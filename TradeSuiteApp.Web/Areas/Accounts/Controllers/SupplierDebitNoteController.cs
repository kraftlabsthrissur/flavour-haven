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
    public class SupplierDebitNoteController : Controller
    {
        #region Private members
        private IDropdownContract _dropdown;
        private ISupplierDebitNoteContract debitnoteBL;
        private IDepartmentContract departmentBL;
        private IEmployeeContract employeeBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        private IPeriodClosingContract periodClosingBL;
        private ISupplierCreditNoteContract creditnoteBL;
        private IAddressContract addressBL;
        #endregion

        #region Constructor
        public SupplierDebitNoteController(IDropdownContract dropdown)
        {
            debitnoteBL = new SupplierDebitNoteBL();
            departmentBL = new DepartmentBL();
            employeeBL = new EmployeeBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
            periodClosingBL = new PeriodClosingBL();
            creditnoteBL = new SupplierCreditNoteBL();
            addressBL = new AddressBL();
            this._dropdown = dropdown;
        }
        #endregion

        #region Public methods
        // GET: Accounts/SupplierDebitNote
        public ActionResult Index()
        {
            return View();
        }
        //GET: Accounts/SupplierDebitNote/Create
        public ActionResult Create()
        {
            SupplierDebitNoteModel DebitNote = new SupplierDebitNoteModel();

            DebitNote.TransNo = generalBL.GetSerialNo("SupplierDebitNote", "Code");

            DebitNote.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("SDNStatus").ToString("MM/dd/yyyy");
            DebitNote.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            DebitNote.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");

            DebitNote.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");

            DebitNote.EmployeeList = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            DebitNote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            DebitNote.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
            DebitNote.Date = General.FormatDate(DateTime.Now);

            return View(DebitNote);
        }

        // POST: Account/SupplierDebitNoteM/Create
        [HttpPost]
        public ActionResult Save(SupplierDebitNoteModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    SupplierDebitNoteBO Temp = debitnoteBL.GetDebitNoteDetail(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                SupplierDebitNoteBO DebitNote = new SupplierDebitNoteBO()
                {
                    TransNo = model.TransNo,
                    Date = General.ToDateTime(model.Date),
                    SupplierID = model.SupplierID,
                    TotalAmount = model.TotalAmount,
                    CGSTAmt = model.CGSTAmt,
                    IGSTAmt = model.IGSTAmt,
                    SGSTAmt = model.SGSTAmt,
                    TaxableAmount = model.TaxableAmount,
                    IsDraft = model.IsDraft,
                    ID = model.ID,
                    RoundOff = model.RoundOff
                };
                List<SupplierDebitNoteTransBO> ItemList = new List<SupplierDebitNoteTransBO>();
                SupplierDebitNoteTransBO debitNoteItem;
                foreach (var item in model.Items)
                {
                    debitNoteItem = new SupplierDebitNoteTransBO()
                    {
                        CreditNoteID = item.CreditNoteID,
                        ReferenceInvoiceNumber = item.ReferenceInvoiceNumber,
                        ReferenceDocumentDate = General.ToDateTime(item.ReferenceDocumentDate),
                        ItemID = item.ItemID,
                        Qty = item.Qty,
                        Rate = item.Rate,
                        TaxableAmount = item.Amount,
                        Amount = item.Amount,
                        Remarks = item.Remarks,
                        DepartmentID = item.DepartmentID,
                        LocationID = item.LocationID,
                        InterCompanyID = item.InterCompanyID,
                        EmployeeID = item.EmployeeID,
                        ProjectID = item.ProjectID,
                        PurchaseReturnID = item.PurchaseReturnID,
                        NetAmount = item.NetAmount,
                        CGSTAmt = item.CGSTAmt,
                        IGSTAmt = item.IGSTAmt,
                        SGSTAmt = item.SGSTAmt,
                        GSTPercentage = item.GSTPercentage

                    };
                    ItemList.Add(debitNoteItem);
                }
                var outId = debitnoteBL.Save(DebitNote, ItemList);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "SupplierDebitNote", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Accounts/SupplierDebitNote/Details/5
        public ActionResult Details(int Id)
        {

            SupplierDebitNoteModel DebitNote = debitnoteBL.GetDebitNoteDetail(Id).Select(m => new SupplierDebitNoteModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date, "view"),
                SupplierName = m.SupplierName,
                TotalAmount = m.TotalAmount,
                TaxableAmount = m.TaxableAmount,
                CGSTAmt = m.CGSTAmt,
                SGSTAmt = m.SGSTAmt,
                IGSTAmt = m.IGSTAmt,
                IsDraft = m.IsDraft,
                RoundOff = m.RoundOff
               
            }).First();
            DebitNote.Items = debitnoteBL.GetDebitNoteTransDetail(Id).Select(m => new SupplierDebitNoteItemModel()
            {
                ReferenceInvoiceNumber = m.ReferenceInvoiceNumber,
                ReferenceDocumentDate = General.FormatDate(m.ReferenceDocumentDate, "view"),
                ItemName = m.Item,
                Qty = m.Qty,
                Rate = m.Rate,
                NetAmount = m.Amount,
                Location = m.Location,
                Department = m.Department,
                Employee = m.Employee,
                InterCompany = m.InterCompany,
                Project = m.Project,
                Remarks = m.Remarks,
                Amount = m.TaxableAmount,
                CGSTAmt = m.CGSTAmt,
                SGSTAmt = m.SGSTAmt,
                IGSTAmt = m.IGSTAmt,
                GSTPercentage = m.GSTPercentage,
                PurchaseReturnNo = m.PurchaseReturnNo,

            }).ToList();
            return View(DebitNote);
        }
        public JsonResult getItemForAutoComplete(string Areas, string term = "", string ItemCategoryID = "", string PurchaseCategoryID = "")
        {
            List<ItemBO> _outItems = new List<ItemBO>();
            ItemContract itemBL = new ItemBL();
            var ItemCategoryIDInt = (ItemCategoryID != "") ? Convert.ToInt32(ItemCategoryID) : 0;
            var PurchaseCategoryIDInt = (PurchaseCategoryID != "") ? Convert.ToInt32(PurchaseCategoryID) : 0;

            _outItems = itemBL.GetServiceItems(term, ItemCategoryIDInt, PurchaseCategoryIDInt);
            return Json(_outItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            SupplierDebitNoteModel DebitNote = debitnoteBL.GetDebitNoteDetail(id).Select(m => new SupplierDebitNoteModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date),
                SupplierName = m.SupplierName,
                TotalAmount = m.TotalAmount,
                TaxableAmount = m.TaxableAmount,
                CGSTAmt = m.CGSTAmt,
                SGSTAmt = m.SGSTAmt,
                IGSTAmt = m.IGSTAmt,
                SupplierID = m.SupplierID,
                IsDraft = m.IsDraft,
                IsGSTRegistred = m.IsGSTRegistered,
                StateID = m.StateID,
                RoundOff = m.RoundOff,
                LocationStateID = m.LocationStateID
            }).First();
            if (!DebitNote.IsDraft)
            {
                return RedirectToAction("Index");
            }
            DebitNote.Items = debitnoteBL.GetDebitNoteTransDetail(id).Select(m => new SupplierDebitNoteItemModel()
            {
                ReferenceInvoiceNumber = m.ReferenceInvoiceNumber,
                ReferenceDocumentDate = General.FormatDate(m.ReferenceDocumentDate),
                ItemName = m.Item,
                Qty = m.Qty,
                Rate = m.Rate,
                NetAmount = m.Amount,
                Location = m.Location,
                Department = m.Department,
                Employee = m.Employee,
                InterCompany = m.InterCompany,
                Project = m.Project,
                Remarks = m.Remarks,
                Amount = m.TaxableAmount,
                CGSTAmt = m.CGSTAmt,
                SGSTAmt = m.SGSTAmt,
                IGSTAmt = m.IGSTAmt,
                GSTPercentage = m.GSTPercentage,
                PurchaseReturnNo = m.PurchaseReturnNo,
                ItemID = m.ItemID,
                DepartmentID = m.DepartmentID,
                EmployeeID = m.EmployeeID,
                LocationID = m.LocationID,
                ProjectID = m.ProjectID,
                InterCompanyID = m.InterCompanyID                
            }).ToList();

            DebitNote.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            DebitNote.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");

            DebitNote.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");

            DebitNote.EmployeeList = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            DebitNote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            DebitNote.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
            DebitNote.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("SDNStatus").ToString("MM/dd/yyyy");
            return View(DebitNote);
        }

        public JsonResult GetSupplierDebitNoteList(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string TransDate = Datatable.Columns[2].Search.Value;
                string Supplier = Datatable.Columns[3].Search.Value;
                string ReferenceInvoiceNumber = Datatable.Columns[4].Search.Value;
                string ReferenceDocumentDate = Datatable.Columns[5].Search.Value;
                string TotalAmount = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = debitnoteBL.GetSupplierDebitNoteList(Type, TransNo, TransDate, Supplier, ReferenceInvoiceNumber, ReferenceDocumentDate, TotalAmount, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        public ActionResult Cancel(int id)
        {
            return null;
        }

        [HttpPost]
        public ActionResult Print(int Id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + debitnoteBL.GetPrintTextFile(Id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SupplierDebitNotePrintPdf(int Id)
        {
            return null;
        }
        public ActionResult SaveAsDraft(SupplierDebitNoteModel model)
        {
            return Save(model);
        }
        #endregion
    }
}