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
    public class DirectPurchaseInvoiceDAL
    {

        public int Save(LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO, string XMLItems)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "LocalPurchaseInvoice";
                        ObjectParameter SerialNoGRN = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter PurchaseInvoiceID = new ObjectParameter("PurchaseInvoiceID", typeof(int));
                        ObjectParameter GRNID = new ObjectParameter("GRNID", typeof(int));
                        ObjectParameter POId = new ObjectParameter("PurchaseOrderID", typeof(int));
                        ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (LocalPurchaseInvoiceBO.IsDraft)
                        {
                            FormName = "DraftLocalPurchaseInvoice";
                        }


                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = dbEntity.SpCreateDirectPurchaseOrder(
                            SerialNo.Value.ToString(),
                            LocalPurchaseInvoiceBO.PurchaseOrderDate,
                            LocalPurchaseInvoiceBO.SupplierID,
                            0, 0, 0, 0, 0,
                            false,
                            true,
                            0, "", 0, 0,
                            LocalPurchaseInvoiceBO.SGSTAmount,
                            LocalPurchaseInvoiceBO.CGSTAmount,
                            LocalPurchaseInvoiceBO.IGSTAmount,
                             0, LocalPurchaseInvoiceBO.VATAmount, 0, 0,
                            LocalPurchaseInvoiceBO.NetAmount,
                            true,
                            LocalPurchaseInvoiceBO.IsDraft,
                            LocalPurchaseInvoiceBO.IsVAT,
                            LocalPurchaseInvoiceBO.IsGST,
                            LocalPurchaseInvoiceBO.Remarks,
                            LocalPurchaseInvoiceBO.SupplierReference,
                            "",
                            false,
                            null,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            0,
                            LocalPurchaseInvoiceBO.StoreID,
                            LocalPurchaseInvoiceBO.Discount,
                            LocalPurchaseInvoiceBO.OtherDeductions,
                            LocalPurchaseInvoiceBO.InvoiceNo,
                            LocalPurchaseInvoiceBO.InvoiceDate,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            POId
                        );

                        if (Convert.ToInt32(POId.Value) != 0)
                        {
                            dbEntity.SpCreatePurchaseOrderXMLMethod(Convert.ToInt16(POId.Value),
                                LocalPurchaseInvoiceBO.IsDraft,
                                XMLItems,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                ReturnValue
                            );

                            if (LocalPurchaseInvoiceBO.IsDraft != true)
                            {
                                var g = dbEntity.SpUpdateSerialNo("GRN", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNoGRN);
                                dbEntity.SpCreateGoodsReceiptNoteForDirectPurchaseInvoice(
                                    SerialNoGRN.Value.ToString(),
                                    Convert.ToInt32(POId.Value),
                                    GeneralBO.LocationID,
                                    GRNID
                                );

                                if (Convert.ToInt32(GRNID.Value) != 0)
                                {
                                    dbEntity.SpCreateGoodsReceiptNoteTransForDirectPurchaseInvoice(
                                        Convert.ToInt32(GRNID.Value),
                                        Convert.ToInt32(POId.Value),
                                        LocalPurchaseInvoiceBO.CurrencyID,
                                        GeneralBO.CreatedUserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                    );

                                    dbEntity.SpCreatePurchaseInvoiceForLocalPurchase(
                                        SerialNo.Value.ToString(),
                                        Convert.ToInt32(POId.Value),
                                        LocalPurchaseInvoiceBO.SupplierReference,
                                        LocalPurchaseInvoiceBO.TaxableAmount,
                                        LocalPurchaseInvoiceBO.InvoiceNo,
                                        LocalPurchaseInvoiceBO.InvoiceDate,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        PurchaseInvoiceID
                                    );
                                    if (Convert.ToInt32(PurchaseInvoiceID.Value) != 0)
                                    {
                                        dbEntity.SpCreatePurchaseInvoiceTransForLocalPurchase(
                                            Convert.ToInt32(PurchaseInvoiceID.Value),
                                            Convert.ToInt32(GRNID.Value),
                                            Convert.ToInt32(POId.Value)
                                        );
                                        transaction.Commit();
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return 0;
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return 0;
                                }
                            }
                            else
                            {
                                transaction.Commit();
                                return 1;
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return 0;
                        }
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

        public int Update(LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO, string XMLItems)
        {

            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNoGRN = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter SerialNoPurchaseInvoice = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter PurchaseInvoiceID = new ObjectParameter("PurchaseInvoiceID", typeof(int));
                        ObjectParameter GRNID = new ObjectParameter("GRNID", typeof(int));
                        ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        var k = dbEntity.SpUpdateSerialNo("LocalPurchaseInvoice", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNoPurchaseInvoice);


                        var i = dbEntity.SpUpdateDirectPurchaseOrder(LocalPurchaseInvoiceBO.ID,
                        LocalPurchaseInvoiceBO.SupplierID, 0, 0, 0,
                        LocalPurchaseInvoiceBO.PurchaseOrderDate, 0, 0, 0,
                        null,
                        LocalPurchaseInvoiceBO.IsDraft,
                        LocalPurchaseInvoiceBO.Remarks,
                        LocalPurchaseInvoiceBO.SupplierReference,
                        null, 0, 0,
                        LocalPurchaseInvoiceBO.SGSTAmount,
                        LocalPurchaseInvoiceBO.CGSTAmount,
                        LocalPurchaseInvoiceBO.IGSTAmount,
                        0, 0, 0,
                        LocalPurchaseInvoiceBO.NetAmount,
                        0,
                        LocalPurchaseInvoiceBO.StoreID,
                        LocalPurchaseInvoiceBO.Discount,
                        LocalPurchaseInvoiceBO.OtherDeductions,
                        LocalPurchaseInvoiceBO.InvoiceNo,
                        LocalPurchaseInvoiceBO.InvoiceDate,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                      );
                        dbEntity.SpCreatePurchaseOrderXMLMethod(LocalPurchaseInvoiceBO.ID,
                        LocalPurchaseInvoiceBO.IsDraft,
                        XMLItems,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        ReturnValue
                        );

                        if (LocalPurchaseInvoiceBO.IsDraft != true)
                        {
                            var g = dbEntity.SpUpdateSerialNo("GRN", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNoGRN);
                            dbEntity.SpCreateGoodsReceiptNoteForDirectPurchaseInvoice(
                            SerialNoGRN.Value.ToString(),
                            LocalPurchaseInvoiceBO.ID,
                            GeneralBO.LocationID,
                            GRNID
                            );

                            if (Convert.ToInt32(GRNID.Value) != 0)
                            {
                                dbEntity.SpCreateGoodsReceiptNoteTransForDirectPurchaseInvoice(
                                Convert.ToInt16(GRNID.Value),
                                LocalPurchaseInvoiceBO.ID,
                                LocalPurchaseInvoiceBO.CurrencyID,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                                );

                                dbEntity.SpCreatePurchaseInvoiceForLocalPurchase(
                                SerialNoPurchaseInvoice.Value.ToString(),
                                LocalPurchaseInvoiceBO.ID,
                                LocalPurchaseInvoiceBO.SupplierReference,
                                LocalPurchaseInvoiceBO.TaxableAmount,
                                LocalPurchaseInvoiceBO.InvoiceNo,
                                LocalPurchaseInvoiceBO.InvoiceDate,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                PurchaseInvoiceID
                                );
                                if (Convert.ToInt32(PurchaseInvoiceID.Value) != 0)
                                {
                                    dbEntity.SpCreatePurchaseInvoiceTransForLocalPurchase(
                                    Convert.ToInt16(PurchaseInvoiceID.Value),
                                    Convert.ToInt16(GRNID.Value),
                                    LocalPurchaseInvoiceBO.ID
                                    );
                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                            }
                        }
                        else
                        {
                            transaction.Commit();
                        }
                        return 1;
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                }
            }
        }

        public List<PurchaseOrderTransBO> GetDirectPurchaseOrderItems(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetDirectPurchaseOrderItems(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new PurchaseOrderTransBO
                    {
                        ItemID = (int)k.ItemID,
                        ItemCode = k.ItemCode,
                        ItemName = k.ItemName,
                        Unit = k.Unit,
                        UnitID = (int)k.UnitID,
                        QtyOrdered = (decimal)k.Quantity,
                        Rate = (decimal)k.Rate,
                        RetailMRP = k.RetailMRP,
                        RetailRate = k.RetailLoosePrice,
                        //Remarks = k.Remarks,
                        Amount = k.Amount,
                        CGSTAmt = k.CGSTAmt,
                        CGSTPercent = k.CGSTPercent,
                        SGSTAmt = k.SGSTAmt,
                        SGSTPercent = k.SGSTPercent,
                        IGSTAmt = k.IGSTAmt,
                        IGSTPercent = k.IGSTPercent,
                        VATAmount = k.VATAmount,
                        PartsNumber = k.PartsNumber,
                        Remark = k.Remark,
                        Model = k.Model,
                        VATPercentage = k.VATPercent,
                        IsGST = (int)k.IsGST,
                        IsVat = (int)k.IsVAT,
                        NetAmount = (decimal)k.NetAmount,
                        Name = k.ItemName,
                        CurrencyName = k.CurrencyName,
                        HSNCode = k.HSNCode,
                        GSTPercentage = k.GSTPercent,
                        BatchNo = k.BatchNo,
                        //ExpDate = (DateTime)k.ExpDate,
                        MRP = k.MRP,
                        Discount = (decimal)k.Discount,
                        DiscountPercent = (decimal)k.DiscountPercent
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LocalPurchaseInvoiceBO GetDirectPurchaseOrder(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetDirectPurchaseOrder(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new LocalPurchaseInvoiceBO
                    {
                        PurchaseOrderNo = k.PurchaseOrderNo,
                        CurrencyCode = k.CurrencyCode,
                        PurchaseOrderDate = (DateTime)k.PurchaseOrderDate,
                        IsDraft = (bool)k.IsDraft,
                        ID = k.ID,
                        SupplierID = k.SupplierID,
                        GrossAmnt = (decimal)k.TaxableAmount,
                        TaxableAmount = (decimal)k.TaxableAmount,
                        NetAmount = (decimal)k.NetAmt,
                        IGSTAmount = (decimal)k.IGSTAmt,
                        SGSTAmount = (decimal)k.SGSTAmt,
                        CGSTAmount = (decimal)k.CGSTAmt,
                        VATAmount = (decimal)k.VATAmount,
                        SupplierReference = k.Supplier,
                        SupplierStateID = (int)k.StateID,
                        IsGSTRegistered = (bool)k.IsGSTRegistered,
                        IsGST = (int)k.IsGST,
                        IsVAT = (int)k.IsVAt,
                        StoreID = k.StoreID,
                        Discount = k.Discount,
                        OtherDeductions = k.OtherDeductions,
                        Store = k.Store,
                        IsCanceled = k.Cancelled,
                        InvoiceDate = (DateTime)k.InvoiceDate,
                        InvoiceNo = k.InvoiceNo,
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetDirectPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string SupplierHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetDirectPurchaseInvoiceList(Type, TransNoHint, TransDateHint, SupplierHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = (int)item.ID,
                                TransNo = item.TransNo,
                                PODate = ((DateTime)item.PODate).ToString("dd-MMM-yyyy"),
                                Supplier = item.Supplier,
                                Status = item.Status,
                                NetAmount = item.NetAmount
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

        public List<MRPBO> GetMRPForPurchaseInvoice(int ItemID)
        {
            List<MRPBO> item = new List<MRPBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    item = dbEntity.SpGetMRPForPurchaseInvoice(ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MRPBO
                    {
                        MRP = (decimal)a.MRP,
                        Rate = (decimal)a.Rate
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MRPBO> GetMRPForPurchaseInvoiceByBatchID(int ItemID, string Batch)
        {
            List<MRPBO> item = new List<MRPBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    item = dbEntity.SpGetMRPForPurchaseInvoiceByBatchID(ItemID, Batch, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MRPBO
                    {
                        MRP = (decimal)a.MRP,
                        ExpDate = (DateTime)a.ExpiryDate
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsCancelable(int PurchaseOrderID)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                try
                {
                    ObjectParameter IsCancelable = new ObjectParameter("IsCancelable", typeof(bool));
                    dbEntity.SpIsDirectPurchaseInvoiceCancelable(PurchaseOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsCancelable);
                    return Convert.ToBoolean(IsCancelable.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int Cancel(int PurchaseOrderID)
        {
            PurchaseEntities dEntity = new PurchaseEntities();
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            dEntity.SpCancelDirectPurchaseInvoice(PurchaseOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, ReturnValue);
            return (int)ReturnValue.Value;
        }


        public List<ItemCategoryAndUnitBO> GetUnitsAndCategoryByItemID(int ItemID)
        {
            //List<ItemCategoryAndUnitBO> item = new List<ItemCategoryAndUnitBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var item = dbEntity.SpGetUnitsAndCategoryByItemID(ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return item.Select(a => new ItemCategoryAndUnitBO
                    {
                        ConversionFactorPtoI = (decimal)a.ConversionFactorPtoI,
                        PurchaseUnitID = (int)a.PurchaseUnitID,
                        PrimaryUnitID = (int)a.PrimaryUnitID,
                        PrimaryUnit = a.PrimaryUnit,
                        PurchaseUnit = a.PurchaseUnit,
                        Category = a.SalesCategory
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
