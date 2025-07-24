using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Utils;

namespace WebAPI.Areas.Masters.Models
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


        public int BusinessCategoryID { get; set; }
        public string BusinessCategory { get; set; }

        public decimal FullSellingPrice { get; set; }
        public decimal LooseSellingPrice { get; set; }
        public decimal FullPurchasePrice { get; set; }
        public decimal LoosePurchasePrice { get; set; }
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
        public decimal Profit { get; set; }
        public decimal CurrentBatchRetailMRP { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal CurrentDiscountAmt { get; set; }
        public decimal CurrentOfferQty { get; set; }
        public decimal CurrentQty { get; set; }
        public string CurrentSupplierName { get; set; }
    }
}