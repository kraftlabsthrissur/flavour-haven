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
    public class CustomerDebitNoteController : Controller
    {
        #region Private members
        private IDropdownContract _dropdown;
        private ICustomerDebitNoteContract debitnoteBL;
        private IDepartmentContract departmentBL;
        private IEmployeeContract employeeBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        private IPeriodClosingContract periodClosingBL;
        private ISupplierCreditNoteContract creditnoteBL;
        private IAddressContract addressBL;
        #endregion

        #region Constructor
        public CustomerDebitNoteController(IDropdownContract dropdown)
        {
            debitnoteBL = new CustomerDebitNoteBL();
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
        public ActionResult Index()
        {
            return View();
        }
        //GET: Accounts/CustomerDebitNote/Create
        public ActionResult Create()
        {
            CustomerDebitNoteModel DebitNote = new CustomerDebitNoteModel();

            DebitNote.TransNo = generalBL.GetSerialNo("CustomerDebitNote", "Code");

            DebitNote.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("CDNStatus").ToString("MM/dd/yyyy");

            DebitNote.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            DebitNote.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");

            DebitNote.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            DebitNote.Date = General.FormatDate(DateTime.Now);
            DebitNote.EmployeeList = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            DebitNote.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
            DebitNote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            return View(DebitNote);
        }

        // POST: Account/SupplierDebitNote/Create
        [HttpPost]
        public ActionResult Save(CustomerDebitNoteModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    CustomerDebitNoteBO Temp = debitnoteBL.GetDebitNoteDetail(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                CustomerDebitNoteBO DebitNote = new CustomerDebitNoteBO()
                {
                    TransNo = model.TransNo,
                    Date = General.ToDateTime(model.Date),
                    CustomerID = model.CustomerID,
                    TotalAmount = model.TotalAmount,
                    CGSTAmt = model.CGSTAmt,
                    IGSTAmt = model.IGSTAmt,
                    SGSTAmt = model.SGSTAmt,
                    TaxableAmount = model.TaxableAmount,
                    IsDraft = model.IsDraft,
                    ID = model.ID,
                    RoundOff = model.RoundOff
                };
                List<CustomerDebitNoteTransBO> ItemList = new List<CustomerDebitNoteTransBO>();
                CustomerDebitNoteTransBO creditNoteItem;
                foreach (var item in model.Items)
                {
                    creditNoteItem = new CustomerDebitNoteTransBO()
                    {
                        CreditNoteID = item.CreditNoteID,
                        ReferenceInvoiceNumber = item.ReferenceInvoiceNumber,
                        ReferenceDocumentDate = General.ToDateTime(item.ReferenceDocumentDate),
                        ItemID = item.ItemID,
                        Qty = item.Qty,
                        Rate = item.Rate,
                        Amount = item.Amount,
                        TaxableAmount = item.Amount,
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
                        ProjectID = item.ProjectID

                    };
                    ItemList.Add(creditNoteItem);
                }
                var outId = debitnoteBL.Save(DebitNote, ItemList);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "CustomerDebitNote", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
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

        // GET: Accounts/CustomerDebitNote/Details/5
        public ActionResult Details(int Id)
        {

            CustomerDebitNoteModel DebitNote = debitnoteBL.GetDebitNoteDetail(Id).Select(m => new CustomerDebitNoteModel()
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
                RoundOff = m.RoundOff
            }).First();
            DebitNote.Items = debitnoteBL.GetDebitNoteDetailTrans(Id).Select(m => new CustomerDebitNoteItemModel()
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
            return View(DebitNote);
        }

        public ActionResult Cancel(int id)
        {
            return null;
        }

        public ActionResult Edit(int id)
        {

            CustomerDebitNoteModel DebitNote = debitnoteBL.GetDebitNoteDetail(id).Select(m => new CustomerDebitNoteModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date),
                CustomerName = m.CustomerName,
                TotalAmount = m.TotalAmount,
                TaxableAmount = m.TaxableAmount,
                CGSTAmt = m.CGSTAmt,
                SGSTAmt = m.SGSTAmt,
                IGSTAmt = m.IGSTAmt,
                IsDraft = m.IsDraft,
                CustomerID = m.CustomerID,
                StateID = m.StateID,
                IsGSTRegistred = m.IsGSTRegistred,
                RoundOff = m.RoundOff,
                LocationStateID=m.LocationStateID
            }).First();
            if (!DebitNote.IsDraft)
            {
                return RedirectToAction("Index");
            }
            DebitNote.Items = debitnoteBL.GetDebitNoteDetailTrans(id).Select(m => new CustomerDebitNoteItemModel()
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
                ItemID = m.ItemID,
                LocationID = m.LocationID,
                DepartmentID = m.DepartmentID,
                EmployeeID = m.EmployeeID,
                InterCompanyID = m.InterCompanyID,
                ProjectID = m.ProjectID,

            }).ToList();

            DebitNote.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            DebitNote.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");

            DebitNote.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            DebitNote.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("CDNStatus").ToString("MM/dd/yyyy");

            DebitNote.EmployeeList = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            DebitNote.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");

            DebitNote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            return View(DebitNote);
        }

        [HttpPost]
        public ActionResult Print(int Id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + debitnoteBL.GetPrintTextFile(Id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerDebitNotePrintPdf(int Id)
        {
            return null;
        }

        public ActionResult SaveAsDraft(CustomerDebitNoteModel model)
        {
            return Save(model);
        }

        public JsonResult GetCustomerDebitNoteList(DatatableModel Datatable)
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

                DatatableResultBO resultBO = debitnoteBL.GetCustomerDebitNoteList(Type, TransNoNoHint, TransDateHint, CustomerHint, InvoiceNoHint, DocumentDateHint, AmountHint, SortField, SortOrder, Offset, Limit);
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