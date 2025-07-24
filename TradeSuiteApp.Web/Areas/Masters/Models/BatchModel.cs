using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;
using BusinessObject;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class BatchModel
    {
        public int ID { get; set; }
        public int BatchID { get; set; }
        public string Batch { get; set; }
        public string ItemCode { get; set; }
        public string Itemtype { get; set; }
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public string BatchNo { get; set; }
        public string CustomBatchNo { get; set; }
        public string ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchTypeName { get; set; }
        public int IsSuspended { get; set; }
        public string SuspendedDate { get; set; }
        public string createdDate { get; set; }
        public decimal Rate { get; set; }
        public decimal Stock { get; set; }
        public string ExpDate { get; set; }

        public decimal ISKPrice { get; set; }
        public decimal ExportPrice { get; set; }
        public decimal OSKPrice { get; set; }
        public decimal PurchaseMRP { get; set; }

        public decimal RetailMRP { get; set; }
        public decimal RetailLooseRate { get; set; }
        public decimal BatchRate { get; set; }

        public decimal PurchaseLooseRate { get; set; }
        public int UnitID { get; set; }
        public decimal ProfitPrice { get; set; }
        public string Unit { get; set; }
        public decimal PackSize { get; set; }
        public string ExpiryDateStr
        {
            get
            {
                return General.FormatDate(ExpiryDate);
            }

            set
            {
                ExpiryDateStr = value;
            }
        }


        public decimal CessPercentage { get; set; }
        public decimal GSTPercentage { get; set; }
        public List<UnitModel> UOMList { get; set; }
        public List<PreviousBatchModel> Trans { get; set; }
        public List<GSTBO> GstList { get; set; }
        public decimal TaxPercentage { get; set; }

        public string Category { get; set; }
        public string PrimaryUnit { get; set; }
        public string InventoryUnit { get; set; }
        public int PrimaryUnitID { get; set; }
        public int InventoryUnitID { get; set; }
        public decimal ConversionFactorPtoI { get; set; }
        public decimal LooseRatePercent { get; set; }
    }
    public class StockIssueBatchModel : BatchModel
    {
        public decimal IssueQty { get; set; }
        public string StockRequisitionNo { get; set; }
        public int StockRequisitionTransID { get; set; }
        public int StockRequisitionID { get; set; }

    }

    public class SalesBatchModel : BatchModel
    {
        public decimal CGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public string Code { get; set; }//item code
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public string BatchType { get; set; }
        public string ExpiryDateString { get; set; }
        public decimal Qty { get; set; }
        public decimal OfferQty { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceOfferQty { get; set; }
        public string SalesOrderNo { get; set; }
        public int SalesOrderID { get; set; }
        public int SalesOrderTransID { get; set; }
        public decimal LooseRate { get; set; }
        public decimal FullRate { get; set; }
        public int SalesUnitID { get; set; }
        public decimal CessPercentage { get; set; }
        public bool IsGSTRegisteredLocation { get; set; }
    }
    public class PreviousBatchModel : BatchModel
    {

        public string TransNo { get; set; }
        public string InvoiceNo { get; set; }
        public string SupplierName { get; set; }
        public string PODate { get; set; }
        public decimal Quantity { get; set; }
        public decimal OfferQty { get; set; }
        public decimal ProfitRatio { get; set; }
        public decimal ProfitTolerance { get; set; }
        public decimal CurrentBatchNetProfit { get; set; }
        public decimal CurrentProfitTolerance { get; set; }
        public int CurrentUnitID { get; set; }
        public string CurrentUnit { get; set; }
        public decimal Profit { get; set; }
        public decimal CurrentBatchRetailMRP { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal CurrentDiscountAmt { get; set; }
        public decimal CurrentOfferQty { get; set; }
        public decimal CurrentQty { get; set; }
        public string CurrentSupplierName { get; set; }
        public string InvoiceDate { get; set; }
        public decimal SalesRate { get; set; }
        public decimal LooseSalesRate { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal DisCountAmount { get; set; }
        public decimal LooseQty { get; set; }
        public int DiscountID { get; set; }
        public decimal InvoiceRate { get; set; }
        public decimal PreviousQuantity { get; set; }
        public decimal PreviousOfferQty { get; set; }
        public decimal PreviousInvoiceRate { get; set; }
        public decimal PreviousPurchasePrice { get; set; }
        public decimal PreviousPurchaseLooseRate { get; set; }
        public decimal PreviousLooseSalesRate { get; set; }
        public decimal PreviousGSTAmount { get; set; }
        public decimal PreviousDiscount { get; set; }
        public decimal PreviousProfitRatio { get; set; }
        public decimal PreviousSalesRate { get; set; }
        public decimal PreviousLooseQty { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal PreviousSGSTAmt { get; set; }
        public decimal PreviousCGSTAmt { get; set; }
        public string PreviousUnit { get; set; }
        public int InvoiceID { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public List<DiscountCategoryModel> DiscountList { get; set; }
     



    }
}