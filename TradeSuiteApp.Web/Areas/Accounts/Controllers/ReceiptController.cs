using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObject;
using PresentationContractLayer;
using System.Net;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Utils;
using BusinessLayer;
using TradeSuiteApp.Web.Models;
using System.IO;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class ReceiptController : Controller
    {
        private ICustomerContract customerBL;
        private ITreasuryContract treasuryBL;
        private IPaymentVoucher paymentVoucherBL;
        private IReceiptVoucher receiptVoucherBL;
        private IPaymentTypeContract paymentTypeBL;
        private IGeneralContract generalBL;
        private ICurrencyContract currencyBL;
        private IReceivablesContract receivableBL;

        public ReceiptController()
        {
            customerBL = new CustomerBL();
            currencyBL = new CurrencyBL();
            treasuryBL = new TreasuryBL();
            paymentVoucherBL = new PaymentVoucherBL();
            receiptVoucherBL = new ReceiptVoucherBL();
            paymentTypeBL = new PaymentTypeBL();
            generalBL = new GeneralBL();
            receivableBL = new ReceivableBL();
        }

        // GET: Accounts/Receipt
        public ActionResult Index()
        {
            List<ReceiptVoucherModel> recipt = receiptVoucherBL.GetReceiptList().Select(a => new ReceiptVoucherModel()
            {
                ID = a.ID,
                ReceiptNo = a.ReceiptNo,
                ReceiptDate = General.FormatDate(a.ReceiptDate, "view"),
                ReceiptAmount = a.ReceiptAmount,
                CustomerName = a.CustomerName,
                BankName = a.BankName,
                PaymentTypeName = a.PaymentTypeName,
                Remarks = a.Remarks,
                BankReferanceNumber = a.BankReferanceNumber,
                IsDraft = (bool)a.IsDraft,
                Status = a.IsDraft ? "draft" : ""

            }).ToList();

            return View(recipt);

        }

        // GET: Accounts/Receipt/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            ReceiptVoucherBO receiptVoucher = receiptVoucherBL.GetReceiptDetails((int)id);
            ReceiptVoucherModel receipt = new ReceiptVoucherModel()
            {
                ID = receiptVoucher.ID,
                ReceiptNo = receiptVoucher.ReceiptNo,
                ReceiptDate = General.FormatDate(receiptVoucher.ReceiptDate, "view"),
                ReceiptAmount = receiptVoucher.ReceiptAmount,
                CustomerName = receiptVoucher.CustomerName,
                BankName = receiptVoucher.BankName,
                Date = receiptVoucher.Date == null ? "" : General.FormatDate((DateTime)receiptVoucher.Date, "view"),
                PaymentTypeName = receiptVoucher.PaymentTypeName,
                Remarks = receiptVoucher.Remarks,
                BankReferanceNumber = receiptVoucher.BankReferanceNumber,
                CustomerID = receiptVoucher.CustomerID,
                IsDraft = receiptVoucher.IsDraft
            };

            receipt.Item = receiptVoucherBL.GetReceiptTrans((int)id).Select(a => new ReceiptItemModel()
            {
                DocumentNo = a.DocumentNo,
                DocumentType = a.DocumentType,
                ReceivableDate = General.FormatDate(a.ReceivableDate, "view"),
                Amount = a.Amount,
                Balance = a.Balance,
                AmountToBeMatched = a.AmountToBeMatched,
                ReceivableID = a.ReceivableID,
                VoucherID = a.VoucherID,
                Status = a.Status,
                PendingDays = a.PendingDays
            }).ToList();

            return View(receipt);


        }

        [HttpPost]
        public JsonResult GetInvoiceForReceiptVoucher(int CustomerID)
        {
            TimeSpan Difference;

            List<ReceivablesBO> Receivables = receivableBL.GetReceivables(CustomerID);
            Receivables.Select(a =>
            {
                a.TransDateStr = a.TransDate == null ? "" : General.FormatDate(a.TransDate);
                a.DueDateStr = a.DueDate == null ? "" : General.FormatDate(a.DueDate);
                DateTime current = DateTime.Now;
                Difference = current.Subtract(a.TransDate);
                a.PendingDays = Convert.ToString(Difference.Days);

                return a;
            }).ToList();
            return Json(Receivables, JsonRequestBehavior.AllowGet);
        }

        // GET: Accounts/Payment/Create
        public ActionResult Create()
        {
            ReceiptVoucherModel receipt = new ReceiptVoucherModel();

            receipt.ReceiptNo = generalBL.GetSerialNo("ReceiptVoucher", "Code");

            receipt.ReceiptDate = General.FormatDate(DateTime.Now);
            receipt.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            receipt.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            receipt.BankList = new SelectList(treasuryBL.GetBank("Receipt", ""), "ID", "BankName");
            // receipt.ReceiverBankList = new SelectList(treasuryBL.GetReceiverBankList(),"ID", "BankName");
             receipt.ReceiverBankList = new SelectList(treasuryBL.GetReceiverBankList(), "ReceiverBankID", "BankName");
          



            return View(receipt);
        }

        // POST: Accounts/Payment/Create
        [HttpPost]
        public ActionResult Save(ReceiptVoucherModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    ReceiptVoucherBO Temp = receiptVoucherBL.GetReceiptDetails(model.ID);
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                // TODO: Add insert logic here
                ReceiptVoucherBO receipt = new ReceiptVoucherBO();
                receipt.ReceiptNo = model.ReceiptNo;
                receipt.ReceiptDate = General.ToDateTime(model.ReceiptDate);
                receipt.CustomerID = model.CustomerID;
                receipt.ReceiptAmount = model.ReceiptAmount;
                receipt.PaymentTypeID = model.PaymentTypeID;
                receipt.BankInstrumentNumber = model.BankInstrumentNumber;
                receipt.checqueDate = model.ChecqueDate;
                receipt.ReceiverBankID= model.ReceiverBankID;

                receipt.BankID = model.BankID;
                if (model.Date != "" && model.Date != null)
                {
                    receipt.Date = General.ToDateTime(model.Date);
                }
                receipt.BankReferanceNumber = model.BankReferanceNumber;
                receipt.Remarks = model.Remarks;
                receipt.IsDraft = model.IsDraft;
                receipt.ID = model.ID;
                var ItemList = new List<ReceiptItemBO>();
                ReceiptItemBO receiptItemBO;
                foreach (var itm in model.Item)
                {
                    receiptItemBO = new ReceiptItemBO();
                    receiptItemBO.ReceivableID = itm.ReceivableID;
                    receiptItemBO.CreditNoteID = itm.CreditNoteID;
                    receiptItemBO.DebitNoteID = itm.DebitNoteID;
                    receiptItemBO.AdvanceID = itm.AdvanceID;
                    receiptItemBO.DocumentType = itm.DocumentType;
                    receiptItemBO.DocumentNo = itm.DocumentNo;
                    receiptItemBO.Amount = itm.Amount;
                    receiptItemBO.Balance = itm.Balance;
                    receiptItemBO.AmountToBeMatched = itm.AmountToBeMatched;
                    receiptItemBO.Status = itm.Status;
                    receiptItemBO.AdvanceReceivedAmount = itm.AdvanceReceivedAmount;
                    receiptItemBO.PendingDays = itm.PendingDays;
                    receiptItemBO.SalesReturnID = itm.SalesReturnID;
                    receiptItemBO.CustomerReturnVoucherID = itm.CustomerReturnVoucherID;
                    if (itm.ReceivableDate != null && itm.ReceivableDate != "")
                    {
                        receiptItemBO.ReceivableDate = General.ToDateTime(itm.ReceivableDate);
                    }
                    ItemList.Add(receiptItemBO);
                }
                List<ReceiptSettlementBO> Settlements = new List<ReceiptSettlementBO>();
                ReceiptSettlementBO SettlementBO;
                if (model.Settlements != null)
                {
                    foreach (var item in model.Settlements)
                    {
                        SettlementBO = new ReceiptSettlementBO();
                        SettlementBO.AdvanceID = item.AdvanceID;
                        SettlementBO.CreditNoteID = item.CreditNoteID;
                        SettlementBO.DebitNoteID = item.DebitNoteID;
                        SettlementBO.ReceivableID = item.ReceivableID;
                        SettlementBO.SalesReturnID = item.SalesReturnID;

                        SettlementBO.SettlementFrom = item.SettlementFrom;
                        SettlementBO.DocumentNo = item.DocumentNo;
                        SettlementBO.DocumentType = item.DocumentType;

                        SettlementBO.Amount = item.Amount;
                        SettlementBO.SettlementAmount = item.SettlementAmount;

                        Settlements.Add(SettlementBO);
                    }
                }

                var outId = receiptVoucherBL.Save(receipt, ItemList, Settlements);
                return Json(new { Status = "success", message = "Invoice Already Settled" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "PaymentVoucher", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Accounts/Payment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            ReceiptVoucherBO receiptVoucher = receiptVoucherBL.GetReceiptDetails((int)id);
            ReceiptVoucherModel receipt = new ReceiptVoucherModel()
            {
                ID = receiptVoucher.ID,
                ReceiptNo = receiptVoucher.ReceiptNo,
                ReceiptDate = General.FormatDate(receiptVoucher.ReceiptDate),
                ReceiptAmount = receiptVoucher.ReceiptAmount,
                CustomerName = receiptVoucher.CustomerName,
                BankName = receiptVoucher.BankName,
                BankID = receiptVoucher.BankID,
                Date = receiptVoucher.Date == null ? "" : General.FormatDate((DateTime)receiptVoucher.Date),
                PaymentTypeID = receiptVoucher.PaymentTypeID,
                PaymentTypeName = receiptVoucher.PaymentTypeName,
                Remarks = receiptVoucher.Remarks,
                BankReferanceNumber = receiptVoucher.BankReferanceNumber,
                CustomerID = receiptVoucher.CustomerID,
                IsDraft = receiptVoucher.IsDraft,
                IsBlockedForChequeReceipt = receiptVoucher.IsBlockedForChequeReceipt,


            };
            if (!receipt.IsDraft)
            {
                return RedirectToAction("Index");
            }
            receipt.Item = receiptVoucherBL.GetReceiptTransForEdit((int)id).Select(a => new ReceiptItemModel()
            {
                DocumentNo = a.DocumentNo,
                DocumentType = a.DocumentType,
                ReceivableDate = General.FormatDate(a.ReceivableDate),
                Amount = a.Amount,
                Balance = a.Balance,
                AmountToBeMatched = a.AmountToBeMatched,
                ReceivableID = a.ReceivableID,
                VoucherID = a.VoucherID,
                Status = a.Status,
                CreditNoteID = a.CreditNoteID,
                DebitNoteID = a.DebitNoteID,
                AdvanceID = a.AdvanceID,
                ClassType = a.ClassType,
                PendingDays = a.PendingDays,
                SalesReturnID = a.SalesReturnID,
                CustomerReturnVoucherID = a.CustomerReturnVoucherID
            }).ToList();
            receipt.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            receipt.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            string Mode = "Bank";
            string Module = "Receipt";
            if (receipt.PaymentTypeName == "CASH")
            {
                Mode = "Cash";
            }
            receipt.BankList = new SelectList(treasuryBL.GetBank(Module, Mode), "ID", "BankName");
            receipt.ReceiverBankList = new SelectList(treasuryBL.GetReceiverBankList(), "ReceiverBankID", "BankName");
            return View(receipt);

        }

        public JsonResult GetReceiptVoucherList(DatatableModel Datatable)
        {
            try
            {
                string ReceiptNoHint = Datatable.Columns[1].Search.Value;
                string InvoiceDateHint = Datatable.Columns[2].Search.Value;
                string CustomerHint = Datatable.Columns[3].Search.Value;
                string ReceiptAmountHint = Datatable.Columns[4].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = receiptVoucherBL.GetReceiptVoucherList(Type, ReceiptNoHint, InvoiceDateHint, CustomerHint, ReceiptAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(ReceiptVoucherModel model)
        {
            return Save(model);
        }

        [HttpPost]
        public ActionResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + receiptVoucherBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReceiptVoucherPrintPdf(int Id)
        {
            return null;
        }

        //changed for version3

        public ActionResult CreateV3()
        {
            ReceiptVoucherModel receipt = new ReceiptVoucherModel();
            receipt.ReceiptNo = generalBL.GetSerialNo("ReceiptVoucher", "Code");

            receipt.ReceiptDate = General.FormatDate(DateTime.Now);
            receipt.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");
            int? currencyId = currencyBL.GetCurrencyByLocationID(GeneralBO.LocationID)?.Id; // Assuming the property holding the currency ID is named 'Id'
            receipt.CurrencyID = currencyId ?? 0; 

            receipt.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            receipt.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            receipt.BankList = new SelectList(treasuryBL.GetBank("Receipt", ""), "ID", "BankName");
            receipt.Item = new List<ReceiptItemModel>();
            receipt.ReceiverBankList = new SelectList(treasuryBL.GetReceiverBankList(), "ReceiverBankID", "ReceiverBankName");

            return View(receipt);
        }
        [HttpPost]
        public ActionResult SaveV3(ReceiptVoucherModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    ReceiptVoucherBO Temp = receiptVoucherBL.GetReceiptDetailsV3(model.ID);
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                // TODO: Add insert logic here
                ReceiptVoucherBO receipt = new ReceiptVoucherBO();
                receipt.ReceiptNo = model.ReceiptNo;
                receipt.ReceiptDate = General.ToDateTime(model.ReceiptDate);
                receipt.CustomerID = model.CustomerID;
                receipt.CurrencyID = model.CurrencyID;
                receipt.ReceiptAmount = model.ReceiptAmount;
                receipt.PaymentTypeID = model.PaymentTypeID;
                receipt.BankID = model.BankID;
                receipt.AccountHeadID = model.AccountHeadID;
                receipt.AccountID = model.AccountID;
                receipt.BankInstrumentNumber = model.BankInstrumentNumber;
                receipt.checqueDate = model.ChecqueDate;
                receipt.ReceiverBankID = model.ReceiverBankID;

                if (model.Date != "" && model.Date != null)
                {
                    receipt.Date = General.ToDateTime(model.Date);
                }
                receipt.BankReferanceNumber = model.BankReferanceNumber;
                receipt.Remarks = model.Remarks;
                receipt.IsDraft = model.IsDraft;
                receipt.ID = model.ID;
                var ItemList = new List<ReceiptItemBO>();
                ReceiptItemBO receiptItemBO;
                foreach (var itm in model.Item)
                {
                    receiptItemBO = new ReceiptItemBO();
                    receiptItemBO.ReceivableID = itm.ReceivableID;
                    receiptItemBO.CreditNoteID = itm.CreditNoteID;
                    receiptItemBO.DebitNoteID = itm.DebitNoteID;
                    receiptItemBO.AdvanceID = itm.AdvanceID;
                    receiptItemBO.DocumentType = itm.DocumentType;
                    receiptItemBO.DocumentNo = itm.DocumentNo;
                    receiptItemBO.Amount = itm.Amount;
                    receiptItemBO.Balance = itm.Balance;
                    receiptItemBO.AmountToBeMatched = itm.AmountToBeMatched;
                    receiptItemBO.Status = itm.Status;
                    receiptItemBO.AdvanceReceivedAmount = itm.AdvanceReceivedAmount;
                    receiptItemBO.PendingDays = itm.PendingDays;
                    receiptItemBO.SalesReturnID = itm.SalesReturnID;
                    receiptItemBO.CustomerReturnVoucherID = itm.CustomerReturnVoucherID;
                    if (itm.ReceivableDate != null && itm.ReceivableDate != "")
                    {
                        receiptItemBO.ReceivableDate = General.ToDateTime(itm.ReceivableDate);
                    }
                    ItemList.Add(receiptItemBO);
                }
                List<ReceiptSettlementBO> Settlements = new List<ReceiptSettlementBO>();
                ReceiptSettlementBO SettlementBO;
                if (model.Settlements != null)
                {
                    foreach (var item in model.Settlements)
                    {
                        SettlementBO = new ReceiptSettlementBO();
                        SettlementBO.AdvanceID = item.AdvanceID;
                        SettlementBO.CreditNoteID = item.CreditNoteID;
                        SettlementBO.DebitNoteID = item.DebitNoteID;
                        SettlementBO.ReceivableID = item.ReceivableID;
                        SettlementBO.SalesReturnID = item.SalesReturnID;
                        SettlementBO.SettlementFrom = item.SettlementFrom;
                        SettlementBO.DocumentNo = item.DocumentNo;
                        SettlementBO.DocumentType = item.DocumentType;

                        SettlementBO.Amount = item.Amount;
                        SettlementBO.SettlementAmount = item.SettlementAmount;

                        Settlements.Add(SettlementBO);
                    }
                }

                var outId = receiptVoucherBL.SaveV3(receipt, ItemList, Settlements);
                return Json(new { Status = "success", message = "Invoice Already Settled" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "PaymentVoucher", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraftV3(ReceiptVoucherModel model)
        {
            return SaveV3(model);
        }

        [HttpPost]
        public JsonResult GetInvoiceForReceiptVoucherV3(int AccountHeadID)
        {
            TimeSpan Difference;

            List<ReceivablesBO> Receivables = receivableBL.GetReceivablesV3(AccountHeadID);
            Receivables.Select(a =>
            {
                a.TransDateStr = a.TransDate == null ? "" : General.FormatDate(a.TransDate);
                a.DueDateStr = a.DueDate == null ? "" : General.FormatDate(a.DueDate);
                DateTime current = DateTime.Now;
                Difference = current.Subtract(a.TransDate);
                a.PendingDays = Convert.ToString(Difference.Days);

                return a;
            }).ToList();
            return Json(Receivables, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReceiptVoucherListV3(DatatableModel Datatable)
        {
            try
            {
                string ReceiptNoHint = Datatable.Columns[1].Search.Value;
                string InvoiceDateHint = Datatable.Columns[2].Search.Value;
                string AccountHeadHint = Datatable.Columns[3].Search.Value;
                string ReceiptAmountHint = Datatable.Columns[4].Search.Value;
                string ReconciledDateHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = receiptVoucherBL.GetReceiptVoucherListV3(Type, ReceiptNoHint, InvoiceDateHint, AccountHeadHint, ReceiptAmountHint, ReconciledDateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult IndexV3()
        {
            return View();

        }

        public ActionResult DetailsV3(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            ReceiptVoucherBO receiptVoucher = receiptVoucherBL.GetReceiptDetailsV3((int)id);
            ReceiptVoucherModel receipt = new ReceiptVoucherModel()
            {
                ID = receiptVoucher.ID,
                ReceiptNo = receiptVoucher.ReceiptNo,
                ReceiptDate = General.FormatDate(receiptVoucher.ReceiptDate, "view"),
                ReceiptAmount = receiptVoucher.ReceiptAmount,
                AccountHead = receiptVoucher.AccountHead,
                BankName = receiptVoucher.BankName,
                Currency=receiptVoucher.Currency,
                Date = receiptVoucher.Date == null ? "" : General.FormatDate((DateTime)receiptVoucher.Date, "view"),
                PaymentTypeName = receiptVoucher.PaymentTypeName,
                Remarks = receiptVoucher.Remarks,
                BankReferanceNumber = receiptVoucher.BankReferanceNumber,
                AccountHeadID = receiptVoucher.AccountHeadID,
                IsDraft = receiptVoucher.IsDraft,
                ReconciledDate = (General.FormatDate(receiptVoucher.ReconciledDate, "view") == "01-Jan-1900" ? "" : General.FormatDate(receiptVoucher.ReconciledDate, "view")),
                ChecqueDate=receiptVoucher.checqueDate,
                BankInstrumentNumber = receiptVoucher.BankInstrumentNumber,
                ReceiverBankName = receiptVoucher.ReceiverBankName,

            };

            receipt.Item = receiptVoucherBL.GetReceiptTransV3((int)id).Select(a => new ReceiptItemModel()
            {
                DocumentNo = a.DocumentNo,
                DocumentType = a.DocumentType,
                ReceivableDate = General.FormatDate(a.ReceivableDate, "view"),
                Amount = a.Amount,
                Balance = a.Balance,
                AmountToBeMatched = a.AmountToBeMatched,
                ReceivableID = a.ReceivableID,
                VoucherID = a.VoucherID,
                Status = a.Status,
                PendingDays = a.PendingDays
            }).ToList();

            return View(receipt);


        }

        public ActionResult EditV3(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            ReceiptVoucherBO receiptVoucher = receiptVoucherBL.GetReceiptDetailsV3((int)id);
            ReceiptVoucherModel receipt = new ReceiptVoucherModel()
            {
                ID = receiptVoucher.ID,
                ReceiptNo = receiptVoucher.ReceiptNo,
                ReceiptDate = General.FormatDate(receiptVoucher.ReceiptDate),
                ReceiptAmount = receiptVoucher.ReceiptAmount,
                AccountHead = receiptVoucher.AccountHead,
                BankName = receiptVoucher.BankName,
                BankID = receiptVoucher.BankID,
                Date = receiptVoucher.Date == null ? "" : General.FormatDate((DateTime)receiptVoucher.Date),
                PaymentTypeID = receiptVoucher.PaymentTypeID,
                PaymentTypeName = receiptVoucher.PaymentTypeName,
                Remarks = receiptVoucher.Remarks,
                BankReferanceNumber = receiptVoucher.BankReferanceNumber,
                AccountHeadID = receiptVoucher.AccountHeadID,
                IsDraft = receiptVoucher.IsDraft,
                IsBlockedForChequeReceipt = receiptVoucher.IsBlockedForChequeReceipt,
                Currency = receiptVoucher.Currency,
                CurrencyID = receiptVoucher.CurrencyID,
                ChecqueDate = receiptVoucher.checqueDate,
                BankInstrumentNumber = receiptVoucher.BankInstrumentNumber,
                ReceiverBankName = receiptVoucher.ReceiverBankName,
                ReceiverBankID = receiptVoucher.ReceiverBankID,


            };
            if (!receipt.IsDraft)
            {
                return RedirectToAction("Index");
            }
            receipt.Item = receiptVoucherBL.GetReceiptTransForEditV3((int)id).Select(a => new ReceiptItemModel()
            {
                DocumentNo = a.DocumentNo,
                DocumentType = a.DocumentType,
                ReceivableDate = General.FormatDate(a.ReceivableDate),
                Amount = a.Amount,
                Balance = a.Balance,
                AmountToBeMatched = a.AmountToBeMatched,
                ReceivableID = a.ReceivableID,
                VoucherID = a.VoucherID,
                Status = a.Status,
                CreditNoteID = a.CreditNoteID,
                DebitNoteID = a.DebitNoteID,
                AdvanceID = a.AdvanceID,
                ClassType = a.ClassType,
                PendingDays = a.PendingDays,
                SalesReturnID = a.SalesReturnID,
                CustomerReturnVoucherID = a.CustomerReturnVoucherID
            }).ToList();
            receipt.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            receipt.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            receipt.ReceiverBankList = new SelectList(treasuryBL.GetReceiverBankList(), "ReceiverBankID", "ReceiverBankName");
            string Mode = "Bank";
            string Module = "Receipt";
            if (receipt.PaymentTypeName == "CASH")
            {
                Mode = "Cash";
            }
            receipt.BankList = new SelectList(treasuryBL.GetBank(Module, Mode), "ID", "BankName");
            receipt.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");
            return View(receipt);

        }

        public ActionResult SaveReconciledDate(int ID, DateTime ReconciledDate, string BankReferanceNumber, string Remarks)
        {
            var result = new List<object>();
            try
            {
                int out_id = receiptVoucherBL.SaveReconciledDate(ID, ReconciledDate, BankReferanceNumber, Remarks);
                return Json(new { Status = "Success", message = "Invoice Already Settled" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "Receipt", "SaveReconciledDate", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}