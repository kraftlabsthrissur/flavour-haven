//File craeted by prama on 26-4-18
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
    public class PurchaseReturnDAL
    {
        public GRNTransItemBO GetGRNTransForPurchaseReturn(int ItemID, int GRNID)
        {
            GRNTransItemBO GrnTrans = new GRNTransItemBO();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                return dEntity.SpGetGRNTransForPurchaseReturn(ItemID, GRNID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNTransItemBO
                {
                    AcceptedQty = a.AcceptedQty,
                    Rate = (decimal)a.Rate,
                    IGSTPercent = (decimal)a.IGSTPercent,
                    SGSTPercent = (decimal)a.SGSTPercent,
                    CGSTPercent = (decimal)a.CGSTPercent,
                    Remarks = a.Remarks,

                }).FirstOrDefault();


            }

        }

        public bool SavePurchaseReturn(PurchaseReturnBO purchaseReturnBO, List<PurchaseReturnTransItemBO> TransItems)

        {
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "PurchaseReturn";
                        ObjectParameter PrId = new ObjectParameter("PurchaseReturnID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int)); 
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (purchaseReturnBO.IsDraft)
                        {
                            FormName = "DraftPurchaseReturn";
                        }

                        var j = dEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var CreatedUserID = GeneralBO.CreatedUserID;
                        dEntity.SaveChanges();

                        var i = dEntity.SpCreatePurchaseReturn(
                            SerialNo.Value.ToString(),
                            purchaseReturnBO.ReturnDate,
                            purchaseReturnBO.SupplierID,
                            purchaseReturnBO.SGSTAmount,
                            purchaseReturnBO.CGSTAmount,
                            purchaseReturnBO.IGSTAmount,
                            0,
                            0,
                            0,
                            purchaseReturnBO.NetAmount,
                            purchaseReturnBO.IsDraft,
                            false,
                            purchaseReturnBO.ReturnDate,
                            GeneralBO.CreatedUserID,
                             purchaseReturnBO.ReturnDate,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID,
                             PrId);

                        dEntity.SaveChanges();

                        if (PrId.Value != null)
                        {
                            foreach (var itm in TransItems)
                            {

                                dEntity.SpCreatePurchaseReturnTrans(
                                    Convert.ToInt32(PrId.Value),
                                    itm.InvoiceID,0,
                                    itm.ItemID,
                                    itm.Quantity,
                                    0,
                                    itm.Rate,
                                    itm.SGSTPercent,
                                    itm.CGSTPercent,
                                    itm.IGSTPercent,
                                    itm.SGSTAmount,
                                    itm.CGSTAmount,
                                    itm.IGSTAmount,
                                    itm.Amount,
                                    0,
                                    itm.SecondaryUnitSize,
                                    itm.SecondaryUnit,
                                    itm.SecondaryReturnQty,
                                    itm.SecondaryRate,
                                    itm.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    itm.WarehouseID,
                                    itm.BatchTypeID,
                                    itm.UnitID,
                                    itm.VATPercentage,
                                    itm.VATAmount,
                                    itm.PurchaseReturnOrderID,
                                    RetValue
                                        );
                            }
                            if (Convert.ToInt32(RetValue.Value) == -2)
                            {
                                throw new Exception("Item out of stock");
                            }
                        };
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        public bool UpdatePurchaseReturn(PurchaseReturnBO purchaseReturnBO, List<PurchaseReturnTransItemBO> TransItems)

        {
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(bool));

                    try
                    {

                        var i = dEntity.SpUpdatePurchaseReturn(
                           purchaseReturnBO.Id,
                            purchaseReturnBO.ReturnDate,
                            purchaseReturnBO.SGSTAmount,
                            purchaseReturnBO.CGSTAmount,
                            purchaseReturnBO.IGSTAmount,
                            purchaseReturnBO.NetAmount,
                            purchaseReturnBO.IsDraft,
                            GeneralBO.CreatedUserID,
                             purchaseReturnBO.ReturnDate,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID
                             );

                        dEntity.SaveChanges();


                        foreach (var itm in TransItems)
                        {

                            dEntity.SpCreatePurchaseReturnTrans(
                                 purchaseReturnBO.Id,
                                itm.InvoiceID,
                                0,
                                itm.ItemID,
                                itm.Quantity,
                                0,
                                itm.Rate,
                                itm.SGSTPercent,
                                itm.CGSTPercent,
                                itm.IGSTPercent,
                                itm.SGSTAmount,
                                itm.CGSTAmount,
                                itm.IGSTAmount,
                                itm.Amount,
                                0,
                                itm.SecondaryUnitSize,
                                itm.SecondaryUnit,
                                itm.SecondaryReturnQty,
                                itm.SecondaryRate,
                                itm.Remarks,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                itm.WarehouseID,
                                itm.BatchTypeID,
                                itm.UnitID,
                                0,0,
                                itm.PurchaseReturnOrderID,
                                RetValue
                                    );


                        };
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public List<PurchaseReturnBO> GetPurchaseReturnList(int ID)
        {
            List<PurchaseReturnBO> list = new List<PurchaseReturnBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetPurchaseReturn(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseReturnBO
                {
                    Id = a.ID,
                    ReturnNo = a.RETURNNO,
                    ReturnDate = (DateTime)a.CreatedDate,
                    SupplierName = a.suppliername,
                    Freight = (decimal)a.FreightAmount,
                    PackingCharges = (decimal)a.PackingCahrge,
                    OtherCharges = (decimal)a.OtherCharge,
                    NetAmount = (decimal)a.NetAmount,
                    IsDraft = (bool)a.IsDraft,
                    SupplierID = (int)a.SupplierID,
                    IsProcessed = (bool)a.IsProcessed
                }).ToList();
                return list;
            }

        }

        public List<PurchaseReturnTransItemBO> GetPurchaseReturnTransList(int ID)
        {
            List<PurchaseReturnTransItemBO> list = new List<PurchaseReturnTransItemBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                try
                {
                    list = dEntity.SpGetPurchaseReturnTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseReturnTransItemBO
                    {
                        ItemName = a.ITEMNAME,
                        ReturnNo = a.PurchaseReturnOrderNo,
                        ItemID = (int)a.ItemID,
                        UnitID = (int)a.UnitID,
                        Unit = a.unit,
                        AcceptedQty = (decimal)a.AcceptedQty,
                        Quantity = (decimal)a.QTY,
                        Rate = a.Rate,
                        SGSTAmount = (decimal)a.SGSTAmount,
                        SGSTPercent = (decimal)a.SGSTPercent,
                        IGSTAmount = (decimal)a.IGSTAmount,
                        IGSTPercent = (decimal)a.IGSTPercent,
                        CGSTAmount = (decimal)a.CGSTAmount,
                        CGSTPercent = (decimal)a.CGSTPercent,
                        Amount = a.Amount,
                        Remarks = a.Remarks,
                        BatchTypeID = (int)a.BatchTypeID,
                        Stock = (decimal)a.Stock,
                        InvoiceID = (int)a.InvoiceID,
                        PurchaseReturnOrderID = (int)a.ReturnOrderID,
                        PurchaseReturnOrderTransID = (int)a.ReturnOrderTransID,
                        WarehouseID = (int)a.WarehouseID,
                        GRNQty=(decimal)a.GRNQty
                    }).ToList();
                    return list;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

        public List<PurchaseReturnBO> GetPurchaseReturnAutocompleteByID(string term, int ItemID, int SupplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseReturnAutocompleteByID(ItemID, SupplierID, term, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.LocationID).Select(a => new PurchaseReturnBO
                    {
                        Id = a.ID,
                        ReturnNo = a.Code

                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<PurchaseReturnBO> GetPurchaseReturnListForIRG(int SupplierID)
        {

            PurchaseReturnBO invoiceTrans = new PurchaseReturnBO();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                return dEntity.SpGetPurchaseReturnForIRG(SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseReturnBO
                {
                    ReturnNo = m.PurchaseNo,
                    ReturnDate = (DateTime)m.PurchaseDate,
                    NetAmount = (decimal)m.InvoiceTotal,
                    Id = m.ID
                }).ToList();


            }
        }

        public List<PurchaseReturnTransItemBO> GetPurchaseReturnForIRG(int returnid)
        {
            List<PurchaseReturnTransItemBO> list = new List<PurchaseReturnTransItemBO>();

            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetPurchaseReturnTransForIRG(returnid, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseReturnTransItemBO
                {
                    PurchaseReturnOrderID = (int)m.PurchaseReturnID,
                    PurchaseReturnOrderTransID = m.PurchaseReturnTransID,
                    ItemID = m.ItemID,
                    ItemName = m.ItemName,
                    Unit = m.Unit,
                    UnitID = (int)m.UnitID,
                    Rate = (decimal)m.Rate,
                    AcceptedQty = (decimal)m.Qty,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    ReturnNo = m.ReturnNo,
                    Remarks = (m.Remarks == null) ? "" : m.Remarks,
                    WarehouseID = (int)m.warehouseID,
                    InvoiceID = (int)m.InvoiceID,
                    InvoiceQty=(decimal)m.InvoiceQty

                }).ToList();
                return list;
            }

        }

        public DatatableResultBO GetPurchaseReturn(string Type, string TransNo, string TransDate, string SupplierName, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetPurchaseReturnList(Type, TransNo, TransDate, SupplierName, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransNo = item.RETURNNO,
                                TransDate = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                SupplierName=item.SupplierName,
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

