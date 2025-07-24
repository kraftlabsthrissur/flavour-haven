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
   public class PurchaseReturnProcessingDAL
    {
        public List<PurchaseReturnProcessingItemBO> GetPurchaseReturnProcessingItem(string ProcessingType, DateTime FromTransactionDate, DateTime ToTransactionDate, DateTime AsOnDate,int Days)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    return dbEntity.SpGetPurchaseReturnProcessingItem(ProcessingType, FromTransactionDate, ToTransactionDate, AsOnDate, Days, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseReturnProcessingItemBO
                    {
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        BatchID = (int)a.BatchID,
                        BatchNo = a.BatchNo,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        Supplier = a.Supplier,
                        SupplierID = (int)a.SupplierID,
                        InvoiceNo = a.InvoiceNo,
                        InvoiceID = (int)a.InvoiceID,
                        InvoiceDate = (DateTime)a.InvoiceDate,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        OfferQty = (decimal)a.OfferQty,
                        Qty = (decimal)a.Qty,
                        PurchaseRate = (decimal)a.NetPurchasePrice,
                        Stock = (decimal)a.Stock,
                        SupplierStateID = (int)a.StateID,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        IGSTPercentage = (decimal)a.IGSTPercent,
                        SGSTPercentage = (decimal)a.SGSTPercent,
                        CGSTPercentage = (decimal)a.CGSTPercent,
                        UnitID = (int)a.UnitID,
                        InvoiceTransID=(int)a.InvoiceTransID,
                        IGSTAmount=(decimal)a.IGSTAmount,
                        CGSTAmount=(decimal)a.CGSTAmount,
                        SGSTAmount=(decimal)a.SGSTAmount,
                        Unit=a.Unit,
                        NoOFDaysInventoryHeld=(int)a.NoOFDaysInventoryHeld
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(string XMLPurchaseReturnProcessingItems)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PurchaseReturn";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Id = new ObjectParameter("Id", typeof(int));
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SpCreatePurchaseReturnProcessing(
                        XMLPurchaseReturnProcessingItems,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID
                            );

                        transaction.Commit();
                        return 1;
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
