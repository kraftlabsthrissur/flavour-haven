using BusinessLayer;
using BusinessObject;
using Microsoft.AspNet.Identity;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class SupplierCreditNoteController : Controller
    {
        #region Private members
        private IDropdownContract _dropdown;
        private ISupplierCreditNoteContract creditnoteBL;
        private IDepartmentContract departmentBL;
        private IEmployeeContract employeeBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        private IPeriodClosingContract periodClosingBL;
        private IAddressContract addressBL;
        #endregion

        #region Constructor
        public SupplierCreditNoteController(IDropdownContract dropdown)
        {
            creditnoteBL = new SupplierCreditNoteBL();
            departmentBL = new DepartmentBL();
            employeeBL = new EmployeeBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
            periodClosingBL = new PeriodClosingBL();
            addressBL = new AddressBL();
            this._dropdown = dropdown;
        }
        #endregion

        #region Public methods
        public ActionResult Index()
        {
            return View();
        }
        //GET: Accounts/SupplierCreditNote/Create
        public ActionResult Create()
        {
            SupplierCreditNoteModel CreditNote = new SupplierCreditNoteModel();
            CreditNote.TransNo = generalBL.GetSerialNo("SupplierCreditNote", "Code");
            CreditNote.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("SCNStatus").ToString("MM/dd/yyyy");
            CreditNote.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            CreditNote.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");

            CreditNote.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            CreditNote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            CreditNote.EmployeeList = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            CreditNote.Date = General.FormatDate(DateTime.Now);
            CreditNote.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");


            return View(CreditNote);
        }
        // GET: Accounts/CustomerCreditNote/Details/5
        public ActionResult Details(int Id)
        {
            SupplierCreditNoteModel CreditNote = creditnoteBL.GetCreditNoteDetail(Id).Select(m => new SupplierCreditNoteModel()
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
            CreditNote.Items = creditnoteBL.GetCreditNoteDetailTrans(Id).Select(m => new SupplierCreditNoteItemModel()
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
                LocationID = m.LocationID,
                DepartmentID = m.DepartmentID,
                EmployeeID = m.EmployeeID,
                InterCompanyID = m.InterCompanyID,
                ProjectID = m.ProjectID

            }).ToList();
            return View(CreditNote);
        }
        [HttpPost]
        public ActionResult Save(SupplierCreditNoteModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    SupplierCreditNoteBO Temp = creditnoteBL.GetCreditNoteDetail(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                SupplierCreditNoteBO CreditNote = new SupplierCreditNoteBO()
                {
                    ID = model.ID,
                    TransNo = model.TransNo,
                    Date = General.ToDateTime(model.Date),
                    SupplierID = model.SupplierID,
                    TotalAmount = model.TotalAmount,
                    CGSTAmt = model.CGSTAmt,
                    IGSTAmt = model.IGSTAmt,
                    SGSTAmt = model.SGSTAmt,
                    TaxableAmount = model.TaxableAmount,
                    IsDraft = model.IsDraft,
                    RoundOff = model.RoundOff
                };
                List<SupplierCreditNoteTransBO> ItemList = new List<SupplierCreditNoteTransBO>();
                SupplierCreditNoteTransBO creditNoteItem;
                foreach (var item in model.Items)
                {
                    creditNoteItem = new SupplierCreditNoteTransBO()
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
                        NetAmount = item.NetAmount,
                        CGSTAmt = item.CGSTAmt,
                        IGSTAmt = item.IGSTAmt,
                        SGSTAmt = item.SGSTAmt,
                        GSTPercentage = item.GSTPercentage

                    };
                    ItemList.Add(creditNoteItem);
                }
                var outId = creditnoteBL.Save(CreditNote, ItemList);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "SupplierCreditNote", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(SupplierCreditNoteModel model)
        {
            return Save(model);
        }

        public ActionResult Edit(int Id)
        {
            SupplierCreditNoteModel CreditNote = creditnoteBL.GetCreditNoteDetail(Id).Select(m => new SupplierCreditNoteModel()
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
                StateID = m.StateID,
                IsGSTRegistred = m.IsGSTRegistred,
                SupplierID = m.SupplierID,
                IsDraft = m.IsDraft,
                RoundOff = m.RoundOff,
                LocationStateID=m.LocationStateID

            }).First();
            if (!CreditNote.IsDraft)
            {
                return RedirectToAction("Index");
            }
            CreditNote.Items = creditnoteBL.GetCreditNoteDetailTrans(Id).Select(m => new SupplierCreditNoteItemModel()
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
                LocationID = m.LocationID,
                DepartmentID = m.DepartmentID,
                EmployeeID = m.EmployeeID,
                InterCompanyID = m.InterCompanyID,
                ProjectID = m.ProjectID,
                ItemID = m.ItemID

            }).ToList();
            CreditNote.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            CreditNote.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            CreditNote.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
            CreditNote.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            CreditNote.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("SCNStatus").ToString("MM/dd/yyyy");
            CreditNote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            CreditNote.EmployeeList = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            return View(CreditNote);
        }

        public JsonResult GetSupplierCreditNoteList(DatatableModel Datatable)
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

                DatatableResultBO resultBO = creditnoteBL.GetSupplierCreditNoteList(Type, TransNo, TransDate, Supplier, ReferenceInvoiceNumber, ReferenceDocumentDate, TotalAmount, SortField, SortOrder, Offset, Limit);
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
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + creditnoteBL.GetPrintTextFile(Id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SupplierCreditNotePrintPdf(int Id)
        {
            return null;
        }
        #endregion
    }

}
