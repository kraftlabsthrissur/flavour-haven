using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using System.Collections.Generic;
namespace TradeSuiteApp.Web.Areas.Purchase.Models
{


    public class PurchaseOrderModel
    {
        public int ID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string RequestedBy { get; set; }
        public string SupplierName { get; set; }
        public decimal NetAmt { get; set; }
        public string ItemName { get; set; }
        public string Status { get; set; }
        public string ItemCatagory { get; set; }
    }
    public class PurchaseOrderTransModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public string PartsNumber { get; set; }
        public string Model { get; set; }
        public string Remark { get; set; }
        public string CurrencyName { get; set; }
        public string HSNCode { get; set; }
        public int PurchaseOrderID { get; set; }
        public int? PurchaseReqID { get; set; }
        //public int? PurServiceTransID { get; set; }
        public int? PRTransID { get; set; }
        public int ItemID { get; set; }
        public int CurrencyID { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }

        public decimal? Quantity { get; set; }
        public decimal? SecondaryQuantity { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public string SecondaryUnit { get; set; }
        public string Category { get; set; }
        public decimal Rate { get; set; }
        public decimal SecondaryRate { get; set; }
        public decimal MRP { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal Discount { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? LastPurchaseRate { get; set; }
        public decimal? LowestPR { get; set; }
        public bool? Purchased { get; set; }
        public decimal? QtyMet { get; set; }
        public decimal? QtyInQC { get; set; }
        public decimal? QtyAvailable { get; set; }
        public decimal? QtyOrdered { get; set; }
        public string Remarks { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public decimal QtyTolerancePercent { get; set; }
        public bool IsQCRequired { get; set; }

        // public int? ServiceLocationID { get; set; }
        //  public int? EmployeeID { get; set; }
        //  public int? DepartmentID { get; set; }
        // public int? CompanyID { get; set; }
        // public int? ProjectID { get; set; }

        public string ServiceLocation { get; set; }
        public string Employee { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string Project { get; set; }

        public decimal? GSTPercentage { get; set; }

        // ADDITIONAL 
        public string PurchaseOrderNo { get; set; }
        public string PurchaseRequisitionNo { get; set; }
        public decimal PendingPOQty { get; set; }
        public decimal PendingPOSecondaryQty { get; set; }
        public int ItemCategoryID { get; set; }

        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public int FGCategoryID { get; set; }

        // public string TravelFrom { get; set; }
        // public string TravelTo { get; set; }
        // public string TransportMode { get; set; }
        // public string TravelingRemarks { get; set; }
        //////  public int? TravelToID { get; set; }
        //  public int? TravelFromID { get; set; }
        //  public int? TransportModeID { get; set; }
        public string TravelDate { get; set; }
        // public int TravelCategoryID { get; set; }
        //  public string TravelDateString { get; set; }
        public bool IsSuspended { get; set; }

        public string BatchNo { get; set; }
        public string ExpDate { get; set; }
        public List<UnitModel> UOMList { get; set; }
        public decimal PackSize { get; set; }
        public decimal DiscountID { get; set; }
        public List<DiscountCategoryModel> DiscountList { get; set; }
        public List<GSTCategoryModel> GSTPercentageList { get; set; }
        public string Batch { get; set; }
        public int BatchID { get; set; }
        public string normalclass { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal SuppOtherCharge { get; set; }
        public string BinCode { get; set; }
        public int BinID { get; set; }
    }

}