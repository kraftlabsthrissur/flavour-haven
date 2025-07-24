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
    public class SalesInquiryDAL
    {
        public List<SalesItemBO> GetInqueryCustomerAutoComplete(string CustomerName)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var getData = dbEntity.SpGetInqueryCustomer(CustomerName).ToList();
                    return getData.Select(a => new SalesItemBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public SalesInquiryBO GetSalesInauiry(int SalesInquiryID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetInquiry(SalesInquiryID, GeneralBO.LocationID).Select(a => new SalesInquiryBO()
                    {
                        ID = a.ID,
                        IsDraft = a.IsDraft ?? false,
                        SalesInquiryNo = a.SalesInquiryNo,
                        SalesInquiryDate = a.SalesInquiryDate,
                        RequestedDelivaryDate = a.RequestedDelivaryDate,
                        RequestExpiryDate = a.RequestExpiryDate,
                        RequestedCustomerName = a.RequestedCustomerName,
                        RequestedCustomerAddress = a.RequestedCustomerAddress,
                        PhoneNo1 = a.PhoneNo1,
                        PhoneNo2 = a.PhoneNo2,
                        Make = a.Make,
                        Model = a.Model,
                        Year = a.Year,
                        SIOrVINNumber = a.SIOrVINNumber,
                        GrossAmount = (decimal)a.GrossAmount,
                        NetAmount = (decimal)a.NetAmount,
                        Remarks = a.Remarks
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SalesItemBO> GetSalesInquiryItemsPurchaseRequisition(int SalesInquiryID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var data = dbEntity.SpGetSalesInquiryItemsPurchaseRequisition(SalesInquiryID, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return data.Select(a => new SalesItemBO()
                    {
                        SalesInquiryItemID = a.SalesInquiryItemID,
                        ItemID = a.ItemID,
                        Code = a.ItemCode,
                        Name = a.ItemName,
                        PartsNumber = a.PartsNumber,
                        Remarks = a.Remarks,
                        Model = a.Model,
                        DeliveryTerm = a.DeliveryTerm,
                        UnitName = a.UnitName,
                        UnitID = a.UnitID.HasValue ? a.UnitID.Value : 0,
                        Qty = (decimal)a.Quantity,
                        Rate = (decimal)a.PurchaseMRP,
                        GrossAmount = (decimal)a.GrossAmount,
                        VATAmount = a.VATAmount.HasValue ? a.VATAmount.Value : 0,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        NetAmount = (decimal)a.NetAmount
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SalesItemBO> GetSalesInauiryItems(int SalesInquiryID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetSalesInquiryItems(SalesInquiryID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                    {
                        SalesInquiryID = (int)a.SalesInquiryID,
                        ItemID = a.ItemID.HasValue ? a.ItemID.Value : 0,
                        Code = a.ItemCode,
                        Name = a.ItemName,
                        PartsNumber = a.PartsNumber,
                        Remarks = a.Remarks,
                        Year = a.Year,
                        SIOrVINNumber = a.SIOrVINNumber,
                        Model = a.Model,
                        DeliveryTerm = a.DeliveryTerm,
                        UnitName = a.UnitName,
                        Qty = (decimal)a.Quantity,
                        Rate = (decimal)a.Rate,
                        GrossAmount = (decimal)a.GrossAmount,
                        VATAmount = a.VATAmount.HasValue ? a.VATAmount.Value : 0,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        NetAmount = (decimal)a.NetAmount
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SalesItemBO> GetGoodsReceiptSalesOrderItems(string SalesOrderID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var getData = dbEntity.SpGetGoodsReceiptSalesOrderItems(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return getData.Select(a => new SalesItemBO()
                    {
                        SalesOrderItemID = (int)a.SalesOrderTransID,
                        SalesOrderID = (int)a.SalesOrderID,
                        TransNo = a.SalesOrderNo,
                        ItemID = (int)a.ItemID,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        BatchID = (int)a.BatchID,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchName = a.BatchNo,
                        ItemCategoryID = (int)a.CategoryID,
                        Code = a.Code,
                        Name = a.ItemName,
                        PartsNumber = a.PartsNumber,
                        PrintWithItemCode = a.PrintWithItemName,
                        Model = a.Model,
                        Remarks = a.Remarks,
                        SecondaryQty = a.SecondaryQty,
                        SecondaryMRP = a.SecondaryMRP,
                        SecondaryUnit = a.SecondaryUnit,
                        FullOrLoose = a.FullOrLoose,
                        Qty = (decimal)a.Qty,
                        OfferQty = (decimal)a.OfferQty,
                        Rate = (decimal)a.Rate,
                        DiscountPercentage = a.DiscountPercentage.HasValue ? a.DiscountPercentage.Value : 0,
                        SGSTPercentage = a.SGSTPercentage.HasValue ? a.SGSTPercentage.Value : 0,
                        CGSTPercentage = a.CGSTPercentage.HasValue ? a.CGSTPercentage.Value : 0,
                        IGSTPercentage = a.IGSTPercentage.HasValue ? a.IGSTPercentage.Value : 0,
                        CessPercentage = a.CessPercentage,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        IsVat = a.IsVat.HasValue ? a.IsVat.Value : 0,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        CurrencyName = a.CurrencyName,
                        CessAmount = (decimal)a.CessAmount,
                        Stock = (decimal)a.Stock,
                        SalesOrderNo = a.SalesOrderNo,
                        SalesUnitID = (int)a.SalesUnitID,
                        LooseRate = a.LooseRate.HasValue ? a.LooseRate.Value : 0,
                        BatchTypeName = a.BatchTypeName,
                        FreightAmount = (decimal)a.FreightAmount,
                        Category = a.CategoryName,
                        PackSize = a.PackSize.HasValue ? a.PackSize.Value : 0,
                        PrimaryUnit = a.PrimaryUnit,

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SalesItemBO> GetBatchwiseSalesOrderItems(string SalesOrderID, int StoreID, int CustomerID, int SchemeID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var getData = dbEntity.SpGetBatchwiseSalesOrderItems(SalesOrderID, StoreID, CustomerID, SchemeID,
                        GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return getData.Select(a => new SalesItemBO()
                    {
                        SalesOrderItemID = (int)a.SalesOrderTransID,
                        SalesOrderID = (int)a.SalesOrderID,
                        ItemID = (int)a.ItemID,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        BatchID = (int)a.BatchID,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchName = a.BatchNo,
                        ItemCategoryID = (int)a.CategoryID,
                        Code = a.Code,
                        Name = a.ItemName,
                        PartsNumber = a.PartsNumber,
                        PrintWithItemCode = a.PrintWithItemName,
                        Model = a.Model,
                        DeliveryTerm = a.DeliveryTerm,
                        FullOrLoose = a.FullOrLoose,
                        Qty = (decimal)a.Qty,
                        OfferQty = (decimal)a.OfferQty,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                        Rate = (decimal)a.Rate,
                        SecondaryMRP = (decimal)a.SecondaryMRP,
                        SecondaryOfferQty = (decimal)a.SecondaryOfferQty,
                        SecondaryQty = (decimal)a.SecondaryQty,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryUnitSize = a.SecondaryUnitSize,
                        DiscountPercentage = a.DiscountPercentage.HasValue ? a.DiscountPercentage.Value : 0,
                        SGSTPercentage = a.SGSTPercentage.HasValue ? a.SGSTPercentage.Value : 0,
                        CGSTPercentage = a.CGSTPercentage.HasValue ? a.CGSTPercentage.Value : 0,
                        IGSTPercentage = a.IGSTPercentage.HasValue ? a.IGSTPercentage.Value : 0,
                        CessPercentage = a.CessPercentage,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        IsVat = a.IsVat.HasValue ? a.IsVat.Value : 0,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        CurrencyName = a.CurrencyName,
                        CessAmount = (decimal)a.CessAmount,
                        Stock = (decimal)a.Stock,
                        ActualOfferQty = (decimal)a.ActualOfferQty,
                        SalesOrderNo = a.SalesOrderNo,
                        SalesUnitID = (int)a.SalesUnitID,
                        LooseRate = a.LooseRate.HasValue ? a.LooseRate.Value : 0,
                        BatchTypeName = a.BatchTypeName,
                        FreightAmount = (decimal)a.FreightAmount,
                        Category = a.CategoryName,
                        PackSize = a.PackSize.HasValue ? a.PackSize.Value : 0,
                        PrimaryUnit = a.PrimaryUnit,

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SalesItemBO> GetSalesOrderItems(string SalesOrderID, int StoreID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpSGetSalesOrderItems(SalesOrderID, StoreID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                    {
                        SalesOrderItemID = a.SalesOrderTransID,
                        SalesOrderID = (int)a.SalesOrderID,
                        ItemID = (int)a.ItemID,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        BatchID = (int)a.BatchID,
                        BatchName = a.BatchName,
                        ItemCategoryID = a.CategoryID,
                        Code = a.ItemCode,
                        Name = a.ItemName,
                        FullOrLoose = a.FullOrLoose,
                        Qty = (decimal)a.Quantity,
                        OfferQty = (decimal)a.OfferQty,
                        QtyMet = (decimal)a.QtyMet,
                        OfferQtyMet = (decimal)a.OfferMet,
                        MRP = (decimal)a.MRP,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGST = (decimal)a.SGSTAmt,
                        CGST = (decimal)a.CGSTAmt,
                        IGST = (decimal)a.IGSTAmt,
                        CessPercentage = a.CessPercentage,
                        CessAmount = a.CessAmount,
                        NetAmount = (decimal)a.NetAmt,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.IGSTPercentage,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        Stock = (decimal)a.Stock,

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckItemCreatedForSalesInquiryItems(int SalesInquiryItemID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ObjectParameter ItemID = new ObjectParameter("ItemID", typeof(int));
                    dbEntity.SpCheckItemCreatedForSalesInquiryItems(SalesInquiryItemID, ItemID);
                    return Convert.ToInt32(ItemID.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetSchemeAllocation(int CustomerID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ObjectParameter SchemeAllocationID = new ObjectParameter("SchemeAllocationID", typeof(int));
                    dbEntity.SpGetSchemeAllocation(CustomerID, GeneralBO.ApplicationID, SchemeAllocationID);
                    return Convert.ToInt32(SchemeAllocationID.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DiscountAndOfferBO GetDiscountAndOfferDetails(int CustomerID, int SchemeID, int ItemID, decimal Qty, int UnitID)
        {
            DiscountAndOfferBO DiscountAndOffer = new DiscountAndOfferBO();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    DiscountAndOffer.OfferDetails = dbEntity.SpGetSchemeItem(CustomerID, SchemeID, ItemID, Convert.ToInt32(Qty), UnitID, GeneralBO.ApplicationID).Select(a => new OfferBO()
                    {
                        ItemID = (int)a.OfferItemID,
                        ItemName = a.Item,
                        Qty = (decimal)a.InvoiceQty,
                        OfferQty = (decimal)a.OfferQty,
                        UnitID = (int)a.SalesUnitID
                    }).ToList();

                    DiscountAndOffer.ItemID = ItemID;
                    DiscountAndOffer.UnitID = DiscountAndOffer.OfferDetails.Count() > 0 ? DiscountAndOffer.OfferDetails.FirstOrDefault().UnitID : 0;
                    DiscountAndOffer.DiscountPercentage = GetDiscountPercentage(CustomerID, ItemID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DiscountAndOffer;
        }

        public decimal GetDiscountPercentage(int CustomerID, int ItemID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ObjectParameter DiscountPercentage = new ObjectParameter("DiscountPercentage", 111.01);
                    dbEntity.SpGetDiscountPercentage(CustomerID, ItemID, GeneralBO.ApplicationID, DiscountPercentage);
                    return Convert.ToDecimal(DiscountPercentage.Value.ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DiscountAndOfferBO> GetOfferDetails(int CustomerID, int SchemeID, int[] ItemID, int[] UnitID)
        {
            List<DiscountAndOfferBO> OfferDetails = new List<DiscountAndOfferBO>();
            DiscountAndOfferBO Offer;
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    string CommaSeperatedItemIDs = string.Join(",", ItemID.Select(x => x.ToString()).ToArray());
                    string CommaSeperatedUnitIDs = string.Join(",", UnitID.Select(x => x.ToString()).ToArray());
                    ////CommaSeperatedItemIDs
                    var obj = dbEntity.SpGetOfferDetails(CustomerID, SchemeID, CommaSeperatedItemIDs, CommaSeperatedUnitIDs, GeneralBO.ApplicationID).Select(a => new OfferBO()
                    {
                        ItemID = (int)a.OfferItemID,
                        Qty = (decimal)a.InvoiceQty,
                        OfferQty = (decimal)a.OfferQty,
                        UnitID = (int)a.UnitID
                    }).ToList();
                    foreach (var item in ItemID)
                    {
                        Offer = new DiscountAndOfferBO();
                        Offer.OfferDetails = obj.Select(a => new OfferBO()
                        {
                            ItemID = a.ItemID,
                            Qty = a.Qty,
                            OfferQty = a.OfferQty,


                        }).Where(a => a.ItemID == item).ToList();
                        Offer.ItemID = item;
                        Offer.UnitID = obj.Where(a => a.ItemID == item).Count() == 0 ? 0 : obj.Where(a => a.ItemID == item).FirstOrDefault().UnitID;
                        Offer.DiscountPercentage = GetDiscountPercentage(CustomerID, Offer.ItemID);
                        OfferDetails.Add(Offer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OfferDetails;
        }

        public bool SaveSalesInquiry(SalesInquiryBO salesInquiryBO, string XMLItems)
        {

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var Transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "SalesInquiry";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (salesInquiryBO.IsDraft)
                        {
                            FormName = "DraftSalesInquiry";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SpCreateSalesInquiry(
                            SerialNo.Value.ToString(),
                            salesInquiryBO.SalesInquiryDate,
                            salesInquiryBO.RequestedDelivaryDate,
                            salesInquiryBO.RequestExpiryDate,
                            salesInquiryBO.RequestedCustomerName,
                            salesInquiryBO.RequestedCustomerAddress,
                            salesInquiryBO.Remarks,
                            salesInquiryBO.PhoneNo1,
                            salesInquiryBO.PhoneNo2,
                            salesInquiryBO.Make,
                            salesInquiryBO.Model,
                            salesInquiryBO.Year,
                            salesInquiryBO.SIOrVINNumber,
                            salesInquiryBO.GrossAmount,
                            salesInquiryBO.NetAmount,
                            salesInquiryBO.IsDraft,
                            GeneralBO.LocationID,
                            GeneralBO.CreatedUserID,
                            XMLItems);
                        Transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Transaction.Rollback();
                        throw e;
                    }
                }

            }

        }

        public bool UpdateSalesInquiry(SalesInquiryBO salesInquiryBO, string XMLItems)
        {

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var Transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        dbEntity.SpUpdateSalesInquiry(
                        salesInquiryBO.ID,
                        salesInquiryBO.SalesInquiryDate,
                        salesInquiryBO.RequestedDelivaryDate,
                        salesInquiryBO.RequestExpiryDate,
                        salesInquiryBO.RequestedCustomerName,
                        salesInquiryBO.RequestedCustomerAddress,
                        salesInquiryBO.Remarks,
                        salesInquiryBO.PhoneNo1,
                        salesInquiryBO.PhoneNo2,
                        salesInquiryBO.Make,
                        salesInquiryBO.Model,
                        salesInquiryBO.Year,
                        salesInquiryBO.SIOrVINNumber,
                        salesInquiryBO.GrossAmount,
                        salesInquiryBO.NetAmount,
                        salesInquiryBO.IsDraft,
                        GeneralBO.LocationID,
                        GeneralBO.CreatedUserID,
                        XMLItems);
                        Transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }


        public DatatableResultBO GetCustomerSalesOrderList(string TransNo, string TransDateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetCustomerSalesOrderList(null,TransNo, TransDateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                SalesOrderNo = item.SalesOrderNo,
                                OrderDate = ((DateTime)item.OrderDate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
                                NetAmount = (decimal)item.NetAmount,
                               
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
        public DatatableResultBO GetAllSalesInquiryList(string Type, string SalesInquiryNo, string SalesInquiryDateHint, string RequestedCustomerNameHint, string PhoneNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {

                    var result = dbEntity.SpGetAllSalesInquiryList(Type, SalesInquiryNo, SalesInquiryDateHint, RequestedCustomerNameHint, PhoneNo, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                SalesInquiryNo = item.SalesInquiryNo,
                                SalesInquiryDate = ((DateTime)item.SalesInquiryDate).ToString("dd-MMM-yyyy"),
                                RequestedCustomerName = item.RequestedCustomerName,
                                PhoneNo1 = item.PhoneNo1,
                                PhoneNo2 = item.PhoneNo2,
                                RequestedCustomerAddress = item.RequestedCustomerAddress,
                                Remarks = item.Remarks,
                                GrossAmount = item.GrossAmount,
                                NetAmount = item.NetAmount
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
        public DatatableResultBO GetSalesInquiryList(string Type, string SalesInquiryNo, string SalesInquiryDateHint, string RequestedCustomerNameHint, string PhoneNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {

                    var result = dbEntity.SpGetSalesInquiryList(Type, SalesInquiryNo, SalesInquiryDateHint, RequestedCustomerNameHint, PhoneNo, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                SalesInquiryNo = item.SalesInquiryNo,
                                SalesInquiryDate = ((DateTime)item.SalesInquiryDate).ToString("dd-MMM-yyyy"),
                                RequestedCustomerName = item.RequestedCustomerName,
                                PhoneNo1 = item.PhoneNo1,
                                PhoneNo2 = item.PhoneNo2,
                                RequestedCustomerAddress = item.RequestedCustomerAddress,
                                Remarks = item.Remarks,
                                GrossAmount = item.GrossAmount,
                                NetAmount = item.NetAmount
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

        public bool IsCancelable(int SalesOrderID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter IsCancelable = new ObjectParameter("IsCancelable", typeof(bool));
                    dbEntity.SpIsSalesOrderCancelable(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsCancelable);
                    return Convert.ToBoolean(IsCancelable.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void Cancel(int SalesOrderID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    dbEntity.SpCancelSalesOrder(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public DatatableResultBO GetSalesOrderHistory(string Type, int ItemID, string SalesOrderNo, string OrderDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var fromDate = DateTime.Now.AddYears(-1);
                    var ToDate = DateTime.Now;

                    var result = dbEntity.SpGetSalesOrderTransHistory(Type, ItemID, SalesOrderNo, OrderDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result[i];
                            obj = new
                            {
                                item.SalesOrderNo,
                                OrderDate = ((DateTime)item.OrderDate).ToString("dd-MMM-yyyy"),
                                item.CustomerName,
                                item.Itemcode,
                                item.ItemName,
                                item.PartsNumber,
                                item.SecondaryMRP,
                                item.SecondaryQty,
                                item.SecondaryUnit,
                                item.DiscountPercentage,
                                item.VATPercentage,
                                item.TaxableAmount,
                                NetAmount = item.NetAmt
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
        public void Approve(int SalesOrderID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    dbEntity.SpApproveSalesOrder(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
