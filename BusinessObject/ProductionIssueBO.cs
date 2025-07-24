using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProductionIssueBO
    {
        public int ProductionIssueID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }

        public string BatchNo { get; set; }
        public decimal StandardBatchSize { get; set; }
        public decimal ActualBatchSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal OutputQty { get; set; }
        public DateTime ProductionStartDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public string ProductionStatus { get; set; }
        public bool IsAborted { get; set; }
        public decimal AverageCost { get; set; }
        public int ProductionGroupID { get; set; }
        public string ProductionGroupName { get; set; }
        public int ProductionSequence { get; set; }

        public string ProductionStageItem { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public string ProcessStage { get; set; }
        public bool IsKalkan { get; set; }
        public int ProductionLocationID { get; set; }
        public string ProductionLocation { get; set; }
        public int CreatedUserID { get; set; }
        public int ProductionID { get; set; }
        public int ProductionScheduleID { get; set; }
        public string ProductionScheduleName { get; set; }

        public List<MaterialProductionIssueBO> MaterialProductionIssueBOList { get; set; }
        public List<ProcessProductionIssueBO> ProcessProductionIssueBOList { get; set; }
        public List<MaterialTransBO> MaterialTransBOList { get; set; }
        public List<ProductionOutputBO> OutputBOList { get; set; }
    }
    public class ProductionOutputBO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int ProductionSequence { get; set; }
        public decimal StandardBatchSize { get; set; }
        public decimal ActualBatchSize { get; set; }
        public decimal StandardOutput { get; set; }
        public decimal ActualOutput { get; set; }
        public decimal Variance { get; set; }
        public int ReceiptStoreID { get; set; }
        public string ReceiptStore { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsAborted { get; set; }
        public int ProductionID { get; set; }
        public int ProductionIssueID { get; set; }
        public bool IsQcRequired { get; set; }
        public string ProductionStatus { get; set; }
        public bool IsSubProduct { get; set; }
        public string ProcessStage { get; set; }
        public string Unit { get; set; }

    }
    public class MaterialProductionIssueBO
    {
        public int MaterialProductionIssueID { get; set; }
        public int RawMaterialId { get; set; }
        public string RawMaterialName { get; set; }
        public decimal RawMaterialQty { get; set; }
        public int RawMaterialUnitID { get; set; }
        public string UOM { get; set; }
        public int QOM { get; set; }
        public string UnitName { get; set; }
        public decimal Stock { get; set; }
        public int BatchID { get; set; }
        public int ProductionSequence { get; set; }
        public bool IsCompleted { get; set; }
        public decimal StandardQty { get; set; }
        public decimal ActualStandardQty { get; set; }
        public decimal IssueQty { get; set; }
        public decimal AdditionalIssueQty { get; set; }
        public decimal Variance { get; set; }
        public string BatchNo { get; set; }
        public int WareHouseID { get; set; }
        public DateTime? IssueDate { get; set; }
        public string Remarks { get; set; }
        public decimal AverageRate { get; set; }
        //public int ItemID { get; set; }
        //public string ItemName { get; set; }
        public int StoreID { get; set; }
        public string Store { get; set; }
        public bool IsQcRequired { get; set; }
        public bool? IsAdditionalIssue { get; set; }
        public int ProductDefinitionTransID { get; set; }
        public decimal ActualOutPutForStdBatch { get; set; }
        public string Category { get; set; }
        public bool IsSubProduct { get; set; }
        public int BatchTypeID { get; set; }
        public List<BatchBO> Batches { get; set; }

    }

    //public class ProductionIssueBatchBO
    //{
    //    public int ID { get; set; }
    //    public string BatchNo{ get; set; }
    //    public int ItemID{ get; set; }
    //    public decimal Stock{ get; set; }

    //}

    public partial class ProcessProductionIssueBO
    {
        public int ProcessProductionIssueID { get; set; }
        public int ProductionIssueID { get; set; }
        public string Stage { get; set; }
        public string ProcessName { get; set; }
        public decimal UnSkilledLabourHours { get; set; }
        public decimal SkilledLabourHours { get; set; }
        public decimal MachineHours { get; set; }
        public int ProductionSequence { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal SkilledLabourActualHours { get; set; }
        public decimal UnSkilledLabourActualHours { get; set; }
        public decimal MachineActualHours { get; set; }
        public string Status { get; set; }
        public string DoneBy { get; set; }
        public string Remarks { get; set; }
        public decimal AverageProcessCost { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProcessDefinitionTransID { get; set; }
    }

    public class AdditionalIssueItemBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public decimal Stock { get; set; }
        public decimal QtyUnderQC { get; set; }
        public decimal QtyOrdered { get; set; }
    }

    public class MaterialReturnItemBO
    {
        public int RawMaterialId { get; set; }
        public string RawMaterialName { get; set; }
        public decimal RawMaterialQty { get; set; }
        public int RawMaterialUnitID { get; set; }
        public string UOM { get; set; }
        public int QOM { get; set; }
        public string UnitName { get; set; }
        public decimal Stock { get; set; }
    }

    public class BatchWiseStockBO
    {
        public int BatchID { get; set; }
        public string BatchNo { get; set; }
        public int ItemID { get; set; }
        public decimal Stock { get; set; }
    }

    public class MaterialTransBO
    {
        public int ID { get; set; }
        public int ProductionIssueID { get; set; }
        public int ProductionIssueMaterialsID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public int BatchID { get; set; }
        public string BatchNo { get; set; }
        public decimal IssueQty { get; set; }
        public decimal AverageRate { get; set; }
        public string Remarks { get; set; }
        public DateTime IssueDate { get; set; }
    }


}
