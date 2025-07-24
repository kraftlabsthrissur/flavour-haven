using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class PurchaseReturnProcessingBO
    {
    }
    public class PurchaseReturnProcessingItemBO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int BatchID { get; set; }
        public string BatchNo { get; set; }
        public bool IsGSTRegistered { get; set; }
        public string Supplier { get; set; }
        public int SupplierID { get; set; }
        public string InvoiceNo { get; set; }
        public int InvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal OfferQty { get; set; }
        public decimal Qty { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal Stock { get; set; }
        public int SupplierStateID { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public int UnitID { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public int InvoiceTransID { get; set; }
        public decimal Value { get; set; }

        public int NoOFDaysInventoryHeld { get; set; }
        public string Unit { get; set; }
    }
}
