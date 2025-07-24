using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;


namespace DataAccessLayer
{
    public class CounterSalesDAL
    {
        public int SaveCounterSalesInvoice(CounterSalesBO counterSalesBO, string XMLItem, string XMLAmountDetails)
        {
            using (SalesEntities salesEntity = new SalesEntities())
            {
                using (var transaction = salesEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "CounterSales";
                        ObjectParameter PrId = new ObjectParameter("CounterSalesID", typeof(long));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ERR = new ObjectParameter("ERR", typeof(string));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));

                        if (counterSalesBO.IsDraft)
                        {
                            FormName = "DraftCounterSales";
                        }
                        var j = salesEntity.SpUpdateSerialNo(
                                        FormName,
                                        "Code",
                                        //2019, 1, 1,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        SerialNo);
                        var CreatedUserID = GeneralBO.CreatedUserID;
                        salesEntity.SaveChanges();
                        var i = salesEntity.SpCreateCounterSaleInvoice(
                            SerialNo.Value.ToString(),
                            counterSalesBO.TransDate,
                            counterSalesBO.PartyName,
                            counterSalesBO.CustomerID,
                            counterSalesBO.ContactID,
                            counterSalesBO.CivilID,
                            counterSalesBO.MobileNumber,
                            counterSalesBO.PrintWithItemCode,
                            counterSalesBO.PatientID,
                            counterSalesBO.PackingPrice,
                            counterSalesBO.DoctorID,
                            counterSalesBO.DoctorName,
                            counterSalesBO.WarehouseID,
                            counterSalesBO.GrossAmount,
                            counterSalesBO.DiscountAmt,
                            counterSalesBO.DiscountPercentage,
                            counterSalesBO.TaxableAmt,
                            counterSalesBO.IsDraft,
                            counterSalesBO.CurrencyID,
                            counterSalesBO.IsVAT,
                            counterSalesBO.IsGST,
                            counterSalesBO.SGSTAmount,
                            counterSalesBO.CGSTAmount,
                            counterSalesBO.IGSTAmount,
                            counterSalesBO.CessAmount,
                            counterSalesBO.TotalVATAmount,
                            counterSalesBO.RoundOff,
                            counterSalesBO.NetAmount,
                            counterSalesBO.BalanceAmount,
                            counterSalesBO.Remarks,
                            counterSalesBO.EmployeeID,
                            counterSalesBO.TypeID,
                            counterSalesBO.BankID,
                            counterSalesBO.DiscountCategoryID,
                            XMLAmountDetails,
                            //616,2019,1,1,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            counterSalesBO.TotalAmountReceived,
                            counterSalesBO.PaymentModeID,
                            counterSalesBO.BalanceToBePaid,
                            XMLItem,
                            ERR,
                            RetValue,
                            PrId,
                            counterSalesBO.AmountRecieveds,
                            counterSalesBO.ReferenceNo,
                            0, null, "", 0, "",
                            counterSalesBO.VATPercentage,
                            counterSalesBO.VATPercentageID
                            );
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            transaction.Rollback();
                            throw new OutofStockException(ERR.Value.ToString());
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            transaction.Rollback();
                            throw new DatabaseException("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            transaction.Rollback();
                            throw new DatabaseException("NetAmount Greater than CreditLimit");
                        }
                        transaction.Commit();
                        return (int)PrId.Value;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public int UpdateCounterSalesInvoice(CounterSalesBO counterSalesBO, string XMLItem, string XMLAmountDetails)
        {
            using (SalesEntities salesEntity = new SalesEntities())
            {
                using (var transaction = salesEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter ERR = new ObjectParameter("ERR", typeof(string));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        var CreatedUserID = GeneralBO.CreatedUserID;
                        salesEntity.SaveChanges();
                        var i = salesEntity.SpUpdateCounterSaleInvoice(
                                    counterSalesBO.ID,
                                    counterSalesBO.WarehouseID,
                                    counterSalesBO.GrossAmount,
                                    counterSalesBO.DiscountAmt,
                                    counterSalesBO.DiscountPercentage,
                                    counterSalesBO.TaxableAmt,
                                    counterSalesBO.IsDraft,
                                    counterSalesBO.CurrencyID,
                                    counterSalesBO.IsVAT,
                                    counterSalesBO.IsGST,
                                    counterSalesBO.PrintWithItemCode,
                                    counterSalesBO.SGSTAmount,
                                    counterSalesBO.CGSTAmount,
                                    counterSalesBO.IGSTAmount,
                                    counterSalesBO.CessAmount,
                                    counterSalesBO.TotalVATAmount,
                                    counterSalesBO.RoundOff,
                                    counterSalesBO.NetAmount,
                                    counterSalesBO.BalanceAmount,
                                    counterSalesBO.TypeID,
                                    counterSalesBO.EmployeeID,
                                    counterSalesBO.PatientID,
                                    counterSalesBO.PartyName,
                                    counterSalesBO.CustomerID,
                                    counterSalesBO.ContactID,
                                    counterSalesBO.CivilID,
                                    counterSalesBO.MobileNumber,
                                    counterSalesBO.Remarks,
                                    counterSalesBO.BankID,
                                    counterSalesBO.DiscountCategoryID,
                                    XMLAmountDetails,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    counterSalesBO.TotalAmountReceived,
                                    counterSalesBO.PaymentModeID,
                                    counterSalesBO.BalanceToBePaid,
                                    XMLItem,
                                    ERR,
                                    RetValue,
                                    counterSalesBO.AmountRecieveds,
                                    0,null,"",0,"",
                                    counterSalesBO.VATPercentage,
                                    counterSalesBO.VATPercentageID);

                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            transaction.Rollback();
                            throw new OutofStockException(ERR.Value.ToString());
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            transaction.Rollback();
                            throw new DatabaseException("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            transaction.Rollback();
                            throw new DatabaseException("NetAmount Greater than CreditLimit");
                        }

                        transaction.Commit();
                        return (int)counterSalesBO.ID;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public List<CounterSalesBO> GetCounterSalesList(Int32 ID)
        {
            try
            {
                using (SalesEntities salesEntity = new SalesEntities())
                {
                    return salesEntity.SpGetCounterSales(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new CounterSalesBO()
                    {
                        ID = m.ID,
                        TransNo = m.TransNo,
                        TransDate = (DateTime)m.Transdate,
                        WarehouseName = m.Warehouse,
                        PartyName = m.PartyName,
                        NetAmount = (decimal)m.NetAmountTotal,
                        IsDraft = (bool)m.IsDraft,
                        IsCancelled = (bool)m.IsCancelled,
                        DoctorID = (int)m.DoctorID,
                        DoctorName = m.DoctorName,
                        PatientID = (int)m.PatientID,
                        PackingPrice = (int)m.PackingPrice,
                        RoundOff = (decimal)m.RoundOff,
                        Type = m.Type


                    }).ToList();

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CounterSalesBO> GetCounterSalesDetail(int ID)
        {
            try
            {
                using (SalesEntities salesEntity = new SalesEntities())
                {
                    return salesEntity.SpGetCounterSalesDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new CounterSalesBO()
                    {
                        ID = m.ID,
                        IsGST = m.IsGST.HasValue ? (int)m.IsGST : 0,
                        IsVAT = m.IsVAT.HasValue ? (int)m.IsVAT : 0,
                        TransNo = m.TransNo,
                        TransDate = (DateTime)m.Transdate,
                        WarehouseName = m.Warehouse,
                        PartyName = m.PartyName,
                        CustomerID = m.CustomerID.HasValue ? m.CustomerID.Value : 0,
                        ContactName = m.ContactName,
                        ContactID = m.ContactID.HasValue ? m.ContactID.Value : 0,
                        CivilID = m.CivilID,
                        MobileNumber = m.MobileNumber,
                        PrintWithItemCode = m.PrintWithItemName,
                        NetAmount = (decimal)m.NetAmountTotal,
                        IsDraft = (bool)m.IsDraft,
                        IsCancelled = (bool)m.IsCancelled,
                        DoctorID = (int)m.DoctorID,
                        DoctorName = m.DoctorName,
                        PatientID = (int)m.PatientID,
                        PackingPrice = (int)m.PackingPrice,
                        RoundOff = (decimal)m.RoundOff,
                        CGSTAmount = (decimal)m.CGSTAmount,
                        SGSTAmount = (decimal)m.SGSTAmount,
                        IGSTAmount = (decimal)m.IGSTAmount,
                        TotalVATAmount = (decimal)m.TotalVATAmount,
                        PaymentModeID = (int)m.PaymentModeID,
                        TotalAmountReceived = (decimal)m.TotalAmountReceived,
                        BalanceToBePaid = (decimal)m.BalanceToBePaid,
                        CessAmount = (decimal)m.CessAmount,
                        EmployeeID = (int)m.EmployeeID,
                        TypeID = (int)m.SalesTypeID,
                        Type = m.SalesType,
                        EmployeeName = m.Employee,
                        PatientName = m.PatientName,
                        BalanceAmount = (decimal)m.BalAmount,
                        EmployeeCode = m.EmployeeCode,
                        TaxableAmt = (decimal)m.TaxableAmt,
                        GrossAmount = (decimal)m.GrossAmount,
                        BankID = m.BankID,
                        CashSalesName = m.CashSalesName,
                        OutstandingAmount = (decimal)m.OutstandingAmount,
                        DiscountCategoryID = m.DiscountCategoryID,
                        DiscountCategory = m.DiscountCategory,
                        DiscountAmt = (decimal)m.DiscountAmt,
                        DiscountPercentage = (decimal)m.DiscountPercentage,
                        BusinessCategoryID = (int)m.BusinessCategoryID,
                        Remarks = m.Remarks,
                        currencyCode=m.currencyCode,
                        AmountInWords = m.AmountInWords,
                        MinimumCurrency =  m.MinimumCurrency,
                        ReferenceNo = m.ReferenceNo,
                        VATPercentage = (decimal)m.VATPercentage,
                        VATPercentageID = (int)m.VATPercentageID,
                        DecimalPlaces=(int)m.DecimalPlaces
                    }).ToList();

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CounterSalesItemsBO> GetCounterSalesListDetails(Int32 ID)
        {
            try
            {
                using (SalesEntities salesEntity = new SalesEntities())
                {
                    return salesEntity.SpGetCounterSalesTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new CounterSalesItemsBO()
                    {

                        CounterSalesID = (int)m.CounterSalesID,
                        FullOrLoose = m.FullOrLoose,
                        ItemID = (int)m.ItemID,
                        Name = m.ItemName,
                        PartsNumber = m.PartsNumber,
                        DeliveryTerm = m.DeliveryTerm,
                        Model = m.Model,
                        BatchID = (int)m.BatchID,
                        Quantity = (decimal)m.Quantity,
                        Rate = (decimal)m.Rate,
                        MRP = (decimal)m.MRP,
                        SecondaryUnit = m.SecondaryUnit,
                        SecondaryRate = m.SecondaryRate,
                        SecondaryUnitSize = m.SecondaryUnitSize,
                        SecondaryOfferQty = m.SecondaryOfferQty,
                        SecondaryQty = m.SecondaryQty,
                        SecondaryUnits = m.SecondaryUnits,
                        GrossAmount = (decimal)m.GrossAmount,
                        SGSTAmount = m.SGSTAmount.HasValue ? m.SGSTAmount.Value : 0,
                        CGSTAmount = m.CGSTAmount.HasValue ? m.CGSTAmount.Value : 0,
                        IGSTAmount = m.IGSTAmount.HasValue ? m.IGSTAmount.Value : 0,
                        VATAmount = m.VATAmount.HasValue ? m.VATAmount.Value : 0,
                        NetAmount = (decimal)m.NetAmount,
                        IGSTPercentage = m.IGSTPercent.HasValue ? m.IGSTPercent.Value : 0,
                        SGSTPercentage = m.SGSTPercent.HasValue ? m.SGSTPercent.Value : 0,
                        CGSTPercentage = m.CGSTPercent.HasValue ? m.CGSTPercent.Value : 0,
                        VATPercentage = m.VATPercentage.HasValue ? m.VATPercentage.Value : 0,
                        Code = m.Code,
                        IsGST = m.IsGST.HasValue ? m.IsGST.Value : 0,
                        IsVAT = m.IsVAT.HasValue ? m.IsVAT.Value : 0,
                        BatchNo = m.BatchNo,
                        CurrencyName = m.CurrencyName,
                        CurrencyID = m.CurrencyID.HasValue ? m.CurrencyID.Value : 0,
                        BatchTypeID = (int)m.BatchTypeID,
                        WareHouseID = (int)m.WarehouseID,
                        ExpiryDate = (DateTime)m.ExpiryDate,
                        Unit = m.Unit,
                        Stock = m.Stock,
                        TaxableAmount = (decimal)m.TaxableAmount,
                        UnitID = (int)m.UnitID,
                        CessAmount = (decimal)m.CessAmount,
                        CessPercentage = (decimal)m.CessPercentage,
                        BasicPrice = (decimal)m.BasicPrice,
                        HSNCode = m.HSNCode,
                        MinimumSalesQty = (decimal)m.MinSalesQty,
                        MaximumSalesQty = (decimal)m.MaxSalesQty,
                        GSTPercentage = m.GSTPercent,
                        DiscountAmount = (decimal)m.DiscountAmount,
                        DiscountPercentage = (decimal)m.DiscountPercentage,
                        Make=m.Make,
                        DecimalPlaces=(int)m.DecimalPlaces
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CounterSalesAmountDetailsBO> GetCounterSalesListAmount(int ID)
        {
            try
            {
                using (SalesEntities salesEntity = new SalesEntities())
                {
                    return salesEntity.SpGetCounterSalesAmountDetails(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new CounterSalesAmountDetailsBO()
                    {
                        Particulars = m.Particulars,
                        Percentage = (decimal)m.Percentage,
                        Amount = (decimal)m.Amount
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Cancel(int CounterSalesID)
        {
            SalesEntities dEntity = new SalesEntities();
            return dEntity.SpCancelCounterSale(CounterSalesID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
        }

        public DatatableResultBO GetCounterSalesForReturn(int PartyID, string TransHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var fromDate = DateTime.Now.AddYears(-1);
                    var ToDate = DateTime.Now;

                    var result = dbEntity.SpGetCounterSalesForReturn(PartyID, fromDate, ToDate, TransHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                NetAmount = item.NetAmount,
                                PartyName = item.PartyName,
                                BillDiscount = item.DiscountAmt
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

        public List<CounterSalesItemsBO> GetBatchwiseItemForCounterSales(int ItemID, int WarehouseID, int BatchTypeID, int UnitID, decimal Qty, string CustomerType, int TaxTypeID)
        {
            List<CounterSalesItemsBO> list = new List<CounterSalesItemsBO>();
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetBatchwiseItemForCounterSales(ItemID, WarehouseID, BatchTypeID, UnitID, Qty, CustomerType, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, TaxTypeID).Select(m => new CounterSalesItemsBO
                {
                    ItemID = (int)m.ItemID,
                    BatchID = (int)m.BatchID,
                    BatchTypeID = (int)m.BatchTypeID,
                    BatchNo = m.BatchNo,
                    CGSTPercentage = (decimal)m.CGSTPercent,
                    SGSTPercentage = (decimal)m.SGSTPercent,
                    IGSTPercentage = (decimal)m.IGSTPercent,
                  //  VATPercentage = (decimal)m.VATPercentage,
                    Name = m.ItemName,
                    Code = m.ItemCode,
                    PartsNumber = m.PartsNumber,
                    DeliveryTerm = "",
                    Model = m.Model,
                    Stock = (decimal)m.Stock,
                    Unit = m.Unit,
                   // SecondaryUnits = m.SecondaryUnits,
                    UnitID = (int)m.UnitID,
                    BatchType = m.BatchType,
                    ExpiryDate = m.ExpiryDate,
                    Quantity = (decimal)m.Quantity,
                    FullPrice = m.FullPrice,
                    LoosePrice = m.LoosePrice,
                    SalesUnitID = (int)m.SalesUnitID,
                    CessPercentage = (decimal)m.CessPercentage,
                    IsGSTRegisteredLocation = (bool)m.IsGSTRegisteredLocation,
                 //   IsGST = m.IsGST,
                  //  IsVAT = m.IsVAT,
                  //  TaxType = m.TaxType,
                  //  CurrencyID = m.CurrencyID,
                   // CurrencyName = m.CurrencyName,
                  //  Make=m.Make
                }).ToList();
                return list;
            }
        }

        public List<CounterSalesItemsBO> GetGoodsReceiptItemForCounterSales(string counterSalesIDs)
        {
            List<CounterSalesItemsBO> list = new List<CounterSalesItemsBO>();
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetGoodsReceiptCounterSalesItems(counterSalesIDs, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new CounterSalesItemsBO
                {
                    ID = (int)m.ID,
                    CounterSalesID = m.CounterSalesID.HasValue ? (int)m.CounterSalesID.Value : 0,
                    Name = m.ItemName,
                    Code = m.ItemCode,
                    PartsNumber = m.PartsNumber,
                    DeliveryTerm = m.Remark,
                    Model = m.Model,
                    SecondaryQty = m.SecondaryQty,
                    SecondaryRate = m.SecondaryRate,
                    SecondaryUnit = m.SecondaryUnit,
                    TransNo = m.TransNo,
                    ItemID = (int)m.ItemID,
                    BatchID = (int)m.BatchID,
                    BatchTypeID = (int)m.BatchTypeID,
                    BatchNo = m.BatchNo,
                    CGSTPercentage = (decimal)m.CGSTPercent,
                    SGSTPercentage = (decimal)m.SGSTPercent,
                    IGSTPercentage = (decimal)m.IGSTPercent,
                    VATPercentage = (decimal)m.VATPercentage,
                    Stock = (decimal)m.Stock,
                    Unit = m.Unit,
                    UnitID = (int)m.UnitID,
                    ExpiryDate = m.ExpiryDate,
                    Quantity = (decimal)m.Quantity,
                    MRP = m.MRP.HasValue ? m.MRP.Value : 0,
                    Rate = m.MRP.HasValue ? m.MRP.Value : 0,
                    LooseRate = m.MRP.HasValue ? m.MRP.Value : 0,
                    //Rate = m.Rate.HasValue ? m.Rate.Value : 0,
                    CessPercentage = (decimal)m.CessPercentage,
                    //IsGSTRegisteredLocation = (bool)m.IsGSTRegisteredLocation,
                    IsGST = m.IsGST.HasValue ? m.IsGST.Value : 0,
                    IsVAT = m.IsVAT.HasValue ? m.IsVAT.Value : 0,
                    CurrencyID = m.CurrencyID.HasValue ? m.CurrencyID.Value : 0,
                    CurrencyName = m.CurrencyName,
                    PrintWithItemName = m.PrintWithItemName.HasValue ? m.PrintWithItemName.Value : false,
                }).ToList();
                return list;
            }
        }

        public List<CounterSalesItemsBO> GetCounterSalesTransForCounterSalesReturn(int InvoiceID, int PriceListID)
        {
            List<CounterSalesItemsBO> list = new List<CounterSalesItemsBO>();
            var fromDate = DateTime.Now.AddYears(-1);
            var ToDate = DateTime.Now;
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetCounterSalesTransForCounterSalesReturn(InvoiceID, PriceListID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new CounterSalesItemsBO
                {
                    CounterSalesID = (int)m.CounterSalesID,
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
                    IGSTPercentage = (decimal)m.IGSTPercentage,
                    SGSTPercentage = (decimal)m.SGSTPercentage,
                    CGSTPercentage = (decimal)m.CGSTPercentage,
                    Code = m.Code,
                    BatchNo = m.BatchNo,
                    BatchTypeID = (int)m.BatchTypeID,
                    WareHouseID = (int)m.WareHouseID,
                    ExpiryDate = (DateTime)m.ExpiryDate,
                    Unit = m.Unit,
                    Stock = m.Stock,
                    TaxableAmount = (decimal)m.TaxableAmount,
                    ID = (int)m.TransID,
                    UnitID = (int)m.UnitID,
                    SalesUnitID = (int)m.SalesUnitID,
                    SalesUnitName = m.SalesUnit,
                    CounterSalesTransUnitID = (int)m.TransUnitID,
                    LoosePrice = m.LooseRate,
                    FullPrice = m.FullRate,
                    ConvertedQuantity = (decimal)m.ConvertedQuantity,
                    CessPercentage = (decimal)m.CessPercentage,
                    CessAmount = (decimal)m.CessAmount,
                    SecondaryQty = (decimal)m.SecondaryQty,
                    SecondaryRate = (decimal)m.SecondaryRate,
                    SecondaryUnit = m.SecondaryUnit,
                    SecondaryUnitSize = (decimal)m.SecondaryUnitSize,
                    DiscountPercentage = (decimal)m.DiscountPercentage,
                    DiscountAmount = (decimal)m.DiscountAmount,
                    VATPercentage = (decimal)m.VATPercentage,
                    VATAmount = (decimal)m.VATAmount
                }).ToList();
                return list;
            }
        }

        public List<CounterSalesBO> GetCounterSalesType()
        {
            try
            {
                List<CounterSalesBO> CounterSalesType = new List<CounterSalesBO>();
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetCounterSalesTypeList().Select(a => new CounterSalesBO
                    {
                        ID = a.ID,
                        Type = a.Type
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetCustomerCounterSalesList(int CustomerID,string TransNoHint, string TransDateHint, string PartyNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetCustomerCounterSalesList(CustomerID,TransNoHint, TransDateHint, PartyNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.Transdate).ToString("dd-MMM-yyyy"),
                                PartyName = item.PartyName,
                                NetAmount = (decimal)item.NetAmount
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

        public DatatableResultBO GetListForCounterSales(string Type, string TransNoHint, string TransDateHint, string SalesTypeHint, string PartyNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetListForCounterSales(Type, TransNoHint, TransDateHint, SalesTypeHint, PartyNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.Transdate).ToString("dd-MMM-yyyy"),
                                WarehouseName = item.Warehouse,
                                PartyName = item.PartyName,
                                NetAmount = (decimal)item.NetAmountTotal,
                                IsDraft = (bool)item.IsDraft ? "draft" : "",
                                IsCancelled = (bool)item.IsCancelled ? "cancelled" : "",
                                DoctorID = (int)item.DoctorID,
                                DoctorName = item.DoctorName,
                                PatientID = (int)item.PatientID,
                                PackingPrice = (int)item.PackingPrice,
                                RoundOff = (decimal)item.RoundOff,
                                Type = item.SalesType,
                                CurrencyName = item.CurrencyName,
                                TaxType = item.TaxTypeName
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

        public DatatableResultBO GetAppointmentProcessList(string TransNoHint, string TransDateHint, string PatientNameHint, string DoctorNameHint, string PhoneHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetAppointmentProcessList(TransNoHint, TransDateHint, PatientNameHint, DoctorNameHint, PhoneHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                PatientName = item.PatientName,
                                PatientID = (int)item.PatientID,
                                DoctorName = item.DoctorName,
                                DoctorID = (int)item.DoctorID,
                                Phone = item.MobileNo
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

        public List<SalesItemBO> GetBatchwisePrescriptionItems(int AppointmentProcessID, int PatientID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetBatchwisePrescriptionItems(AppointmentProcessID, PatientID,
                        GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                        {
                            SalesOrderItemID = (int)a.PrescriptionOrderItemID,
                            SalesOrderID = (int)a.PrescriptionOrderID,
                            ItemID = (int)a.ItemID,
                            UnitID = (int)a.UnitID,
                            Unit = a.Unit,
                            BatchID = (int)a.BatchID,
                            BatchTypeID = (int)a.BatchTypeID,
                            BatchName = a.BatchNo,
                            ItemCategoryID = (int)a.CategoryID,
                            Code = a.Code,
                            Name = a.ItemName,
                            Qty = (decimal)a.Qty,
                            InvoiceQty = (decimal)a.InvoiceQty,
                            Rate = (decimal)a.Rate,
                            SGSTPercentage = (decimal)a.SGSTPercentage,
                            CGSTPercentage = (decimal)a.CGSTPercentage,
                            IGSTPercentage = (decimal)a.IGSTPercentage,
                            GSTPercentage = (decimal)a.IGSTPercentage,
                            CessPercentage = (decimal)a.CessPercentage,
                            Stock = (decimal)a.Stock,
                            SalesOrderNo = a.PrescriptionOrderNo,
                            SalesUnitID = (int)a.SalesUnitID,
                            LooseRate = (decimal)a.LooseRate,
                            BatchTypeName = a.BatchTypeName,
                            Category = a.CategoryName,
                            PackSize = (decimal)a.PackSize,
                            PrimaryUnit = a.PrimaryUnit
                        }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsCancelable(int counterSalesID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter IsCancelable = new ObjectParameter("IsCancelable", typeof(bool));
                    dbEntity.SpIsCounterSalesCancelable(counterSalesID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsCancelable);
                    return Convert.ToBoolean(IsCancelable.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<CounterSalesBO> GetCounterSalesSignOutPrint(string Type)
        {
            try
            {
                List<CounterSalesBO> TotalBill = new List<CounterSalesBO>();
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    TotalBill = dbEntity.SpGetCounterSalesSignOutPrint(Type, GeneralBO.CreatedUserID).Select(a => new CounterSalesBO()
                    {
                        //StartingBillNo = a.StartingBillNo,
                        //EndingBillNo = a.EndingBillNo,
                        //Count = (int)a.Count,
                        NetAmount = (decimal)a.NetAmount,
                        //TotalCash =(decimal) a.TotalCash,
                        //CashOnCard = (decimal)a.Card,
                        EmployeeName = a.Employee

                    }
                    ).ToList();

                    return TotalBill;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsDotMatrixPrint()
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter IsDotMatrixPrint = new ObjectParameter("IsDotMatrixPrint", typeof(bool));
                    dbEntity.SpGetCounterSalesPrintConfiguration(GeneralBO.ApplicationID, IsDotMatrixPrint);
                    return Convert.ToBoolean(IsDotMatrixPrint.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public CurrencyClassBO GetCurrencyDecimalClassByCurrencyID(int CurrencyID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    return dbEntity.SpGetCurrencyDecimalClassByCurrencyID(CurrencyID).Select(x => new CurrencyClassBO
                    {
                        largeclass = x.largeclass,
                        normalclass = x.normalclass,
                        DecimalPlaces = x.DecimalPlaces,
                    }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool IsThermalPrint()
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter IsThermalPrint = new ObjectParameter("IsThermalPrint", typeof(bool));
                    dbEntity.SpGetCounterSalesThermalPrintConfiguration(GeneralBO.ApplicationID, IsThermalPrint);
                    return Convert.ToBoolean(IsThermalPrint.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public CounterSalesBO GetIsCounterSalesAlreadyExists(string PartyName, decimal NetAmount)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.spGetIsCounterSalesAlreadyExists(PartyName, NetAmount).Select(a => new CounterSalesBO
                    {
                        Counts = (int)a.Counts
                    }
                    ).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
