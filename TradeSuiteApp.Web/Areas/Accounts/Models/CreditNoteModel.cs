using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class CreditNoteModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsDraft { get; set; }
        public int DebitAccountID { get; set; }
        public int CreditAccountID { get; set; }
        public string DebitAccount { get; set; }
        public string CreditAccount { get; set; }
        public string Remarks { get; set; }
        public string BillNo { get; set; }
        public bool IsProcessed { get; set; }
        public int GSTCategoryID { get; set; }
        public int GSTID { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GSTPercent { get; set; }
        public bool IsInclusive { get; set; }


        public SelectList GSTList { get; set; }
        public SelectList GSTCategoryList { get; set; }
        

    }
}