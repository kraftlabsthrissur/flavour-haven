using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public class SalesGoodsReceiptNoteDAL
    {
        public SalesGoodsReceiptBO GetGoodReceiptNote(int GoodReceiptNoteID)
        {
            SalesGoodsReceiptBO Invoice = new SalesGoodsReceiptBO();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var Data = dbEntity.SpGetGoodsReceiptDetail(GoodReceiptNoteID).ToList();
                    Invoice = Data.Select(a => new SalesGoodsReceiptBO()
                    {
                        ID = a.ID,
                        TransNo = a.TransNo,
                        TransDate = a.ReceiptDate.Value,
                        SalesOrderNos = a.SalesOrders,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGSTAmount = (decimal)a.SGSTAmount,
                        CGSTAmount = (decimal)a.CGSTAmount,
                        IGSTAmount = (decimal)a.IGSTAmount,
                        RoundOff = (decimal)a.RoundOff,
                        NetAmount = (decimal)a.NetAmount,
                        IsDraft = a.IsDraft.HasValue ? a.IsDraft.Value : false,
                        IsCancelled = a.IsCancelled.HasValue ? a.IsCancelled.Value : false,
                        CustomerName = a.CustomerName,
                        CessAmount = (decimal)a.CessAmount,
                        Remarks = a.Remarks,
                        
                        
                        
                        

                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Invoice;
        }

        public List<SalesGoodsReceiptItemBO> GetGoodReceiptNoteItem(int GoodReceiptNoteID)
        {
            List<SalesGoodsReceiptItemBO> Items = new List<SalesGoodsReceiptItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var DataList = dbEntity.SpGetGoodsReceiptItemDetail(GoodReceiptNoteID).ToList();
                    Items = DataList.Select(a => new SalesGoodsReceiptItemBO()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.ItemName,
                        PartsNumber = a.PartsNumber,
                        Model = a.Model,
                        CurrencyName = a.CurrencyName,
                        CurrencyID = a.CurrencyID.HasValue ? a.CurrencyID.Value : 0,
                        GoodReceiptNoteID = GoodReceiptNoteID,
                        SalesInvoiceNo = a.SalesOrderNo,
                        SalesOrderNo = a.SalesInvoiceNo,
                        CounterSalesNo = a.CounterSalesNo,
                        ItemName = a.ItemName,
                        OfferQty = a.OfferQty.HasValue ? a.OfferQty.Value : 0,
                        Qty = a.Quantity.HasValue ? a.Quantity.Value : 0,
                        InvoiceQty = a.Quantity.HasValue ? a.Quantity.Value : 0,
                        SecondaryMRP = a.SecondaryMRP,
                        SecondaryQty = a.SecondaryQty,
                        SecondaryUnit = a.SecondaryUnit,
                        MRP = a.MRP.HasValue ? a.MRP.Value : 0,
                        Rate = a.MRP.HasValue ? a.MRP.Value : 0,
                        LooseRate = a.MRP.HasValue ? a.MRP.Value : 0,
                        BasicPrice = a.BasicPrice.HasValue ? a.BasicPrice.Value : 0,
                        GrossAmount = a.GrossAmount.HasValue ? a.GrossAmount.Value : 0,
                        DiscountPercentage = a.DiscountPercentage.HasValue ? a.DiscountPercentage.Value : 0,
                        DiscountAmount = a.DiscountAmount.HasValue ? a.DiscountAmount.Value : 0,
                        TaxableAmount = a.TaxableAmount.HasValue ? a.TaxableAmount.Value : 0,
                        SGST = a.SGSTAmount.HasValue ? a.SGSTAmount.Value : 0,
                        CGST = a.CGSTAmount.HasValue ? a.CGSTAmount.Value : 0,
                        IGST = a.IGSTAmount.HasValue ? a.IGSTAmount.Value : 0,
                        CessAmount = a.CessAmount,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        NetAmount = a.NetAmount.HasValue ? a.NetAmount.Value : 0,
                        IsGST = a.IsGST.HasValue ? a.IsGST.Value : 0,
                        IsVat = a.IsVat.HasValue ? a.IsVat.Value : 0,
                        PrintWithItemName = a.PrintWithItemName.HasValue ? a.PrintWithItemName.Value : false,
                        SONO=a.SONO
                        
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                ;
                throw e;
            }
            return Items;
        }

        public int Save(string XMLInvoice, string XMLItems)
        {
            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter GoodsReceiptNoteID = new ObjectParameter("GoodsReceiptNoteID", typeof(int));
                        dbEntity.SpCreateGoodsReceiptNote(XMLInvoice, XMLItems, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue, GoodsReceiptNoteID);
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new OutofStockException("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new AlreadyCancelledException("SO Already Cancelled");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new QuantityExceededException("Some items quantity already met");
                        }
                        InvoiceID = Convert.ToInt32(GoodsReceiptNoteID.Value);
                        dbEntity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return InvoiceID;
        }

        public int Update(string XMLInvoice, string XMLItems, int GoodsReceiptNoteID)
        {

            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        //dbEntity.SpUpdateProformaInvoice(ProformaInvoiceID, XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue);
                        dbEntity.SpUpdateGoodsReceiptNote(GoodsReceiptNoteID,XMLInvoice, XMLItems, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue);
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new OutofStockException("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {

                            throw new DatabaseException("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -2 || Convert.ToInt32(RetValue.Value) == -3)
                        {

                            throw new AlreadyCancelledException("Salesorder already cancelled");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new QuantityExceededException("Some items quantity already met");

                        }
                        InvoiceID = GoodsReceiptNoteID;
                        dbEntity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return InvoiceID;
        }

        public DatatableResultBO GetGoodReceiptNoteList(string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string ReceiptType, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetGoodsReceiptList(CodeHint, DateHint, CustomerNameHint, NetAmountHint, ReceiptType, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                ReceiptDate = ((DateTime)item.ReceiptDate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
                                NetAmount = (decimal)item.NetAmount,
                                Status = item.GoodsReceiptStatus,
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public bool IsCancelable(int GoodReceiptNoteID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter IsCancelable = new ObjectParameter("IsCancelable", typeof(bool));
                    dbEntity.SpIsProformaInvoiceCancelable(GoodReceiptNoteID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsCancelable);
                    return Convert.ToBoolean(IsCancelable.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void Cancel(int ProformaInvoiceID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    dbEntity.SpCancelProformaInvoice(ProformaInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


        public List<SalesGoodsReceiptItemBO> GetGoodReceiptNotes1(string salesOrderIDs)
        {
            List<SalesGoodsReceiptItemBO> Items = new List<SalesGoodsReceiptItemBO>();

            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var DataList = dbEntity.SpGetGoodsReceiptSalesOrderItems(salesOrderIDs, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    Items = DataList.Select(a => new SalesGoodsReceiptItemBO()
                    {
                        Qty = (int)a.Qty,
                        Unit = a.Unit,
                        OrderDate=a.OrderDate
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Items;
        }

        public List<SalesGoodsReceiptItemBO> GetGoodReceiptNotesItems(string counterSalesIDs)
        {
            List<SalesGoodsReceiptItemBO> Invoice = new List<SalesGoodsReceiptItemBO>();

            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var DataList = dbEntity.SpGetGoodsReceiptCounterSalesItems(counterSalesIDs, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    Invoice = DataList.Select(a => new SalesGoodsReceiptItemBO()
                    {
                        Itemcode = a.ItemCode,
                        ItemName = a.ItemName,
                        Remarks = a.Remark,
                        Quantity = (int)a.Quantity,
                        PartsNumber = a.PartsNumber,
                        Make = a.Make,
                        Unit=a.Unit,
                        Location = a.Location
                      //  FullOrLoose = a.FullOrLoose

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Invoice;
        }


        public SalesGoodsReceiptBO GetGoodReceiptNotes(string counterSalesIDs)
        {
            SalesGoodsReceiptBO Invoice = new SalesGoodsReceiptBO();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var Data = dbEntity.SpGetGoodsReceiptCounterSalesItems(counterSalesIDs, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    Invoice = Data.Select(a => new SalesGoodsReceiptBO()
                    {
                        Itemcode = a.ItemCode,
                        ItemName = a.ItemName,
                        Remarks = a.Remark,
                        Quantity = (int)a.Quantity,
                        PartsNumber = a.PartsNumber,
                        Make = a.Make,
                      Location =a.Location
                      // FullOrLoose = a.FullOrLoose

                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Invoice;
        }

        //public SalesGoodsReceiptBO GetGoodReceiptNotes(string SalesOrderIDs, int FinYear, int LocationID,int ApplicationID)
        //{
        //    SalesGoodsReceiptBO Invoice = new SalesGoodsReceiptBO();
        //    try
        //    {
        //        using (SalesEntities dbEntity = new SalesEntities())
        //        {
        //            var Data = dbEntity.SpGetGoodsReceiptSalesOrderItems(SalesOrderIDs, FinYear, LocationID, ApplicationID).ToList();
        //            Invoice = Data.Select(a => new SalesGoodsReceiptBO()
        //            {
        //               Qty=(int)a.Qty,
        //               Unit=a.Unit,

        //            }).
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return Invoice;
        //}



        //public List<SalesGoodsReceiptItemBO> GetGoodReceiptNoteItem(int GoodReceiptNoteID)
        //{
        //    List<SalesGoodsReceiptItemBO> Items = new List<SalesGoodsReceiptItemBO>();
        //    try
        //    {
        //        using (SalesEntities dbEntity = new SalesEntities())
        //        {
        //            var DataList = dbEntity.SpGetGoodsReceiptItemDetail(GoodReceiptNoteID).ToList();
        //            Items = DataList.Select(a => new SalesGoodsReceiptItemBO()



    }
}
