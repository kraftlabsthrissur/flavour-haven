using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class PurchaseReturnProcessingModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public decimal NetAmount { get; set; }
        public int ItemID { get; set; }
        public decimal ReturnQty { get; set; }
        public int SupplierID { get; set; }
        public string ItemName { get; set; }
        public bool IsDraft { get; set; }
        public string SupplierName { get; set; }
        public decimal PurchaseQty { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal Discount { get; set; }
        public List<PurchaseReturnProcessingItemModel> Items { get; set; }
        public SelectList ProcessingTypeList { get; set; }
        public string ProcessingType { get; set; }
        public string AsOnDate { get; set; }
        public string FromTransactionDate { get; set; }
        public string ToTransactionDate { get; set; }
        public int LoationStateID { get; set; }
        public int Days { get; set; }
    }

    public class PurchaseReturnProcessingItemModel
    {
        public int BatchID { get; set; }
        public string Batch { get; set; }
        public string InvoiceNo { get; set; }
        public string ExpiryDate { get; set; }
        public string InvoiceDate { get; set; }
        public string ExpDate { get; set; }
        public decimal Stock { get; set; }
        public decimal Value { get; set; }
        public decimal NetAmount { get; set; }
        public int ItemID { get; set; }
        public decimal ReturnQty { get; set; }
        public int SupplierID { get; set; }
        public string ItemName { get; set; }
        public bool IsDraft { get; set; }
        public string Supplier { get; set; }
        public decimal OfferQty { get; set; }
        public int InvoiceID { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public int SupplierStateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public string BatchNo { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal Qty { get; set; }
        public int UnitID { get; set; }
        public int InvoiceTransID { get; set; }

        public int NoOFDaysInventoryHeld { get; set; }
        public string Unit { get; set; }

    }
}