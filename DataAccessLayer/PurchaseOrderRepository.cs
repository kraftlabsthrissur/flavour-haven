using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccessLayer
{
    public class PurchaseOrderRepository
    {

        private readonly PurchaseEntities entity;

        public PurchaseOrderRepository()
        {
            entity = new PurchaseEntities();
        }

        public DatatableResultBO GetPurchaseOrderList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string ItemNameHint, string CategoryNameHint, string NetAmtHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetPurchaseOrderList(Type, TransNoHint, TransDateHint, SupplierNameHint, ItemNameHint, CategoryNameHint, NetAmtHint, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                NetAmount = item.NetAmt,
                                ItemName = item.ItemName,
                                CategoryName = item.CategoryName,
                                Status = item.Status,
                                IsCancellable = (item.Status.ToLower() != "processed" && item.Status.ToLower() != "cancelled" && item.Status.ToLower() != "suspended") ? 1 : 0,
                                IsSuspendable = (item.Status.ToLower() != "cancelled" && item.Status.ToLower() != "suspended" && item.Status.ToLower() != "processed") ? 1 : 0,
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

        public PurchaseOrderBO GetPurchaseOrder(int ID)
        {
            PurchaseOrderBO itm = new PurchaseOrderBO();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetPurchaseOrderDetails(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new PurchaseOrderBO
                    {
                        AdvanceAmount = k.AdvanceAmount,
                        CurrencyName = k.CurrencyName,
                        CurrencyCode = k.CurrencyCode,
                        SupplierID = k.SupplierID,
                        SupplierName = k.SupplierName,
                        CountryName = k.CountryName,
                        MobileNo = k.MobileNo,
                        Email = k.Email,
                        AddressLine1 = k.AddressLine1,
                        AddressLine2 = k.AddressLine2,
                        AddressLine3 = k.AddressLine3,
                        BAddressLine1 = k.BAddressLine1,
                        BAddressLine2 = k.BAddressLine2,
                        BAddressLine3 = k.BAddressLine3,
                        AdvancePercentage = k.AdvancePercentage,
                        BillingAddressID = k.BillingAddressID,
                        BillingLocation = k.BillingLocation,
                        Cancelled = k.Cancelled,
                        CancelledDate = k.CancelledDate,
                        CGSTAmt = k.CGSTAmt,
                        IsGST = k.IsGST.HasValue ? k.IsGST.Value : 0,
                        IsVat = k.IsVAT.HasValue ? k.IsVAT.Value : 0,
                        CurrencyExchangeRate = k.CurrencyExchangeRate,
                        CreatedDate = k.CreatedDate,
                        DeliveryWithin = k.DeliveryWithin,
                        FinYear = k.FinYear,
                        FreightAmt = k.freightAmt,
                        IGSTAmt = k.IGSTAmt,
                        GrossAmount = k.GrossAmount,
                        Discount = k.Discount,
                        DiscountPercentage = k.GrossAmount > 0 ? (k.Discount.HasValue ? k.Discount.Value : 0) / k.GrossAmount * 100 : 0,
                        VATAmount = k.VATAmount,
                        VATPercentage = ((k.GrossAmount - (k.Discount.HasValue ? k.Discount.Value : 0))) > 0 ? (k.VATAmount.HasValue ? k.VATAmount.Value : 0) / (k.GrossAmount - (k.Discount.HasValue ? k.Discount.Value : 0)) * 100 : 0,
                        NetAmt = k.NetAmt.HasValue ? k.NetAmt.Value : 0,
                        SuppDocCode = k.SuppDocCode,
                        SuppShipCode = k.SuppShipCode,
                        SuppOtherRemark = k.SuppOtherRemark,
                        SuppQuotNo = k.SuppQuotNo,
                        Shipment = k.Shipment,
                        OrderType = k.OrderType,
                        SuppDocAmount = k.SuppDocAmount,
                        SuppShipAmount = k.SuppShipAmount,
                        SuppOtherCharge = k.SuppOtherCharge,
                        SGSTAmt = k.SGSTAmt,
                        ShippingAddressID = k.ShippingAddressID,
                        GstExtra = k.GSTExtra,
                        ID = k.ID,
                        InclusiveGST = k.InclusiveGST,
                        IsDraft = (bool)k.IsDraft,
                        OrderMet = k.OrderMet,
                        OtherCharges = k.OtherCharges,
                        OtherQuotationIDS = k.OtherQuotationIDS,
                        PackingShippingCharge = k.PackingShippingCharge,
                        PurchaseOrderDate = k.PurchaseOrderDate,
                        PurchaseOrderNo = k.PurchaseOrderNo,
                        Remarks = k.Remarks,
                        SelectedQuotationID = k.SelectedQuotationID,
                        ShipplingLocation = k.ShipplingLocation,
                        ShippingStateID = k.ShippingStateID,
                        ItemCatagory = k.ItemType,
                        ItemName = k.ItemName,
                        PaymentModeID = k.PaymentModeID,
                        PaymentMode = k.PaymentMode,
                        PaymentWithin = (int)k.PaymentWithin,
                        PaymentWithinID = k.PaymentWithinID,
                        StateId = (int)k.SupplierStateID,
                        IsGSTRegistred = (bool)k.IsGSTRegistered,
                        SupplierLocation = k.SupplierLocation,
                        IsApproved = (bool)k.IsApproved,
                        SupplierReferenceNo = k.SupplierReferenceNo,
                        TermsOfPrice = k.TermsOfPrice,
                        IsSuspended = (bool)k.IsSuspended,
                        IsInterCompany = (int)k.IsInterCompany,
                        SalesOrderLocationID = (int)k.SalesOrderLocationID,
                        InterCompanyLocationID = k.InterCompanyLocationID,
                        AmountInWords = k.AmountInWords,
                        MinimumCurrency = k.MinimumCurrency,
                        DecimalPlaces = k.DecimalPlaces,
                    }).FirstOrDefault();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get purchase oreder Trans details by PO ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="FinYear"></param>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        public List<PurchaseOrderTransBO> GetPurchaseOrderItems(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseOrderTransDetails(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseOrderTransBO
                    {
                        Amount = a.Amount,
                        ApplicationID = a.ApplicationID,
                        CGSTAmt = a.CGSTAmt,
                        IGSTAmt = a.IGSTAmt,
                        SGSTAmt = a.SGSTAmt,
                        CGSTPercent = a.CGSTPercent,
                        FinYear = a.FinYear,
                        //IGSTPercent = a.ig,
                        ItemID = a.ItemID,
                        ItemCode = a.ItemCode,
                        Name = a.ItemName,
                        PartsNumber = a.PartsNumber,
                        Model = a.Model,
                        Remark = a.Remark,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        CurrencyName = a.CurrencyName,
                        CurrencyCode = a.CurrencyCode,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        IsVat = a.IsVAT.HasValue ? a.IsVAT.Value : 0,
                        ItemCategoryID = a.ItemCategoryID,
                        LocationID = a.LocationID,
                        Purchased = a.IsPurchased,
                        PurchaseOrderID = a.PurchaseOrderID,
                        QtyMet = a.QtyMet,
                        Quantity = a.Quantity,
                        QtyOrdered = a.QtyOrdered,
                        Rate = a.Rate,
                        MRP = a.Rate,
                        SecondaryRate = a.SecondaryRate,
                        SecondaryQty = a.SecondaryQty,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryUnitSize = a.SecondaryUnitSize,
                        SecondaryUnitList = SecondaryUnitList(a.Unit, a.SecondaryUnits),
                        Discount = a.Discount,
                        DiscountPercent = a.DiscountPercent.HasValue ? a.DiscountPercent.Value : 0,
                        VATAmount = a.VATAmount,
                        VATPercentage = a.VATPercent,
                        GrossAmount = a.Rate * a.Quantity,
                        TaxableAmount = a.NetAmount - ((a.IGSTAmt.HasValue ? a.IGSTAmt.Value : 0) + (a.SGSTAmt.HasValue ? a.SGSTAmt.Value : 0) + (a.CGSTAmt.HasValue ? a.CGSTAmt.Value : 0) + (a.VATAmount.HasValue ? a.VATAmount.Value : 0)),
                        NetAmount = a.NetAmount.HasValue ? a.NetAmount : 0,
                        Remarks = a.Remarks,
                        SGSTPercent = a.SGSTPercent,
                        ID = a.ID,
                        LowestPR = a.LowestPurchaseRate,
                        Unit = a.Unit,
                        PRTransID = a.PRTransID,
                        LastPurchaseRate = a.LastPurchaseRate,
                        QtyInQC = a.QtyInQC,
                        QtyAvailable = a.QtyAvailable,
                        PurchaseRequisitionNo = a.Code,
                        BatchType = a.BatchType,
                        BatchTypeID = a.BatchTypeID == null ? 0 : (int)a.BatchTypeID,
                        FGCategoryID = a.FGCategoryID,
                        UnitID = a.UnitID.HasValue ? a.UnitID.Value : 0,
                        IsSuspended = (bool)a.IsSuspended,
                        Make = a.Make,
                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SecondaryUnitBO> SecondaryUnitList(string Unit, string SecondaryUnits)
        {
            List<SecondaryUnitBO> secondaryUnits = new List<SecondaryUnitBO>();
            SecondaryUnitBO primaryUnitBO = new SecondaryUnitBO();
            primaryUnitBO.Name = Unit;
            primaryUnitBO.PackSize = 1;
            secondaryUnits.Add(primaryUnitBO);
            string[] SecondaryUnitsArray = SecondaryUnits.Split(',');
            for (int i = 0; i < SecondaryUnitsArray.Length; i++)
            {
                var SecondaryUnitItem = SecondaryUnitsArray[i].Split('|'); ;
                if (SecondaryUnitItem.Length > 1)
                {
                    SecondaryUnitBO secondaryUnitBO = new SecondaryUnitBO();
                    var text = SecondaryUnitItem[0];
                    var value = SecondaryUnitItem[1];
                    secondaryUnitBO.Name = text;
                    secondaryUnitBO.PackSize = Convert.ToDecimal(value);
                    secondaryUnits.Add(secondaryUnitBO);
                }
            }
            return secondaryUnits;
        }
        public JSONOutputBO SavePurchaseOrder(PurchaseOrderBO _masterPO, string XMLItems)
        {
            JSONOutputBO output = new JSONOutputBO();

            using (var transaction = entity.Database.BeginTransaction())
            {
                try
                {
                    string FormName = "PurchaseOrder";
                    ObjectParameter POId = new ObjectParameter("PurchaseOrderID", typeof(int));

                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                    if (_masterPO.IsDraft)
                    {
                        FormName = "DraftPurchaseOrder";
                    }

                    var j = entity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    entity.SaveChanges();

                    var i = entity.SpCreatePurchaseOrder(SerialNo.Value.ToString(),
                     _masterPO.PurchaseOrderDate, _masterPO.SupplierID, _masterPO.AdvancePercentage, _masterPO.AdvanceAmount, _masterPO.PaymentModeID,
                     _masterPO.ShippingAddressID, _masterPO.BillingAddressID, _masterPO.InclusiveGST, _masterPO.GstExtra, _masterPO.SelectedQuotationID,
                    _masterPO.OtherQuotationIDS, _masterPO.DeliveryWithin, _masterPO.PaymentWithinID, _masterPO.SGSTAmt, _masterPO.CGSTAmt, _masterPO.IGSTAmt,
                    _masterPO.VATAmount, _masterPO.FreightAmt, _masterPO.OtherCharges, _masterPO.PackingShippingCharge, _masterPO.Discount, _masterPO.NetAmt,
                    _masterPO.GrossAmount, _masterPO.OrderMet, _masterPO.IsDraft, _masterPO.IsGST, _masterPO.IsVat, _masterPO.CurrencyExchangeRate, _masterPO.Remarks,
                    _masterPO.SuppDocCode, _masterPO.SuppShipCode, _masterPO.SuppOtherRemark, _masterPO.SuppQuotNo, _masterPO.Shipment, _masterPO.OrderType,
                    _masterPO.SuppDocAmount, _masterPO.SuppShipAmount, _masterPO.SuppOtherCharge, _masterPO.SupplierReferenceNo,
                    _masterPO.TermsOfPrice, _masterPO.Cancelled, _masterPO.CancelledDate, GeneralBO.CreatedUserID, DateTime.Now,
                    _masterPO.SalesOrderLocationID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, POId);

                    entity.SaveChanges();

                    if (POId.Value != null && Convert.ToInt32(POId.Value) > 0)
                    {
                        entity.SpCreatePurchaseOrderXMLMethod(Convert.ToInt32(POId.Value),
                            _masterPO.IsDraft,
                            XMLItems,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ReturnValue
                            );


                        if (Convert.ToInt16(ReturnValue.Value) == -1)
                        {
                            transaction.Rollback();
                            output.Message = "PR already mapped to another PO";
                            output.Status = "failure";
                            return output;
                        }
                        transaction.Commit();

                        output.Data = new OutputDataBO
                        {
                            ID = Convert.ToInt32(POId.Value),
                            IsDraft = _masterPO.IsDraft,
                            TransNo = SerialNo.Value.ToString()
                        };
                        return output;
                    }
                    else
                    {
                        output.Message = "Failed to create purchase order";
                        output.Status = "failure";
                        return output;
                    };

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    output.Message = "Failed to create purchase order " + ex.ToString();
                    output.Status = "failure";
                    return output;
                }
            }

        }

        public JSONOutputBO UpdatePurchaseOrder(PurchaseOrderBO _masterPO, string XMLItems)
        {
            JSONOutputBO output = new JSONOutputBO();
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (var transaction = entity.Database.BeginTransaction())
            {
                try
                {
                    entity.SpUpdatePurchaseOrder(_masterPO.ID,
                        _masterPO.SupplierID,
                        _masterPO.DeliveryWithin,
                        _masterPO.AdvancePercentage,
                        _masterPO.AdvanceAmount,
                        _masterPO.PurchaseOrderDate,
                        _masterPO.ShippingAddressID,
                        _masterPO.BillingAddressID,
                        _masterPO.SelectedQuotationID,
                        _masterPO.OtherQuotationIDS,
                        _masterPO.IsDraft,
                        _masterPO.Remarks,
                        _masterPO.SuppShipCode,
                        _masterPO.SuppShipCode,
                        _masterPO.SuppOtherRemark,
                        _masterPO.SuppQuotNo,
                        _masterPO.Shipment,
                        _masterPO.OrderType,
                        _masterPO.SuppDocAmount,
                        _masterPO.SuppShipAmount,
                        _masterPO.SuppOtherCharge,
                        _masterPO.SupplierReferenceNo,
                        _masterPO.TermsOfPrice,
                        _masterPO.PaymentModeID,
                        _masterPO.PaymentWithinID,
                        _masterPO.SGSTAmt,
                        _masterPO.CGSTAmt,
                        _masterPO.IGSTAmt,
                        _masterPO.VATAmount,
                        _masterPO.FreightAmt,
                        _masterPO.OtherCharges,
                        _masterPO.PackingShippingCharge,
                        _masterPO.Discount,
                        _masterPO.NetAmt,
                        _masterPO.GrossAmount,
                        _masterPO.SalesOrderLocationID,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                      );

                    entity.SaveChanges();

                    entity.SpCreatePurchaseOrderXMLMethod(_masterPO.ID,
                            _masterPO.IsDraft,
                            XMLItems,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ReturnValue
                            );


                    if (Convert.ToInt16(ReturnValue.Value) == -1)
                    {
                        transaction.Rollback();
                        output.Message = "Some of the items in the purchase requisition already processed already";
                        output.Status = "failure";
                        return output;
                    }

                    transaction.Commit();
                    output.Data = new OutputDataBO
                    {
                        ID = _masterPO.ID,
                        IsDraft = _masterPO.IsDraft
                    };
                    return output;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    output.Message = "Failed to create purchase order " + ex.ToString();
                    output.Status = "failure";
                    return output;
                }
            }

        }

        /// <summary>
        /// Check Invoice number is valid.
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        public bool CheckInvoiceNumberValid(int supplierID, string invoiceNo)
        {
            bool isExists = false;
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                var result = dbEntity.SpValidatePurchaseInvoiceNo(supplierID, invoiceNo).FirstOrDefault();
                isExists = result != null ? result > 0 : false;
            }
            return !isExists;
        }


        public bool IsPOCancellable(int POID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    bool a = (bool)dbEntity.SpIsPOCancellable(POID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().IsCancellable;
                    return a;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CancelPurchaseOrder(int PurchaseOrderID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpCancelPurchaseOrder(PurchaseOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().Value;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal GetRateForInterCompany(int ItemID, string BatchType)
        {
            try
            {
                ObjectParameter ReturnValue = new ObjectParameter("Rate", typeof(decimal));
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var rate = dbEntity.SpGetRateForInterCompanytItem(ItemID, BatchType, ReturnValue);
                    return Convert.ToDecimal(ReturnValue.Value.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //this is not giving full details of product
        public List<PurchaseOrderItemBO> GetUnProcessedPurchaseRequisitionTransForPO(int PurchaseRequisitionID, int SupplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    return dbEntity.spGetUnProcessedPurchaseRequisitionTransByID(PurchaseRequisitionID, SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(itm => new PurchaseOrderItemBO
                    {
                        ID = itm.ItemID,
                        Code = itm.Code,
                        PurchaseRequisitionID = itm.PurchaseRequisitionID,
                        PRTransID = itm.PRTransID,
                        Name = itm.ItemName,
                        UnitID = (int)itm.UnitID,
                        Unit = itm.Unit,
                        LastPR = itm.LastPR,
                        LowestPR = itm.LowestPR,
                        PendingOrderQty = itm.PendingOrderQty,
                        Qty = itm.Qty,
                        QtyUnderQC = itm.QtyWithQC,
                        QtyAvailable = itm.QtyAvailable,
                        RequestedQty = Convert.ToDecimal(itm.RequestedQty) == 0 ? 0 : Convert.ToInt32(itm.RequestedQty),
                        OrderedQty = itm.OrderedQty == null ? 0 : Convert.ToInt32(itm.OrderedQty),
                        GSTCategoryID = itm.GSTCategoryID,
                        GSTPercentage = (decimal)itm.GSTPercentage,
                        BatchType = "",
                        BatchTypeID = 0,
                        FGCategoryID = (int)itm.FGCategoryID

                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RequisitionBO> GetUnProcessedPurchaseRequisitionForPO()
        {
            List<RequisitionBO> itm = new List<RequisitionBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    itm = dbEntity.spGetUnProcessedPurchaseRequisition(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).Select(k => new RequisitionBO
                    {
                        Code = k.Code,
                        ID = k.PurchaseRequisistionID,
                        RequisitionNo = k.Code,
                        FromDepartment = k.FromDepartment,
                        ToDepartment = k.ToDepartment,
                        Date = (DateTime)k.PurchaseRequisistionDate,
                        ItemCategory = k.ItemCateogry
                    }).ToList();

                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IsItemSuppliedBySupplier> IsItemSuppliedBySupplier(string ItemLists, int SupplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    return dbEntity.SpIsItemSuppliedBySupplier(ItemLists, SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new IsItemSuppliedBySupplier
                    {
                        ItemID = (int)a.ItemID,
                        Status = a.CloneStatus
                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SuspendPurchaseOrder(int ID, string Table)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                    int output;
                    dbEntity.SpSuspendTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, ReturnValue);
                    output = (int)ReturnValue.Value;
                    return output;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SuspendPurchaseOrderItem(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                    dbEntity.SpSuspendPurchaseOrderItem(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, ReturnValue);

                    return (int)ReturnValue.Value;
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public List<PurchaseOrderBO> GetOrderTypeList()
        {
            try
            {
                List<PurchaseOrderBO> OrderType = new List<PurchaseOrderBO>();
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    OrderType = dbEntity.SpGetOrderTypeList().Select(a => new PurchaseOrderBO
                    {
                        ID = a.ID,
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
}
