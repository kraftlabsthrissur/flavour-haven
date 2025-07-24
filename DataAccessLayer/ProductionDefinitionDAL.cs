using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class ProductionDefinitionDAL
    {
        public int Save(ProductionDefinitionBO productionDefinitionBO, List<ProductionDefinitionSequenceBO> SequenceList, List<ProductionDefinitionMaterialBO> MaterialList, List<ProductionDefinitionProcessBO> ProcessList,
                        List<ProductionDefinitionMaterialBO> DeleteMaterialList, List<ProductionDefinitionProcessBO> DeleteProcessList, List<ProductionDefinitionBO> LocationList)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                ObjectParameter ProductionGroupID = new ObjectParameter("ProductionGroupID", typeof(int));
                ObjectParameter ID = new ObjectParameter("DefinitionID", typeof(int));
                ObjectParameter PackingDefinitionID = new ObjectParameter("PackingDefinitionID", typeof(int));
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var p = dbEntity.SpCreateProductionGroup(
                                productionDefinitionBO.ProductionGroupItemID,
                                productionDefinitionBO.ProductionGroupName,
                                productionDefinitionBO.IsKalkan,
                                productionDefinitionBO.ProductionLocationID,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                ProductionGroupID);
                        foreach (var sequence in SequenceList)
                        {
                            if (sequence.PackingSequence)
                            {
                                var items = MaterialList.Where(a => a.ProductionSequence == sequence.ProductionSequence);
                                var batchtypes = items.GroupBy(a => a.BatchTypeID).Select(g => g.ToList());
                                //var batchtypes = items.GroupBy(a => a.BatchTypeID).Select(g => new{ BatchTypeID =g.Key});
                                foreach (var batchtype in batchtypes)
                                {
                                    var i = dbEntity.SpCreatePackingDefinition(
                                    Convert.ToInt32(ProductionGroupID.Value),
                                    sequence.ProductID,
                                    sequence.UnitID,
                                    batchtype.FirstOrDefault().BatchTypeID,
                                    sequence.ProductionSequence,
                                    sequence.ProcessStage,
                                    GeneralBO.ApplicationID,
                                    PackingDefinitionID);
                                    foreach (var item in MaterialList.Where(a => a.ProductionSequence == sequence.ProductionSequence && a.BatchTypeID == batchtype.FirstOrDefault().BatchTypeID))
                                    {
                                        dbEntity.SpCreatePackingDefinitionMaterials(
                                            Convert.ToInt32(PackingDefinitionID.Value),
                                            item.MaterialID,
                                            item.MaterialUnitID,
                                            item.Qty,
                                            item.PrimaryToPackUnitConFact,
                                            productionDefinitionBO.ProductionGroupID,
                                            item.BatchTypeID,
                                            item.ProductionSequence,
                                            item.StartDate,
                                            item.EndDate,
                                            GeneralBO.ApplicationID
                                            );
                                    }
                                    foreach (var item in ProcessList.Where(a => a.ProductionSequence == sequence.ProductionSequence && a.BatchTypeID == batchtype.FirstOrDefault().BatchTypeID))
                                    {
                                        dbEntity.SpCreatePackingDefinitionProcesses(
                                            Convert.ToInt32(PackingDefinitionID.Value),
                                            item.ProcessName,
                                            item.Steps,
                                            item.SkilledLabourMinutes,
                                            item.SkilledLabourCost,
                                            item.UnSkilledLabourMinutes,
                                            item.UnSkilledLabourCost,
                                            item.MachineMinutes,
                                            item.MachineCost,
                                            productionDefinitionBO.ProductionGroupID,
                                            item.BatchTypeID,
                                            item.ProductionSequence,
                                            GeneralBO.ApplicationID
                                            );
                                    }
                                }
                            }
                            else
                            {
                                var i = dbEntity.SpCreateProductionDefinition(
                                Convert.ToInt32(ProductionGroupID.Value),
                                productionDefinitionBO.ProductionGroupItemID,
                                productionDefinitionBO.BatchSize,
                                sequence.ProductID,
                                sequence.UnitID,
                                sequence.StandardOutputQty,
                                sequence.ProcessStage,
                                sequence.ProductionSequence,
                                GeneralBO.ApplicationID,
                                ID);
                                foreach (var item in MaterialList.Where(a => a.ProductionSequence == sequence.ProductionSequence))
                                {
                                    dbEntity.SpCreateProductionDefinitionMaterials(
                                        Convert.ToInt32(ID.Value),
                                        item.MaterialID,
                                        item.MaterialUnitID,
                                        item.Qty,
                                        item.UsageMode,
                                        item.StartDate,
                                        item.EndDate,
                                        GeneralBO.ApplicationID
                                        );
                                }
                                foreach (var item in ProcessList.Where(a => a.ProductionSequence == sequence.ProductionSequence))
                                {
                                    dbEntity.SpCreateProductionDefinitionProcesses(
                                        Convert.ToInt32(ID.Value),
                                        item.ProcessName,
                                        item.Steps,
                                        item.SkilledLabourMinutes,
                                        item.SkilledLabourCost,
                                        item.UnSkilledLabourMinutes,
                                        item.UnSkilledLabourCost,
                                        item.MachineMinutes,
                                        item.MachineCost,
                                        item.Process,
                                        GeneralBO.ApplicationID
                                        );
                                }
                            }
                        }
                        foreach (var item in LocationList)
                        {
                            dbEntity.SpCreateProductionLocationMapping(
                                Convert.ToInt32(ProductionGroupID.Value),
                                productionDefinitionBO.ProductionGroupName,
                                item.LocationID);
                        }
                        transaction.Commit();
                        return (int)ProductionGroupID.Value;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int Update(ProductionDefinitionBO productionDefinitionBO, List<ProductionDefinitionSequenceBO> SequenceList, List<ProductionDefinitionMaterialBO> MaterialList, List<ProductionDefinitionProcessBO> ProcessList,
                          List<ProductionDefinitionMaterialBO> DeleteMaterialList, List<ProductionDefinitionProcessBO> DeleteProcessList, List<ProductionDefinitionBO> LocationList)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                ObjectParameter ID = new ObjectParameter("DefinitionID", typeof(int));
                ObjectParameter PackingDefinitionID = new ObjectParameter("PackingDefinitionID", typeof(int));
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var p = dbEntity.SpUpdateProductionGroup(
                                productionDefinitionBO.ProductionGroupID,
                                productionDefinitionBO.ProductionGroupItemID,
                                productionDefinitionBO.ProductionGroupName,
                                productionDefinitionBO.IsKalkan,
                                productionDefinitionBO.ProductionLocationID,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                GeneralBO.CreatedUserID
                                );
                        foreach (var sequence in SequenceList)
                        {
                            if (sequence.PackingSequence)
                            {
                                var items = MaterialList.Where(a => a.ProductionSequence == sequence.ProductionSequence);
                                var batchtypes = items.GroupBy(a => a.BatchTypeID).Select(g => g.ToList());
                                //var batchtypes = items.GroupBy(a => a.BatchTypeID).Select(g => new{ BatchTypeID =g.Key});
                                foreach (var batchtype in batchtypes)
                                {
                                    if (productionDefinitionBO.ProductionGroupID > 0)
                                    {
                                        var i = dbEntity.SpCreatePackingDefinition(
                                    productionDefinitionBO.ProductionGroupID,
                                    sequence.ProductID,
                                    sequence.UnitID,
                                    batchtype.FirstOrDefault().BatchTypeID,
                                    sequence.ProductionSequence,
                                    sequence.ProcessStage,
                                    GeneralBO.ApplicationID,
                                    PackingDefinitionID);
                                    }
                                    foreach (var item in MaterialList.Where(a => a.ProductionSequence == sequence.ProductionSequence && a.BatchTypeID == batchtype.FirstOrDefault().BatchTypeID))
                                    {
                                        if (item.ID > 0)
                                        {
                                            dbEntity.SpUpdatePackingDefinitionMaterials(
                                            item.ID,
                                            item.ProductionDefinitionID,
                                            item.MaterialID,
                                            item.MaterialUnitID,
                                            item.Qty,
                                            item.PrimaryToPackUnitConFact,
                                            item.StartDate,
                                            item.EndDate,
                                            GeneralBO.ApplicationID
                                        );
                                        }
                                        else
                                        {
                                            dbEntity.SpCreatePackingDefinitionMaterials(
                                            Convert.ToInt32(PackingDefinitionID.Value),
                                            item.MaterialID,
                                            item.MaterialUnitID,
                                            item.Qty,
                                            item.PrimaryToPackUnitConFact,
                                            productionDefinitionBO.ProductionGroupID,
                                            item.BatchTypeID,
                                            item.ProductionSequence,
                                            item.StartDate,
                                            item.EndDate,
                                            GeneralBO.ApplicationID
                                            );
                                        }
                                    }
                                    foreach (var item in ProcessList.Where(a => a.ProductionSequence == sequence.ProductionSequence))
                                    {
                                        if (item.ProcessID > 0)
                                        {
                                            dbEntity.SpUpdatePackingDefinitionProcesses(
                                            item.ProcessID,
                                            item.ProductionDefinitionID,
                                            item.ProcessName,
                                            item.Steps,
                                            item.SkilledLabourMinutes,
                                            item.SkilledLabourCost,
                                            item.UnSkilledLabourMinutes,
                                            item.UnSkilledLabourCost,
                                            item.MachineMinutes,
                                            item.MachineCost,
                                            GeneralBO.ApplicationID
                                            );
                                        }
                                        else
                                        {
                                            dbEntity.SpCreatePackingDefinitionProcesses(
                                            Convert.ToInt32(PackingDefinitionID.Value),
                                            item.ProcessName,
                                            item.Steps,
                                            item.SkilledLabourMinutes,
                                            item.SkilledLabourCost,
                                            item.UnSkilledLabourMinutes,
                                            item.UnSkilledLabourCost,
                                            item.MachineMinutes,
                                            item.MachineCost,
                                            productionDefinitionBO.ProductionGroupID,
                                            item.BatchTypeID,
                                            item.ProductionSequence,
                                            GeneralBO.ApplicationID
                                            );
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (sequence.ProductionID > 0)
                                {
                                    var i = dbEntity.SpUpdateProductionDefinition(
                                    sequence.ProductionID,
                                    productionDefinitionBO.ProductionGroupID,
                                    productionDefinitionBO.ProductionGroupItemID,
                                    sequence.ProductID,
                                    sequence.UnitID,
                                    productionDefinitionBO.BatchSize,
                                    sequence.StandardOutputQty,
                                    sequence.ProcessStage,
                                    sequence.ProductionSequence,
                                    GeneralBO.ApplicationID);
                                }
                                else
                                {
                                    var i = dbEntity.SpCreateProductionDefinition(
                                    productionDefinitionBO.ProductionGroupID,
                                    productionDefinitionBO.ProductionGroupItemID,
                                    productionDefinitionBO.BatchSize,
                                    sequence.ProductID,
                                    sequence.UnitID,
                                    sequence.StandardOutputQty,
                                    sequence.ProcessStage,
                                    sequence.ProductionSequence,
                                    GeneralBO.ApplicationID,
                                    ID);
                                }
                                foreach (var item in MaterialList.Where(a => a.ProductionSequence == sequence.ProductionSequence))
                                {
                                    if (item.ID > 0)
                                    {
                                        dbEntity.SpUpdateProductionDefinitionMaterials(
                                        item.ID,
                                        item.ProductionDefinitionID,
                                        item.MaterialID,
                                        item.MaterialUnitID,
                                        item.Qty,
                                        item.UsageMode,
                                        item.StartDate,
                                        item.EndDate,
                                        GeneralBO.ApplicationID
                                        );
                                    }
                                    else
                                    {
                                        var id = 0;
                                        if (sequence.ProductionID > 0)
                                        {
                                            id = sequence.ProductionID;
                                        }
                                        else
                                        {
                                            id = Convert.ToInt32(ID.Value);
                                        }
                                        dbEntity.SpCreateProductionDefinitionMaterials(
                                        id,
                                        item.MaterialID,
                                        item.MaterialUnitID,
                                        item.Qty,
                                        item.UsageMode,
                                        item.StartDate,
                                        item.EndDate,
                                        GeneralBO.ApplicationID
                                        );
                                    }
                                }
                                foreach (var item in ProcessList.Where(a => a.ProductionSequence == sequence.ProductionSequence))
                                {
                                    if (item.ProcessID > 0)
                                    {
                                        dbEntity.SpUpdateProductionDefinitionProcesses(
                                            item.ProcessID,
                                            item.ProductionDefinitionID,
                                            item.ProcessName,
                                            item.Steps,
                                            item.SkilledLabourMinutes,
                                            item.SkilledLabourCost,
                                            item.UnSkilledLabourMinutes,
                                            item.UnSkilledLabourCost,
                                            item.MachineMinutes,
                                            item.MachineCost,
                                            item.Process,
                                            GeneralBO.ApplicationID
                                            );
                                    }
                                    else
                                    {
                                        var id = 0;
                                        if (sequence.ProductionID > 0)
                                        {
                                            id = sequence.ProductionID;
                                        }
                                        else
                                        {
                                            id = Convert.ToInt32(ID.Value);
                                        }
                                        dbEntity.SpCreateProductionDefinitionProcesses(
                                            id,
                                            item.ProcessName,
                                            item.Steps,
                                            item.SkilledLabourMinutes,
                                            item.SkilledLabourCost,
                                            item.UnSkilledLabourMinutes,
                                            item.UnSkilledLabourCost,
                                            item.MachineMinutes,
                                            item.MachineCost,
                                            item.Process,
                                            GeneralBO.ApplicationID
                                            );
                                    }
                                }
                            }
                        }
                        foreach (var deleteMaterials in DeleteMaterialList)
                        {
                            dbEntity.SpDeleteProductionDefinitionMaterials(
                            deleteMaterials.ID,
                            deleteMaterials.PackingSequence,
                            GeneralBO.ApplicationID);
                        }
                        foreach (var deleteProcesses in DeleteProcessList)
                        {
                            dbEntity.SpDeleteProductionDefinitionProcesses(
                            deleteProcesses.ProcessID,
                            deleteProcesses.PackingSequence,
                            GeneralBO.ApplicationID);
                        }
                        foreach (var item in LocationList)
                        {
                            dbEntity.SpCreateProductionLocationMapping(
                                productionDefinitionBO.ProductionGroupID,
                                productionDefinitionBO.ProductionGroupName,
                                item.LocationID);
                        }
                        transaction.Commit();
                        return productionDefinitionBO.ProductionGroupID;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<ProductionDefinitionBO> GetProductionDefinitionList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetProductionDefinitionList(GeneralBO.ApplicationID).Select(a => new ProductionDefinitionBO()
                    {
                        ID = (int)a.ID,
                        ProductionGroupID = (int)a.ProductGroupID,
                        ProductionGroupName = a.ProductionGroupName,
                        ProductName = a.ProductName,
                        BatchSize = Convert.ToDecimal(a.BatchSize),
                        StandardOutputQty = Convert.ToDecimal(a.StandardOutPut)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductionDefinitionBO> GetProductionDefinition(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetProductionDefinition(ID, GeneralBO.ApplicationID).Select(a => new ProductionDefinitionBO()
                    {
                        ProductionID = (int)a.ID,
                        ProductionGroupID = (int)a.ProductGroupID,
                        ProductionGroupItemID = (int)a.ProductID,
                        ProductionGroupName = a.ProductionGroupName,
                        ProductID = (int)a.ProductID,
                        ProductName = a.ProductName,
                        ItemName = a.ItemName,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        BatchSize = (decimal)a.BatchSize,
                        ProcessStage = a.ProcessStage,
                        StandardOutputQty = (decimal)a.StandardOutPut,
                        ProductionSequence = (int)a.ProductionSequence,
                        SequenceType = a.SequenceType,
                        IsKalkan = (bool)a.IsKalkan,
                        ProductionLocationID = (int)a.ProductionLocationID,
                        Location = a.ProductionLocation
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductionDefinitionMaterialBO> GetProductionDefinitionMaterials(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetProductionDefinitionMaterials(ID, GeneralBO.ApplicationID).Select(a => new ProductionDefinitionMaterialBO()
                    {
                        ID = (int)a.ID,
                        ProductionDefinitionID = (int)a.ProductDefenitionMasterID,
                        MaterialID = (int)a.MaterialID,
                        MaterialName = a.MaterialName,
                        MaterialUnit = a.Unit,
                        MaterialUnitID = (int)a.MaterialUnitID,
                        Qty = (decimal)a.MaterialQty,
                        UsageMode = a.UsageMode,
                        PrimaryToPackUnitConFact = (decimal)a.PrimaryToPackingUnitConversionFactor,
                        ProductionSequence = (int)a.ProductionSequence,
                        SequenceType = a.SequenceType,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchType = a.BatchType,
                        StartDate =a.StartDate,
                        EndDate=a.EndDate
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductionDefinitionProcessBO> GetProductionDefinitionProcesses(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetProductionDefinitionProcesses(ID, GeneralBO.ApplicationID).Select(a => new ProductionDefinitionProcessBO()
                    {
                        ProcessID = a.ID,
                        ProductionDefinitionID = (int)a.ProductDefenitionMasterID,
                        ProcessName = a.ProcessName,
                        Steps = a.ProcessSteps,
                        SkilledLabourMinutes = (decimal)a.SkilledLabourMinutes,
                        SkilledLabourCost = (decimal)a.SkilledLabourCost,
                        UnSkilledLabourMinutes = (decimal)a.UnSkilledLabourMinutes,
                        UnSkilledLabourCost = (decimal)a.UnSkilledLabourCost,
                        MachineMinutes = (decimal)a.MachineMinutes,
                        MachineCost = (decimal)a.MachineCost,
                        Process = a.Process,
                        ProductionSequence = (int)a.ProductionSequence,
                        SequenceType = a.SequenceType,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchType = a.BatchType
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsEditable(int ProductionGroupID)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    ObjectParameter IsEditable = new ObjectParameter("IsEditable", typeof(bool));
                    dbEntity.SpIsProductionDefinitionEditable(ProductionGroupID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsEditable);
                    return Convert.ToBoolean(IsEditable.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public DatatableResultBO GetProductionDefinitionList(string ProductionGroup, string Name, string BatchSize, string OutputQty, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetProductionDefinitionListForDatatable(ProductionGroup, Name, BatchSize, OutputQty, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID =item.ID,
                                ProductionGroupID = item.ProductGroupID,
                                ProductionGroupName = item.ProductionGroupName,
                                ProductName = item.ProductName,
                                BatchSize = Convert.ToDecimal(item.BatchSize),
                                StandardOutputQty = Convert.ToDecimal(item.StandardOutPut)
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

    }

}
