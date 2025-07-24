using System.Collections.Generic;
using System.Web.Mvc;
using BusinessObject;
using System;
using TradeSuiteApp.Web.Areas.Masters.Models;
using System.ComponentModel.DataAnnotations;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{


    public class GRNModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Supplier { get; set; }
        public string Date { get; set; }
        public string ReceiptStore { get; set; }

        //CREATE
        [Required]
        public string GRNNo { get; set; }
        [Required]
        public string GRNDate { get; set; }

        public List<SupplierBO> SupplierList { get; set; }
        [Required]
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public List<WareHouseBO> WarehoueList { get; set; }
        public int WarehouseID { get; set; }
        public string Warehouse { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public decimal LocalLandinngCost { get; set; }
        public int ShippingStateID { get; set; }
        public int StateID { get; set; }
        public int DiscountID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public List<DiscountCategoryModel> DiscountList { get; set; }
        public List<PurchaseOrderModel> UnProcesedPOList { get; set; }

        public string PONo { get; set; }
        public string PODate { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public string InvoiceDate { get; set; }
        public string RequestedBy { get; set; }
        public List<PurchaseOrderTransModel> UnProcesedPOTransList { get; set; }
        public string ItemName { get; set; }
        public string PurchaseOrderNo { get; set; }
        public List<GRNItemModel> grnItems { get; set; }
        public string Status { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string SupplierCode { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsPurchaseCompleted { get; set; }
        public bool IsBarCodeGenerator { get; set; }
        public bool IsDirectPurchaseInvoice { get; set; }  //set from configurtation
        public bool IsCheckedDirectInvoice { get; set; }  //set from checkbox

        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public int UnitID { get; set; }
        public List<QRCodeItemModel> QRCodeItems { get; set; }
        public List<UnitModel> UOMList { get; set; }
        public List<GSTCategoryModel> GSTPercentageList { get; set; }
        public SelectList BusinessCategoryList { get; set; }
        public int BusinessCategoryID { get; set; }

        public SelectList UnitList { get; set; }
        public string normalclass { get; set; }
        public int DecimalPlaces { get; set; }
        public int CurrencyID { get; set; }
        public int TaxTypeID { get; set; }
        public int IsVat { get; set; }
        public int IsGST { get; set; }

        public decimal VATAmount { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal PackingForwarding { get; set; }
        public decimal SuppOtherCharges { get; set; }
        public decimal SuppFreight { get; set; }
        public decimal LocalCustomsDuty { get; set; }
        public decimal LocalFreight { get; set; }
        public decimal LocalMiscCharge { get; set; }
        public decimal LocalOtherCharges { get; set; }
        public string Remarks { get; set; }
        public string BAddressLine1 { get; set; }
        public string BAddressLine2 { get; set; }
        public string BAddressLine3 { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string CountryName { get; set; }
        public string BinCode { get; set; }
    }

    public class GRNItemModel
    {
        public int ID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int? POTransID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string Model { get; set; }
        public string Remark { get; set; }
        public int CurrencyID { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public string Batch { get; set; }
        public string ExpiryDate { get; set; }
        public decimal PurchaseOrderQty { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal QualityCheckQty { get; set; }
        public decimal? AcceptedQty { get; set; }
        public decimal RejectedQty { get; set; }
        public int ItemOrderPreference { get; set; }
        public string Remarks { get; set; }
        public bool IsQCRequired { get; set; }
        public decimal PendingPOQty { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string Unit { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryRate { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryReceivedQty { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public decimal SecondaryPurchaseOrderQty { get; set; }
        public string BatchType { get; set; }
        public decimal QtyTolerance { get; set; }
        public decimal POQuantity { get; set; }
        public decimal QtyTolerancePercent { get; set; }
        public string ItemCategory { get; set; }
        public decimal AllowedQty { get; set; }
        public int UnitID { get; set; }

        public decimal LooseQty { get; set; }
        public decimal LooseRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal OfferQty { get; set; }
        public int DiscountID { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public int BatchID { get; set; }
        public decimal RetailMRP { get; set; }
        public decimal PackSize { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Stock { get; set; }
        public decimal PrintingQty { get; set; }
        public List<UnitModel> UOMList { get; set; }
        public List<DiscountCategoryModel> DiscountList { get; set; }
        public SelectList GSTPercentageList { get; set; }
        public string BinCode { get; set; }
}

    public class QRCodeItemModel
    {
        public int BatchID { get; set; }
        public int ItemID { get; set; }
        public string QRCode { get; set; }
    }
}