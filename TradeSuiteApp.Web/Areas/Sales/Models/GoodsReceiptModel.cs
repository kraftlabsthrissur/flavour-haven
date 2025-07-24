using BusinessLayer;
using BusinessObject;
using System.Collections.Generic;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class GoodsReceiptModel
    {
        public bool PrintWithItemCode { get; set; }
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
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
        public List<GoodsReceiptItemModel> Items { get; set; }



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
    }

    public class GoodsReceiptItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string Remarks { get; set; }
        public string Model { get; set; }
        public string Code { get; set; }
        public string TransNo { get; set; }
        public int SalesOrderID { get; set; }
        public int SalesOrderItemTransID { get; set; }
        public int CounterSalesID { get; set; }
        public int CounterSalesItemTransID { get; set; }
        public int SalesInvoiceID { get; set; }
        public int SalesInvoiceTransID { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal SecondaryQty { get; set; }
        public decimal InvoiceOfferQty { get; set; }
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
        public string BatchTypeName { get; set; }
        public string PrimaryUnit { get; set; }
        public int CategoryID { get; set; }
        public bool PrintWithItemName { get; set; }
    }



    public static partial class Mapper
    {
        public static SalesGoodsReceiptBO MapToBO(this GoodsReceiptModel model)
        {
            SalesGoodsReceiptBO Invoice = new SalesGoodsReceiptBO()
            {
                ID = model.ID,
                SalesTypeID = model.SalesTypeID,
                TransNo = model.TransNo,
                TransDate = General.ToDateTime(model.TransDate),
                CustomerID = model.CustomerID,
                SalesOrderNos = model.SalesOrderNos,
                TurnoverDiscount = model.TurnoverDiscount,
                TaxableAmount = model.TaxableAmount,
                GrossAmount = model.GrossAmount,
                DiscountAmount = model.DiscountAmount,
                AdditionalDiscount = model.AdditionalDiscount,
                SGSTAmount = model.SGSTAmount,
                CGSTAmount = model.CGSTAmount,
                IGSTAmount = model.IGSTAmount,
                RoundOff = model.RoundOff,
                NetAmount = model.NetAmount,
                IsDraft = model.IsDraft,
                CessAmount = model.CessAmount,
                Remarks = model.Remarks
            };
            return Invoice;
        }

        public static List<SalesGoodsReceiptItemBO> MapToBO(this List<GoodsReceiptItemModel> items)
        {
            List<SalesGoodsReceiptItemBO> SalesItems = new List<SalesGoodsReceiptItemBO>();
            SalesGoodsReceiptItemBO salesGoodsReceiptItem;
            foreach (var item in items)
            {
                salesGoodsReceiptItem = new SalesGoodsReceiptItemBO()
                {
                    ItemID = item.ItemID,
                    CounterSalesID = item.CounterSalesID,
                    CounterSalesItemTransID = item.CounterSalesItemTransID,
                    SalesOrderID = item.SalesOrderID,
                    SalesOrderItemTransID = item.SalesOrderItemTransID,
                    SalesInvoiceID = item.SalesInvoiceID,
                    SalesInvoiceTransID = item.SalesInvoiceTransID,
                    TransNo = item.TransNo,
                    Code = item.Code,
                    Name = item.ItemName,
                    PartsNumber = item.PartsNumber,
                    IsGST = item.IsGST,
                    IsVat = item.IsVat,
                    CurrencyID = item.CurrencyID,
                    Remarks = item.Remarks,
                    Model = item.Model,
                    VATPercentage = item.VATPercentage,
                    BatchID = item.BatchID,
                    BatchTypeID = item.BatchTypeID,
                    PrintWithItemName = item.PrintWithItemName,
                    BatchName = item.BatchName,
                    MRP = item.MRP,
                    BasicPrice = item.BasicPrice,
                    InvoiceQty = item.InvoiceQty,
                    InvoiceOfferQty = item.InvoiceOfferQty,
                    Qty = item.Qty,
                    SecondaryUnit = item.SecondaryUnit,
                    SecondaryMRP = item.SecondaryMRP,
                    SecondaryQty = item.SecondaryQty,
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
                SalesItems.Add(salesGoodsReceiptItem);
            }

            return SalesItems;
        }



        public static GoodsReceiptModel MapToModel(this SalesGoodsReceiptBO Invoice)
        {
            GoodsReceiptModel model = new GoodsReceiptModel()
            {
                ID = Invoice.ID,
                TransNo = Invoice.TransNo,
                TransDate = General.FormatDate(Invoice.TransDate),
                CustomerID = Invoice.CustomerID,
                CustomerName = Invoice.CustomerName,
                SalesOrderNos = Invoice.SalesOrderNos,
                TurnoverDiscount = Invoice.TurnoverDiscount,
                GrossAmount = Invoice.GrossAmount,
                DiscountAmount = Invoice.DiscountAmount,
                SGSTAmount = Invoice.SGSTAmount,
                CGSTAmount = Invoice.CGSTAmount,
                IGSTAmount = Invoice.IGSTAmount,
                RoundOff = Invoice.RoundOff,
                NetAmount = Invoice.NetAmount,
                CheckStock = Invoice.CheckStock,
                IsDraft = Invoice.IsDraft,
                IsCanceled = Invoice.IsCancelled,
                NoOfBags = Invoice.NoOfBags,
                NoOfBoxes = Invoice.NoOfBoxes,
                NoOfCans = Invoice.NoOfCans,
                CheckedBy = Invoice.CheckedBy,
                PackedBy = Invoice.PackedBy,
                CessAmount = Invoice.CessAmount,
                Remarks = Invoice.Remarks,
            };
            return model;
        }

        public static List<GoodsReceiptItemModel> MapToModel(this List<SalesGoodsReceiptItemBO> items)
        {
            List<GoodsReceiptItemModel> ModelItems = new List<GoodsReceiptItemModel>();
            GoodsReceiptItemModel ModelItem;
            foreach (var item in items)
            {
                ModelItem = new GoodsReceiptItemModel()
                {
                    ItemID = item.ItemID,
                    Code = item.Code,
                    ItemName = item.Name,
                    PartsNumber = item.PartsNumber,
                    PrintWithItemName = item.PrintWithItemName,
                    Remarks = item.Remarks,
                    Model = item.Model,
                    UnitName = item.Unit,
                    BatchName = item.BatchName,
                    BatchID = item.BatchID,
                    BatchTypeID = item.BatchTypeID,
                    SalesOrderItemID = item.SalesOrderItemID,
                    Rate = item.Rate,
                    BasicPrice = item.BasicPrice,
                    Qty = item.Qty,
                    SecondaryQty = item.SecondaryQty,
                    SecondaryMRP = item.SecondaryMRP,
                    SecondaryUnit = item.SecondaryUnit,
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
                    BatchTypeName = item.BatchTypeName,
                    PrimaryUnit = item.PrimaryUnit,
                    CategoryID = item.ItemCategoryID
                };
                ModelItems.Add(ModelItem);
            }

            return ModelItems;
        }

    }
}