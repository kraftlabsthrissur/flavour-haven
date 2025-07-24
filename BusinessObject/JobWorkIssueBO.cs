using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class JobWorkIssueBO
    {
        public int ID { get; set; }
        public string IssueNo { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsDraft { get; set; }
        public string Supplier { get; set; }
        public int SupplierID { get; set; }
        public string Warehouse { get; set; }
        public int WarehouseID { get; set; }
    }
    public class JobWorkIssueItemBO
    {
        public string IssueItemName { get; set; }
        public int IssueItemID { get; set; }
        public string IssueUnit { get; set; }
        public decimal IssueQty { get; set; }
        public string ReceiptItemName { get; set; }
        public int IssueTransID { get; set; }
        public decimal QtyMet { get; set; }
        public int WarehouseID { get; set; }
        public decimal Stock { get; set; }

    }
}
