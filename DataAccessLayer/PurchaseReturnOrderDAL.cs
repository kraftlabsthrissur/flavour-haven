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
    public class PurchaseReturnOrderDAL
    {
        public string SavePurchaseReturn(PurchaseReturnBO purchaseReturnBO, List<GRNTransItemBO> grnTransItems)

        {
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "PurchaseReturn";
                        ObjectParameter PurchaseReturnOrderID = new ObjectParameter("PurchaseReturnOrderID", typeof(int));
                        ObjectParameter PurchaseReturnID = new ObjectParameter("PurchaseReturnID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(bool));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (purchaseReturnBO.IsDraft)
                        {
                            FormName = "DraftPurchaseReturn";
                        }

                        var j = dEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var CreatedUserID = GeneralBO.CreatedUserID;
                        dEntity.SaveChanges();

                        var i = dEntity.SpCreatePurchaseReturnOrder(
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
                            purchaseReturnBO.Discount,
                            purchaseReturnBO.VATPercentage,
                            purchaseReturnBO.CurrencyID,
                            purchaseReturnBO.IsVat,
                            purchaseReturnBO.IsGST,
                            purchaseReturnBO.CurrencyExchangeRate,
                            0,
                            0,
                            purchaseReturnBO.IsDraft,
                            false,
                            purchaseReturnBO.ReturnDate,
                            GeneralBO.CreatedUserID,
                             purchaseReturnBO.ReturnDate,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID,
                             PurchaseReturnOrderID,
                             PurchaseReturnID);

                        dEntity.SaveChanges();

                        if (PurchaseReturnOrderID.Value != null)
                        {
                            foreach (var itm in grnTransItems)
                            {

                                dEntity.SpCreatePurchaseReturnTrans(
                                    Convert.ToInt32(PurchaseReturnOrderID.Value),
                                    itm.InvoiceID,
                                    itm.InvoiceTransID,
                                    itm.ItemID,
                                    itm.Quantity,
                                    itm.OfferQty,
                                    itm.Rate,
                                    itm.SGSTPercent,
                                    itm.CGSTPercent,
                                    itm.IGSTPercent,
                                    itm.SGSTAmt,
                                    itm.CGSTAmt,
                                    itm.IGSTAmt,
                                    itm.Amount,
                                    itm.Discount,
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
                                    Convert.ToInt32(PurchaseReturnID.Value),
                                    RetValue
                                        );
                            }
                            if (Convert.ToBoolean(RetValue.Value) == true)
                            {
                                throw new Exception("Item out of stock");
                            }
                        };
                        transaction.Commit();
                        return PurchaseReturnOrderID.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public bool UpdatePurchaseOrderReturn(PurchaseReturnBO purchaseReturnBO, List<GRNTransItemBO> grnTransItems)

        {
            bool IsSuccess = false;
            ObjectParameter PurchaseReturnID = new ObjectParameter("PurchaseReturnID", typeof(int));
            ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(bool));
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        var j = dEntity.SpUpdatePurchaseReturnOrder(purchaseReturnBO.Id, purchaseReturnBO.SupplierID, purchaseReturnBO.SGSTAmount, purchaseReturnBO.CGSTAmount,
                                                                  purchaseReturnBO.IGSTAmount, purchaseReturnBO.Freight, purchaseReturnBO.OtherCharges, purchaseReturnBO.PackingCharges,
                                                                  purchaseReturnBO.NetAmount, purchaseReturnBO.Discount, purchaseReturnBO.IsDraft, purchaseReturnBO.IsProcessed, GeneralBO.CreatedUserID,
                                                                  GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, PurchaseReturnID);
                        foreach (var itm in grnTransItems)
                        {
                            var i = dEntity.SpCreatePurchaseReturnTrans(
                                    purchaseReturnBO.Id,
                                    itm.InvoiceID,
                                    itm.InvoiceTransID,
                                    itm.ItemID,
                                    itm.Quantity,
                                    itm.OfferQty,
                                    itm.Rate,
                                    itm.SGSTPercent,
                                    itm.CGSTPercent,
                                    itm.IGSTPercent,
                                    itm.SGSTAmt,
                                    itm.CGSTAmt,
                                    itm.IGSTAmt,
                                    itm.Amount,
                                    itm.Discount,
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
                                    Convert.ToInt32(PurchaseReturnID.Value),
                                    RetValue
                                        );
                            if (Convert.ToBoolean(RetValue.Value) == true)
                            {
                                throw new Exception("Item out of stock");
                            }

                        }
                        transaction.Commit();
                        IsSuccess = true;
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();

                    }
                }
                return IsSuccess;
            }
        }

        public List<PurchaseReturnBO> GetPurchaseReturnList()
        {
            List<PurchaseReturnBO> list = new List<PurchaseReturnBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetPurchaseReturnOrder(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseReturnBO
                {
                    Id = a.ID,
                    ReturnNo = a.RETURNNO,
                    ReturnDate = (DateTime)a.CreatedDate,
                    SupplierName = a.suppliername,
                    Freight = (decimal)a.FreightAmount,
                    PackingCharges = (decimal)a.PackingCahrge,
                    OtherCharges = (decimal)a.OtherCharge,
                    NetAmount = (decimal)a.NetAmount,
                    IsProcessed = (bool)a.IsProcessed,
                    IsDraft = (bool)a.IsDraft
                }).ToList();
                return list;
            }

        }

        public List<PurchaseReturnBO> GetPurchaseReturnOrderList(int SupplierID)
        {

            PurchaseReturnBO invoiceTrans = new PurchaseReturnBO();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                return dEntity.SpGetPurchaseReturnOrderForPurchaseReturn(SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseReturnBO
                {
                    ReturnNo = m.asPurchaseNo,
                    ReturnDate = (DateTime)m.PurchaseDate,
                    NetAmount = (decimal)m.InvoiceTotal,
                    Id = m.ID
                }).ToList();


            }
        }

        public List<PurchaseReturnTransItemBO> GetPurchaseReturnOrder(int orderid)
        {
            List<PurchaseReturnTransItemBO> list = new List<PurchaseReturnTransItemBO>();

            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetPurchaseReturnOrderItemForPurchaseReturn(orderid, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseReturnTransItemBO
                {
                    PurchaseReturnOrderID = (int)m.PurchaseReturnOrderID,
                    PurchaseReturnOrderTransID = m.PurchaseReturnOrderTransID,
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
                    Stock = (decimal)m.Stock,
                    InvoiceID = (int)m.InvoiceID,
                    WarehouseID = (int)m.warehouseID,
                    BatchTypeID = (int)m.BatchTypeID,
                    InvoiceQty = (decimal)m.InvoiceQty,
                    GRNQty = (decimal)m.GRNQty
                }).ToList();
                return list;
            }

        }
        public List<PurchaseReturnBO> GetPurchaseReturnDetail(int ID)
        {
            List<PurchaseReturnBO> list = new List<PurchaseReturnBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetPurchaseReturnOrderDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseReturnBO
                {
                    Id = a.ID,
                    ReturnNo = a.RETURNNO,
                    ReturnDate = (DateTime)a.Date,
                    SupplierName = a.suppliername,
                    SupplierID = (int)a.SupplierID,
                    Freight = (decimal)a.FreightAmount,
                    PackingCharges = (decimal)a.PackingCahrge,
                    OtherCharges = (decimal)a.OtherCharge,
                    GrossAmount = (decimal)a.NetAmount + (decimal)a.Discount,
                    NetAmount = (decimal)a.NetAmount,
                    IsDraft = (bool)a.IsDraft,
                    Discount = (decimal)a.Discount,
                    IsGSTRegistred = (bool)a.IsGSTRegistered,
                    StateID = (int)a.StateID,
                    IGSTAmount = (decimal)a.IGSTAmount,
                    SGSTAmount = (decimal)a.SGSTAmount,
                    CGSTAmount = (decimal)a.CGSTAmount,
                    GSTNo = a.GSTNo,
                    State = a.State,
                    Addresses1 = a.Addresses1,
                    Addresses2 = a.Addresses2
                }).ToList();
                return list;
            }

        }
        public List<GRNTransItemBO> GetPurchaseReturnTransList(int ID)
        {
            List<GRNTransItemBO> list = new List<GRNTransItemBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetPurchaseReturnOrderTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNTransItemBO
                {
                    ItemName = a.ItemName,
                    ItemCode = a.ItemCode,
                    PartsNumber = a.PartsNumber,
                    Remark = a.Remarks,
                    Model = a.Model,
                    CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                    PurchaseInvoiceID = a.PurchaseInvoiceID,
                    PurchaseNo = a.PurchaseNo,
                    Unit = a.unit,
                    UnitID = (int)a.UnitID,
                    ItemID = a.ItemID,
                    AcceptedQty = (decimal)a.AcceptedQty,
                    ReturnQty = a.QTY,
                    Rate = a.Rate,
                    GrossAmount = a.Amount + a.Discount,
                    SGSTAmt = a.SGSTAmount,
                    SGSTPercent = a.SGSTPercent,
                    IGSTAmt = a.IGSTAmount,
                    IGSTPercent = a.IGSTPercent,
                    CGSTAmt = a.CGSTAmount,
                    CGSTPercent = a.CGSTPercent,
                    Amount = a.Amount,
                    Remarks = a.Remarks,
                    InvoiceID = a.PurchaseInvoiceID,
                    Stock = (decimal)a.Stock,
                    WarehouseID = (int)a.warehouseID,
                    ConvertedQty = (decimal)a.ConvertedQuantity,
                    ConvertedStock = (decimal)a.ConvertedStock,
                    PrimaryUnitID = (int)a.PrimaryUnitID,
                    PurchaseUnitID = (int)a.PurchaseUnitID,
                    PurchaseUnit = a.PurchaseUnit,
                    PrimaryUnit = a.PrimaryUnit,
                    InvoiceQty = (decimal)a.InvoiceQty,
                    InvoiceTransID = (int)a.InvoiceTransID,
                    InvoiceNo = a.InvoiceNo,
                    GSTPercentage = a.GSTPercent,
                    GSTAmount = a.GSTAmount,
                    GSTID = a.GSTID,
                    Discount = a.Discount,
                    OfferQty = (decimal)a.OfferQty,
                    OfferReturnQty = (decimal)a.OfferReturnQty,
                    SecondaryReturnQty = a.SecondaryReturnQty,
                    SecondaryRate = a.SecondaryRate,
                    SecondaryUnit = a.SecondaryUnit,
                    SecondaryUnitSize = a.SecondaryUnitSize,
                    VATPercentage = (decimal)a.VATPercentage,
                    VATAmount = (decimal)a.VATAmount,
                }).ToList();


                return list;
            }

        }

        public DatatableResultBO GetPurchaseReturnOderListForDataTable(string Type, string TransNo, string TransDate, string SupplierName, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetPurchaseReturnOderListForDataTable(Type, TransNo, TransDate, SupplierName, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                SupplierName = item.SupplierName,
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

