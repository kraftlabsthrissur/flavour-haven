using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class JobWorkReceiptBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCancelled { get; set; }
        public string Supplier { get; set; }
        public int SupplierID { get; set; }
        public int IssueID { get; set; }
        public string IssueNo { get; set; }
        public string Warehouse { get; set; }
        public int WarehouseID { get; set; }

    }
    public class JobWorkIssuedItemBO
    {
        public int IssueTransID { get; set; }
        public decimal PendingQuantity { get; set; }
        public bool IsCompleted { get; set; }
        public decimal IssuedQty { get; set; }
        public string IssuedItem { get; set; }
        public string IssuedUnit { get; set; }


    }
    public class JobWorkReceiptItemBO
    {
        public int ReceiptItemID { get; set; }
        public string ReceiptUnit { get; set; }
        public decimal ReceiptQty { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string ReceiptItemName { get; set; }
        public int WarehouseID { get; set; }

    }
}
