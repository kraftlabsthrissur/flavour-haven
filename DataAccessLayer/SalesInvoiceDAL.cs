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
    public class SalesInvoiceDAL
    {

        public DatatableResultBO GetInvoiceListForSalesReturn(int CustomerID, string TransHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var fromDate = DateTime.Now.AddYears(-1);
                    var ToDate = DateTime.Now;

                    var result = dbEntity.SpGetSalesInvoiceIDForSalesReturn(CustomerID, fromDate, ToDate, TransHint, DateHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                InvoiceDate = ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                NetAmount = item.NetAmt
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
        public DatatableResultBO GetCounterSalesHistory(int ItemID, string TransNo, string TransDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var fromDate = DateTime.Now.AddYears(-1);
                    var ToDate = DateTime.Now;

                    var result = dbEntity.SpGetCounterSalesTransHistory(ItemID, TransNo, TransDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                item.CustomerName,
                                item.ItemName,
                                item.Itemcode,
                                item.PartsNumber,
                                Quantity = item.SecondaryQty,
                                MRP = item.SecondaryRate,
                                Unit = item.SecondaryUnit,
                                item.VATPercentage,
                                item.DiscountPercentage,
                                item.NetAmount
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
        public DatatableResultBO GetSalesInvoiceHistory(int ItemID, string SalesOrderNos, string InvoiceDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var fromDate = DateTime.Now.AddYears(-1);
                    var ToDate = DateTime.Now;

                    var result = dbEntity.SpGetSalesInvoiceTransHistory(ItemID, SalesOrderNos, InvoiceDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                item.SalesOrderNos,
                                InvoiceDate = ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                item.CustomerName,
                                item.Itemcode,
                                item.ItemName,
                                item.PartsNumber,
                                MRP = item.SecondaryMRP,
                                Quantity = item.SecondaryQty,
                                Unit = item.SecondaryUnit,
                                item.SecondaryMRP,
                                item.VATPercentage,
                                item.DiscountPercentage,
                                NetAmount = item.NetAmt,
                                CurrencyCode = item.CurrencyCode
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

        public DatatableResultBO GetPendingPOHistory(int ItemID, string PurchaseOrderNo, string PurchaseOrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var fromDate = DateTime.Now.AddYears(-1);
                    var ToDate = DateTime.Now;

                    var result = dbEntity.SpGetPendingPOTransHistory(ItemID, GeneralBO.LocationID, PurchaseOrderNo, PurchaseOrderDate, SupplierName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result[i];
                            obj = new
                            {
                                item.PurchaseOrderNo,
                                PurchaseOrderDate = ((DateTime)item.PurchaseOrderDate).ToString("dd-MMM-yyyy"),
                                item.SupplierName,
                                item.Itemcode,
                                item.ItemName,
                                item.PartsNumber,
                                item.Model,
                                item.LandedCost,
                                item.SecondaryRate,
                                item.SecondaryQty,
                                item.QtyMet,
                                item.SecondaryUnit,
                                DiscountPercentage = item.DiscountPercent,
                                VATPercentage = item.VATPercent,
                                NetAmount = item.NetAmount,
                                CurrencyCode = item.CurrencyCode
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

        public DatatableResultBO GetPurchaseHistory(int ItemID, string PurchaseOrderNo, string PurchaseOrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var fromDate = DateTime.Now.AddYears(-1);
                    var ToDate = DateTime.Now;

                    var result = dbEntity.SpGetPurchaseOrderTransHistory(ItemID, GeneralBO.LocationID, PurchaseOrderNo, PurchaseOrderDate, SupplierName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result[i];
                            obj = new
                            {
                                item.PurchaseOrderNo,
                                PurchaseOrderDate = ((DateTime)item.PurchaseOrderDate).ToString("dd-MMM-yyyy"),
                                item.SupplierName,
                                item.Itemcode,
                                item.ItemName,
                                item.PartsNumber,
                                item.Model,
                                item.LandedCost,
                                item.SecondaryRate,
                                item.SecondaryQty,
                                item.SecondaryUnit,
                                DiscountPercentage = item.DiscountPercent,
                                VATPercentage = item.VATPercent,
                                NetAmount = item.NetAmount,
                                CurrencyCode = item.CurrencyCode
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
        public DatatableResultBO GetLegacyPurchaseHistory(int ItemID, string ReferenceOrderNo, string OrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var fromDate = DateTime.Now.AddYears(-1);
                    var ToDate = DateTime.Now;

                    var result = dbEntity.SpGetPurchaseLegacyHistory(ItemID, ReferenceOrderNo, OrderDate, SupplierName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result[i];
                            obj = new
                            {
                                item.ReferenceNo,
                                item.ItemCode,
                                item.ItemName,
                                item.SupplierName,
                                item.PartsNumber,
                                OrderDate = item.OrderDate.HasValue ? ((DateTime)item.OrderDate).ToString("dd-MMM-yyyy") : "",
                                item.Quantity,
                                item.Rate,
                                item.GrossAmount,
                                item.Discount,
                                item.TaxAmount,
                                item.NetAmount,
                                item.CurrencyCode
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
        public List<SalesInvoiceItemBO> GetInvoiceTransList(int InvoiceID, int PriceListID)
        {
            List<SalesInvoiceItemBO> list = new List<SalesInvoiceItemBO>();
            var fromDate = DateTime.Now.AddYears(-1);
            var ToDate = DateTime.Now;
            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetSalesInvoiceItemForSalesReturn(InvoiceID, PriceListID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new SalesInvoiceItemBO
                {
                    InvoiceID = (int)m.SalesInvoiceID,
                    InvoiceTransID = m.SalesInvoiceTransID,
                    ItemID = m.ItemID,
                    ItemCode = m.ItemCode,
                    ItemName = m.ItemName,
                    UnitID = (int)m.UnitID,
                    UnitName = m.Unit,
                    MRP = (decimal)m.MRP,
                    SecondaryMRP = m.SecondaryMRP,
                    SecondaryOfferQty = m.SecondaryOfferQty,
                    SecondaryQty = m.SecondaryQty,
                    SecondaryUnit = m.SecondaryUnit,
                    SecondaryUnitSize = m.SecondaryUnitSize,
                    BasicPrice = (decimal)m.BasicPrice,
                    Qty = (decimal)m.Quantity,
                    OfferQty = (decimal)m.OfferQty,
                    DiscPercentage = (decimal)m.DiscountPercentage,
                    CashDiscount = (decimal)m.DiscountAmount,
                    GrossAmount = (decimal)m.GrossAmount,
                    IGST = (decimal)m.IGSTAmt,
                    CGST = (decimal)m.CGSTAmt,
                    SGST = (decimal)m.SGSTAmt,
                    NetAmount = (decimal)m.NetAmt,
                    CGSTPercent = (decimal)m.CGSTPercentage,
                    IGSTPercent = (decimal)m.IGSTPercentage,
                    SGSTPercent = (decimal)m.SGSTPercentage,
                    InvoiceNo = m.TransNo,
                    Stock = (decimal)0.0,
                    BatchID = (int)m.BatchID,
                    BatchName = m.BatchNo,
                    BatchTypeID = (int)m.BatchTypeID,
                    SalesUnitID = (int)m.SalesUnitID,
                    SalesUnitName = m.SalesUnit,
                    CounterSalesTransUnitID = (int)m.TransUnitID,
                    LoosePrice = (decimal)m.LooseRate,
                    FullPrice = (decimal)m.FullRate,
                    ConvertedQuantity = (decimal)m.ConvertedQuantity,
                    ConvertedOfferQuantity = (decimal)m.ConvertedOfferQuantity,
                    VATPercentage = m.VATPercentage.HasValue ? m.VATPercentage.Value : 0,
                    VATAmount = m.VATAmount.HasValue ? m.VATAmount.Value : 0,
                }).ToList();
                return list;
            }

        }

        public List<SalesInvoiceBO> GetIntercompanySalesInvoiceList(int SupplierID, int LocationID)
        {
            List<SalesInvoiceBO> list = new List<SalesInvoiceBO>();

            using (SalesEntities dEntity = new SalesEntities())
            {
                list = dEntity.SpGetSalesInvoiceBySupplierID(SupplierID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).Select(m => new SalesInvoiceBO
                {
                    ID = m.SalesInvoiceID,
                    InvoiceDate = (DateTime)m.InvoiceDate,
                    InvoiceNo = m.TransNo,
                    NetAmount = (decimal)m.NetAmt

                }).ToList();
                return list;
            }

        }

        public SalesInvoiceBO GetSalesInvoice(int SalesInvoiceID, int LocationID)
        {
            SalesInvoiceBO Invoice = new SalesInvoiceBO();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var DataList = dbEntity.SpGetSalesInvoice(SalesInvoiceID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).ToList();
                    Invoice = DataList.Select(a => new SalesInvoiceBO()
                    {
                        ID = a.ID,
                        InvoiceNo = a.TransNo,
                        InvoiceDate = (DateTime)a.InvoiceDate,
                        CustomerID = (int)a.CustomerID,
                        SalesOrderNos = a.SalesOrders,
                        OtherCharges = a.OtherCharges.HasValue ? a.OtherCharges.Value : 0,
                        CustomerPONo = a.CustomerPONo,
                        SchemeID = (int)a.SchemeID,
                        GrossAmount = (decimal)a.GrossAmt,
                        DiscountAmount = (decimal)a.DiscountAmt,
                        TurnoverDiscount = (decimal)a.TurnoverDiscount,
                        PaymentModeID = (int)a.PaymentModeID,
                        PaymentMode = a.PaymentMode,
                        FreightAmount = (decimal)a.FreightAmount,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TaxableAmount = (decimal)a.TaxableAmt,
                        SGSTAmount = a.SGSTAmt.HasValue ? a.SGSTAmt.Value : 0,
                        CGSTAmount = a.CGSTAmt.HasValue ? a.CGSTAmt.Value : 0,
                        IGSTAmount = a.IGSTAmt.HasValue ? a.IGSTAmt.Value : 0,
                        VATAmount = a.VATAmount.HasValue ? a.VATAmount.Value : 0,
                        //PartsNumber = a.PartsNumber,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        IsVat = a.IsVat.HasValue ? a.IsVat.Value : 0,
                        CurrencyExchangeRate = a.CurrencyExchangeRate.HasValue ? a.CurrencyExchangeRate.Value : 0,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        CurrencyName = a.CurrencyName,
                        CustomerCode = a.CurrencyCode,
                        RoundOff = a.RoundOff.HasValue ? a.RoundOff.Value : 0,
                        NetAmount = (decimal)a.NetAmt,
                        IsProcessed = (bool)a.IsProcessed,
                        IsDraft = (bool)a.IsDraft,
                        IsCancelled = (bool)a.IsCancelled,
                        CheckStock = (bool)a.HoldStock,
                        SalesTypeID = (int)a.SalesTypeID,
                        SalesTypeName = a.SalesTypeName,
                        CustomerName = a.CustomerName,
                        CustomerCategory = a.CustomerCategory,
                        CustomerCategoryID = (int)a.CustomerCategoryID,
                        PriceListID = (int)a.PriceListID,
                        StateID = a.StateID.HasValue ? a.StateID.Value : 0,
                        BillingAddressID = a.BillingAddressID,
                        ShippingAddressID = a.ShippingAddressID,
                        BillingAddress = a.BillingAddress,
                        ShippingAddress = a.ShippingAddress,
                        NoOfBags = (int)a.NoOfBags,
                        NoOfBoxes = (int)a.NoOfBoxes,
                        NoOfCans = (int)a.NoOfCans,
                        CashDiscount = (decimal)a.CashDiscount,
                        CessAmount = (decimal)a.CessAmount,
                        CreditAmount = (decimal)a.CreditBalance,
                        MinCreditLimit = (decimal)a.MinimumCreditLimit,
                        MaxCreditLimit = (decimal)a.MaxCreditLimit,
                        CashDiscountPercentage = (decimal)a.CashDiscountPercentage,
                        OutstandingAmount = (decimal)a.OutstandingAmount,
                        VehicleNo = a.VehicleNo,
                        CustomerGSTNo = a.CustomerGSTNo,
                        Remarks = a.Remarks,
                        PrintWithItemCode = a.PrintWithItemCode,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        CurrencyCode=a.CurrencyCode,
                        CustomerPODate = (DateTime)a.CustomerPODate,
                        VatRegNo = a.VatRegNo,
                        ReceiptDate = (DateTime)a.ReceiptDate,
                        DONO=a.DONO,
                        AmountInWords = a.AmountInWords,
                        AadhaarNo = a.AadhaarNo,
                        MinimumCurrency = a.MinimumCurrency,
                        VATPercentageID = (int)a.VATPercentageID,
                        VATPercentage = (decimal)a.VATPercentage,
                        OtherChargesVATAmount = (decimal)a.OtherChargesVATAmount,
                        DecimalPlaces=(int)a.DecimalPlaces,
                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Invoice;
        }



        public List<SalesItemBO> GetGoodsReceiptSalesInvoiceItems(string SalesInvoiceIDs, int LocationID)
        {
            List<SalesItemBO> Items = new List<SalesItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var getItems = dbEntity.SpGetGoodsReceiptSalesInvoiceItems(SalesInvoiceIDs, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).ToList();

                    Items = getItems.Select(a => new SalesItemBO()
                    {
                        SalesInvoiceTransID = a.ID,
                        SalesInvoiceID = (int)a.SalesInvoiceID,
                        TransNo = a.TransNo,
                        BatchID = (int)a.BatchID,
                        BatchTypeID = (int)a.BatchTypeID,
                        ItemID = (int)a.ItemID,
                        Qty = (decimal)a.Quantity,
                        OfferQty = (decimal)a.OfferQty,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                        SecondaryQty = (decimal)a.SecondaryQty,
                        SecondaryMRP = (decimal)a.SecondaryMRP,
                        SecondaryUnit = a.SecondaryUnit,
                        BatchName = a.BatchName,
                        Stock = (decimal)a.Stock,
                        MRP = (decimal)a.MRP,
                        PartsNumber = a.PartsNumber,
                        Model = a.Model,
                        Remarks = a.Remarks,
                        CurrencyName = a.CurrencyName,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        IsVat = a.IsVat.HasValue ? a.IsVat.Value : 0,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TurnoverDiscount = (decimal)a.TurnoverDiscount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.IGSTPercentage,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        VATAmount = a.VATAmount.HasValue ? a.VATAmount.Value : 0,
                        SGST = (decimal)a.SGSTAmt,
                        CGST = (decimal)a.CGSTAmt,
                        IGST = (decimal)a.IGSTAmt,
                        NetAmount = (decimal)a.NetAmt,
                        StoreID = (int)a.WareHouseID,
                        Name = a.ItemName,
                        PrintWithItemCode = a.PrintWithItemName.HasValue ? a.PrintWithItemName.Value : false,
                        UnitName = a.UnitName,
                        Code = a.Code,
                        UnitID = (int)a.UnitID,
                        LooseRate = (decimal)a.LooseRate,
                        Rate = (decimal)a.Rate,
                        SalesUnitID = (int)a.SalesUnitID,
                        CashDiscount = (decimal)a.CashDiscount,
                        PORate = (decimal)a.PORate,
                        POQuantity = (decimal)a.POQuantity,
                        CessAmount = (decimal)a.CessAmount,
                        CessPercentage = (decimal)a.CessPercentage,
                        ExpiryDate = a.ExpiryDate,
                        PackSize = a.PackSize.HasValue ? a.PackSize.Value : 0,
                        BatchTypeName = a.BatchType,
                        PrimaryUnit = a.PrimaryUnit,
                        ItemCategoryID = a.CategoryID
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Items;
        }

        public List<SalesItemBO> GetSalesInvoiceItems(int SalesInvoiceID, int LocationID)
        {
            List<SalesItemBO> Items = new List<SalesItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var getItems = dbEntity.SpGetSalesInvoiceItems(SalesInvoiceID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).ToList();

                    Items = getItems.Select(a => new SalesItemBO()
                    {
                        SalesInvoiceTransID = a.ID,
                        ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                        SalesOrderItemID = (int)a.SalesOrderTransID,
                        BatchID = (int)a.BatchID,
                        BatchTypeID = (int)a.BatchTypeID,
                        ItemID = (int)a.ItemID,
                        Qty = (decimal)a.Quantity,
                        OfferQty = (decimal)a.OfferQty,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                        BatchName = a.BatchName,
                        Stock = (decimal)a.Stock,
                        MRP = (decimal)a.MRP,
                        dndate = Convert.ToString(a.dndate),
                        packnumber =a.packnumber,
                        PartsNumber = a.PartsNumber,
                        Model = a.Model,
                        DeliveryTerm = a.DeliveryTerm,
                        SecondaryMRP = a.SecondaryMRP,
                        SecondaryOfferQty = a.SecondaryOfferQty,
                        SecondaryQty = a.SecondaryQty,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryUnitSize = a.SecondaryUnitSize,
                        CurrencyName = a.CurrencyName,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        IsVat = a.IsVat.HasValue ? a.IsVat.Value : 0,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TurnoverDiscount = (decimal)a.TurnoverDiscount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.IGSTPercentage,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        VATAmount = a.VATAmount.HasValue ? a.VATAmount.Value : 0,
                        SGST = (decimal)a.SGSTAmt,
                        CGST = (decimal)a.CGSTAmt,
                        IGST = (decimal)a.IGSTAmt,
                        NetAmount = (decimal)a.NetAmt,
                        StoreID = (int)a.WareHouseID,
                        Name = a.ItemName,
                        //PrintWithItemCode = a.PrintWithItemName.HasValue ? a.PrintWithItemName.Value : false,
                        PrintWithItemCode = a.PrintWithItemCode,
                        UnitName = a.UnitName,
                        Code = a.Code,
                        UnitID = (int)a.UnitID,
                        LooseRate = (decimal)a.LooseRate,
                        Rate = (decimal)a.Rate,
                        SalesUnitID = (int)a.SalesUnitID,
                        CashDiscount = (decimal)a.CashDiscount,
                        POID = a.PurchaseOrderID,
                        POTransID = a.POTransID,
                        SalesInvoiceID = (int)a.SalesInvoiceID,
                        PORate = (decimal)a.PORate,
                        POQuantity = (decimal)a.POQuantity,
                        CessAmount = (decimal)a.CessAmount,
                        CessPercentage = (decimal)a.CessPercentage,
                        ExpiryDate = a.ExpiryDate,
                        PackSize = a.PackSize.HasValue ? a.PackSize.Value : 0,
                        BatchTypeName = a.BatchType,
                        PrimaryUnit = a.PrimaryUnit,
                        ItemCategoryID = a.CategoryID,
                        PurchaseOrderNo = a.PurchaseOrderNo,
                        PurchaseOrderDate = a.PurchaseOrderDate,
                        Remarks=a.Remarks,
                        Make=a.Make,
                        DecimalPlaces = (int)a.DecimalPlaces


                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Items;
        }

        public List<SalesAmountBO> GetSalesInvoiceAmountDetails(int SalesInvoiceID, int LocationID)
        {
            List<SalesAmountBO> AmountDetails = new List<SalesAmountBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    AmountDetails = dbEntity.SpGetSalesInvoiceAmountDetails(SalesInvoiceID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).Select(a => new SalesAmountBO()
                    {
                        Amount = (decimal)a.Amount,
                        Particulars = a.Particulars,
                        Percentage = (decimal)a.Percentage,
                        TaxableAmount = (decimal)a.TaxableAmount
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return AmountDetails;
        }

        public List<SalesPackingDetailsBO> GetSalesInvoicePackingDetails(int SalesInvoiceID, int LocationID)
        {
            List<SalesPackingDetailsBO> PackingDetails = new List<SalesPackingDetailsBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    PackingDetails = dbEntity.SpGetPackingDetails(SalesInvoiceID, "SalesInvoice", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesPackingDetailsBO()
                    {
                        UnitName = a.Unit,
                        UnitID = (int)a.UnitID,
                        PackSize = a.PackSize,
                        Quantity = (decimal)a.Quantity

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return PackingDetails;
        }

        public JSONOutputBO Save(string XMLInvoice, string XMLItems, string XMLAmountDetails, string XMLPackingDetails)
        {
            JSONOutputBO output = new JSONOutputBO();
            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter SalesInvoiceID = new ObjectParameter("SalesInvoiceID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(int));

                        var i = dbEntity.SpCreateSalesInvoice(
                            XMLInvoice,
                            XMLItems,
                            XMLAmountDetails,
                            XMLPackingDetails,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SalesInvoiceID,
                            RetValue,
                            SerialNo
                            );
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new Exception("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new Exception("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new Exception("Cancelled  sales orders / proforma invoices are selected");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new Exception("Some items quantity already met");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -5)
                        {
                            throw new Exception("Credit Limit exceeded");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -6)
                        {
                            throw new Exception("Net amount is below minimum billing amount");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -7)
                        {
                            throw new Exception("Credit days exceeded");
                        }
                        InvoiceID = Convert.ToInt32(SalesInvoiceID.Value);
                        output.Data = new OutputDataBO
                        {
                            ID = InvoiceID,
                            TransNo = SerialNo.Value.ToString()
                        };
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return output;
        }

        public int Update(string XMLInvoice, string XMLItems, string XMLAmountDetails, int SalesInvoiceID, string XMLPackingDetails)
        {

            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        dbEntity.SpUpdateSalesInvoice(SalesInvoiceID, XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue);
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new Exception("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new Exception("Some items quantity already met");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new Exception("Cancelled  sales orders / proforma invoices are selected");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new Exception("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -5)
                        {
                            throw new Exception("Credit Limit exceeded");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -6)
                        {
                            throw new Exception("Net amount is below minimum billing amount");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -7)
                        {
                            throw new Exception("Credit days exceeded");
                        }
                        InvoiceID = SalesInvoiceID;
                        dbEntity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return InvoiceID;
        }
        public DatatableResultBO GetCustomerSalesInvoiceList(int CustomerID,string TransNoHint, string TranDateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetCustomerSalesInvoiceList(CustomerID,TransNoHint, TranDateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                InvoiceDate = ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
                                NetAmount = (decimal)item.NetAmount,
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
        public DatatableResultBO GetSalesInvoiceList(string Type, string CodeHint, string DateHint, string SalesTypeHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetSalesInvoiceList(Type, CodeHint, DateHint, SalesTypeHint, CustomerNameHint, LocationHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                InvoiceDate = ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                SalesType = item.SalesType,
                                CustomerName = item.CustomerName,
                                Location = item.Location,
                                NetAmount = (decimal)item.NetAmount,
                                Status = (bool)item.IsCancelled ? "cancelled" : (bool)item.IsProcessed ? "processed" : (bool)item.IsDraft ? "draft" : ""
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

        public bool IsCancelable(int SalesInvoiceID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter IsCancelable = new ObjectParameter("IsCancelable", typeof(bool));
                    dbEntity.SpIsSalesInvoiceCancelable(SalesInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsCancelable);
                    return Convert.ToBoolean(IsCancelable.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public decimal GetCreditAmountByCustomer(int CustomerID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter CreditAmount = new ObjectParameter("CreditAmount", typeof(decimal));
                    dbEntity.SpGetCreditBalanceByCustomer(CustomerID, CreditAmount);
                    return Convert.ToDecimal(CreditAmount.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int GetFreightTaxForEcommerceCustomer()
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter FreightTax = new ObjectParameter("FreightTax", typeof(int));
                    dbEntity.SpGetFreightTaxForEcommerceCustomer(GeneralBO.ApplicationID, FreightTax);
                    return Convert.ToInt16(FreightTax.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int Cancel(int SalesInvoiceID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                    dbEntity.SpCancelSalesInvoice(SalesInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, ReturnValue);
                    return (int)ReturnValue.Value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<SalesInvoiceBO> GetSalesInvoiceCodeAutoCompleteForReport(string CodeHint, DateTime FromDate, DateTime ToDate)
        {
            List<SalesInvoiceBO> InvoiceCode = new List<SalesInvoiceBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    InvoiceCode = dbEntity.SpGetSalesInvoiceCodeAutoCompleteForReport(CodeHint, FromDate, ToDate, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesInvoiceBO
                    {
                        ID = a.ID,
                        InvoiceNo = a.TransNo,
                    }).ToList();
                    return InvoiceCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
