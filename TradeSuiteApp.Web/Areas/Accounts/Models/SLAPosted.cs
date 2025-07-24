using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class SLAPosted
    {
        public int AccountDebitID { get; set; }
        public string DebitAccount { get; set; }
        public int AccountCreditID { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string VoucherDate { get; set; }
        public string DocumentTable { get; set; }
        public string DocumentNo { get; set; }

    }
}