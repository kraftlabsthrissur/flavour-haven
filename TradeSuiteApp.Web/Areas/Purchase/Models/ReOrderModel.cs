using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class ReOrderModel
    {
        public int ReOrderDays { get; set; }
        public int OrderDays { get; set; }
        public SelectList ItemTypeList { get; set; }
        public string ItemType { get; set; }
        public List<ReOrderItemModel> Items { get; set; }
    }
    public class ReOrderItemModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Supplier { get; set; }
        public decimal Rate { get; set; }
        public decimal Qty { get; set; }
        public decimal Stock { get; set; }
        public decimal ReOrderQty { get; set; }
        public decimal ReOrderQtyFull { get; set; }
        public string LastPurchasedDate { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public string LastPurchasedSupplier { get; set; }
        public int LastPurchasedQty { get; set; }
        public int SupplierID { get; set; }
        public bool IsOrdered { get; set; }
        public SelectList SupplierList { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal LastPurchaseQty { get; set; }
        public decimal LastPurchaseOfferQty { get; set; }
        public string PurchaseUnit { get; set; }
    }
}