using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class SLAToBePostedBO
    {
        public int AccountDebitID { get; set; }
        public string DebitAccount { get; set; }
        public int AccountCreditID { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public int SLAMappingItemID { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime Date { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public int TablePrimaryID { get; set; }
        public string DocumentTable { get; set; }
        public string DocumentNo { get; set; }

    }
}
