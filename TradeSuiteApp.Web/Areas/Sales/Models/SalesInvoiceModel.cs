using BusinessObject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class SalesInvoiceModel
    {
        public SalesInvoiceModel()
        {
        }
        public int ID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public string Status { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategory { get; set; }
        public int SalesTypeID { get; set; }
        public string SalesTypeName { get; set; }
        public int PaymentModeID { get; set; }
        public string PaymentMode { get; set; }
        public int StateID { get; set; }
        public int PriceListID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int SchemeID { get; set; }
        public int LocationStateID { get; set; }
        public bool CheckStock { get; set; }
        public string InvoiceType { get; set; }
        public int ItemCategoryID { get; set; }
        public int SalesCategoryID { get; set; }
        public int StoreID { get; set; }
        public string SalesOrderNos { get; set; }
        public int SalesOrderID { get; set; }

        public int FreightTax { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal TurnoverDiscountAvailable { get; set; }
        public bool CashDiscountEnabled { get; set; }
        public decimal CashDiscountPercentage { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal CessAmount { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCanceled { get; set; }
        public bool PrintWithItemCode { get; set; }
        public bool IsCancelable { get; set; }
        public bool IsProcessed { get; set; }
        public decimal MaxCreditLimit { get; set; }
        public decimal MinCreditLimit { get; set; }
        public decimal CreditAmount { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public SelectList PaymentModeList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList StoreList { get; set; }

        public List<SalesInvoiceItemModel> Items { get; set; }
        public List<SalesAmount> AmountDetails { get; set; }
        public List<PackingDetailsModel> PackingDetails { get; set; }

        public SelectList BillingAddressList { get; set; }
        public SelectList ShippingAddressList { get; set; }
        public int BillingAddressID { get; set; }
        public int ShippingAddressID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int? NoOfBoxes { get; set; }
        public int? NoOfCans { get; set; }
        public int? NoOfBags { get; set; }

        public string District { get; set; }
        public int PPSNo { get; set; }
        public string DeliveryDate { get; set; }

        public string CustomerPONo { get; set; }
        public string ServiceSalesOrderNos { get; set; }

        public BillingAddressModel BillingTo { get; set; }
        public ShippingAddressModel ShippingTo { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string GSTNo { get; set; }
        public string NetAmountInWords { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string Remarks { get; set; }
        public List<CategoryBO> DiscountPercentageList { get; set; }

        public int DiscountCategoryID { get; set; }
        public string DiscountCategory { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int TaxTypeID { get; set; }
        public string TaxType { get; set; }
        public string normalclass { get; set; }
        public int DecimalPlaces { get; set; }
        public string CustomerPODate { get; set; }
        public int IsVATExtra { get; set; }
        public int VATPercentageID { get; set; }
        public decimal VATPercentage { get; set; }
        public SelectList VATPercentageList { get; set; }
        public decimal OtherChargesVATAmount { get; set; }
    }

    public class SalesInvoiceItemModel
    {
        public int SalesOrderItemID { get; set; }
        public int ProformaInvoiceTransID { get; set; }
        public int SalesInvoiceTransID { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public string Code { get; set; }

        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public int UnitID { get; set; }
        public int CurrencyID { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public decimal ExchangeRate { get; set; }
        public int TaxTypeID { get; set; }
        public decimal LooseRate { get; set; }
        public string UnitName { get; set; }
        public decimal Qty { get; set; }
        public decimal OfferQty { get; set; }
        public int SalesUnitID { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceOfferQty { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public decimal DiscPercentage { get; set; }
        public decimal Amount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public string CurrencyName { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TradeDiscPercentage { get; set; }
        public decimal TradeDiscAmount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal CashDiscount { get; set; }
        public int StoreID { get; set; }

        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchName { get; set; }
        public bool InvoiceQtyMet { get; set; }
        public bool InvoiceOfferQtyMet { get; set; }
        public decimal Stock { get; set; }
        public int POID { get; set; }
        public int POTransID { get; set; }
        public decimal PORate { get; set; }
        public int SalesInvoiceID { get; set; }
        public string Batch { get; set; }
        public decimal POQuantity { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal CessAmount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal PackSize { get; set; }
        public string BatchType { get; set; }
        public string PrimaryUnit { get; set; }
        public string DeliveryTerm { get; set; }
        public string Model { get; set; }
        public bool PrintWithItemCode { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryInvoiceQty { get; set; }
        public decimal SecondaryInvoiceOfferQty { get; set; }
        public decimal SecondaryMRP { get; set; }
        public string Remarks { get; set; }
    }
    public class SalesAmount
    {
        public string Particulars { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxableAmount { get; set; }
    }

    public class PackingDetailsModel
    {
        public decimal Quantity { get; set; }
        public string PackSize { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
    }

    public class BillingAddressModel
    {
        public Nullable<int> LocationID { get; set; }
        public string LocationCode { get; set; }
        public string Location { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string SupplierCode { get; set; }
        public string Supplier { get; set; }
        public int AddressID { get; set; }
        public string Place { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public int StateID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string MobileNo { get; set; }
        public int DistrictID { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
    }

    public class ShippingAddressModel
    {
        public Nullable<int> LocationID { get; set; }
        public string LocationCode { get; set; }
        public string Location { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string SupplierCode { get; set; }
        public string Supplier { get; set; }
        public int AddressID { get; set; }
        public string Place { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public int StateID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string MobileNo { get; set; }
        public int DistrictID { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
    }



    public static partial class Mapper
    {
        public static SalesInvoiceBO MapToBO(this SalesInvoiceModel model)
        {
            SalesInvoiceBO Invoice = new SalesInvoiceBO()
            {
                ID = model.ID,
                SalesTypeID = model.SalesTypeID,
                InvoiceNo = model.InvoiceNo,
                InvoiceDate = General.ToDateTime(model.InvoiceDate),
                CustomerID = model.CustomerID,
                PaymentModeID = model.PaymentModeID,
                SalesOrderNos = model.SalesOrderNos,
                SchemeID = model.SchemeID,
                GrossAmount = model.GrossAmount,
                FreightAmount = model.FreightAmount,
                AdditionalDiscount = model.AdditionalDiscount,
                DiscountAmount = model.DiscountAmount,
                DiscountPercentage = model.DiscountPercentage,
                TurnoverDiscount = model.TurnoverDiscount,
                TaxableAmount = model.TaxableAmount,
                SGSTAmount = model.SGSTAmount,
                CGSTAmount = model.CGSTAmount,
                IGSTAmount = model.IGSTAmount,
                VATAmount = model.VATAmount,
                IsGST = model.IsGST,
                IsVat = model.IsVat,
                CurrencyID = model.CurrencyID,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                RoundOff = model.RoundOff,
                OtherCharges = model.OtherCharges,
                CustomerPONo = model.CustomerPONo,
                CustomerPODate = General.ToDateTimeNull(model.CustomerPODate),
                CashDiscount = model.CashDiscount,
                NetAmount = model.NetAmount,
                CheckStock = model.CheckStock,
                IsDraft = model.IsDraft,
                BillingAddressID = model.BillingAddressID,
                ShippingAddressID = model.ShippingAddressID,
                NoOfBags = model.NoOfBags,
                NoOfBoxes = model.NoOfBoxes,
                NoOfCans = model.NoOfCans,
                CessAmount = model.CessAmount,
                PrintWithItemCode = model.PrintWithItemCode,
                Remarks = model.Remarks,
                DiscountCategoryID = model.DiscountCategoryID,
                VATPercentageID = model.VATPercentageID,
                VATPercentage = model.VATPercentage,
                OtherChargesVATAmount = model.OtherChargesVATAmount
            };
            return Invoice;
        }

        public static List<SalesItemBO> MapToBO(this List<SalesInvoiceItemModel> items)
        {
            List<SalesItemBO> SalesItems = new List<SalesItemBO>();
            SalesItemBO SalesItem;
            foreach (var item in items)
            {
                SalesItem = new SalesItemBO()
                {
                    ItemID = item.ItemID,
                    BatchID = item.BatchID,
                    Name = item.ItemName,
                    PartsNumber = item.PartsNumber,
                    DeliveryTerm = item.DeliveryTerm,
                    Model = item.Model,
                    BatchTypeID = item.BatchTypeID,
                    SalesOrderItemID = item.SalesOrderItemID,
                    ProformaInvoiceTransID = item.ProformaInvoiceTransID,
                    MRP = item.MRP,
                    BasicPrice = item.BasicPrice,
                    Qty = item.Qty,
                    OfferQty = item.OfferQty,
                    InvoiceQty = item.InvoiceQty,
                    InvoiceOfferQty = item.InvoiceOfferQty,
                    GrossAmount = item.GrossAmount,
                    DiscountPercentage = item.DiscountPercentage,
                    DiscountAmount = item.DiscountAmount,
                    AdditionalDiscount = item.AdditionalDiscount,
                    TurnoverDiscount = item.TurnoverDiscount,
                    TaxableAmount = item.TaxableAmount,
                    GSTPercentage = item.GSTPercentage,
                    SGSTPercentage = item.SGSTPercentage,
                    CGSTPercentage = item.CGSTPercentage,
                    IGSTPercentage = item.IGSTPercentage,
                    VATPercentage = item.VATPercentage,
                    IGST = item.IGST,
                    CGST = item.CGST,
                    SGST = item.SGST,
                    VATAmount = item.VATAmount,
                    IsGST = item.IsGST,
                    IsVat = item.IsVat,
                    CurrencyID = item.CurrencyID,
                    ExchangeRate = item.ExchangeRate,
                    CashDiscount = item.CashDiscount,
                    NetAmount = item.NetAmount,
                    StoreID = item.StoreID,
                    UnitID = item.UnitID,
                    SecondaryMRP = item.SecondaryMRP,
                    SecondaryOfferQty = item.SecondaryInvoiceOfferQty,
                    SecondaryQty = item.SecondaryInvoiceQty,
                    SecondaryUnit = item.SecondaryUnit,
                    SecondaryUnitSize = item.SecondaryUnitSize,
                    CessAmount = item.CessAmount,
                    CessPercentage = item.CessPercentage,
                    PrintWithItemCode = item.PrintWithItemCode,
                };
                SalesItems.Add(SalesItem);
            }

            return SalesItems;
        }

        public static List<SalesAmountBO> MapToBO(this List<SalesAmount> amountDetails)
        {
            List<SalesAmountBO> SalesAmountDetails = new List<SalesAmountBO>();
            SalesAmountBO SalesAmount;
            foreach (var item in amountDetails)
            {
                SalesAmount = new SalesAmountBO()
                {
                    Particulars = item.Particulars,
                    Amount = item.Amount,
                    Percentage = item.Percentage,
                    TaxableAmount = item.TaxableAmount
                };
                SalesAmountDetails.Add(SalesAmount);
            }
            return SalesAmountDetails;
        }

        public static List<SalesPackingDetailsBO> MapToBO(this List<PackingDetailsModel> packingDetails)
        {
            List<SalesPackingDetailsBO> SalesPackingDetails = new List<SalesPackingDetailsBO>();
            SalesPackingDetailsBO PackingDetails;
            foreach (var item in packingDetails)
            {
                PackingDetails = new SalesPackingDetailsBO()
                {
                    PackSize = item.PackSize,
                    UnitID = item.UnitID,
                    UnitName = item.UnitName,
                    Quantity = item.Quantity
                };
                SalesPackingDetails.Add(PackingDetails);
            }
            return SalesPackingDetails;
        }

        public static SalesInvoiceModel MapToSalesModel(this SalesInvoiceBO Invoice)
        {
            SalesInvoiceModel model = new SalesInvoiceModel()
            {
                ID = Invoice.ID,
                SalesTypeID = Invoice.SalesTypeID,
                SalesTypeName = Invoice.SalesTypeName,
                InvoiceNo = Invoice.InvoiceNo,
                InvoiceDate = General.FormatDate(Invoice.InvoiceDate),
                CustomerID = Invoice.CustomerID,
                CustomerName = Invoice.CustomerName.Trim(),
                CurrencyCode = Invoice.CustomerCode,
                CustomerCategory = Invoice.CustomerCategory,
                CustomerCategoryID = Invoice.CustomerCategoryID,
                FreightAmount = Invoice.FreightAmount,
                SalesOrderNos = Invoice.SalesOrderNos,
                SchemeID = Invoice.SchemeID,
                TurnoverDiscount = Invoice.TurnoverDiscount,
                GrossAmount = Invoice.GrossAmount,
                DiscountAmount = Invoice.DiscountAmount,
                AdditionalDiscount = Invoice.AdditionalDiscount,
                SGSTAmount = Invoice.SGSTAmount,
                CGSTAmount = Invoice.CGSTAmount,
                IGSTAmount = Invoice.IGSTAmount,
                VATAmount = Invoice.VATAmount,
                CurrencyName = Invoice.CurrencyName,
                CurrencyID = Invoice.CurrencyID,
                IsGST = Invoice.IsGST,
                IsVat = Invoice.IsVat,
                CurrencyExchangeRate = Invoice.CurrencyExchangeRate,
                RoundOff = Invoice.RoundOff,
                CashDiscount = Invoice.CashDiscount,
                NetAmount = Invoice.NetAmount,
                CheckStock = Invoice.CheckStock,
                IsDraft = Invoice.IsDraft,
                IsCanceled = Invoice.IsCancelled,
                IsProcessed = Invoice.IsProcessed,
                PriceListID = Invoice.PriceListID,
                StateID = Invoice.StateID,
                PaymentModeID = Invoice.PaymentModeID,
                PaymentMode = Invoice.PaymentMode,
                BillingAddressID = Invoice.BillingAddressID,
                ShippingAddressID = Invoice.ShippingAddressID,
                BillingAddress = Invoice.BillingAddress,
                ShippingAddress = Invoice.ShippingAddress,
                NoOfCans = Invoice.NoOfCans,
                NoOfBoxes = Invoice.NoOfBoxes,
                NoOfBags = Invoice.NoOfBags,
                CessAmount = Invoice.CessAmount,
                MinCreditLimit = Invoice.MinCreditLimit,
                MaxCreditLimit = Invoice.MaxCreditLimit,
                CreditAmount = Invoice.CreditAmount,
                CashDiscountPercentage = Invoice.CashDiscountPercentage,
                CompanyName = GeneralBO.CompanyName,
                Address1 = GeneralBO.Address1,
                Address2 = GeneralBO.Address2,
                Address3 = GeneralBO.Address3,
                Address4 = GeneralBO.Address4,
                Address5 = GeneralBO.Address5,
                GSTNo = GeneralBO.GSTNo,
                TaxableAmount = Invoice.TaxableAmount,
                OutstandingAmount = Invoice.OutstandingAmount,
                Remarks = Invoice.Remarks,
                PrintWithItemCode = Invoice.PrintWithItemCode,
                IsGSTRegistered = Invoice.IsGSTRegistered,
                DiscountCategory = Invoice.DiscountCategory,
                DiscountCategoryID = Invoice.DiscountCategoryID,
                OtherCharges = Invoice.OtherCharges,
                CustomerPONo = Invoice.CustomerPONo,
                CustomerPODate = Invoice.CustomerPODate.HasValue ? General.FormatDate(Invoice.CustomerPODate.Value) : "",
                VATPercentageID = Invoice.VATPercentageID,
                VATPercentage = Invoice.VATPercentage,
                OtherChargesVATAmount = Invoice.OtherChargesVATAmount
            };
            return model;
        }

        public static List<SalesInvoiceItemModel> MapToSalesModel(this List<SalesItemBO> items)
        {
            List<SalesInvoiceItemModel> ModelItems = new List<SalesInvoiceItemModel>();
            SalesInvoiceItemModel ModelItem;
            foreach (var item in items)
            {
                ModelItem = new SalesInvoiceItemModel()
                {
                    ItemID = item.ItemID,
                    BatchID = item.BatchID,
                    BatchTypeID = item.BatchTypeID,
                    ItemName = item.Name,
                    Code = item.Code,
                    PartsNumber = item.PartsNumber,
                    DeliveryTerm = item.DeliveryTerm,
                    Model = item.Model,
                    CurrencyName = item.CurrencyName,
                    CurrencyID = item.CurrencyID,
                    IsGST = item.IsGST,
                    IsVat = item.IsVat,
                    VATAmount = item.VATAmount,
                    VATPercentage = item.VATPercentage,
                    UnitName = item.UnitName,
                    BatchName = item.BatchName,
                    SalesOrderItemID = item.SalesOrderItemID,
                    ProformaInvoiceTransID = item.ProformaInvoiceTransID,
                    SalesInvoiceTransID = item.SalesInvoiceTransID,
                    MRP = item.MRP,
                    BasicPrice = item.BasicPrice,
                    Qty = item.Qty,
                    OfferQty = item.OfferQty,
                    InvoiceQty = item.InvoiceQty,
                    InvoiceOfferQty = item.InvoiceOfferQty,
                    SecondaryUnitSize = item.SecondaryUnitSize,
                    SecondaryUnit = item.SecondaryUnit,
                    SecondaryMRP = item.SecondaryMRP,
                    SecondaryInvoiceOfferQty = item.SecondaryOfferQty,
                    SecondaryInvoiceQty = item.SecondaryQty,
                    GrossAmount = item.GrossAmount,
                    DiscountPercentage = item.DiscountPercentage,
                    DiscountAmount = item.DiscountAmount,
                    TurnoverDiscount = item.TurnoverDiscount,
                    AdditionalDiscount = item.AdditionalDiscount,
                    TaxableAmount = item.TaxableAmount,
                    GSTPercentage = item.GSTPercentage,
                    SGSTPercentage = item.SGSTPercentage,
                    CGSTPercentage = item.CGSTPercentage,
                    IGSTPercentage = item.IGSTPercentage,
                    IGST = item.IGST,
                    CGST = item.CGST,
                    SGST = item.SGST,
                    GSTAmount = item.IGST + item.CGST + item.SGST,
                    CashDiscount = item.CashDiscount,
                    NetAmount = item.NetAmount,
                    StoreID = item.StoreID,
                    Stock = item.Stock,
                    InvoiceQtyMet = item.InvoiceQtyMet,
                    InvoiceOfferQtyMet = item.InvoiceOfferQtyMet,
                    UnitID = item.UnitID,
                    SalesUnitID = item.SalesUnitID,
                    LooseRate = item.LooseRate,
                    Rate = item.Rate,
                    POID = item.POID,
                    POTransID = item.POTransID,
                    PORate = item.PORate,
                    SalesInvoiceID = item.SalesInvoiceID,
                    Batch = item.BatchName,
                    POQuantity = item.POQuantity,
                    CessAmount = item.CessAmount,
                    CessPercentage = item.CessPercentage,
                    ExpiryDate = item.ExpiryDate,
                    PackSize = item.PackSize,
                    BatchType = item.BatchTypeName,
                    PrimaryUnit = item.PrimaryUnit
                };
                ModelItems.Add(ModelItem);
            }

            return ModelItems;
        }

        public static List<SalesAmount> MapToSalesModel(this List<SalesAmountBO> amountDetails)
        {
            List<SalesAmount> ModelSalesAmountDetails = new List<SalesAmount>();
            SalesAmount ModelSalesAmount;
            foreach (var item in amountDetails)
            {
                ModelSalesAmount = new SalesAmount()
                {
                    Particulars = item.Particulars,
                    Amount = item.Amount,
                    Percentage = item.Percentage,
                    TaxableAmount = item.TaxableAmount
                };
                ModelSalesAmountDetails.Add(ModelSalesAmount);
            }
            return ModelSalesAmountDetails;
        }

        public static List<PackingDetailsModel> MapToSalesModel(this List<SalesPackingDetailsBO> PackingDetails)
        {
            List<PackingDetailsModel> SalesPackingDetailsBO = new List<PackingDetailsModel>();
            PackingDetailsModel PackingDetailsBO;
            foreach (var item in PackingDetails)
            {
                PackingDetailsBO = new PackingDetailsModel()
                {
                    PackSize = item.PackSize,
                    Quantity = item.Quantity,
                    UnitID = item.UnitID,
                    UnitName = item.UnitName
                };
                SalesPackingDetailsBO.Add(PackingDetailsBO);
            }
            return SalesPackingDetailsBO;
        }
    }
}