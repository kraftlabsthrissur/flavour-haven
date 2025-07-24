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
    public class BatchDAL
    {
        public List<SalesBatchBO> GetAvailableBatchesForSales(int ItemID, decimal OrderQty, string SalesOrderTransIDs, int WarehouseID, int CustomerID, int SchemeID, int UnitID, int ProformaInvoiceID)
        {
            List<SalesBatchBO> Batches = new List<SalesBatchBO>();
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    Batches = dbEntity.SpGetAvailableBatchesForSales(ItemID, OrderQty, SalesOrderTransIDs, WarehouseID, CustomerID, SchemeID, UnitID, ProformaInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesBatchBO()
                    {
                        BatchID = (int)a.BatchID,
                        BatchNo = a.BatchNo,
                        CustomBatchNo = a.CustomBatchNo,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchTypeName = a.BatchTypeName,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        Stock = (decimal)a.Stock,
                        Rate = (decimal)a.Rate,
                        Qty = (decimal)a.Qty,
                        OfferQty = (decimal)a.OfferQty,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                        SalesOrderNo = a.SalesOrderNo,
                        SalesOrderTransID = (int)a.SalesOrderTransID,
                        SalesOrderID = (int)a.SalesOrderID
                    }).ToList();
                    return Batches;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<BatchBO> GetAvailableBatchesForSales(int ItemID, string FullOrLoose, int WarehouseID, int ItemCategoryID, int PriceListID)
        {
            List<BatchBO> Batches = new List<BatchBO>();

            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    Batches = dbEntity.SpGetAvailableBatches(
                        ItemID,
                        FullOrLoose,
                        WarehouseID,
                        ItemCategoryID,
                        PriceListID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).Select(
                        a => new BatchBO
                        {
                            BatchNo = a.BatchNo,
                            ID = a.BatchID,
                            CustomBatchNo = a.CustomBatchNo,
                            Stock = (decimal)a.Stock,
                            ExpiryDate = (DateTime)a.ExpiryDate,
                            Rate = a.Rate
                        }
                        ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Batches;
        }

        public List<StockIssueBatchBO> GetAvailableBatchesForStockIssue(int ItemID, decimal RequiredQty, int WarehouseID, string RequestTransIDs, int BatchTypeID, int UnitID, int StockIssueID)
        {
            List<StockIssueBatchBO> Batches = new List<StockIssueBatchBO>();

            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    Batches = dbEntity.SpGetAvailableBatchesForStockIssue(
                        ItemID,
                        RequiredQty,
                        StockIssueID,
                        WarehouseID,
                        RequestTransIDs,
                        BatchTypeID,
                        UnitID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).Select(
                        a => new StockIssueBatchBO
                        {
                            BatchNo = a.BatchNo,
                            BatchTypeName = a.BatchTypeName,
                            ID = a.BatchID,
                            CustomBatchNo = a.CustomBatchNo,
                            Stock = (decimal)a.Stock,
                            ExpiryDate = (DateTime)a.ExpiryDate,
                            Rate = (decimal)a.Rate,
                            IssueQty = (decimal)a.IssueQty,
                            StockRequisitionID = a.StockRequisitionID.HasValue ? (int)a.StockRequisitionID : 0,
                            StockRequisitionTransID = a.StockRequisitionTransID.HasValue ? (int)a.StockRequisitionTransID : 0,
                            StockRequisitionNo = a.StockRequisitionNo,

                        }
                        ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Batches;
        }

        public List<BatchBO> GetBatchList(int ItemID, int StoreID)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetBatchWithStock(ItemID, StoreID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    BatchNo = a.BatchNo,
                    ID = a.BatchID,
                }).ToList();
                return Batch;
            }
        }
        public List<BatchBO> GetBatchBatchTypeWise(int ItemID, int StoreID, int BatchTypeID)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetBatchWithStockBatchTypeWise(ItemID, StoreID, BatchTypeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    BatchNo = a.BatchNo,
                    ID = a.BatchID,
                }).ToList();
                return Batch;
            }
        }

        public List<BatchBO> GetBatchesBatchTypeWise(int ItemID, int BatchTypeID)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.GetbatchesBatchtypewise(ItemID, BatchTypeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    BatchNo = a.BatchNo,
                    ID = a.ID,
                }).ToList();
                return Batch;
            }

        }
        public decimal GetBatchWiseStock(int BatchID, int StoreID)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                var result = dEntity.SpGetBatchWiseStock(BatchID, StoreID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault();
                if (result == null)
                {
                    return 0;
                }
                return (decimal)result;
            }
        }
        public decimal GetBatchWiseStockForPackingSemiFinishedGood(int BatchID, int StoreID, int ProductionGroupID)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                var result = dEntity.SpGetBatchWiseStockForPackingSemiFinishedGood(BatchID, StoreID, ProductionGroupID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault();
                if (result == null)
                {
                    return 0;
                }
                return (decimal)result;
            }
        }
        public List<SalesBatchBO> GetBatchesByItemIDForCounterSales(int ItemID, int WarehouseID, int BatchTypeID, int UnitID, decimal Qty)
        {
            List<SalesBatchBO> list = new List<SalesBatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetBatchesByItemIDForCounterSales(ItemID, WarehouseID, BatchTypeID, UnitID, Qty, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new SalesBatchBO
                {
                    ItemID = m.ItemID,
                    BatchID = m.BatchID,
                    BatchTypeID = (int)m.BatchTypeID,
                    BatchNo = m.BatchNo,
                    CGSTPercentage = (decimal)m.CGSTPercent,
                    SGSTPercentage = (decimal)m.SGSTPercent,
                    IGSTPercentage = (decimal)m.IGSTPercent,
                    Stock = (decimal)m.stock,
                    Unit = m.Unit,
                    UnitID = (int)m.UnitID,
                    FullRate = (decimal)m.FullPrice,
                    LooseRate = (decimal)m.LoosePrice,
                    BatchType = m.BatchType,
                    ExpiryDate = (DateTime)m.ExpiryDate,
                    Code = m.ItemCode,
                    SalesUnitID = (int)m.SalesUnitID,
                    CessPercentage = (decimal)m.CessPercentage,
                    IsGSTRegisteredLocation = m.IsGSTRegisteredLocation

                }).ToList();
                return list;
            }
        }

        public DatatableResultBO GetAllBatchList(string BatchNoHint, string CustomBatchNoHint, string ItemNameHint, string ItemCategoryHint, string RetailMRPHint,  string BasePriceHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetBatchList(BatchNoHint, CustomBatchNoHint, ItemNameHint, ItemCategoryHint,RetailMRPHint,  BasePriceHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                BatchNo = item.BatchNo,
                                ItemName = item.ItemName,
                                ItemType = item.ItemType,
                                BatchType = item.BatchType,
                                StartDate = ((DateTime)item.ManufacturingDate).ToString("dd-MMM-yyyy"),
                                ExpiryDate = ((DateTime)item.ExpiryDate).ToString("dd-MMM-yyy"),
                                ISKPrice = item.ISKPrice,
                                ItemCategory = item.ItemCategory,
                                RetailMRP = item.RetailMRP,
                                SalesCategory=item.SalesCategory,
                                NetProfitRatio=item.NetProfitRatio
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

        public BatchBO GetbatchDetails(int BatchID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetBatchDetail(BatchID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                    {
                        ID = a.ID,
                        BatchNo = a.BatchNo,
                        CustomBatchNo = a.CustomBatchNo,
                        Name = a.ItemName,
                        ItemType = a.ItemType,
                        BatchTypeName = a.BatchType,
                        ManufacturingDate = (DateTime)a.ManufacturingDate,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        ISKPrice = (decimal)a.ISKPrice,
                        OSKPrice = (decimal)a.OSKPrice,
                        ExportPrice = (decimal)a.ExportPrice,
                        PurchaseMRP = (decimal)a.PurchaseMRP,
                        PurchaseLooseRate = (decimal)a.PurchaseLooseRate,
                        RetailMRP = (decimal)a.RetailMRP,
                        RetailLooseRate = (decimal)a.RetailLooseRate,
                        PackSize=(decimal)a.PackSize,
                        UnitID=(int)a.UnitID
                    }
                      ).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(BatchBO batchBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateBatch(
                        batchBO.ID,
                        batchBO.ItemID,
                        batchBO.ExpiryDate,
                        batchBO.ISKPrice,
                        batchBO.OSKPrice,
                        batchBO.ExportPrice,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int UpdateBatch(BatchBO batchBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateBatchDetails(
                        batchBO.ID,
                        batchBO.ItemID,
                        batchBO.ExpiryDate,
                        batchBO.ISKPrice,
                        batchBO.OSKPrice,
                        batchBO.ExportPrice,
                        batchBO.ISKPrice,
                        batchBO.RetailLooseRate,
                        batchBO.BatchRate,
                        batchBO.PurchaseLooseRate,
                        batchBO.ManufacturingDate,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int EditBatchInvoice(BatchBO Batch, List<PreviousBatchBO> PreviousBatch)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                foreach (var item in PreviousBatch)
                {
                    dbEntity.SpUpdateBatchInvoices(
                      item.InvoiceID,
                      Batch.ID,
                      Batch.UnitID,
                      Batch.RetailMRP,
                      Batch.RetailLooseRate,
                      item.Quantity,
                      item.OfferQty,
                      item.InvoiceRate,item.PurchasePrice,item.PurchaseLooseRate,
                      item.SalesRate,
                      item.LooseQty,
                      item.CGSTAmt,
                      item.SGSTAmt,
                      item.DiscountID,item.DiscountPercent,
                      item.Discount,
                      item.ProfitRatio,
                      item.GrossAmount,
                      item.NetAmount,
                      GeneralBO.FinYear,
                      GeneralBO.LocationID,
                      GeneralBO.ApplicationID
                       );
                }
              
                
            }
            return 1;

        }

        public int Save(BatchBO batchBO)
        {
            ObjectParameter IsExist = new ObjectParameter("IsExist", typeof(bool));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpSaveBatch(
                       batchBO.ItemID,
                       batchBO.BatchNo,
                       batchBO.CustomBatchNo,
                       batchBO.CreatedDate,
                       batchBO.ExpiryDate,
                       batchBO.ISKPrice,
                       batchBO.OSKPrice,
                       batchBO.ExportPrice,
                       GeneralBO.CreatedUserID,
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID,
                       IsExist);

                if (Convert.ToBoolean(IsExist.Value) == true)
                {
                    throw new Exception("Already exists");
                }
            }
            return 1;
        }

        public int CreateBatch(BatchBO batchBO)
        {
            ObjectParameter IsExist = new ObjectParameter("IsExist", typeof(bool));
            ObjectParameter BatchID = new ObjectParameter("BatchID", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateNewBatch(
                       batchBO.ItemID,
                       batchBO.BatchNo,
                       batchBO.CustomBatchNo,
                       batchBO.ManufacturingDate,
                       batchBO.ExpiryDate,
                       batchBO.ISKPrice,
                       batchBO.OSKPrice,
                       batchBO.ExportPrice,
                       batchBO.RetailMRP,
                       batchBO.RetailLooseRate,
                       batchBO.BatchRate,
                       batchBO.PurchaseLooseRate,
                       batchBO.ProfitPrice,
                       batchBO.UnitID,
                       GeneralBO.CreatedUserID,
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID,
                       IsExist,
                       BatchID
                      );

                if (Convert.ToBoolean(IsExist.Value) == true)
                {
                    throw new Exception("Already exists");
                }
            }
            return (int)BatchID.Value;
        }

        public List<BatchBO> GetBatchForProductionIssueMaterialReturn(int productionID, int itemID)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetBatchForProductionIssueMaterialReturn(productionID, itemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    BatchNo = a.BatchNo,
                    ID = a.BatchID,
                    IssueQty = (decimal)a.IssueQty
                }).ToList();
                return Batch;
            }
        }

        public List<PreProcessBatchBO> GetPreProcessItemBatchwise(int ItemID, int UnitID, decimal Quantity)

        {
            List<PreProcessBatchBO> Batch = new List<PreProcessBatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetBatchwiseItemsForPurificationIssue(ItemID, UnitID, Quantity, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PreProcessBatchBO
                {
                    BatchNo = a.BatchNo,
                    ID = (int)a.BatchID,
                    ItemID = (int)a.ItemID,
                    RequestedQty = (decimal)a.RequestedQty,
                    Unit = a.Unit,
                    UnitID = (int)a.UnitID,
                    Stock = (decimal)a.Stock,
                    IssueQty = (decimal)a.IssueQty,
                    ItemName = a.ItemName
                }).ToList();
                return Batch;
            }
        }

        public List<BatchBO> GetBatchesForAutoComplete(int ItemID, string Hint)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetBatchesForOpeningStock(ItemID, Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    BatchNo = a.BatchNo,
                    ID = a.BatchID,
                    BatchTypeID = (int)a.BatchTypeID
                }).ToList();
                return Batch;
            }
        }

        public DatatableResultBO GetBatchListForGrn(string BatchNoHint, int ItemIDHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetBatchForGrn(BatchNoHint, ItemIDHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                BatchNo = item.BatchNo,
                                ItemName = item.ItemName,
                                ItemType = item.ItemType,
                                BatchType = item.BatchType,
                                StartDate = ((DateTime)item.ManufacturingDate).ToString("dd-MMM-yyyy"),
                                ExpiryDate = item.ItemName == "Create new Batch" ? "" : ((DateTime)item.ExpiryDate).ToString("dd-MMM-yyyy"),
                                ExpiryDateStr = ((DateTime)item.ExpiryDate).ToString("dd-MM-yyy"),
                                ISKPrice = item.ISKPrice,
                                OSKPrice = item.OSKPrice,
                                ExportPrice = item.ExportPrice,
                                ItemCategory = item.ItemCategory,
                                RetailMRP = item.RetailMRP,
                                RetailLooseRate = item.RetailLooseRate,
                                PurchaseMRP = item.PurchaseMRP,
                                PurchaseLooseRate = item.PurchaseLooseRate,
                                PackSize=item.PackSize,
                                UnitID=item.UnitID,
                                Unit=item.Unit

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


        public List<BatchBO> GetLatestBatchDetails(int itemID)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetLatestBatchDetails(itemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    RetailLooseRate = (decimal)a.RetailLooseRate,
                    RetailMRP = (decimal)a.RetailMRP,
                    BatchRate = (decimal)a.PurchaseMRP,
                    PurchaseLooseRate = (decimal)a.PurchaseLooseRate,
                    ProfitPrice = (decimal)a.ProfitPrice,
                    Unit = a.Unit,
                    UnitID = a.UnitID,
                    PackSize = (decimal)a.PackSize
                }).ToList();
                return Batch;
            }
        }
        public List<PreviousBatchBO> GetPreviousBatchDetails(int BatchID)
        {
            List<PreviousBatchBO> Batch = new List<PreviousBatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetPreviousBatchDetails(BatchID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PreviousBatchBO
                {
                    RetailLooseRate = a.RetailLooseRate,
                    RetailMRP = a.RetailMRP,
                    BatchRate = a.BatchRate,
                    PurchaseLooseRate = a.PurchaseLooseRate,
                    ProfitPrice = a.ProfitPrice,
                    InvoiceNo = a.InvoiceNo,
                    TransNo = a.TransNo,
                    PODate = a.InvoiceDate,
                    Quantity = a.ReceivedQty,
                    OfferQty = a.OfferQty,
                    SupplierName = a.SupplierName,
                    ProfitRatio = a.NetProfitRatio,
                    ProfitTolerance = (decimal)a.ProfitTolerance,
                    Discount=(decimal)a.DiscountAmount,
                    Unit=a.Unit,
                    UnitID=(int)a.UnitID

                }).ToList();
                return Batch;
            }
        }

        public BatchBO GetBatchDetailsByBatchNo(string BatchNo)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetBatchDetailsByBatchNo(BatchNo).Select(a => new BatchBO
                    {
                    ID = a.BatchID,
                    BatchNo = BatchNo,
                    CustomBatchNo = BatchNo,
                    Name = a.ItemName,
                    ExpiryDate = (DateTime)a.ExpiryDate,
                    Unit = a.Unit,
                    UnitID = a.UnitID,
                    Stock = (decimal)a.Stock,
                    ItemID = a.ItemID,
                    GSTPercentage = (decimal)a.GSTPercent,
                    CessPercentage = (decimal)a.CessPercentage,
                    RetailLooseRate = (decimal)a.RetailLooseRate,
                    BusinessCategoryID=(int)a.businessCategoryID,
                    BusinessCategory=a.BusinessCategory
                    }
                      ).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<PreviousBatchBO> GetBatchTrans(int BatchID,string Type)
        {
            List<PreviousBatchBO> Batch = new List<PreviousBatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetBatchInvoices(Type,BatchID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PreviousBatchBO
                {
                    ID = a.ID,
                    InvoiceNo = a.InvoiceNo,
                    InvoiceDate = (DateTime)a.PurchaseDate,
                    SupplierName = a.Supplier,
                    Unit = a.POUnit,
                    Quantity =(decimal) a.InvoiceQty,
                    OfferQty = a.OfferQty,
                    PurchasePrice = a.PurchasePrice,
                    PurchaseLooseRate =(decimal) a.PurchaseLooseRate,
                    SalesRate = a.SalesRate,
                    LooseSalesRate =(decimal) a.LooseSalesRate,
                    LooseQty =(decimal) a.LooseQty,
                    GSTPercentage =(decimal) a.IgstPercent,
                    GSTAmount = (decimal)a.GSTAmount,
                    ProfitRatio =(decimal) a.BatchNetProfitRatio,
                    DiscountID=(int)a.DiscountID,
                    RetailMRP=(decimal)a.RetailMRP,
                    CessPercentage=(decimal)a.CessPercentage,
                    InvoiceRate=(decimal)a.InvoiceRate,
                    SGSTAmt=(decimal)a.SGSTAmount,
                    CGSTAmt=(decimal)a.CGSTAmount,
                    Discount=(decimal)a.DiscountAmount,
                    DiscountPercent=a.DiscountPercentage

                }).ToList();
                return Batch;
            }
        }

        public DatatableResultBO GetCustomBatchForGrnAutocomplete(string BatchNoHint, int ItemIDHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetCustomBatchForGrn(BatchNoHint, ItemIDHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                BatchNo = item.CustomBatchNo
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

        public List<BatchBO> GetLatestBatchDetailsByCustomBatchNo(int ItemID,string CustomBatchNo)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetLatestBatchDetailsByCustomBatchNo(ItemID, CustomBatchNo, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    ID = a.ID,
                    BatchNo = a.BatchNo,
                    ItemType = a.ItemType,
                    ManufacturingDate = (DateTime)a.ManufacturingDate,
                    ExpiryDate = (DateTime)a.ExpiryDate,
                    ISKPrice = a.ISKPrice,
                    OSKPrice = a.OSKPrice,
                    ExportPrice = a.ExportPrice,
                    RetailMRP = (decimal)a.RetailMRP,
                    RetailLooseRate = (decimal)a.RetailLooseRate,
                    PurchaseMRP = (decimal)a.PurchaseMRP,
                    PurchaseLooseRate = (decimal)a.PurchaseLooseRate,
                    PackSize = (decimal)a.PackSize,
                    UnitID = a.UnitID,
                    Unit = a.Unit
                }).ToList();
                return Batch;
            }
        }

        public BatchBO GetStockIssueItemDetailsByQRCodeBatchNo(string BatchNo, int WarehouseID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetStockIssueItemDetailsByQRCodeBatchNo(BatchNo, WarehouseID,GeneralBO.LocationID,GeneralBO.ApplicationID,GeneralBO.FinYear).Select(a => new BatchBO
                    {
                        ID = a.BatchID,
                        BatchNo = BatchNo,
                        CustomBatchNo = BatchNo,
                        Name = a.ItemName,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        Stock = (decimal)a.Stock,
                        ItemID = a.ItemID,
                        GSTPercentage = (decimal)a.GSTPercent,
                        CessPercentage = (decimal)a.CessPercentage,
                        RetailLooseRate = (decimal)a.RetailLooseRate,
                        BusinessCategoryID = (int)a.businessCategoryID,
                        BusinessCategory = a.BusinessCategory
                    }
                      ).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BatchBO> GetLatestBatchDetailsV3(int itemID)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetLatestBatchDetailsV3(itemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    RetailLooseRate = (decimal)a.RetailLooseRate,
                    RetailMRP = (decimal)a.RetailMRP,
                    BatchRate = (decimal)a.PurchaseMRP,
                    PurchaseLooseRate = (decimal)a.PurchaseLooseRate,
                    ProfitPrice = (decimal)a.ProfitPrice,
                    Unit = a.Unit,
                    UnitID = a.UnitID,
                    PackSize = (decimal)a.PackSize,
                    Category=a.SalesCategory,
                    PrimaryUnitID=(int)a.PrimaryUnitID,
                    InventoryUnitID=(int)a.InventoryUnitID,
                    PrimaryUnit=a.PrimaryUnit,
                    InventoryUnit=a.InventoryUnit,
                    ConversionFactorPtoI=(decimal)a.ConversionFactorPtoI,
                    LooseRatePercent=(decimal)a.LooseRatePercent,
                    GSTPercentage=(decimal)a.GSTPercentage
                }).ToList();
                return Batch;
            }
        }

        public BatchBO GetBatchDetailsByBatchID(int BatchID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetBatchDetailsByBatchID(BatchID,GeneralBO.FinYear, GeneralBO.LocationID,GeneralBO.ApplicationID).Select(a => new BatchBO
                    {
                        ID = a.BatchID,
                        BatchNo = a.BatchNo,
                        CustomBatchNo = a.BatchNo,
                        Name = a.ItemName,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        Stock = (decimal)a.Stock,
                        ItemID = a.ItemID,
                        GSTPercentage = (decimal)a.GSTPercent,
                        CessPercentage = (decimal)a.CessPercentage,
                        RetailLooseRate = (decimal)a.RetailLooseRate,
                        BusinessCategoryID = (int)a.businessCategoryID,
                        BusinessCategory = a.BusinessCategory
                    }
                      ).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BatchBO> GetBatchListForAPI(int ItemID)
        {
            List<BatchBO> Batch = new List<BatchBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.SpGetBatchListForAPI(ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BatchBO
                {
                    BatchID = a.BatchID,
                    BatchNo = a.BatchNo,
                    ItemID = (int)a.ItemID,
                    ExpiryDate = (DateTime)a.ExpiryDate,
                    FullSellingPrice = (decimal)a.FullSellingPrice,
                    LooseSellingPrice = (decimal)a.LooseSellingPrice,
                    FullPurchasePrice = (decimal)a.FullPurchasePrice,
                    LoosePurchasePrice = (decimal)a.LoosePurchasePrice,
                    IsSuspended = (int)a.IsSuspended,
                }).ToList();
                return Batch;
            }
        }



    }
}
