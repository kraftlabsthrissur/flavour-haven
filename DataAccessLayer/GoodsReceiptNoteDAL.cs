using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccessLayer
{
    public class GoodsReceiptNoteDAL
    {

        #region UnProcessed GRN by Supplier
        /// <summary>
        /// Get UnProcessed GRN
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public List<GRNBO> GetUnProcessedGRNBySupplier(int supplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.spGetUnProcessedGRN(supplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Date = a.Date,
                        SupplierID = a.SupplierID,
                        SupplierName = a.SupplierName,
                        LocationID = a.LocationID,
                        Location = a.Location,
                        PurchaseOrderDate = (DateTime)a.PurchaseOrderDate,
                        DeliveryChallanNo = a.DeliveryChallanNo
                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<GRNBO> GetUnProcessedMilkPurchase(int SupplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnProcessedMilkPurchase(SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNBO
                    {
                        ID = a.ID,
                        Code = a.TransNo,
                        Date = (DateTime)a.Date,
                        Quantity = (decimal)a.TotalQty,
                        Amount = (decimal)a.TotalAmount
                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<GRNTransItemBO> GetUnProcessedGRNItems(int grnID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.spGetUnProcessedGRNTrans(grnID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNTransItemBO
                    {
                        GRNID = grnID,            //GRNID not returning from sp
                        GRNTransID = a.GRNTransID,
                        MilkPurchaseID = 0,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        AcceptedQty = a.AcceptedQty,
                        ApprovedQty = a.ApprovedQty,
                        UnMatchedQty = (decimal)a.UnMatchedQty,
                        PORate = (decimal)a.PORate,
                        ApprovedValue = (decimal)(a.ApprovedQty * a.PORate),
                        SGSTPercent = (decimal)a.SGSTPercent,
                        CGSTPercent = (decimal)a.CGSTPercent,
                        IGSTPercent = (decimal)a.IGSTPercent,
                        SGSTAmt = (decimal)a.SGSTAmt,
                        CGSTAmt = (decimal)a.CGSTAmt,
                        IGSTAmt = (decimal)a.IGSTAmt,
                        FreightAmt = (decimal)a.FreightAmt,
                        OtherCharges = (decimal)a.OtherCharges,
                        PackingShippingCharge = a.PackingShippingCharge,
                        PurchaseOrderID = a.PurchaseOrderID,
                        PurchaseOrderNo = a.PurchaseOrderNo,
                        InclusiveGST = a.InclusiveGST
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<GRNTransItemBO> GetUnProcessedMilkItems(int grnID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnProcessedMilkPurchaseByID(grnID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNTransItemBO
                    {
                        GRNID = 0,            //GRNID not returning from sp
                        GRNTransID = 0,
                        MilkPurchaseID = a.ID,
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        AcceptedQty = (decimal)a.TotalQty,
                        ApprovedQty = (decimal)a.TotalQty,
                        UnMatchedQty = (decimal)a.UnmatchedQty,
                        PORate = (decimal)(a.TotalAmount / a.TotalQty),
                        ApprovedValue = (decimal)(a.TotalAmount),
                        SGSTPercent = 0,
                        CGSTPercent = 0,
                        IGSTPercent = 0,
                        SGSTAmt = 0,
                        CGSTAmt = 0,
                        IGSTAmt = 0,
                        FreightAmt = 0,
                        OtherCharges = 0,
                        PackingShippingCharge = 0,
                        PurchaseOrderID = 0,
                        PurchaseOrderNo = "",
                        InclusiveGST = false
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get GRN for Purchase Return
        /// </summary>
        /// <param name="supplierID"></param>

        /// <returns></returns>
        public List<GRNBO> GetGRNForPurchaseReturnBySupplier(int supplierID)
        {
            List<GRNBO> grnBOList = new List<GRNBO>();
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {

                var purchaseReturnGRNs = dbEntity.spGetGRNForPurchaseReturn(supplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                if (purchaseReturnGRNs != null && purchaseReturnGRNs.Count() > 0)
                {
                    grnBOList = (from grn in purchaseReturnGRNs
                                 select new GRNBO
                                 {
                                     ID = grn.ID,
                                     Code = grn.Code,
                                     Date = grn.Date,
                                     SupplierID = grn.SupplierID,
                                     SupplierName = grn.SupplierName,
                                     LocationID = grn.LocationID,
                                     Location = grn.Location
                                 }).ToList();
                }

            }
            return grnBOList;
        }
        #endregion

        public static double RoundUp(double input, int places = 2)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        public DatatableResultBO GetGRNList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string DeliveryChallanNoHint, string DeliveryChallanDateHint, string WarehouseNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetGRNList(Type,
                        TransNoHint,
                        TransDateHint,
                        SupplierNameHint,
                        DeliveryChallanNoHint,
                        DeliveryChallanDateHint,
                        WarehouseNameHint,
                        SortField,
                        SortOrder,
                        Offset,
                        Limit,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).ToList();
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
                                SupplierName = item.SupplierName,
                                WarehouseName = item.WarehouseName,
                                DeliveryChallanNo = item.DeliveryChallanNo,
                                DeliveryChallanDate = item.DeliveryChallanDate == null ? "" : ((DateTime)item.DeliveryChallanDate).ToString("dd-MMM-yyyy"),
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

        public List<PurchaseOrderBO> GetUnProcessedPurchaseOrderForGrn(int SupplierID)
        {
            List<PurchaseOrderBO> itm = new List<PurchaseOrderBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetUnProcessedPurchaseOrder(SupplierID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseOrderBO()
                    {
                        ID = m.ID,
                        PurchaseOrderNo = m.PurchaseOrderNo,
                        PurchaseOrderDate = m.PurchaseOrderDate,
                        SupplierName = m.SupplierName,
                        NetAmt = (decimal)m.NetAmt,
                        RequestedBy = m.RequestedBy

                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PurchaseOrderTransBO> GetUnProcessedPurchaseOrderTransItemForGrn(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var dataList = dbEntity.SpGetUnProcessedPurchaseOrderTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return dataList.Select(m => new PurchaseOrderTransBO()
                    {
                        ID = m.ID,
                        ItemID = m.ItemID,
                        ItemCode = m.ItemCode,
                        Name = m.ItemName,
                        PartsNumber = m.PartsNumber,
                        Model = m.Model,
                        Remark = m.Remark,
                        CurrencyID = m.CurrencyID.HasValue ? m.CurrencyID.Value : 0,
                        CurrencyName = m.CurrencyName,
                        Category = m.Category,
                        PurchaseOrderID = m.PurchaseOrderID,
                        PurchaseOrderNo = m.PurchaseOrderNo,
                        PendingPOQty = (decimal)m.PendingPOQty,
                        Quantity = m.Quantity,
                        QtyTolerancePercent = m.QtyTolerancePercent.HasValue ? (decimal)m.QtyTolerancePercent.Value : 0,
                        Unit = m.Unit,
                        PendingPOSecondaryQty = (decimal)m.PendingPOQty / m.SecondaryUnitSize,
                        SecondaryQty = (decimal)m.Quantity / m.SecondaryUnitSize,
                        SecondaryRate = m.SecondaryRate,
                        SecondaryUnit = m.SecondaryUnit,
                        SecondaryUnitSize = m.SecondaryUnitSize,
                        IsQCRequired = m.IsQCRequired,
                        BatchType = m.BatchType,
                        BatchTypeID = m.BatchTypeID.HasValue ? (int)m.BatchTypeID.Value : 0,
                        UnitID = (int)m.UnitID,
                        Rate = m.Rate,
                        IsGST = m.IsGST.HasValue ? m.IsGST.Value : 0,
                        IsVat = m.IsVAT.HasValue ? m.IsVAT.Value : 0,
                        QtyOrdered = m.Quantity,
                        PackSize = (decimal)m.Packsize,
                        SGSTPercent = m.SGSTPercent,
                        CGSTPercent = m.CGSTPercent,
                        IGSTPercent = m.IGSTPercent,
                        VATAmount = m.VATAmount,
                        VATPercentage = m.VATPercent,
                        Discount = m.Discount,
                        DiscountPercent = m.DiscountPercent.HasValue ? m.DiscountPercent.Value : 0,
                        NetAmount = m.NetAmount,
                        SuppDocAmount = m.SuppDocAmount,
                        SuppShipAmount = m.SuppShipAmount,
                        SuppOtherCharge = m.SuppOtherCharge,
                        BinID = (int)m.BinID,
                        BinCode = m.BinCode,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveGRN(GRNBO grnBO, List<GRNTransItemBO> grnItemBO)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "GRN";
                        ObjectParameter GRNId = new ObjectParameter("goodsReceiptNoteID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                        if (grnBO.IsDraft)
                        {
                            FormName = "DraftGRN";
                        }

                        var i = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        //dbEntity.SaveChanges();
                        string GRNCode = SerialNo.Value.ToString();
                        var j = dbEntity.SpCreateGoodsReceiptNote(
                            SerialNo.Value.ToString(),
                            grnBO.Date,
                            grnBO.SupplierID,
                            grnBO.ReceiptDate,
                            grnBO.DeliveryChallanNo,
                            grnBO.DeliveryChallanDate,
                            grnBO.WarehouseID,
                            grnBO.PurchaseCompleted,
                            grnBO.IsCancelled,
                            grnBO.CancelledDate,
                            grnBO.CreatedUserId,
                            grnBO.CreatedDate,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            GRNId,
                            grnBO.IsDraft
                            );

                        //dbEntity.SaveChanges();
                        var k = 0;
                        int l = 0;
                        string BatchNo;
                        if (GRNId.Value != null)
                        {
                            foreach (var item in grnItemBO)
                            {
                                if (!grnBO.IsDraft && item.IsQCRequired)
                                {
                                    k = dbEntity.SpUpdateSerialNo("QC", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                                }
                                l++;
                                BatchNo = GRNCode + l.ToString().PadLeft(3, '0');
                                dbEntity.SpCreateGoodsReceiptNoteTrans(
                                    Convert.ToInt32(GRNId.Value),
                                    item.PurchaseOrderID,
                                    item.POTransID,
                                    item.ItemID,
                                    item.Batch,
                                    item.ExpiryDate,
                                    item.PurchaseOrderQty,
                                    item.ReceivedQty,
                                    item.QualityCheckQty,
                                    item.AcceptedQty,
                                    item.RejectedQty,
                                    item.ItemOrderPreference,
                                    item.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    GRNCode,
                                    SerialNo.Value.ToString(),
                                    item.IsQCRequired,
                                    BatchNo,
                                    item.QtyTolerance,
                                    item.VATPercentage,
                                    item.CurrencyID,
                                    item.IsGST,
                                    item.IsVat,
                                    item.Model,
                                    item.PartsNumber,
                                    item.ItemName,
                                    item.NetAmount,
                                    item.UnitID,
                                    ReturnValue
                                    );
                                if (Convert.ToInt32(ReturnValue.Value) <= 0)
                                {
                                    // transaction.Rollback();
                                    return false;

                                }
                            }
                        };
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public int CreateGRN(GRNBO grnBO, List<GRNTransItemBO> grnItemBO)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "GRN";
                        ObjectParameter GRNId = new ObjectParameter("goodsReceiptNoteID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                        if (grnBO.IsDraft)
                        {
                            FormName = "DraftGRN";
                        }

                        var i = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        //dbEntity.SaveChanges();
                        string GRNCode = SerialNo.Value.ToString();
                        var j = dbEntity.SpCreateGRN(
                            SerialNo.Value.ToString(),
                            grnBO.Date,
                            grnBO.SupplierID,
                            grnBO.ReceiptDate,
                            grnBO.DeliveryChallanNo,
                            grnBO.DeliveryChallanDate,
                            grnBO.WarehouseID,
                            grnBO.PurchaseCompleted,
                            grnBO.IsCancelled,
                            grnBO.CancelledDate,
                            grnBO.CreatedUserId,
                            grnBO.CreatedDate,
                            grnBO.IGSTAmt,
                            grnBO.SGSTAmt,
                            grnBO.CGSTAmt,
                            grnBO.RoundOff,
                            grnBO.DiscountAmt,
                            grnBO.GrossAmt,
                            grnBO.VATAmount,
                            grnBO.SuppDocAmount,
                            grnBO.SuppOtherCharges,
                            grnBO.SuppShipAmount,
                            grnBO.CurrencyExchangeRate,
                            grnBO.PackingForwarding,
                            grnBO.SuppFreight,
                            grnBO.LocalCustomsDuty,
                            grnBO.LocalFreight,
                            grnBO.LocalMiscCharge,
                            grnBO.LocalOtherCharges,
                            grnBO.Remarks,
                            grnBO.NetAmount,
                            grnBO.IsCheckedDirectInvoice,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            GRNId,
                            grnBO.IsDraft
                            );


                        if (GRNId.Value != null)
                        {
                            foreach (var item in grnItemBO)
                            {


                                dbEntity.SpCreateGRNTrans(
                                    Convert.ToInt32(GRNId.Value),
                                    item.PurchaseOrderID,
                                    item.POTransID,
                                    item.ItemID,
                                    item.Batch,
                                    item.ExpiryDate,
                                    item.PurchaseOrderQty,
                                    item.ReceivedQty,
                                    item.LooseRate,
                                    item.LooseQty,
                                    item.Remarks,
                                    item.PurchaseRate,
                                    item.OfferQty,
                                    item.DiscountID,
                                    item.DiscountPercent,
                                    item.DiscountAmount,
                                    item.BatchID,
                                    item.IGSTPercent,
                                    item.CGSTPercent,
                                    item.SGSTPercent,
                                    item.IGSTAmt,
                                    item.SGSTAmt,
                                    item.CGSTAmt,
                                    grnBO.IsCheckedDirectInvoice,
                                    item.VATAmount,
                                    item.VATPercentage,
                                    item.TaxableAmount,
                                    item.CurrencyID,
                                    item.IsGST,
                                    item.IsVat,
                                    item.Model,
                                    item.Remark,
                                    item.PartsNumber,
                                    item.ItemName,
                                    item.NetAmount,
                                    item.SecondaryUnit,
                                    item.SecondaryRate,
                                    item.SecondaryUnitSize,
                                    item.LandingCost,
                                 
                                    grnBO.CurrencyExchangeRate,

                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    item.UnitID,
                                    item.BinCode,
                                    item.PurchaseOrderNo,
                                    ReturnValue
                                    );
                                if (Convert.ToInt32(ReturnValue.Value) <= 0)
                                {
                                    transaction.Rollback();
                                    return 0;

                                }
                            }
                        };
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return Convert.ToInt32(GRNId.Value);
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public bool UpdateGRN(GRNBO grnBO, List<GRNTransItemBO> grnItemBO)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                        //string GRNCode = SerialNo.Value.ToString();
                        string GRNCode = grnBO.Code;
                        var j = dbEntity.SpUpdateGoodsReceiptNote(
                                grnBO.ID,
                                grnBO.Date,
                                grnBO.SupplierID,
                                grnBO.ReceiptDate,
                                grnBO.DeliveryChallanNo,
                                grnBO.DeliveryChallanDate,
                                grnBO.WarehouseID,
                                grnBO.PurchaseCompleted,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                grnBO.IsDraft
                                );

                        //dbEntity.SaveChanges();
                        var k = 0;
                        int l = 0;
                        string BatchNo;
                        foreach (var item in grnItemBO)
                        {
                            k = dbEntity.SpUpdateSerialNo("QC", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                            l++;
                            BatchNo = GRNCode + l.ToString().PadLeft(3, '0');
                            dbEntity.SpCreateGoodsReceiptNoteTrans(
                                Convert.ToInt32(grnBO.ID),
                                item.PurchaseOrderID,
                                item.POTransID,
                                item.ItemID,
                                item.Batch,
                                item.ExpiryDate,
                                item.PurchaseOrderQty,
                                item.ReceivedQty,
                                item.QualityCheckQty,
                                item.AcceptedQty,
                                item.RejectedQty,
                                item.ItemOrderPreference,
                                item.Remarks,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                GRNCode,
                                SerialNo.Value.ToString(),
                                item.IsQCRequired,
                                BatchNo,
                                item.QtyTolerance,
                                item.VATPercentage,
                                item.CurrencyID,
                                item.IsGST,
                                item.IsVat,
                                item.Model,
                                item.PartsNumber,
                                item.ItemName,
                                item.NetAmount,
                                item.UnitID,
                                ReturnValue
                                );
                            if (Convert.ToInt32(ReturnValue.Value) <= 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        };
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int UpdateGoodsReceiptNote(GRNBO grnBO, List<GRNTransItemBO> grnItemBO)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                        //string GRNCode = SerialNo.Value.ToString();
                        string GRNCode = grnBO.Code;
                        var j = dbEntity.SpUpdateGRN(
                                grnBO.ID,
                                grnBO.Date,
                                grnBO.SupplierID,
                                grnBO.ReceiptDate,
                                grnBO.DeliveryChallanNo,
                                grnBO.DeliveryChallanDate,
                                grnBO.WarehouseID,
                                grnBO.PurchaseCompleted,
                                GeneralBO.CreatedUserID,
                                grnBO.IGSTAmt,
                                grnBO.SGSTAmt,
                                grnBO.CGSTAmt,
                                grnBO.RoundOff,
                                grnBO.DiscountAmt,
                                grnBO.GrossAmt,
                                grnBO.VATAmount,
                                grnBO.SuppDocAmount,
                                grnBO.SuppOtherCharges,
                                grnBO.SuppShipAmount,
                                grnBO.NetAmount,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                grnBO.IsDraft
                                );

                        foreach (var item in grnItemBO)
                        {


                            dbEntity.SpCreateGRNTrans(
                                  grnBO.ID,
                                  item.PurchaseOrderID,
                                  item.POTransID,
                                  item.ItemID,
                                  item.Batch,
                                  item.ExpiryDate,
                                  item.PurchaseOrderQty,
                                  item.ReceivedQty,
                                  item.LooseRate,
                                  item.LooseQty,
                                  item.Remarks,
                                  item.PurchaseRate,
                                  item.OfferQty,
                                  item.DiscountID,
                                  item.DiscountPercent,
                                  item.DiscountAmount,
                                  item.BatchID,
                                  item.IGSTPercent,
                                  item.CGSTPercent,
                                  item.SGSTPercent,
                                  item.IGSTAmt,
                                  item.SGSTAmt,
                                  item.CGSTAmt,
                                  grnBO.IsCheckedDirectInvoice,
                                  item.VATAmount,
                                  item.VATPercentage,
                                  item.TaxableAmount,
                                  item.CurrencyID,
                                  item.IsGST,
                                  item.IsVat,
                                  item.Model,
                                  item.Remark,
                                  item.PartsNumber,
                                  item.ItemName,
                                  item.NetAmount,
                                  item.SecondaryUnit,
                                  item.SecondaryRate,
                                  item.SecondaryUnitSize,
                                  item.LandingCost,
                                  grnBO.CurrencyExchangeRate,
                                  GeneralBO.FinYear,
                                  GeneralBO.LocationID,
                                  GeneralBO.ApplicationID,
                                  item.UnitID,
                                  item.BinCode,
                                  item.PurchaseOrderNo,
                                  ReturnValue
                                  );
                            if (Convert.ToInt32(ReturnValue.Value) <= 0)
                            {
                                // transaction.Rollback();
                                return 0;

                            }
                        };
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return grnBO.ID;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<GRNBO> GetGoodsReceiptNoteDetails(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetGoodsReceiptNoteDetails(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new GRNBO()
                    {
                        ID = m.ID,
                        Code = m.GrnNo,
                        SupplierID = m.SupplierID,
                        SupplierName = m.Supplier,
                        WarehouseID = m.WarehouseID,
                        WarehouseName = m.Warehouse,
                        ReceiptDate = m.ReceiptDate,
                        DeliveryChallanDate = m.DeliveryChallanDate,
                        DeliveryChallanNo = m.DeliveryChallanNo,
                        PurchaseCompleted = m.PurchaseCompleted,
                        IsDraft = (bool)m.IsDraft,
                        PurchaseOrderDate = (DateTime)m.PurchaseOrderDate,
                        SupplierCode = m.SupplierCode,
                        IsCancelled = m.Cancelled




                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GRNTransItemBO> GetGoodsRecieptNoteItems(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetGoodsReceiptNoteTransDetails(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new GRNTransItemBO()
                    {
                        ItemID = m.ItemID,
                        ItemName = m.Item,
                        Batch = m.Batch == null ? " " : m.Batch,
                        ExpiryDate = m.ExpiryDate,
                        PurchaseOrderQty = m.PurchaseOrderQty,
                        ReceivedQty = m.ReceivedQty,
                        QualityCheckQty = m.QualityCheckQty,
                        AcceptedQty = m.AcceptedQty,
                        PendingPOQty = (decimal)m.PendingPOQuantity,
                        PurchaseOrderNo = m.PurchaseorderNo,
                        Unit = m.Unit,
                        BatchType = m.BatchType,
                        PurchaseOrderID = m.PurchaseOrderID,
                        POTransID = m.PurchaseOrderTansID,
                        PurchaseOrderQuantity = (decimal)m.Quantity,
                        QtyTolerancePercent = (decimal)m.QtyTolerancePercent,
                        IsQCRequired = m.IsQCRequired,
                        ItemCategory = m.ItemCategory,
                        AllowedQty = (decimal)m.AllowedQty,
                        Remarks = m.Remarks,
                        Quantity = (decimal)m.Quantity,
                        UnitID = (int)m.UnitID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<GRNBO> GetGRNNoWithItemID(int itemID)
        {
            List<GRNBO> item = new List<GRNBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    item = dbEntity.SpGetGRNNoWithItemID(itemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNBO
                    {
                        ID = a.ID,
                        Code = a.Code

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GRNBO> GetGrnInvoiceAutoComplete(string term)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetGRNInvoiceNoAutoComplete(term, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.LocationID).Select(a => new GRNBO
                    {
                        ID = a.ID,
                        Code = a.DeliveryChallanNo

                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Cancel(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpCancelGRN(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
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

        public List<GRNBO> GetGRNDetail(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var data = dbEntity.SpGetGRNDetail(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return data.Select(m => new GRNBO()
                    {
                        ID = m.ID,
                        Code = m.GrnNo,
                        SupplierID = m.SupplierID,
                        SupplierName = m.Supplier,
                        AddressLine1 = m.AddressLine1,
                        AddressLine2 = m.AddressLine2,
                        AddressLine3 = m.AddressLine3,
                        BAddressLine1 = m.BAddressLine1,
                        BAddressLine2 = m.BAddressLine2,
                        BAddressLine3 = m.BAddressLine3,
                        CountryName = m.CountryName,
                        Email = m.Email,
                        CurrencyName = m.CurrencyName,
                        WarehouseID = m.WarehouseID,
                        WarehouseName = m.Warehouse,
                        ReceiptDate = m.ReceiptDate,
                        DeliveryChallanDate = m.DeliveryChallanDate,
                        DeliveryChallanNo = m.DeliveryChallanNo,
                        PurchaseCompleted = m.PurchaseCompleted,
                        IsDraft = (bool)m.IsDraft,
                        PurchaseOrderDate = m.PurchaseOrderDate,
                        SupplierCode = m.SupplierCode,
                        IsCancelled = m.Cancelled,
                        IGSTAmt = (decimal)m.IGSTAmt,
                        CGSTAmt = (decimal)m.CGSTAmt,
                        SGSTAmt = (decimal)m.SGSTAmt,
                        RoundOff = (decimal)m.RoundOff,
                        GrossAmt = (decimal)m.GrossAmount,
                        VATAmount = m.VATAmount,
                        SuppDocAmount = m.SuppDocAmount,
                        SuppShipAmount = m.SuppShipAmount,
                        SuppOtherCharges = m.SuppOtherCharge,
                        SuppFreight = m.SuppFreight,
                        PackingForwarding = m.PackingForwarding,
                        LocalCustomsDuty = m.LocalCustomsDuty,
                        LocalMiscCharge = m.LocalMiscCharge,
                        LocalFreight = m.LocalFreight,
                        LocalOtherCharges = m.LocalOtherCharges,
                        NetAmount = (decimal)m.NetAmount,
                        DiscountAmt = (decimal)m.DiscountAmount,
                        PurchaseOrderNo = m.PurchaseOrderNo,
                        Remarks = m.Remarks,
                        currencycode=m.currencycode,
                        CurrencyCodeL = m.CurrencyCodeL,
                        SuuplierCurrencyconverion = m.SuuplierCurrencyconverion
                       
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GRNBO> GetGRNDetailV3(int[] ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    string CommaSeperatedIDs = string.Join(",", ID.Select(x => x.ToString()).ToArray());
                    return dbEntity.SpGetGRNDetailV3(CommaSeperatedIDs, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new GRNBO()
                    {
                        ID = m.ID,
                        Code = m.GrnNo,
                        SupplierID = m.SupplierID,
                        SupplierName = m.Supplier,
                        PurchaseOrderDate = (DateTime)m.PurchaseOrderDate,
                        SupplierCode = m.SupplierCode,
                        IGSTAmt = (decimal)m.IGSTAmt,
                        CGSTAmt = (decimal)m.CGSTAmt,
                        SGSTAmt = (decimal)m.SGSTAmt,
                        RoundOff = (decimal)m.RoundOff,
                        GrossAmt = (decimal)m.GrossAmount,
                        NetAmount = (decimal)m.NetAmount,
                        DiscountAmt = (decimal)m.DiscountAmount
                        
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GRNTransItemBO> GetGRNItems(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var DataList = dbEntity.SpGetGRNTransDetails(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return DataList.Select(m => new GRNTransItemBO()
                    {
                        ItemID = m.ItemID,
                        ItemCode = m.ItemCode,
                        ItemName = m.ItemName,
                        PartsNumber = m.PartsNumber,
                        Remark = m.Remark,
                        Model = m.Model,
                        
                        Batch = m.Batch == null ? " " : m.Batch,
                        ExpiryDate = m.ExpiryDate,
                        //PurchaseOrderQty = m.PurchaseOrderQty.HasValue ? m.PurchaseOrderQty.Value : 0,
                        PurchaseOrderQty = m.PurchaseOrderQty,
                        ReceivedQty = m.ReceivedQty,
                        Unit = m.Unit,
                        SecondaryUnit = m.SecondaryUnit,
                        SecondaryRate = m.SecondaryRate,
                        SecondaryUnitSize = m.SecondaryUnitSize,
                        //SecondaryPurchaseOrderQty = m.PurchaseOrderQty.HasValue && m.SecondaryUnitSize > 0 ? m.PurchaseOrderQty.Value / m.SecondaryUnitSize : 0,
                        SecondaryPurchaseOrderQty = m.PurchaseOrderQty / m.SecondaryUnitSize,
                        SecondaryReceivedQty = m.SecondaryUnitSize > 0 ? m.ReceivedQty / m.SecondaryUnitSize : 0,
                        SecondaryOfferQty = m.OfferQty.HasValue && m.SecondaryUnitSize > 0 ? m.OfferQty.Value / m.SecondaryUnitSize : 0,
                        PurchaseOrderID = m.PurchaseOrderID,
                        POTransID = m.PurchaseOrderTansID,
                        Remarks = m.Remarks,
                        CurrencyID = m.CurrencyID.HasValue ? m.CurrencyID.Value : 0,
                        UnitID = m.UnitID.HasValue ? m.UnitID.Value : 0,
                        LooseQty = m.LooseQty.HasValue ? m.LooseQty.Value : 0,
                        LooseRate = m.LooseRate.HasValue ? m.LooseRate.Value : 0,
                        PurchaseRate = m.PurchaseRate.HasValue ? m.PurchaseRate.Value : 0,
                        OfferQty = m.OfferQty.HasValue ? m.OfferQty.Value : 0,
                        GrossAmount = m.GrossAmount.HasValue ? m.GrossAmount.Value : 0,
                        DiscountAmount = m.DiscountAmount.HasValue ? m.DiscountAmount.Value : 0,
                        DiscountPercent = m.DiscPercent.HasValue ? m.DiscPercent.Value : 0,
                        //RetailMRP = m.RetailMRP ? m.RetailMRP.Value : 0,
                        RetailMRP = m.RetailMRP,
                        NetAmount = m.NetPurchasePrice.HasValue ? m.NetPurchasePrice.Value : 0,
                        BatchID = m.BatchID,
                        PendingPOQty = m.PendingQty.HasValue ? m.PendingQty.Value : 0,
                        DiscountID = m.DiscountID.HasValue ? m.DiscountID.Value : 0,
                        PackSize = m.PackSize.HasValue ? m.PackSize.Value : 0,
                        SGSTPercent = m.SGSTPercent,
                        CGSTPercent = m.CGSTPercent,
                        IGSTPercent = m.IGSTPercent,
                        VATPercentage = m.VATPercentage.HasValue ? m.VATPercentage.Value : 0,
                        VATAmount = m.VATAmount,
                        TaxableAmount = m.TaxableAmount,
                        GSTPercentage = m.GSTPercentage.HasValue ? m.GSTPercentage.Value : 0,
                        IGSTAmt = m.IGSTAmt,
                        SGSTAmt = m.SGSTAmt,
                        CGSTAmt = m.CGSTAmt,
                        QtyTolerancePercent = m.QtyTolerancePercent.HasValue ? m.QtyTolerancePercent.Value : 0,
                        Make=m.Make,
                        PurchaseOrderNo = m.PurchaseOrderNo,
                        BinCode = m.BinCode,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //For  GetGRNItemsForPurchaseInvoice FOR Allopathy(In Ayurware GetUnProcessedGRNItems)

        public List<GRNTransItemBO> GetGRNItemsForPurchaseInvoice(int grnID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var dataList = dbEntity.SpGetGRNItemsForPurchaseInvoice(grnID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return dataList.Select(a => new GRNTransItemBO
                    {
                        GRNID = grnID,            //GRNID not returning from sp
                        GRNTransID = a.GRNTransID,
                        MilkPurchaseID = 0,
                        ItemID = a.ItemID,
                        ItemCode = a.ItemCode,
                        ItemName = a.ItemName,
                        PartsNumber = a.PartsNumber,
                        Remark = a.Remark,
                        Model = a.Model,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        IsVat = a.IsVat.HasValue ? a.IsVat.Value : 0,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        SGSTPercent = (decimal)a.SGSTPercent,
                        CGSTPercent = (decimal)a.CGSTPercent,
                        IGSTPercent = (decimal)a.IGSTPercent,
                        SGSTAmt = (decimal)a.SGSTAmt,
                        CGSTAmt = (decimal)a.CGSTAmt,
                        IGSTAmt = (decimal)a.IGSTAmt,
                        PurchaseOrderID = a.PurchaseOrderID,
                        PurchaseOrderNo = a.PurchaseOrderNo,
                        InclusiveGST = a.InclusiveGST,
                        InvoiceRate = (decimal)a.PurchaseRate,
                        OfferQty = (decimal)a.OfferQty,
                        ProfitPrice = (decimal)a.ProfitPrice,
                        ExchangeRate = a.ExchangeRate,
                        GrossAmount = a.GrossAmount.HasValue ? a.GrossAmount.Value : 0,
                        DiscountAmount = a.DiscountAmount.HasValue ? a.DiscountAmount.Value : 0,
                        InvoiceValue = (decimal)a.InvoiceAmount,
                        NetAmount = (decimal)a.NetAmount,
                        DiscountPercent = a.DiscountPercentage.HasValue ? a.DiscountPercentage.Value : 0,
                        VATAmount = a.VATAmount,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        SuppDocAmount = a.SuppDocAmount,
                        SuppOtherCharge = a.SuppOtherCharge,
                        SuppShipAmount = a.SuppShipAmount,
                        SuppFreight = a.SuppFreight,
                        LocalCustomsDuty = a.LocalCustomsDuty,
                        LocalFreight = a.LocalFreight,
                        LocalMiscCharge = a.LocalMiscCharge,
                        LocalOtherCharges = a.LocalOtherCharges,
                        PackingForwarding = a.PackingForwarding,
                        SecondaryRate = a.SecondaryRate,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryUnitSize = a.SecondaryUnitSize,
                        SecondaryInvoiceQty = a.SecondaryUnitSize > 0 ? a.ReceivedQty / a.SecondaryUnitSize : 0,
                        InvoiceQty = a.ReceivedQty,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        PurchaseMRP = (decimal)a.PurchaseMRP,
                        RetailMRP = (decimal)a.RetailMRP,
                        ProfitRatio = (decimal)a.NetProfitRatio,
                        GSTPercent = (int)a.GSTPercent,
                        CessPercent = a.CessPercentage.HasValue ? (decimal)a.CessPercentage.Value : 0,
                        CurrentProfitTolerance = a.CurrentProfitTolerance.HasValue ? (decimal)a.CurrentProfitTolerance.Value : 0,
                        PrevoiusBatchNetProfitRatio = (decimal)a.PrevoiusBatchNetProfitRatio,
                        LooseQty = (decimal)a.GRNLooseQty,
                        POLooseQty = (decimal)a.POLooseQty

                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<GRNBO> GetUnProcessedGRN(string Hint)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.spGetUnProcessedGRNForAllopathy(Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Date = a.Date,
                        SupplierID = a.SupplierID,
                        SupplierName = a.SupplierName,
                        LocationID = a.LocationID,
                        Location = a.Location,
                        PurchaseOrderDate = (DateTime)a.PurchaseOrderDate,
                        DeliveryChallanNo = a.DeliveryChallanNo,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        StateID = a.StateID
                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<GRNBO> GetUnProcessedGRNV3(int SupplierID, string Hint)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnProcessedGRNBySupplierIDForAllopathyV3(SupplierID, Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Date = a.Date,
                        SupplierID = a.SupplierID,
                        SupplierName = a.SupplierName,
                        LocationID = a.LocationID,
                        Location = a.Location,
                        PurchaseOrderDate = (DateTime)a.PurchaseOrderDate,
                        DeliveryChallanNo = a.DeliveryChallanNo,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        StateID = a.StateID,
                        NetAmount = a.NetAmount ?? 0
                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<GRNBO> GetUnProcessedGRNV4(int BusinessCategoryID, string Hint)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnProcessedGRNForAllopathyV4(Hint, BusinessCategoryID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Date = a.Date,
                        SupplierID = a.SupplierID,
                        SupplierName = a.SupplierName,
                        LocationID = a.LocationID,
                        Location = a.Location,
                        PurchaseOrderDate = (DateTime)a.PurchaseOrderDate,
                        DeliveryChallanNo = a.DeliveryChallanNo,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        StateID = a.StateID
                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<GRNTransItemBO> GetItemForQRCodeGenerator(int GRNID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetItemForQRCodeGenerator(GRNID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GRNTransItemBO()
                    {
                        ItemID = a.ItemID,
                        ItemCode = a.ItemCode,
                        ItemName = a.ItemName,
                        Batch = a.BatchNo,
                        BatchID = a.BatchID,
                        RetailMRP = (decimal)a.RetailMRP,
                        LooseRate = (decimal)a.RetailLooseRate,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveQRCode(List<GRNBO> QRCodeList)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                try
                {
                    if (QRCodeList != null)
                    {
                        foreach (var item in QRCodeList)
                        {
                            dbEntity.SpCreateQRCode(
                               item.BatchID,
                               item.ItemID,
                               item.QRCode
                                );
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return 1;
        }

        public int CreateGRNV4(GRNBO grnBO, List<GRNTransItemBO> grnItemBO)
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "GRN";
                        ObjectParameter GRNId = new ObjectParameter("goodsReceiptNoteID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                        if (grnBO.IsDraft)
                        {
                            FormName = "DraftGRN";
                        }

                        var i = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        //dbEntity.SaveChanges();
                        string GRNCode = SerialNo.Value.ToString();
                        var j = dbEntity.SpCreateGRN(
                            SerialNo.Value.ToString(),
                            grnBO.Date,
                            grnBO.SupplierID,
                            grnBO.ReceiptDate,
                            grnBO.DeliveryChallanNo,
                            grnBO.DeliveryChallanDate,
                            grnBO.WarehouseID,
                            grnBO.PurchaseCompleted,
                            grnBO.IsCancelled,
                            grnBO.CancelledDate,
                            grnBO.CreatedUserId,
                            grnBO.CreatedDate,
                            grnBO.IGSTAmt,
                            grnBO.SGSTAmt,
                            grnBO.CGSTAmt,
                            grnBO.RoundOff,
                            grnBO.DiscountAmt,
                            grnBO.GrossAmt,
                            grnBO.VATAmount,
                            grnBO.SuppDocAmount,
                            grnBO.SuppOtherCharges,
                            grnBO.SuppShipAmount,
                            grnBO.CurrencyExchangeRate,
                            grnBO.PackingForwarding,
                            grnBO.SuppFreight,
                            grnBO.LocalCustomsDuty,
                            grnBO.LocalFreight,
                            grnBO.LocalMiscCharge,
                            grnBO.LocalOtherCharges,
                            grnBO.Remarks,
                            grnBO.NetAmount,
                            grnBO.IsCheckedDirectInvoice,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            GRNId,
                            grnBO.IsDraft
                            );


                        if (GRNId.Value != null)
                        {
                            foreach (var item in grnItemBO)
                            {


                                dbEntity.SpCreateGRNTransForJanaushadhi(
                                    Convert.ToInt32(GRNId.Value),
                                    item.PurchaseOrderID,
                                    item.POTransID,
                                    item.ItemID,
                                    item.Batch,
                                    item.ExpiryDate,
                                    item.PurchaseOrderQty,
                                    item.ReceivedQty,
                                    item.LooseRate,
                                    item.LooseQty,
                                    item.Remarks,
                                    item.PurchaseRate,
                                    item.OfferQty,
                                    item.DiscountID,
                                    item.DiscountPercent,
                                    item.DiscountAmount,
                                    item.BatchID,
                                    item.IGSTPercent,
                                    item.CGSTPercent,
                                    item.SGSTPercent,
                                    item.IGSTAmt,
                                    item.SGSTAmt,
                                    item.CGSTAmt,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    item.UnitID,
                                    ReturnValue
                                    );
                                if (Convert.ToInt32(ReturnValue.Value) <= 0)
                                {
                                    // transaction.Rollback();
                                    return 0;

                                }
                            }
                        };
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return Convert.ToInt32(GRNId.Value);
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<PurchaseOrderBO> GetUnProcessedPurchaseOrderForGrnV4(int SupplierID, int BusinessCategoryID)
        {
            List<PurchaseOrderBO> itm = new List<PurchaseOrderBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetUnProcessedPurchaseOrderV4(SupplierID, BusinessCategoryID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseOrderBO()
                    {
                        ID = m.ID,
                        PurchaseOrderNo = m.PurchaseOrderNo,
                        PurchaseOrderDate = m.PurchaseOrderDate,
                        SupplierName = m.SupplierName,
                        NetAmt = (decimal)m.NetAmt,
                        RequestedBy = m.RequestedBy

                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool IsBarCodeGenerator()
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                try
                {
                    ObjectParameter IsDotMatrixPrint = new ObjectParameter("IsBarCodeGenerator", typeof(bool));
                    dbEntity.SpGetBarCodeConfiguration(GeneralBO.ApplicationID, IsDotMatrixPrint);
                    return Convert.ToBoolean(IsDotMatrixPrint.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool IsDirectPurchaseInvoice()
        {
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                try
                {
                    ObjectParameter IsDirectPurchaseInvoice = new ObjectParameter("IsDirectPurchaseInvoice", typeof(bool));
                    dbEntity.SpGetDirectPurchaseInvoiceConfiguration(GeneralBO.ApplicationID, IsDirectPurchaseInvoice);
                    return Convert.ToBoolean(IsDirectPurchaseInvoice.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


        public List<GRNTransItemBO> GetGRNPrintPDF(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var data = dbEntity.SpGetGoodsReceiptNotePrintPDF(ID).ToList();

                    return data.Select(m => new GRNTransItemBO()
                    {
                        ItemID = m.ItemID,
                        Item = m.Item,
                        UnitID = (int)m.UnitID,
                        Unit = m.Unit,
                        Batch = m.Batch,
                        ExpiryDate = m.ExpiryDate,
                        PurchaseOrderQty = (decimal)m.PurchaseOrderQty,
                        PurchaseRate = (decimal)m.PurchaseRate,
                        ReceivedQty = m.ReceivedQty,
                        DiscountPercent = (decimal)m.DiscPercent,
                        DiscountAmount = (decimal)m.DiscountAmount,
                        CGSTPercent = m.CGSTPercent,
                        SGSTPercent = m.SGSTPercent,
                        CGSTAmt = m.CGSTAmt,
                        SGSTAmt = m.SGSTAmt,
                        LooseQty = (decimal)m.LooseQty,
                        MRP = (decimal)m.MRP,
                        GrnNo = m.GrnNo,
                        SupplierName = m.SupplierName,
                        GSTNo = m.GSTNo,
                        SupplierLocation = m.SupplierLocation,
                        NetAmount = (decimal)m.NetAmount,
                        Date = m.Date

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<GRNTransItemBO> GetBatchListForQRCodePrint(int ItemID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetBatchListForQRCodePrint(ItemID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new GRNTransItemBO()
                    {
                        ItemID = m.ItemID,
                        ItemCode = m.ItemCode,
                        ItemName = m.ItemName,
                        Batch = m.Batch == null ? " " : m.Batch,
                        BatchID = m.BatchID,
                        RetailMRP = m.RetailMRP,
                        ExpiryDate = m.ExpiryDate,
                        Stock = (decimal)m.Stock,
                        PrintingQty = (decimal)m.PrintingQty,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}

