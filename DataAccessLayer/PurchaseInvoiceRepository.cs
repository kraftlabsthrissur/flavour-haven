using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PurchaseInvoiceRepository
    {
        private readonly PurchaseEntities _entity;


        public PurchaseInvoiceRepository()
        {
            _entity = new PurchaseEntities();
        }

        public DatatableResultBO GetPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string InvoiceNoHint, string InvoiceDateHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetPurchaseInvoiceList(Type, TransNoHint, TransDateHint, InvoiceNoHint, InvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                InvoiceNo = item.InvoiceNo,
                                InvoiceDate = item.InvoiceDate == null ? "" : ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                SupplierName = item.SupplierName,
                                NetAmount = item.NetAmount,
                                Status = item.Status
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }


        /// <summary>
        /// Save purchase invoice details
        /// </summary>
        /// <param name="purchaseInvoiceBO"></param>
        /// <returns></returns>
        public int SavePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {

            int purchaseInvoiceID = 0;
            if (purchaseInvoiceBO != null && purchaseInvoiceBO.Id <= 0)         //Create
            {
                purchaseInvoiceID = CreatePurchaseInvoice(purchaseInvoiceBO);
            }
            else                                                                   //Edit
            {
                purchaseInvoiceID = !UpdatePurchaseInvoice(purchaseInvoiceBO) ? 0 : purchaseInvoiceBO.Id;
            }
            UpdatePayable(purchaseInvoiceID);
            return purchaseInvoiceID;
        }

        public int GeneratePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {

            int purchaseInvoiceID = 0;
            if (purchaseInvoiceBO != null && purchaseInvoiceBO.Id <= 0)         //Create
            {
                purchaseInvoiceID = PurchaseInvoiceSave(purchaseInvoiceBO);
            }
            else                                                                   //Edit
            {
                purchaseInvoiceID = !UpdatePurchaseInvoiceData(purchaseInvoiceBO) ? 0 : purchaseInvoiceBO.Id;
            }
            UpdatePayable(purchaseInvoiceID);
            return purchaseInvoiceID;
        }
        public int ApprovePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {

            int purchaseInvoiceID = 0;
            if (purchaseInvoiceBO != null && purchaseInvoiceBO.Id <= 0)         //Create
            {
                purchaseInvoiceID = SaveApprovePurchaseInvoice(purchaseInvoiceBO);
            }
            else                                                                   //Edit
            {
                purchaseInvoiceID = UpdateApprovePurchaseInvoiceData(purchaseInvoiceBO);
            }
            UpdatePayable(purchaseInvoiceID);
            return purchaseInvoiceID;
        }

        public List<PurchaseInvoiceBO> GetPurchaseInvoiceWithItemID(int ItemID)
        {
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                return entity.SpGetStockInvoiceIDWithItemID(
                    ItemID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                ).Select(
                    a => new PurchaseInvoiceBO()
                    {
                        Id = a.ID,
                        PurchaseNo = a.PurchaseNo

                    }
                ).ToList();
            }
        }
        public GRNTransItemBO GetStockInvoiceTransForPurchaseReturn(int ItemID, int InvoiceID)
        {
            GRNTransItemBO invoiceTrans = new GRNTransItemBO();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                return dEntity.SpGetStockInvoiceTransForPurchaseReturn(ItemID, InvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNTransItemBO
                {
                    AcceptedQty = (decimal)a.AcceptedQty,
                    Rate = (decimal)a.InvoiceRate,
                    IGSTPercent = (decimal)a.IGSTPercent,
                    SGSTPercent = (decimal)a.SGSTPercent,
                    CGSTPercent = (decimal)a.CGSTPercent,
                    Remarks = a.Remarks,

                }).FirstOrDefault();


            }
        }

        public List<PurchaseInvoiceBO> GetInvoiceList(int SupplierID)
        {
            var fromDate = DateTime.Now.AddYears(-1);
            var ToDate = DateTime.Now;

            PurchaseInvoiceBO invoiceTrans = new PurchaseInvoiceBO();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                return dEntity.SpGetPurchaseInvoiceIDForPurchaseReturn(SupplierID, fromDate, ToDate, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseInvoiceBO
                {
                    PurchaseNo = m.PurchaseNo,
                    InvoiceDate = (DateTime)m.PurchaseDate,
                    NetAmount = (decimal)m.InvoiceTotal,
                    Id = m.ID,
                    InvoiceNo = m.InvoiceNo
                }).ToList();


            }
        }
        public List<PurchaseInvoiceTransItemBO> GetInvoiceTransList(int InvoiceID)
        {
            List<PurchaseInvoiceTransItemBO> list = new List<PurchaseInvoiceTransItemBO>();
            var fromDate = DateTime.Now.AddYears(-1);
            var ToDate = DateTime.Now;
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetPurchaseInvoiceItemForPurchaseReturn(InvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseInvoiceTransItemBO
                {
                    InvoiceID = (int)m.PurchaseInvoiceID,
                    SuppDocAmount = m.SuppDocAmount,
                    SuppShipAmount = m.SuppShipAmount,
                    PackingForwarding = m.PackingForwarding,
                    SuppOtherCharge = m.SupplierOtherCharges ?? 0,
                    FreightAmt = m.FreightAmount,
                    LocalCustomsDuty = m.LocalCustomsDuty,
                    LocalFreight = m.LocalFreight,
                    LocalMiscCharge = m.LocalMiscCharge,
                    LocalOtherCharges = m.LocalOtherCharges,
                    InvoiceTransID = m.PurchaseInvoiceTransID,
                    ItemID = m.ItemID.HasValue ? m.ItemID.Value : 0,
                    ItemCode = m.ItemCode,
                    ItemName = m.ItemName,
                    PartsNumber = m.PartsNumber,
                    Remark = m.Remarks,
                    Model = m.Model,
                    Unit = m.Unit,
                    UnitID = (int)m.UnitID,
                    Rate = (decimal)m.InvoiceRate,
                    AcceptedQty = (decimal)m.AcceptedQty,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    PurchaseNo = m.PurchaseNo,
                    Remarks = (m.Remarks == null) ? "" : m.Remarks,
                    Stock = (decimal)m.Stock,
                    WarehouseID = (int)m.warehouseID,
                    ConvertedStock = (decimal)m.ConvertedStock,
                    PrimaryUnit = m.PrimaryUnit,
                    PrimaryUnitID = m.PrimaryUnitID ?? 0,
                    PurchaseUnit = m.PurchaseUnit,
                    PurchaseUnitID = (int)m.PurchaseUnitID,
                    ConvertedQty = (decimal)m.ConvertedQuantity,
                    InvoiceQty = (decimal)m.InvoiceQty,
                    CGSTAmt = (decimal)m.CGSTAmount,
                    SGSTAmt = (decimal)m.SGSTAmount,
                    IGSTAmt = (decimal)m.IGSTAmount,
                    InvoiceNo = m.InvoiceNo,
                    GrossAmount = (decimal)m.GrossAmount,
                    Discount = (decimal)m.DiscountAmount,
                    DiscountPercent = (decimal)m.DiscountPercentage,
                    OfferQty = (decimal)m.OfferQty,
                    SecondaryInvoiceQty = (decimal)m.SecondaryInvoiceQty,
                    SecondaryOfferQty = (decimal)m.SecondaryOfferQty,
                    SecondaryRate = (decimal)m.SecondaryRate,
                    SecondaryUnit = m.SecondaryUnit,
                    SecondaryUnitSize = m.SecondaryUnitSize,
                    VATPercentage = m.VATPercentage,
                    VATAmount = m.VATAmount

                }).ToList();
                return list;
            }

        }


        /// <summary>
        /// Insert new Purchase Invoice and Purchase Invoice details
        /// </summary>
        /// <param name="purchaseInvoiceBO"></param>
        /// <returns></returns>
        private int CreatePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            int purchaseInvoiceID = 0;
            if (purchaseInvoiceBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PurchaseInvoice";
                        ObjectParameter purchaseInvoiceIDOut = new ObjectParameter("purchaseInvoiceID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                        if (purchaseInvoiceBO.IsDraft)
                        {
                            FormName = "DraftPurchaseInvoice";
                        }

                        var j = _entity.SpUpdateSerialNo(
                            FormName,
                            "Code",
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SerialNo
                        );
                        var i = _entity.SpCreatePurchaseInvoice(
                            SerialNo.Value.ToString(),
                            purchaseInvoiceBO.PurchaseDate,
                            purchaseInvoiceBO.SupplierID,
                            purchaseInvoiceBO.LocalSupplierName,
                            purchaseInvoiceBO.InvoiceNo,
                            purchaseInvoiceBO.InvoiceDate,
                            purchaseInvoiceBO.GrossAmount,
                            purchaseInvoiceBO.InvoiceTotal,
                            purchaseInvoiceBO.TotalDifference,
                            purchaseInvoiceBO.SGSTAmount,
                            purchaseInvoiceBO.CGSTAmount,
                            purchaseInvoiceBO.IGSTAmount,
                            purchaseInvoiceBO.Discount,
                            purchaseInvoiceBO.FreightAmount,
                            purchaseInvoiceBO.PackingCharges,
                            purchaseInvoiceBO.SupplierOtherCharges,
                            purchaseInvoiceBO.TaxOnFreight,
                            purchaseInvoiceBO.TaxOnPackingCharges,
                            purchaseInvoiceBO.TaxOnOtherCharge,
                            purchaseInvoiceBO.TDSOnFreight,
                            purchaseInvoiceBO.LessTDS,
                            purchaseInvoiceBO.OtherDeductions,
                            purchaseInvoiceBO.AmountPayable,
                            purchaseInvoiceBO.NetAmount,
                            purchaseInvoiceBO.IsDraft,
                            false,
                            null,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            purchaseInvoiceBO.Status,
                            purchaseInvoiceBO.TDSID,
                            0, 0, 0, 0, 0, 0, 0,
                            false,
                            purchaseInvoiceBO.SelectedQuotationID,
                            purchaseInvoiceBO.Remarks,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            purchaseInvoiceIDOut
                        );


                        _entity.SaveChanges();

                        if (purchaseInvoiceIDOut.Value != null && Convert.ToInt32(purchaseInvoiceIDOut.Value) > 0)
                        {
                            purchaseInvoiceID = Convert.ToInt32(purchaseInvoiceIDOut.Value);

                            if (purchaseInvoiceBO.OtherChargeDetails != null)
                                foreach (var otherCharge in purchaseInvoiceBO.OtherChargeDetails)
                                {
                                    var result = _entity.SpCreatePurchaseInvoiceOtherChargesDetails(
                                        purchaseInvoiceID,
                                        otherCharge.PurchaseOrderID,
                                        otherCharge.Particular,
                                        otherCharge.POValue,
                                        otherCharge.InvoiceValue,
                                        otherCharge.DifferenceValue,
                                        otherCharge.Remarks,
                                        DateTime.Now);
                                }

                            if (purchaseInvoiceBO.TaxDetails != null)
                                foreach (var tax in purchaseInvoiceBO.TaxDetails)
                                {
                                    var result = _entity.SpCreatePurchaseInvoiceTaxDetails(
                                        purchaseInvoiceID,
                                        tax.Particular,
                                        tax.TaxPercentage,
                                        tax.POValue,
                                        tax.InvoiceValue,
                                        tax.DifferenceValue,
                                        tax.Remarks,
                                        DateTime.Now
                                    );

                                }


                            if (purchaseInvoiceBO.InvoiceTransItems != null)
                                foreach (var transItem in purchaseInvoiceBO.InvoiceTransItems)
                                {
                                    var result = _entity.SpCreatePurchaseInvoiceTrans(
                                        purchaseInvoiceID,
                                        transItem.GRNID,
                                        transItem.GRNTransID,
                                        transItem.ItemID,
                                        transItem.InvoiceQty,
                                        transItem.InvoiceRate,
                                        transItem.InvoiceValue,
                                        transItem.AcceptedQty,
                                        transItem.ApprovedQty,
                                        transItem.PORate,
                                        transItem.Difference,
                                        transItem.Remarks,
                                        transItem.UnMatchedQty,
                                        transItem.IGSTPercent,
                                        transItem.CGSTPercent,
                                        transItem.SGSTPercent,
                                        transItem.InvoiceGSTPercent,
                                        transItem.MilkPurchaseID,
                                        transItem.UnitID,
                                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        ReturnValue
                                    );
                                    if (Convert.ToInt32(ReturnValue.Value) == -2)
                                    {
                                        throw new DuplicateEntryException("Some of the items in the purchase invoice already processed");
                                    }
                                    if (Convert.ToInt32(ReturnValue.Value) == -1)
                                    {
                                        throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                                    }
                                }
                        }
                        ;
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        purchaseInvoiceID = 0;
                        throw ex;
                    }
                }
            }
            return purchaseInvoiceID;
        }
        private int PurchaseInvoiceSave(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            int purchaseInvoiceID = 0;
            if (purchaseInvoiceBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PurchaseInvoice";
                        ObjectParameter purchaseInvoiceIDOut = new ObjectParameter("purchaseInvoiceID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                        if (purchaseInvoiceBO.IsDraft)
                        {
                            FormName = "DraftPurchaseInvoice";
                        }

                        var j = _entity.SpUpdateSerialNo(
                            FormName,
                            "Code",
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SerialNo
                        );
                        var i = _entity.SpCreatePurchaseInvoiceMaster(
                            SerialNo.Value.ToString(),
                            purchaseInvoiceBO.PurchaseDate,
                            purchaseInvoiceBO.SupplierID,
                            purchaseInvoiceBO.LocalSupplierName,
                            purchaseInvoiceBO.InvoiceNo,
                            purchaseInvoiceBO.InvoiceDate,
                            purchaseInvoiceBO.GrossAmount,
                            purchaseInvoiceBO.InvoiceTotal,
                            purchaseInvoiceBO.TotalDifference,
                            purchaseInvoiceBO.SGST,
                            purchaseInvoiceBO.CGST,
                            purchaseInvoiceBO.IGST,
                            purchaseInvoiceBO.Discount,
                            purchaseInvoiceBO.VATAmount,
                            purchaseInvoiceBO.FreightAmount,
                            purchaseInvoiceBO.PackingCharges,
                            purchaseInvoiceBO.SuppDocAmount,
                            purchaseInvoiceBO.SuppShipAmount,
                            purchaseInvoiceBO.SupplierOtherCharges,
                            purchaseInvoiceBO.CurrencyID,
                            purchaseInvoiceBO.IsGST,
                            purchaseInvoiceBO.IsVAT,
                            purchaseInvoiceBO.PackingForwarding,
                            purchaseInvoiceBO.LocalCustomsDuty,
                            purchaseInvoiceBO.LocalFreight,
                            purchaseInvoiceBO.LocalMiscCharge,
                            purchaseInvoiceBO.LocalOtherCharges,
                            purchaseInvoiceBO.CurrencyExchangeRate,
                            purchaseInvoiceBO.VATPercentage,
                            purchaseInvoiceBO.TaxOnFreight,
                            purchaseInvoiceBO.TaxOnPackingCharges,
                            purchaseInvoiceBO.TaxOnOtherCharge,
                            purchaseInvoiceBO.TDSOnFreight,
                            purchaseInvoiceBO.LessTDS,
                            purchaseInvoiceBO.OtherDeductions,
                            purchaseInvoiceBO.AmountPayable,
                            purchaseInvoiceBO.NetAmount,
                            purchaseInvoiceBO.IsDraft,
                            false,
                            null,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            purchaseInvoiceBO.Status,
                            purchaseInvoiceBO.TDSID,
                            0, 0, 0, 0, 0, 0, 0,
                            false,
                            purchaseInvoiceBO.SelectedQuotationID,
                            purchaseInvoiceBO.Remarks,
                            purchaseInvoiceBO.GrnNo,
                            purchaseInvoiceBO.Freight,
                            purchaseInvoiceBO.WayBillNo,
                            purchaseInvoiceBO.InvoiceType,
                            purchaseInvoiceBO.OtherChargesVATAmount,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            purchaseInvoiceIDOut
                        );


                        _entity.SaveChanges();

                        if (purchaseInvoiceIDOut.Value != null && Convert.ToInt32(purchaseInvoiceIDOut.Value) > 0)
                        {
                            purchaseInvoiceID = Convert.ToInt32(purchaseInvoiceIDOut.Value);

                            if (purchaseInvoiceBO.InvoiceTransItems != null)
                                foreach (var transItem in purchaseInvoiceBO.InvoiceTransItems)
                                {
                                    var result = _entity.SpCreatePurchaseInvoiceTransDetails(
                                        purchaseInvoiceID,
                                        transItem.GRNID,
                                        transItem.GRNTransID,
                                        transItem.ItemID,
                                        transItem.InvoiceQty,
                                        transItem.InvoiceRate,
                                        transItem.InvoiceValue,
                                        transItem.AcceptedQty,
                                        transItem.ApprovedQty,
                                        transItem.PORate,
                                        transItem.Difference,
                                        transItem.Remarks,
                                        transItem.UnMatchedQty,
                                        transItem.IGSTPercent,
                                        transItem.CGSTPercent,
                                        transItem.SGSTPercent,
                                        transItem.InvoiceGSTPercent,
                                        transItem.MilkPurchaseID,
                                        transItem.UnitID,
                                        transItem.BatchID,
                                        0, 0,
                                        transItem.DiscountAmount,
                                        0, 0, 0, 0, 0, 0,
                                        0,
                                        transItem.NetAmount,
                                        transItem.SGSTAmt,
                                        transItem.IGSTAmt,
                                        transItem.CGSTAmt,
                                        transItem.OfferQty,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        ReturnValue
                                    );
                                    if (Convert.ToInt32(ReturnValue.Value) == -2)
                                    {
                                        throw new DuplicateEntryException("Some of the items in the purchase invoice already processed");
                                    }
                                    if (Convert.ToInt32(ReturnValue.Value) == -1)
                                    {
                                        throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                                    }
                                    if (Convert.ToInt32(ReturnValue.Value) == -3)
                                    {
                                        throw new LessProfitException("Profit tatio for " + transItem.ItemName + " is less than previous batch. please contact administrator");
                                    }
                                    if (Convert.ToInt32(ReturnValue.Value) == -4)
                                    {
                                        throw new LessProfitException("Unit for " + transItem.ItemName + " is different for previous batch. please contact administrator");
                                    }
                                }
                        }
                        ;
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        purchaseInvoiceID = 0;
                        throw ex;
                    }
                }
            }
            return purchaseInvoiceID;
        }
        private int SaveApprovePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            int purchaseInvoiceID = 0;
            if (purchaseInvoiceBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PurchaseInvoice";
                        ObjectParameter purchaseInvoiceIDOut = new ObjectParameter("purchaseInvoiceID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                        if (purchaseInvoiceBO.IsDraft)
                        {
                            FormName = "DraftPurchaseInvoice";
                        }

                        var j = _entity.SpUpdateSerialNo(
                            FormName,
                            "Code",
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SerialNo
                        );
                        var i = _entity.SpCreatePurchaseInvoiceMaster(
                            SerialNo.Value.ToString(),
                            purchaseInvoiceBO.PurchaseDate,
                            purchaseInvoiceBO.SupplierID,
                            purchaseInvoiceBO.LocalSupplierName,
                            purchaseInvoiceBO.InvoiceNo,
                            purchaseInvoiceBO.InvoiceDate,
                            purchaseInvoiceBO.GrossAmount,
                            purchaseInvoiceBO.InvoiceTotal,
                            purchaseInvoiceBO.TotalDifference,
                            purchaseInvoiceBO.SGST,
                            purchaseInvoiceBO.CGST,
                            purchaseInvoiceBO.IGST,
                            purchaseInvoiceBO.Discount,
                            purchaseInvoiceBO.VATAmount,
                            purchaseInvoiceBO.FreightAmount,
                            purchaseInvoiceBO.PackingCharges,
                            purchaseInvoiceBO.SuppDocAmount,
                            purchaseInvoiceBO.SuppShipAmount,
                            purchaseInvoiceBO.SupplierOtherCharges,
                            purchaseInvoiceBO.CurrencyID,
                            purchaseInvoiceBO.IsGST,
                            purchaseInvoiceBO.IsVAT,
                            purchaseInvoiceBO.PackingForwarding,
                            purchaseInvoiceBO.LocalCustomsDuty,
                            purchaseInvoiceBO.LocalFreight,
                            purchaseInvoiceBO.LocalMiscCharge,
                            purchaseInvoiceBO.LocalOtherCharges,
                            purchaseInvoiceBO.CurrencyExchangeRate,
                            purchaseInvoiceBO.VATPercentage,
                            purchaseInvoiceBO.TaxOnFreight,
                            purchaseInvoiceBO.TaxOnPackingCharges,
                            purchaseInvoiceBO.TaxOnOtherCharge,
                            purchaseInvoiceBO.TDSOnFreight,
                            purchaseInvoiceBO.LessTDS,
                            purchaseInvoiceBO.OtherDeductions,
                            purchaseInvoiceBO.AmountPayable,
                            purchaseInvoiceBO.NetAmount,
                            purchaseInvoiceBO.IsDraft,
                            false,
                            null,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            purchaseInvoiceBO.Status,
                            purchaseInvoiceBO.TDSID,
                            0, 0, 0, 0, 0, 0, 0,
                            false,
                            purchaseInvoiceBO.SelectedQuotationID,
                            purchaseInvoiceBO.Remarks,
                            purchaseInvoiceBO.GrnNo,
                            purchaseInvoiceBO.Freight,
                            purchaseInvoiceBO.WayBillNo,
                            purchaseInvoiceBO.InvoiceType,
                            purchaseInvoiceBO.OtherChargesVATAmount,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            purchaseInvoiceIDOut
                        );


                        _entity.SaveChanges();

                        if (purchaseInvoiceIDOut.Value != null && Convert.ToInt32(purchaseInvoiceIDOut.Value) > 0)
                        {
                            purchaseInvoiceID = Convert.ToInt32(purchaseInvoiceIDOut.Value);

                            if (purchaseInvoiceBO.InvoiceTransItems != null)
                                foreach (var transItem in purchaseInvoiceBO.InvoiceTransItems)
                                {
                                    var result = _entity.SpCreatePurchaseInvoiceTransWithOverRule(
                                        purchaseInvoiceID,
                                        transItem.GRNID,
                                        transItem.GRNTransID,
                                        transItem.ItemID,
                                        transItem.InvoiceQty,
                                        transItem.InvoiceRate,
                                        transItem.InvoiceValue,
                                        transItem.AcceptedQty,
                                        transItem.ApprovedQty,
                                        transItem.PORate,
                                        transItem.Difference,
                                        transItem.Remarks,
                                        transItem.UnMatchedQty,
                                        transItem.IGSTPercent,
                                        transItem.CGSTPercent,
                                        transItem.SGSTPercent,
                                        transItem.InvoiceGSTPercent,
                                        transItem.MilkPurchaseID,
                                        transItem.UnitID,
                                        transItem.BatchID,
                                        0,
                                        transItem.DiscountPercent,
                                        transItem.DiscountAmount,
                                        0, 0,
                                        transItem.TaxableAmount,
                                        transItem.VATPercentage,
                                        transItem.VATAmount,
                                        transItem.SecondaryUnit,
                                        transItem.SecondaryInvoiceQty,
                                        transItem.SecondaryOfferQty,
                                        transItem.SecondaryRate,
                                        transItem.SecondaryUnitSize,
                                        transItem.CurrencyID,
                                        transItem.IsGST,
                                        transItem.IsVat,
                                        transItem.Model,
                                        transItem.PartsNumber,
                                        transItem.ItemName,
                                        0, 0, 0,
                                        transItem.GrossAmount,
                                        transItem.NetAmount,
                                        transItem.SGSTAmt,
                                        transItem.IGSTAmt,
                                        transItem.CGSTAmt,
                                        transItem.OfferQty,
                                        transItem.LandingCost,
                                        purchaseInvoiceBO.CurrencyExchangeRate,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        ReturnValue
                                    );
                                    if (Convert.ToInt32(ReturnValue.Value) == -2)
                                    {
                                        throw new DuplicateEntryException("Some of the items in the purchase invoice already processed");
                                    }
                                    if (Convert.ToInt32(ReturnValue.Value) == -1)
                                    {
                                        throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                                    }

                                }
                        }
                        ;
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        purchaseInvoiceID = 0;
                        throw ex;
                    }
                }
            }
            return purchaseInvoiceID;
        }
        private int UpdateApprovePurchaseInvoiceData(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            int purchaseInvoiceID = 0;
            if (purchaseInvoiceBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                        _entity.SpUpdatePurchaseInvoiceMaster(
                            purchaseInvoiceBO.Id,
                            purchaseInvoiceBO.InvoiceNo,
                            purchaseInvoiceBO.InvoiceDate,
                            purchaseInvoiceBO.GrossAmount,
                            purchaseInvoiceBO.InvoiceTotal,
                            purchaseInvoiceBO.TotalDifference,
                            purchaseInvoiceBO.SGST,
                            purchaseInvoiceBO.CGST,
                            purchaseInvoiceBO.IGST,
                            purchaseInvoiceBO.Discount,
                            purchaseInvoiceBO.VATAmount,
                            purchaseInvoiceBO.SuppDocAmount,
                            purchaseInvoiceBO.SuppShipAmount,
                            purchaseInvoiceBO.SupplierOtherCharges,
                            purchaseInvoiceBO.FreightAmount,
                            purchaseInvoiceBO.PackingCharges,
                            purchaseInvoiceBO.PackingForwarding,
                            purchaseInvoiceBO.LocalCustomsDuty,
                            purchaseInvoiceBO.LocalFreight,
                            purchaseInvoiceBO.LocalMiscCharge,
                            purchaseInvoiceBO.LocalOtherCharges,
                            purchaseInvoiceBO.TaxOnFreight,
                            purchaseInvoiceBO.TaxOnPackingCharges,
                            purchaseInvoiceBO.TaxOnOtherCharge,
                            purchaseInvoiceBO.TDSOnFreight,
                            purchaseInvoiceBO.LessTDS,
                            purchaseInvoiceBO.OtherDeductions,
                            purchaseInvoiceBO.AmountPayable,
                            purchaseInvoiceBO.NetAmount,
                            purchaseInvoiceBO.IsDraft,
                            false,
                            null,
                            purchaseInvoiceBO.SelectedQuotationID,
                            purchaseInvoiceBO.Remarks,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            purchaseInvoiceBO.Status,
                            purchaseInvoiceBO.TDSID,
                            purchaseInvoiceBO.GrnNo,
                            purchaseInvoiceBO.Freight,
                            purchaseInvoiceBO.WayBillNo,
                            purchaseInvoiceBO.InvoiceType,
                            purchaseInvoiceBO.OtherChargesVATAmount,
                            GeneralBO.FinYear, GeneralBO.LocationID,
                            GeneralBO.ApplicationID
);

                        _entity.SaveChanges();


                        if (purchaseInvoiceBO.InvoiceTransItems != null)
                            foreach (var transItem in purchaseInvoiceBO.InvoiceTransItems)
                            {
                                var result = _entity.SpCreatePurchaseInvoiceTransWithOverRule(
                                     purchaseInvoiceBO.Id,
                                    transItem.GRNID,
                                    transItem.GRNTransID,
                                    transItem.ItemID,
                                    transItem.InvoiceQty,
                                    transItem.InvoiceRate,
                                    transItem.InvoiceValue,
                                    transItem.AcceptedQty,
                                    transItem.ApprovedQty,
                                    transItem.PORate,
                                    transItem.Difference,
                                    transItem.Remarks,
                                    transItem.UnMatchedQty,
                                    transItem.IGSTPercent,
                                    transItem.CGSTPercent,
                                    transItem.SGSTPercent,
                                    transItem.InvoiceGSTPercent,
                                    transItem.MilkPurchaseID,
                                    transItem.UnitID,
                                    transItem.BatchID,
                                    0,
                                    transItem.DiscountPercent,
                                    transItem.DiscountAmount,
                                    0, 0,
                                    transItem.TaxableAmount,
                                    transItem.VATPercentage,
                                    transItem.VATAmount,
                                    transItem.SecondaryUnit,
                                    transItem.SecondaryInvoiceQty,
                                    transItem.SecondaryOfferQty,
                                    transItem.SecondaryRate,
                                    transItem.SecondaryUnitSize,
                                    transItem.CurrencyID,
                                    transItem.IsGST,
                                    transItem.IsVat,
                                    transItem.Model,
                                    transItem.PartsNumber,
                                    transItem.ItemName,
                                    0, 0, 0,
                                    transItem.GrossAmount,
                                    transItem.NetAmount,
                                    transItem.SGSTAmt,
                                    transItem.IGSTAmt,
                                    transItem.CGSTAmt,
                                    transItem.OfferQty,
                                    transItem.LandingCost,
                                    purchaseInvoiceBO.CurrencyExchangeRate,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    ReturnValue
                                );
                                if (Convert.ToInt32(ReturnValue.Value) == -2)
                                {
                                    throw new DuplicateEntryException("Some of the items in the purchase invoice already processed");
                                }
                                if (Convert.ToInt32(ReturnValue.Value) == -1)
                                {
                                    throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                                }


                            }
                        ;
                        transaction.Commit();
                        return purchaseInvoiceBO.Id;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        purchaseInvoiceID = 0;
                        throw ex;
                    }
                }
            }
            return purchaseInvoiceID;
        }
        /// <summary>
        /// Edit PurchaseInvoice
        /// </summary>
        /// <param name="purchaseInvoiceBO"></param>
        /// <returns></returns>
        private bool UpdatePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

            bool updateResult = true;
            using (var transaction = _entity.Database.BeginTransaction())
            {
                try
                {
                    _entity.SpUpdatePurchaseInvoice(
                        purchaseInvoiceBO.Id,
                        purchaseInvoiceBO.InvoiceNo,
                        purchaseInvoiceBO.InvoiceDate,
                        purchaseInvoiceBO.GrossAmount,
                        purchaseInvoiceBO.InvoiceTotal,
                        purchaseInvoiceBO.TotalDifference,
                        purchaseInvoiceBO.SGST,
                        purchaseInvoiceBO.CGST,
                        purchaseInvoiceBO.IGST,
                        purchaseInvoiceBO.Discount,
                        purchaseInvoiceBO.FreightAmount,
                        purchaseInvoiceBO.PackingCharges,
                        purchaseInvoiceBO.SupplierOtherCharges,
                        purchaseInvoiceBO.TaxOnFreight,
                        purchaseInvoiceBO.TaxOnPackingCharges,
                        purchaseInvoiceBO.TaxOnOtherCharge,
                        purchaseInvoiceBO.TDSOnFreight,
                        purchaseInvoiceBO.LessTDS,
                        purchaseInvoiceBO.OtherDeductions,
                        purchaseInvoiceBO.AmountPayable,
                        purchaseInvoiceBO.NetAmount,
                        purchaseInvoiceBO.IsDraft,
                        false,
                        null,
                        purchaseInvoiceBO.SelectedQuotationID,
                        purchaseInvoiceBO.Remarks,
                        GeneralBO.CreatedUserID,
                        DateTime.Now,
                         purchaseInvoiceBO.Status,
                         purchaseInvoiceBO.TDSID,
                        GeneralBO.FinYear, GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    );

                    //_entity.SaveChanges();


                    if (purchaseInvoiceBO.OtherChargeDetails != null)
                        foreach (var otherCharge in purchaseInvoiceBO.OtherChargeDetails)
                        {
                            var result = _entity.SpCreatePurchaseInvoiceOtherChargesDetails(
                                purchaseInvoiceBO.Id,
                                otherCharge.PurchaseOrderID,
                                otherCharge.Particular,
                                otherCharge.POValue,
                                otherCharge.InvoiceValue,
                                otherCharge.DifferenceValue,
                                otherCharge.Remarks,
                                DateTime.Now
                            );
                        }

                    if (purchaseInvoiceBO.TaxDetails != null)
                        foreach (var tax in purchaseInvoiceBO.TaxDetails)
                        {
                            var result = _entity.SpCreatePurchaseInvoiceTaxDetails(
                                purchaseInvoiceBO.Id,
                                tax.Particular,
                                tax.TaxPercentage,
                                tax.POValue,
                                tax.InvoiceValue,
                                tax.DifferenceValue,
                                tax.Remarks,
                                DateTime.Now
                            );

                        }


                    if (purchaseInvoiceBO.InvoiceTransItems != null)
                        foreach (var transItem in purchaseInvoiceBO.InvoiceTransItems)
                        {
                            var result = _entity.SpCreatePurchaseInvoiceTrans(
                                purchaseInvoiceBO.Id,
                                transItem.GRNID,
                                transItem.GRNTransID,
                                transItem.ItemID,
                                transItem.InvoiceQty,
                                transItem.InvoiceRate,
                                transItem.InvoiceValue,
                                transItem.AcceptedQty,
                                transItem.ApprovedQty,
                                transItem.PORate,
                                transItem.Difference,
                                transItem.Remarks,
                                transItem.UnMatchedQty,
                                transItem.IGSTPercent,
                                transItem.CGSTPercent,
                                transItem.SGSTPercent,
                                transItem.InvoiceGSTPercent,
                                transItem.MilkPurchaseID,
                                transItem.UnitID,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                ReturnValue
                            );
                            if (Convert.ToInt32(ReturnValue.Value) == -2)
                            {
                                throw new DuplicateEntryException("Some of the items in the purchase invoice already processed");
                            }
                            if (Convert.ToInt32(ReturnValue.Value) == -1)
                            {
                                throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                            }
                            if (Convert.ToInt32(ReturnValue.Value) == -3)
                            {
                                throw new LessProfitException("Profit tatio for " + transItem.ItemName + " is less than previous batch. please contact administrator");
                            }
                        }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    updateResult = false;
                    throw ex;
                }
            }
            return updateResult;
        }
        private bool UpdatePurchaseInvoiceData(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

            bool updateResult = true;
            using (var transaction = _entity.Database.BeginTransaction())
            {
                try
                {
                    _entity.SpUpdatePurchaseInvoiceMaster(
                        purchaseInvoiceBO.Id,
                        purchaseInvoiceBO.InvoiceNo,
                        purchaseInvoiceBO.InvoiceDate,
                        purchaseInvoiceBO.GrossAmount,
                        purchaseInvoiceBO.InvoiceTotal,
                        purchaseInvoiceBO.TotalDifference,
                        purchaseInvoiceBO.SGST,
                        purchaseInvoiceBO.CGST,
                        purchaseInvoiceBO.IGST,
                        purchaseInvoiceBO.Discount,
                        purchaseInvoiceBO.VATAmount,
                        purchaseInvoiceBO.SuppDocAmount,
                        purchaseInvoiceBO.SuppShipAmount,
                        purchaseInvoiceBO.SupplierOtherCharges,
                        purchaseInvoiceBO.FreightAmount,
                        purchaseInvoiceBO.PackingCharges,
                        purchaseInvoiceBO.PackingForwarding,
                        purchaseInvoiceBO.LocalCustomsDuty,
                        purchaseInvoiceBO.LocalFreight,
                        purchaseInvoiceBO.LocalMiscCharge,
                        purchaseInvoiceBO.LocalOtherCharges,
                        purchaseInvoiceBO.TaxOnFreight,
                        purchaseInvoiceBO.TaxOnPackingCharges,
                        purchaseInvoiceBO.TaxOnOtherCharge,
                        purchaseInvoiceBO.TDSOnFreight,
                        purchaseInvoiceBO.LessTDS,
                        purchaseInvoiceBO.OtherDeductions,
                        purchaseInvoiceBO.AmountPayable,
                        purchaseInvoiceBO.NetAmount,
                        purchaseInvoiceBO.IsDraft,
                        false,
                        null,
                        purchaseInvoiceBO.SelectedQuotationID,
                        purchaseInvoiceBO.Remarks,
                        GeneralBO.CreatedUserID,
                        DateTime.Now,
                         purchaseInvoiceBO.Status,
                         purchaseInvoiceBO.TDSID,
                         purchaseInvoiceBO.GrnNo,
                         purchaseInvoiceBO.Freight,
                         purchaseInvoiceBO.WayBillNo,
                         purchaseInvoiceBO.InvoiceType,
                         purchaseInvoiceBO.OtherChargesVATAmount,
                        GeneralBO.FinYear, GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    );
                    if (purchaseInvoiceBO.InvoiceTransItems != null)
                        foreach (var transItem in purchaseInvoiceBO.InvoiceTransItems)
                        {
                            var result = _entity.SpCreatePurchaseInvoiceTransDetails(
                                      purchaseInvoiceBO.Id,
                                      transItem.GRNID,
                                      transItem.GRNTransID,
                                      transItem.ItemID,
                                      transItem.InvoiceQty,
                                      transItem.InvoiceRate,
                                      transItem.InvoiceValue,
                                      transItem.AcceptedQty,
                                      transItem.ApprovedQty,
                                      transItem.PORate,
                                      transItem.Difference,
                                      transItem.Remarks,
                                      transItem.UnMatchedQty,
                                      transItem.IGSTPercent,
                                      transItem.CGSTPercent,
                                      transItem.SGSTPercent,
                                      transItem.InvoiceGSTPercent,
                                      transItem.MilkPurchaseID,
                                      transItem.UnitID,
                                      transItem.BatchID,
                                      0, 0,
                                      transItem.DiscountAmount,
                                      0, 0, 0, 0, 0, 0,
                                      0,
                                      transItem.NetAmount,
                                      transItem.SGSTAmt,
                                      transItem.IGSTAmt,
                                      transItem.CGSTAmt,
                                      transItem.OfferQty,
                                      GeneralBO.FinYear,
                                      GeneralBO.LocationID,
                                      GeneralBO.ApplicationID,
                                      ReturnValue
                                  );
                            if (Convert.ToInt32(ReturnValue.Value) == -2)
                            {
                                throw new DuplicateEntryException("Some of the items in the purchase invoice already processed");
                            }
                            if (Convert.ToInt32(ReturnValue.Value) == -1)
                            {
                                throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                            }
                            if (Convert.ToInt32(ReturnValue.Value) == -3)
                            {
                                throw new LessProfitException("Profit ratio for " + transItem.ItemName + " is less than previous batch. please contact administrator");
                            }
                            if (Convert.ToInt32(ReturnValue.Value) == -4)
                            {
                                throw new LessProfitException("Unit for " + transItem.ItemName + " is different for previous batch. please contact administrator");
                            }
                        }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    updateResult = false;
                    throw ex;
                }
            }
            return updateResult;

        }
        public bool UpdatePayable(int ID)
        {
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                try
                {
                    entity.SpUpdatePayableDueDate(ID, "Stock", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                catch (Exception ex)
                {
                    return false;

                }
            }
            return true;

        }

        /// <summary>
        /// Get UnSettled Invoices by Supplier
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public List<UnSettledPurchaseInoviceBO> GetUnSettledInvoicesBySupplier(int supplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnSettledPurchaseInvoicesBySupplier(
                        supplierID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new UnSettledPurchaseInoviceBO()
                        {
                            PayableID = a.PayableID,
                            SupplierName = a.SupplierName,
                            Description = a.Description,
                            CreatedDate = a.CreatedDate ?? new DateTime(),
                            InvoiceAmount = (double)a.InvoiceAmount,
                            AmountToBePaid = a.AmountToBePayed != null ? (double)a.AmountToBePayed : 0
                        }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Purchase Invoice by ID
        /// </summary>
        /// <param name="purchaseInvoiceID"></param>
        /// <returns></returns>
        public PurchaseInvoiceBO GetPurchaseInvoice(int purchaseInvoiceID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseInvoiceByID(
                        purchaseInvoiceID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new PurchaseInvoiceBO()
                        {
                            Id = a.Id,
                            PurchaseNo = a.PurchaseNo,
                            PurchaseDate = a.PurchaseDate ?? new DateTime(),
                            InvoiceNo = a.InvoiceNo,
                            InvoiceDate = a.InvoiceDate ?? new DateTime(),
                            SupplierID = a.SupplierID ?? 0,
                            SupplierName = a.SupplierName,
                            LocalSupplierName = a.LocalSupplier,
                            NetAmount = a.NetAmount ?? 0,
                            InvoiceTotal = (decimal)a.InvoiceTotal,
                            GrossAmount = a.GrossAmount ?? 0,
                            SGSTAmount = a.SGSTAmount ?? 0,
                            CGSTAmount = a.CGSTAmount ?? 0,
                            IGSTAmount = a.IGSTAmount ?? 0,
                            Discount = a.Discount ?? 0,
                            FreightAmount = a.FreightAmount ?? 0,
                            PackingCharges = a.PackingCharges ?? 0,
                            //OtherCharges = a.OtherCharges ?? 0,
                            TaxOnFreight = a.TaxOnFreight ?? 0,
                            TaxOnPackingCharges = a.TaxOnPackingCharges ?? 0,
                            TDSOnFreight = a.TDSOnFreightPercentage ?? 0,
                            LessTDS = a.LessTDS ?? 0,
                            AmountPayable = a.AmountPayable ?? 0,
                            IsDraft = (bool)a.IsDraft,
                            TotalDifference = (decimal)a.TotalDifference,
                            OtherDeductions = (decimal)a.OtherDeductions,
                            StateID = a.StateID,
                            IsGSTRegistered = (bool)a.IsGSTRegistered,
                            Status = a.Status,
                            PurchaseOrderDate = a.PurchaseOrderDate ?? new DateTime(),
                            SupplierCode = a.SupplierCode,
                            TDSCode = a.TDSCode,
                            TDSID = a.TDSID,
                            Rate = string.Concat(a.TDSID, "#", a.TDSRate),
                            SupplierLocation = a.SupplierLocation,
                            GSTNo = a.GSTNo,
                            IsCancelled = (bool)a.IsCancelled,
                            SupplierCategory = a.SupplierCategory,
                            SelectedQuotationID = a.SelectedQuotationID,
                            Remarks = a.Remarks
                        }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public PurchaseInvoiceBO GetPurchaseInvoiceDetails(int purchaseInvoiceID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var data = dbEntity.SpGetPurchaseInvoiceDetailsByID(purchaseInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return data.Select(a => new PurchaseInvoiceBO()
                    {

                        Id = a.Id,
                        PurchaseNo = a.PurchaseNo,
                        PurchaseDate = a.PurchaseDate,
                        InvoiceNo = a.InvoiceNo,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        SecondaryQty = a.SecondaryQty,
                        InvoiceDate = a.InvoiceDate,
                        SupplierID = a.SupplierID ?? 0,
                        SupplierName = a.SupplierName,
                        LocalSupplierName = a.LocalSupplier,
                        NetAmount = a.NetAmount ?? 0,
                        InvoiceTotal = (decimal)a.InvoiceTotal,
                        GrossAmount = a.GrossAmount ?? 0,
                        SGST = a.SGSTAmount ?? 0,
                        CGST = a.CGSTAmount ?? 0,
                        IGST = a.IGSTAmount ?? 0,
                        Discount = a.Discount ?? 0,
                        VATAmount = a.VATAmount,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        AmountPayable = a.AmountPayable ?? 0,
                        IsDraft = (bool)a.IsDraft,
                        TotalDifference = (decimal)a.TotalDifference,
                        OtherDeductions = (decimal)a.OtherDeductions,
                        //OtherCharges = a.OtherCharges ?? 0,
                        //DocumentCharges = a.DocumentCharges ?? 0,
                        SuppDocAmount = a.SuppDocAmount,
                        SuppShipAmount = a.SuppShipAmount,
                        SupplierOtherCharges = a.SupplierOtherCharges ?? 0,
                        FreightAmount = a.FreightAmount ?? 0,
                        PackingForwarding = a.PackingForwarding,
                        LocalCustomsDuty = a.LocalCustomsDuty,
                        LocalFreight = a.LocalFreight,
                        LocalMiscCharge = a.LocalMiscCharge,
                        LocalOtherCharges = a.LocalOtherCharges,
                        LocalLandingCost = a.LocalCustomsDuty + a.LocalMiscCharge + a.LocalOtherCharges + a.LocalFreight,
                        CurrencyExchangeRate = a.CurrencyExchangeRate,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        CurrencyCode = a.CurrencyCode,
                        CurrencyName = a.CurrencyName,
                        IsGST = a.IsGST ?? 0,
                        IsVAT = a.IsVat ?? 0,
                        StateID = a.StateID,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        Status = a.Status,
                        PurchaseOrderDate = a.PurchaseOrderDate ?? new DateTime(),
                        SupplierCode = a.SupplierCode,
                        SupplierLocation = a.SupplierLocation,
                        GSTNo = a.GSTNo,
                        IsCancelled = (bool)a.IsCancelled,
                        SupplierCategory = a.SupplierCategory,
                        SelectedQuotationID = a.SelectedQuotationID,
                        Remarks = a.Remarks,
                        GrnNo = a.GRNNo,
                        shipmentmode = a.shipmentmode,
                        suppliercurrencyCode = a.suppliercurrencyCode,
                        SuuplierCurrencyconverion = (decimal)a.SuuplierCurrencyconverion,
                        PItype = a.PItype,
                        Freight = a.Freight,
                        WayBillNo = a.WayBillNo,
                        VatRegNo = a.VatRegNo,
                        InvoiceType = a.InvoiceType,
                        OtherChargesVATAmount = (decimal)a.OtherChargesVATAmount
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<GRNTransItemBO> GetPurchaseInvoiceTrans(int purchaseID)
        {
            List<GRNTransItemBO> gRNTransItemBOList = new List<GRNTransItemBO>();
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                var purchaseOrderTrans = dbEntity.SpGetPurchaseInvoiceTransDetails(
                    purchaseID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                ).ToList();
                if (purchaseOrderTrans != null && purchaseOrderTrans.Count() > 0)
                {
                    gRNTransItemBOList = purchaseOrderTrans.MapToBOList();
                }
            }
            return gRNTransItemBOList;
        }
        public List<GRNTransItemBO> GetPurchaseInvoiceTransDetails(int purchaseID)
        {
            List<GRNTransItemBO> gRNTransItemBOList = new List<GRNTransItemBO>();
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                var purchaseOrderTrans = dbEntity.SpGetPurchaseInvoiceTransDetails(
                    purchaseID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                ).ToList();
                if (purchaseOrderTrans != null && purchaseOrderTrans.Count() > 0)
                {
                    gRNTransItemBOList = purchaseOrderTrans.MapToBOList();
                }
            }
            return gRNTransItemBOList;
        }

        public List<PurchaseInvoiceOtherChargeDetailBO> GetPurchaseInvoiceOtherChargeDetails(int purchaseID)
        {
            List<PurchaseInvoiceOtherChargeDetailBO> otherChargeDetailBOList = null;
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                var otherChargeDetailList = dbEntity.SpGetPurchaseInvoiceOtherChargesDetails(purchaseID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                if (otherChargeDetailList != null && otherChargeDetailList.Count() > 0)
                {
                    otherChargeDetailBOList = otherChargeDetailList.MapToBOList();
                }
            }
            return otherChargeDetailBOList;
        }
        public List<PurchaseInvoiceTaxDetailBO> GetPurchaseInvoiceTaxDetails(int purchaseID)
        {
            List<PurchaseInvoiceTaxDetailBO> invoiceTaxDetailBOList = null;
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                var invoiceTaxDetailList = dbEntity.SpGetPurchaseInvoiceTaxDetails(purchaseID).ToList();
                if (invoiceTaxDetailList != null && invoiceTaxDetailList.Count() > 0)
                {
                    invoiceTaxDetailBOList = invoiceTaxDetailList.MapToBOList();
                }
            }
            return invoiceTaxDetailBOList;
        }
        public int Cancel(int ID, string Table)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())

                    return dbEntity.SpCancelTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            try
            {
                ObjectParameter count = new ObjectParameter("count", typeof(int));
                int value;
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    dbEntity.SpGetInvoiceNoCount(Table, Hint, SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, count);
                    value = (int)count.Value;
                    return value;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PurchaseInvoiceBO> GetPurchaseInvoiceDetails(int? ID)
        {
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                return entity.SpGetPurchaseInvoiceDetails(
                    ID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                ).Select(
                    a => new PurchaseInvoiceBO()
                    {
                        Id = a.Id,
                        PurchaseNo = a.PurchaseNo,
                        PurchaseDate = (DateTime)a.PurchaseDate,
                        InvoiceNo = a.InvoiceNo,
                        InvoiceDate = (DateTime)a.InvoiceDate,
                        SupplierID = (int)a.SupplierID,
                        SupplierName = a.SupplierName,
                        NetAmount = (decimal)a.NetAmount,
                        IsDraft = (bool)a.IsDraft,
                        Status = a.Status,
                        IsCancelled = (bool)a.IsCancelled
                    }
                ).ToList();
            }
        }
        public bool Approve(int ID, String Status)
        {
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                try
                {
                    entity.SpUpdatePurchaseInvoiceStatus(ID, Status, GeneralBO.FinYear,
                         GeneralBO.LocationID, GeneralBO.ApplicationID);
                    entity.SpUpdatePayableDueDate(ID, "Stock", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);


                }
                catch (Exception ex)
                {
                    return false;

                }
            }
            return true;
        }
        public List<PurchaseInvoiceBO> GetInvoiceTypeList()
        {
            try
            {
                List<PurchaseInvoiceBO> OrderType = new List<PurchaseInvoiceBO>();
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    OrderType = dbEntity.SpGetPurchaseInvoiceTypeList().Select(a => new PurchaseInvoiceBO
                    {
                        Id = a.ID,
                        Name = a.Name,
                    }).ToList();

                    return OrderType;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }



    public static partial class Mapper
    {
        public static List<GRNTransItemBO> MapToBOList(this List<SpGetPurchaseInvoiceTransDetails_Result> purchaseInvoiceTransDetailList)
        {
            List<GRNTransItemBO> gRNTransItemBOList = new List<GRNTransItemBO>();
            if (purchaseInvoiceTransDetailList != null && purchaseInvoiceTransDetailList.Count() > 0)
            {
                gRNTransItemBOList = (from trans in purchaseInvoiceTransDetailList
                                      select new GRNTransItemBO
                                      {
                                          Id = trans.Id,
                                          PurchaseInvoiceID = trans.PurchaseInvoiceID ?? 0,
                                          GRNID = trans.GRNID ?? 0,
                                          GRNTransID = trans.GRNTransID ?? 0,
                                          ItemID = trans.ItemID ?? 0,
                                          ItemCode = trans.ItemCode,
                                          ItemName = trans.ItemName,
                                          PartsNumber = trans.PartsNumber,
                                          Remark = trans.Remarks,
                                          Model = trans.Model,
                                          IsGST = trans.IsGST ?? 0,
                                          IsVat = trans.IsVat ?? 0,
                                          Unit = trans.Unit,
                                          UnMatchedQty = trans.UnMatchedQty,
                                          InvoiceQty = trans.InvoiceQty ?? 0,
                                          InvoiceRate = trans.InvoiceRate ?? 0,
                                          InvoiceValue = trans.InvoiceValue ?? 0,
                                          AcceptedQty = trans.AcceptedQty,
                                          ApprovedQty = trans.ApprovedQty,
                                          ApprovedValue = (decimal)(trans.ApprovedQty * trans.PORate),
                                          PORate = trans.PORate ?? 0,
                                          Difference = trans.Difference ?? 0,
                                          Remarks = trans.Remarks,
                                          SGSTPercent = (decimal)trans.SGSTPercent,
                                          CGSTPercent = (decimal)trans.CGSTPercent,
                                          IGSTPercent = (decimal)trans.IGSTPercent,
                                          InvoiceGSTPercent = (decimal)trans.InvoiceGSTPercent,
                                          SGSTAmt = (decimal)trans.SGSTAmt,
                                          CGSTAmt = (decimal)trans.CGSTAmt,
                                          IGSTAmt = (decimal)trans.IGSTAmt,
                                          FreightAmt = (decimal)trans.FreightAmt,
                                          OtherCharges = (decimal)trans.OtherCharges,
                                          PackingShippingCharge = trans.PackingShippingCharge,
                                          PurchaseOrderID = trans.PurchaseOrderID,
                                          PurchaseOrderNo = trans.PurchaseOrderNo,
                                          InclusiveGST = trans.InclusiveGST,
                                          MilkPurchaseID = (int)trans.MilkPurchaseID,
                                          UnitID = (int)trans.UnitID,
                                          GrossAmount = trans.GrossAmount ?? 0,
                                          DiscountAmount = (decimal)trans.DiscountAmount,
                                          DiscountPercent = trans.DiscountPercentage ?? 0,
                                          VATAmount = trans.VATAmount,
                                          VATPercentage = trans.VATPercentage ?? 0,
                                          SecondaryUnit = trans.SecondaryUnit,
                                          SecondaryInvoiceQty = trans.SecondaryInvoiceQty,
                                          SecondaryQty = trans.SecondaryInvoiceQty,
                                          SecondaryOfferQty = trans.SecondaryOfferQty,
                                          SecondaryRate = trans.SecondaryRate,
                                          SecondaryUnitSize = trans.SecondaryUnitSize,
                                          NetAmount = (decimal)trans.NetAmount,
                                          BatchNo = trans.BatchNo,
                                          BatchID = trans.BatchID,
                                          PurchaseMRP = (decimal)trans.PurchaseMRP,
                                          RetailMRP = (decimal)trans.RetailMRP,
                                          ProfitRatio = (decimal)trans.NetProfitRatio,
                                          OfferQty = (decimal)trans.OfferQty,
                                          GSTPercent = (int)trans.GSTPercent, //Convert.ToInt16((decimal)trans.SGSTPercent + (decimal)trans.SGSTPercent + (decimal)trans.CGSTPercent),
                                          CessPercent = trans.CessPercentage.HasValue ? trans.CessPercentage.Value : 0,
                                          POLooseQty = (decimal)trans.POLooseQty,
                                          LooseQty = (decimal)trans.GRNLooseQty
                                      }).ToList();
            }

            return gRNTransItemBOList;
        }
        public static List<PurchaseInvoiceOtherChargeDetailBO> MapToBOList(this List<SpGetPurchaseInvoiceOtherChargesDetails_Result> purchaseInvoiceOtherChargeDetailList)
        {
            List<PurchaseInvoiceOtherChargeDetailBO> otherChargeDetailBOList = null;
            if (purchaseInvoiceOtherChargeDetailList != null && purchaseInvoiceOtherChargeDetailList.Count() > 0)
            {
                otherChargeDetailBOList = (from charge in purchaseInvoiceOtherChargeDetailList
                                           select new PurchaseInvoiceOtherChargeDetailBO
                                           {
                                               Id = charge.Id,
                                               PurchaseInvoiceID = charge.PurchaseInvoiceID ?? 0,
                                               PurchaseOrderID = charge.PurchaseOrderID ?? 0,
                                               Particular = charge.Particular,
                                               POValue = charge.POValue ?? 0,
                                               InvoiceValue = charge.InvoiceValue ?? 0,
                                               DifferenceValue = charge.DifferenceValue ?? 0,
                                               Remarks = charge.Remarks,
                                               PurchaseOrderNumber = charge.PurchaseOrderNo
                                           }).ToList();
            }


            return otherChargeDetailBOList;
        }

        public static List<PurchaseInvoiceTaxDetailBO> MapToBOList(this List<SpGetPurchaseInvoiceTaxDetails_Result> purchaseTaxDetailList)
        {
            List<PurchaseInvoiceTaxDetailBO> invoiceTaxDetailBOList = null;
            if (purchaseTaxDetailList != null && purchaseTaxDetailList.Count() > 0)
            {
                invoiceTaxDetailBOList = (from tax in purchaseTaxDetailList
                                          select new PurchaseInvoiceTaxDetailBO
                                          {
                                              Id = tax.Id,
                                              PurchaseInvoiceID = tax.PurchaseInvoiceID ?? 0,
                                              Particular = tax.Particular,
                                              TaxPercentage = tax.TaxPercentage ?? 0,
                                              POValue = tax.POValue ?? 0,
                                              InvoiceValue = tax.InvoiceValue ?? 0,
                                              DifferenceValue = tax.DifferenceValue ?? 0,
                                              Remarks = tax.Remarks,

                                              //CGSTAmt=tax.Particular.ToLower().Contains("cgst")?tax.InvoiceValue??0:0,
                                              //CGSTPercent= tax.Particular.ToLower().Contains("cgst") ? tax.TaxPercentage ?? 0 : 0,
                                              //SGSTAmt= tax.Particular.ToLower().Contains("sgst") ? tax.InvoiceValue ?? 0 : 0,
                                              //SGSTPercent= tax.Particular.ToLower().Contains("sgst") ? tax.TaxPercentage ?? 0 : 0,
                                              //IGSTAmt = tax.Particular.ToLower().Contains("igst") ? tax.InvoiceValue ?? 0 : 0,
                                              //IGSTPercent= tax.Particular.ToLower().Contains("igst") ? tax.TaxPercentage ?? 0 : 0,
                                          }).ToList();

            }
            return invoiceTaxDetailBOList;

        }
    }
}

