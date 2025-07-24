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
    public class DamageEntryDAL
    {
        public List<DamageEntryItemBO> GetDamageEntryItems(int WarehouseID, int ItemCategoryID, int ItemID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetDamageEntryItem(WarehouseID, ItemCategoryID, ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DamageEntryItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.Unit,
                        UnitID = a.UnitID,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        BatchType = a.BatchTypeName,
                        CurrentQty = a.CurrentQty,
                        DamageQty = a.CurrentQty,
                        WarehouseID = a.WarehouseID,
                        ExpiryDate = (DateTime)a.ExpiryDate
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DamageEntryItemBO> GetBatchesByItemIDForDamageEntry(int WarehouseID, int ItemID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.GetBatchesByItemIDForDamageEntry(WarehouseID, ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DamageEntryItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.Unit,
                        UnitID = a.UnitID,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        BatchType = a.BatchTypeName,
                        CurrentQty = a.CurrentQty,
                        ExpiryDate = (DateTime)a.ExpiryDate
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(DamageEntryBO damageEntryBO, List<DamageEntryItemBO> items)
        {
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("DamageEntryID", typeof(int));

                        var j = dbEntity.SpUpdateSerialNo("DamageEntry", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = dbEntity.SpCreateDamageEntry(
                            SerialNo.Value.ToString(), damageEntryBO.Date, damageEntryBO.WarehouseID, damageEntryBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Id);


                        if (Id.Value != null)
                        {
                            foreach (var itm in items)
                            {
                                dbEntity.SpCreateDamageEntryTrans(
                                Convert.ToInt32(Id.Value),
                                itm.WarehouseID,
                                itm.ItemID,
                                itm.UnitID,
                                itm.BatchTypeID,
                                itm.BatchID,
                                itm.ExpiryDate,
                                itm.CurrentQty,
                                itm.DamageQty,
                                itm.DamageTypeID,
                                itm.Remarks,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);
                            }

                        };
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

        public List<DamageEntryBO> GetDamageEntryList()
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetDamageEntry(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DamageEntryBO()
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
        public List<DamageEntryBO> GetDamageEntryDetail(int ID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetDamageEntryDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DamageEntryBO()
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
        public List<DamageEntryItemBO> GetDamageEntryTrans(int ID)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetDamageEntryTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DamageEntryItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.Unit,
                        UnitID = a.UnitID,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        BatchType = a.BatchType,
                        CurrentQty = a.CurrentQty,
                        DamageQty = a.DamageQty,
                        WarehouseID = a.WarehouseID,
                        DamageTypeID = a.DamageTypeID,
                        DamageType = a.DamageType,
                        Remarks = a.Remarks,
                        ExpiryDate = (DateTime)a.ExpiryDate
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(DamageEntryBO damageEntryBO, List<DamageEntryItemBO> items)
        {
            //try
            //{
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {


                        var i = dbEntity.SpUpdateDamageEntry(
                            damageEntryBO.ID, damageEntryBO.Date, damageEntryBO.WarehouseID, damageEntryBO.IsDraft, GeneralBO.CreatedUserID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);


                        if (damageEntryBO.ID != null)
                        {
                            foreach (var itm in items)
                            {
                                dbEntity.SpCreateDamageEntryTrans(
                                damageEntryBO.ID,
                                itm.WarehouseID,
                                itm.ItemID,
                                itm.UnitID,
                                itm.BatchTypeID,
                                itm.BatchID,
                                itm.ExpiryDate,
                                itm.CurrentQty,
                                itm.DamageQty,
                                itm.DamageTypeID,
                                itm.Remarks,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);
                            }

                        };
                        transaction.Commit();
                        return (int)damageEntryBO.ID;

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
