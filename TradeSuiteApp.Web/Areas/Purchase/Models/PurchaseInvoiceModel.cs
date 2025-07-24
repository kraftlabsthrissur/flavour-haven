using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;
using static iTextSharp.text.pdf.AcroFields;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{

    /// <summary>
    /// PurchaseInvoice Create or Edit
    /// </summary>
    public class PurchaseInvoiceModel
    {
        public int Id { get; set; }
        public string TransNo { get; set; }     //Trans No.
        public string TransDate { get; set; }
        public string GRNNo { get; set; }
        public string PurchaseOrderDate { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string LocalSupplierName { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal TotalInvoiceValue { get; set; }
        public decimal TotalDifference { get; set; }
        public decimal TotalFreight { get; set; }
        public decimal PackingForwarding { get; set; }
        public decimal LocalCustomsDuty { get; set; }
        public decimal LocalFreight { get; set; }
        public decimal LocalMiscCharge { get; set; }
        public decimal LocalOtherCharges { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal SupplierOtherCharges { get; set; }
        public decimal TDSOnFreight { get; set; }
        public decimal LessTDS { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal AmountPayable { get; set; }
        public decimal LocalLandingCost { get; set; }
        public decimal NetAmount { get; set; }
        public string PurchaseNo { get; set; }
        public string PurchaseDate { get; set; }
        public decimal SuppDocAmount { get; set; }

        public decimal SuppShipAmount { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public string normalclass { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int DecimalPlaces { get; set; }
        public int CurrencyID { get; set; }
        public int TaxTypeID { get; set; }
        public int GSTID { get; set; }
        public decimal GST { get; set; }
        public bool IsDraft { get; set; }
        public string SupplierCode { get; set; }
        public string TDSCode { get; set; }
        public int? TDSID { get; set; }
        public List<SupplierBO> Suppliers { get; set; }
        public List<GRNTransItemBO> Items { get; set; }
        public List<PurchaseInvoiceOtherChargeDetailBO> OtherChargeDetails { get; set; }
        public List<PurchaseInvoiceTaxDetailBO> TaxDetails { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Particular { get; set; }
        public string Rate { get; set; }
        public List<GSTBO> GstList { get; set; }
        public SelectList TDSCodeList { get; set; }
        public SelectList GSTPercentageList { get; set; }
        public int ShippingStateID { get; set; }
        public string GSTNo { get; set; }
        public string SupplierLocation { get; set; }
        public string SupplierCategory { get; set; }
        public bool IsCancelled { get; set; }
        public string Remarks { get; set; }
        public decimal SGSTAmt { get; set; }

        public decimal IGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public List<FileBO> SelectedQuotation { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal RoundOff { get; set; }

        public decimal DiscountAmt { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public SelectList BusinessCategoryList { get; set; }
        public int BusinessCategoryID { get; set; }
        public string PItype { get; set; }
        public string shipmentmode { get; set; }
        public string Freight { get; set; }
        public string WayBillNo { get; set; }
        public string InvoiceType { get; set; }
        public SelectList InvoiceTypeList { get; set; }
        public string suppliercurrencyCode { get; set; }
        public decimal OtherChargesVATAmount { get; set; }
    }

    public class GRNItemViewModel
    {
        public List<int> GrnIDList { get; set; }
        public List<ItemViewModel> ItemList { get; set; }
    }
    public class ItemViewModel
    {
        public int ItemID { get; set; }
        public int POID { get; set; }

        #region Purchase Invoice Save
        public int GRNID { get; set; }
        public int GRNTransID { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal InvoiceRate { get; set; }
        public decimal SecondaryRate { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal SecondaryInvoiceQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal AcceptedQty { get; set; }
        public decimal ApprovedQty { get; set; }
        public decimal ApprovedValue { get; set; }
        public decimal PORate { get; set; }
        public decimal Difference { get; set; }
        public string Remarks { get; set; }
        public decimal UnMatchedQty { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal InvoiceGSTPercent { get; set; }
        public int MilkPurchaseID { get; set; }
        public int UnitID { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal OfferQty { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public int BatchID { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string Remark { get; set; }
        public string Model { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int CurrencyID { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        #endregion
    }

    public class PurchaseInvoiceTaxViewModel
    {
        public decimal CGSTPercent { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal IGSTAmt { get; set; }
        public int Count { get; set; }
        public PurchaseOrderTransBO PurchaseOrderTransBO { get; set; }
    }

    public class PurchaseOrderOtherDeductionViewModel
    {
        public int PurchaseOrderID { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public decimal Freight { get; set; }
        public decimal OtherCharge { get; set; }
        public decimal PackingCharge { get; set; }
    }
    /// <summary>
    /// Purchase Invoice Save
    /// </summary>
    public class PurchaseInvoiceSaveViewModel
    {
        public int ID { get; set; }
        public string PurchaseNo { get; set; }
        public string PurchaseDateStr { get; set; }
        public Nullable<int> SelectedQuotationID { get; set; }
        public string Remarks { get; set; }
        public DateTime PurchaseDate
        {
            get
            {
                if (!string.IsNullOrEmpty(PurchaseDateStr))
                {
                    var purchaseDate = DateTime.ParseExact(PurchaseDateStr, "dd-mm-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    return purchaseDate;
                }
                else
                    return new DateTime();
            }
        }
        public int SupplierID { get; set; }
        public string LocalSupplierName { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int CurrencyID { get; set; }
        public bool IsDraft { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDateStr { get; set; }  ////dd-mm-yyyy
        public DateTime InvoiceDate
        {
            get
            {
                if (!string.IsNullOrEmpty(InvoiceDateStr))
                {
                    var invoiceDate = DateTime.ParseExact(InvoiceDateStr, "dd-mm-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    return invoiceDate;
                }
                else
                    return new DateTime();
            }
        }
        public decimal GrossAmount { get; set; }
        public decimal SGSTAmount
        {
            get
            {
                if (TaxDetails != null && TaxDetails.Count > 0)
                {
                    return TaxDetails.Where(x => x.Particular.Equals(TradeSuiteApp.Web.App_LocalResources.Purchase.SGST)).Sum(x => x.POValue);
                }
                else
                    return 0;
            }
        }
        public decimal CGSTAmount
        {
            get
            {
                if (TaxDetails != null && TaxDetails.Count > 0)
                {
                    return TaxDetails.Where(x => x.Particular.Equals(TradeSuiteApp.Web.App_LocalResources.Purchase.CGST)).Sum(x => x.POValue);
                }
                else
                    return 0;
            }
        }
        public decimal IGSTAmount
        {
            get
            {
                if (TaxDetails != null && TaxDetails.Count > 0)
                {
                    return TaxDetails.Where(x => x.Particular.Equals(TradeSuiteApp.Web.App_LocalResources.Purchase.IGST)).Sum(x => x.POValue);
                }
                else
                    return 0;
            }
        }
        public decimal Discount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal SupplierOtherCharges { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal PackingForwarding { get; set; }
        public decimal LocalCustomsDuty { get; set; }
        public decimal LocalFreight { get; set; }
        public decimal LocalMiscCharge { get; set; }
        public decimal LocalOtherCharges { get; set; }

        public decimal LocalLandingCost { get; set; }
        public decimal CurrencyExchangeRate { get; set; }

        //public decimal PackingCharges
        //{
        //    get
        //    {
        //        if (OtherChargeDetails != null && OtherChargeDetails.Count > 0)
        //        {
        //            return OtherChargeDetails.Where(x => x.Particular.Equals(TradeSuiteApp.Web.App_LocalResources.Purchase.PackingCharges)).Sum(x => x.POValue);
        //        }
        //        else
        //            return 0;
        //    }
        //}
        //public decimal OtherCharges
        //{
        //    get
        //    {
        //        if (OtherChargeDetails != null && OtherChargeDetails.Count > 0)
        //        {
        //            return OtherChargeDetails.Where(x => x.Particular.Equals(TradeSuiteApp.Web.App_LocalResources.Purchase.OtherDeductions)).Sum(x => x.POValue);
        //        }
        //        else
        //            return 0;
        //    }
        //}
        public decimal TaxOnFreight { get; set; }
        public decimal TaxOnPackingCharges { get; set; }
        public decimal TaxOnOtherCharge { get; set; }
        public decimal TotalDifference { get; set; }
        public decimal TDSOnFreightPercentage { get; set; }
        public decimal LessTDS { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal AmountPayable { get; set; }
        public decimal NetAmount { get; set; }
        public decimal LocationID { get; set; }
        public decimal InvoiceTotal { get; set; }

        public List<OtherChargeDetailSaveViewModel> OtherChargeDetails { get; set; }
        public List<TaxDetailSaveViewModel> TaxDetails { get; set; }
        public List<ItemViewModel> InvoiceTransItems { get; set; }
        //code below by prama on 5-6-18
        public string Status { get; set; }
        public int TDSID { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public string GrnNo { get; set; }
        public string Freight { get; set; }
        public string WayBillNo { get; set; }
        public string InvoiceType { get; set; }
        public decimal OtherChargesVATAmount { get; set; }
        public decimal LandingCost
        {
            get
            {
                return ((SuppDocAmount + FreightAmount + SupplierOtherCharges + SuppShipAmount + PackingForwarding) / (CurrencyExchangeRate > 0 ? CurrencyExchangeRate : 1)) + LocalLandingCost;

            }
        }
        //                   

    }
    public class OtherChargeDetailSaveViewModel
    {
        public int PurchaseOrderID { get; set; }
        public string Particular { get; set; }
        public decimal POValue { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal DifferenceValue { get { return InvoiceValue - POValue; } }
        public string Remarks { get; set; }
    }
    public class TaxDetailSaveViewModel
    {
        public string Particular { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal POValue { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal DifferenceValue { get { return InvoiceValue - POValue; } }
        public string Remarks { get; set; }
    }

    public static partial class Mapper
    {
        public static PurchaseInvoiceBO MapToBo(this PurchaseInvoiceSaveViewModel model)
        {
            return new PurchaseInvoiceBO()
            {
                Id = model.ID,
                PurchaseNo = model.PurchaseNo,
                PurchaseDate = General.ToDateTime(model.PurchaseDateStr),
                SupplierID = model.SupplierID,
                LocalSupplierName = model.LocalSupplierName,
                IsDraft = model.IsDraft,
                InvoiceNo = model.InvoiceNo,
                InvoiceDate = General.ToDateTime(model.InvoiceDateStr),
                SGSTAmount = model.SGSTAmount,
                CGSTAmount = model.CGSTAmount,
                IGSTAmount = model.IGSTAmount,
                IsGST = model.IsGST,
                IsVAT = model.IsVat,
                CurrencyID = model.CurrencyID,
                IGST = model.IGST,
                CGST = model.CGST,
                SGST = model.SGST,
                GrossAmount = model.GrossAmount,
                Discount = model.Discount,
                VATPercentage = model.VATPercentage,
                VATAmount = model.VATAmount,
                FreightAmount = model.FreightAmount,
                //PackingCharges = model.PackingCharges,
                PackingForwarding = model.PackingForwarding,
                SuppDocAmount = model.SuppDocAmount,
                SuppShipAmount = model.SuppShipAmount,
                SupplierOtherCharges = model.SupplierOtherCharges,
                LocalCustomsDuty = model.LocalCustomsDuty,
                LocalFreight = model.LocalFreight,
                LocalMiscCharge = model.LocalMiscCharge,
                LocalOtherCharges = model.LocalOtherCharges,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                TaxOnFreight = model.TaxOnFreight,
                TaxOnPackingCharges = model.TaxOnPackingCharges,
                TaxOnOtherCharge = model.TaxOnOtherCharge,
                TotalDifference = model.TotalDifference,
                TDSOnFreight = model.TDSOnFreightPercentage,
                LessTDS = model.LessTDS,
                OtherDeductions = model.OtherDeductions,
                AmountPayable = model.AmountPayable,
                NetAmount = model.NetAmount,
                InvoiceTotal = model.InvoiceTotal,
                LocationID = model.LocationID,
                Status = model.Status,
                TDSID = model.TDSID,
                SelectedQuotationID = model.SelectedQuotationID,
                Remarks = model.Remarks,
                GrnNo = model.GrnNo,
                Freight = model.Freight,
                WayBillNo = model.WayBillNo,
                InvoiceType = model.InvoiceType,
                OtherChargesVATAmount = model.OtherChargesVATAmount,
                OtherChargeDetails = model.OtherChargeDetails == null ? new List<PurchaseInvoiceOtherChargeDetailBO>() :
                                    model.OtherChargeDetails.Select(ocd => new PurchaseInvoiceOtherChargeDetailBO()
                                    {
                                        PurchaseOrderID = ocd.PurchaseOrderID,
                                        Particular = ocd.Particular,
                                        POValue = ocd.POValue,
                                        InvoiceValue = ocd.InvoiceValue,
                                        DifferenceValue = ocd.DifferenceValue,
                                        Remarks = ocd.Remarks

                                    }).ToList(),
                TaxDetails = model.TaxDetails == null ? new List<PurchaseInvoiceTaxDetailBO>() :
                               model.TaxDetails.Select(td => new PurchaseInvoiceTaxDetailBO()
                               {
                                   Particular = td.Particular,
                                   TaxPercentage = td.TaxPercentage,
                                   POValue = td.POValue,
                                   InvoiceValue = td.InvoiceValue,
                                   DifferenceValue = td.DifferenceValue,
                                   Remarks = td.Remarks
                               }).ToList(),
                InvoiceTransItems = model.InvoiceTransItems == null ? new List<PurchaseInvoiceTransItemBO>() :
                                    model.InvoiceTransItems.Select(iti => new PurchaseInvoiceTransItemBO()
                                    {
                                        ItemID = iti.ItemID,
                                        ItemName = iti.ItemName,
                                        PartsNumber = iti.PartsNumber,
                                        Remark = iti.Remark,
                                        Model = iti.Model,
                                        IsVat = iti.IsVat,
                                        IsGST = iti.IsGST,
                                        CurrencyID = iti.CurrencyID,
                                        GRNID = iti.GRNID,
                                        GRNTransID = iti.GRNTransID,
                                        MilkPurchaseID = iti.MilkPurchaseID,
                                        InvoiceQty = iti.InvoiceQty,
                                        InvoiceRate = iti.InvoiceRate,
                                        InvoiceValue = iti.InvoiceValue,
                                        SecondaryInvoiceQty = iti.SecondaryInvoiceQty,
                                        SecondaryRate = iti.SecondaryRate,
                                        SecondaryUnit = iti.SecondaryUnit,
                                        SecondaryOfferQty = iti.SecondaryOfferQty,
                                        SecondaryUnitSize = iti.SecondaryUnitSize,
                                        AcceptedQty = iti.AcceptedQty,
                                        ApprovedQty = iti.ApprovedQty,
                                        ApprovedValue = iti.ApprovedValue,
                                        PORate = iti.PORate,
                                        Difference = iti.Difference,
                                        Remarks = iti.Remarks,
                                        UnMatchedQty = iti.UnMatchedQty,
                                        CGSTPercent = iti.CGSTPercent,
                                        IGSTPercent = iti.IGSTPercent,
                                        SGSTPercent = iti.SGSTPercent,
                                        InvoiceGSTPercent = iti.InvoiceGSTPercent,
                                        UnitID = iti.UnitID,
                                        OfferQty = iti.OfferQty,
                                        BatchID = iti.BatchID,
                                        GrossAmount = iti.GrossAmount,
                                        DiscountAmount = iti.DiscountAmount,
                                        DiscountPercent = iti.DiscountPercentage,
                                        VATAmount = iti.VATAmount,
                                        VATPercentage = iti.VATPercentage,
                                        NetAmount = iti.NetAmount,
                                        SGSTAmt = iti.SGSTAmt,
                                        CGSTAmt = iti.CGSTAmt,
                                        IGSTAmt = iti.IGSTAmt,
                                        LandingCost = (iti.NetAmount / model.GrossAmount * model.LandingCost) / iti.InvoiceQty

                                    }).ToList(),
            };
        }
    }

}