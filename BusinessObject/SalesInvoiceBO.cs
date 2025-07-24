//file created by prama on  6-7-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessObject
{
    public class SalesInvoiceBO
    {
        public int ID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public string Status { get; set; }
        public int SalesTypeID { get; set; }
        public int PaymentModeID { get; set; }
        public string PaymentMode { get; set; }
        public int PaymentTypeID { get; set; }
        public int SalesOrderID { get; set; }
        public string SalesType { get; set; }
        public DateTime EnquiryDate { get; set; }
        public int ItemCategoryID { get; set; }
        public int SalesCategoryID { get; set; }
        public int StoreID { get; set; }
        public decimal Balance { get; set; }
        public string InvoiceType { get; set; }
        public string SalesTypeName { get; set; }

        public string CustomerCategory { get; set; }
        public int CustomerCategoryID { get; set; }
        public int PriceListID { get; set; }
        public int StateID { get; set; }

        public decimal FreightAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal OtherCharges { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int CurrencyID { get; set; }
        public string CustomerPONo { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyExchangeRate { get; set; }

        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public bool IsProcessed { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDraft { get; set; }
        public string RefferenceNo { get; set; }
        public bool CheckStock { get; set; }

        public string SalesOrderNos { get; set; }
        public int SchemeID { get; set; }
        public int? NoOfBoxes { get; set; }
        public int? NoOfCans { get; set; }
        public int? NoOfBags { get; set; }
        public List<SalesInvoiceItemBO> Items { get; set; }
        public List<SalesAmountBO> AmountDetails { get; set; }

        public int BillingAddressID { get; set; }
        public int ShippingAddressID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }

        public string District { get; set; }
        public int PPSNO { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal Amount { get; set; }
        public decimal CessAmount { get; set; }

        public decimal MaxCreditLimit { get; set; }
        public decimal MinCreditLimit { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal CashDiscountPercentage { get; set; }
        public decimal OutstandingAmount { get; set; }
        public int CreditNoteID { get; set; }
        public int DebitNoteID { get; set; }
        public int SalesReturnID { get; set; }
        public string VehicleNo { get; set; }
        public string CustomerGSTNo { get; set; }
        public string Remarks { get; set; }
        public bool PrintWithItemCode {  get; set; }
        public bool IsGSTRegistered { get; set; }

        public int DiscountCategoryID { get; set; }
        public string DiscountCategory { get; set; }

        public int SchemeAllocationID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int FsoID { get; set; }
        public string Source { get; set; }
        public bool IsApproved { get; set; }
        public int IPID { get; set; }
        public int OPID { get; set; }
        public int BankID { get; set; }
        public string PatientType { get; set; }
        public string BankName { get; set; }
        public string DoctorName { get; set; }
        public string WarehouseName { get; set; }
        public int WarehouseID { get; set; }
        public int DoctorID { get; set; }
        public string Form { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Reportlogopath { get; set; }
        public bool IsDirectSalesInvoice { get; set; }
        public DateTime? CustomerPODate { get; set; }
        public string DONO { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string VatRegNo { get; set; }
        public string MinimumCurrency { get; set; }
        public string AadhaarNo { get; set; }
        public string AmountInWords { get; set; }
        public int VATPercentageID { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal OtherChargesVATAmount { get; set; }
        public int DecimalPlaces { get; set; }

    }

    public class SalesInvoiceItemBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int SalesOrderID { get; set; }

        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Qty { get; set; }
        public decimal OfferQty { get; set; }
        public decimal MRP { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryQty { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public decimal SecondaryMRP { get; set; }
        public decimal DiscPercentage { get; set; }
        public decimal Amount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TradeDiscPercentage { get; set; }
        public decimal TradeDiscAmount { get; set; }
        public decimal TurnoverDisc { get; set; }
        public decimal CashDiscount { get; set; }
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public int InvoiceID { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal SGSTPercent { get; set; }
        public string InvoiceNo { get; set; }
        public int InvoiceTransID { get; set; }
        public decimal Stock { get; set; }
        public int WarehoueID { get; set; }
        public int BatchTypeID { get; set; }
        public int SalesUnitID { get; set; }
        public string SalesUnitName { get; set; }
        public int CounterSalesTransUnitID { get; set; }
        public decimal LoosePrice { get; set; }
        public decimal FullPrice { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public decimal ConvertedOfferQuantity { get; set; }

        public decimal SGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal PackSize { get; set; }
        public decimal VATAmount { get; set; }
        public decimal VATPercentage { get; set; }

    }
    public class SalesAmountBO
    {
        public string Particulars { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxableAmount { get; set; }
    }

    public class SalesPackingDetailsBO
    {
        public decimal Quantity { get; set; }
        public string PackSize { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
    }

    public class SalesByLocationBO
    {
        public string LocationCode { get; set; }
        public string LocationType { get; set; }
        public string LocationName { get; set; }
        public decimal Amount { get; set; }
        public decimal PreviousMonthAmount { get; set; }
        public decimal Budget { get; set; }
    }

}