using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SupplierDebitNoteBO
    {
        public int ID { get; set; }
        public String TransNo { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceInvoiceNumber { get; set; }
        public DateTime ReferenceDocumentDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsDraft { get; set; }
        public int ItemID { get; set; }
        public int ItemName { get; set; }
        public int SupplierID { get; set; }
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
        public decimal RoundOff { get; set; }

        public int DepartmentID { get; set; }
        public int LocationID { get; set; }
        public int InterCompanyID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }

        public string Location { get; set; }
        public string InterCompany { get; set; }
        public string DepartmentName { get; set; }
        public string Employee { get; set; }
        public string Project { get; set; }

        public List<CustomerCreditNoteTransBO> Items { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int StateID { get; set; }
        public int LocationStateID { get; set; }
        
    }
    public class SupplierDebitNoteTransBO
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
        public int DepartmentID { get; set; }
        public int LocationID { get; set; }
        public int InterCompanyID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public int PurchaseReturnID { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GSTPercentage { get; set; }

        public string Location { get; set; }
        public string InterCompany { get; set; }
        public string Department { get; set; }
        public string Employee { get; set; }
        public string Project { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
    }
}
