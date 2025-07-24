using BusinessLayer;
using BusinessObject;
using Microsoft.AspNet.Identity;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Utils;
using System.Collections;
using static TradeSuiteApp.Web.Areas.Accounts.Models.BankExpensesModel;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class BankExpensesController : Controller
    {

        #region Private members


        private ITreasuryContract treasuryBL;
        private IBankExpensesContract bankexpensesBL;
        private IPaymentModeContract paymentModeBL;
        private IGeneralContract generalBL;

        #endregion       

        #region Constructor

        public BankExpensesController()
        {
            generalBL = new GeneralBL();
            treasuryBL = new TreasuryBL();
            bankexpensesBL = new BankExpensesBL();
            paymentModeBL = new PaymentModeBL();
        }

        #endregion

        #region Public methods

        // GET: Accounts/BankExpenses
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft" };
            return View();
        }


        // GET: Accounts/BankExpenses/Create
        public ActionResult Create()
        {
            BankExpensesModel bankexpense = new BankExpensesModel();

            bankexpense.TransNo = generalBL.GetSerialNo("BankExpenses", "Code");

            bankexpense.ItemList = new SelectList(bankexpensesBL.GetItemName(), "ID", "ItemName");
            bankexpense.Date = General.FormatDate(DateTime.Now);
            bankexpense.BankList = new SelectList(treasuryBL.GetBank(), "ID", "BankName");
            bankexpense.ModeOfPaymentList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            bankexpense.TransactionDate = General.FormatDate(DateTime.Now);
            return View(bankexpense);
        }

        // POST: Accounts/BankExpenses/Create  
        [HttpPost]
        public ActionResult Save(BankExpensesModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    BankExpensesBO Temp = bankexpensesBL.GetBankExpensesDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                BankExpensesBO bankexpenses = new BankExpensesBO()
                {
                    ID = model.ID,
                    TransNo = model.TransNo,
                    Date = General.ToDateTime(model.Date),
                    BankID = model.BankID,
                    TotalAmount = model.TotalAmount,
                    IsDraft = model.IsDraft

                };
                List<BankExpensesTransBO> ItemList = new List<BankExpensesTransBO>();
                BankExpensesTransBO BankExpencesItem;
                foreach (var item in model.Items)
                {
                    BankExpencesItem = new BankExpensesTransBO()
                    {
                        TransactionNumber = item.TransactionNumber,
                        TransactionDate = General.ToDateTime(item.TransactionDate),
                        ModeOfPaymentID = item.ModeOfPaymentID,
                        Amount = item.Amount,
                        ItemID = item.ItemID,
                        Remarks = item.Remarks,
                        ReferenceNo = item.ReferenceNo
                    };
                    ItemList.Add(BankExpencesItem);
                }
                if (model.ID == 0)
                {
                    var outId = bankexpensesBL.Save(bankexpenses, ItemList);
                }
                else
                {
                    var outId = bankexpensesBL.Update(bankexpenses, ItemList);
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "BankExpenses", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(BankExpensesModel model)
        {
            return Save(model);
        }

        // GET: Accounts/BankExpenses/Deatils        
        public ActionResult Details(int Id)
        {
            BankExpensesModel bankExpenses = bankexpensesBL.GetBankExpensesDetails(Id).Select(m => new BankExpensesModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date, "view"),
                Bank = m.Bank,
                ModeOfPayment = m.ModeOfPayment,
                TotalAmount = m.TotalAmount,
                IsDraft = m.IsDraft
            }).First();
            bankExpenses.Items = bankexpensesBL.GetBankExpensesTransDetails(Id).Select(m => new BankExpensesItemModel()
            {
                TransactionNumber = m.TransactionNumber,
                TransactionDate = General.FormatDate(m.TransactionDate, "view"),
                ModeOfPayment = m.ModeOfPayment,
                Amount = m.Amount,
                ItemName = m.ItemName,
                Remarks = m.Remarks,
                ReferenceNo = m.ReferenceNo
            }).ToList();
            return View(bankExpenses);
        }
        public JsonResult GetAccountAutoComplete(string AccountName, string AccountCode)
        {
            List<BankExpensesModel> itemList = new List<BankExpensesModel>();
            itemList = bankexpensesBL.GetAccountAutoComplete(AccountName, AccountCode).Select(a => new BankExpensesModel()
            {
                AccountHeadID = a.AccountHeadID,
                AccountCode = a.AccountCode,
                AccountName = a.AccountName
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        // GET: Accounts/BankExpenses/Deatils        
        public ActionResult Edit(int Id)
        {
            BankExpensesModel bankExpenses = bankexpensesBL.GetBankExpensesDetails(Id).Select(m => new BankExpensesModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date, "view"),
                BankID = m.BankID,
                Bank = m.Bank,
                ModeOfPayment = m.ModeOfPayment,
                TotalAmount = m.TotalAmount,
                IsDraft = m.IsDraft
            }).First();
            if(!bankExpenses.IsDraft)
            {
                return RedirectToAction("Index");
            }
            bankExpenses.Items = bankexpensesBL.GetBankExpensesTransDetails(Id).Select(m => new BankExpensesItemModel()
            {
                TransactionNumber = m.TransactionNumber,
                TransactionDate = General.FormatDate(m.TransactionDate),
                ModeOfPaymentID = m.ModeOfPaymentID,
                ModeOfPayment = m.ModeOfPayment,
                Amount = m.Amount,
                ItemID = m.ItemID,
                ItemName = m.ItemName,
                Remarks = m.Remarks,
                ReferenceNo = m.ReferenceNo
            }).ToList();
            bankExpenses.ItemList = new SelectList(bankexpensesBL.GetItemName(), "ID", "ItemName");
            bankExpenses.Date = General.FormatDate(DateTime.Now);
            bankExpenses.BankList = new SelectList(treasuryBL.GetBank(), "ID", "BankName");
            bankExpenses.ModeOfPaymentList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            return View(bankExpenses);
        }

        public JsonResult BankExpensesPrintPdf(int Id)
        {
            return null;
        }

        public JsonResult GetFinancialExpensesList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string ItemNameHint = Datatable.Columns[3].Search.Value;
                string PaymentHint = Datatable.Columns[4].Search.Value;
                string AmountHint = Datatable.Columns[5].Search.Value;


                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = bankexpensesBL.GetFinancialExpensesList(Type, TransNoHint, TransDateHint, ItemNameHint, PaymentHint, AmountHint, SortField, SortOrder, Offset, Limit);
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