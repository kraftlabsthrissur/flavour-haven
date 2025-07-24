using BusinessObject;
using DataAccessLayer;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;


namespace DataAccessLayer
{
    public class StockAdjustmentDAL
    {
        public List<StockAdjustmentItemBO> GetStockAdjustmentItems(int WarehouseID, int ItemCategoryID, int ItemID, int SalesCategoryID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockAdjustmentItem(WarehouseID, ItemCategoryID, ItemID, SalesCategoryID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockAdjustmentItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.Unit,
                        UnitID = a.UnitID,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        BatchType = a.BatchTypeName,
                        CurrentQty = (decimal)a.CurrentQty,
                        PhysicalQty = (decimal)a.CurrentQty,
                        WarehouseID = a.WarehouseID,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        Rate = (decimal)a.Rate,
                        LooseRate = (decimal)a.LooseRate,
                        InventoryUnit = a.InventoryUnit,
                        InventoryUnitID = (int)a.InventoryUnitID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StockAdjustmentItemBO> GetStockAdjustmentItemsForAlopathy(DateTime FromDate, DateTime ToDate, int ItemID,int StockAjustmentPremise)
        {
            try
            {
                //StockAjustmentPremise
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockAdjustmentItemForAlopathy(FromDate, ToDate, ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockAdjustmentItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.SalesUnit,
                        UnitID = a.SalesUnitID,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        CurrentQty = (decimal)a.CurrentQty,
                        PhysicalQty = (decimal)a.CurrentQty,
                        WarehouseID = a.WarehouseID,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        WareHouse = a.WarehouseName,
                        Status = (bool)a.IsPending == true ? "Pending" : "Completed",
                        ID = a.ScheduleTransID


                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StockAdjustmentItemBO> GetBatchesByItemIDForStockAdjustment(int WarehouseID, int ItemID, int BatchTypeID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.GetBatchesByItemIDForStockAdjustment(WarehouseID, ItemID, BatchTypeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockAdjustmentItemBO()
                    {

                        Batch = a.BatchNo,
                        BatchID = a.BatchID,

                        ExpiryDate = (DateTime)a.ExpiryDate,

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(StockAdjustmentBO StockAdjBO, List<StockAdjustmentItemBO> items)
        {
            //try
            //{
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "StockAdjustment";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("StockAdjustmentID", typeof(int));
                        ObjectParameter RetVal = new ObjectParameter("RetVal", typeof(int));
                        if (StockAdjBO.IsDraft)
                        {
                            FormName = "DraftStockAdjustment";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = dbEntity.SpCreateStockAdjustment(
                            SerialNo.Value.ToString(), StockAdjBO.Date, 0, StockAdjBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Id);


                        if (Id.Value != null)
                        {
                            foreach (var itm in items)
                            {
                                dbEntity.SpCreateStockAdjustmentTrans(
                                Convert.ToInt32(Id.Value),
                                itm.ID,
                                itm.WarehouseID,
                                itm.ItemID,
                                itm.BatchTypeID,
                                itm.BatchID,
                                itm.UnitID,
                                itm.CurrentQty,
                                itm.PhysicalQty,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                RetVal);
                            }

                        };
                        if (Convert.ToInt32(RetVal.Value) == 1)
                        {
                            throw new AlreadyAdjustedException("Some items already adjusted");
                        }
                        transaction.Commit();
                        return (int)Id.Value;
                        //    }
                        //}
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public int SaveV3(StockAdjustmentBO StockAdjBO, List<StockAdjustmentItemBO> items)
        {
            //try
            //{
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "StockAdjustment";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("StockAdjustmentID", typeof(int));
                         if (StockAdjBO.IsDraft)
                        {
                            FormName = "DraftStockAdjustment";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = dbEntity.SpCreateStockAdjustment(
                            SerialNo.Value.ToString(), StockAdjBO.Date, 0, StockAdjBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Id);


                        if (Id.Value != null)
                        {
                            foreach (var itm in items)
                            {
                                dbEntity.SpCreateStockAdjustmentTransV3(
                                Convert.ToInt32(Id.Value),
                                itm.ID,
                                itm.WarehouseID,
                                itm.ItemID,
                                itm.BatchTypeID,
                                itm.BatchID,
                                itm.UnitID,
                                itm.CurrentQty,
                                itm.PhysicalQty,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                                );
                            }

                        };
                        transaction.Commit();
                        return (int)Id.Value;
                        //    }
                        //}
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int Revert(List<StockAdjustmentItemBO> items)
        {
            //try
            //{
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var itm in items)
                        {
                            dbEntity.SpRevertHoldStock(
                            itm.ID,
                            itm.ItemID,
                            itm.BatchID,
                            itm.BatchTypeID,
                            itm.WarehouseID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );
                        }

                        transaction.Commit();
                        return 1;
                        //    }
                        //}
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<StockAdjustmentBO> GetStockAdjustmentList()
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockAdjustment(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockAdjustmentBO()
                    {
                        ID = a.ID,
                        TransNo = a.TransNO,
                        Warehouse = a.WareHouse,
                        Date = a.Date,
                        IsDraft = (bool)a.IsDraft
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StockAdjustmentItemBO> GetStockAdjustmentTrans(int ID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockAdjustmentTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockAdjustmentItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.Unit,
                        UnitID = a.UnitID,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        BatchType = a.BatchType,
                        CurrentQty = (decimal)a.CurrentQty,
                        PhysicalQty = (decimal)a.PhysicalQty,
                        WarehouseID = a.WarehouseID,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        DamageTypeID = (int)a.DamageTypeID,
                        Remark = a.Remark,
                        DamageType = a.DamageTypes,
                        WareHouse = a.WareHouse,
                        Rate = (decimal)a.Rate,
                        FullRate = (decimal)a.FullRate,
                        LooseRate = (decimal)a.LooseRate,
                        InventoryUnit = a.InventoryUnit,
                        InventoryUnitID = (int)a.InventoryUnitID,
                        PrimaryUnit = a.PrimaryUnit,
                        PrimaryUnitID = a.PrimaryUnitID,
                        ExcessValue = (decimal)a.ExcessValue,
                        ExcessQty = (decimal)a.ExcessQty
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StockAdjustmentBO> GetStockAdjustmentDetail(int ID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockAdjustmentDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockAdjustmentBO()
                    {
                        ID = a.ID,
                        TransNo = a.TransNO,
                        Warehouse = a.Name,
                        Date = a.Date,
                        WarehouseID = a.WarehouseID,
                        IsDraft = (bool)a.IsDraft
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        public int Update(StockAdjustmentBO StockAdjBO, List<StockAdjustmentItemBO> items)
        {
            //try
            //{
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetVal = new ObjectParameter("RetVal", typeof(int));


                        var i = dbEntity.SpUpdateStockAdjustment(
                            StockAdjBO.ID, StockAdjBO.Date, StockAdjBO.WarehouseID, StockAdjBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);



                        foreach (var itm in items)
                        {
                            dbEntity.SpCreateStockAdjustmentTrans(
                            StockAdjBO.ID,
                            itm.ID,
                            itm.WarehouseID,
                            itm.ItemID,
                            itm.BatchTypeID,
                            itm.BatchID,
                            itm.UnitID,
                            itm.CurrentQty,
                            itm.PhysicalQty,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID,
                    RetVal);
                        }

                        if (Convert.ToInt32(RetVal.Value) == 1)
                        {
                            throw new AlreadyAdjustedException("Some items already adjusted");
                        }
                        transaction.Commit();
                        return (int)StockAdjBO.ID;

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public int UpdateV3(StockAdjustmentBO StockAdjBO, List<StockAdjustmentItemBO> items)
        {
            //try
            //{
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                    

                        var i = dbEntity.SpUpdateStockAdjustment(
                            StockAdjBO.ID, StockAdjBO.Date, StockAdjBO.WarehouseID, StockAdjBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);



                        foreach (var itm in items)
                        {
                            dbEntity.SpCreateStockAdjustmentTransV3(
                            StockAdjBO.ID,
                            itm.ID,
                            itm.WarehouseID,
                            itm.ItemID,
                            itm.BatchTypeID,
                            itm.BatchID,
                            itm.UnitID,
                            itm.CurrentQty,
                            itm.PhysicalQty,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID);
                        }

                        
                        transaction.Commit();
                        return (int)StockAdjBO.ID;

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<StockAdjustmentItemBO> ProcessUploadedStockAdjustment(string Items)
        {
            try
            {
                List<StockAdjustmentItemBO> StockAdjustment = new List<StockAdjustmentItemBO>();

                using (StockEntities dbEntity = new StockEntities())
                {
                    StockAdjustment = dbEntity.SpGetStockAdjustmentItems(Items, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockAdjustmentItemBO()
                    {
                        ItemID = a.ItemID,
                        UnitID = a.UnitID,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        WarehouseID = a.WarehouseID,
                        ItemCode = a.ItemCode,
                        ItemName = a.ItemName,
                        UnitName = a.UnitName,
                        Batch = a.Batch,
                        BatchType = a.BatchType,
                        WareHouse = a.WareHouse,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        CurrentQty = (decimal)a.CurrentQty,
                        PhysicalQty = (decimal)a.PhysicalQty
                    }).ToList();
                    return StockAdjustment;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetStockAdjustmentList(string TransNo, string TransDate, string Store, string ItemName, string SalesCategory, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {

                    var result = dbEntity.SpGetStockAdjustmentList(TransNo, TransDate, Store, ItemName, SalesCategory, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransNo = item.TransNO,
                                Warehouse = item.WareHouse,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                IsDraft = (bool)item.IsDraft,
                                SalesCategory = item.SalesCategory,
                                ItemName = item.ItemName
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

        public List<StockAdjustmentItemBO> GetScheduledStockItems(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    List<StockAdjustmentItemBO> StockAdjustment = new List<StockAdjustmentItemBO>();
                    //return dbEntity.SpGetStockAdjustmentScheduledItemsForPrint(FromDate, ToDate, GeneralBO.LocationID, GeneralBO.FinYear, GeneralBO.ApplicationID).Select(a => new StockAdjustmentItemBO()
                    //{

                    //    ItemName = a.ItemName,
                    //    WareHouse = a.WareHouseName
                    //}).ToList();
                    return StockAdjustment;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveStockAdjustmentForAPI(StockAdjustmentBO StockAdjBO)
        {
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "StockAdjustment";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("StockAdjustmentID", typeof(int));
                        ObjectParameter RetVal = new ObjectParameter("RetVal", typeof(int));
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        var i = dbEntity.SpCreateStockAdjustmentForAPI(
                            SerialNo.Value.ToString(),
                            StockAdjBO.ItemCode,
                            StockAdjBO.Batch,
                            StockAdjBO.PhysicalQty, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Id, RetVal);
                        if (Convert.ToInt32(RetVal.Value) == 1)
                        {
                            throw new AlreadyAdjustedException("Some items already adjusted");
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

        public List<StockAdjustmentItemBO> GetStockAdjustmentItemsForAlopathyAPI(DateTime FromDate, DateTime ToDate, int ItemID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockAdjustmentItemForAlopathyAPI(FromDate, ToDate, ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockAdjustmentItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.SalesUnit,
                        UnitID = a.SalesUnitID,
                        CurrentQty = (decimal)a.CurrentQty,
                        PhysicalQty = (decimal)a.CurrentQty,
                        Status = (bool)a.IsPending == true ? "Pending" : "Completed",
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public int GetIsStockCheckingDone(string ItemCode,string Batch)
        //{

        //    try
        //    {
        //        ObjectParameter IsPending = new ObjectParameter("IsPending", typeof(int));
        //        using (StockEntities dbEntity = new StockEntities())
        //        {
        //            dbEntity.spGetIsStockCheckingDone(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID,ItemCode,Batch,IsPending);
        //            return IsPending.Value.;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //}
        public StockAdjustmentBO GetIsStockCheckingDone(string ItemCode, string Batch)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.spGetIsStockCheckingDone(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID,ItemCode, Batch).Select(a => new StockAdjustmentBO
                    {
                        IsPending = (int)a.IsPending
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

    }
}


