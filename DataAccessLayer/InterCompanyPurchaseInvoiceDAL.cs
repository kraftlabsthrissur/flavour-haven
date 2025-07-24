using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class InterCompanyPurchaseInvoiceDAL
    {

        public string SaveInvoice(InterCompanyPurchaseInvoiceBO invoiceBO, List<InterCompanyPurchaseInvoiceItemBO> TransItems, List<SalesAmountBO> AmountDetails)
        {
            int WarehouseID;
            int ShippingLocationID;
            int BillingLocationID;
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                ShippingLocationID = Convert.ToInt32(dbEntity.SpGetLocationIDForInterCompanyPurchase(invoiceBO.ShippingAddressID, "Location").FirstOrDefault());
                BillingLocationID = Convert.ToInt32(dbEntity.SpGetLocationIDForInterCompanyPurchase(invoiceBO.BillingAddressID, "Location").FirstOrDefault());

                WarehouseID = Convert.ToInt32(dbEntity.SpGetConfig("DefaultRMStore", ShippingLocationID, GeneralBO.ApplicationID).FirstOrDefault().ConfigValue);

            }

            using (PurchaseEntities dEntity = new PurchaseEntities())
            {

                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        // ObjectParameter PrId = new ObjectParameter("InterCompanyPurchaseInvoice", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter GRNId = new ObjectParameter("GoodsReceiptNoteID", typeof(int));
                        ObjectParameter GrnTransID = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter purchaseInvoiceIDOut = new ObjectParameter("purchaseInvoiceID", typeof(int));
                        var j = dEntity.SpUpdateSerialNo("InterCompanyPurchaseInvoice", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        // var CreatedUserID = GeneralBO.CreatedUserID;
                        dEntity.SaveChanges();

                        var i = dEntity.SpCreateGoodsReceiptNote(
                            SerialNo.Value.ToString(),
                            invoiceBO.InvoiceDate,
                            invoiceBO.SupplierID,
                            invoiceBO.InvoiceDate,
                            invoiceBO.SalesInvoiceNo,
                            invoiceBO.SalesInvoiceDate,
                            WarehouseID,
                            false,
                            false,
                            DateTime.Now,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            GeneralBO.FinYear,
                            ShippingLocationID,
                            GeneralBO.ApplicationID,
                            GRNId,
                            false
                            );

                        dEntity.SaveChanges();
                        var k = 0;
                        int l = 0;
                        string GRNCode = SerialNo.Value.ToString();
                        string BatchNo;
                        BatchNo = GRNCode + l.ToString().PadLeft(3, '0');
                        if (GRNId.Value != null)
                        {
                            foreach (var item in TransItems)
                            {

                                var InvoiceDate = invoiceBO.InvoiceDate ?? DateTime.Now.Date;
                                dEntity.SpCreateGoodsReceiptNoteTrans(
                                    Convert.ToInt32(GRNId.Value),
                                    item.PurchaseOrderID,
                                    item.POTransID,
                                    item.ItemID,
                                    item.Batch,
                                    InvoiceDate.AddDays(365),
                                    item.InvoiceQty,
                                    item.InvoiceQty,
                                    0,
                                    item.InvoiceQty,
                                    0,
                                    0,
                                    item.Remarks,
                                    GeneralBO.FinYear,
                                    ShippingLocationID,
                                    GeneralBO.ApplicationID,
                                    null,
                                    SerialNo.Value.ToString(),
                                    false,
                                    item.Batch,
                                    0,
                                    item.VATPercentage,
                                    item.CurrencyID,
                                    item.IsGST,
                                    item.IsVat,
                                    item.Model,
                                    item.PartsNumber,
                                    item.ItemName,
                                    item.NetAmount,
                                    item.UnitID,
                                    GrnTransID
                                    );
                                if (Convert.ToInt32(GrnTransID.Value) <= 0)
                                {
                                    transaction.Rollback();


                                }
                            }
                        };
                        dEntity.SaveChanges();
                        if (Convert.ToInt32(GrnTransID.Value) > 0)
                        {
                            var ii = dEntity.SpCreatePurchaseInvoice(
                              SerialNo.Value.ToString(),
                              invoiceBO.InvoiceDate,
                              invoiceBO.SupplierID,
                              invoiceBO.LocalSupplierName,
                              invoiceBO.SalesInvoiceNo,
                              invoiceBO.SalesInvoiceDate,
                              invoiceBO.GrossAmount,
                              invoiceBO.NetAmount,
                              invoiceBO.TotalDifference,
                              invoiceBO.SGSTAmount,
                              invoiceBO.CGSTAmount,
                              invoiceBO.IGSTAmount,
                              invoiceBO.Discount,
                              invoiceBO.FreightAmount,
                              invoiceBO.PackingCharges,
                              invoiceBO.SupplierOtherCharges,
                              invoiceBO.TaxOnFreight,
                              invoiceBO.TaxOnPackingCharges,
                              invoiceBO.TaxOnOtherCharge,
                              invoiceBO.TDSOnFreight,
                              invoiceBO.LessTDS,
                              invoiceBO.OtherDeductions,
                              invoiceBO.NetAmount,
                              invoiceBO.NetAmount,
                              invoiceBO.IsDraft,
                              false,
                              null,
                              GeneralBO.CreatedUserID,
                              DateTime.Now,
                              "Approved",
                              invoiceBO.TDSID,
                              invoiceBO.PaymentModeID,
                              invoiceBO.TurnoverDiscount,
                              invoiceBO.AdditionalDiscount,
                              invoiceBO.TaxableAmount,
                              invoiceBO.TradeDiscount,
                              invoiceBO.CashDiscount,
                              invoiceBO.SalesInvoiceID,
                              invoiceBO.CashDiscountEnabled,
                              0, "",
                              GeneralBO.FinYear,
                              BillingLocationID,
                              GeneralBO.ApplicationID,
                              purchaseInvoiceIDOut
                          );

                            if (purchaseInvoiceIDOut.Value != null && Convert.ToInt32(purchaseInvoiceIDOut.Value) > 0)
                            {
                                foreach (var tax in AmountDetails)
                                {
                                    var result = dEntity.SpCreatePurchaseInvoiceTaxDetails(
                                       Convert.ToInt32(purchaseInvoiceIDOut.Value),
                                        tax.Particulars,
                                        tax.Percentage,
                                        tax.Amount,
                                        tax.Amount,
                                        0,
                                        null,
                                        DateTime.Now
                                    );

                                }



                                foreach (var transItem in TransItems)
                                {
                                    var results = dEntity.SpCreatePurchaseInvoiceTrans(
                                        Convert.ToInt32(purchaseInvoiceIDOut.Value),
                                        Convert.ToInt32(GRNId.Value),
                                       0,
                                        transItem.ItemID,
                                        transItem.InvoiceQty,
                                        transItem.InvoiceRate,
                                        transItem.InvoiceValue,
                                        transItem.InvoiceQty,
                                        transItem.InvoiceQty,
                                        transItem.PORate,
                                        transItem.Difference,
                                        transItem.Remarks,
                                        0,
                                        transItem.IGSTPercent,
                                        transItem.CGSTPercent,
                                        transItem.SGSTPercent,
                                        transItem.GSTPercentage,
                                        0,
                                        transItem.UnitID,
                                        transItem.BatchID,
                                        transItem.BasicPrice,
                                        transItem.DiscountPercentage,
                                        transItem.DiscountAmount,
                                        transItem.TurnoverDiscount,
                                        transItem.AdditionalDiscount,
                                        transItem.TaxableAmount,
                                        transItem.CashDiscount,
                                        transItem.SalesInvoiceID,
                                        transItem.SalesInvoiceTransID,
                                        transItem.GrossAmount,
                                        transItem.NetAmount,
                                        transItem.SGSTAmt,
                                        transItem.IGSTAmt,
                                        transItem.CGSTAmt,
                                        GeneralBO.FinYear,
                                        BillingLocationID,
                                        GeneralBO.ApplicationID,
                                        ReturnValue
                                    );
                                    if (Convert.ToInt32(ReturnValue.Value) == -2)
                                    {
                                        throw new DuplicateEntryException("Some of the items in the purchase invoice has been processed already");
                                    }
                                    if (Convert.ToInt32(ReturnValue.Value) == -1)
                                    {
                                        throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                                    }


                                }
                            }
                        };
                        transaction.Commit();
                        return purchaseInvoiceIDOut.Value.ToString();
                        //    }
                    }


                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<InterCompanyPurchaseInvoiceBO> GetPurchaseInvoiceList()
        {
            List<InterCompanyPurchaseInvoiceBO> list = new List<InterCompanyPurchaseInvoiceBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetInterCompanyPurchaseInvoiceList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new InterCompanyPurchaseInvoiceBO
                {
                    Id = a.Id,
                    PurchaseDate = (DateTime)a.PurchaseDate,
                    PurchaseNo = a.PurchaseNo,
                    InvoiceNo = a.InvoiceNo,
                    InvoiceDate = (DateTime)a.InvoiceDate,
                    SupplierName = a.SupplierName
                }).ToList();
                return list;
            }

        }
        public InterCompanyPurchaseInvoiceBO GetPurchaseInvoiceDetails(int ID)
        {
            InterCompanyPurchaseInvoiceBO list = new InterCompanyPurchaseInvoiceBO();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetInterCompanyPurchaseInvoiceDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new InterCompanyPurchaseInvoiceBO
                {
                    SupplierID = (int)a.SupplierID,
                    PurchaseNo = a.PurchaseNo,
                    PurchaseOrderDate = (DateTime)a.PurchaseDate,
                    GrossAmount = (decimal)a.GrossAmount,
                    SGSTAmount = (decimal)a.SGSTAmount,
                    CGSTAmount = (decimal)a.CGSTAmount,
                    IGSTAmount = (decimal)a.IGSTAmount,
                    Discount = (decimal)a.Discount,

                    CashDiscountEnabled = (bool)a.CashDiscountEnabled,

                    CashDiscount = (decimal)a.CashDiscount,
                    TurnoverDiscount = (decimal)a.TurnOverDiscount,

                    AdditionalDiscount = (decimal)a.AdditionalDiscount,
                    TaxableAmount = (decimal)a.TaxableAmt,
                    RoundOff = (decimal)a.RoundOFf,
                    PurchaseDate = (DateTime)a.PurchaseDate,
                    InvoiceDate = (DateTime)a.InvoiceDate,
                    InvoiceNo = a.InvoiceNo,
                    SupplierName = a.SupplierName,

                    TradeDiscount = (decimal)a.TradeDiscount,
                    NetAmount = (decimal)a.NetAmount,
                    PaymentMode = a.PaymentMode,
                    OutstandingAmount = (decimal)a.OutstandingAmount

                }).FirstOrDefault();
                return list;
            }

        }
        public List<InterCompanyPurchaseInvoiceItemBO> GetPurchaseInvoiceTrans(int ID)
        {
            List<InterCompanyPurchaseInvoiceItemBO> list = new List<InterCompanyPurchaseInvoiceItemBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                try
                {
                    list = dEntity.SpGetInterCompanyPurchaseInvoiceTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new InterCompanyPurchaseInvoiceItemBO
                    {
                        ItemID = (int)a.ItemID,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceRate = (decimal)a.InvoiceRate,
                        InvoiceValue = (decimal)a.InvoiceValue,
                        AcceptedQty = a.AcceptedQty,
                        ApprovedQty = a.ApprovedQty,
                        ItemName = a.ItemName,
                        Unit = a.Unit,
                        Rate = (decimal)a.InvoiceRate,
                        PORate = (decimal)a.PORate,
                        Difference = (decimal)a.Difference,
                        Remarks = a.Remarks,
                        UnMatchedQty = a.UnMatchedQty,
                        CGSTPercent = a.CGSTPercent,
                        IGSTPercent = a.IGSTPercent,
                        SGSTPercent = a.SGSTPercent,
                        InvoiceGSTPercent = a.InvoiceGSTPercent,
                        UnitID = (int)a.UnitID,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        GSTAmount = (decimal)a.GSTAmount,
                        DiscPercentage = (decimal)a.DiscountPercentage,
                        GSTPercentage = (decimal)a.InvoiceGSTPercent,
                        //   TradeDiscPercentage = (decimal)a.TradeDiscPercentage,            
                        TurnoverDiscount = (decimal)a.TurnOverDiscount,
                        CashDiscount = (decimal)a.CashDiscount,
                        NetAmount = (decimal)a.NetAmount,
                        Batch = a.Batch,
                        ExpiryDate = a.ExpiryDate,
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return list;
        }
        public List<SalesAmountBO> GetPurchaseInvoiceTaxDetails(int ID)
        {
            List<SalesAmountBO> list = new List<SalesAmountBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetPurchaseInvoiceTaxDetails(ID).Select(a => new SalesAmountBO
                {
                    Particulars = a.Particular,
                    Amount = (decimal)a.InvoiceValue,
                    Percentage = (decimal)a.TaxPercentage,



                }).ToList();


                return list;
            }

        }

        public DatatableResultBO GetInterCompanyList(string TransNoHint, string TransDateHint, string SalesInvoiceNoHint, string SalesInvoiceDateHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetInterCompanyListForDatatable(TransNoHint, TransDateHint, SalesInvoiceNoHint, SalesInvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                PurchaseNo = item.PurchaseNo,
                                PurchaseDate = ((DateTime)item.PurchaseDate).ToString("dd-MMM-yyyy"),
                                InvoiceNo = item.InvoiceNo,
                                InvoiceDate = ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                SupplierName = item.SupplierName

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

    }
}

