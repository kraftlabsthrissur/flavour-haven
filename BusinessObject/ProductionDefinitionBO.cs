using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProductionDefinitionBO
    {
        public int ID { get; set; }
        public int ProductionID { get; set; }
        public int ProductionGroupID { get; set; }
        public int ProductionGroupItemID { get; set; }
        public string ProductionGroupName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal BatchSize { get; set; }
        public string ProcessStage { get; set; }
        public decimal StandardOutputQty { get; set; }

        public int MaterialID { get; set; }
        public string MaterialName { get; set; }
        public int MaterialUnitID { get; set; }
        public string MaterialUnit { get; set; }
        public decimal Qty { get; set; }
        public string UsageMode { get; set; }

        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string Steps { get; set; }
        public decimal SkilledLabourMinutes { get; set; }
        public decimal SkilledLabourCost { get; set; }
        public decimal UnSkilledLabourMinutes { get; set; }
        public decimal UnSkilledLabourCost { get; set; }
        public decimal MachineMinutes { get; set; }
        public decimal MachineCost { get; set; }
        public string Process { get; set; }
        public int ProductionSequence { get; set; }
        public bool PackingSequence { get; set; }
        public string ItemName { get; set; }
        public string SequenceType{ get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public decimal PrimaryToPackUnitConFact { get; set; }
        public bool IsKalkan { get; set; }
        public int ProductionLocationID { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
    }

    public class ProductionDefinitionSequenceBO
    {
        public int ProductionID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal BatchSize { get; set; }
        public string ProcessStage { get; set; }
        public decimal StandardOutputQty { get; set; }
        public int ProductionSequence { get; set; }
        public bool PackingSequence { get; set; }
        public string SequenceType { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
    }

    public class ProductionDefinitionMaterialBO
    {
        public int ID { get; set; }
        public int ProductionDefinitionID { get; set; }
        public int MaterialID { get; set; }
        public string MaterialName { get; set; }
        public int MaterialUnitID { get; set; }
        public string MaterialUnit { get; set; }
        public decimal Qty { get; set; }
        public string UsageMode { get; set; }
        public int ProductionSequence { get; set; }
        public bool PackingSequence { get; set; }
        public string SequenceType { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public decimal PrimaryToPackUnitConFact { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
    }
    public class ProductionDefinitionProcessBO
    {
        public int ProcessID { get; set; }
        public int ProductionDefinitionID { get; set; }
        public string ProcessName { get; set; }
        public string Steps { get; set; }
        public decimal SkilledLabourMinutes { get; set; }
        public decimal SkilledLabourCost { get; set; }
        public decimal UnSkilledLabourMinutes { get; set; }
        public decimal UnSkilledLabourCost { get; set; }
        public decimal MachineMinutes { get; set; }
        public decimal MachineCost { get; set; }
        public string Process { get; set; }
        public int ProductionSequence { get; set; }
        public bool PackingSequence { get; set; }
        public string SequenceType { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
    }
}