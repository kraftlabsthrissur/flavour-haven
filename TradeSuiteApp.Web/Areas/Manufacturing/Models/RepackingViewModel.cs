using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class RepackingViewModel
    {
        public int ID { get; set; }
        public string RepackingNo { get; set; }
        public string RepackingDate { get; set; }

        public string ItemIssue { get; set; }
        public int IssueItemID { get; set; }
        public int IssueItemBatchID { get; set; }
        public int IssueItemBatchTypeID { get; set; }
        public decimal QuantityIn { get; set; }

        public string ItemReceipt { get; set; }
        public int ReceiptItemID { get; set; }
        public int ReceiptItemBatchID { get; set; }
        public int ReceiptItemBatchTypeID { get; set; }
        public decimal QuantityOut { get; set; }
        public string ReceiptItemBatchType { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Isprocessed { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDraft { get; set; }

        public int ItemID { get; set; }
        public string Packing { get; set; }
        public string BatchName { get; set; }
        public decimal BatchSize { get; set; }
        public string BatchNo { get; set; }
        public string ItemName { get; set; }
        public int BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public SelectList BatchNameList { get; set; }
        public string AdditionalName { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public int DefaultPackingStoreID { get; set; }
        public int ProductGroupID { get; set; }
        public int ProductID { get; set; }
        public int Stock { get; set; }
        public string BatchType { get; set; }
        public int PakingIssueID { get; set; }
        public bool IsQCCompleted { get; set; }
        public string Status { get; set; }
        public decimal StandardQty { get; set; }
        public string Remark { get; set; }
        public decimal UnSkilledLabourStandard { get; set; }
        public decimal SkilledLaboursStandard { get; set; }
        public decimal MachineHoursStandard { get; set; }
        public int AdditionalBatchTypeID { get; set; }
        public decimal ReceiptConversionFactorP2S { get; set; }
        public decimal IssueConversionFactorP2S { get; set; }
        public List<ProductionRePackingMaterialItemModel> Materials { get; set; }
        public List<ProductionREPackingProcesItemModel> Process { get; set; }
        public List<RepakingPackingOutputModel> Output { get; set; }

    }

    public class ProductionRePackingMaterialItemModel
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int ItemID { get; set; }
        public int MyProperty { get; set; }
        public int UnitID { get; set; }
        public string UOM { get; set; }
        public decimal StandardQty { get; set; }
        public string Store { get; set; }
        public decimal ActualQty { get; set; }
        public decimal AvailableStock { get; set; }
        public string BatchNo { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public int StoreID { get; set; }
        public bool IsDraft { get; set; }
        public int PakingIssueID { get; set; }
        public decimal IssueQty { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Variance { get; set; }
        public bool IsCompleted { get; set; }
        public SelectList BatchList { get; set; }
        public SelectList StoreList { get; set; }
        public string ItemType { get; set; }
        public string ReceiptItem { get; set; }
        public string ReceiptBatchType { get; set; }
        public string ReceiptUnit { get; set; }
        public decimal ReceiptQty { get; set; }
        public decimal StandardQtyForStdBatch { get; set; }
        public bool IsAdditionalIssue { get; set; }
        public bool IsMaterialReturn { get; set; }

    }

    public class ProductionREPackingProcesItemModel
    {
        public string Stage { get; set; }
        public string ProcessName { get; set; }
        public string StartTime { get; set; }
        public int InputQuantity { get; set; }
        public string EndTime { get; set; }
        public int OutputQty { get; set; }
        public decimal SkilledLaboursStandard { get; set; }
        public decimal UnSkilledLabourStandard { get; set; }
        public decimal MachineHoursStandard { get; set; }
        public decimal SkilledLaboursActual { get; set; }
        public decimal UnSkilledLabourActual { get; set; }
        public decimal MachineHoursActual { get; set; }
        public String DoneBy { get; set; }
        public int StatusId { get; set; }
        public SelectList StatusList { get; set; }
        public string Status { get; set; }
        public int BatchTypeID { get; set; }
        public string StartTimeStr { get; set; }
        public string EndTimeStr { get; set; }
        public int PackingIssueID { get; set; }
        public bool IsDraft { get; set; }
        public decimal AverageProcessCost { get; set; }
        public string Remarks { get; set; }
    }
    public class RepakingPackingOutputModel
    {
        public int RepackingIssueID { get; set; }
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
        public string Date { get; set; }



        public string ItemReceipt { get; set; }
        public int ReceiptItemID { get; set; }
        public int ReceiptItemBatchID { get; set; }
        public int ReceiptItemBatchTypeID { get; set; }
        public decimal QuantityOut { get; set; }
        public int CreatedUserID { get; set; }
        public string CreatedDate { get; set; }
        public string QCCompleted { get; set; }

    }

}