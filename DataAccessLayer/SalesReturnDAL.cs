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
    public class SalesReturnDAL
    {
        public List<SalesReturnBO> GetSalesReturnList()
        {
            List<SalesReturnBO> list = new List<SalesReturnBO>();
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetSalesReturn(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesReturnBO
                {
                    ID = a.ID,
                    SRNo = a.Code,
                    SRDate = (DateTime)a.TranDate,
                    CustomerName = a.CustomerName,
                    IsCancelled = a.Cancelled,
                    IsDraft = (bool)a.IsDraft
                }).ToList();


                return list;
            }

        }
        public List<SalesReturnBO> GetSalesReturn(int ID)
        {
            List<SalesReturnBO> list = new List<SalesReturnBO>();
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetSalesReturnDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesReturnBO
                {
                    ID = a.ID,
                    SRNo = a.Code,
                    CurrencyCode = a.CurrencyCode,
                    SRDate = (DateTime)a.TranDate,
                    CustomerName = a.CustomerName.TrimStart(),
                    NetAmount = (decimal)a.NetAmount,
                    IsDraft = (bool)a.IsDraft,
                    IsCancelled = a.Cancelled,
                    CustomerID = (int)a.CustomerID,
                    GrossAmount = (decimal)a.GrossAmount,
                    SGSTAmount = (decimal)a.SGSTAmount,
                    CGSTAmount = (decimal)a.CGSTAmount,
                    IGSTAmount = (decimal)a.IGSTAmount,
                    RoundOff = (decimal)a.RoundOff,
                    SalesInvoiceID = (int)a.SalesInvoiceID,
                    InvoiceNo = a.InvoiceNo,
                    IsNewInvoice = (bool)a.IsNewInvoice,
                    AmountInWords = a.AmountInWords,
                    DecimalPlaces = (int)a.DecimalPlaces,
                }).ToList();
                return list;
            }

        }
        public List<SalesItemBO> GetSalesReturnTrans(int ID)
        {
            List<SalesItemBO> list = new List<SalesItemBO>();
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetSalesReturnTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO
                {
                    ItemID = a.ItemID,
                    Name = a.ItemName,
                    MRP = (decimal)a.MRP,
                    SecondaryMRP = a.SecondaryMRP,
                    SecondaryQty = a.SecondaryQty,
                    SecondaryUnit = a.SecondaryUnit,
                    SecondaryOfferQty = a.SecondaryOfferQty,
                    SecondaryUnitSize = a.SecondaryUnitSize,
                    BasicPrice = (decimal)a.BasicPrice,
                    SaleQty = a.Qty,
                    OfferQty = a.OfferQty,
                    DiscountPercentage = (decimal)a.DiscountPercentage,
                    DiscountAmount = (decimal)a.DiscountAmount,
                    GrossAmount = (decimal)a.GrossAmount,
                    CGST = (decimal)a.CGSTAmt,
                    IGST = (decimal)a.IGSTAmt,
                    SGST = (decimal)a.SGSTAmt,
                    IGSTPercentage = (decimal)a.IGSTPercentage,
                    CGSTPercentage = (decimal)a.CGSTPercentage,
                    SGSTPercentage = (decimal)a.SGSTPercentage,
                    NetAmount = (decimal)a.NetAmt,
                    Unit = a.Unit,
                    UnitID = (int)a.UnitID,
                    Qty = (decimal)a.ReturnQty,
                    Code = a.ItemCode,
                    BatchName = a.BatchNo,
                    BatchID = (int)a.BatchID,
                    OfferReturnQty = (decimal)a.ReturnOfferQty,
                    TransNo = a.TransNo,
                    SalesInvoiceTransID = (int)a.SalesInvoiceTransID,
                    SalesUnitID = (int)a.SalesUnitID,
                    PrimaryUnitID = (int)a.PrimaryUnitID,
                    LoosePrice = (decimal)a.LooseRate,
                    FullPrice = (decimal)a.FullRate,
                    ConvertedQuantity = (decimal)a.ConvertedQuantity,
                    SalesInvoiceQty = (decimal)a.InvoiceQuantity,
                    SalesTransUnitID = (int)a.CounterSalesTransUnitID,
                    SalesUnit = a.SalesUnit,
                    PrimaryUnit = a.PrimaryUnit,
                    LogicCodeID = (int)a.LogicCodeID,
                    LogicCode = a.LogicCode,
                    LogicName = a.LogicName,
                    ConvertedOfferQuantity = (decimal)a.ConvertedOfferQuantity,
                    SalesOfferQty = (decimal)a.InvoiceOfferQuantity,
                    BatchTypeID = (int)a.BatchtypeID,
                    VATPercentage = (decimal)a.VATPercentage,
                    VATAmount = (decimal)a.VATAmount,
                    Make = a.Make
                }).ToList();


                return list;
            }

        }


        public bool SaveSalesReturn(SalesReturnBO salesReturnBO, List<SalesItemBO> salesItems)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "SalesReturn";
                        ObjectParameter SrId = new ObjectParameter("SalesReturnID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (salesReturnBO.IsDraft)
                        {
                            FormName = "DraftSalesReturn";
                        }

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        dbEntity.SaveChanges();
                        var i = dbEntity.SpCreateSalesReturn(SerialNo.Value.ToString(),
                            salesReturnBO.SRDate,
                            salesReturnBO.CustomerID,
                            salesReturnBO.GrossAmount,
                            salesReturnBO.TaxableAmount,
                            salesReturnBO.SGSTAmount,
                            salesReturnBO.CGSTAmount,
                            salesReturnBO.IGSTAmount,
                            salesReturnBO.RoundOff,
                            salesReturnBO.NetAmount,
                            salesReturnBO.IsDraft,
                            salesReturnBO.IsCancelled,
                            salesReturnBO.CancelledDate,
                            salesReturnBO.SalesInvoiceID,
                            salesReturnBO.InvoiceNo,
                            salesReturnBO.IsNewInvoice,
                             GeneralBO.CreatedUserID,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SrId);

                        //   dbEntity.SaveChanges();
                        if (SrId.Value != null)
                        {
                            foreach (var item in salesItems)
                            {
                                dbEntity.SpCreateSalesReturnTrans
                                    (Convert.ToInt32(SrId.Value),
                                    item.SalesInvoiceID,
                                    item.TransNo,
                                    item.ItemID,
                                    item.SaleQty,
                                    item.Rate,
                                    item.MRP,
                                    item.SecondaryQty,
                                    item.SecondaryUnit,
                                    item.SecondaryUnitSize,
                                    item.SecondaryMRP,
                                    item.BasicPrice,
                                    item.OfferQty,
                                    item.GrossAmount,
                                    item.ID,
                                    item.DiscountPercentage,
                                    item.DiscountAmount,
                                    item.TaxableAmount,
                                    item.SGSTPercentage,
                                    item.CGSTPercentage,
                                    item.IGSTPercentage,
                                    item.SGST,
                                    item.CGST,
                                    item.IGST,
                                    item.NetAmount,
                                    item.Qty,
                                    item.OfferReturnQty,
                                    item.BatchTypeID,
                                    item.BatchID,
                                    item.BatchName,
                                    item.UnitID,
                                    item.SalesInvoiceTransID,
                                    item.LogicCodeID,
                                    item.VATPercentage,
                                    item.VATAmount,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    );
                            }
                        };
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;

                    }
                }
            }
        }

        public bool UpdateSalesReturn(SalesReturnBO salesReturnBO, List<SalesItemBO> salesItems)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var i = dbEntity.SpUpdateSalesReturn(
                            salesReturnBO.ID,
                            DateTime.Now,
                            salesReturnBO.CustomerID,
                            salesReturnBO.GrossAmount,
                            salesReturnBO.TaxableAmount,
                            salesReturnBO.SGSTAmount,
                            salesReturnBO.CGSTAmount,
                            salesReturnBO.IGSTAmount,
                            salesReturnBO.RoundOff,
                            salesReturnBO.NetAmount,
                            salesReturnBO.IsDraft,
                            salesReturnBO.SalesInvoiceID,
                            salesReturnBO.InvoiceNo,
                            salesReturnBO.IsNewInvoice,
                             GeneralBO.CreatedUserID,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID
                           );

                        //dbEntity.SaveChanges();

                        foreach (var item in salesItems)
                        {
                            dbEntity.SpCreateSalesReturnTrans
                                    (salesReturnBO.ID,
                                    item.SalesInvoiceID,
                                    item.TransNo,
                                    item.ItemID,
                                    item.SaleQty,
                                    item.Rate,
                                    item.MRP,
                                    item.SecondaryQty,
                                    item.SecondaryUnit,
                                    item.SecondaryUnitSize,
                                    item.SecondaryMRP,
                                    item.BasicPrice,
                                    item.OfferQty,
                                    item.GrossAmount,
                                    item.ID,
                                    item.DiscountPercentage,
                                    item.DiscountAmount,
                                    item.TaxableAmount,
                                    item.SGSTPercentage,
                                    item.CGSTPercentage,
                                    item.IGSTPercentage,
                                    item.SGST,
                                    item.CGST,
                                    item.IGST,
                                    item.NetAmount,
                                    item.Qty,
                                    item.OfferReturnQty,
                                    item.BatchTypeID,
                                    item.BatchID,
                                    item.BatchName,
                                    item.UnitID,
                                    item.SalesInvoiceTransID,
                                    item.LogicCodeID,
                                    item.VATPercentage,
                                    item.VATAmount,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    );
                        };
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;

                    }
                }
            }
        }

        public List<SalesReturnBO> GetSalesReturnLogicCodeList()
        {
            List<SalesReturnBO> SalesReturnLogicCode = new List<SalesReturnBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    SalesReturnLogicCode = dbEntity.SpGetSalesReturnLogicCode().Select(a => new SalesReturnBO
                    {
                        LogicCodeID = a.ID,
                        LogicCode = a.Code,
                        LogicName = a.Code + " : " + a.Name
                    }).ToList();
                }
                return SalesReturnLogicCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetSalesReturnListForDataTable(string Type, string ReturnNo, string ReturnDate, string CustomerName, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetSalesReturnListForDataTable(Type, ReturnNo, ReturnDate, CustomerName, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                ReturnNo = item.Code,
                                ReturnDate = ((DateTime)item.TranDate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
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
