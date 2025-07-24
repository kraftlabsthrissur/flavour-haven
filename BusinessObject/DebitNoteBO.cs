using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public  class DebitNoteBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsDraft { get; set; }
        public int DebitAccountID { get; set; }
        public int CreditAccountID { get; set; }
        public string DebitAccount { get; set; }
        public string DebitAccountCode { get; set; }
        public string CreditAccount { get; set; }
        public string Remarks { get; set; }
        public string BillNo { get; set; }
        public bool IsProcessed { get; set; }

        public int GSTCategoryID { get; set; }
        public int GSTID { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsInclusive { get; set; }
        public decimal GSTPercent { get; set; }
        public int SupplierID { get; set; }

    }
}
