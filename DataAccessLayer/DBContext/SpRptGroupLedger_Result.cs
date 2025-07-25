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
    
    public partial class SpRptGroupLedger_Result
    {
        public string TransCode { get; set; }
        public int LedgerAccId { get; set; }
        public string LedgerCode { get; set; }
        public string Ledger { get; set; }
        public Nullable<int> GroupId { get; set; }
        public string GroupName { get; set; }
        public string AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string Narration { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public decimal DECValue1 { get; set; }
        public decimal DECValue2 { get; set; }
        public int INTValue1 { get; set; }
        public int INTValue2 { get; set; }
        public string CHARValue1 { get; set; }
        public string CHARValue2 { get; set; }
        public string CurrencyCode { get; set; }
        public string BaseCurrencyCode { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
    }
}
