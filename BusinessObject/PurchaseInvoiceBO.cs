using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PurchaseInvoiceBO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string suppliercurrencyCode { get; set; }
        public string PItype { get; set; }
        public string Freight { get; set; }
        public string WayBillNo { get; set; }
        public string VatRegNo { get; set; }


        public string shipmentmode { get; set; }
        public Decimal SuuplierCurrencyconverion  { get; set; }
        public string PurchaseNo { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int SupplierID { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public string SupplierName { get; set; }
        public decimal SecondaryQty { get; set; }
        public string LocalSupplierName { get; set; }
        public bool IsDraft { get; set; }
        public string InvoiceNo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime GRNDate { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal PackingForwarding { get; set; }
        
        public decimal PackingCharges { get; set; }
        public decimal SupplierOtherCharges { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public int CurrencyID { get; set; }
        public int IsGST { get; set; }
        public int IsVAT { get; set; }
        public decimal LocalCustomsDuty { get; set; }
        public decimal LocalFreight { get; set; }
        public decimal LocalMiscCharge { get; set; }
        public decimal LocalOtherCharges { get; set; }
        public decimal LocalLandingCost { get; set; }
        public decimal TaxOnFreight { get; set; }
        public decimal TaxOnPackingCharges { get; set; }
        public decimal TaxOnOtherCharge { get; set; }
        public decimal IGST { get; set; }
        public decimal CGST { get; set; }
             public decimal SGST { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal LessTDS { get; set; }
        public decimal AmountPayable { get; set; }
        public decimal NetAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal LocationID { get; set; }
        public List<PurchaseInvoiceOtherChargeDetailBO> OtherChargeDetails { get; set; }
        public List<PurchaseInvoiceTaxDetailBO> TaxDetails { get; set; }
        public List<PurchaseInvoiceTransItemBO> InvoiceTransItems { get; set; }
        public List<SupplierBO> Suppliers { get; set; }
        public decimal TotalDifference { get; set; }
        public decimal TotalFreight { get; private set; }
        public decimal TDSOnFreight { get; set; }

        public string Status { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string SupplierCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string SupplierCategory { get; set; }
        public int? TDSID { get; set; }
        public string TDSCode { get; set; }
        public string Rate { get; set; }
        public string GSTNo { get; set; }
        public string SupplierLocation { get; set; }
        public bool IsCancelled { get; set; }
        public Nullable<int> SelectedQuotationID { get; set; }
        public string OtherQuotationIDS { get; set; }
        public string Remarks { get; set; }
        public string GrnNo { get; set; }
        public string InvoiceType { get; set; }
        public decimal OtherChargesVATAmount { get; set; }
    }

    public class PurchaseInvoiceOtherChargeDetailBO
    {
        public int Id { get; set; }
        public int PurchaseOrderID { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public string Particular { get; set; }
        public decimal POValue { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal DifferenceValue { get; set; }
        public string Remarks { get; set; }
        public string PurchaseOrderNumber { get; set; }



    }

    public class PurchaseInvoiceTaxDetailBO
    {
        public int Id { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public string Particular { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal POValue { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal DifferenceValue { get; set; }
        public string Remarks { get; set; }
        //public bool IsLocal { get; set; }
        //public decimal CGSTPercent { get; set; }
        //public decimal CGSTAmt { get; set; }
        //public decimal SGSTPercent { get; set; }
        //public decimal SGSTAmt { get; set; }
        //public decimal IGSTPercent { get; set; }
        //public decimal IGSTAmt { get; set; }
    }
    public class PurchaseInvoiceTransItemBO:GRNTransItemBO
    {
        public new string PrimaryUnit { get; set; }
        public new int PrimaryUnitID { get; set; }
        public new string PurchaseUnit { get; set; }
        public new int PurchaseUnitID { get; set; }
        public new decimal ConvertedStock { get; set; }
        public new decimal ConvertedQty { get; set; }
    }
}

