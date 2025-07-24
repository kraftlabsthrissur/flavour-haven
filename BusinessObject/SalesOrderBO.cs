using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SalesOrderBO
    {
        public int ID { get; set; }
        public string SONo { get; set; }
        public DateTime SODate { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public int PriceListID { get; set; }
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public DateTime DespatchDate { get; set; }
        public bool PrintWithItemCode { get; set; }
        public string Status { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategory { get; set; }
        public int PaymentModeID { get; set; }
        public int ItemCategoryID { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsProcessed { get; set; }
        public int SalesCategoryID { get; set; }
        public int SchemeAllocationID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int FsoID { get; set; }
        public string Source { get; set; }
        public bool IsApproved { get; set; }
        public int SchemeID { get; set; }
        public int SalesTypeID { get; set; }
        public string QuotationNo { get; set; }
        public string Remarks { get; set; }


        public decimal FreightAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public int CurrencyID { get; set; }
        public string Currencyname { get; set; }
        public string CurrencyCode { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CessAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public int BillingAddressID { get; set; }
        public int ShippingAddressID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public CustomerBO CustomerDetails { get; set; }
        public bool DirectInvoice { get; set; }
        public int PaymentTypeID { get; set; }
        public decimal VATAmount { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int TaxTypeID { get; set; }
        public string TaxType { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public DateTime? QuotationExpiry { get; set; }
        public string PaymentTerms { get; set; }
        public string CustomerEnquiryNumber { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public string AadhaarNo { get; set; }
        public string AmountInWords { get; set; }
        public string MinimumCurrency { get; set; }
        public int DecimalPlaces { get; set; }
        public int VATPercentageID { get; set; }
        public decimal VATPercentage { get; set; }

    }

    public class UploadOrderBO
    {
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string PackingCode { get; set; }
        public string PackingName { get; set; }
        public decimal Qty { get; set; }
        public decimal MRP { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal CessPercentage { get; set; }
        public int ItemCategoryID { get; set; }
        public string CustomerCode { get; set; }
        public string OldCode { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
    }
}
