using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccessLayer
{
    public class RateAdjustmentDAL
    {
        public List<RateAdjustmentItemBO> GetRateAdjustmentItems(int ItemCategoryID, int ItemID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetRateAdjustmentItemForRateAdjustment(ItemCategoryID, ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new RateAdjustmentItemBO()
                    {
                        ItemID = a.ItemID,
                        Category = a.ItemCategory,
                        ItemName = a.ItemName,
                        SystemStockQty = a.SystemStockQty,
                        SystemAvgCost = a.SystemAverageCost
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(RateAdjustmentBO RateAdjBO, List<RateAdjustmentItemBO> items)
        {
            //try
            //{
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "RateAdjustment";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("RateAdjustmentID", typeof(int));

                        if (RateAdjBO.IsDraft)
                        {
                            FormName = "DraftRateAdjustment";
                        }

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = dbEntity.SpCreateRateAdjustment(
                            SerialNo.Value.ToString(), RateAdjBO.Date, RateAdjBO.IsDraft, GeneralBO.CreatedUserID,
                            DateTime.Now,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Id);

                        //  dbContext.SaveChanges();
                        if (Id.Value != null)
                        {
                            //purchaseReturnID = Convert.ToInt32(purchaseReturnIDOut.Value);

                            //if (purchaseReturnBO.PurchaseReturnTrnasItemBOList != null)
                            foreach (var itm in items)
                            {
                                dbEntity.SpCreateRateAdjustmentTrans(
                       Convert.ToInt32(Id.Value),

                        itm.ItemID,

                        itm.SystemStockQty,
                        itm.SystemAvgCost,
                        itm.ActualAvgCost,
                        itm.DifferenceInAvgCost,
                        itm.SystemStockValue,
                        itm.ActualStockValue,
                        itm.DifferenceInStockValue,
                        itm.EffectDate,
                        itm.Remark,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID);
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
        public List<RateAdjustmentBO> GetRateAdjustmentList()
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetRateAdjustment(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new RateAdjustmentBO()
                    {
                        ID = a.ID,
                        TransNo = a.TransNO,
                        //Warehouse = a.WareHouse,
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
        public List<RateAdjustmentItemBO> GetRateAdjustmentTrans(int ID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetRateAdjustmentTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(item => new RateAdjustmentItemBO()
                    {
                        ItemID = item.ItemID,
                        SystemAvgCost = item.SystemAvgCost,
                        SystemStockQty = item.SystemStockQty,
                        SystemStockValue = item.SystemStockValue,
                        ActualAvgCost = item.ActualAvgCost,
                        ActualStockValue = item.ActualStockValue,
                        DifferenceInAvgCost = item.DiffInAvgCost,
                        DifferenceInStockValue = item.DifferenceInStockQty,
                        ItemName = item.ItemName,
                        Category = item.Category,
                        Remark = item.Remark
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<RateAdjustmentBO> GetRateAdjustmentDetail(int ID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetRateAdjustmentDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new RateAdjustmentBO()
                    {
                        ID = a.ID,
                        TransNo = a.TransNO,
                        // Warehouse = a.Name,
                        Date = a.Date,
                        // WarehouseID = a.WarehouseID,
                        IsDraft = (bool)a.IsDraft
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(RateAdjustmentBO RateAdjBO, List<RateAdjustmentItemBO> items)
        {
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var i = dbEntity.SpUpdateRateAdjustment(
                            RateAdjBO.ID, RateAdjBO.Date, RateAdjBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        if (RateAdjBO.ID != null)
                        {
                            foreach (var itm in items)
                            {
                                dbEntity.SpCreateRateAdjustmentTrans(
                       RateAdjBO.ID,
                        itm.ItemID,
                        itm.SystemStockQty,
                        itm.SystemAvgCost,
                        itm.ActualAvgCost,
                        itm.DifferenceInAvgCost,
                        itm.SystemStockValue,
                        itm.ActualStockValue,
                        itm.DifferenceInStockValue,
                        itm.EffectDate,
                        itm.Remark,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID);
                            }
                        };
                        transaction.Commit();
                        return (int)RateAdjBO.ID;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public DatatableResultBO GetRateAdjustmentListForDataTable(string Type, string TransNo, string TransDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetRateAdjustmentListForDataTable(Type, TransNo, TransDate, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransDate = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
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

    }
}
