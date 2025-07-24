using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace BusinessObject
{
    public class ServicePurchaseInvoiceBO
    {
        public int ServicePurchaseInvoiceID { get; set; }
        public string PurchaseNo { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int SupplierID { get; set; }
        public string LocalSupplierName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal DifferenceAmount { get; set; }
        public decimal AcceptedAmount { get; set; }
        public decimal TDS { get; set; }
        public decimal? TDSOnAdvance { get; set; }
        public decimal NetTDS { get; set; }      
        public decimal OtherDeductions { get; set; }
        public decimal NetAmountPayable { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime CancelledDate { get; set; }
        public int CreatedUserID { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }       
        public string GstNumber { get; set; }
        public List<ServicePurchaseInvoiceTransItemBO> InvoiceTransItems { get; set; }
        public List<ServicePurchaseInvoiceTaxDetailsBO> TaxDetails { get; set; }
        public string SupplierName { get; set; }
        public List<SupplierBO> SupplierList { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<GSTBO> GstList { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string Status { get; set; }
        public bool IsGSTRegistered { get; set; }
        public string Rate { get; set; }
        public string TDSCode { get; set; }
        public int TDSID { get; set; }
        public SelectList TDSCodeList { get; set; }
        public int ShippingStateID { get; set; }
        public int StateID { get; set; }
        public string GSTNo { get; set; }
        public string SupplierLocation { get; set; }
        public string SrDate { get; set; }
      
    }
    public class ServicePurchaseInvoiceTransItemBO
    {
        public int PurchaseInvoiceID { get; set; }
        public int SRNID { get; set; }
        public int SRNTransID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal AcceptedQty { get; set; }
        public decimal ApprovedValue { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal AcceptedValue { get; set; }
        public decimal UnMatchedQty { get; set; }
        public decimal PORate { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceRate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal Difference { get; set; }
        public string TDSCode { get; set; }
        public decimal TDSAmount { get; set; }
        public string Remarks { get; set; }
        public int ServiceLocationID { get; set; }
        public int DepartmentID { get; set; }
        public int EmployeeID { get; set; }
        public int? CompanyID { get; set; }
        public int? ProjectID { get; set; }
        public bool IsInclusiveGST { get; set; }
        public int POServiceID { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal InvoiceGSTPercent { get; set; }
        public string TravelFrom { get; set; }
        public string TravelTo { get; set; }
        public string TransportMode { get; set; }
        public string TravelingRemarks { get; set; }
        public int CategoryID { get; set; }
        public Nullable<System.DateTime> TravelDate { get; set; }
        public string TravelDateString { get; set; }     
        public bool InclusiveGST { get; set; }
        public int? GSTPercent { get { return Convert.ToInt16(SGSTPercent + CGSTPercent + IGSTPercent); } set { } }


    }

    public class ServicePurchaseInvoiceTaxDetailsBO
    {
        public int PurchaseInvoiceID { get; set; }
        public string Particular { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal POValue { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal DifferenceValue { get; set; }
        public string Remarks { get; set; }
    }

    public class PurchaseOrderTransBOService
    {
        public int ID { get; set; }
        public int POServiceID { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal NetAmount { get; set; }
        public int ServiceLocationID { get; set; }
        public int DepartmentID { get; set; }
        public int EmployeeID { get; set; }
        public int CompanyID { get; set; }
        public int ProjectID { get; set; }
        public bool IsPurchased { get; set; }
        public decimal QtyMet { get; set; }
        public string Remarks { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public string ServiceLocation { get; set; }
        public string Department { get; set; }
        public string Employee { get; set; }
        public string InterCompany { get; set; }
        public string Project { get; set; }
        public int Count { get; set; }

        //Edit
        public int PurchaseInvoiceID { get; set; }
        public string Particular { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal POAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal DifferenceAmount { get; set; }
    }
}
