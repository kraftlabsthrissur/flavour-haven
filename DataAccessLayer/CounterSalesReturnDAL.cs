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
    public class CounterSalesReturnDAL
    {
        public List<CounterSalesReturnBO> GetSalesReturnList()
        {
            List<CounterSalesReturnBO> list = new List<CounterSalesReturnBO>();
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetCounterSalesReturn(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CounterSalesReturnBO
                {
                    ID = a.ID,
                    ReturnNo = a.TransNo,
                    ReturnDate = (DateTime)a.TransDate,


                    IsDraft = (bool)a.IsDraft
                }).ToList();


                return list;
            }

        }
        public List<CounterSalesReturnBO> GetCounterSalesReturn(int ID)
        {
            List<CounterSalesReturnBO> list = new List<CounterSalesReturnBO>();
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetCounterSalesReturnDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CounterSalesReturnBO
                {
                    ID = a.ID,
                    ReturnNo = a.TransNo,
                    ReturnDate = (DateTime)a.TransDate,
                    NetAmount = (decimal)a.NetAmount,
                    IsDraft = (bool)a.IsDraft,
                    SGSTAmount = (decimal)a.SGSTAmount,
                    CGSTAmount = (decimal)a.CGSTAmount,
                    IGSTAmount = (decimal)a.IGSTAmount,
                    RoundOff = (decimal)a.RoundOff,
                    BankID = a.BankID,
                    BankName = a.BankName,
                    PaymentModeID = (int)a.PaymentModeID,
                    PaymentMode = a.PaymentMode,
                    InvoiceID = (int)a.InvoiceID,
                    PartyName = a.PartyName,
                    InvoiceNo = a.InvoiceNo,
                    Reason = a.Reason,
                    BillDiscount = (decimal)a.BillDiscount,
                    PartyID = (int)a.CustomerID,
                    AmountInWords = a.AmountInWords,
                    DecimalPlaces = (int)a.DecimalPlaces,
                    currencyCode = a.currencyCode,
                    VATAmount = (decimal)a.VATAmount,
                }).ToList();
                return list;
            }

        }
        public List<CounterSalesReturnItemBO> GetCounterSalesReturnTrans(int ID)
        {
            List<CounterSalesReturnItemBO> list = new List<CounterSalesReturnItemBO>();
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetCounterSalesReturnTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new CounterSalesReturnItemBO
                {
                    FullOrLoose = m.FullOrLoose,
                    ItemID = (int)m.ItemID,
                    Name = m.ItemName,
                    BatchID = (int)m.BatchID,
                    Quantity = (decimal)m.Quantity,
                    Rate = (decimal)m.Rate,
                    MRP = (decimal)m.MRP,
                    GrossAmount = (decimal)m.GrossAmount,
                    SGSTAmount = (decimal)m.SGSTAmount,
                    CGSTAmount = (decimal)m.CGSTAmount,
                    IGSTAmount = (decimal)m.IGSTAmount,
                    NetAmount = (decimal)m.NetAmount,
                    IGSTPercentage = (decimal)m.IGSTPercent,
                    SGSTPercentage = (decimal)m.SGSTPercent,
                    CGSTPercentage = (decimal)m.CGSTPercent,
                    Code = m.Code,
                    BatchNo = m.BatchNo,
                    BatchTypeID = (int)m.BatchTypeID,
                    WareHouseID = (int)m.WarehouseID,
                    ExpiryDate = (DateTime)m.ExpiryDate,
                    Unit = m.Unit,
                    TaxableAmount = (decimal)m.TaxableAmount,
                    ReturnQty = (decimal)m.ReturnQty,
                    CounterSalesTransID = (int)m.CounterSalesTransID,
                    UnitID = (int)m.UnitID,
                    SalesUnitID = (int)m.SalesUnitID,
                    PrimaryUnitID = (int)m.PrimaryUnitID,
                    LoosePrice = (decimal)m.LooseRate,
                    FullPrice = (decimal)m.FullRate,
                    ConvertedQuantity = (decimal)m.ConvertedQuantity,
                    CounterSalesQty = (decimal)m.CounterSalesQuantity,
                    CounterSalesTransUnitID = (int)m.CounterSalesTransUnitID,
                    SalesUnitName = m.SalesUnit,
                    PrimaryUnit = m.PrimaryUnit,
                    CessPercentage = m.CessPercentage,
                    CessAmount = m.CessAmount,
                    SecondaryRate = m.SecondaryRate,
                    SecondaryUnitSize = m.SecondaryUnitSize,
                    SecondaryReturnQty = m.SecondaryReturnQty,
                    SecondaryUnit = m.SecondaryUnit,
                    VATPercentage = (decimal)m.VATPercentage,
                    VATAmount = (decimal)m.VATAmount,
                    DiscountPercentage = (decimal)m.DiscountPercentage,
                    DiscountAmount = (decimal)m.DiscountAmount,
                    Make = m.Make,
                    DecimalPlaces=(int)m.DecimalPlaces

                }).ToList();


                return list;
            }

        }


        public bool SaveCounterSalesReturn(CounterSalesReturnBO counterSalesBO, string XMLItem)
        {
            using (SalesEntities salesEntity = new SalesEntities())
            {
                using (var transaction = salesEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "CountersalesReturn";
                        ObjectParameter PrId = new ObjectParameter("CounterSalesID", typeof(long));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ERR = new ObjectParameter("ERR", typeof(string));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));

                        if (counterSalesBO.IsDraft)
                        {
                            FormName = "DraftCountersalesReturn";
                        }
                        var j = salesEntity.SpUpdateSerialNo(
                                        FormName,
                                        "Code",
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        SerialNo);
                        var CreatedUserID = GeneralBO.CreatedUserID;
                        salesEntity.SaveChanges();
                        var i = salesEntity.SpCreateCounterSalesReturn(
                            SerialNo.Value.ToString(),
                            counterSalesBO.ReturnDate,
                             counterSalesBO.IsDraft,
                            counterSalesBO.SGSTAmount,
                            counterSalesBO.CGSTAmount,
                            counterSalesBO.IGSTAmount,
                            counterSalesBO.RoundOff,
                            counterSalesBO.NetAmount,
                            counterSalesBO.BankID,
                            counterSalesBO.PaymentModeID,
                            counterSalesBO.InvoiceID,
                            counterSalesBO.Reason,
                            counterSalesBO.BillDiscount,
                            counterSalesBO.PartyID,
                            counterSalesBO.VATAmount,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            XMLItem,
                            ERR,
                            RetValue
                            );
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new DatabaseException("Something went wrong");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new CreditLimitExceededException("Total exceeds credit limit");
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return true;
        }

        public bool UpdateCounterSalesReturn(CounterSalesReturnBO counterSalesBO, string XMLItem)
        {
            using (SalesEntities salesEntity = new SalesEntities())
            {
                using (var transaction = salesEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter ERR = new ObjectParameter("ERR", typeof(string));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        var i = salesEntity.SpUpdateCounterSaleReturn(
                            counterSalesBO.ID,
                            counterSalesBO.IsDraft,
                            counterSalesBO.SGSTAmount,
                            counterSalesBO.CGSTAmount,
                            counterSalesBO.IGSTAmount,
                            counterSalesBO.RoundOff,
                            counterSalesBO.NetAmount,
                            counterSalesBO.BankID,
                            counterSalesBO.PaymentModeID,
                            counterSalesBO.InvoiceID,
                            counterSalesBO.Reason,
                            counterSalesBO.BillDiscount,
                            counterSalesBO.PartyID,
                            counterSalesBO.VATAmount,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            XMLItem,
                            ERR,
                            RetValue
                            );

                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new DatabaseException("Something went wrong");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new CreditLimitExceededException("Total exceeds credit limit");
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return true;

        }

        public DatatableResultBO GetCounterSalesReturnListForDataTable(string Type, string ReturnNo, string ReturnDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetCounterSalesReturnListForDataTable(Type, ReturnNo, ReturnDate, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                ReturnNo = item.TransNo,
                                ReturnDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
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