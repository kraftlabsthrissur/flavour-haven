//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer.DBContext
{
    using System;
    
    public partial class SpRptSalesGSTReport_Result
    {
        public string Location { get; set; }
        public string Premise { get; set; }
        public string CustomerTaxCategory { get; set; }
        public string CustomerCategory { get; set; }
        public string CustomerName { get; set; }
        public string GSTNo { get; set; }
        public string ItemName { get; set; }
        public string ItemCategory { get; set; }
        public string SalesCategory { get; set; }
        public string SalesOrderNo { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<decimal> TaxableValue { get; set; }
        public decimal GSTRate { get; set; }
        public Nullable<decimal> SGSTAmt { get; set; }
        public Nullable<decimal> CGSTAmt { get; set; }
        public Nullable<decimal> IGSTAmt { get; set; }
        public Nullable<decimal> TotalGST { get; set; }
    }
}
