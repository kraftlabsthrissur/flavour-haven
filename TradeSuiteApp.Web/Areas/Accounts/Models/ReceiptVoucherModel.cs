using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class ReceiptVoucherModel
    {
        public string ReceiptDate { get; set; }
        public int ID { get; set; }
        public decimal ReceiptAmount { get; set; }
        public string BankName { get; set; }

        public string Currency { get; set; }
        public string Date { get; set; }
        public string BankReferanceNumber { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string ReceiptNo { get; set; }
        public int CustomerCategoryID { get; set; }
        public int CustomerID { get; set; }


        public int CurrencyID { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountToBeMatched { get; set; }
        public SelectList BankCashACList { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public SelectList CurrencyLists { get; set; }
        public List<ReceiptItemModel> Item { get; set; }
        public List<ReceiptSettlement> Settlements { get; set; }
        public bool IsDraft { get; set; }
        public SelectList PaymentTypeList { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public int BankID { get; set; }
        public SelectList BankList { get; set; }
        public List<AdvanceItemModel> Items { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string GSTNo { get; set; }
        public string NetAmountInWords { get; set; }
        public bool IsBlockedForChequeReceipt { get; set; }

        //created for version3
        public int AccountHeadID { get; set; }
        public int AccountID { get; set; }
        public string AccountHead { get; set; }
        public string ReconciledDate { get; set; }
        public int ReceiverBankID { get; set; }
        //public List<SelectListItem> ReceiverBankList { get; set; }
        // public List<SelectListItem> ReceiverBankList { get; set; }
        //public SelectList ReceiverBankList { get; set; }
        public SelectList ReceiverBankList { get; set; }
       
        public string BankInstrumentNumber { get; set; }
        public string ChecqueDate { get; set; }
        public string ReceiverBankName { get; set; }
    }
}
    public class ReceiptItemModel
    {
        public int VoucherID { get; set; }
        public int DebitNoteID { get; set; }
        public int CreditNoteID { get; set; }
        public int AdvanceID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string ReceivableDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountToBeMatched { get; set; }
        public string Status { get; set; }
        public int ReceivableID { get; set; }
        public decimal AdvanceReceivedAmount { get; set; }
        public string ClassType { get; set; }
        public int PendingDays { get; set; }
        public int SalesReturnID { get; set; }
        public int CustomerReturnVoucherID { get; set; }
    }

    public class AdvanceItemModel
    {
        public string DocumentType { get; set; }
        public string ReceivableDate { get; set; }
        public decimal Amount { get; set; }
        public decimal AdvanceID { get; set; }
    }

    public class ReceiptSettlement
    {
        public int AdvanceID { get; set; }
        public int CreditNoteID { get; set; }
        public int DebitNoteID { get; set; }
        public int ReceivableID { get; set; }
        public int SalesReturnID { get; set; }

        public string SettlementFrom { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentType { get; set; }

        public decimal Amount { get; set; }
        public decimal SettlementAmount { get; set; }
        
    
}