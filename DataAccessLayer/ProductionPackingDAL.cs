using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductionPackingDAL
    {

        public List<PackingBO> GetProductionPacking(int PackingID)
        {
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                return dEntity.SpGetProductionPacking(PackingID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PackingBO
                {
                    ID = (int)a.ID,
                    TransNo = a.TransNo,
                    TransDate = (DateTime)a.TransDate,
                    ItemID = (int)a.ItemID,
                    ItemName = a.ItemName,
                    ProductID = (int)a.ProductID,
                    ProductName = a.ProductName,
                    ProductGroupID = (int)a.ProductGroupID,
                    ProductGroupName = a.ProductionGroupName,
                    BatchID = (int)a.BatchID,
                    BatchNo = a.BatchNo,
                    UOM = a.Unit,
                    BatchSize = (decimal)a.BatchSize,
                    PackedQty = (decimal)a.PackedQty,
                    IsCompleted = (bool)a.IsCompleted,
                    IsDraft = (bool)a.IsDraft,
                    IsCancelled = (bool)a.IsCancelled,
                    IsAborted = (bool)a.IsAborted,
                    BatchType = a.BatchType,
                    BatchTypeID = a.BatchTYpeID,
                    IsBatchSuspended=(int)a.IsBatchSuspended,
                    Remarks=a.Remarks

                }).ToList();
            }
        }
        public List<PackingBO> GetProductionPackingList()
        {
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                return dEntity.SpGetProductionPackingList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PackingBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    TransDate = (DateTime)a.TransDate,
                    ItemID = (int)a.ItemID,
                    ItemName = a.ItemName,
                    ProductID = (int)a.ProductID,
                    ProductName = a.ProductName,
                    ProductGroupID = (int)a.ProductGroupID,
                    ProductGroupName = a.ProductionGroupName,
                    BatchID = (int)a.BatchID,
                    BatchNo = a.BatchNo,
                    UOM = a.Unit,
                    BatchSize = (decimal)a.BatchSize,
                    PackedQty = (decimal)a.PackedQty,
                    IsCompleted = (bool)a.IsCompleted,
                    IsDraft = (bool)a.IsDraft,
                    IsCancelled = (bool)a.IsCancelled,
                    IsAborted = (bool)a.IsAborted,

                }).ToList();
            }
        }
        public List<PackingProcessBO> GetPackingIssueProcess(int PackingID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpPGetPackingIssueProcess(
                        PackingID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        ).Select(item => new PackingProcessBO
                        {
                            PackingIssueID = (int)item.PackingIssueID,
                            Stage = item.Stage,
                            ProcessName = item.ProcessName,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            SkilledLaboursStandard = (decimal)item.SkilledLabourStandardHour,
                            SkilledLaboursActual = (decimal)item.SkilledLabourActualHour,
                            UnSkilledLabourStandard = (decimal)item.UnSkilledLabourStandardHour,
                            UnSkilledLabourActual = (decimal)item.UnSkilledLabourActual,
                            MachineHoursStandard = (decimal)item.MachineHourStandard,
                            MachineHoursActual = (decimal)item.MachineHourActual,
                            DoneBy = item.DoneBy,
                            Status = item.ProcessStatus,
                            BatchTypeID = (int)item.BatchTypeID,
                            PackingProcessDefinitionTransID = (int)item.PackingProcessDefinitionTransID,
                            BatchSize = (decimal)item.BatchSize
                        }).ToList();

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public List<PackingMaterialBO> GetProductionPackingMaterials(int PackingID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpGetProductionPackingMaterials(
                        PackingID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID)
                        .Select(a => new PackingMaterialBO()
                        {
                            PackingIssueID = (int)a.PackingIssueID,
                            ItemCode = a.ItemCode,
                            ItemName = a.ItemName,
                            UOM = a.Unit,
                            BatchNo = a.Batch,
                            WarehouseName = a.WareHouse,
                            AvailableStock = (decimal)a.AvailableStock,
                            CurrentStock=(decimal)a.CurrentStock,
                            StandardQty = (decimal)a.StandardQty,
                            ActualQty = (decimal)a.ActualQty,
                            IssueQty = (decimal)a.IssuedQty,
                            ItemID = a.ItemID,
                            BatchType = a.BatchType,
                            BatchTypeID = a.BatchTypeID,
                            StoreID = (int)a.StoreID,
                            PackingMaterialMasterID = (int)a.PackingDefinitionMasterID,
                            UnitID = (int)a.UnitID,
                            Remarks=a.Remarks,
                            IsAdditionalIssue=(bool)a.IsAdditionalIssue,
                            IsMaterialReturn=(bool)a.IsMaterialReturn,
                            Variance=(decimal)a.Variance

                        }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public List<PackingMaterialBO> GetPackingMaterials(int ItemID, int BatchID, int ProductGroupID, int BatchTypeID, int StoreID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpPGetPackingMaterials(
                        ItemID,
                        BatchID,
                        ProductGroupID,
                        BatchTypeID,
                        StoreID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new PackingMaterialBO()
                        {
                            Code = a.ItemCode,
                            Name = a.ItemName,
                            ItemID = a.ItemID,
                            Unit = a.Unit,
                            UnitID = (int)a.UnitID,
                            StandardQty = (decimal)a.StandardQty,
                            AvailableStock = (decimal)a.AvailableStock,
                            PackingMaterialMasterID = a.PackingDefinitionMasterID
                        }
                   ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int CreatePacking(ProductionEntities dbEntity, PackingBO Packing)
        {
            string FormName = "PackingIssue";
            ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
            ObjectParameter PackingID = new ObjectParameter("PackingID", typeof(int));
            if (Packing.IsDraft)
            {
                FormName = "DraftPackingIssue";
            }


            var i = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
            var j = dbEntity.SpPCreatePacking(
                SerialNo.Value.ToString(),
                Packing.TransDate,
                Packing.StartDate,
                Packing.EndDate,
                Packing.ItemID,
                Packing.ProductID,
                Packing.ProductGroupID,
                Packing.BatchID,
                Packing.BatchSize,
                Packing.PackedQty,
                Packing.IsDraft,
                Packing.IsCompleted,
                Packing.Remarks,
                GeneralBO.CreatedUserID,
                DateTime.Now,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID,
                PackingID
                );
            return Convert.ToInt32(PackingID.Value);
        }
        public int CreatePackingIssue(ProductionEntities dbEntity, PackingOutputBO item, int PackingID)
        {
            ObjectParameter PackingIssueID = new ObjectParameter("PackingIssueID", typeof(int));

            dbEntity.SpPCreatePackingIssue(
                PackingID,
                item.ItemID,
                item.UnitID,
                item.BatchID,
                item.Date,
                item.BatchTypeID,
                item.ProductionSequence,
                item.PackedQty,
                item.StoreID,
                item.IsCompleted,
                item.IsDraft,
                item.IsQCCompleted,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID,
                PackingIssueID
            );
            return Convert.ToInt32(PackingIssueID.Value);
        }
        public void CreatePackingIssueMaterial(ProductionEntities dbEntity, PackingMaterialBO Material, int PackingIssueID,PackingBO Packing)
        {
            ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
            dbEntity.SpPCreatePackingIssueMaterails(
                PackingIssueID,
                Material.ItemID,
                Material.UnitID,
                "",
                Material.BatchID,
                Material.StoreID,
                Material.AvailableStock,
                Material.StandardQty,
                Material.IssueQty,
                Material.IsCompleted,
                Packing.IsDraft,
                Material.AverageRate,
                Material.Remarks,
                Material.PackingMaterialMasterID,
                GeneralBO.CreatedUserID,
                DateTime.Now,
                Material.IsMaterialReturn,
                Material.IsAdditionalIssue,
                Material.Variance,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID,
                RetValue
            );
            if (Convert.ToInt16(RetValue.Value) == 0)
            {
                throw new OutofStockException("Item Out of stock");
            } 
        }
        public void CreatePackingIssueProcess(ProductionEntities dbEntity, PackingProcessBO Process, int PackingIssueID)
        {
            dbEntity.SpPCreatePackingIssueProcess(
                PackingIssueID,
                Process.Stage,
                Process.ProcessName,
                Process.StartTime,
                Process.EndTime,
                Process.SkilledLaboursStandard,
                Process.SkilledLaboursActual,
                Process.UnSkilledLabourStandard,
                Process.UnSkilledLabourStandard,
                Process.UnSkilledLabourActual,
                Process.MachineHoursStandard,
                Process.MachineHoursActual,
                Process.Status,
                "",
                0,
                Process.DoneBy,
                Process.PackingProcessDefinitionTransID,
                GeneralBO.CreatedUserID,
                DateTime.Now,
                Process.BatchSize,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
            );
        }
        public void UpdateProductionPacking(ProductionEntities dbEntity, PackingBO Packing)
        {
            dbEntity.SpPUpdateProductionPacking(Packing.ID,
                Packing.EndDate,
                Packing.PackedQty,
                Packing.IsDraft,
                Packing.IsCompleted,
                Packing.Remarks,
                GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
             );
        }
        public bool Save(PackingBO Packing, List<PackingMaterialBO> Materials, List<PackingOutputBO> Items, List<PackingProcessBO> Processes)
        {
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                if (Packing.ID <= 0) 
                {
                    using (var transaction = dbEntity.Database.BeginTransaction())
                    {
                        try
                        {
                            Packing.PackedQty = Items.Sum(a => a.PackedQty);
                            Packing.StartDate = Items[0].Date;
                            if (Packing.IsCompleted)
                            {
                                Packing.EndDate = Items[Items.Count() - 1].Date;
                            }
                            int PackingID = CreatePacking(dbEntity, Packing);
                            int PackingIssueID;
                            foreach (var item in Items)
                            {
                                PackingIssueID = CreatePackingIssue(dbEntity, item, PackingID);
                                foreach (var Material in Materials.Where(a => a.BatchTypeID == item.BatchTypeID || a.BatchTypeID == 0))
                                {
                                    CreatePackingIssueMaterial(dbEntity, Material, PackingIssueID,Packing);
                                }
                                foreach (var Process in Processes.Where(a => a.BatchTypeID == item.BatchTypeID))
                                {
                                    CreatePackingIssueProcess(dbEntity, Process, PackingIssueID);
                                }
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch (OutofStockException e)
                        {
                            transaction.Rollback();
                            throw e;
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw e;
                        }
                    }
                }
                else
                {
                    using (var transaction = dbEntity.Database.BeginTransaction())
                    {
                        try
                        {
                            Packing.PackedQty = Items.Sum(a => a.PackedQty);
                            if (Packing.IsCompleted)
                            {
                                Packing.EndDate = Items[Items.Count() - 1].Date;
                            }
                            //PackingBO TempPacking = GetProductionPacking(Packing.ID).FirstOrDefault();
                            UpdateProductionPacking(dbEntity, Packing);
                            foreach (var item in Items)
                            {
                                //if (item.PackingIssueID == 0)
                                //{
                                //    item.PackingIssueID = CreatePackingIssue(dbEntity, item, Packing.ID);
                                //    foreach (var Material in Materials.Where(a => a.BatchTypeID == item.BatchTypeID && a.PackingIssueID == 0))
                                //    {
                                //        CreatePackingIssueMaterial(dbEntity, Material, item.PackingIssueID, Packing);
                                //    }
                                //    foreach (var Process in Processes.Where(a => a.BatchTypeID == item.BatchTypeID && a.PackingIssueID == 0))
                                //    {
                                //        CreatePackingIssueProcess(dbEntity, Process, item.PackingIssueID);
                                //    }
                                //}
                                //else
                                //{
                                    //if (TempPacking.IsDraft)
                                    //{
                                        item.PackingIssueID = CreatePackingIssue(dbEntity, item, Packing.ID);
                                        foreach (var Material in Materials.Where(a => a.BatchTypeID == item.BatchTypeID || a.PackingIssueID == 0))
                                        
                                        {
                                            CreatePackingIssueMaterial(dbEntity, Material, item.PackingIssueID,Packing);
                                        }
                                        foreach (var Process in Processes.Where(a => a.BatchTypeID == item.BatchTypeID || a.PackingIssueID == 0))
                                        {
                                            CreatePackingIssueProcess(dbEntity, Process, item.PackingIssueID);
                                        }
                                    //}
                                //}

                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (OutofStockException e)
                        {
                            transaction.Rollback();
                            throw e;
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw e;
                        }
                    }
                }
            }
        }
        public List<PackingOutputBO> GetProductionPackingOutput(int PackingID)
        {
            List<PackingOutputBO> Output = new List<PackingOutputBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    Output = dbEntity.SpPGetProductionPackingOutput(
                        PackingID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new PackingOutputBO()
                        {
                            PackingIssueID = a.PackingIssueID,
                            ItemID = a.ItemID,
                            ItemName = a.ItemName,
                            UnitID = a.UnitID,
                            UnitName = a.Unit,
                            BatchID = a.BatchID,
                            BatchNo = a.BatchNo,
                            PackedQty = (decimal)a.PackedQty,
                            BatchType = a.BatchType,
                            BatchTypeID = (int)a.BatchTypeID,
                            IsDraft = (bool)a.IsDraft,
                            IsQCCompleted = (bool)a.IsQCCompleted,
                            Date = a.IssueDate
                        }
                   ).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return Output;
        }
        public List<PackingOutputBO> GetPackingOutputByBatchID(int BatchID)
        {
            List<PackingOutputBO> Output = new List<PackingOutputBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    Output = dbEntity.SpPGetProductionPackingOutputByBatchID(
                        BatchID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new PackingOutputBO()
                        {
                            PackingIssueID = a.PackingIssueID,
                            ItemID = a.ItemID,
                            ItemName = a.ItemName,
                            UnitID = a.UnitID,
                            UnitName = a.Unit,
                            BatchID = a.BatchID,
                            BatchNo = a.BatchNo,
                            PackedQty = (decimal)a.PackedQty,
                            BatchType = a.BatchType,
                            BatchTypeID = (int)a.BatchTypeID,
                            IsDraft = (bool)a.IsDraft,
                            IsQCCompleted = (bool)a.IsQCCompleted,
                            Date = a.IssueDate
                        }
                   ).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return Output;
        }

        public List<PackingOutputBO> GetProductionPackingOutput(int ItemID, int BatchID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpPGetProductionPackingByProductionBatch(
                        ItemID,
                        BatchID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new PackingOutputBO()
                        {
                            ItemName = a.ItemName,
                            BatchNo = a.BatchNo,
                            PackedQty = (decimal)a.PackedQty,
                            BatchType = a.BatchType,
                            BatchTypeID = (int)a.BatchTypeID,
                            ItemID = a.ItemID,
                            BatchID = a.BatchID,
                            Date = (DateTime)a.TransDate,

                        }
                   ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public int CancelPacking(int PackingID, string Table)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    dbEntity.SpCancelTransaction(PackingID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public List<PackingProcessBO> GetPackingProcess(int ItemID, int ProductGroupID, int BatchTypeID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpPGetPackingProcesses(

                        ProductGroupID,
                        1,
                        ItemID,
                        BatchTypeID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new PackingProcessBO()
                        {
                            Stage = a.Stage,
                            ProcessName = a.ProcessName,
                            UnSkilledLabourStandard = (decimal)a.UnSkilledLabourHours,
                            SkilledLaboursStandard = (decimal)a.SkilledLabourHours,
                            MachineHoursStandard = (decimal)a.MachineHours,
                            PackingProcessDefinitionTransID = a.PackingProcessDefinitionTransID,
                            BatchSize = (decimal)a.BatchSize
                        }
                   ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public DatatableResultBO GetPackingList(string Type, string TransNoHint, string TransDateHint, string ProductionGroupHint, string ItemNameHint, string BatchNoHint, string PackedQtyHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetPackingList(Type, TransNoHint, TransDateHint, ProductionGroupHint, ItemNameHint, BatchNoHint, PackedQtyHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                ProductionGroupName = item.ProductionGroupName,
                                ItemName = item.ItemName,
                                PackingQty = item.PackedQty,
                                ItemID = item.ItemID,
                                IsDraft = (bool)item.IsDraft,
                                IsCancelled = (bool)item.IsCancelled,
                                BatchNo = item.BatchNo,
                                Status = item.Status
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

    }
}