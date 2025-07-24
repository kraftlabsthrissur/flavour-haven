using BusinessObject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{


    public class PaymentVoucherModel
    {
        public string VoucherNumber { get; set; }
        public string VoucherDate { get; set; }
        public List<KeyValuePair<string, string>> BankDetails { get; set; }
        public string BankDetailJsonStr
        {
            get
            {
                return BankDetails != null ?
                    Newtonsoft.Json.JsonConvert.SerializeObject(BankDetails)
                    : string.Empty;
            }
        }
       
        public int ID { get; set; }
        public string SupplierName { get; set; }
        public decimal VoucherAmt { get; set; }
        public String Remark { get; set; }
        public String Currency { get; set; }
        public string BankName { get; set; }
        public string ReferenceNumber { get; set; }

        public int CurrencyID { get; set; }
        public List<PaymentVoucherList> List { get; set; }
        public SelectList PaymentTypeList { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public int BankID { get; set; }
        public List<TreasuryModel> BankList { get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string GSTNo { get; set; }
        public string NetAmountInWords { get; set; }
        public bool IsDraft { get; set; }

        public SelectList CurrencyLists { get; set; }
        public int CashPaymentLimit { get; set; }

        public string AccountHead { get; set; }
        public int AccountHeadID { get; set; }
        public string ReconciledDate { get; set; }
        public string ChecqueDate { get; set; }
        public SelectList ReceiverBankList { get; set; }
        public string BankInstrumentNumber { get; set; }
        public int ReceiverBankID { get; set; }
         public decimal Bankcharges { get; set; }
        public string ReceiverBankName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public string LocalCurrencyCode { get; set; }
        public int LocalCurrencyID { get; set; }
        public decimal LocalVoucherAmt { get; set; }
    }
    public class CurrencyLists
    {
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }


    public class PaymentVoucherList
    {
        public int ID { get; set; }
        public string InvoiceNo { get; set; }
        public string Date { get; set; }
        public decimal OrginalAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal PaidAmount { get; set; }
        public string DocumentType { get; set; }

        public int PayableID { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string DateStr { get; set; }
        public string DocumentNo { get; set; }
        public decimal Amount { get; set; }
        public string SupplierName { get; set; }
        public decimal DocumentAmount { get; set; }
        public decimal AmountToBePaid { get; set; }
        public decimal PayNow { get; set; }
        public decimal OriginalAmount { get; set; }
        public int AdvanceID { get; set; }
        public int DebitNoteID { get; set; }
        public int CreditNoteID { get; set; }
        public int IRGID { get; set; }
        public int PaymentReturnVoucherTransID { get; set; }
        public string CreatedDateStr { get; set; }
        public decimal AmountToBePayed { get; set; }
        public DateTime CreatedDate
        {
            get
            {
                return General.ToDateTime(CreatedDateStr);

            }
        }
        public string DueDateStr { get; set; }
        public string Narration { get; set; }
        public DateTime DueDate
        {
            get
            {
                return General.ToDateTime(DueDateStr);
                //return !string.IsNullOrEmpty(DueDateStr) ?
                //    DateTime.ParseExact(DueDateStr, "dd-mm-yyyy", CultureInfo.InvariantCulture) :
                //    new DateTime();
            }
        }
    }
    //
    public class PaymentVoucherSaveModel
    {
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDateStr { get; set; }
        public DateTime VoucherDate
        {
            get
            {
                return General.ToDateTime(VoucherDateStr);
              
            }
        }
        public int SupplierID { get; set; }
        public int CurrencyID { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string SaveType { get; set; }
        public string Description { get; set; }
        public int PaymentTypeID { get; set; }
        public bool IsDraft { get; set; }

        public string AccountHead { get; set; }
        public int AccountHeadID { get; set; }
        public List<UnProcessedItemModel> UnProcessedItems { get; set; }
        public string ReceiverBankName { get; set; }
        public string BankInstrumentNumber { get; set; }
        public string checqueDate { get; set; }
        public int ReceiverBankID { get; set; }
        public Decimal Bankcharges { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public string LocalCurrencyCode { get; set; }
        public int LocalCurrencyID { get; set; }
        public decimal LocalVoucherAmt { get; set; }
    }

    public class UnProcessedItemModel
    {
        public int PayableID { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string DateStr { get; set; }
        public DateTime Date
        {
            get
            {
                return General.ToDateTime(DateStr);
               
            }
        }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountToBePaid { get; set; }
        public decimal PayNow { get; set; }
        public decimal OriginalAmount { get; set; }
        public int AdvanceID { get; set; }
        public int DebitNoteID { get; set; }
        public int CreditNoteID { get; set; }
        public int IRGID { get; set; }
        public int PaymentReturnVoucherTransID { get; set; }
        public string CreatedDateStr { get; set; }
        public DateTime CreatedDate
        {
            get
            {
                return General.ToDateTime(CreatedDateStr);
               
            }
        }
        public string DueDateStr { get; set; }
        public string Narration { get; set; }
        public DateTime DueDate
        {
            get
            {
                return General.ToDateTime(DueDateStr);
                //return !string.IsNullOrEmpty(DueDateStr) ?
                //    DateTime.ParseExact(DueDateStr, "dd-mm-yyyy", CultureInfo.InvariantCulture) :
                //    new DateTime();
            }
        }

    }

    public static partial class Mapper
    {
        public static PaymentVoucherBO MapToBo(this PaymentVoucherSaveModel purchaseInvoiceSaveViewModel)
        {
            return new PaymentVoucherBO()
            {
                ID= purchaseInvoiceSaveViewModel.ID,
                VoucherNo = purchaseInvoiceSaveViewModel.VoucherNo,
                VoucherDate = purchaseInvoiceSaveViewModel.VoucherDate,
                AccountNumber = purchaseInvoiceSaveViewModel.AccountNumber,
                BankName = purchaseInvoiceSaveViewModel.BankName,
                Description = purchaseInvoiceSaveViewModel.Description,
                PaymentMode = purchaseInvoiceSaveViewModel.PaymentMode,
                ReferenceNumber = purchaseInvoiceSaveViewModel.ReferenceNumber,
                SaveType = purchaseInvoiceSaveViewModel.SaveType,
                SupplierID = purchaseInvoiceSaveViewModel.SupplierID,
                PaymentTypeID = purchaseInvoiceSaveViewModel.PaymentTypeID,
                IsDraft=purchaseInvoiceSaveViewModel.IsDraft,
                AccountHeadID= purchaseInvoiceSaveViewModel.AccountHeadID,
                CurrencyID=purchaseInvoiceSaveViewModel.CurrencyID,
                ReceiverBankName= purchaseInvoiceSaveViewModel.ReceiverBankName,
                ReceiverBankID= purchaseInvoiceSaveViewModel.ReceiverBankID,
                Bankcharges=purchaseInvoiceSaveViewModel.Bankcharges,
                BankInstrumentNumber= purchaseInvoiceSaveViewModel.BankInstrumentNumber,
                checqueDate=purchaseInvoiceSaveViewModel.checqueDate,
                CurrencyCode = purchaseInvoiceSaveViewModel.CurrencyCode,
                LocalCurrencyID = purchaseInvoiceSaveViewModel.LocalCurrencyID,
                LocalCurrencyCode = purchaseInvoiceSaveViewModel.LocalCurrencyCode,
                CurrencyExchangeRate = purchaseInvoiceSaveViewModel.CurrencyExchangeRate,
                LocalVoucherAmt = purchaseInvoiceSaveViewModel.LocalVoucherAmt,
                UnProcessedPurchaseInvoiceItems = purchaseInvoiceSaveViewModel.UnProcessedItems != null ?
                                                     purchaseInvoiceSaveViewModel.UnProcessedItems.Select(upi => new PayableDetailsBO
                                                     {
                                                         DocumentType = upi.Type,
                                                         DocumentNo = upi.Number,
                                                         PayableID = upi.PayableID,
                                                         AdvanceID = upi.AdvanceID,
                                                         DebitNoteID = upi.DebitNoteID,
                                                         CreatedDate = upi.CreatedDate,
                                                         DocumentAmount = upi.Amount,
                                                         AmountToBePayed = upi.AmountToBePaid,
                                                         DueDate = upi.DueDate,
                                                         OriginalAmount = upi.OriginalAmount,
                                                         PayNow = upi.PayNow,
                                                         CreditNoteID=upi.CreditNoteID,
                                                         IRGID = upi.IRGID,
                                                         PaymentReturnVoucherTransID=upi.PaymentReturnVoucherTransID,
                                                         Narration=upi.Narration
                                                     }).ToList()
                                                    : new List<PayableDetailsBO>()

            };
        }


    }
}