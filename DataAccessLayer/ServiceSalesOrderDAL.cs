using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataAccessLayer
{
    public class ServiceSalesOrderDAL
    {
        public decimal GetDiscountPercentage(int CustomerID, int ItemID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ObjectParameter DiscountPercentage = new ObjectParameter("DiscountPercentage", 11.01);
                    dbEntity.SpGetDiscountPercentage(CustomerID, ItemID, GeneralBO.ApplicationID, DiscountPercentage);
                    return Convert.ToDecimal(DiscountPercentage.Value.ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool SaveServiceSalesOrder(SalesOrderBO SalesOrderBO, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails)
        {

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var Transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "ServiceSalesInvoice";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (SalesOrderBO.IsDraft)
                        {
                            FormName = "DraftServiceSalesInvoice";
                        }

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        ObjectParameter SalesOrderID = new ObjectParameter("SalesOrderID", typeof(int));
                        var SalesOrderNo = SerialNo.Value.ToString();
                        dbEntity.SpCreateServiceSalesOrder(
                            SerialNo.Value.ToString(),
                            SalesOrderBO.SODate,
                            SalesOrderBO.CustomerID,
                            SalesOrderBO.GrossAmount,
                            SalesOrderBO.DiscountAmount,
                            SalesOrderBO.TaxableAmount,
                            SalesOrderBO.SGSTAmount,
                            SalesOrderBO.CGSTAmount,
                            SalesOrderBO.IGSTAmount,
                            SalesOrderBO.CessAmount,
                            SalesOrderBO.RoundOff,
                            SalesOrderBO.NetAmount,
                            SalesOrderBO.FsoID,
                            SalesOrderBO.Source,
                            SalesOrderBO.BillingAddressID,
                            SalesOrderBO.ShippingAddressID,
                            SalesOrderBO.IsDraft,
                            SalesOrderBO.DirectInvoice,
                            GeneralBO.CreatedUserID,
                            SalesOrderBO.PaymentModeID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SalesOrderID
                            );
                        if (SalesOrderID.Value != null)
                        {
                            foreach (var item in Items)
                            {
                                dbEntity.SpCreateServiceSalesOrderTrans(
                                        Convert.ToInt16(SalesOrderID.Value),
                                        item.ItemID,
                                        item.MRP,
                                        item.BasicPrice,
                                        item.Qty,
                                        item.GrossAmount,
                                        item.DiscountPercentage,
                                        item.DiscountAmount,
                                        item.TaxableAmount,
                                        item.GSTPercentage,
                                        item.IGSTPercentage,
                                        item.SGSTPercentage,
                                        item.CGSTPercentage,
                                        item.CGST,
                                        item.SGST,
                                        item.IGST,
                                        item.CessPercentage,
                                        item.CessAmount,
                                        item.NetAmount,
                                        item.UnitID,
                                        item.DoctorID,
                                        item.Remarks,
                                        item.BillableID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                    );
                            }
                        }

                        if (SalesOrderBO.DirectInvoice && !SalesOrderBO.IsDraft)
                        {
                            ObjectParameter IsBlockedForSalesInvoice = new ObjectParameter("IsBlockedForSalesInvoice", typeof(int));
                            ObjectParameter Processed = new ObjectParameter("Processed", typeof(bool));
                            ObjectParameter SalesInvoiceID = new ObjectParameter("SalesInvoiceID", typeof(int));
                            dbEntity.SpCreateServiceSalesInvoice(
                                SerialNo.Value.ToString(),
                                SalesOrderBO.SODate,
                                SalesOrderBO.CustomerID,
                                Convert.ToInt32(SalesOrderID.Value),
                                SalesOrderNo,
                                SalesOrderBO.PaymentModeID,
                                SalesOrderBO.PaymentTypeID,
                                SalesOrderBO.GrossAmount,
                                SalesOrderBO.DiscountAmount,
                                SalesOrderBO.TaxableAmount,
                                SalesOrderBO.SGSTAmount,
                                SalesOrderBO.CGSTAmount,
                                SalesOrderBO.IGSTAmount,
                                SalesOrderBO.CessAmount,
                                SalesOrderBO.RoundOff,
                                SalesOrderBO.NetAmount,
                                false,
                                SalesOrderBO.BillingAddressID,
                                SalesOrderBO.ShippingAddressID,
                                0,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                SalesInvoiceID,
                                IsBlockedForSalesInvoice
                                );
                            if ((int)IsBlockedForSalesInvoice.Value == 1)
                            {
                                throw new DatabaseException("Customer is blocked for sales invoice");
                            }
                            if (SalesInvoiceID.Value != null)
                            {
                                foreach (var item in Items)
                                {
                                    dbEntity.SpCreateDirectServiceSalesInvoiceTrans(
                                            Convert.ToInt16(SalesInvoiceID.Value),
                                           SalesOrderBO.ID,
                                            item.ItemID,
                                            item.Qty,
                                            item.Qty,
                                            item.MRP,
                                            item.BasicPrice,
                                            item.GrossAmount,
                                            item.DiscountPercentage,
                                            item.DiscountAmount,
                                            item.TaxableAmount,
                                            item.SGSTPercentage,
                                            item.CGSTPercentage,
                                            item.IGSTPercentage,
                                            item.CessPercentage,
                                            item.SGST,
                                            item.CGST,
                                            item.IGST,
                                            item.CessAmount,
                                            item.NetAmount,
                                            item.UnitID,
                                            item.Remarks,
                                            item.BillableID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            Processed
                                        );
                                    if (Convert.ToBoolean(Processed.Value) == true)
                                    {
                                        throw new DatabaseException("Bill Paid Already");
                                    }
                                }
                            }
                            if (SalesInvoiceID.Value != null)
                            {
                                foreach (var item in AmountDetails)
                                {
                                    dbEntity.SpCreateSalesInvoiceAmountDetails(
                                            Convert.ToInt16(SalesInvoiceID.Value),
                                            item.Amount,
                                            item.Particulars,
                                            item.Percentage,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            item.TaxableAmount
                                        );
                                }
                                if (!SalesOrderBO.IsDraft)
                                {
                                    dbEntity.SpCreateSLAMappingofServiceSalesItem(
                                    Convert.ToInt16(SalesInvoiceID.Value),
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
                                }
                            }
                            Transaction.Commit();
                            if (!SalesOrderBO.IsDraft)
                            {
                                ReceivableDAL receivableDAL = new ReceivableDAL();
                                ReceivablesBO receivableBO = new ReceivablesBO()
                                {
                                    PartyID = SalesOrderBO.CustomerID,
                                    TransDate = SalesOrderBO.SODate,
                                    ReceivableType = "INVOICE",
                                    ReferenceID = Convert.ToInt32(SalesInvoiceID.Value),
                                    DocumentNo = SerialNo.Value.ToString(),
                                    ReceivableAmount = SalesOrderBO.NetAmount,
                                    Description = "Service Sales Invoice ",
                                    ReceivedAmount = 0,
                                    Status = "",
                                    Discount = 0,
                                };
                                receivableDAL.SaveReceivables(receivableBO);
                            }

                        }
                        if (!SalesOrderBO.DirectInvoice || (SalesOrderBO.DirectInvoice && SalesOrderBO.IsDraft))
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

        public bool UpdateServiceSalesOrder(SalesOrderBO SalesOrderBO, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails)
        {

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var Transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SalesOrderno = new ObjectParameter("SerialNo", typeof(string));

                        dbEntity.SpUpdateServiceSalesOrder(
                            SalesOrderBO.ID,
                            SalesOrderBO.CustomerID,
                            SalesOrderBO.GrossAmount,
                            SalesOrderBO.DiscountAmount,
                            SalesOrderBO.TaxableAmount,
                            SalesOrderBO.SGSTAmount,
                            SalesOrderBO.CGSTAmount,
                            SalesOrderBO.IGSTAmount,
                            SalesOrderBO.CessAmount,
                            SalesOrderBO.RoundOff,
                            SalesOrderBO.NetAmount,
                            SalesOrderBO.FsoID,
                            SalesOrderBO.Source,
                            SalesOrderBO.BillingAddressID,
                            SalesOrderBO.ShippingAddressID,
                            SalesOrderBO.DirectInvoice,
                            SalesOrderBO.IsDraft,
                            GeneralBO.CreatedUserID,
                            SalesOrderBO.PaymentModeID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SalesOrderno
                            );
                        foreach (var item in Items)
                        {
                            dbEntity.SpCreateServiceSalesOrderTrans(
                                    SalesOrderBO.ID,
                                    item.ItemID,
                                    item.MRP,
                                    item.BasicPrice,
                                    item.Qty,
                                    item.GrossAmount,
                                    item.DiscountPercentage,
                                    item.DiscountAmount,
                                    item.TaxableAmount,
                                    item.GSTPercentage,
                                    item.IGSTPercentage,
                                    item.SGSTPercentage,
                                    item.CGSTPercentage,
                                    item.CGST,
                                    item.SGST,
                                    item.IGST,
                                    item.CessPercentage,
                                    item.CessAmount,
                                    item.NetAmount,
                                    item.UnitID,
                                    item.DoctorID,
                                    item.Remarks,
                                    item.BillableID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                );
                        }

                        if (SalesOrderBO.DirectInvoice && !SalesOrderBO.IsDraft)
                        {
                            ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                            ObjectParameter IsBlockedForSalesInvoice = new ObjectParameter("IsBlockedForSalesInvoice", typeof(int));
                            ObjectParameter Processed = new ObjectParameter("Processed", typeof(bool));
                            

                            var i = dbEntity.SpUpdateSerialNo("ServiceSalesInvoice", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                            ObjectParameter SalesInvoiceID = new ObjectParameter("SalesInvoiceID", typeof(int));
                            dbEntity.SpCreateServiceSalesInvoice(
                                SerialNo.Value.ToString(),
                                SalesOrderBO.SODate,
                                SalesOrderBO.CustomerID,
                                SalesOrderBO.ID,
                                SalesOrderno.Value.ToString(),
                                SalesOrderBO.PaymentModeID,
                                SalesOrderBO.PaymentTypeID,
                                SalesOrderBO.GrossAmount,
                                SalesOrderBO.DiscountAmount,
                                SalesOrderBO.TaxableAmount,
                                SalesOrderBO.SGSTAmount,
                                SalesOrderBO.CGSTAmount,
                                SalesOrderBO.IGSTAmount,
                                SalesOrderBO.CessAmount,
                                SalesOrderBO.RoundOff,
                                SalesOrderBO.NetAmount,
                                false,
                                SalesOrderBO.BillingAddressID,
                                SalesOrderBO.ShippingAddressID,
                                0,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                SalesInvoiceID,
                                IsBlockedForSalesInvoice
                                );
                            if ((int)IsBlockedForSalesInvoice.Value == 1)
                            {
                                throw new DatabaseException("Customer is blocked for sales invoice");
                            }
                            if (SalesInvoiceID.Value != null)
                            {
                                foreach (var item in Items)
                                {
                                    dbEntity.SpCreateDirectServiceSalesInvoiceTrans(
                                            Convert.ToInt16(SalesInvoiceID.Value),
                                           SalesOrderBO.ID,
                                            item.ItemID,
                                            item.Qty,
                                            item.Qty,
                                            item.MRP,
                                            item.BasicPrice,
                                            item.GrossAmount,
                                            item.DiscountPercentage,
                                            item.DiscountAmount,
                                            item.TaxableAmount,
                                            item.SGSTPercentage,
                                            item.CGSTPercentage,
                                            item.IGSTPercentage,
                                            item.CessPercentage,
                                            item.SGST,
                                            item.CGST,
                                            item.IGST,
                                            item.CessAmount,
                                            item.NetAmount,
                                            item.UnitID,
                                            item.Remarks,
                                            item.BillableID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            Processed
                                        );
                                    if (Convert.ToBoolean(Processed.Value) == true)
                                    {
                                        throw new DatabaseException("Bill Paid Already");
                                    }
                                }
                            }
                            if (SalesInvoiceID.Value != null)
                            {
                                foreach (var item in AmountDetails)
                                {
                                    dbEntity.SpCreateSalesInvoiceAmountDetails(
                                            Convert.ToInt16(SalesInvoiceID.Value),
                                            item.Amount,
                                            item.Particulars,
                                            item.Percentage,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            item.TaxableAmount
                                        );
                                }
                                if (!SalesOrderBO.IsDraft)
                                {
                                    dbEntity.SpCreateSLAMappingofServiceSalesItem(
                                    Convert.ToInt16(SalesInvoiceID.Value),
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
                                }
                            }

                            if (!SalesOrderBO.IsDraft)
                            {
                                ReceivableDAL receivableDAL = new ReceivableDAL();
                                ReceivablesBO receivableBO = new ReceivablesBO()
                                {
                                    PartyID = SalesOrderBO.CustomerID,
                                    TransDate = SalesOrderBO.SODate,
                                    ReceivableType = "INVOICE",
                                    ReferenceID = Convert.ToInt32(SalesInvoiceID.Value),
                                    DocumentNo = SerialNo.Value.ToString(),
                                    ReceivableAmount = SalesOrderBO.NetAmount,
                                    Description = "Service Sales Invoice ",
                                    ReceivedAmount = 0,
                                    Status = "",
                                    Discount = 0,
                                };
                                receivableDAL.SaveReceivables(receivableBO);
                            }

                        }

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

        public DatatableResultBO GetServiceSalesOrderList(string Type, string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {

                    var result = dbEntity.SpGetServiceSalesOrderList(Type, CodeHint, DateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                SONo = item.SONo,
                                SODate = ((DateTime)item.SODate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
                                NetAmount = (decimal)item.NetAmount,
                                Status = (bool)item.IsCancelled ? "cancelled" : (bool)item.IsProcessed ? "processed" : (bool)item.IsDraft ? "draft" : ""
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

        public DatatableResultBO GetServiceSalesUnprocessOrderList(int CustomerID, string SalesType, string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {

                    var result = dbEntity.SpGetServiceSalesOrderUnprocessedList(CustomerID, SalesType, CodeHint, DateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                SONo = item.SONo,
                                SODate = ((DateTime)item.SODate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
                                NetAmount = (decimal)item.NetAmount,
                                Status = (bool)item.IsCancelled ? "cancelled" : (bool)item.IsProcessed ? "processed" : (bool)item.IsDraft ? "draft" : ""
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

        public SalesOrderBO GetServiceSalesOrder(int SalesOrderID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetServiceSalesOrder(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesOrderBO()
                    {
                        ID = a.ID,
                        SONo = a.SalesOrderNo,
                        SODate = (DateTime)a.OrderDate,
                        CustomerID = (int)a.CustomerID,
                        CustomerName = a.Customer,
                        CustomerCategoryID = (int)a.CustomerCategoryID,
                        CustomerCategory = a.CustomerCategory,
                        GrossAmount = (decimal)a.GrossAmt,
                        DiscountAmount = (decimal)a.DiscountAmt,
                        TaxableAmount = (decimal)a.TaxableAmt,
                        SGSTAmount = (decimal)a.SGSTAmt,
                        CGSTAmount = (decimal)a.CGSTAmt,
                        IGSTAmount = (decimal)a.IGSTAmt,
                        CessAmount = (decimal)a.CessAmount,
                        RoundOff = (decimal)a.RoundOff,
                        NetAmount = (decimal)a.NetAmt,
                        IsDraft = (bool)a.IsDraft,
                        IsCancelled = (bool)a.IsCancelled,
                        IsProcessed = (bool)a.IsProcessed,
                        PriceListID = (int)a.PriceListID,
                        StateID = (int)a.StateID,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        Source = a.Source,
                        FsoID = a.FsoID ?? 0,
                        BillingAddressID = (int)a.BillingAddressID,
                        ShippingAddressID = (int)a.ShippingAddressID,
                        BillingAddress = a.BillingAddress,
                        ShippingAddress = a.ShippingAddress,
                        IsApproved = (bool)a.IsApproved,
                        DirectInvoice = (bool)a.IsDirectInvoice,
                        PaymentModeID = (int)a.PaymentModeID
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesItemBO> GetServiceSalesOrderItems(int SalesOrderID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetServiceSalesOrderItems(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                    {
                        SalesOrderTransID = a.SalesOrderTransID,
                        SalesOrderID = (int)a.SalesOrderID,
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
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
                        CessPercentage = (decimal)a.CessPercentage,
                        CessAmount = (decimal)a.CessAmount,
                        NetAmount = (decimal)a.NetAmt,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.GSTPercentage,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        DoctorID = (int)a.Doctor,
                        DoctorName = a.DoctorName,
                        Remarks=a.Remarks
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsCancelable(int SalesOrderID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter IsCancelable = new ObjectParameter("IsCancelable", typeof(bool));
                    dbEntity.SpIsServiceSalesOrderCancelable(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsCancelable);
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
                    dbEntity.SpCancelServiceSalesOrder(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<SalesItemBO> GetServiceSalesOrderItemsBySalesOrderIDs(string SalesOrderID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetServiceSalesOrderItemsBySalesOrderIDs(SalesOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                    {
                        SalesOrderTransID = a.SalesOrderTransID,
                        SalesOrderID = (int)a.SalesOrderID,
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
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
                        CessPercentage = (decimal)a.CessPercentage,
                        CessAmount = (decimal)a.CessAmount,
                        NetAmount = (decimal)a.NetAmt,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.GSTPercentage,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        DoctorID = (int)a.Doctor,
                        DoctorName = a.DoctorName,
                        Remarks = a.Remarks
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SalesItemBO> GetBillableDetails(int IPID,int CustomerID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetBillableDetails(IPID, CustomerID,GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                    {
                        //SalesOrderTransID = a.SalesOrderTransID,
                        //SalesOrderID = (int)a.SalesOrderID,
                        BillableID=(int)a.ID,
                        ItemID = (int)a.ItemID,
                        ItemName = a.Item,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        ItemCategoryID = a.CategoryID,
                        Code = a.ItemCode,
                        Name = a.Item,
                        //FullOrLoose = a.FullOrLoose,
                        Qty = (decimal)a.Quantity,
                        //OfferQty = (decimal)a.OfferQty,
                        QtyMet = (decimal)a.QtyMet,
                       // OfferQtyMet = (decimal)a.OfferMet,
                        MRP = (decimal)a.MRP,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        //DiscountPercentage = (decimal)a.DiscountPercentage,
                        //DiscountAmount = (decimal)a.DiscountAmount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGST = (decimal)a.SGSTAmt,
                        CGST = (decimal)a.CGSTAmt,
                        IGST = (decimal)a.IGSTAmt,
                        CessPercentage = (decimal)a.CessPercentage,
                        //CessAmount = (decimal)a.CessAmount,
                        NetAmount = (decimal)a.NetAmt,
                        SGSTPercentage = (decimal)a.SGSTPercent,
                        CGSTPercentage = (decimal)a.CGSTPercent,
                        IGSTPercentage = (decimal)a.IGSTPercent,
                        GSTPercentage = (decimal)a.GSTPercent,
                        //AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        DoctorID = (int)a.DoctorID,
                        DoctorName = a.Doctor,
                        //GSTAmount=(decimal)a.SGSTAmt+(decimal)a.CGSTAmt+(decimal)a.IGSTAmt
                        //Remarks = a.Remarks
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetCustomerID(int IPID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ObjectParameter CustomerID = new ObjectParameter("CustomerID",typeof(int));
                    dbEntity.SpGetCustomerIDByIPID(IPID, GeneralBO.ApplicationID, CustomerID);
                    return Convert.ToInt16(CustomerID.Value.ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
