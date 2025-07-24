using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class ProductionDefinitionModel
    {
        public int ID { get; set; }
        public int ProductionID { get; set; }
        public int ProductionGroupID { get; set; }
        public int ProductionGroupItemID { get; set; }
        public string ProductionGroupName { get; set; }
        public string ItemName { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal BatchSize { get; set; }
        public string ProcessStage { get; set; }
        public decimal StandardOutputQty { get; set; }
        public int ProductionSequence { get; set; }
        public bool PackingSequence { get; set; }
        public string SequenceType { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public bool IsEditable { get; set; }
        public bool IsDraft { get; set; }
        public int ItemCategoryID { get; set; }
        public int FinishedGoodsItemCategoryID { get; set; }
        public int SemifinishedGoodsItemCategoryID { get; set; }
        public bool IsKalkan { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public int ProductionLocationID { get; set; }
        public List<SequenceModel> Sequences { get; set; }
        
        
        public SelectList LocationList { get; set; }

    }

    public class SequenceModel
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
        public SelectList BatchTypeList { get; set; }
        public SelectList UnitList { get; set; }       
        public List<ProductionDefinitionMaterial> Items { get; set; }
        public List<ProductionDefinitionProcess> Processes { get; set; }
    }

    public class ProductionDefinitionMaterial
    {
        public int ID { get; set; }
        public int ProductionDefinitionID { get; set; }
        public int MaterialID { get; set; }
        public string MaterialName { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public string UsageMode { get; set; }
        public int ProductionSequence { get; set; }
        public bool PackingSequence { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public decimal PrimaryToPackUnitConFact { get; set; }
        public string EndDate { get; set; }
        public string StartDate { get; set; }
    }

    public class ProductionDefinitionProcess
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
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
    }

    public class ProductionDefinitionSaveModel
    {
        public int ID { get; set; }
        public int ProductionID { get; set; }
        public int ProductionGroupID { get; set; }
        public int ProductionGroupItemID { get; set; }
        public string ProductionGroupName { get; set; }
        public decimal BatchSize { get; set; }
        public bool IsKalkan { get; set; }
        public int ProductionLocationID { get; set; }
        public List<SequenceModel> Sequences { get; set; }
        public List<ProductionDefinitionMaterial> Materials { get; set; }
        public List<ProductionDefinitionProcess> Processes { get; set; }
        public List<SequenceModel> DeleteSequences { get; set; }
        public List<ProductionDefinitionMaterial> DeleteMaterials { get; set; }
        public List<ProductionDefinitionProcess> DeleteProcesses { get; set; }
        public List<ProductionLocationList> LocationList { get; set; }
    }

    public class ProductionLocationList
    {
        public int LocationID { get; set; }
        public int ProductionLocationID { get; set; }
        public string LocationName { get; set; }
    }
}