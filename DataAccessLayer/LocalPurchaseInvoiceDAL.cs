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
    public class LocalPurchaseInvoiceDAL
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

                        var i = dbEntity.SpCreatePurchaseOrder(
                            SerialNo.Value.ToString(),
                            LocalPurchaseInvoiceBO.PurchaseOrderDate,
                            LocalPurchaseInvoiceBO.SupplierID,
                            0, 0, 0, 0, 0,
                            false,
                            true,
                            0, "", "", 0,
                            LocalPurchaseInvoiceBO.GSTAmount,
                            LocalPurchaseInvoiceBO.GSTAmount,
                            0, 0, 0, 0, 0,
                            LocalPurchaseInvoiceBO.Discount,
                            LocalPurchaseInvoiceBO.NetAmount,
                            LocalPurchaseInvoiceBO.GrossAmnt,
                            false,
                            LocalPurchaseInvoiceBO.IsDraft,
                            LocalPurchaseInvoiceBO.IsGST,
                            LocalPurchaseInvoiceBO.IsVAT,
                            0,
                            LocalPurchaseInvoiceBO.Remarks,
                            "","","","","","",0,0,0,
                            LocalPurchaseInvoiceBO.SupplierReference,
                            "",
                            false,
                            null,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            0,
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
                                dbEntity.SpCreateGoodsReceiptNoteForLocalPurchase(
                                    SerialNoGRN.Value.ToString(),
                                    Convert.ToInt16(POId.Value),
                                    GeneralBO.LocationID,
                                    GRNID
                                );

                                if (Convert.ToInt32(GRNID.Value) != 0)
                                {
                                    dbEntity.SpCreateGoodsReceiptNoteTransForLocalPurchase(
                                        Convert.ToInt16(GRNID.Value),
                                        Convert.ToInt16(POId.Value),
                                        GeneralBO.CreatedUserID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                    );

                                    dbEntity.SpCreatePurchaseInvoiceForLocalPurchase(
                                        SerialNo.Value.ToString(),
                                        Convert.ToInt16(POId.Value),
                                        LocalPurchaseInvoiceBO.SupplierReference,
                                        LocalPurchaseInvoiceBO.GrossAmnt,
                                        "", null,
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
                                            Convert.ToInt16(POId.Value)
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
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        return 1;
                    }
                    catch (Exception e)
                    {
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


                        var i = dbEntity.SpUpdatePurchaseOrder(LocalPurchaseInvoiceBO.ID,
                        LocalPurchaseInvoiceBO.SupplierID,"" , 0, 0,
                        LocalPurchaseInvoiceBO.PurchaseOrderDate, 0, 0, 0,
                        null,
                        LocalPurchaseInvoiceBO.IsDraft,
                        LocalPurchaseInvoiceBO.Remarks,
                        "", "", "", "", "", "", 0, 0, 0,
                        LocalPurchaseInvoiceBO.SupplierReference,
                        null, 0, 0,
                        LocalPurchaseInvoiceBO.GSTAmount,
                        LocalPurchaseInvoiceBO.GSTAmount,
                        0, 0, 0, 0, 0,
                        LocalPurchaseInvoiceBO.Discount,
                        LocalPurchaseInvoiceBO.NetAmount,
                        LocalPurchaseInvoiceBO.GrossAmnt,
                        0,
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
                            dbEntity.SpCreateGoodsReceiptNoteForLocalPurchase(
                            SerialNoGRN.Value.ToString(),
                            LocalPurchaseInvoiceBO.ID,
                            GeneralBO.LocationID,
                            GRNID
                            );

                            if (Convert.ToInt32(GRNID.Value) != 0)
                            {
                                dbEntity.SpCreateGoodsReceiptNoteTransForLocalPurchase(
                                Convert.ToInt16(GRNID.Value),
                                LocalPurchaseInvoiceBO.ID,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                                );

                                dbEntity.SpCreatePurchaseInvoiceForLocalPurchase(
                                SerialNoPurchaseInvoice.Value.ToString(),
                                LocalPurchaseInvoiceBO.ID,
                                LocalPurchaseInvoiceBO.SupplierReference,
                                LocalPurchaseInvoiceBO.NetAmount,
                                "", null,
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

        public DatatableResultBO GetLocalPurchases(string Type, string TransNoHint, string TransDateHint, string SupplierHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetLocalPurchases(Type, TransNoHint, TransDateHint, SupplierHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Status = item.Status
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

        public LocalPurchaseInvoiceBO GetLocalPurchaseOrder(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetLocalPurchaseOrder(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new LocalPurchaseInvoiceBO
                    {
                        PurchaseOrderNo = k.PurchaseOrderNo,
                        PurchaseOrderDate = (DateTime)k.PurchaseOrderDate,
                        IsDraft = (bool)k.IsDraft,
                        ID = k.ID,
                        SupplierReference = k.SupplierReferenceNo,
                        NetAmount = (decimal)k.NetAmt,
                        GSTAmount = (decimal)k.SGSTAmt
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PurchaseOrderTransBO> GetLocalPurchaseOrderItems(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetLocalPurchaseOrderItems(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new PurchaseOrderTransBO
                    {
                        ItemID = (int)k.ItemID,
                        Unit = k.Unit,
                        UnitID = (int)k.UnitID,
                        QtyOrdered = (decimal)k.Quantity,
                        Rate = (decimal)k.Rate,
                        Remarks = k.Remarks,
                        Amount = k.Amount,
                        CGSTAmt = k.CGSTAMT,
                        CGSTPercent = k.CGSTPercent,
                        SGSTAmt = k.SGSTAmt,
                        SGSTPercent = k.SGSTPercent,
                        NetAmount = (decimal)k.NetAmount,
                        Name = k.ItemName,
                        HSNCode = k.HSNCode,
                        GSTPercentage = (decimal)k.GSTPercen,
                        IGSTAmt = (decimal)k.GSTAMT
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LocalPurchaseInvoiceBO GetLocalPurchaseID()
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetLocalPurchaseID(GeneralBO.ApplicationID).Select(k => new LocalPurchaseInvoiceBO
                    {
                        SupplierID = (int)k.ID,
                        IsGSTRegistered = (bool)k.IsGSTRegistered
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetIsLocalPurchase(int id)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    ObjectParameter IsExist = new ObjectParameter("IsExist", typeof(bool));
                    var i = dbEntity.SpIsLocalPurchaseID(id, GeneralBO.ApplicationID, IsExist);
                    return Convert.ToBoolean(IsExist.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
