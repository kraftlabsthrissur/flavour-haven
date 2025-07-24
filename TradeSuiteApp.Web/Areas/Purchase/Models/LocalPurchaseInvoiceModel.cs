using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class LocalPurchaseInvoiceModel
    {
        public int ID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string SupplierReference { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public int IsPriceEditable { get; set; }
        public SelectList PurchaseCategoryList { get; set; }
        public int ItemCategoryID { get; set; }
        public int PurchaseCategoryID { get; set; }
        public int LocationID { get; set; }
        public int CountryID { get; set; }
        public string TaxType { get; set; }
        public int TaxTypeID { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }

        public string CountryName { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlaces { get; set; }
        public SelectList BatchTypeList { get; set; }
        public int BatchTypeID { get; set; }
        public decimal NetAmount { get; set; }
        public SelectList UnitList { get; set; }
        public SelectList StoreList { get; set; }
        public int UnitID { get; set; }
        public bool IsDraft { get; set; }
        public bool IsGSTRegisteredLocation { get; set; }
        public decimal GSTAmount { get; set; }
        public int SupplierID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public decimal GrossAmnt { get; set; }
        public int DDLItemCategory { get; set; }
        public int SupplierStateID { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherDeductions { get; set; }
        public int StoreID { get; set; }
        public string Store { get; set; }
        public bool IsCancelable { get; set; }
        public bool IsCanceled { get; set; }
        public List<LocalPurchaseInvoiceItemsModel> Items { get; set; }
        public List<LocalPurchaseAmountDetails> AmountDetails { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Category { get; set; }
        public int PurchaseUnitID { get; set; }
        public int PrimaryUnitID { get; set; }
        public string PurchaseUnit { get; set; }
        public string PrimaryUnit { get; set; }
        public decimal ConversionFactorPtoI { get; set; }
        public decimal DiscountPercent { get; set; }
    }
    public class LocalPurchaseAmountDetails
    {
        public string Particulars { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
    }
    public class LocalPurchaseInvoiceItemsModel
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal CessAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public bool IsGSTRegisteredLocation { get; set; }
        public int CurrencyID { get; set; }
        public decimal NetAmount { get; set; }
        public int UnitID { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public decimal RetailRate { get; set; }
        public decimal RetailMRP { get; set; }
        public string CurrencyName { get; set; }
        public decimal Value { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public string PartsNumber { get; set; }
        public string Remark { get; set; }
        public string Model { get; set; }
        public decimal ExchangeRate { get; set; }
        public string SecondaryUnit { get; set; }
        public Nullable<decimal> SecondaryUnitSize { get; set; }
        public Nullable<decimal> SecondaryRate { get; set; }
        public Nullable<decimal> SecondaryQty { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public int PurchaseCategoryID { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string BatchNo { get; set; }
        public int BatchTypeID { get; set; }
        public string ExpDate { get; set; }
        public decimal? Discount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public int IsGST { get; set; }
        public int IsVAT { get; set; }

    }

    public class MRPModel
    {
        public decimal MRP { get; set; }
        public decimal Rate { get; set; }
        public string ExpDate { get; set; }
    }

    public class ItemCategoryAndUnitModel
    {
        public string Category { get; set; }
        public int PurchaseUnitID { get; set; }
        public int PrimaryUnitID { get; set; }
        public string PurchaseUnit { get; set; }
        public string PrimaryUnit { get; set; }
        public decimal ConversionFactorPtoI { get; set; }

    }
}