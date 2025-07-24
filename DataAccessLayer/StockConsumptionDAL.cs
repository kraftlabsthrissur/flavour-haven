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
   public class StockConsumptionDAL
    {
        public List<StockConsumptionItemBO> GetStockConsumptionItems(int WarehouseID, int ItemCategoryID, int ItemID, int SalesCategoryID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockConsumptionItem(WarehouseID, ItemCategoryID, ItemID, SalesCategoryID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockConsumptionItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.Unit,
                        UnitID = a.UnitID,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        BatchType = a.BatchTypeName,
                        AvailableQty = (decimal)a.AvailableQty,
                        PhysicalQty = (decimal)a.AvailableQty,
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

        public int Save(StockConsumptionBO StockConBO, List<StockConsumptionItemBO> items)
        {
            
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "StockConsumption";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("StockConsumptionID", typeof(int));

                        if (StockConBO.IsDraft)
                        {
                            FormName = "DraftStockConsumption";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                        var i = dbEntity.SpCreateStockConsumption(
                            SerialNo.Value.ToString(), StockConBO.Date, StockConBO.WarehouseID, StockConBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Id);


                        if (Id.Value != null)
                        {
                            foreach (var itm in items)
                            {
                                dbEntity.SpCreateStockConsumptionTrans(
                                Convert.ToInt32(Id.Value),
                                itm.WarehouseID,
                                itm.ItemID,
                                itm.BatchTypeID,
                                itm.BatchID,
                                itm.UnitID,
                                itm.AvailableQty,
                                itm.PhysicalQty,
                                itm.ExpiryDate,
                                itm.Rate,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                ReturnValue);
                            }

                        };
                        transaction.Commit();
                        return (int)Id.Value;
                        //   }
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

        public DatatableResultBO GetStockConsumptionList(string Type,string TransNo, string TransDate, string Store, string ItemName, string SalesCategory, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {

                    var result = dbEntity.SpGetStockConsumptionList(Type,TransNo, TransDate, Store, ItemName, SalesCategory, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                ItemName = item.ItemName,
                                Status=item.Status
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

        public List<StockConsumptionBO> GetStockConsumptionDetail(int ID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockConsumptionDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockConsumptionBO()
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

        public List<StockConsumptionItemBO> GetStockConsumptionTrans(int ID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetStockConsumptionTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockConsumptionItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.Unit,
                        UnitID = a.UnitID,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        BatchType = a.BatchType,
                        AvailableQty = (decimal)a.Value,
                        PhysicalQty = (decimal)a.ConsumptionQty,
                        WarehouseID = a.WarehouseID,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        WareHouse = a.WareHouse,
                        Rate = (decimal)a.Rate,
                        FullRate = (decimal)a.FullRate,
                        LooseRate = (decimal)a.LooseRate,
                        InventoryUnit = a.InventoryUnit,
                        InventoryUnitID = (int)a.InventoryUnitID,
                        PrimaryUnit = a.PrimaryUnit,
                        PrimaryUnitID = a.PrimaryUnitID,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(StockConsumptionBO StockConBO, List<StockConsumptionItemBO> items)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {


                        var i = dbEntity.SpUpdateStockConsumption(
                            StockConBO.ID, StockConBO.Date, StockConBO.WarehouseID, StockConBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);



                        foreach (var itm in items)
                        {
                            dbEntity.SpCreateStockConsumptionTrans(
                                StockConBO.ID,
                                itm.WarehouseID,
                                itm.ItemID,
                                itm.BatchTypeID,
                                itm.BatchID,
                                itm.UnitID,
                                itm.AvailableQty,
                                itm.PhysicalQty,
                                itm.ExpiryDate,
                                itm.Rate,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                ReturnValue);

                            if (Convert.ToInt16(ReturnValue.Value) == -1)
                            {
                                throw new Exception("Selected Item does not have Stock");
                            }
                        }


                        transaction.Commit();
                        return (int)StockConBO.ID;

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
}
