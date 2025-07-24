using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class SupplierCreditNoteModel
    {
        public int ID { get; set; }
        public String TransNo { get; set; }
        public string Date { get; set; }
        public string ReferenceInvoiceNumber { get; set; }
        public string ReferenceDocumentDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal TaxableAmount { get; set; }
        public bool IsDraft { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int SupplierID { get; set; }
        public String SupplierName { get; set; }
        public int StateID { get; set; }
        public string PriceListID { get; set; }
        public string IsGSTRegistered { get; set; }
        public string SchemeID { get; set; }
        public int LocationStateID { get; set; }
        public string CustomerName { get; set; }
        public decimal RoundOff { get; set; }

        public int DepartmentID { get; set; }
        public int LocationID { get; set; }
        public int InterCompanyID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public string FirstOpenDate { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList InterCompanyList { get; set; }
        public SelectList DepartmentList { get; set; }
        public SelectList EmployeeList { get; set; }
        public SelectList ProjectList { get; set; }
        public List<SupplierCreditNoteItemModel> Items { get; set; }

        public bool IsGSTRegistred { get; set; }
        public string Status { get; set; }
    }

    public class SupplierCreditNoteItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int CreditNoteID { get; set; }
        public string ItemName { get; set; }
        public string ReferenceInvoiceNumber { get; set; }
        public String ReferenceDocumentDate { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public string Remarks { get; set; }
        public int DepartmentID { get; set; }
        public int LocationID { get; set; }
        public int InterCompanyID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string InterCompany { get; set; }
        public string Employee { get; set; }
        public string Project { get; set; }
        public decimal GSTPercentage { get; set; }
    }
}