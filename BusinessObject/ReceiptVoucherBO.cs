using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ReceiptVoucherBO
    {
        public DateTime ReceiptDate { get; set; }
        public int ID { get; set; }
        public decimal ReceiptAmount { get; set; }
        public string BankName { get; set; }
        public string BankReferanceNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Remarks { get; set; }
        public string ReceiptNo { get; set; }
        public string CustomerName { get; set; }
        public int CustomerCategoryID { get; set; }
        public int CustomerID { get; set; }
        public int CurrencyID { get; set; }
        public List<ReceiptItemBO> Item { get; set; }
        public bool IsDraft { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public int BankID { get; set; }
        public int DebitNoteID { get; set; }
        public int CreditNoteID { get; set; }
        public bool IsBlockedForChequeReceipt { get; set; }

        //created for version3
        public int AccountHeadID { get; set; }
        public int AccountID { get; set; }
        public string AccountHead { get; set; }

        public string Currency { get; set; }
        public DateTime ReconciledDate { get; set; }

        public int DiscountTypeID { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public string currencycode { get; set; }
        public string AmountInWords { get; set; }
        public decimal SuuplierCurrencyconverion { get; set; }
        public string supplierCurrencycode { get; set; }
        public decimal CalculatedAmount { get; set; }
        public string ReceiverBankName { get; set; }
        public string BankInstrumentNumber {  get; set; }
        public string checqueDate { get; set; }
        public int ReceiverBankID { get; set; }






    }
    public class ReceiptItemBO
    {
        public int VoucherID { get; set; }
        public int DebitNoteID { get; set; }
        public int CreditNoteID { get; set; }
        public int AdvanceID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public DateTime ReceivableDate { get; set; }
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

    public class ReceiptSettlementBO
    {
        public int AdvanceID    { get; set; }
        public int CreditNoteID { get; set; }
        public int DebitNoteID  { get; set; }
        public int ReceivableID { get; set; }
        public int SalesReturnID { get; set; }

        public string SettlementFrom { get; set; }
        public string DocumentNo        { get; set; }
        public string DocumentType      { get; set; }

        public decimal Amount           { get; set; }
        public decimal SettlementAmount { get; set; }

    }
}