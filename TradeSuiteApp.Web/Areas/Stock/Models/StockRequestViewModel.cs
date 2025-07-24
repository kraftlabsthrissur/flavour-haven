using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using BusinessObject;
namespace TradeSuiteApp.Web.Areas.Stock.Models
{
    public class StockRequestViewModel
    {
        public int ID { get; set; }
        public string RequestNo { get; set; }
        public string Date { get; set; }

        public string IssueLocationName { get; set; }
        public string ReceiptLocationName { get; set; }
        public string IssuePremiseName { get; set; }
        public string ReceiptPremiseName { get; set; }

        public int ReceiptLocationID { get; set; }
        public int IssueLocationID { get; set; }
        public int IssuePremiseID { get; set; }
        public int ReceiptPremiseID { get; set; }

        public int DefaultBatchTypeID { get; set; }

        public string BatchTypeName { get; set; }
        public string Status { get; set; }
        public int UnitID { get; set; }
        public bool IsCancelled { get; set; }
        public string ItemName { get; set; }
        public bool IsSuspended { get; set; }
        public int ItemCategoryID { get; set; }
        public bool IsDraft { get; set; }
        public bool IsClone { get; set; }
        public string ProductionGroup { get; set; }
        public string Batch { get; set; }
        public Nullable<DateTime> CancelledDate { get; set; }
        public List<StockRequestItem> Items { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList PremiseList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public SelectList IssuePremiseList { get; set; }
        public SelectList UnitList { get; set; }
    }


    public class StockRequestItem
    {
        public int ItemID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PartsNumber { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public string Remarks { get; set; }
        public string RequiredDate { get; set; }
        public decimal RequiredQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryQty { get; set; }
        public string AddedTime { get; set; }
        public int RequestTransID { get; set; }
        public int RequestedQty { get; set; }
        public decimal Stock { get; set; }
        public decimal AverageSales { get; set; }
        public string IssueDate { get; set; }
        public string BatchType { get; set; }
        public int BatchTypeID { get; set; }
        public decimal SuggestedQty { get; set; }
        public string SalesCategory { get; set; }
        public SelectList BatchList { get; set; }
        public List<SecondaryUnitBO> SecondaryUnitList { get; set; }

    }
}