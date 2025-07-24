using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObject;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class SRNViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "SRN Number is required")]
        public string SRNNumber { get; set; }
        [Required]
        public string Date { get; set; }
        public List<SupplierBO> SupplierList { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ReceiptDate { get; set; }
        public string DeliveryChallanNo { get; set; }
        public string DeliveryChallanDate { get; set; }
        public bool IsDraft { get; set; }
        public int CreatedUserID { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public string Status { get; set; }
        public SelectList DDLDepartment { get; set; }
        public SelectList DDLLocation { get; set; }
        public SelectList DDLEmployee { get; set; }
        public SelectList DDLInterCompany { get; set; }
        public SelectList DDLProject { get; set; }
        public string  ServicePODate { get; set; }
        public bool IsCancelled { get; set; }

        public List<PurchaseOrderViewModel> UnProcessedPOService { get; set; }
        public List<SRNtransViewModel> Trans { get; set; }
    }

    public class SRNtransViewModel
    {
        public int SRNID { get; set; }
        public int POServiceID { get; set; }
        public int? POServiceTransID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal PurchaseOrderQty { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal AcceptedQty { get; set; }
        public decimal PORate { get; set; }
        public int? ServiceLocationID { get; set; }
        public string ServiceLocation { get; set; }
        public int? DepartmentID { get; set; }
        public string Department { get; set; }
        public int? EmployeeID { get; set; }
        public string Employee { get; set; }
        public int? CompanyID { get; set; }
        public string Company { get; set; }
        public int? ProjectID { get; set; }
        public string Project { get; set; }
        public string Remarks { get; set; }
        public decimal AcceptedValue { get; set; }

        public string PurchaseOrderNo { get; set; }
        public string Unit { get; set; }

        //code below by prama on 12-6-18
        public string TravelFrom { get; set; }
        public string TravelTo { get; set; }
        public string TransportMode { get; set; }
        public string TravelingRemarks { get; set; }
        public DateTime? TravelDate { get; set; }
        public string TravelDateString { get; set; }
        public int CategoryID { get; set; }
        public decimal TolaranceQty { get; set; }
        public decimal? QtyTolerancePercent { get; set; }



    }
}