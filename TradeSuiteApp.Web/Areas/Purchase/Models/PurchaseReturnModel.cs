//File created by prama on 23-4-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessObject;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class PurchaseReturnModel
    {
        public int ID { get; set; }
        public string ReturnNo { get; set; }
        public string ReturnDate { get; set; }
        public decimal Freight { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal PackingCharges { get; set; }
        public decimal NetAmount { get; set; }
        public string GRNno { get; set; }
        public int GRNnoID { get; set; }
        public int ItemID { get; set; }
        public string PremisesName { get; set; }
        public int PremisesID { get; set; }
        public decimal ReturnQty { get; set; }
        public int SupplierID { get; set; }
        public string ItemName { get; set; }
        public bool IsDraft { get; set; }
        public string SupplierName { get; set; }
        public decimal AcceptedQty { get; set; }
        public decimal Rate { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal PackingForwarding { get; set; }
        public decimal SupplierOtherCharges { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal LocalCustomsDuty { get; set; }
        public decimal LocalFreight { get; set; }
        public decimal LocalMiscCharge { get; set; }
        public decimal LocalOtherCharges { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Discount { get; set; }
        public bool IsProcessed { get; set; }
        public string Status { get; set; }
        public List<GRNTransItemModel> Items { get; set; }
        public List<SupplierModel> SupplierList { get; set; }
        public List<PurchaseReturnTransModel> ReturnItems { get; set; }
        public SelectList UOMList { get; set; }
        public int ShippingStateID { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistred { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int TaxTypeID { get; set; }
        public int CurrencyID { get; set; }
        public int DecimalPlaces { get; set; }
        public string normalclass { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public string CurrencyCode { get; set; }

        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
    }


    public class PurchaseReturnTransModel
    {
        public int PurchaseReturnID { get; set; }
        public int GRNID { get; set; }
        public int ItemID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public int BatchTypeID { get; set; }
        public int UnitID { get; set; }
        public int PurchaseReturnTransID { get; set; }
        public string ReturnNo { get; set; }
        public decimal Stock { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal AcceptedQty { get; set; }
        public int InvoiceID { get; set; }
        public int WarehouseID { get; set; }
        public int PurchaseReturnOrderTransID { get; set; }
        public int PurchaseReturnOrderID { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal GRNQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryUnit { get; set; }
        public decimal SecondaryReturnQty { get; set; }
        public decimal SecondaryRate { get; set; }
        
    }
}


