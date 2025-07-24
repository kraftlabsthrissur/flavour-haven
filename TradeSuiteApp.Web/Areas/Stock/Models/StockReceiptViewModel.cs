using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Stock.Models
{
    public class StockReceiptViewModel
    {
        public int ID { get; set; }

        public string ReceiptNo { get; set; }
        public string Date { get; set; }

        public string IssueLocationName { get; set; }
        public string IssuePremiseName { get; set; }
        public string ReceiptLocationName { get; set; }
        public string ReceiptPremiseName { get; set; }

        public int IssueLocationID { get; set; }
        public int IssuePremiseID { get; set; }
        public int ReceiptLocationID { get; set; }
        public int ReceiptPremiseID { get; set; }

        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }

        public int LocationStateID { get; set; }

        public string Status { get; set; }
        public string ReceiptType { get; set; }
        public List<StockReceiptItem> Item { get; set; }
        public SelectList PremiseList { get; set; }
        public SelectList IssuePremiseList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public SelectList LocationList { get; set; }
        public List<LocationModel> IssueLocationList { get; set; }
        public decimal NetAmount { get; set; }
        public bool IsService { get; set; }
    }

    public class StockReceiptItem
    {
        public int ItemID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string BatchType { get; set; }
        public string BatchName { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal IssueQty { get; set; }
        public string ReceiptDate { get; set; }
        public int StockIssueTransID { get; set; }
        public int StockIssueID { get; set; }
        public decimal ReceiptQty { get; set; }
        public decimal SecondaryReceiptQty { get; set; }
        public decimal SecondaryIssueQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public string SecondaryUnit { get; set; }
        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public int IssuePremiseID { get; set; }
        public int ReceiptPremiseID { get; set; }
        public int IssueLocationID { get; set; }
        public int ReceiptLocationID { get; set; }
        public decimal Rate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TradeDiscount { get; set; }
        public decimal TradeDiscountPercent { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal BasicPrice { get; set; }
        public int UnitID { get; set; }
    }
}