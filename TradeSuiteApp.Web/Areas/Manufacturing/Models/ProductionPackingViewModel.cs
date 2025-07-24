using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class PackingViewModel
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

        public List<PackingProcessModel> Process { get; set; }

        public List<PackingOutputModel> Output { get; set; }
        public SelectList StoreList { get; set; }

        public string Remarks { get; set; }
        public SelectList ItemNameList { get; set; }
    }
    public class PackingMaterialModel
    {
        public int PackingIssueID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string UOM { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public string Store { get; set; }
        public int StoreID { get; set; }
        public decimal AvailableStock { get; set; }
        public int ?BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchNo { get; set; }
        public decimal StandardQty { get; set; }
        public decimal ActualQty { get; set; }
        public decimal IssueQty { get; set; }
        public decimal Variance { get; set; }
        public bool IsCompleted { get; set; }
        public SelectList BatchList { get; set; }
        public bool IsDraft { get; set; }
        public SelectList StoreList { get; set; }
        public string BatchType { get; set; }
        public int PackingMaterialMasterID { get; set; }
        public string Remarks { get; set; }
        public bool IsAdditionalIssue { get; set; }
        public bool IsMaterialReturn { get; set; }
    }
    public class PackingProcessModel
    {
        public int PackingIssueID { get; set; }
        public DateTime? StartDate
        {
            get
            {
                return StartDateStr.ToDateTime();
            }
            set { StartDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public string StartDateStr { get; set; }
        public string Stage { get; set; }
        public string ProcessName { get; set; }

        public DateTime? StartTime { get { return StartTimeStr.ProcessToTime(); } set { this.StartTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }
        public string StartTimeStr { get; set; }
       public DateTime? EndTime { get { return EndTimeStr.ProcessToTime(); } set { this.EndTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }
        public string EndTimeStr { get; set; }

        public int InputQuantity { get; set; }
      
        public int OutputQty { get; set; }
        public decimal SkilledLaboursStandard { get; set; }
        public decimal UnSkilledLabourStandard { get; set; }
        public decimal MachineHoursStandard { get; set; }
        public DateTime? IssueDate { get { return IssueDateStr.ToDateTime(); } set { this.IssueDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); } }
        public string IssueDateStr { get; set; }

        public decimal SkilledLaboursActual { get; set; }
        public decimal UnSkilledLabourActual { get; set; }
        public decimal MachineHoursActual { get; set; }
        public int BatchTypeID { get; set; }
        public bool IsDraft { get; set; }
        public String DoneBy { get; set; }
        public int StatusID { get; set; }
        public SelectList StatusList { get; set; }
        public string Status { get; set; }
        public int PackingProcessDefinitionTransID { get; set; }
        public decimal BatchSize { get; set; }
        public decimal PackedQty { get; set; }
    }
    public class PackingOutputModel
    {
        public int PackingIssueID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string BatchNo { get; set; }
        public decimal PackedQty { get; set; }
        public string BatchType { get; set; }
        public int BatchTypeID { get; set; }
        public int BatchID { get; set; }
        public int StoreID { get; set; }
        public int UnitID { get; set; }
        public int ProductionSequence { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsQCCompleted { get; set; }

        //code below by prama 
        public string Date { get; set; }
    }

}