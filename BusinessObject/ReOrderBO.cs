using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class ReOrderBO
    {
        public int ReOrderDays { get; set; }
        public int OrderDays { get; set; }
        public string ItemType { get; set; }
    }
    public class ReOrderItemBO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Supplier { get; set; }
        public decimal Rate { get; set; }
        public decimal Qty { get; set; }
        public decimal Stock { get; set; }
        public decimal ReOrderQty { get; set; }
        public decimal ReOrderQtyFull { get; set; }
        public DateTime LastPurchasedDate { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public int SupplierID { get; set; }
        public bool IsOrdered { get; set; }
        public int ReOrderItemID { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal LastPurchaseQty { get; set; }
        public decimal LastPurchaseOfferQty { get; set; }
        public string PurchaseUnit { get; set; }
    }
}
