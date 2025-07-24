using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using DataAccessLayer.DBContext;
using Microsoft.AspNet.Identity;
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
    public class JournalController : Controller
    {
        #region Private members
        private IDropdownContract _dropdown;
        private IJournalContract journalBL;
        private IDepartmentContract departmentBL;
        private IEmployeeContract employeeBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        private ICurrencyContract currencyBL;
        private IPeriodClosingContract periodClosingBL;
        private ICounterSalesContract counterSalesBL;
        #endregion

        #region Constructor 
        public JournalController(IDropdownContract dropdown)
        {
            this._dropdown = dropdown;
            journalBL = new JournalBL();
            currencyBL = new CurrencyBL();
            departmentBL = new DepartmentBL();
            employeeBL = new EmployeeBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
            periodClosingBL = new PeriodClosingBL();
            counterSalesBL = new CounterSalesBL();
        }
        #endregion

        #region Public methods
        // GET: Accounts/Journal
        public ActionResult Index()
        {

            List<JournalModel> journal = journalBL.JournalList().Select(a => new JournalModel()
            {
                ID = a.ID,
                VoucherNo = a.VoucherNo,
                Date = General.FormatDate(a.Date, "view"),
                CreditAccountName = a.CreditAccountName,
                TotalCreditAmount = a.TotalCreditAmount,
                DebitAccountName = a.DebitAccountName,
                TotalDebitAmount = a.TotalDebitAmount,
                IsDraft=a.IsDraft,
                Status = a.IsDraft ? "draft" : ""
            }).ToList();
            ViewBag.Statuses = new List<string>() { "draft" };
            return View(journal);
        }

        // GET: Accounts/Journal/Create
        [HttpGet]
        public ActionResult Create()
        {
            JournalModel journal = new JournalModel();

            journal.VoucherNo = generalBL.GetSerialNo("Journal", "Code");
            journal.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("JournalStatus").ToString("MM/dd/yyyy");
            journal.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            journal.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            journal.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            journal.EmployeeList = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            journal.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
            journal.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");
            int? currencyId = currencyBL.GetCurrencyByLocationID(GeneralBO.LocationID)?.Id; // Assuming the property holding the currency ID is named 'Id'
            journal.CurrencyID = currencyId ?? 0;
            journal.CreditCurrencyID = currencyId ?? 0;
            journal.DebitCurrencyID = currencyId ?? 0;
            journal.Date = General.FormatDate(DateTime.Now);
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID((int)currencyId);
            if (classdata != null)
            {
                journal.normalclass = classdata.normalclass;
                journal.largeclass = classdata.largeclass;
                journal.DecimalPlaces = classdata.DecimalPlaces;
            }
            journal.Items = new List<JournalItemModel>();
            return View(journal);
        }

        // POST: Accounts/Journal/Create
        [HttpPost]
        public ActionResult Save(JournalModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    JournalBO Temp = journalBL.GetJournalDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                JournalBO Journal = new JournalBO()
                {
                    ID=model.ID,
                    VoucherNo = model.VoucherNo,
                    Date = General.ToDateTime(model.Date),
                    TotalCreditAmount = model.TotalCreditAmount,
                    TotalDebitAmount = model.TotalDebitAmount,
                    CurrencyID=model.CurrencyID,
                    Currency = model.Currency,
                    IsDraft = model.IsDraft

                };
                List<JournalTransBO> ItemList = new List<JournalTransBO>();
                JournalTransBO journalItem;
                foreach (var item in model.Items)
                {
                    journalItem = new JournalTransBO()
                    {
                        CreditAccountHeadID = item.CreditAccountHeadID,
                        CreditAccountCode = item.CreditAccountCode,
                        CreditAmount = item.CreditAmount,
                        DebitAccountHeadID = item.DebitAccountHeadID,
                        DebitAccountCode = item.DebitAccountCode,
                        DebitAmount = item.DebitAmount,
                        DepartmentID = item.DepartmentID,
                        JournalLocationID = item.JournalLocationID,
                        InterCompanyID = item.InterCompanyID,
                        EmployeeID = item.EmployeeID,
                        ProjectID = item.ProjectID,
                        Remarks = item.Remarks,
                        LocalCurrencyID = Journal.CurrencyID,
                        LocalCurrency = Journal.Currency,
                        DebitCurrencyID = item.DebitCurrencyID,
                        DebitCurrency = item.DebitCurrency,
                        CreditCurrencyID = item.CreditCurrencyID,
                        CreditCurrency = item.CreditCurrency,
                        DebitExchangeRate = item.DebitExchangeRate,
                        CreditExchangeRate = item.CreditExchangeRate,
                        LocalDebitAmount = item.LocalDebitAmount,
                        LocalCreditAmount = item.LocalCreditAmount
                    };
                    ItemList.Add(journalItem);
                }

                if (model.ID == 0)
                {
                    var outId = journalBL.Save(Journal, ItemList);
                }
                else
                {
                    var outId = journalBL.Update(Journal, ItemList);
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "Journal", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveAsDraft(JournalModel model)
        {
            return Save(model);
        }

        //Post: Accounts/Journal/Create
        public ActionResult Details(int Id)
        {
            JournalModel Journal = journalBL.GetJournalDetails(Id).Select(m => new JournalModel()
            {
                ID = m.ID,
                Date = General.FormatDate(m.Date, "view"),
                VoucherNo = m.VoucherNo,
                TotalCreditAmount = m.TotalCreditAmount,
                TotalDebitAmount = m.TotalDebitAmount,
                Currency = m.Currency,
                IsDraft = m.IsDraft
                 
            }).First();
            Journal.Items = journalBL.GetJournalTransDetails(Id).Select(m => new JournalItemModel()
            {
                CreditAccountName = m.CreditAccountName,
                CreditAccountCode = m.CreditAccountCode,
                CreditAmount = m.CreditAmount,
                DebitAccountName = m.DebitAccountName,
                DebitAccountCode = m.DebitAccountCode,
                DebitAmount = m.DebitAmount,
                Department = m.Department,
                Location = m.Location,
                InterCompany = m.InterCompany,
                Employee = m.Employee,
                Project = m.Project,
                Remarks = m.Remarks,
                LocalCurrencyID = Journal.CurrencyID,
                LocalCurrency = Journal.Currency,
                DebitCurrencyID = m.DebitCurrencyID,
                DebitCurrency = m.DebitCurrency,
                CreditCurrencyID = m.CreditCurrencyID,
                CreditCurrency = m.CreditCurrency,
                DebitExchangeRate = m.DebitExchangeRate,
                CreditExchangeRate = m.CreditExchangeRate,
                LocalDebitAmount = m.LocalDebitAmount,
                LocalCreditAmount = m.LocalCreditAmount
            }).ToList();
            return View(Journal);
        }        
        public ActionResult Edit(int Id)
        {
            JournalModel Journal = journalBL.GetJournalDetails(Id).Select(m => new JournalModel()
            {
                ID = m.ID,
                Date = General.FormatDate(m.Date),
                VoucherNo = m.VoucherNo,
                TotalCreditAmount = m.TotalCreditAmount,
                TotalDebitAmount=m.TotalDebitAmount,
                Currency = m.Currency,
                CurrencyID = m.CurrencyID,
                IsDraft = m.IsDraft               
            }).First();
            if(!Journal.IsDraft)
            {
                return RedirectToAction("Index");
            }
            Journal.Items = journalBL.GetJournalTransDetails(Id).Select(m => new JournalItemModel()
            {
                CreditAccountHeadID = m.CreditAccountHeadID,
                CreditAccountName = m.CreditAccountName,
                CreditAccountCode = m.CreditAccountCode,
                CreditAmount = m.CreditAmount,
                DebitAccountHeadID = m.DebitAccountHeadID,
                DebitAccountName = m.DebitAccountName   ,
                DebitAccountCode = m.DebitAccountCode,
                DebitAmount = m.DebitAmount,
                DepartmentID = m.DepartmentID,
                Department = m.Department,
                JournalLocationID = m.JournalLocationID,
                Location = m.Location,
                InterCompanyID = m.InterCompanyID,
                InterCompany = m.InterCompany,
                EmployeeID = m.EmployeeID,
                Employee = m.Employee,
                ProjectID = m.ProjectID,
                Project = m.Project,
                Remarks = m.Remarks,
                LocalCurrencyID = Journal.CurrencyID,
                LocalCurrency = Journal.Currency,
                DebitCurrencyID = m.DebitCurrencyID,
                DebitCurrency = m.DebitCurrency,
                CreditCurrencyID = m.CreditCurrencyID,
                CreditCurrency = m.CreditCurrency,
                DebitExchangeRate = m.DebitExchangeRate,
                CreditExchangeRate = m.CreditExchangeRate,
                LocalDebitAmount = m.LocalDebitAmount,
                LocalCreditAmount = m.LocalCreditAmount
            }).ToList();             
            Journal.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            Journal.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            Journal.InterCompanyList = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            Journal.EmployeeList = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            Journal.ProjectList = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
            Journal.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");
            Journal.FirstOpenDate = periodClosingBL.GetFirstOpenMonth("JournalStatus").ToString("MM/dd/yyyy");
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(Journal.CurrencyID);
            if (classdata != null)
            {
                Journal.normalclass = classdata.normalclass;
                Journal.largeclass = classdata.largeclass;
                Journal.DecimalPlaces = classdata.DecimalPlaces;
            }
            return View(Journal);

        }

        public JsonResult GetCreditAccountAutoComplete(string CreditAccountName, string CreditAccountCode)
        {
            List<JournalModel> itemList = new List<JournalModel>();
            itemList = journalBL.GetCreditAccountAutoComplete(CreditAccountName, CreditAccountCode).Select(a => new JournalModel()
            {
                CreditAccountHeadID = a.CreditAccountHeadID,
                CreditAccountCode = a.CreditAccountCode,
                CreditAccountName = a.CreditAccountName
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDebitAccountAutoComplete(string DebitAccountName, string DebitAccountCode)
        {
            List<JournalModel> itemList = new List<JournalModel>();
            itemList = journalBL.GetDebitAccountAutoComplete(DebitAccountName, DebitAccountCode).Select(a => new JournalModel()
            {
                DebitAccountHeadID = a.DebitAccountHeadID,
                DebitAccountCode = a.DebitAccountCode,
                DebitAccountName = a.DebitAccountName
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cancel(int id)
        {
            return null;
        }

        public JsonResult GetJournalList(DatatableModel Datatable)
        {
            try
            {
                string VoucherNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string DebitAccountNameHint = Datatable.Columns[3].Search.Value;
                string TotalDebitAmountHint = Datatable.Columns[4].Search.Value;
                string CreditAccountNameHint = Datatable.Columns[5].Search.Value;
                string TotalCreditAmountHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = journalBL.GetJournalList(Type, VoucherNoHint, TransDateHint, DebitAccountNameHint, TotalDebitAmountHint, CreditAccountNameHint, TotalCreditAmountHint, SortField, SortOrder, Offset, Limit);
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