using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class CustomerCreditNoteController : Controller
    {

        #region Private members
        private IDropdownContract _dropdown;
        private ICustomerCreditNoteContract creditnoteBL;
        private IDepartmentContract departmentBL;
        private IEmployeeContract employeeBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        private IPeriodClosingContract periodClosingBL;
        private ISupplierCreditNoteContract supplierCreditnoteBL;
        private IAddressContract addressBL;
        #endregion

        #region Constructor
        public CustomerCreditNoteController(IDropdownContract dropdown)
        {
            creditnoteBL = new CustomerCreditNoteBL();
            departmentBL = new DepartmentBL();
            employeeBL = new EmployeeBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
            periodClosingBL = new PeriodClosingBL();
            supplierCreditnoteBL = new SupplierCreditNoteBL();
            addressBL = new AddressBL();
            this._dropdown = dropdown;
        }
        #endregion

        #region Public methods
        public ActionResult Index()
        {
            return View();
        }
        //GET: Accounts/CustomercreditNote/Create
        public ActionResult Create()
        {
            CustomerCreditNoteModel CreditNote = new CustomerCreditNoteModel();

            CreditNote.TransNo = generalBL.GetSerialNo("CustomerCreditNote", "Code");
            CreditNote.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("CCNStatus").ToString("MM/dd/yyyy");
            CreditNote.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            CreditNote.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            CreditNote.Date = General.FormatDate(DateTime.Now);
            CreditNote.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");

            CreditNote.EmployeeList = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            CreditNote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            CreditNote.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");

            return View(CreditNote);
        }

        // POST: Accounts/CreditNote/Create
        [HttpPost]
        public ActionResult Save(CustomerCreditNoteModel model)
        {
            var result = new List<object>();
            try
            {

                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    CustomerCreditNoteBO Temp = creditnoteBL.GetCreditNoteDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                CustomerCreditNoteBO creditNote = new CustomerCreditNoteBO()
                {
                    TransNo = model.TransNo,
                    Date = General.ToDateTime(model.Date),
                    CustomerID = model.CustomerID,
                    IsDraft = model.IsDraft,
                    TotalAmount = model.TotalAmount,
                    CGSTAmt = model.CGSTAmt,
                    IGSTAmt = model.IGSTAmt,
                    SGSTAmt = model.SGSTAmt,
                    RoundOff = model.RoundOff,
                    TaxableAmount = model.TaxableAmount,
                    ID = model.ID,

                };
                List<CustomerCreditNoteTransBO> ItemList = new List<CustomerCreditNoteTransBO>();
                CustomerCreditNoteTransBO creditNoteItem;
                foreach (var item in model.Items)
                {
                    creditNoteItem = new CustomerCreditNoteTransBO()
                    {
                        CreditNoteID = item.CreditNoteID,
                        ReferenceInvoiceNumber = item.ReferenceInvoiceNumber,
                        ReferenceDocumentDate = General.ToDateTime(item.ReferenceDocumentDate),
                        ItemID = item.ItemID,
                        Qty = item.Qty,
                        Rate = item.Rate,
                        TaxableAmount = item.Amount,
                        Amount = item.Amount,
                        NetAmount = item.NetAmount,
                        CGSTAmt = item.CGSTAmt,
                        IGSTAmt = item.IGSTAmt,
                        SGSTAmt = item.SGSTAmt,
                        GSTPercentage = item.GSTPercentage,
                        Remarks = item.Remarks,
                        DepartmentID = item.DepartmentID,
                        LocationID = item.LocationID,
                        InterCompanyID = item.InterCompanyID,
                        EmployeeID = item.EmployeeID,
                        ProjectID = item.ProjectID,
                    };
                    ItemList.Add(creditNoteItem);
                }
                var outId = creditnoteBL.Save(creditNote, ItemList);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "CustomerCreditNote", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Accounts/CustomerCreditNote/Details/5
        public ActionResult Details(int Id)
        {

            CustomerCreditNoteModel CreditNote = creditnoteBL.GetCreditNoteDetails(Id).Select(m => new CustomerCreditNoteModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date, "view"),
                CustomerName = m.CustomerName,
                TotalAmount = m.TotalAmount,
                TaxableAmount = m.TaxableAmount,
                CGSTAmt = m.CGSTAmt,
                SGSTAmt = m.SGSTAmt,
                IGSTAmt = m.IGSTAmt,
                IsDraft = m.IsDraft,
                IsGSTRegistred = m.IsGSTRegistered,
                StateID = m.StateID,
                RoundOff = m.RoundOff,
            }).First();
            CreditNote.Items = creditnoteBL.GetCreditNoteTransDetails(Id).Select(m => new CustomerCreditNoteItemModel()
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
                GSTPercentage = m.GSTPercentage
            }).ToList();
            return View(CreditNote);
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
            CustomerCreditNoteModel creditnote = creditnoteBL.GetCreditNoteDetails(id).Select(m => new CustomerCreditNoteModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date),
                CustomerName = m.CustomerName,
                ReferenceInvoiceNumber = m.ReferenceInvoiceNumber,
                ReferenceDocumentDate = General.FormatDate(m.ReferenceDocumentDate),
                CustomerID = m.CustomerID,
                TotalAmount = m.TotalAmount,
                TaxableAmount = m.TaxableAmount,
                CGSTAmt = m.CGSTAmt,
                SGSTAmt = m.SGSTAmt,
                IGSTAmt = m.IGSTAmt,
                IsDraft = m.IsDraft,
                IsGSTRegistred = m.IsGSTRegistered,
                StateID = m.StateID,
                RoundOff = m.RoundOff,
                LocationStateID=m.LocationStateID
            }).First();
            if (!creditnote.IsDraft)
            {
                return RedirectToAction("Index");
            }
            List<SupplierCreditNoteTransBO> ItemList = new List<SupplierCreditNoteTransBO>();
            creditnote.Items = creditnoteBL.GetCreditNoteTransDetails(id).Select(m => new CustomerCreditNoteItemModel()
            {
                ReferenceInvoiceNumber = m.ReferenceInvoiceNumber,
                ReferenceDocumentDate = General.FormatDate(m.ReferenceDocumentDate),
                ItemName = m.Item,
                ItemID = m.ItemID,
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
                DepartmentID = m.DepartmentID,
                EmployeeID = m.EmployeeID,
                LocationID = m.LocationID,
                ProjectID = m.ProjectID,
                InterCompanyID = m.InterCompanyID
            }).ToList();
            creditnote.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            creditnote.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");

            creditnote.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");

            creditnote.EmployeeList = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            creditnote.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("CDNStatus").ToString("MM/dd/yyyy");

            creditnote.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
            creditnote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            return View(creditnote);
        }

        public ActionResult Cancel(int id)
        {
            return null;
        }
        [HttpPost]
        public ActionResult Print(int Id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + creditnoteBL.GetPrintTextFile(Id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerCreditNotePrintPdf(int Id)
        {
            return null;
        }
        public ActionResult SaveAsDraft(CustomerCreditNoteModel model)
        {
            return Save(model);
        }

        public JsonResult GetCustomerCreditNoteList(DatatableModel Datatable)
        {
            try
            {
                string TransNoNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string CustomerHint = Datatable.Columns[3].Search.Value;
                string InvoiceNoHint = Datatable.Columns[4].Search.Value;
                string DocumentDateHint = Datatable.Columns[5].Search.Value;
                string AmountHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = creditnoteBL.GetCustomerCreditNoteList(Type, TransNoNoHint, TransDateHint, CustomerHint, InvoiceNoHint, DocumentDateHint, AmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}