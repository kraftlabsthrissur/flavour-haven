using System.Collections.Generic;
using System.Web.Mvc;
using BusinessObject;
using System;
using System.ComponentModel.DataAnnotations;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{


    public class PurchaseRequisitionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Purchase requisition number is required")]
        public string Code { get; set; }

        public string Name { get; set; }
        [Required(ErrorMessage = "Purchase requisition date is required")]

        public string Date { get; set; }
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public int SalesInquiryID { get; set; }
        public string ItemID { get; set; }
        public string FromDepartmentName { get; set; }
        public string ToDepartmentName { get; set; }
        public int PRID { get; set; }

        //Create
        public string PurchaseRequisitionNumber { get; set; }
        public string PrDate { get; set; }
        public SelectList DDLDepartment { get; set; }
        public int DepartmentFrom { get; set; }
        public int DepartmentTo { get; set; }
        public SelectList DDLItemCategory { get; set; }
        public SelectList DDLPurchaseCategory { get; set; }
        public int ItemCategoryID { get; set; }
        public int UnitID { get; set; }
        public List<PurchaseRequisitionItem> Item { get; set; }
        public SelectList UnitList { get; set; }
        public string Status  { get; set; }
        public bool IsDraft { get; set; }
        public string PurchaseRequisitedCustomer { get; set; }
        public string RequisitedCustomerAddress { get; set; }
        public string RequisitedPhoneNumber1 { get; set; }
        public string RequisitedPhoneNumber2 { get; set; }
        public string Remarks { get; set; }
        public string normalclass { get; set; }
        public bool FullyOrdered { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
    }

    public class DDLDepartment
    {
        public long Id { get; set; }
        public long Name { get; set; }

    }

    public class PurchaseRequisitionItem
    {
        public int ItemID { get; set; }
        public int SalesInquiryItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public decimal? Stock { get; set; }
        public decimal? Qty { get; set; }
        public decimal? MRP { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public string Remarks { get; set; }
        public string ExpectedDate { get; set; }
        public int ItemTypeID { get; set; }
        public int ItemCategoryID { get; set; }
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
}