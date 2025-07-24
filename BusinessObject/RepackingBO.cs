using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessObject
{
    public class RepackingBO
    {
        public int ID { get; set; }
        public string RepackingNo { get; set; }
        public DateTime RepackingDate { get; set; }

        public string ItemIssue { get; set; }
        public int IssueItemID { get; set; }
        public int IsuueItemBatchID { get; set; }
        public int IsuueItemBatchTypeID { get; set; }
        public decimal QuantityIn { get; set; }
        public string ReceiptItemBatch { get; set; }
        public string ItemReceipt { get; set; }
        public int ReceiptItemID { get; set; }
        public int ReceiptItemBatchID { get; set; }
        public int ReceiptItemBatchTypeID { get; set; }
        public decimal QuantityOut { get; set; }
        public string ReceiptItemBatchType { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Isprocessed { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }

        public string BatchNo { get; set; }
        public string BatchType { get; set; }
        public string ItemName { get; set; }
        public decimal AvailableStock;
        public string Code { get; set; }
        public string Name { get; set; }
        public int ItemID { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public decimal StandardQty { get; set; }
        public object StoreID { get; set; }
        public string Stage { get; set; }
        public string ProcessName { get; set; }
        public decimal UnSkilledLabourStandard { get; set; }
        public decimal SkilledLaboursStandard { get; set; }
        public decimal MachineHoursStandard { get; set; }
        public bool IsQCCompleted { get; set; }
        public int RepackedQuantity { get; set; }
        public string ItemType { get; set; }
        public decimal ActualQty { get; set; }
        public string Remark { get; set; }

        public decimal ReceiptConversionFactorP2S { get; set; }
        public decimal IssueConversionFactorP2S { get; set; }
    }
    public class ProductionRePackingMaterialItemBO
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int ItemID { get; set; }
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
        public decimal StandardQtyForStdBatch { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsAdditionalIssue { get; set; }
        public bool IsMaterialReturn { get; set; }
        public SelectList BatchList { get; set; }
        public SelectList StoreList { get; set; }

    }
    public class ProductionREPackingProcesItemBO
    {

        public int RepackingReceiptID { get; set; }
        public string Stage { get; set; }
        public string ProcessName { get; set; }
        public DateTime StartTime { get; set; }
        public int InputQuantity { get; set; }
        public DateTime EndTime { get; set; }
        public int OutputQty { get; set; }
        public decimal SkilledLaboursStandard { get; set; }
        public decimal UnSkilledLabourStandard { get; set; }
        public decimal UnSkilledLabourActual { get; set; }
        public decimal MachineHoursStandard { get; set; }
        public decimal SkilledLaboursActual { get; set; }
        public decimal UnSkilledLaboursActual { get; set; }
        public decimal MachineHoursActual { get; set; }
        public String DoneBy { get; set; }
        public int StatusId { get; set; }
        public SelectList StatusList { get; set; }
        public string Status { get; set; }
        public int BatchTypeID { get; set; }
        public decimal StartTimeStr { get; set; }
        public decimal EndTimeStr { get; set; }
        public decimal PackingIssueID { get; set; }
        public bool IsDraft { get; set; }
        public string ProcessStatus { get; set; }
        public string Remarks { get; set; }
        public decimal AverageProcessCost { get; set; }
        public int CreatedUserID { get; set; }
        public int CreatedDate { get; set; }
    }
    public class RepakingPackingOutputBO
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
        public DateTime Date { get; set; }
        public int RepackedQuantity { get; set; }
        public string CreatedDate { get; set; }
        public string QCCompleted { get; set; }

        public string ItemReceipt { get; set; }
        public int ReceiptItemID { get; set; }
        public int ReceiptItemBatchID { get; set; }
        public int ReceiptItemBatchTypeID { get; set; }
        public decimal QuantityOut { get; set; }
        public int CreatedUserID { get; set; }

    }
}
