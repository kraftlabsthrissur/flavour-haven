using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SRNTransItemBO
    {
        public int SRNID { get; set; }
        public int SRNTransID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal AcceptedQty { get; set; }
        public decimal ApprovedQty { get; set; }
        public decimal UnMatchedQty { get; set; }
        public decimal PORate { get; set; }
        public decimal AcceptedValue { get; set; }
        public decimal ApprovedValue { get; set; }
        public decimal? SGSTPercent { get; set; }
        public decimal? CGSTPercent { get; set; }
        public decimal? IGSTPercent { get; set; }
        public decimal? SGSTAmt { get; set; }
        public decimal? CGSTAmt { get; set; }
        public decimal? IGSTAmt { get; set; }
        public decimal? FreightAmt { get; set; }
        public decimal? OtherCharges { get; set; }
        public decimal? PackingShippingCharge { get; set; }
        public int PurchaseOrderID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public int POServiceID { get; set; }
        public string Batch { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
        public decimal PurchaseOrderQty { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal QualityCheckQty { get; set; }
        public decimal RejectedQty { get; set; }
        public int ItemOrderPreference { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceRate { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal Difference { get; set; }
        public string Remarks { get; set; }
        public int Id { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public bool IsQCRequired { get; set; }
        public decimal ReturnQty { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal LastPurchaseRate { get; set; }
        public decimal PendingPOQty { get; set; }
        public decimal QtyTolerancePercent { get; set; }
        public decimal PurchaseAmount { get; set; }
        
        public int ServiceLocationID { get; set; }
        public int DepartmentID { get; set; }
        public int EmployeeID { get; set; }
        public int CompanyID { get; set; }
        public int ProjectID { get; set; }

        public decimal InvoiceAmount { get; set; }
        public string TDSCode { get; set; }
        public decimal TDSAmount { get; set; }
        public bool IsInclusiveGST { get; set; }

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
}
