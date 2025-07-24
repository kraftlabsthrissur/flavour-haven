using System.Collections.Generic;
using System.Web.Mvc;
using BusinessObject;
using System;
using System.ComponentModel.DataAnnotations;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class ServicePRViewModel
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string ItemCatagory { get; set; }
        public List<SupplierBO> SupplierList { get; set; }
        [Required(ErrorMessage = "Purchase Requisition Number is required")]
        public string PurchaseRequisitionNumber { get; set; }

        [Required(ErrorMessage = "Please select dateto proceed.")]
        public string PrDate { get; set; }
        public SelectList DDLItemCategory { get; set; }
        public SelectList DDLPurchaseCategory { get; set; }
        public List<ServicePurchaseRequisitionItem> Item { get; set; }

        public SelectList DDLDepartment { get; set; }
        public SelectList DDLLocation { get; set; }
        public SelectList DDLEmployee { get; set; }
        public SelectList DDLInterCompany { get; set; }
        public SelectList DDLProject { get; set; }

        public List<ServicePRTrans> PrTrans { get; set; }
        public string Status { get; set; }
        public bool IsBranchLocation { get; set; }
        public int PRID { get; set; }
        public string Code { get; set; }
        public string PrNo { get; set; }
        public string Remarks { get; set; }
        public string ToDepartmentName { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int FromDeptID { get; set; }
        public string POSNumber { get; set; }

        public SelectList TravelFromList { get; set; }
        public SelectList TravelToList { get; set; }
        public SelectList TransportModeList { get; set; }
        public bool IsDraft { get; set; }

    }

    public class ServicePurchaseRequisitionItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public decimal? Stock { get; set; }
        public decimal? QtyUnderQC { get; set; }
        public decimal? ReqQty { get; set; }
        public decimal? QtyOrdered { get; set; }

        //
        public string Remarks { get; set; }
        public string ExpectedDate { get; set; }
        public int ItemTypeID { get; set; }
        public int applicationID { get; set; }
        public int LocationID { get; set; }
        public int FinYear { get; set; }
        public string RequiredStatus { get; set; }

        //GoodsReceiptNoteTrans

        public string PurchaseOrderNo { get; set; }
        public double PendingPOQty { get; set; }
        public double ReceivedQty { get; set; }
        public double AcceptedQty { get; set; }
        public string BatchNo { get; set; }
        public string ExpiryDate { get; set; }


    }

    public class ServicePRTrans
    {
        public int ID { get; set; }

        public decimal ReqQuantity { get; set; }
        public string ExpDate { get; set; }
        public int? DepartmentID { get; set; }
        public int? LocationID { get; set; }
        public int? EmployeeID { get; set; }
        public int? InterCompanyID { get; set; }
        public int? ProjectID { get; set; }
        public string Remark { get; set; }

        public string ItemName { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Employee { get; set; }
        public string InterCompany { get; set; }
        public string Project { get; set; }
        public string Unit { get; set; }
        public int? TravelFromID { get; set; }
        public int? TravelToID { get; set; }
        public int? TransportModeID { get; set; }
        public string TravelFrom { get; set; }
        public string TravelTo { get; set; }
        public string TransportMode { get; set; }
        public string TravelingRemarks { get; set; }
        public string TravelDate { get; set; }
        public int CategoryID { get; set; }
        public int TravelCategoryID { get; set; }
    }
}