using BusinessObject;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Razor.Text;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class SalesOrderModel
    {
        public int ID { get; set; }
        public string SONo { get; set; }
        public string SODate { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public string DespatchDate { get; set; }
        public string Status { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategory { get; set; }
        public int SchemeID { get; set; }
        public int PaymentModeID { get; set; }
        public int ItemCategoryID { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int SalesCategoryID { get; set; }
        public int PriceListID { get; set; }
        public int SchemeAllocationID { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsCancelable { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsApproved { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal Rate { get; set; }
        public int PurchaseOrderID { get; set; }
        public int FsoID { get; set; }
        public string Source { get; set; }

        public decimal FreightAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal CessAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public bool IsClone { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList UnitList { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public SelectList PaymentModeList { get; set; }
        public List<SalesItemModel> Items { get; set; }

        public int LocationStateID { get; set; }
        public int BatchTypeID { get; set; }
        public SelectList BillingAddressList { get; set; }
        public SelectList ShippingAddressList { get; set; }
        public int BillingAddressID { get; set; }
        public int ShippingAddressID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string BatchType { get; set; }
        public string PartsNumber { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }
        public CustomersModel CustomerDetails { get; set; }
        public int StoreID { get; set; }
        public string DoctorName { get; set; }
        public int DoctorID { get; set; }
        public bool DirectInvoice { get; set; }
        public int IsPriceEditable { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlaces { get; set; }
        public bool PrintWithItemCode { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int TaxTypeID { get; set; }
        public string TaxType { get; set; }
        public List<SalesAmount> AmountDetails { get; set; }
        public string normalclass { get; set; }
        public string QuotationExpiry { get; set; }
        public string EnquiryDate { get; set; }
        
        public string PaymentTerms { get; set; }
        public string CustomerEnquiryNumber { get; set; }
        public int IsVATExtra { get; set; }
        public int VATPercentageID { get; set; }
        public decimal VATPercentage { get; set; }
        public SelectList VATPercentageList { get; set; }
    }
    public class CustomersModel
    {
        public string Name { get; set; }
        public string CustomerCategoryName { get; set; }
        public string CustomerAccountsCategoryName { get; set; }
        public bool IsBlockedForSalesOrders { get; set; }
        public string GstNo { get; set; }
        public string Color { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string CustomerCode { get; set; }
    }
    public class SalesItemModel
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public List<SecondaryUnitBO> SecondaryUnitList { get; set; }
        public string BatchName { get; set; }
        public int BatchID { get; set; }
        public int ItemCategoryID { get; set; }
        public string Category { get; set; }
        public int SalesOrderItemID { get; set; }
        public int UnitID { get; set; }
        public int CategoryID { get; set; }
        public int SalesUnitID { get; set; }
        public int CurrencyID { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int TaxTypeID { get; set; }
        public string CurrencyName { get; set; }
        public string PartsNumber { get; set; }
        public int BatchTypeID { get; set; }
        public string Remarks { get; set; }
        public string DeliveryTerm { get; set; }
        public string Model { get; set; }
        public decimal? Stock { get; set; }
        public decimal? LooseRate { get; set; }
        
        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal Rate { get; set; }
        public int SalesOrderID { get; set; }
        public int SalesOrderItemTransID { get; set; }
        public int CounterSalesID { get; set; }
        public int CounterSalesItemTransID { get; set; }
        public int SalesInvoiceID { get; set; }
        public int SalesInvoiceTransID { get; set; }
        
        public decimal Qty { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceOfferQty { get; set; }
        public bool InvoiceQtyMet { get; set; }
        public bool InvoiceOfferQtyMet { get; set; }
        public string TransNo { get; set; }
        public decimal OfferQty { get; set; }
        public string FullOrLoose { get; set; }
        public decimal MRP { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal CessAmount { get; set; }

        public int MaxSalesQty { get; set; }
        public int MinSalesQty { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryQty { get; set; }
        public decimal SecondaryMRP { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal NetAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public string DoctorName { get; set; }
        public int DoctorID { get; set; }
        public string BatchType { get; set; }
        public int BillableID { get; set; }
        public int StoreID { get; set; }
        public decimal VATAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool PrintWithItemName { get; set; }
        public decimal GSTAmount
        {
            get
            {
                return CGST + SGST + IGST;
            }

            set
            {
                GSTAmount = value;
            }
        }

    }

    public class UploadOrderModel
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string PackingCode { get; set; }
        public string PackingName { get; set; }
        public decimal Qty { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerID { get; set; }
    }

}