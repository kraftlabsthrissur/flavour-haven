using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccessLayer
{
    public class OpeningStockDAL
    {
        public int Save(OpeningStockBO openingStockBO, List<OpeningStockItemBO> openingStockItemsBO)
        {
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "openingStock";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("OpeningStockID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        if (openingStockBO.IsDraft)
                        {
                            FormName = "DraftopeningStock";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        dbEntity.SpCreateOpeningStock(
                            SerialNo.Value.ToString(),
                            openingStockBO.Date,
                            openingStockBO.IsDraft,
                            openingStockBO.WarehouseID,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            Id
                            );
                        foreach (var item in openingStockItemsBO)
                        {
                            dbEntity.SpCreateOpeningStockTrans(
                            Convert.ToInt32(Id.Value),
                            SerialNo.Value.ToString(),
                            openingStockBO.Date,
                            item.WarehouseID,
                            item.ItemID,
                            item.BatchTypeID,
                            item.Batch,
                            item.Qty,
                            item.Value,
                            item.UnitID,
                            item.ExpDate,
                            openingStockBO.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            RetValue
                                );
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Item has stock");
                        }
                        transaction.Commit();
                        return (int)Id.Value;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }


        public int Update(OpeningStockBO openingStockBO, List<OpeningStockItemBO> openingStockItemsBO)
        {
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        dbEntity.SpUpdateOpeningStock(
                        openingStockBO.ID,
                        openingStockBO.TransNo,
                        openingStockBO.Date,
                        openingStockBO.IsDraft,
                        openingStockBO.WarehouseID,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                        foreach (var item in openingStockItemsBO)
                        {
                            dbEntity.SpCreateOpeningStockTrans(
                            openingStockBO.ID,
                            openingStockBO.TransNo,
                            openingStockBO.Date,
                            item.WarehouseID,
                            item.ItemID,
                            item.BatchTypeID,
                            item.Batch,
                            item.Qty,
                            item.Value,
                            item.UnitID,
                            item.ExpDate,
                            openingStockBO.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            RetValue);
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Item has stock");
                        }
                        transaction.Commit();
                        return (int)openingStockBO.ID;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public List<OpeningStockBO> GetOpeningStocks()
        {
            List<OpeningStockBO> item = new List<OpeningStockBO>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    item = dbEntity.SpOpeningStockDetails(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new OpeningStockBO
                    {
                        TransNo = k.TransNo,
                        Date = k.Date,
                        Store = k.Store,
                        IsDraft = (bool)k.IsDraft,
                        ID = k.ID

                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OpeningStockItemBO> GetOpeningStockItems(int OpeningStockID)
        {
            List<OpeningStockItemBO> item = new List<OpeningStockItemBO>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    item = dbEntity.SpGetOpeningStockTransDetails(OpeningStockID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new OpeningStockItemBO
                    {

                        Batch = k.BatchNo,
                        ExpDate=(DateTime)k.ExpDate,
                        BatchType = k.BatchType,
                        ItemName = k.ItemName,
                        Qty = k.Qty,
                        BatchTypeID = k.BatchTypeID,
                        ItemID = k.ItemID,
                        BatchID = (int)k.BatchID,
                        Unit = k.Unit,
                        WarehouseID = (int)k.StoreID,
                        UnitID = (int)k.UnitID,
                        Value = (decimal)k.Value
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OpeningStockBO GetOpeningStock(int OpeningStockID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetOpeningStock(OpeningStockID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new OpeningStockBO
                    {
                        TransNo = k.TransNo,
                        Date = k.Date,
                        IsDraft = (bool)k.IsDraft,
                        ID = k.ID,
                        Store = k.Store,
                        WarehouseID = (int)k.StoreID,
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetOpeningStockListForDataTable(string Type, string TransNo, string Date, string Store, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetOpeningStockList(Type, TransNo, Date, Store, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                Store=item.Store,
                                Status = item.Status

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

        public List<OpeningStockMRPBO> GetMRPForOpeningStock(int ItemID, int BatchTypeID, string Batch)
        {
            List<OpeningStockMRPBO> item = new List<OpeningStockMRPBO>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    item = dbEntity.SpGetMRPForOpeningStock(ItemID, BatchTypeID, Batch, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new OpeningStockMRPBO
                    {
                        MRP = (decimal)a.MRP,
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

