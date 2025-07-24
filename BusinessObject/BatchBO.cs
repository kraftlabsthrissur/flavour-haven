using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class BatchBO
    {
        public int ID { get; set; }
        public string BatchNo { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchTypeName { get; set; }
        public string Name { get; set; }
        public int ItemID { get; set; }
        public decimal Stock { get; set; }
        public decimal Rate { get; set; }
        public string ItemCode { get; set; }
        public int WarehouseID { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CustomBatchNo { get; set; }
        public string ItemType { get; set; }
        public decimal IssueQty { get; set; }
        public DateTime ManufacturingDate { get; set; }

        public decimal ExportPrice { get; set; }
        public decimal ISKPrice { get; set;}
        public decimal OSKPrice { get; set; }

        public decimal PurchaseMRP { get; set; }
        public decimal RetailMRP { get; set; }
        public decimal RetailLooseRate { get; set; }
        public decimal BatchRate { get; set; }

        public decimal PurchaseLooseRate { get; set; }
        public int UnitID { get; set; }
        public decimal PackSize { get; set; }
        public string Unit { get; set; }
        public decimal ProfitPrice { get; set; }

        public decimal CessPercentage { get; set; }
        public decimal GSTPercentage { get; set; }

        public int BusinessCategoryID { get; set; }
        public string BusinessCategory { get; set; }

        public string Category { get; set; }
        public string PrimaryUnit { get; set; }
        public string InventoryUnit { get; set; }
        public int PrimaryUnitID { get; set; }
        public int InventoryUnitID { get; set; }
        public decimal ConversionFactorPtoI { get; set; }
        public decimal LooseRatePercent { get; set; }

        public int BatchID { get; set; }
        public decimal FullSellingPrice { get; set; }
        public decimal LooseSellingPrice { get; set; }
        public decimal FullPurchasePrice { get; set; }
        public decimal LoosePurchasePrice { get; set; }
        public int IsSuspended { get; set; }
    }

    public class StockIssueBatchBO : BatchBO
    {
        public decimal IssueQty { get; set; }
        public string StockRequisitionNo { get; set; }
        public int StockRequisitionTransID { get; set; }
        public int StockRequisitionID { get; set; }

    }

    public class SalesBatchBO : BatchBO
    {
        public int BatchID { get; set; }
        public decimal Qty { get; set; }
        public decimal OfferQty { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceOfferQty { get; set; }
        public bool InvoiceQtyMet { get; set; }
        public bool InvoiceOfferQtyMet { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public string BatchType { get; set; }
        public string Code { get; set; } //Item Code
        public string SalesOrderNo { get; set; }
        public int SalesOrderID { get; set; }
        public int SalesOrderTransID { get; set; }
        public decimal LooseRate { get; set; }
        public decimal FullRate { get; set; }
        public int SalesUnitID { get; set; }
        public decimal CessPercentage { get; set; }
        public bool IsGSTRegisteredLocation { get; set; }
    }
    public class PreProcessBatchBO : BatchBO
    {
        public decimal RequestedQty { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public string ItemName { get; set; }

    }
public class PreviousBatchBO:BatchBO
    {
        public string SupplierName { get; set; }
        public DateTime? PODate { get; set; }
        public decimal Quantity { get; set; }
        public decimal OfferQty { get; set; }
        public decimal ProfitRatio { get; set; }
        public decimal ProfitTolerance { get; set; }
        public decimal Discount { get; set; }

        public string TransNo { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesRate { get; set; }
        public decimal LooseSalesRate { get; set; }
        public decimal LooseQty { get; set; }
        public decimal GSTAmount { get; set; }
        public int DiscountID { get; set; }
        public decimal InvoiceRate { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public int InvoiceID { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
    }
}
