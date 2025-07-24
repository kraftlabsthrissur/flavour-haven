using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DebitOrCreditBO
    {
        public int ID { get; set; }
        public String TransNo { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceInvoiceNumber { get; set; }
        public DateTime ReferenceDocumentDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int PartyID { get; set; }
        public string PartyName { get; set; }
        public string PartyType { get; set; }
        public string DebitOrCreditType { get; set; }
        public int BillingStateID { get; set; }
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public string Addresses { get; set; }
        public string BillingState { get; set; }
        public string BankName { get; set; }
        public string IFSCNo { get; set; }
        public string BankACNo { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public List<DebitOrCreditNoteItemBO> Items { get; set; }
    }

    public class DebitOrCreditNoteItemBO
    {
        public int ID;
        public int CreditNoteID { get; set; }
        public int ItemID { get; set; }
        public String Item { get; set; }
        public String HSNCode { get; set; }
        public String Unit { get; set; }
        public string ReferenceInvoiceNumber { get; set; }
        public DateTime ReferenceDocumentDate { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string PurchaseReturnNo { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
    }
}
