using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Reports.Models
{
    public class ProformaInvoiceItemsModel
    {
        public Nullable<int> ProformaInvoiceID { get; set; }
        public Nullable<int> SalesOrderTranID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string DeliveryTerm { get; set; }
        public string SalesOrderNO { get; set; }
        
        public string Model { get; set; }
        public string Make { get; set; }
        public string SalesorderNO { get; set; }
        public DateTime? Orderdate { get; set; }
        public Nullable<bool> PrintWithItemName { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<int> IsGST { get; set; }
        public Nullable<int> IsVat { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public Nullable<decimal> BasicPrice { get; set; }
        public Nullable<decimal> OfferQty { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> AdditionalDiscount { get; set; }
        public Nullable<decimal> TurnoverDiscount { get; set; }
        public Nullable<decimal> TaxableAmount { get; set; }
        public decimal VATAmount { get; set; }
        public Nullable<decimal> VatPercentage { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryMRP { get; set; }
        public decimal SecondaryQty { get; set; }
        public Nullable<decimal> SGSTPercentage { get; set; }
        public Nullable<decimal> CGSTPercentage { get; set; }
        public Nullable<decimal> IGSTPercentage { get; set; }
        public Nullable<decimal> SGSTAmt { get; set; }
        public Nullable<decimal> CGSTAmt { get; set; }
        public Nullable<decimal> IGSTAmt { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public Nullable<int> WareHouseID { get; set; }
        public string UnitName { get; set; }
        public string Code { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> BatchID { get; set; }
        public string BatchNo { get; set; }
        public Nullable<int> BatchTypeID { get; set; }
        public string BatchTypeName { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceOfferQty { get; set; }
        public DateTime? OrderDate { get; set; }

        public string SalesOrderNo { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> LooseRate { get; set; }
        public Nullable<int> SalesUnitID { get; set; }
        public decimal CessAmount { get; set; }
        public decimal CessPercentage { get; set; }
        public Nullable<decimal> Stock { get; set; }
        public Nullable<int> UnitID { get; set; }
        public string Category { get; set; }
        public Nullable<decimal> PackSize { get; set; }
        public string MalayalamName { get; set; }
        public string CurrencyName { get; set; }
        public string PrimaryUnit { get; set; }
        public int CategoryID { get; set; }
    }
}