using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SalesGoodsReceiptItemBO
    {
        public int ID { get; set; }
        public int GoodReceiptNoteID { get; set; }
        public string TransNo { get; set; }
        public int SalesOrderID { get; set; }
        public int SalesOrderItemTransID { get; set; }
        public int CounterSalesID { get; set; }
        public int CounterSalesItemTransID { get; set; }
        public int SalesInvoiceID { get; set; }
        public int SalesInvoiceTransID { get; set; }
        public int ItemID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int UnitID { get; set; }
        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchName { get; set; }
        public string BatchTypeName { get; set; }
        public decimal Stock { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal VATAmount { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal Rate { get; set; }
        public decimal OfferReturnQty { get; set; }
        public int ItemCategoryID { get; set; }
        public int SalesOrderItemID { get; set; }
        public string SalesOrderNo { get; set; }
        public int SalesUnitID { get; set; }
        public decimal LooseRate { get; set; }
        public string FullOrLoose { get; set; }
        public decimal MRP { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceOfferQty { get; set; }
        public decimal Qty { get; set; }
        public decimal OfferQty { get; set; }
        public decimal QtyMet { get; set; }
        public decimal OfferQtyMet { get; set; }
        public bool InvoiceQtyMet { get; set; }
        public bool InvoiceOfferQtyMet { get; set; }

        public decimal FreightAmount { get; set; }
        public decimal ActualOfferQty { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal Amount { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal CessAmount { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal NetAmount { get; set; }
        public int StoreID { get; set; }
        public decimal SaleQty { get; set; }
        public int SalesTransID { get; set; }
        public int PrimaryUnitID { get; set; }
        public decimal LoosePrice { get; set; }
        public decimal FullPrice { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public decimal SalesInvoiceQty { get; set; }
        public int SalesTransUnitID { get; set; }
        public string SalesUnit { get; set; }
        public string PrimaryUnit { get; set; }
        public int LogicCodeID { get; set; }
        public string LogicCode { get; set; }
        public string LogicName { get; set; }
        public int POID { get; set; }
        public int POTransID { get; set; }
        public decimal PORate { get; set; }
        public decimal POQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal PackSize { get; set; }
        public string Category { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryMRP { get; set; }
        public decimal SecondaryQty { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public decimal ConvertedOfferQuantity { get; set; }
        public decimal SalesOfferQty { get; set; }
        public string MalayalamName { get; set; }
        public string Remarks { get; set; }
        public string Model { get; set; }
        public int MaxSalesQty { get; set; }
        public int MinSalesQty { get; set; }

        public int BillableID { get; set; }
        public decimal GSTAmount { get; set; }

        public string PatientCode { get; set; }
        public string PatientName { get; set; }

        public string Form { get; set; }
        public int MedicineIssueID { get; set; }
        public int MedicineIssueTransID { get; set; }
        public int CurrencyID { get; set; }
        public string PartsNumber { get; set; }
        public string CurrencyName { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public bool PrintWithItemName { get; set; }
        public string SalesInvoiceNo { get; set; }
        public string CounterSalesNo { get; set; }
        public string Itemcode {  get; set; }
        public int Quantity { get; set; }
        public string Make { get; set; }
        public DateTime? SONO { get; set; }
        public DateTime? OrderDate { get; set; }
        // public DateTime? FullOrLoose { get; set; }
        public string Location { get; set; }




    }
}
