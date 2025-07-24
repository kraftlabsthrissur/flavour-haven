using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProductionScheduleBO
    {
        public int ID { get; set; }

        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public int ProductionGroupID { get; set; }
        public int ProductID { get; set; }
        public DateTime ProductionStartDate { get; set; }
        public DateTime ProductionStartTime { get; set; }
        public decimal StandardBatchSize { get; set; }
        public decimal ActualBatchSize { get; set; }
        public int ProductionLocationID { get; set; }
        public string ProductionLocationName { get; set; }

        public int RequestedStoreID { get; set; }
        public string RequestedStoreName { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public string ProductionStatus { get; set; }
        public bool IsAborted { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductionScheduleItemBO> Items { get; set; }

        public int MouldID { get; set; }
        public string MouldName { get; set; }
        public int MachineID { get; set; }
        public string Machine { get; set; }
        public int ProcessID { get; set; }
        public string Process { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public string StartTime { get; set; }


        public string ProductionGroupName { get; set; }
        public string ItemName { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal QtyMet { get; set; }
        public decimal RequiredQty { get; set; }
        public DateTime RequiredDate { get; set; }
        public string BatchNo { get; set; }
        public int BatchID { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsKalkan { get; set; }
    }

    public class ProductionScheduleItemBO
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal YogamQty { get; set; }
        public decimal StandardBatchSize { get; set; }
        public int UnitID { get; set; }
        public decimal RequiredQty { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string Remarks { get; set; }
        public decimal QtyMet { get; set; }
        public decimal StandardOutputQty { get; set; }
        public int ProductionScheduleID { get; set; }
        public int ProductDefinitionTransID { get; set; }
        public string MalayalamName { get; set; }
        public string ProcessStage { get; set; }
        public int ProductionSequence { get; set; }
        public string UsageMode { get; set; }
    }
}
