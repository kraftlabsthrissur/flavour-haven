using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SLAPostedBO
    {
        public int AccountDebitID { get; set; }
        public string DebitAccount { get; set; }
        public int AccountCreditID { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime VoucherDate { get; set; }
        public string DocumentTable { get; set; }
        public string DocumentNo { get; set; }

    }
}
