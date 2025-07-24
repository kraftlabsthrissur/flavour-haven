using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class BRSModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string Date { get; set; }    
        public bool IsDraft { get; set; }
        public string FromTransactionDate { get; set; }
        public string ToTransactionDate { get; set; }
        public int AttachmentID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string Status { get; set; }

        public decimal BalAsPerCompanyBooks { get; set; }
        public decimal CreditAmountNotReflectedInBank { get; set; }
        public decimal DebitAmountNotReflectedInBank { get; set; }
        public decimal BalAsPerBank { get; set; }


        public SelectList BankList { get; set; }
        public List<BRSItemModel> Items { get; set; }
        public List<BankStatementModel> Statements { get; set; }

    }
    public class BRSItemModel
    {
        public int ID { get; set; }
        public string DocumentNumber { get; set; }
        public string InstrumentNumber { get; set; }
        public string InstrumentDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Status { get; set; }
        public int EquivalentBankTransactionNumber { get; set; }
        public decimal BankCharges { get; set; }       
        public string DocumentType { get; set; }
        public string ReferenceNo { get; set; }
        public string AccountName { get; set; }
        public string Remarks { get; set; }
        public string ReconciledDate { get; set; }
        public int DocumentID { get; set; }
        
    }
        public class BankStatementModel
    {
        public int ID { get; set; }
        public string InstrumentNumber { get; set; }
        public string InstrumentDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
       
    }
   
}