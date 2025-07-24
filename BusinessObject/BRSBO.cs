using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class BRSBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime Date { get; set; }
        public bool IsDraft { get; set; }
        public DateTime FromTransactionDate { get; set; }
        public DateTime ToTransactionDate { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int AttachmentID { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }

        public decimal BalAsPerCompanyBooks { get; set; }
        public decimal CreditAmountNotReflectedInBank { get; set; }
        public decimal DebitAmountNotReflectedInBank { get; set; }
        public decimal BalAsPerBank { get; set; }

        public List<BRSTransBO> Items { get; set; }
        public List<BankStatementBO> Statements { get; set; }

    }
    public class BRSTransBO
    {
        public int ID { get; set; }
        public string DocumentNumber { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime InstrumentDate { get; set; }
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
        public DateTime ReconciledDate { get; set; }
        public int DocumentID { get; set; }
    }
    public class BankStatementBO
    {
        public int ID { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime InstrumentDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

    }

}
