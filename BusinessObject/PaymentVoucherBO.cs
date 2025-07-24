using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class PaymentVoucherBO
    {
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public int SupplierID { get; set; }

        //Below code by prama on 28-3-2018

        public string SupplierName { get; set; }
        public int ID { get; set; }
        public String Remark { get; set; }
        public string ReferenceNo { get; set; }
        public List<PaymentVoucherItemBO> List { get; set; }
        public int PaymentTypeID { get; set; }

        public int CurrencyID { get; set; }
        public string PaymentTypeName { get; set; }
        //

        public int BankID { get; set; }
        public decimal VoucherAmount { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string SaveType { get; set; }
        public string Description { get; set; }
        public bool IsDraft { get; set; }
        public string SupplierBankName { get; set; }
        public string SupplierBankACNo { get; set; }
        public string SupplierIFSCNo { get; set; }
        public int AccountHeadID { get; set; }

        public string Currency { get; set; }
        public string AccountHead { get; set; }
        public DateTime ReconciledDate { get; set; }
        public List<PayableDetailsBO> UnProcessedPurchaseInvoiceItems { get; set; }
        public string MinimumCurrencyCode { get; set; }
        public string MinimumCurrency { get; set; }
        public string AmountInWords { get; set; }
        public decimal SuuplierCurrencyconverion { get; set; }
        public string currencycode { get; set; }
        public string supplierCurrencycode { get; set; }
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

        //public List<PurchaseInvoiceTaxDetailBO> TaxDetails { get; set; }
        //public List<PurchaseInvoiceTransItemBO> InvoiceTransItems { get; set; }
    }
    //public class UnProcessedItemsBO
    //{
    //    public int PayableID { get; set; }
    //    public string Type { get; set; }
    //    public string Number { get; set; }
    //    public DateTime Date { get; set; }
    //    public decimal Amount { get; set; }
    //    public decimal Balance { get; set; }
    //    public decimal AmountToBePaid { get; set; }
    //}

    public class PayableDetailsBO
    {
        public int ID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public int PayableID { get; set; }
        public int AdvanceID { get; set; }
        public int DebitNoteID { get; set; }
        public int CreditNoteID { get; set; }
        public int IRGID { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal DocumentAmount { get; set; }
        public decimal AmountToBePayed { get; set; }
        public DateTime DueDate { get; set; }
        public decimal PayNow { get; set; }
        public decimal OriginalAmount { get; set; }
        public string SupplierName { get; set; }
        public int PaymentReturnVoucherTransID { get; set; }
        public string Narration { get; set; }
    }

    public class PaymentVoucherItemBO
    {
        public int ID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public decimal OrginalAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal PaidAmount { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public int PayableID { get; set; }
        public int AdvanceID { get; set; }
        public int DebitNoteID { get; set; }
        public int CreditNoteID { get; set; }
        public int IRGID { get; set; }
        public int PaymentReturnVoucherTransID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal DocumentAmount { get; set; }
        public decimal AmountToBePayed { get; set; }
        public decimal PayNow { get; set; }
        public string SupplierName { get; set; }
        public string Narration { get; set; }

    }
    //
}

