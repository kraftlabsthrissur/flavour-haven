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
    public class ProformaInvoiceDAL
    {
        public ProformaInvoiceBO GetProformaInvoice(int ProformaInvoiceID)
        {
            ProformaInvoiceBO Invoice = new ProformaInvoiceBO();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var data = dbEntity.SpGetProformaInvoice(ProformaInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    Invoice = data.Select(a => new ProformaInvoiceBO()
                    {
                        ID = a.ID,
                        TransNo = a.TransNo,
                        CurrencyCode = a.CurrencyCode,
                        TransDate = (DateTime)a.InvoiceDate,
                        CustomerID = a.CustomerID ?? 0,
                        SalesOrderNos = a.SalesOrders,
                        SchemeID = a.SchemeID ?? 0,
                        GrossAmount = (decimal)a.GrossAmt,
                        DiscountAmount = (decimal)a.DiscountAmt,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TaxableAmount = (decimal)a.TaxableAmt,
                        SGSTAmount = (decimal)a.SGSTAmt,
                        CGSTAmount = (decimal)a.CGSTAmt,
                        IGSTAmount = (decimal)a.IGSTAmt,
                        RoundOff = (decimal)a.RoundOff,
                        NetAmount = (decimal)a.NetAmt,
                        IsProcessed = (bool)a.IsProcessed,
                        IsDraft = (bool)a.IsDraft,
                        IsCancelled = (bool)a.IsCancelled,
                        CheckStock = (bool)a.HoldStock,
                        SalesTypeID = a.SalesTypeID ?? 0,
                        SalesTypeName = a.SalesTypeName,
                        CustomerName = a.CustomerName,
                        CustomerCategory = a.CustomerCategory,
                        CustomerCategoryID = a.CustomerCategoryID,
                        PriceListID = a.PriceListID ?? 0,
                        StateID = a.StateID ?? 0,
                        BillingAddressID = a.BillingAddressID,
                        ShippingAddressID = a.ShippingAddressID,
                        BillingAddress = a.BillingAddress,
                        ShippingAddress = a.ShippingAddress,
                        NoOfBags = a.NoOfBags ?? 0,
                        NoOfBoxes = a.NoOfBoxes ?? 0,
                        NoOfCans = a.NoOfCans ?? 0,
                        CheckedBy = a.CheckedBy,
                        PackedBy = a.PackedBy,
                        CessAmount = (decimal)a.CessAmount,
                        FreightAmount = (decimal)a.FreightAmount,
                        OutStandingAmount = (decimal)a.OutstandingAmount,
                        MaxCreditLimit = (decimal)a.MaxCreditLimit,
                        Remarks = a.Remarks,
                        IsGSTRegistered = a.IsGSTRegistered ?? false,
                        PrintWithItemCode = a.PrintWithItemCode
                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Invoice;
        }

        public List<SalesItemBO> GetProformaInvoiceItems(string CommaSeperatedProformaInvoiceIDs, string For = "Sales")
        {
            List<SalesItemBO> Items = new List<SalesItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var DataList = dbEntity.SpGetProformaInvoiceItems(CommaSeperatedProformaInvoiceIDs, For, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    Items = DataList.Select(a => new SalesItemBO()
                    {
                        ProformaInvoiceTransID = a.ID,
                        ProformaInvoiceID = (int)a.ProformaInvoiceID,
                        SalesOrderItemID = (int)a.SalesOrderTranID,
                        SalesOrderNo = a.SalesOrderNo,
                        ItemID = (int)a.ItemID,
                        Code = a.ItemCode,
                        ItemName = a.ItemName,
                        PartsNumber = a.PartsNumber,
                        DeliveryTerm = a.DeliveryTerm,
                        Model = a.Model,
                        BatchID = (int)a.BatchID,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchName = a.BatchNo,
                        BatchTypeName = a.BatchTypeName,
                        Stock = (decimal)a.Stock,
                        Qty = (decimal)a.Quantity,
                        OfferQty = (decimal)a.OfferQty,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                        InvoiceOfferQtyMet = true,
                        InvoiceQtyMet = true,
                        MRP = (decimal)a.MRP,
                        Rate = (decimal)a.MRP,
                        LooseRate = (decimal)a.MRP,
                        //Rate = (decimal)a.Rate,
                        //LooseRate = (decimal)a.LooseRate,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountAmount = a.DiscountAmount.HasValue ? a.DiscountAmount.Value : 0,
                        VATAmount = a.VATAmount,
                        VATPercentage = a.VatPercentage.HasValue ? a.VatPercentage.Value : 0,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TurnoverDiscount = (decimal)a.TurnoverDiscount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.IGSTPercentage,

                        SGST = (decimal)a.SGSTAmt,
                        CGST = (decimal)a.CGSTAmt,
                        IGST = (decimal)a.IGSTAmt,
                        NetAmount = (decimal)a.NetAmt,
                        StoreID = (int)a.WareHouseID,
                        Unit = a.UnitName,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        CurrencyName = a.CurrencyName,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        IsVat = a.IsVat.HasValue ? a.IsVat.Value : 0,
                        PrintWithItemCode = a.PrintWithItemName.HasValue ? a.PrintWithItemName.Value : false,
                        UnitID = (int)a.UnitID,
                        Make = a.Model,

                        SalesUnitID = (int)a.SalesUnitID,
                        CessAmount = (decimal)a.CessAmount,
                        CessPercentage = (decimal)a.CessPercentage,
                        Category = a.Category,
                        PackSize = a.PackSize.HasValue ? a.PackSize.Value : 0,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryMRP = a.SecondaryMRP,
                        SecondaryUnitSize = a.SecondaryUnitSize,
                        SecondaryOfferQty = a.SecondaryOfferQty,
                        SecondaryQty = a.SecondaryQty,
                        MalayalamName = a.MalayalamName,
                        PrimaryUnit = a.PrimaryUnit,
                        ItemCategoryID = a.CategoryID,
                        SalesorderNO = a.SalesOrderNo,
                        OrderDate = a.OrderDate,



                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Items;
        }

        public List<SalesAmountBO> GetProformaInvoiceAmountDetails(int ProformaInvoiceID)
        {
            List<SalesAmountBO> AmountDetails = new List<SalesAmountBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    AmountDetails = dbEntity.SpGetProformaInvoiceAmountDetails(ProformaInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesAmountBO()
                    {
                        Amount = (decimal)a.Amount,
                        Particulars = a.Particulars,
                        Percentage = (decimal)a.Percentage,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return AmountDetails;
        }

        public int Save(string XMLInvoice, string XMLItems, string XMLAmountDetails, string XMLPackingDetails)
        {
            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter ProformaInvoiceID = new ObjectParameter("ProformaInvoiceID", typeof(int));
                        dbEntity.SpCreateProformaInvoice(XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue, ProformaInvoiceID);
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new OutofStockException("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new AlreadyCancelledException("SO Already Cancelled");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new QuantityExceededException("Some items quantity already met");
                        }
                        InvoiceID = Convert.ToInt32(ProformaInvoiceID.Value);
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

        public int Update(string XMLInvoice, string XMLItems, string XMLAmountDetails, int ProformaInvoiceID, string XMLPackingDetails)
        {

            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        dbEntity.SpUpdateProformaInvoice(ProformaInvoiceID, XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue);
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new OutofStockException("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {

                            throw new DatabaseException("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -2 || Convert.ToInt32(RetValue.Value) == -3)
                        {

                            throw new AlreadyCancelledException("Salesorder already cancelled");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new QuantityExceededException("Some items quantity already met");

                        }
                        InvoiceID = ProformaInvoiceID;
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

        public DatatableResultBO GetProformaInvoiceList(string CodeHint, string DateHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string InvoiceType, int ItemCategoryID, int CustomerID, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetProformaInvoiceList(CodeHint, DateHint, CustomerNameHint, LocationHint, NetAmountHint, InvoiceType, ItemCategoryID, CustomerID, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Location = item.Location,
                                NetAmount = (decimal)item.NetAmount,
                                SalesType = item.SalesType,
                                SalesTypeID = item.SalesTypeID,
                                Status = item.Status,
                                NoOfBoxes = item.NoOfBoxes,
                                NoOfCans = item.NoOfCans,
                                NoOfBags = item.NoOfBags,
                                FreightAmount = (decimal)item.FreightAmount,
                                ShippingAddressID = item.ShippingAddressID,
                                BillingAddressID = item.BillingAddressID
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

        public List<SalesBatchBO> GetItemBatchwise(int ItemID, decimal Qty, decimal OfferQty, int StoreID, int CustomerID, int UnitID)
        {
            List<SalesBatchBO> ItemBatchwise = new List<SalesBatchBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ItemBatchwise = dbEntity.SpGetItemBatchwise(ItemID, Qty, OfferQty, StoreID, CustomerID, UnitID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesBatchBO()
                    {
                        ItemID = (int)a.ItemID,
                        BatchID = (int)a.BatchID,
                        BatchNo = a.BatchNo,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchTypeName = a.BatchTypeName,
                        Qty = (decimal)a.Qty,
                        OfferQty = (decimal)a.OfferQty,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                        Stock = (decimal)a.Stock,
                        Rate = (decimal)a.Rate

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ItemBatchwise;
        }

        public bool IsCancelable(int ProformaInvoiceID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter IsCancelable = new ObjectParameter("IsCancelable", typeof(bool));
                    dbEntity.SpIsProformaInvoiceCancelable(ProformaInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsCancelable);
                    return Convert.ToBoolean(IsCancelable.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void Cancel(int ProformaInvoiceID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    dbEntity.SpCancelProformaInvoice(ProformaInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public CustomerCreditSummaryBO GetCustomerCreditSummary(int ProformaInvoiceID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    return dbEntity.SpGetCustomerCreditSummary(ProformaInvoiceID).Select(a => new CustomerCreditSummaryBO()
                    {
                        CustomerID = a.CustomerID,
                        CustomerName = a.CustomerName,
                        BillingAddress1 = a.BillingAddress1,
                        BillingAddress2 = a.BillingAddress2,
                        BillingAddress3 = a.BillingAddress3,
                        District = a.District,
                        State = a.State,
                        CreditLimit = a.CreditLimit,
                        ReceivablesAsOnDate = a.ReceivablesAsOnDate,
                        BillAmount = (decimal)a.BillAmount,
                        CurrentReceivables = (decimal)a.CurrentReceivables,
                        CreditDays = a.CreditDays,
                        OutstandingDays = a.OutstandingDays,
                        OverDueAmount = a.OverDueAmount,
                        MinimumCreditLimit = a.MinimumCreditLimit
                    }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<SalesPackingDetailsBO> GetProformaInvoicePackingDetails(int ProformaInvoiceID)
        {
            List<SalesPackingDetailsBO> PackingDetails = new List<SalesPackingDetailsBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    PackingDetails = dbEntity.SpGetPackingDetails(ProformaInvoiceID, "ProformaInvoice", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesPackingDetailsBO()
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
    }
}
