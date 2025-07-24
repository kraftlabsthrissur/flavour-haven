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
    public class ReOrderDAL
    {
        public List<ReOrderItemBO> ReOrderList(int ReOrderDays, int OrderDays, string ItemType)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    return dbEntity.SpGetReOrderList(ReOrderDays, OrderDays, ItemType, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReOrderItemBO()
                    {
                        ReOrderQty = (decimal)a.ReOrderQty,
                        //ReOrderQtyFull = (decimal)a.ReOrderQtyFull,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        LastPurchasedDate = (DateTime)a.LastPurchaseDate,
                        Rate = (decimal)a.LastPurchaseRate,
                        Stock = (decimal)a.Stock,
                        UnitID = a.UnitID,
                        Supplier = a.Supplier,
                        Unit = a.Unit,
                        OrderedQty = (decimal)a.OrderedQty,
                        //LastPurchaseQty = (decimal)a.LastPurchaseQty,
                        //LastPurchaseOfferQty = (decimal)a.LastPurchaseOfferQty,
                        //SupplierID = (int)a.SupplierID,
                        //PurchaseUnit = a.PurchaseUnit

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Save(ReOrderBO ReOrder, List<ReOrderItemBO> Items)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "ReOrder";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("Id", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter POIDs = new ObjectParameter("POIDs", typeof(string));

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        dbEntity.SpCreateReOrder(
                            SerialNo.Value.ToString(),
                            ReOrder.ReOrderDays,
                            ReOrder.OrderDays,
                            ReOrder.ItemType,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            Id
                            );
                        foreach (var item in Items)
                        {
                            dbEntity.SpCreateReOrderTrans(
                            Convert.ToInt32(Id.Value),
                            item.ReOrderItemID,
                            item.ItemID,
                            item.ReOrderQty,
                            item.Qty,
                            item.Rate,
                            item.SupplierID,
                            item.UnitID,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            item.IsOrdered

                                );
                        }

                        dbEntity.SpCreatePurchaseOrderFromReOrder(
                            Convert.ToInt32(Id.Value),
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            POIDs
                           );
                        //if (Convert.ToInt32(RetValue.Value) == -1)
                        //{
                        //    throw new DatabaseException("Item has stock");
                        //}
                        transaction.Commit();
                        return POIDs.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<CategoryBO> GetReOrderSupplierList(int ItemID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetReOrderSupplierList(ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ReOrderBO GetReOrderConfigvalues()
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetReOrderConfigvalues(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new ReOrderBO
                    {
                        ReOrderDays=(int)k.ReOrderDays,
                        OrderDays=(int)k.OrderDays
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
