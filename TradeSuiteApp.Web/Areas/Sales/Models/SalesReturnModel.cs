using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class SalesReturnModel
    {
        public int ID { get; set; }
   
        public string SRNo { get; set; }
        public string SRDate { get; set; }
        public string DespatchDate { get; set; }
        public int CustomerID { get; set; }
        public int StateID { get; set; }
        public string Status { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategory { get; set; }
        public string CustomerName { get; set; }
        public int PaymentModeID { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsProcessed { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public decimal OfferReturnQty { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int SalesCategoryID { get; set; }
        public int PriceListID { get; set; }
        public int SchemeAllocationID { get; set; }
        public int SchemeID { get; set; }
        public int ItemCategoryID { get; set; }
        public int BatchTypeID {get;set;}
        public int UnitID { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public SelectList PaymentModeList { get; set; }
        public SelectList UnitList { get; set; }
        public List<SalesReturnItemModel> Items { get; set; }
        public List<SalesInvoiceModel> ReturnInvoiceList { get; set; }

        public int LocationStateID { get; set; }
        public SelectList SalesReturnLogicCode { get; set; }
        public int LogicCodeID { get; set; }
        public string LogicCode { get; set; }
        public string LogicName { get; set; }
        public int SalesInvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public bool IsNewInvoice { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class SalesReturnItemModel : ItemModel
    {
        public int SalesReturnItemID { get; set; }
        public decimal Qty { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryQty { get; set; }
        public decimal SecondaryMRP { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public decimal OfferQty { get; set; }
        public string FullOrLoose { get; set; }
        public decimal MRP { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal NetAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public string InvoiceTransNo { get; set; }
        public decimal SaleQty { get; set; }
        public string TransNo { get; set; }
        public int SalesInvoiceID { get; set; }
        public decimal OfferReturnQty { get; set; }
        public string Batch { get; set; }
        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public int SalesInvoiceTransID { get; set; }
       // public int SalesTransID { get; set; }
        public int PrimaryUnitID { get; set; }
        public int SalesTransUnitID { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public decimal SalesInvoiceQty { get; set; }
        public int LogicCodeID { get; set; }
        public string LogicCode { get; set; }
        public string LogicName { get; set; }
        public decimal ConvertedOfferQuantity { get; set; }
        public decimal SalesOfferQty { get; set; }

    }
}