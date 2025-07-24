using BusinessObject;
using System.Collections.Generic;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class ProformaInvoiceModel
    {
        public int ID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public string SalesDate { get; set; }
        public string Status { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategory { get; set; }
        public int WareHouseID { get; set; }
        public int SalesTypeID { get; set; }
        public string SalesTypeName { get; set; }
        public int PaymentModeID { get; set; }
        public decimal NetAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal RoundOff { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public int ItemCategoryID { get; set; }
        public int SalesCategoryID { get; set; }
        public int StoreID { get; set; }
        public decimal AmountToBeMatched { get; set; }
        public int UnitID { get; set; }
        public decimal CessAmount { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList StoreList { get; set; }
        public SelectList UnitList { get; set; }
        public decimal Balance { get; set; }
        public string InvoiceType { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public SelectList PaymentModeList { get; set; }
        public SelectList WareHouseList { get; set; }
        public SelectList SalesTypeList { get; set; }
        public List<ProformaInvoiceItemModel> Items { get; set; }
        public List<SalesOrderModel> SalesOrders { get; set; }
        public List<ProformaInvoiceAmount> AmountDetails { get; set; }
        public List<PackingDetails> PackingDetails { get; set; }

        public decimal FreightAmount { get; set; }
        public int FreightTax { get; set; }
        public bool CheckStock { get; set; }
        public int StateID { get; set; }
        public int PriceListID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int SchemeID { get; set; }
        public int LocationStateID { get; set; }
        public int BatchTypeID { get; set; }
        public int? NoOfBoxes { get; set; }
        public int? NoOfCans { get; set; }
        public int? NoOfBags { get; set; }
        public string SalesOrderNos { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCancelable { get; set; }
        public bool IsProcessed { get; set; }

        public SelectList BillingAddressList { get; set; }
        public SelectList ShippingAddressList { get; set; }
        public int BillingAddressID { get; set; }
        public int ShippingAddressID { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int TaxTypeID { get; set; }
        public int CurrencyID { get; set; }
        public int DecimalPlaces { get; set; }
        public string normalclass { get; set; }
        public string CurrencyCode { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string CheckedBy { get; set; }
        public string PackedBy { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string GSTNo { get; set; }
        public decimal OutStandingAmount { get; set; }
        public decimal MaxCreditLimit { get; set; }
        public string Remarks { get; set; }
        public bool PrintWithItemCode { get; set; }
    }

    public class ProformaInvoiceItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string DeliveryTerm { get; set; }
        public string Model { get; set; }
        public string Code { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }

        public decimal InvoiceQty { get; set; }
        public decimal SecondaryInvoiceQty { get; set; }
        public decimal InvoiceOfferQty { get; set; }
        public decimal SecondaryInvoiceOfferQty { get; set; }
        public bool InvoiceQtyMet { get; set; }
        public bool InvoiceOfferQtyMet { get; set; }
        public decimal Qty { get; set; }
        public decimal OfferQty { get; set; }
        public decimal PendingOrderQty { get; set; }
        public decimal MRP { get; set; }
        public decimal SecondaryMRP { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public int TaxTypeID { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public int ProformaInvoiceTransID { get; set; }
        public int SalesOrderItemID { get; set; }
        public string SalesOrderNo { get; set; }
        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchName { get; set; }
        public decimal NetAmount { get; set; }
        public int StoreID { get; set; }
        public decimal Stock { get; set; }
        public int SalesUnitID { get; set; }
        public decimal LooseRate { get; set; }
        public decimal Rate { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal CessAmount { get; set; }
        public decimal PackSize { get; set; }
        public string Category { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public string BatchTypeName { get; set; }
        public string PrimaryUnit { get; set; }
        public int CategoryID { get; set; }
        public bool PrintWithItemCode { get; set; }
    }

    public class ProformaInvoiceAmount
    {
        public string Particulars { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
    }

    public class PackingDetails
    {
        public decimal Quantity { get; set; }
        public string PackSize { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
    }

    public static partial class Mapper
    {
        public static ProformaInvoiceBO MapToBO(this ProformaInvoiceModel model)
        {
            ProformaInvoiceBO Invoice = new ProformaInvoiceBO()
            {
                ID = model.ID,
                SalesTypeID = model.SalesTypeID,
                TransNo = model.InvoiceNo,
                TransDate = General.ToDateTime(model.InvoiceDate),
                CustomerID = model.CustomerID,
                SalesOrderNos = model.SalesOrderNos,
                SchemeID = model.SchemeID,
                FreightAmount = model.FreightAmount,
                TurnoverDiscount = model.TurnoverDiscount,
                TaxableAmount = model.TaxableAmount,
                GrossAmount = model.GrossAmount,
                DiscountAmount = model.DiscountAmount,
                DiscountPercentage = model.DiscountPercentage,
                VATAmount = model.VATAmount,
                VATPercentage = model.VATPercentage,
                AdditionalDiscount = model.AdditionalDiscount,
                SGSTAmount = model.SGSTAmount,
                CGSTAmount = model.CGSTAmount,
                IGSTAmount = model.IGSTAmount,
                RoundOff = model.RoundOff,
                NetAmount = model.NetAmount,
                CheckStock = model.CheckStock,
                IsDraft = model.IsDraft,
                BillingAddressID = model.BillingAddressID,
                ShippingAddressID = model.ShippingAddressID,
                NoOfBags = model.NoOfBags,
                NoOfBoxes = model.NoOfBoxes,
                NoOfCans = model.NoOfCans,
                CheckedBy = model.CheckedBy,
                PackedBy = model.PackedBy,
                CessAmount = model.CessAmount,
                PrintWithItemCode = model.PrintWithItemCode,
                Remarks = model.Remarks
            };
            return Invoice;
        }

        public static List<SalesItemBO> MapToBO(this List<ProformaInvoiceItemModel> items)
        {
            List<SalesItemBO> SalesItems = new List<SalesItemBO>();
            SalesItemBO SalesItem;
            foreach (var item in items)
            {
                SalesItem = new SalesItemBO()
                {
                    ItemID = item.ItemID,
                    Code = item.Code,
                    Name = item.ItemName,
                    PartsNumber = item.PartsNumber,
                    IsGST = item.IsGST,
                    IsVat = item.IsVat,
                    CurrencyID = item.CurrencyID,
                    DeliveryTerm = item.DeliveryTerm,
                    Model = item.Model,
                    VATPercentage = item.VATPercentage,
                    VATAmount = item.VATAmount,
                    SecondaryUnitSize = item.SecondaryUnitSize,
                    SecondaryMRP = item.SecondaryMRP,
                    SecondaryOfferQty = item.SecondaryInvoiceOfferQty,
                    SecondaryQty = item.SecondaryInvoiceQty,
                    SecondaryUnit = item.SecondaryUnit,
                    BatchID = item.BatchID,
                    BatchTypeID = item.BatchTypeID,
                    PrintWithItemCode = item.PrintWithItemCode,
                    BatchName = item.BatchName,
                    SalesOrderItemID = item.SalesOrderItemID,
                    ProformaInvoiceTransID = item.ProformaInvoiceTransID,
                    MRP = item.MRP,
                    BasicPrice = item.BasicPrice,
                    InvoiceQty = item.InvoiceQty,
                    InvoiceOfferQty = item.InvoiceOfferQty,
                    Qty = item.Qty,
                    OfferQty = item.OfferQty,
                    GrossAmount = item.GrossAmount,
                    DiscountAmount = item.DiscountAmount,
                    DiscountPercentage = item.DiscountPercentage,
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
                    NetAmount = item.NetAmount,
                    StoreID = item.StoreID,
                    UnitID = item.UnitID,
                    CessAmount = item.CessAmount,
                    CessPercentage = item.CessPercentage
                };
                SalesItems.Add(SalesItem);
            }

            return SalesItems;
        }

        public static List<SalesAmountBO> MapToBO(this List<ProformaInvoiceAmount> amountDetails)
        {
            List<SalesAmountBO> SalesAmountDetails = new List<SalesAmountBO>();
            SalesAmountBO SalesAmount;
            foreach (var item in amountDetails)
            {
                SalesAmount = new SalesAmountBO()
                {
                    Particulars = item.Particulars,
                    Amount = item.Amount,
                    Percentage = item.Percentage
                };
                SalesAmountDetails.Add(SalesAmount);
            }
            return SalesAmountDetails;
        }

        public static List<SalesPackingDetailsBO> MapToBO(this List<PackingDetails> PackingDetails)
        {
            List<SalesPackingDetailsBO> SalesPackingDetailsBO = new List<SalesPackingDetailsBO>();
            SalesPackingDetailsBO PackingDetailsBO;
            foreach (var item in PackingDetails)
            {
                PackingDetailsBO = new SalesPackingDetailsBO()
                {
                    PackSize = item.PackSize,
                    Quantity = item.Quantity,
                    UnitID = item.UnitID
                };
                SalesPackingDetailsBO.Add(PackingDetailsBO);
            }
            return SalesPackingDetailsBO;
        }

        public static ProformaInvoiceModel MapToModel(this ProformaInvoiceBO Invoice)
        {
            ProformaInvoiceModel model = new ProformaInvoiceModel()
            {
                ID = Invoice.ID,
                SalesTypeID = Invoice.SalesTypeID,
                SalesTypeName = Invoice.SalesTypeName,
                InvoiceNo = Invoice.TransNo,
                InvoiceDate = General.FormatDate(Invoice.TransDate),
                CustomerID = Invoice.CustomerID,
                CustomerName = Invoice.CustomerName.Trim(),
                CurrencyCode = Invoice.CurrencyCode,
                CustomerCategory = Invoice.CustomerCategory,
                CustomerCategoryID = Invoice.CustomerCategoryID,
                SalesOrderNos = Invoice.SalesOrderNos,
                SchemeID = Invoice.SchemeID,
                TurnoverDiscount = Invoice.TurnoverDiscount,
                GrossAmount = Invoice.GrossAmount,
                DiscountAmount = Invoice.DiscountAmount,
                AdditionalDiscount = Invoice.AdditionalDiscount,
                SGSTAmount = Invoice.SGSTAmount,
                CGSTAmount = Invoice.CGSTAmount,
                IGSTAmount = Invoice.IGSTAmount,
                RoundOff = Invoice.RoundOff,
                NetAmount = Invoice.NetAmount,
                CheckStock = Invoice.CheckStock,
                IsDraft = Invoice.IsDraft,
                IsCanceled = Invoice.IsCancelled,
                IsProcessed = Invoice.IsProcessed,
                PriceListID = Invoice.PriceListID,
                StateID = Invoice.StateID,
                BillingAddressID = Invoice.BillingAddressID,
                ShippingAddressID = Invoice.ShippingAddressID,
                BillingAddress = Invoice.BillingAddress,
                ShippingAddress = Invoice.ShippingAddress,
                NoOfBags = Invoice.NoOfBags,
                NoOfBoxes = Invoice.NoOfBoxes,
                NoOfCans = Invoice.NoOfCans,
                CheckedBy = Invoice.CheckedBy,
                PackedBy = Invoice.PackedBy,
                CessAmount = Invoice.CessAmount,
                CompanyName = GeneralBO.CompanyName,
                Address1 = GeneralBO.Address1,
                Address2 = GeneralBO.Address2,
                Address3 = GeneralBO.Address3,
                Address4 = GeneralBO.Address4,
                Address5 = GeneralBO.Address5,
                GSTNo = GeneralBO.GSTNo,
                FreightAmount = Invoice.FreightAmount,
                OutStandingAmount = Invoice.OutStandingAmount,
                MaxCreditLimit = Invoice.MaxCreditLimit,
                Remarks = Invoice.Remarks,
                IsGSTRegistered = Invoice.IsGSTRegistered,
                PrintWithItemCode = Invoice.PrintWithItemCode
            };
            return model;
        }

        public static List<ProformaInvoiceItemModel> MapToModel(this List<SalesItemBO> items)
        {
            List<ProformaInvoiceItemModel> ModelItems = new List<ProformaInvoiceItemModel>();
            ProformaInvoiceItemModel ModelItem;
            foreach (var item in items)
            {
                ModelItem = new ProformaInvoiceItemModel()
                {
                    ItemID = item.ItemID,
                    ItemName = item.ItemName,
                    PartsNumber = item.PartsNumber,
                    PrintWithItemCode = item.PrintWithItemCode,
                    DeliveryTerm = item.DeliveryTerm,
                    Model = item.Model,
                    Code = item.Code,
                    UnitName = item.Unit,
                    BatchName = item.BatchName,
                    BatchID = item.BatchID,
                    BatchTypeID = item.BatchTypeID,
                    SalesOrderItemID = item.SalesOrderItemID,
                    ProformaInvoiceTransID = item.ProformaInvoiceTransID,
                    Rate = item.Rate,
                    BasicPrice = item.BasicPrice,
                    Qty = item.Qty,
                    OfferQty = item.OfferQty,
                    GrossAmount = item.GrossAmount,
                    DiscountPercentage = item.DiscountPercentage,
                    DiscountAmount = item.DiscountAmount,
                    TurnoverDiscount = item.TurnoverDiscount,
                    AdditionalDiscount = item.AdditionalDiscount,
                    TaxableAmount = item.TaxableAmount,
                    GSTPercentage = (decimal)item.GSTPercentage,
                    SGSTPercentage = item.SGSTPercentage,
                    CGSTPercentage = item.CGSTPercentage,
                    IGSTPercentage = item.IGSTPercentage,
                    VATPercentage = item.VATPercentage,
                    VATAmount = item.VATAmount,
                    IsGST = item.IsGST,
                    IsVat = item.IsVat,
                    CurrencyID = item.CurrencyID,
                    CurrencyName = item.CurrencyName,
                    IGST = item.IGST,
                    CGST = item.CGST,
                    SGST = item.SGST,
                    GSTAmount = item.IGST + item.CGST + item.SGST,
                    NetAmount = item.NetAmount,
                    StoreID = item.StoreID,
                    PendingOrderQty = item.Qty - item.QtyMet,
                    InvoiceQty = item.InvoiceQty,
                    InvoiceOfferQty = item.InvoiceOfferQty,
                    InvoiceQtyMet = item.InvoiceQtyMet,
                    InvoiceOfferQtyMet = item.InvoiceOfferQtyMet,
                    SalesOrderNo = item.SalesOrderNo,
                    Stock = item.Stock,
                    UnitID = item.UnitID,
                    SalesUnitID = item.SalesUnitID,
                    LooseRate = item.LooseRate,
                    MRP = item.MRP,
                    CessAmount = item.CessAmount,
                    CessPercentage = item.CessPercentage,
                    Category = item.Category,
                    PackSize = item.PackSize,
                    SecondaryUnit = item.SecondaryUnit,
                    SecondaryMRP = item.SecondaryMRP,
                    SecondaryInvoiceOfferQty = item.SecondaryOfferQty,
                    SecondaryInvoiceQty = item.SecondaryQty,
                    SecondaryUnitSize = item.SecondaryUnitSize,
                    BatchTypeName = item.BatchTypeName,
                    PrimaryUnit = item.PrimaryUnit,
                    CategoryID = item.ItemCategoryID
                };
                ModelItems.Add(ModelItem);
            }

            return ModelItems;
        }

        public static List<ProformaInvoiceAmount> MapToModel(this List<SalesAmountBO> amountDetails)
        {
            List<ProformaInvoiceAmount> ModelSalesAmountDetails = new List<ProformaInvoiceAmount>();
            ProformaInvoiceAmount ModelSalesAmount;
            foreach (var item in amountDetails)
            {
                ModelSalesAmount = new ProformaInvoiceAmount()
                {
                    Particulars = item.Particulars,
                    Amount = item.Amount,
                    Percentage = item.Percentage
                };
                ModelSalesAmountDetails.Add(ModelSalesAmount);
            }
            return ModelSalesAmountDetails;
        }

        public static List<PackingDetails> MapToModel(this List<SalesPackingDetailsBO> packingDetails)
        {
            List<PackingDetails> PackingDetails = new List<PackingDetails>();
            PackingDetails ModelPackingDetails;
            foreach (var item in packingDetails)
            {
                ModelPackingDetails = new PackingDetails()
                {
                    PackSize = item.PackSize,
                    UnitID = item.UnitID,
                    UnitName = item.UnitName,
                    Quantity = item.Quantity
                };
                PackingDetails.Add(ModelPackingDetails);
            }
            return PackingDetails;
        }
    }
}