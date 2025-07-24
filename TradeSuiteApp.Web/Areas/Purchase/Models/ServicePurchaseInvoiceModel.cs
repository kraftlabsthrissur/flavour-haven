using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class ServicePurchaseInvoiceModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public List<SupplierBO> SupplierList { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string LocalSupplierName { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal Difference { get; set; }
        public decimal TDS { get; set; }
        public decimal? TDSOnAdvance { get; set; }
        public decimal NetTDS { get; set; }
        public decimal NetAmountPayable { get; set; }
        public string Status { get; set; }
        public List<SupplierBO> Suppliers { get; set; }

        public string PurchaseNo { get; set; }
        public string PurchaseDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal AcceptedAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal DifferenceAmount { get; set; }
        public decimal Discount { get; set; }
        public bool IsCanceled { get; set; }
        public int TDSID { get; set; }
        public int ServicePurchaseInvoiceID { get; set; }
        public string SrDate { get; set; }
        public decimal OtherDeductions { get; set; }
        public List<ServicePurchaseInvoiceTransItemBO> InvoiceTransItems { get; set; }
        public List<ServicePurchaseInvoiceTaxDetailsBO> TaxDetails { get; set; }

        public bool IsDraft { get; set; }
    }

    public class ServicePurchaseInvoiceItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal AcceptedQty { get; set; }
        public decimal UnMatchedQty { get; set; }
        public int SRNID { get; set; }
        public int SRNTransID { get; set; }
        public bool IsInclusiveGST { get; set; }
        public decimal PORate { get; set; }
        public decimal AcceptedValue { get; set; }
        public decimal ApprovedValue { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceRate { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal FreightAmt { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal PackingShippingCharge { get; set; }
        public decimal Difference { get; set; }
        public string Remarks { get; set; }
        public int ServiceLocationID { get; set; }
        public string LocationName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int POServiceID { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }

        //code below by prama on 12-6-18
        public string TravelFrom { get; set; }
        public string TravelTo { get; set; }
        public string TransportMode { get; set; }
        public string TravelingRemarks { get; set; }
        public Nullable<System.DateTime> TravelDate { get; set; }
        public string TravelDateString { get; set; }
        public int CategoryID { get; set; }
        public decimal TDSOnAdvance { get; set; }

    }

    public class ServicePurchaseInvoiceTaxDetailModel
    {
        public string Particulars { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal POValue { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal Difference { get; set; }
        public string Remarks { get; set; }
    }

    public class ServicePurchaseInvoiceTaxViewModel
    {
        public decimal CGSTPercent { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal IGSTAmt { get; set; }
        public int Count { get; set; }
    }

    public class SRNItemViewModel
    {
        public List<int> SrnIDList { get; set; }
        public List<ItemViewModelService> ItemList { get; set; }
    }
    public class ItemViewModelService
    {
        public int ItemID { get; set; }
        public int POServiceID { get; set; }

    }

}