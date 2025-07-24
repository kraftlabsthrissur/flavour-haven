using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class LocalPurchaseInvoiceBO
    {
        public int ID { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string SupplierReference { get; set; }
        public int ItemCategoryID { get; set; }
        public int PurchaseCategoryID { get; set; }
        public int BatchTypeID { get; set; }
        public decimal NetAmount { get; set; }
        public int UnitID { get; set; }
        public bool IsDraft { get; set; }
        public int SupplierID { get; set; }
        public string Remarks { get; set; }
        public decimal GSTAmount { get; set; }
        public bool IsGSTRegistered { get; set; }
        public DateTime CancelledDate { get; set; }
        public decimal GrossAmnt { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal VATAmount { get; set; }
        public int CurrencyID { get; set; }
        public int SupplierStateID { get; set; }
        public int StoreID { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal Discount { get; set; }
        public string Store { get; set; }
        public bool IsCanceled { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int IsVAT { get; set; }
        public int IsGST { get; set; }
    }
    public class LocalPurchaseInvoiceItemsBO
    {
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public int PurchaseCategoryID { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercent { get; set; }
    }


    public class MRPBO
    {
        public decimal MRP { get; set; }
        public decimal Rate { get; set; }
        public DateTime ExpDate { get; set; }
    }
    public class ItemCategoryAndUnitBO
    {
        public string Category { get; set; }
        public int PurchaseUnitID { get; set; }
        public int PrimaryUnitID { get; set; }
        public string PurchaseUnit { get; set; }
        public string PrimaryUnit { get; set; }
        public decimal ConversionFactorPtoI { get; set; }

    }
}
