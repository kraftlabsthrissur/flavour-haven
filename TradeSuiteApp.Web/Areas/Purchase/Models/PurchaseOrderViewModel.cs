using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class PurchaseOrderViewModel
    {
        public int ID { get; set; }
        [Required]
        public string PurchaseOrderNo { get; set; }
        [Required]
        public string PurchaseOrderDate { get; set; }
        public int SupplierID { get; set; }
        public Nullable<int> AdvancePercentage { get; set; }
        public Nullable<decimal> AdvanceAmount { get; set; }
        public Nullable<int> PaymentModeID { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<int> ShippingAddressID { get; set; }
        public Nullable<int> ShippingStateID { get; set; }
        public Nullable<int> BillingAddressID { get; set; }
        public bool InclusiveGST { get; set; }
        public bool GstExtra { get; set; }
        public Nullable<int> SelectedQuotationID { get; set; }
        public string OtherQuotationIDS { get; set; }
        public Nullable<int> PurchaseRequisitionID { get; set; }
        public Nullable<int> DeliveryWithinID { get; set; }
        //public int DeliveryWithin { get; set; }
        public string DeliveryWithin { get; set; }
        public int PaymentWithinID { get; set; }
        public int PaymentWithin { get; set; }
        public Nullable<decimal> SGSTAmt { get; set; }
        public Nullable<decimal> CGSTAmt { get; set; }
        public Nullable<decimal> IGSTAmt { get; set; }
        public Nullable<decimal> FreightAmt { get; set; }
        public Nullable<decimal> OtherCharges { get; set; }
        public Nullable<decimal> PackingShippingCharge { get; set; }
        public decimal NetAmt { get; set; }
        public bool OrderMet { get; set; }
        public bool IsDraft { get; set; }
        public string Remarks { get; set; }
        public bool Cancelled { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public int CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }

        public int DaysToSupply { get; set; }
        public string ShipplingLocation { get; set; }
        public string SupplierName { get; set; }
        public string BillingLocation { get; set; }
        public string RequestedBy { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }


        //public List<PurchaseOrderTransBO> items { get; set; }

        public SelectList DDLWithinList { get; set; }
        public SelectList DDLBillTo { get; set; }
        public SelectList DDLShipTo { get; set; }
        public SelectList DDLPaymentMode { get; set; }
        public SelectList DDLItemCategory { get; set; }
        public SelectList DDLPurchaseCategory { get; set; }
        //public List<SupplierBO> SupplierList { get; set; }
        //public List<RequisitionBO> UnProcessedPrList { get; set; }

        //public List<FileBO> SelectedQuotation { get; set; }
        //public List<FileBO> OtherQuotations { get; set; }

        //Additional
        public int StateId { get; set; }
        public bool IsGSTRegistred { get; set; }
        public string SupplierLocation { get; set; }
        public string SupplierReferenceNo { get; set; }
        public string TermsOfPrice { get; set; }
    }
}