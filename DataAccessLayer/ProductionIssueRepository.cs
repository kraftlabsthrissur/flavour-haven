using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductionIssueRepository : IProductionIssue
    {


        /// <summary>
        /// Get Material Prodction issue
        /// </summary>
        /// <param name="productionGroupID"></param>
        /// <param name="productionSequence"></param>
        /// <param name="itemID"></param>

        public List<MaterialProductionIssueBO> GetProductionIssueMaterials(int productionGroupID, int productionSequence, int itemID, int ProductID)
        {
            List<MaterialProductionIssueBO> boList;
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    boList = dbEntity.SpGetMaterialsForProductionIssue(productionGroupID, productionSequence, itemID, ProductID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MaterialProductionIssueBO
                    {
                        ProductionSequence = productionSequence,
                        RawMaterialId = a.RawMaterialId ?? 0,
                        RawMaterialName = a.RawMaterialName,
                        RawMaterialQty = a.RawMaterialQty ?? 0,
                        StandardQty = a.RawMaterialQty ?? 0,
                        RawMaterialUnitID = a.RawMaterialUnitID ?? 0,
                        UOM = a.UOM,
                        QOM = a.QOM,
                        UnitName = a.UnitName,
                        Stock = (decimal)a.Stock,
                        BatchID = a.BatchID,
                        IsQcRequired = a.IsQCRequired,
                        ProductDefinitionTransID = a.ProductDefinitionTransID,
                        IsAdditionalIssue = false,
                        Category = a.CategoryName,
                        ActualOutPutForStdBatch = (decimal)a.ActualOutPutForStdBatch,
                        IsSubProduct = (bool)a.IsSubproduct,
                        BatchTypeID=a.BatchTypeID
                        // Batches = GetBatchWithStocks(a.RawMaterialId ?? 0).Select(x => new BatchBO { ID = x.BatchID, ItemID = x.ItemID, BatchNo = x.BatchNo, Stock = x.Stock }).ToList()    //For each and every materials the batch will be varied. Need to change infuture toavoid performance. Instead of loading batches for each and every time, get all the available batches and filter it here
                    }).ToList();
                    return boList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Process Production Issue
        /// </summary>
        /// <param name="productionGroupID"></param>
        /// <param name="productionSequence"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public List<ProcessProductionIssueBO> GetProductionIssueProcesses(int productionGroupID, int productionSequence, int itemID)
        {
            List<ProcessProductionIssueBO> boList;
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    boList = dbEntity.SpGetProcessForProductionIssue(productionGroupID, productionSequence, itemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ProcessProductionIssueBO
                    {

                        Stage = a.Stage,
                        ProcessName = a.ProcessName,
                        UnSkilledLabourHours = a.UnSkilledLabourHours ?? 0,
                        SkilledLabourHours = a.SkilledLabourHours ?? 0,
                        MachineHours = a.MachineHours ?? 0,
                        ProductionSequence = productionSequence,
                        ProcessDefinitionTransID = a.ProcessDefinitionTransID

                    }).ToList();
                    return boList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Batch with Stocks. 
        /// Showing dropdown in Material Grid
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public List<BatchWiseStockBO> GetBatchWithStocks(int itemID)
        {
            List<BatchWiseStockBO> boList;
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    boList = dbEntity.SpGetBatchWithStock(itemID, 0, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchWiseStockBO
                    {

                        BatchID = a.BatchID,
                        BatchNo = a.BatchNo,
                        ItemID = a.ItemID ?? 0,
                        Stock = a.stock ?? 0

                    }).ToList();
                    return boList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the item with issue qty history. 
        /// When click on plus button on Material tab(ProductionIssue)
        /// </summary>
        public List<MaterialTransBO> GetMaterialTrans(int productionIssueMaterialsID)
        {
            List<MaterialTransBO> boList;
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    boList = dbEntity.SpGetProductionIssueMaterialTrans(productionIssueMaterialsID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MaterialTransBO
                    {

                        ID = a.ID,
                        ProductionIssueID = a.ProductionIssueID ?? 0,
                        ProductionIssueMaterialsID = a.ProductionIssueMaterialsID ?? 0,
                        ItemID = a.ItemID ?? 0,
                        ItemName = a.ItemName,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        BatchID = a.BatchID ?? 0,
                        BatchNo = a.BatchNo,
                        IssueQty = a.IssueQty ?? 0,
                        AverageRate = a.AverageRate ?? 0,
                        Remarks = a.Remarks,
                        IssueDate = a.IssueDate ?? new DateTime()

                    }).ToList();
                    return boList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create or Edit Production issue
        /// </summary>
        /// <param name="productionIssueBO"></param>
        /// <returns></returns>
        public int Save(ProductionIssueBO productionIssueBO)
        {
            if (productionIssueBO.ProductionID <= 0)
            {   //Create
                return Create(productionIssueBO);
            }
            else
            {       //Update
                if (IsProductionIssueEditable(productionIssueBO.ProductionID))
                {
                    Update(productionIssueBO);
                    return productionIssueBO.ProductionID;
                }
                else
                {
                    throw new NotEditableException("Production issue is not editable");
                }

            }
        }

        public bool IsProductionIssueEditable(int productionID)
        {
            ProductionIssueBO productionIssue = GetProductionIssue(productionID);
            if (productionIssue.IsAborted || (productionIssue.IsCompleted))
            {
                return false;
            }
            return true;
        }

        public void InsertProductionIssueMaterial(MaterialProductionIssueBO Material, int productionIssueID, ProductionEntities dbEntity)
        {
            ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
            try
            {
                dbEntity.SpPCreateProductionIssueMaterials(
                    productionIssueID,
                    Material.RawMaterialId,
                    Material.ProductDefinitionTransID,
                    Material.StandardQty,
                    Material.ActualStandardQty,
                    Material.IssueQty,
                    Material.Variance,
                    Material.BatchNo,
                    Material.BatchID,
                    Material.WareHouseID,
                    Material.IssueDate,
                    Material.Remarks,
                    Material.IsCompleted,
                    Material.AverageRate,
                    Material.IsAdditionalIssue,
                    Material.BatchTypeID,
                    GeneralBO.CreatedUserID,
                    Material.RawMaterialUnitID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID,
                    RetValue
                    );
                if (Convert.ToInt16(RetValue.Value) == 0)
                {
                    throw new OutofStockException("Item out of stock");
                }
                if (Convert.ToInt16(RetValue.Value) == -1)
                {
                    throw new InvalidReturnException("Invalid Return Quantity");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void UpdateProductionIssueMaterial(MaterialProductionIssueBO Material, int productionIssueID, ProductionEntities dbEntity)
        {
            ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
            try
            {
                dbEntity.SpPUpdateProductionIssueMaterials(
                    Material.MaterialProductionIssueID,
                    productionIssueID,
                    Material.RawMaterialId,
                    Material.ProductDefinitionTransID,
                    Material.IssueQty,
                    Material.AdditionalIssueQty,
                    Material.Variance,
                    Material.WareHouseID,
                    Material.IssueDate,
                    Material.Remarks,
                    Material.IsCompleted,
                    Material.AverageRate,
                     GeneralBO.CreatedUserID,
                    Material.RawMaterialUnitID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID,
                    RetValue
                    );
                if (Convert.ToInt16(RetValue.Value) == 0)
                {
                    throw new OutofStockException("Out of Stock item");
                }
                if (Convert.ToInt16(RetValue.Value) == -1)
                {
                    throw new InvalidReturnException("Invalid Return Quantity");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void InsertProductionProcess(ProcessProductionIssueBO Process, int productionIssueID, ProductionEntities dbEntity)
        {
            dbEntity.SpCreateProductionIssueProcess(
                productionIssueID,
                Process.ProcessDefinitionTransID,
                Process.Stage,
                Process.ProcessName,
                Process.StartTime,
                Process.EndTime,
                Process.SkilledLabourHours,
                Process.SkilledLabourActualHours,
                Process.UnSkilledLabourHours,
                Process.UnSkilledLabourActualHours,
                Process.UnSkilledLabourActualHours,
                Process.MachineHours,
                Process.MachineActualHours,
                Process.DoneBy,
                Process.Status,
                Process.Remarks,
                Process.AverageProcessCost,
                GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
            );
        }

        public void UpdateProductionProcess(ProcessProductionIssueBO Process, int productionIssueID, ProductionEntities dbEntity)
        {
            dbEntity.SpUpdateProductionIssueProcess(
                Process.ProcessProductionIssueID,
                Process.StartTime,
                Process.EndTime,
                Process.SkilledLabourActualHours,
                Process.UnSkilledLabourActualHours,
                Process.MachineActualHours,
                Process.Status,
                Process.DoneBy,
                Process.Remarks,
                Process.AverageProcessCost,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                );
        }

        public int CreateProduction(ProductionIssueBO productionIssueBO, ProductionEntities dbEntity)
        {
            ObjectParameter ProductionIDOut = new ObjectParameter("ProductionID", typeof(int));
            ObjectParameter serialNo = new ObjectParameter("SerialNo", typeof(string));
            dbEntity.SpUpdateSerialNo("ProductionIssue", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, serialNo);

            var production = dbEntity.SpCreateProduction(
                serialNo.Value.ToString(),
                DateTime.Now,
                productionIssueBO.ProductionGroupID,
                productionIssueBO.ProductionSequence,
                productionIssueBO.ProductionScheduleID,
                productionIssueBO.StartDate,
                productionIssueBO.StartTime,
                productionIssueBO.EndDate,
                productionIssueBO.EndTime,
                productionIssueBO.AverageCost,
                productionIssueBO.OutputQty,
                productionIssueBO.ProductionLocationID,
                productionIssueBO.IsDraft,
                productionIssueBO.IsCompleted,
                GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID,
                ProductionIDOut
                );

            return Convert.ToInt16(ProductionIDOut.Value);
        }

        /// <summary>
        /// Create new Production Issue
        /// </summary>
        /// <param name="productionIssueBO"></param>
        /// <returns></returns>
        private int Create(ProductionIssueBO productionIssueBO)
        {

            int productionID = 0;
            int productionIssueID = 0;

            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        productionID = CreateProduction(productionIssueBO, dbEntity);
                        if (productionIssueBO.ProductionScheduleID == 0)
                        {
                            ObjectParameter batchNo = new ObjectParameter("SerialNo", typeof(string));
                            if (productionIssueBO.IsKalkan)
                            {
                                dbEntity.SpUpdateSerialNo("BatchMaster", "Kalkan", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, batchNo);
                            }
                            else
                            {
                                dbEntity.SpUpdateSerialNo("BatchMaster", "Production", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, batchNo);
                            }
                            productionIssueBO.BatchNo = batchNo.Value.ToString();
                        }

                        foreach (var item in productionIssueBO.OutputBOList)
                        {
                            int BatchID = CreateBatch(item.ItemID, productionIssueBO.BatchNo, dbEntity);
                            item.ProductionStatus = productionIssueBO.IsCompleted ? "completed" : item.ProductionStatus;
                            productionIssueID = CreateProductionIssue(item, productionID, BatchID, productionIssueBO.ProductionLocationID, dbEntity);
                            if (productionIssueID > 0 && !item.IsSubProduct)
                            {
                                foreach (var Material in productionIssueBO.MaterialProductionIssueBOList.Where(x => x.ProductionSequence == item.ProductionSequence))
                                {
                                    Material.IsCompleted = item.ProductionStatus == "completed" || Material.IssueQty >= Material.StandardQty ? true : false;
                                    InsertProductionIssueMaterial(Material, productionIssueID, dbEntity);
                                }
                                foreach (var Process in productionIssueBO.ProcessProductionIssueBOList.Where(x => x.ProductionSequence == item.ProductionSequence))
                                {
                                    InsertProductionProcess(Process, productionIssueID, dbEntity);
                                }
                            }
                        }
                        dbEntity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (OutofStockException e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    catch (InvalidReturnException e)
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
            return productionID;
        }

        public int CreateProductionIssue(ProductionOutputBO item, int ProductionID, int BatchID, int ProductionLocationID, ProductionEntities dbEntity)
        {
            ObjectParameter productionIssueIDOut = new ObjectParameter("ProductionIssueID", typeof(int));
            dbEntity.SpPCreateProductionIssue(
                ProductionID,
                item.ProductionSequence,
                item.ItemID,
                BatchID,
                item.StandardBatchSize,
                item.ActualBatchSize,
                item.StandardOutput,
                item.ActualOutput,
                item.Variance,
                item.StartDate,
                item.StartTime,
                item.EndDate,
                item.EndTime,
                ProductionLocationID,
                item.ReceiptStoreID,
                item.ProductionStatus == "started" ? false : true,
                item.ProductionStatus,
                false,
                0,
                GeneralBO.CreatedUserID,
                "",
                true,
                item.IsSubProduct,
                item.ProcessStage,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID,
                productionIssueIDOut
                );
            return (productionIssueIDOut != null && productionIssueIDOut.Value != null) ? Convert.ToInt32(productionIssueIDOut.Value) : 0;
        }

        public void UpdateProductionIssue(ProductionOutputBO item, ProductionEntities dbEntity)
        {
            dbEntity.SpPUpdateProductionIssue(
                item.ProductionIssueID,
                item.StandardOutput,
                item.ActualOutput,
                item.Variance,
                item.ReceiptStoreID,
                item.EndDate,
                item.EndTime,
                item.StartTime,
                true,
                item.ProductionStatus == "started" ? false : true,
                item.ProductionStatus,
                item.IsAborted,
                0,
                GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                );
        }

        public int CreateBatch(int ItemID, string BatchNo, ProductionEntities dbEntity)
        {
            ObjectParameter BatchIDOut = new ObjectParameter("BatchID", typeof(string));
            dbEntity.SpCreateBatch(
                "RM",
                ItemID,

                BatchNo,
                BatchNo,
                DateTime.Now,
                DateTime.Now.AddYears(1),
                0,
                GeneralBO.CreatedUserID,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID,
                BatchIDOut
                );
            return Convert.ToInt16(BatchIDOut.Value);
        }

        private bool Update(ProductionIssueBO productionIssueBO)
        {
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        if (productionIssueBO.IsCompleted || productionIssueBO.IsAborted)
                        {
                            productionIssueBO.EndDate = DateTime.Now;
                            productionIssueBO.EndTime = DateTime.Now;
                        }
                        dbEntity.SpPUpdateProduction(
                            productionIssueBO.ProductionID,
                            productionIssueBO.ProductionSequence,
                            null,
                            null,
                            productionIssueBO.IsDraft,
                            productionIssueBO.IsCompleted,
                            productionIssueBO.IsAborted,
                            productionIssueBO.ProductionStatus,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );

                        foreach (var item in productionIssueBO.OutputBOList)
                        {
                            if (item.ProductionIssueID > 0)
                            {
                                item.IsAborted = productionIssueBO.IsAborted;
                                item.ProductionStatus = productionIssueBO.IsCompleted ? "completed" : item.ProductionStatus;
                                UpdateProductionIssue(item, dbEntity);
                            }
                            else
                            {
                                int BatchID = CreateBatch(item.ItemID, productionIssueBO.BatchNo, dbEntity);
                                item.ProductionIssueID = CreateProductionIssue(item, productionIssueBO.ProductionID, BatchID, productionIssueBO.ProductionLocationID, dbEntity);
                            }

                            if (item.ProductionIssueID > 0 && !item.IsSubProduct)
                            {
                                foreach (var Material in productionIssueBO.MaterialProductionIssueBOList.Where(x => x.ProductionSequence == item.ProductionSequence))
                                {
                                    Material.IsCompleted = item.ProductionStatus == "completed" || Material.IssueQty >= Material.StandardQty ? true : false;
                                    if (Material.MaterialProductionIssueID > 0)
                                    {
                                        // if (Material.AdditionalIssueQty > 0)
                                        // {
                                        UpdateProductionIssueMaterial(Material, item.ProductionIssueID, dbEntity);
                                        //}
                                    }
                                    else
                                    {
                                        InsertProductionIssueMaterial(Material, item.ProductionIssueID, dbEntity);
                                    }
                                }

                                foreach (var Process in productionIssueBO.ProcessProductionIssueBOList.Where(x => x.ProductionSequence == item.ProductionSequence))
                                {
                                    if (Process.ProcessProductionIssueID > 0)
                                    {
                                        UpdateProductionProcess(Process, item.ProductionIssueID, dbEntity);
                                    }
                                    else
                                    {
                                        InsertProductionProcess(Process, item.ProductionIssueID, dbEntity);
                                    }
                                }
                            }
                        }
                        dbEntity.SaveChanges();
                        transaction.Commit();

                    }
                    catch (OutofStockException e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    catch (InvalidReturnException e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return true;
        }

        public List<ProductionIssueBO> GetProductionIssues(string Status)
        {

            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    bool isCompleted = Status == "completed" ? true : false;

                    return new List<ProductionIssueBO>();
                    //return dbEntity.SpGetProductionIssueList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, isCompleted).Select(x => new ProductionIssueBO
                    //{
                    //    TransNo = x.TransNo,
                    //    TransDate = x.TransDate,
                    //    ProductionGroupID = (int)x.ProductionGroupID,
                    //    ProductionGroupName = x.ProductionGroupName,
                    //    ProductionIssueID = x.ProductionIssueID,
                    //    StartDate = x.StartDate ?? new DateTime(),
                    //    BatchNo = x.BatchNo,
                    //    ProductionStageItem = x.ProductionStageItem,
                    //    Unit = x.Unit,
                    //    UnitID = x.UnitID,
                    //    ProductionSequence = x.ProductionSequence ?? 0,
                    //    ProcessStage = x.ProcessStage,
                    //    ActualBatchSize = x.ActualBatchSize ?? 0,
                    //    StandardBatchSize = x.StandardBatchSize ?? 0,
                    //    ProductionID = x.ProductionID,
                    //    IsCompleted = isCompleted
                    //}).ToList();


                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Production Issue ID
        /// </summary>
        /// <param name="productionIssueID"></param>
        /// <returns></returns>
        public ProductionIssueBO GetProductionIssue(int productionID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpGetProductionIssueDetails(productionID, 0, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ProductionIssueBO
                    {
                        ProductionID = a.ProductionID,
                        TransNo = a.TransNo,
                        TransDate = a.TransDate,
                        ProductionGroupName = a.ProductionGroupName,
                        ProductionScheduleName = a.ProductionScheduleName,
                        Unit = a.Unit,
                        BatchNo = a.BatchNo,
                        StandardBatchSize = (decimal)a.StandardBatchSize,
                        ActualBatchSize = (decimal)a.ActualBatchSize,
                        StartDate = a.StartDate,
                        StartTime = a.StartTime,
                        ProductionLocationID = (int)a.ProductionLocationID,
                        IsCompleted = (bool)a.IsCompleted,
                        ProductionStatus = a.ProductionStatus,
                        IsAborted = (bool)a.IsAborted,
                        AverageCost = a.AverageCost == null ? 0 : (decimal)a.AverageCost,
                        CreatedUserID = (int)a.CreatedUserID,
                        ProductionLocation = a.ProductionLocation,
                        ProductionSequence = (int)a.ProductionSequence,
                        ProductionGroupID = a.ProductionGroupID,
                        ProductionScheduleID = a.ProductionScheduleID ?? 0,
                        MaterialProductionIssueBOList = GetProductionIssueMaterials(productionID),        //Loading ProductionIssueMaterials
                        ProcessProductionIssueBOList = GetProductionIssueProcesses(productionID),           //Loading ProductionIssueProcesses
                        OutputBOList = GetProductionIssueOutput(productionID)
                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<ProductionOutputBO> GetProductionIssueOutput(int productionID)
        {
            List<ProductionOutputBO> Outputs = new List<ProductionOutputBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    Outputs = dbEntity.SpGetProductionIssueOutput(productionID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID)
                        .Select(a => new ProductionOutputBO
                        {
                            ProductionID = (int)a.ProductionID,
                            ProductionIssueID = a.ProductionIssueID,
                            ProductionSequence = (int)a.ProductionSequence,
                            ItemID = a.ItemID,
                            StandardBatchSize = (decimal)a.StandardBatchSize,
                            ActualBatchSize = (decimal)a.ActualBatchSize,
                            StandardOutput = (decimal)a.StandardOutput,
                            ActualOutput = (decimal)a.ActualOutput,
                            Variance = (decimal)a.Variance,
                            StartDate = a.StartDate,
                            StartTime = a.StartTime,
                            EndDate = a.EndDate,
                            EndTime = a.EndTime,
                            ReceiptStoreID = (int)a.ReceiptStoreID,
                            IsCompleted = (bool)a.IsCompleted,
                            ProductionStatus = a.ProductionStatus,
                            ItemName = a.ItemName,
                            ReceiptStore = a.ReceiptStore,
                            IsSubProduct = (bool)a.IsSubProduct,
                            ProcessStage = a.ProcessStage,
                            Unit = a.Unit
                        }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return Outputs;
        }

        /// <summary>
        /// Get ProductionIssue Materials
        /// </summary>
        /// <param name="productionID"></param>
        /// <returns></returns>
        public List<MaterialProductionIssueBO> GetProductionIssueMaterials(int productionID)
        {
            List<MaterialProductionIssueBO> Materials = new List<MaterialProductionIssueBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    Materials = dbEntity.SpGetProductionIssueMaterialDetails(productionID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MaterialProductionIssueBO
                    {
                        MaterialProductionIssueID = a.PIMID,
                        RawMaterialId = a.ItemID ?? 0,
                        RawMaterialName = a.ItemName,
                        UOM = a.Unit,
                        StandardQty = a.StandardQty ?? 0,
                        ActualStandardQty = a.ActualStandardQty ?? 0,
                        IssueQty = a.IssueQty ?? 0,
                        Variance = a.Variance ?? 0,
                        BatchID = a.BatchID ?? 0,
                        BatchNo = a.BatchNo,
                        StoreID = a.StoreID ?? 0,
                        Store = a.Store,
                        IssueDate = a.LastIssueDate,
                        Remarks = a.Remarks,
                        ProductionSequence = (int)a.ProductionSequence,
                        Stock = (decimal)a.Stock,
                        ProductDefinitionTransID = (int)a.ProductDefinitionTransID,
                        IsAdditionalIssue = (bool)a.IsAdditionalIssue,
                        RawMaterialUnitID = (int)a.UnitID,
                        ActualOutPutForStdBatch = (decimal)a.ActualOutPutForStdBatch,
                        Category = a.Category,
                        IsSubProduct = (bool)a.IsSubproduct,
                        BatchTypeID=a.BatchTypeID
                    }).ToList();
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return Materials;
        }

        /// <summary>
        /// Get Production Issue Processes
        /// </summary>
        public List<ProcessProductionIssueBO> GetProductionIssueProcesses(int productionID)
        {
            List<ProcessProductionIssueBO> Processes = new List<ProcessProductionIssueBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    Processes = dbEntity.SpGetProductionIssueProcessDetails(productionID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ProcessProductionIssueBO
                    {
                        ProcessProductionIssueID = a.PIPID,
                        ProductionIssueID = a.ProductionIssueID ?? 0,
                        Stage = a.Stage,
                        ProcessName = a.ProcessName,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        SkilledLabourHours = a.SkilledLabourStandardHour ?? 0,
                        SkilledLabourActualHours = a.SkilledLabourActualHour ?? 0,
                        UnSkilledLabourHours = a.UnSkilledLabourStandardHour ?? 0,
                        UnSkilledLabourActualHours = a.UnSkilledLabourActualHour ?? 0,
                        MachineHours = a.MachineHourStandard ?? 0,
                        MachineActualHours = a.MachineHourActual ?? 0,
                        Status = a.ProcessStatus,
                        DoneBy = a.DoneBy,
                        Remarks = a.Remarks,
                        AverageProcessCost = a.AverageProcessCost ?? 0,
                        CreateUserID = a.CreateUserID ?? 0,
                        CreatedDate = a.CreatedDate ?? new DateTime(),
                        ProductionSequence = (int)a.ProductionSequence,
                        ProcessDefinitionTransID = (int)a.ProcessDefinitionTransID
                    }).ToList();

                }
            }
            catch (Exception e)
            {
                throw;
            }
            return Processes;
        }

        public List<ProductionSequenceBO> GetProductionSequences(int ProductGroupID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpGetProductionSequences(ProductGroupID, GeneralBO.ApplicationID)
                        .Select(a => new ProductionSequenceBO
                        {
                            ItemID = (int)a.ItemID,
                            Name = a.Name,
                            ProductionSequence = (int)a.ProductionSequence,
                            ProcessStage = a.ProcessStage,
                            BatchSize = (decimal)a.BatchSize,
                            StandardOutput = (decimal)a.StandardOutput,
                            IsQCRequired = (bool)a.IsQCRequiredForProduction,
                            Unit = a.Unit

                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DatatableResultBO GetProductionIssueList(string Type, string TransNoHint, string TransDateHint, string ExpectedDateHint, string ProductionGroupNameHint, string BatchNoHint, string BatchsizeHint, string UnitHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetProductionIssueList(Type, TransNoHint, TransDateHint, ExpectedDateHint, ProductionGroupNameHint, BatchNoHint, BatchsizeHint, UnitHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ProductionID,
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                ActualBatchSize = item.ActualBatchSize ?? 0,
                                ProductionGroupName = item.ProductionGroupName,
                                Unit = item.Unit,
                                BatchNo = item.BatchNo,
                                Status = item.Status,
                                ExpectedDate = ((DateTime)item.ExpectedDate).ToString("dd-MMM-yyyy"),
                                StandardOutput = item.StandardOutput
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
