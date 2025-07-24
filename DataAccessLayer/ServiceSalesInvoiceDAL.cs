using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace DataAccessLayer
{
    public class ServiceSalesInvoiceDAL
    {

        public int Save(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails)
        {

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var Transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "ServiceSalesInvoice";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (Invoice.IsDraft)
                        {
                            FormName = "DraftServiceSalesInvoice";
                        }
                        ObjectParameter IsBlockedForSalesInvoice = new ObjectParameter("IsBlockedForSalesInvoice", typeof(int));

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        ObjectParameter SalesInvoiceID = new ObjectParameter("SalesInvoiceID", typeof(int));
                        dbEntity.SpCreateServiceSalesInvoice(
                            SerialNo.Value.ToString(),
                            Invoice.InvoiceDate,
                            Invoice.CustomerID,
                            Invoice.SalesOrderID,
                            Invoice.SalesOrderNos,
                            Invoice.PaymentModeID,
                            Invoice.PaymentTypeID,
                            Invoice.GrossAmount,
                            Invoice.DiscountAmount,
                            Invoice.TaxableAmount,
                            Invoice.SGSTAmount,
                            Invoice.CGSTAmount,
                            Invoice.IGSTAmount,
                            Invoice.CessAmount,
                            Invoice.RoundOff,
                            Invoice.NetAmount,
                            Invoice.IsDraft,
                            Invoice.BillingAddressID,
                            Invoice.ShippingAddressID,
                            Invoice.DiscountCategoryID,
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
                                dbEntity.SpCreateServiceSalesInvoiceTrans(
                                        Convert.ToInt16(SalesInvoiceID.Value),
                                        item.SalesOrderItemID,
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
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                    );
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

                            if (!Invoice.IsDraft)
                            {
                                dbEntity.SpCreateSLAMappingofServiceSalesItem(
                                Convert.ToInt16(SalesInvoiceID.Value),
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);
                            }
                        }
                        Transaction.Commit();
                        if (!Invoice.IsDraft)
                        {
                            ReceivableDAL receivableDAL = new ReceivableDAL();
                            ReceivablesBO receivableBO = new ReceivablesBO()
                            {
                                PartyID = Invoice.CustomerID,
                                TransDate = Invoice.InvoiceDate,
                                ReceivableType = "INVOICE",
                                ReferenceID = Convert.ToInt32(SalesInvoiceID.Value),
                                DocumentNo = SerialNo.Value.ToString(),
                                ReceivableAmount = Invoice.NetAmount,
                                Description = "Service Sales Invoice ",
                                ReceivedAmount = 0,
                                Status = "",
                                Discount = 0,
                            };
                            receivableDAL.SaveReceivables(receivableBO);
                        }

                        return 1;
                    }
                    catch (Exception e)
                    {
                        Transaction.Rollback();
                        throw e;
                    }
                }

            }

        }

        public int Update(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var Transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        dbEntity.SpUpdateServiceSalesInvoice(
                            Invoice.ID,
                            Invoice.CustomerID,
                            Invoice.SalesOrderID,
                            Invoice.SalesOrderNos,
                            Invoice.PaymentModeID,
                            Invoice.PaymentTypeID,
                            Invoice.GrossAmount,
                            Invoice.DiscountAmount,
                            Invoice.TaxableAmount,
                            Invoice.SGSTAmount,
                            Invoice.CGSTAmount,
                            Invoice.IGSTAmount,
                            Invoice.CessAmount,
                            Invoice.RoundOff,
                            Invoice.NetAmount,
                            Invoice.IsDraft,
                            Invoice.BillingAddressID,
                            Invoice.ShippingAddressID,
                            0,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );
                        foreach (var item in Items)
                        {
                            dbEntity.SpCreateServiceSalesInvoiceTrans(
                                    Invoice.ID,
                                    item.SalesOrderItemID,
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
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                );
                        }
                        foreach (var item in AmountDetails)
                        {
                            dbEntity.SpCreateSalesInvoiceAmountDetails(
                                    Invoice.ID,
                                    item.Amount,
                                    item.Particulars,
                                    item.Percentage,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    item.TaxableAmount
                                );
                        }
                        if (!Invoice.IsDraft)
                        {
                            dbEntity.SpCreateSLAMappingofServiceSalesItem(
                                Convert.ToInt16(Invoice.ID),
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);
                        }

                        Transaction.Commit();

                        if (!Invoice.IsDraft)
                        {
                            ReceivableDAL receivableDAL = new ReceivableDAL();
                            ReceivablesBO receivableBO = new ReceivablesBO()
                            {
                                PartyID = Invoice.CustomerID,
                                TransDate = Invoice.InvoiceDate,
                                ReceivableType = "INVOICE",
                                ReferenceID = Invoice.ID,
                                DocumentNo = "",
                                ReceivableAmount = Invoice.NetAmount,
                                Description = "Service Sales Invoice ",
                                ReceivedAmount = 0,
                                Status = "",
                                Discount = 0,
                            };
                            receivableDAL.SaveReceivables(receivableBO);
                        }

                        return 1;
                    }
                    catch (Exception e)
                    {
                        Transaction.Rollback();
                        throw e;
                    }
                }

            }

        }

        public DatatableResultBO GetServiceSalesInvoiceList(string Type, string CodeHint, string DateHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetServiceSalesInvoiceList(Type, CodeHint, DateHint, CustomerNameHint, LocationHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                InvoiceDate = ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
                                Location = item.Location,
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

        public SalesInvoiceBO GetServiceSalesInvoice(int SalesInvoiceID, int LocationID)
        {
            SalesInvoiceBO Invoice = new SalesInvoiceBO();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    Invoice = dbEntity.SpGetServiceSalesInvoice(SalesInvoiceID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).Select(a => new SalesInvoiceBO()
                    {
                        ID = a.ID,
                        InvoiceNo = a.TransNo,
                        InvoiceDate = (DateTime)a.InvoiceDate,
                        CustomerID = (int)a.CustomerID,
                        SalesOrderNos = a.SalesOrders,

                        GrossAmount = (decimal)a.GrossAmt,
                        DiscountAmount = (decimal)a.DiscountAmt,

                        PaymentModeID = (int)a.PaymentModeID,
                        PaymentMode = a.PaymentMode,

                        TaxableAmount = (decimal)a.TaxableAmt,
                        SGSTAmount = (decimal)a.SGSTAmt,
                        CGSTAmount = (decimal)a.CGSTAmt,
                        IGSTAmount = (decimal)a.IGSTAmt,
                        RoundOff = (decimal)a.RoundOff,
                        NetAmount = (decimal)a.NetAmt,
                        IsProcessed = (bool)a.IsProcessed,
                        IsDraft = (bool)a.IsDraft,
                        IsCancelled = (bool)a.IsCancelled,

                        CustomerName = a.CustomerName,
                        CustomerCategory = a.CustomerCategory,
                        CustomerCategoryID = (int)a.CustomerCategoryID,
                        PriceListID = (int)a.PriceListID,
                        StateID = (int)a.StateID,
                        BillingAddressID = (int)a.BillingAddressID,
                        ShippingAddressID = (int)a.ShippingAddressID,
                        BillingAddress = a.BillingAddress,
                        ShippingAddress = a.ShippingAddress,
                        CessAmount = (decimal)a.CessAmount,
                        CreditAmount = (decimal)a.CreditBalance,
                        MinCreditLimit = (decimal)a.MinimumCreditLimit,
                        MaxCreditLimit = (decimal)a.MaxCreditLimit,
                        CustomerGSTNo = a.CustomerGSTNo,
                        DiscountCategoryID = a.DiscountCategoryID,
                        DiscountCategory = a.DiscountCategory
                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Invoice;
        }

        public List<SalesItemBO> GetServiceSalesInvoiceItems(int SalesInvoiceID, int LocationID)
        {
            List<SalesItemBO> Items = new List<SalesItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    Items = dbEntity.SpGetServiceSalesInvoiceItems(SalesInvoiceID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                    {
                        SalesInvoiceTransID = a.ID,
                        SalesOrderItemID = (int)a.SalesOrderTransID,
                        ItemID = (int)a.ItemID,
                        Qty = (decimal)a.Quantity,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        MRP = (decimal)a.MRP,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.GSTPercentage,
                        SGST = (decimal)a.SGSTAmt,
                        CGST = (decimal)a.CGSTAmt,
                        IGST = (decimal)a.IGSTAmt,
                        NetAmount = (decimal)a.NetAmt,
                        Name = a.ItemName,
                        UnitName = a.UnitName,
                        Code = a.Code,
                        UnitID = (int)a.UnitID,
                        Rate = (decimal)a.Rate,
                        CashDiscount = (decimal)a.CashDiscount,
                        POID = a.PurchaseOrderID,
                        POTransID = a.POTransID,
                        SalesInvoiceID = (int)a.SalesInvoiceID,
                        PORate = (decimal)a.PORate,
                        POQuantity = (decimal)a.POQuantity,
                        CessAmount = (decimal)a.CessAmount,
                        CessPercentage = (decimal)a.CessPercentage,
                        Remarks=a.Remarks

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Items;
        }

        public int Cancel(int SalesInvoiceID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    return dbEntity.SpCancelServiceSalesInvoice(SalesInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


    }
}
