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
    public class RepackingDAL
    {

        public List<RepackingBO> GetPackingMaterials(int ItemID, int ProductGroupID, int BatchTypeID, int StoreID, int IssueItemID, decimal QuantityIn, decimal PackedQty, int IssueBatchTypeID, int BatchID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpPGetRePackingMaterials(
                        ItemID,
                        ProductGroupID,
                        BatchTypeID,
                        StoreID,
                        IssueItemID,
                        QuantityIn,
                        PackedQty,
                        IssueBatchTypeID,
                        BatchID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new RepackingBO()
                        {
                            Code = a.ItemCode,
                            Name = a.ItemName,
                            ItemID = a.ItemID,
                            Unit = a.Unit,
                            UnitID = a.UnitID,
                            StandardQty = (decimal)a.StandardQty,
                            AvailableStock = (decimal)a.AvailableStock,
                            ActualQty = (decimal)a.ActualQty,
                            BatchType = a.BatchType,
                            ReceiptItemBatchTypeID = (int)a.BatchTypeID
                        }
                   ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<RepackingBO> GetPackingProcess(int ItemID, int ProductGroupID, int BatchTypeID)
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
                        ).Select(a => new RepackingBO()
                        {
                            Stage = a.Stage,
                            ProcessName = a.ProcessName,
                            UnSkilledLabourStandard = (decimal)a.UnSkilledLabourHours,
                            SkilledLaboursStandard = (decimal)a.SkilledLabourHours,
                            MachineHoursStandard = (decimal)a.MachineHours
                        }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Save(RepackingBO repacking, List<ProductionRePackingMaterialItemBO> Materials, List<ProductionREPackingProcesItemBO> Processes, List<RepakingPackingOutputBO> Output)
        {
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        repacking.RepackedQuantity = Output.Sum(a => a.RepackedQuantity);
                        int repakingIssueID = createRepakingIssue(dbEntity, repacking);
                        foreach (var Out in Output)
                        {
                            int repakingreceiptID = createRepakingReceipt(dbEntity, Out, repakingIssueID, repacking);

                            foreach (var Material in Materials)
                            {
                                CreatRepackingMaterials(dbEntity, repacking, Material, repakingreceiptID);
                            }
                            foreach (var process in Processes)
                            {
                                CreatRepackingProcesses(dbEntity, process, repakingreceiptID);
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
        }

        public bool Update(RepackingBO repacking, List<ProductionRePackingMaterialItemBO> Materials, List<ProductionREPackingProcesItemBO> Processes, List<RepakingPackingOutputBO> Output)
        {
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        repacking.RepackedQuantity = Output.Sum(a => a.RepackedQuantity);
                        int repakingIssueID = UpdateRepackingIssue(dbEntity, repacking);
                        foreach (var Out in Output)
                        {
                            int repakingreceiptID = createRepakingReceipt(dbEntity, Out, repakingIssueID, repacking);

                            foreach (var Material in Materials)
                            {
                                CreatRepackingMaterials(dbEntity, repacking, Material, repakingreceiptID);
                            }
                            foreach (var process in Processes)
                            {
                                CreatRepackingProcesses(dbEntity, process, repakingreceiptID);
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
        }

        public int createRepakingIssue(ProductionEntities dbEntity, RepackingBO repacking)
        {
            try
            {
                string FormName = "Repacking";
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                ObjectParameter RepakingIssueID = new ObjectParameter("RepakingIssueID", typeof(int));

                if (repacking.IsDraft)
                {
                    FormName = "DraftRepacking";
                }
                var i = dbEntity.SpUpdateSerialNo(
                                        FormName,
                                        "Code",
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        SerialNo);
                var j = dbEntity.SpCreateRepakingIssue(
                    SerialNo.Value.ToString(),
                    repacking.RepackingDate,
                    repacking.IssueItemID,
                    repacking.IsuueItemBatchID,
                    repacking.IsuueItemBatchTypeID,
                    repacking.ReceiptItemBatchTypeID,
                    repacking.ReceiptItemID,
                    repacking.QuantityIn,
                    repacking.QuantityOut,
                    repacking.Isprocessed,
                    repacking.IsDraft,
                    DateTime.Now,
                    GeneralBO.CreatedUserID,
                    repacking.StandardQty,
                    repacking.Remark,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID,
                    RepakingIssueID
                    );
                return Convert.ToInt32(RepakingIssueID.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateRepackingIssue(ProductionEntities dbEntity, RepackingBO repacking)
        {
            try
            {

                var j = dbEntity.SpUpdateRepackingIssue(
                    repacking.ID,
                    repacking.RepackingDate,
                    repacking.IsuueItemBatchTypeID,
                    repacking.QuantityIn,
                    repacking.ReceiptItemBatchTypeID,
                    repacking.QuantityOut,
                    repacking.IsDraft,
                    DateTime.Now,
                    GeneralBO.CreatedUserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID

                    );
                return Convert.ToInt32(repacking.ID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int createRepakingReceipt(ProductionEntities dbEntity, RepakingPackingOutputBO Output, int RepakingIssueID, RepackingBO repacking)
        {
            try
            {
                ObjectParameter RepakingReceiptID = new ObjectParameter("RepakingReceiptID", typeof(int));
                var j = dbEntity.SpCreateRepackingReceipt(
                    RepakingIssueID,
                    Output.ItemID,
                    Output.BatchID,
                    Output.BatchTypeID,
                    repacking.IsuueItemBatchTypeID,
                    Output.QuantityOut,
                    GeneralBO.CreatedUserID,
                    DateTime.Now,
                    Output.IsQCCompleted,
                    repacking.IssueItemID,
                    repacking.QuantityIn,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID,
                    Output.Date,
                    RepakingReceiptID
                    );
                return Convert.ToInt32(RepakingReceiptID.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreatRepackingMaterials(ProductionEntities dbEntity, RepackingBO Repacking, ProductionRePackingMaterialItemBO Materials, int repakingreceiptID)
        {
            try
            {
                ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                dbEntity.SpCreatRepackingMaterials(
                    repakingreceiptID,
                    Materials.ItemID,
                    Materials.UnitID,
                    Materials.BatchTypeID,
                    Materials.IssueQty,
                    Materials.StandardQty,
                    GeneralBO.CreatedUserID,
                    Repacking.RepackingDate,
                    Materials.StandardQtyForStdBatch,
                    Materials.Variance,
                     Materials.IsMaterialReturn,
                    Materials.IsAdditionalIssue,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID,
                    Repacking.IsDraft,
                    RetValue
                    );
                if (Convert.ToInt16(RetValue.Value) == 0)
                {
                    throw new OutofStockException("Item Out of stock");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreatRepackingProcesses(ProductionEntities dbEntity, ProductionREPackingProcesItemBO process, int repakingreceiptID)
        {
            try
            {
                dbEntity.SpCreatRepackingProcess(
                repakingreceiptID,
                process.Stage,
                process.ProcessName,
                process.StartTime,
                process.EndTime,
                process.SkilledLaboursStandard,
                process.SkilledLaboursActual,
                process.UnSkilledLabourStandard,
                process.UnSkilledLabourActual,
                process.MachineHoursStandard,
                process.MachineHoursActual,
                process.Status,
                process.Remarks,
                process.DoneBy,
                process.BatchTypeID,
                process.AverageProcessCost,
                GeneralBO.CreatedUserID,
                DateTime.Now,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<RepackingBO> GetRepackingIssue(int RepakingID)
        {
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                try
                {
                    return dEntity.SpGetRepackingIssue(RepakingID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new RepackingBO
                    {
                        RepackingNo = a.RepackingNo,
                        RepackingDate = (DateTime)a.RepackingDate,
                        ItemName = a.ItemIssue,
                        IssueItemID = a.IssueItemID,
                        BatchNo = a.IssueBatchNO,
                        BatchType = a.IssueBatchType,
                        ReceiptItemBatchTypeID = a.ReceiptBatchTypeID,
                        QuantityOut = (decimal)a.QuantityOut,
                        IsuueItemBatchTypeID = a.BatchTypeID,
                        ReceiptItemBatchType = a.ReceiptBatchType,
                        QuantityIn = (decimal)a.Quantity,
                        Isprocessed = (bool)a.IsProcessed,
                        IsCancelled = (bool)a.IsCancelled,
                        IsDraft = (bool)a.IsDraft,
                        ID = a.ID,
                        ItemReceipt = a.ReceiptItem,
                        ReceiptItemID = a.ReceiptItemID,
                        StandardQty = (decimal)a.StandardQty,
                        Remark = a.Remark,
                        IssueConversionFactorP2S = (decimal)a.IssueConversionFactorPtoS,
                        ReceiptConversionFactorP2S = (decimal)a.ReceiptConversionFactorPtoS


                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<ProductionRePackingMaterialItemBO> GetRepackingMaterials(int RepakingID)
        {
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                try
                {
                    return dEntity.SpGetRepackingMaterialsDetails(RepakingID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ProductionRePackingMaterialItemBO
                    {

                        ItemName = a.ItemName,
                        UOM = a.Unit,
                        BatchType = a.BatchType,
                        ActualQty = (decimal)a.StdQtyForActualBatch,
                        StandardQtyForStdBatch = (decimal)a.StdQtyForStdBatch,
                        IssueQty = (decimal)a.Quantity,
                        Variance = (decimal)a.Variance,
                        ItemCode = a.ItemCode,
                        AvailableStock = (decimal)a.AvailableStock,
                        UnitID = (int)a.unitID,
                        ItemID = a.ItemID,
                        BatchTypeID = (int)a.BatchTypeID,
                        IsMaterialReturn = (bool)a.IsMaterialReturn,
                        IsAdditionalIssue = (bool)a.IsAdditionalIssue
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<ProductionREPackingProcesItemBO> GetRepackingProcess(int RepakingID)
        {
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                try
                {
                    return dEntity.SpGetRepackingProcess(RepakingID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ProductionREPackingProcesItemBO
                    {
                        RepackingReceiptID = (int)a.RepackingReceiptID,
                        Stage = a.Stage,

                        ProcessName = a.ProcessName,
                        StartTime = (DateTime)a.StartTime,
                        EndTime = (DateTime)a.EndTime,
                        SkilledLaboursStandard = (decimal)a.SkilledLabourStandardTime,
                        SkilledLaboursActual = (decimal)a.SkilledLabourActualTime,
                        UnSkilledLaboursActual = (decimal)a.UnSkilledLabourActualTime,
                        UnSkilledLabourStandard = (decimal)a.UnSkilledLabourStandardTime,
                        MachineHoursActual = (decimal)a.MachineHourActual,
                        MachineHoursStandard = (decimal)a.MachineHourStandard,
                        DoneBy = a.DoneBY,
                        Status = a.ProcessStatus,
                        PackingIssueID = a.PackingIssueID,
                        BatchTypeID = (int)a.BatchTypeID


                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<RepakingPackingOutputBO> GetRepackingOutput(int RepakingID)
        {
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                try
                {
                    return dEntity.SpGetRepackingOutput(RepakingID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new RepakingPackingOutputBO
                    {
                        ItemName = a.ItemName,
                        PackedQty = (decimal)a.OutputQuantity,
                        BatchType = a.BatchType,
                        BatchNo = a.BatchNo,
                        IsQCCompleted = (bool)a.QC,
                        Date = (DateTime)a.RepackingDate,
                        BatchTypeID = a.BatchTypeID,
                        BatchID = a.BatchID,
                        RepackingIssueID = (int)a.RepackingIssueID,
                        ItemID = a.ItemID
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<RepakingPackingOutputBO> GetRepackingReceipt(int RepakingID)
        {
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                try
                {
                    return dEntity.SpGetRepackingReceipt(RepakingID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new RepakingPackingOutputBO
                    {
                        //ReppackingIssueID = a.ReppackingIssueID,
                        //ItemID = a.ID,
                        //ItemName = a.ItemName,
                        //BatchNo = a.
                        //PackedQty = a.
                        //BatchType = a.
                        //BatchTypeID = a.
                        //BatchID = a.
                        //StoreID = a.
                        //UnitID = a.
                        //ProductionSequence = a.
                        //IsDraft = a.
                        //IsCompleted = a.
                        //IsQCCompleted = a.

                        //Date = a.
                        //RepackedQuantity = a.

                    }).ToList();

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public List<RepackingBO> GetRepakingList(int ID)
        {

            try
            {
                using (ProductionEntities dEntity = new ProductionEntities())
                {
                    return dEntity.SpGetRepacking(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new RepackingBO()
                    {
                        RepackingNo = m.RepackingNo,
                        RepackingDate = (DateTime)m.repackingDate,
                        QuantityIn = (decimal)m.Quantity,
                        BatchNo = m.BatchNo,
                        BatchType = m.BatchType,
                        ItemName = m.ItemName,
                        ID = m.ID,
                        IsCancelled = (bool)m.IsCancelled,
                        IsDraft = (bool)m.IsDraft
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public int Cancel(int ID, string Table)
        {
            ProductionEntities dEntity = new ProductionEntities();
            return dEntity.SpCancelTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
        }

        public DatatableResultBO GetRepackingList(string Type, string TransNoHint, string TransDateHint, string ItemNameHint, string BatchNoHint, string BatchTypeHint, string QuantityInHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetRepackingList(Type, TransNoHint, TransDateHint, ItemNameHint, BatchNoHint, BatchTypeHint, QuantityInHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                RepackingNo = item.RepackingNo,
                                repackingDate = ((DateTime)item.repackingDate).ToString("dd-MMM-yyyy"),
                                ItemName = item.ItemName,
                                BatchNo = item.BatchNo,
                                BatchType = item.BatchType,
                                Quantity = item.Quantity,
                                Status = item.Status,

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
