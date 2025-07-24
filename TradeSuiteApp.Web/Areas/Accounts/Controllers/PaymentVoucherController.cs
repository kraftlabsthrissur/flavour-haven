using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessObject;
using PresentationContractLayer;
using System.Net;
using BusinessLayer;
using TradeSuiteApp.Web.Utils;
using System.IO;
using TradeSuiteApp.Web.Models;
using DataAccessLayer;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using TradeSuiteApp.Web.Areas.Sales.Models;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Microsoft.Owin.BuilderProperties;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class PaymentVoucherController : Controller
    {
        #region Private members
        private IDropdownContract _dropdown;
        private IPayment _iPayment;
        private ITreasuryContract treasuryBL;
        private IPaymentTypeContract paymentTypeBL;
        private IPaymentVoucher paymentVoucherBL;
        private IGeneralContract generalBL;
        private IConfigurationContract configBL;
        private ICurrencyContract currencyBL;
        private ILocationContract locationBL;
        #endregion

        #region Constructor
        public PaymentVoucherController(IDropdownContract IDropdown, IPayment iPayment)
        {
            this._dropdown = IDropdown;
            this._iPayment = iPayment;
            currencyBL = new CurrencyBL();
            locationBL = new LocationBL();
            paymentVoucherBL = new PaymentVoucherBL();
            treasuryBL = new TreasuryBL();
            paymentTypeBL = new PaymentTypeBL();
            generalBL = new GeneralBL();
            configBL = new ConfigurationBL();
        }
        #endregion

        // GET: Accounts/Payment
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Voucher View
        /// </summary>
        /// <returns></returns>
        /// 
        public JsonResult GetPaymentVoucherList(DatatableModel Datatable)
        {
            try
            {
                string VoucherNumber = Datatable.Columns[1].Search.Value;
                string VoucherDate = Datatable.Columns[2].Search.Value;
                string SupplierName = Datatable.Columns[3].Search.Value;
                string Amount = Datatable.Columns[4].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = paymentVoucherBL.GetPaymentVoucherList(Type, VoucherNumber, VoucherDate, SupplierName, Amount, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult Create()
        {
            PaymentVoucherModel paymentVoucherModel = new PaymentVoucherModel();
            var listBanks = _dropdown.GetBankDetails();
            //paymentVoucherModel.BankDetails = listBanks.Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.AccountNo + ", " + x.BankName)).ToList();
            paymentVoucherModel.BankDetails = listBanks.Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.BankName)).ToList();
            paymentVoucherModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", paymentVoucherModel.PaymentTypeID);
            paymentVoucherModel.BankList = treasuryBL.GetBank("Payment", "").Select(
          a => new TreasuryModel()
          {
              BankName = a.BankName,
              ID = a.ID,
              CreditBalance = a.CreditBalance
          }).ToList();
            paymentVoucherModel.ReceiverBankList = new SelectList(treasuryBL.GetReceiverBankList(), "ReceiverBankID", "BankName");
            paymentVoucherModel.VoucherNumber = generalBL.GetSerialNo("PaymentVoucher", "Cash");
            paymentVoucherModel.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                paymentVoucherModel.CurrencyID = currency.CurrencyID;
            }
            paymentVoucherModel.VoucherDate = General.FormatDate(DateTime.Now);
            paymentVoucherModel.CashPaymentLimit = configBL.GetCashPayementLimit();
            return View(paymentVoucherModel);
        }

        /// <summary>
        /// Get the UnSettledPurchase Invoices by Supplier ID
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public PartialViewResult GetUnSettledPurchaseInvoices(int supplierID)
        {
            var unsettledPurchaseInvoiceList = _iPayment.GetPayableDetailsForPaymentVoucher(supplierID);
            return PartialView("~/Areas/Accounts/Views/PaymentVoucher/_unSettledPurchaseInvoice.cshtml", unsettledPurchaseInvoiceList);
        }

        /// <summary>
        /// Save the Payment
        /// </summary>
        /// <param name="paymentVoucherSaveModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(PaymentVoucherSaveModel paymentVoucher)
        {
            var result = new List<object>();
            try
            {
                if (paymentVoucher.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PaymentVoucherBO Temp = paymentVoucherBL.GetPaymentVoucherDetail(paymentVoucher.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var paymentVoucherBO = paymentVoucher.MapToBo();
                var paymentID = 0;
                if (paymentVoucherBO.ID <= 0)
                {
                    paymentID = _iPayment.SavePaymentVoucher(paymentVoucherBO);

                }
                else
                {
                    paymentID = _iPayment.UpdatePaymentVoucher(paymentVoucherBO);

                }
                var statusCode = paymentID > 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                if (paymentID >= 0)
                {
                    return Json(new { StatusCode = statusCode, Message = "Payment voucher saved successfully", PaymentID = paymentID, Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Invalid paynow" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    

                }
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "PaymentVoucher", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Accounts/Payment/Details/5
        public ActionResult Details(int? id)
        {

            PaymentVoucherModel paymentVoucherModel = paymentVoucherBL.GetPaymentVoucherDetail((int)id).Select(k => new PaymentVoucherModel()
            {
                ID = k.ID,
                VoucherNumber = k.VoucherNo,
                VoucherDate = General.FormatDate(k.VoucherDate, "view"),
                SupplierName = k.SupplierName,
                BankName = k.BankName,
                ReferenceNumber = k.ReferenceNumber,
                Remark = k.Remark,
                PaymentTypeName = k.PaymentTypeName,
                IsDraft = k.IsDraft,
                VoucherAmt=k.VoucherAmount
            }
            ).First();
            paymentVoucherModel.List = paymentVoucherBL.GetPaymentVoucherTrans((int)id).Select(k => new PaymentVoucherList()
            {
                ID = k.ID,
                InvoiceNo = k.InvoiceNo,
                Date = General.FormatDate(k.Date, "view"),
                PaidAmount = (decimal)k.PaidAmount,
                OrginalAmount = (decimal)k.OrginalAmount,
                Balance = (decimal)k.Balance,
                DocumentType = k.DocumentType,
                Narration=k.Narration

            }).ToList();
            return View(paymentVoucherModel);
        }


        public ActionResult Edit(int? id)
        {

            PaymentVoucherModel paymentVoucherModel = paymentVoucherBL.GetPaymentVoucherDetail((int)id).Select(k => new PaymentVoucherModel()
            {
                ID = k.ID,
                VoucherNumber = k.VoucherNo,
                VoucherDate = General.FormatDate(k.VoucherDate),
                SupplierName = k.SupplierName,
                BankName = k.BankName,
                ReferenceNumber = k.ReferenceNumber,
                Remark = k.Remark,
                PaymentTypeName = k.PaymentTypeName,
                IsDraft = k.IsDraft,
                PaymentTypeID = k.PaymentTypeID,
                BankID = k.BankID,
                VoucherAmt=k.VoucherAmount
            }
            ).First();
            if(!paymentVoucherModel.IsDraft)
            {
                return RedirectToAction("Index");
            }
            paymentVoucherModel.List = paymentVoucherBL.GetPaymentVoucherTransForEdit((int)id).Select(a => new PaymentVoucherList()
            {
                ID = a.ID,
                DocumentType = a.DocumentType,
                DocumentNo = a.DocumentNo,
                PayableID = a.PayableID,
                AdvanceID = a.AdvanceID,
                DebitNoteID = a.DebitNoteID,
                CreatedDateStr = General.FormatDate(a.CreatedDate),
                DocumentAmount = a.DocumentAmount,
                AmountToBePayed = a.AmountToBePayed,
                DueDateStr = General.FormatDate(a.DueDate),
                CreditNoteID = a.CreditNoteID,
                IRGID = a.IRGID,
                SupplierName = a.SupplierName,
                PaidAmount = a.PaidAmount,
                PaymentReturnVoucherTransID=a.PaymentReturnVoucherTransID,
                Narration=a.Narration


            }).ToList();
            paymentVoucherModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", paymentVoucherModel.PaymentTypeID);
            paymentVoucherModel.BankList = treasuryBL.GetBank("Payment", paymentVoucherModel.PaymentTypeName == "CASH" ? "Cash" : "Bank").Select(
                a => new TreasuryModel()
                {
                    BankName = a.BankName,
                    ID = a.ID,
                    CreditBalance = a.CreditBalance
                }).ToList();

            paymentVoucherModel.CashPaymentLimit = configBL.GetCashPayementLimit();



            return View(paymentVoucherModel);
        }

        public JsonResult GetDocumentAutoComplete(string Term = "")
        {
            List<PaymentVoucherList> voucherList = new List<PaymentVoucherList>();
            voucherList = paymentVoucherBL.GetDocumentAutoComplete(Term).Select(a => new PaymentVoucherList()
            {
                ID = a.AdvanceID,
                InvoiceNo = a.DocumentNo,
            }).ToList();

            return Json(voucherList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + paymentVoucherBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSerialNo(string mode)
        {
            var SerialNo = "";
            if (mode == "Cash")
            {
                SerialNo = generalBL.GetSerialNo("PaymentVoucher", "Cash");
            }
            else
            {
                SerialNo = generalBL.GetSerialNo("PaymentVoucher", "Other");
            }
            return Json(new { Status = "success", data = SerialNo }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveAsDraft(PaymentVoucherSaveModel paymentVoucher)
        {
            return Save(paymentVoucher);
        }

        public JsonResult PaymentVoucherPrintPdf(int Id)
        {
            return null;
        }


        //For Version3 Account Changes

        public ActionResult CreateV3()
        {
            PaymentVoucherModel paymentVoucherModel = new PaymentVoucherModel();
            var listBanks = _dropdown.GetBankDetails();
            //paymentVoucherModel.BankDetails = listBanks.Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.AccountNo + ", " + x.BankName)).ToList();
            paymentVoucherModel.BankDetails = listBanks.Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.BankName)).ToList();
            paymentVoucherModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", paymentVoucherModel.PaymentTypeID);
            paymentVoucherModel.BankList = treasuryBL.GetBank("Payment", "").Select(
          a => new TreasuryModel()
          {
              BankName = a.BankName,
              ID = a.ID,
              CreditBalance = a.CreditBalance
          }).ToList();
   


            paymentVoucherModel.ReceiverBankList = new SelectList(treasuryBL.GetReceiverBankList(), "ReceiverBankID", "ReceiverBankName");
            paymentVoucherModel.VoucherNumber = generalBL.GetSerialNo("PaymentVoucher", "Cash");
            paymentVoucherModel.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Code");

            int? currencyId = currencyBL.GetCurrencyByLocationID(GeneralBO.LocationID)?.Id;
            paymentVoucherModel.CurrencyID = currencyId ?? 0;
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                paymentVoucherModel.CurrencyID = currency.CurrencyID;
                paymentVoucherModel.LocalCurrencyID = currency.CurrencyID;
                paymentVoucherModel.LocalCurrencyCode = currency.CurrencyCode;


            }





            paymentVoucherModel.VoucherDate = General.FormatDate(DateTime.Now);
            paymentVoucherModel.CashPaymentLimit = configBL.GetCashPayementLimit();
            return View(paymentVoucherModel);
        }

        public PartialViewResult GetUnSettledPurchaseInvoicesV3(int AccountHeadID)
        {
            var unsettledPurchaseInvoiceList = _iPayment.GetPayableDetailsForPaymentVoucherV3(AccountHeadID);
            return PartialView("~/Areas/Accounts/Views/PaymentVoucher/_unSettledPurchaseInvoice.cshtml", unsettledPurchaseInvoiceList);
        }

        [HttpPost]
        public ActionResult SaveV3(PaymentVoucherSaveModel paymentVoucher)
        {
            var result = new List<object>();
            try
            {
                if (paymentVoucher.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PaymentVoucherBO Temp = paymentVoucherBL.GetPaymentVoucherDetailV3(paymentVoucher.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var paymentVoucherBO = paymentVoucher.MapToBo();
                var paymentID = 0;
                if (paymentVoucherBO.ID <= 0)
                {
                    paymentID = _iPayment.SavePaymentVoucherV3(paymentVoucherBO);

                }
                else
                {
                    paymentID = _iPayment.UpdatePaymentVoucherV3(paymentVoucherBO);

                }
                var statusCode = paymentID > 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                if (paymentID >= 0)
                {
                    return Json(new { StatusCode = statusCode, Message = "Payment voucher saved successfully", PaymentID = paymentID, Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Invalid paynow" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "PaymentVoucher", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditV3(int? id)
        {

          
            PaymentVoucherModel paymentVoucherModel = paymentVoucherBL.GetPaymentVoucherDetailV3((int)id).Select(k => new PaymentVoucherModel()
            {
              
                 ID = k.ID,
                VoucherNumber = k.VoucherNo,
                VoucherDate = General.FormatDate(k.VoucherDate),
                AccountHead = k.AccountHead,
                AccountHeadID=k.AccountHeadID,
                BankName = k.BankName,
                ReferenceNumber = k.ReferenceNumber,
                Remark = k.Remark,
                PaymentTypeName = k.PaymentTypeName,
                IsDraft = k.IsDraft,
                PaymentTypeID = k.PaymentTypeID,
                BankID = k.BankID,
                VoucherAmt = k.VoucherAmount,
                Currency=k.Currency,
                CurrencyID = k.CurrencyID,
                CurrencyCode = k.supplierCurrencycode
            }
            ).First();
           
            if (!paymentVoucherModel.IsDraft)
            {
                return RedirectToAction("IndexV3");
            }
            paymentVoucherModel.List = paymentVoucherBL.GetPaymentVoucherTransForEditV3((int)id).Select(a => new PaymentVoucherList()
            {
                ID = a.ID,
                DocumentType = a.DocumentType,
                DocumentNo = a.DocumentNo,
                PayableID = a.PayableID,
                AdvanceID = a.AdvanceID,
                DebitNoteID = a.DebitNoteID,
                CreatedDateStr = General.FormatDate(a.CreatedDate),
                DocumentAmount = a.DocumentAmount,
                AmountToBePayed = a.AmountToBePayed,
                DueDateStr = General.FormatDate(a.DueDate),
                CreditNoteID = a.CreditNoteID,
                IRGID = a.IRGID,
                SupplierName = a.SupplierName,
                PaidAmount = a.PaidAmount,
                PaymentReturnVoucherTransID = a.PaymentReturnVoucherTransID,
                Narration = a.Narration


            }).ToList();
            paymentVoucherModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", paymentVoucherModel.PaymentTypeID);
            paymentVoucherModel.BankList = treasuryBL.GetBank("Payment", paymentVoucherModel.PaymentTypeName == "CASH" ? "Cash" : "Bank").Select(
                a => new TreasuryModel()
                {
                    BankName = a.BankName,
                    ID = a.ID,
                    CreditBalance = a.CreditBalance
                }).ToList();

            paymentVoucherModel.CashPaymentLimit = configBL.GetCashPayementLimit();
            paymentVoucherModel.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Code");
            paymentVoucherModel.ReceiverBankList = new SelectList(treasuryBL.GetReceiverBankList(), "ReceiverBankID", "ReceiverBankName");




            return View(paymentVoucherModel);
        }

        public ActionResult DetailsV3(int? id)
        {

            PaymentVoucherModel paymentVoucherModel = paymentVoucherBL.GetPaymentVoucherDetailV3((int)id).Select(k => new PaymentVoucherModel()
            {
                ID = k.ID,
                VoucherNumber = k.VoucherNo,
                VoucherDate = General.FormatDate(k.VoucherDate, "view"),
                AccountHead = k.AccountHead,
                AccountHeadID=k.AccountHeadID,
                BankName = k.BankName,
                ReferenceNumber = k.ReferenceNumber,
                Remark = k.Remark,
                PaymentTypeName = k.PaymentTypeName,
                IsDraft = k.IsDraft,
                VoucherAmt = k.VoucherAmount,
                ReconciledDate = (General.FormatDate(k.ReconciledDate, "view")== "01-Jan-1900" ? "": General.FormatDate(k.ReconciledDate, "view")),
                Currency = k.Currency,
                ReceiverBankName = k.ReceiverBankName,
                ReceiverBankID = k.ReceiverBankID,
                Bankcharges = k.Bankcharges,
                BankInstrumentNumber = k.BankInstrumentNumber,
                ChecqueDate=k.checqueDate,
                CurrencyCode = k.CurrencyCode,
                CurrencyID = k.CurrencyID,
                LocalCurrencyID = k.LocalCurrencyID,
                LocalCurrencyCode = k.LocalCurrencyCode,
                CurrencyExchangeRate = k.CurrencyExchangeRate,
                LocalVoucherAmt = k.LocalVoucherAmt,
            }
            ).First();
            paymentVoucherModel.List = paymentVoucherBL.GetPaymentVoucherTransV3((int)id).Select(k => new PaymentVoucherList()
            {
                ID = k.ID,
                InvoiceNo = k.InvoiceNo,
                Date = General.FormatDate(k.Date, "view"),
                PaidAmount = (decimal)k.PaidAmount,
                OrginalAmount = (decimal)k.OrginalAmount,
                Balance = (decimal)k.Balance,
                DocumentType = k.DocumentType,
                Narration = k.Narration

            }).ToList();
            paymentVoucherModel.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Code");
            return View(paymentVoucherModel);
        }

        public ActionResult IndexV3()
        {
            return View();
        }

        public JsonResult GetPaymentVoucherListV3(DatatableModel Datatable)
        {
            try
            {
                string VoucherNumber = Datatable.Columns[1].Search.Value;
                string VoucherDate = Datatable.Columns[2].Search.Value;
                string AccountHead = Datatable.Columns[3].Search.Value;
                string Amount = Datatable.Columns[4].Search.Value;
                string ReconciledDate = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = paymentVoucherBL.GetPaymentVoucherListV3(Type, VoucherNumber, VoucherDate, AccountHead, Amount, ReconciledDate, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraftV3(PaymentVoucherSaveModel paymentVoucher)
        {
            return SaveV3(paymentVoucher);
        }

        public ActionResult SaveReconciledDate(int ID, DateTime ReconciledDate, string BankReferanceNumber, string Remarks)
        {
            var result = new List<object>();
            try
            {
                int out_id = paymentVoucherBL.SaveReconciledDate(ID, ReconciledDate, BankReferanceNumber, Remarks);
                return Json(new { Status = "Success", message = "Invoice Already Settled" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "Payment", "SaveReconciledDate", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
