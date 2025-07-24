using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class ProductionPackingScheduleModel
    {
        public DateTime? StartDate
        {
            get
            {
                return StartDateStr.ToDateTime();
            }
            set { StartDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public string StartDateStr { get; set; }
        public string TransNo { get; set; }
        public string Date { get; set; }
        public string BatchNo { get; set; }
        public decimal BatchSize { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductGroupName { get; set; }
        public int ProductGroupID { get; set; }

        public string UOM { get; set; }
        public decimal NetAmount { get; set; }
        public int ID { get; set; }
        public decimal DefinitionQty { get; set; }
        public string BatchName { get; set; }

        public string StartTime { get; set; }
        public int Quantity { get; set; }
        public string ActualOutput { get; set; }
        public string ProductionType { get; set; }
        public string AdditionalName { get; set; }
        public string ProductionLocation { get; set; }
        public SelectList ReceiptStoreList { get; set; }
        public int ReceiptStoreID { get; set; }
        public string StandardBatchSize { get; set; }
        public int BatchID { get; set; }
        public SelectList BatchList { get; set; }
        public decimal PackedQty { get; set; }
        public int DefaultPackingStoreID { get; set; }
        public int IsBatchSuspended { get; set; }
        public int BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public string BatchType { get; set; }

        public string AddItemName { get; set; }
        public string AddUOM { get; set; }
        public string AddIssueQty { get; set; }
        public decimal AvailableStock { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDraft { get; set; }
        public bool IsAborted { get; set; }
        public int UpdateOrNot { get; set; }
        public string Status { get; set; }
        public int AdditionalBatchID { get; set; }
        public int AdditionalBatchTypeID { get; set; }
        public decimal Stock { get; set; }
        public int StoreID { get; set; }
        public bool IsCancelled { get; set; }
        public List<PackingMaterialModel> Materials { get; set; }
        
        public SelectList StoreList { get; set; }

        public string Remarks { get; set; }
        public SelectList ItemNameList { get; set; }

    }
}