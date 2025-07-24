using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLayer
{
    public class SalesGoodsReceiptNoteBL : ISalesGoodsReceiptNote
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        SalesGoodsReceiptNoteDAL salesGoodsReceiptNoteDAL;
        ProformaInvoiceBO proformaInvoice;
        PrintHelper PHelper;
        List<SalesItemBO> salesItemBO;
        private IGeneralContract generalBL;

        public SalesGoodsReceiptNoteBL()
        {
            salesGoodsReceiptNoteDAL = new SalesGoodsReceiptNoteDAL();
            PHelper = new PrintHelper();
            generalBL = new GeneralBL();
        }

        public void Cancel(int GoodReceiptNoteID)
        {
            salesGoodsReceiptNoteDAL.Cancel(GoodReceiptNoteID);
        }

        public SalesGoodsReceiptBO GetGoodReceiptNote(int ID)
        {
            return salesGoodsReceiptNoteDAL.GetGoodReceiptNote(ID);
        }


        public List<SalesGoodsReceiptItemBO> GetGoodReceiptNotesItems(string counterSalesIDs)
        {
            return salesGoodsReceiptNoteDAL.GetGoodReceiptNotesItems(counterSalesIDs);
        }

        public List<SalesGoodsReceiptItemBO> GetGoodReceiptNoteItem(int GoodReceiptNoteID)
        {
            return salesGoodsReceiptNoteDAL.GetGoodReceiptNoteItem(GoodReceiptNoteID);
        }

        public DatatableResultBO GetGoodReceiptNoteList(string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string InvoiceType, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesGoodsReceiptNoteDAL.GetGoodReceiptNoteList(CodeHint, DateHint, CustomerNameHint, NetAmountHint, InvoiceType, SortField, SortOrder, Offset, Limit);
        }
        public bool IsCancelable(int GoodReceiptNoteID)
        {
            return salesGoodsReceiptNoteDAL.IsCancelable(GoodReceiptNoteID);
        }
        public int Save(SalesGoodsReceiptBO Invoice, List<SalesGoodsReceiptItemBO> Items)
        {
            string XMLInvoice;
            string XMLItems;
            //string XMLAmountDetails;
            //string XMLPackingDetails;

            XMLInvoice = "<Invoices>";
            XMLInvoice += "<Invoice>";
            XMLInvoice += "<TransNo>" + Invoice.TransNo + "</TransNo>";
            XMLInvoice += "<TransDate>" + Invoice.TransDate + "</TransDate>";
            XMLInvoice += "<CustomerID>" + Invoice.CustomerID + "</CustomerID>";
            XMLInvoice += "<SalesOrderNos>" + Invoice.SalesOrderNos + "</SalesOrderNos>";
            XMLInvoice += "<DespatchDate>" + Invoice.DespatchDate + "</DespatchDate>";
            XMLInvoice += "<GrossAmount>" + Invoice.GrossAmount + "</GrossAmount>";
            XMLInvoice += "<DiscountAmount>" + Invoice.DiscountAmount + "</DiscountAmount>";
            XMLInvoice += "<TurnoverDiscount>" + Invoice.TurnoverDiscount + "</TurnoverDiscount>";
            XMLInvoice += "<AdditionalDiscount>" + Invoice.AdditionalDiscount + "</AdditionalDiscount>";
            XMLInvoice += "<TaxableAmount>" + Invoice.TaxableAmount + "</TaxableAmount>";
            XMLInvoice += "<CGSTAmount>" + Invoice.CGSTAmount + "</CGSTAmount>";
            XMLInvoice += "<SGSTAmount>" + Invoice.SGSTAmount + "</SGSTAmount>";
            XMLInvoice += "<IGSTAmount>" + Invoice.IGSTAmount + "</IGSTAmount>";
            XMLInvoice += "<RoundOff>" + Invoice.RoundOff + "</RoundOff>";
            XMLInvoice += "<NetAmount>" + Invoice.NetAmount + "</NetAmount>";
            XMLInvoice += "<IsDraft>" + Invoice.IsDraft + "</IsDraft>";
            XMLInvoice += "<BillingAddressID>" + Invoice.BillingAddressID + "</BillingAddressID>";
            XMLInvoice += "<ShippingAddressID>" + Invoice.ShippingAddressID + "</ShippingAddressID>";
            XMLInvoice += "<CreatedUserID>" + GeneralBO.CreatedUserID + "</CreatedUserID>";
            XMLInvoice += "<FinYear>" + GeneralBO.FinYear + "</FinYear>";
            XMLInvoice += "<LocationID>" + GeneralBO.LocationID + "</LocationID>";
            XMLInvoice += "<ApplicationID>" + GeneralBO.ApplicationID + "</ApplicationID>";
            XMLInvoice += "<CessAmount>" + Invoice.CessAmount + "</CessAmount>";
            XMLInvoice += "<Remarks>" + Invoice.Remarks + "</Remarks>";
            XMLInvoice += "</Invoice>";
            XMLInvoice += "</Invoices>";

            XMLItems = "<InvoiceTrans>";
            int i = 0;
            foreach (var item in Items)
            {
                i++;
                XMLItems += "<Item>";
                XMLItems += "<ItemID>" + item.ItemID + "</ItemID>";
                XMLItems += "<SalesOrderID>" + item.SalesOrderID + "</SalesOrderID>";
                XMLItems += "<SalesOrderItemTransID>" + item.SalesOrderItemTransID + "</SalesOrderItemTransID>";
                XMLItems += "<CounterSalesID>" + item.CounterSalesID + "</CounterSalesID>";
                XMLItems += "<CounterSalesItemTransID>" + item.CounterSalesItemTransID + "</CounterSalesItemTransID>";
                XMLItems += "<SalesInvoiceID>" + item.SalesInvoiceID + "</SalesInvoiceID>";
                XMLItems += "<SalesInvoiceTransID>" + item.SalesInvoiceTransID + "</SalesInvoiceTransID>";
                XMLItems += "<ItemCode>" + item.Code + "</ItemCode>";
                XMLItems += "<ItemName>" + item.Name + "</ItemName>";
                XMLItems += "<PartsNumber>" + item.PartsNumber + "</PartsNumber>";
                XMLItems += "<Remarks>" + item.Remarks + "</Remarks>";
                XMLItems += "<Model>" + item.Model + "</Model>";
                XMLItems += "<IsGST>" + item.IsGST + "</IsGST>";
                XMLItems += "<IsVat>" + item.IsVat + "</IsVat>";
                XMLItems += "<PrintWithItemName>" + item.PrintWithItemName + "</PrintWithItemName>";
                XMLItems += "<VATPercentage>" + item.VATPercentage + "</VATPercentage>";
                XMLItems += "<CurrencyID>" + item.CurrencyID + "</CurrencyID>";
                XMLItems += "<BatchID>" + item.BatchID + "</BatchID>";
                XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<MRP>" + item.MRP + "</MRP>";
                XMLItems += "<BasicPrice>" + item.BasicPrice + "</BasicPrice>";
                XMLItems += "<Quantity>" + item.Qty + "</Quantity>";
                XMLItems += "<SecondaryMRP>" + item.SecondaryMRP + "</SecondaryMRP>";
                XMLItems += "<SecondaryQty>" + item.SecondaryQty + "</SecondaryQty>";
                XMLItems += "<SecondaryUnit>" + item.SecondaryUnit + "</SecondaryUnit>";
                XMLItems += "<OfferQty>" + item.OfferQty + "</OfferQty>";
                XMLItems += "<InvoiceQty>" + item.InvoiceQty + "</InvoiceQty>";
                XMLItems += "<InvoiceOfferQty>" + item.InvoiceOfferQty + "</InvoiceOfferQty>";
                XMLItems += "<GrossAmount>" + item.GrossAmount + "</GrossAmount>";
                XMLItems += "<DiscountPercentage>" + item.DiscountPercentage + "</DiscountPercentage>";
                XMLItems += "<DiscountAmount>" + item.DiscountAmount + "</DiscountAmount>";
                XMLItems += "<AdditionalDiscount>" + item.AdditionalDiscount + "</AdditionalDiscount>";
                XMLItems += "<TaxableAmount>" + item.TaxableAmount + "</TaxableAmount>";
                XMLItems += "<SGSTPercentage>" + item.SGSTPercentage + "</SGSTPercentage>";
                XMLItems += "<CGSTPercentage>" + item.CGSTPercentage + "</CGSTPercentage>";
                XMLItems += "<IGSTPercentage>" + item.IGSTPercentage + "</IGSTPercentage>";
                XMLItems += "<SGSTAmt>" + item.SGST + "</SGSTAmt>";
                XMLItems += "<CGSTAmt>" + item.CGST + "</CGSTAmt>";
                XMLItems += "<IGSTAmt>" + item.IGST + "</IGSTAmt>";
                XMLItems += "<NetAmount>" + item.NetAmount + "</NetAmount>";
                XMLItems += "<StoreID>" + item.StoreID + "</StoreID>";
                XMLItems += "<SortOrder>" + i + "</SortOrder>";
                XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLItems += "<CessAmount>" + item.CessAmount + "</CessAmount>";
                XMLItems += "<CessPercentage>" + item.CessPercentage + "</CessPercentage>";
                XMLItems += "</Item>";
            }
            XMLItems += "</InvoiceTrans>";
            if (Invoice.ID == 0)
            {
                return salesGoodsReceiptNoteDAL.Save(XMLInvoice, XMLItems);
            }
            else
            {
                return salesGoodsReceiptNoteDAL.Update(XMLInvoice, XMLItems, Invoice.ID);
            }
        }

        public List<SalesGoodsReceiptItemBO> GetGoodReceiptNotes1(string salesOrderIDs)
        {
            return salesGoodsReceiptNoteDAL.GetGoodReceiptNotes1(salesOrderIDs);
        }

        public SalesGoodsReceiptBO GetGoodReceiptNotes(string counterSalesIDs)
        {
            return salesGoodsReceiptNoteDAL.GetGoodReceiptNotes(counterSalesIDs);
        }

       

        //public SalesGoodsReceiptBO GetGoodReceiptNotes(int CounterSalesIDs, int FinYear, int LocationID, int ApplicationID)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<SalesGoodsReceiptItemBO> GetGoodReceiptNotes1(string SalesOrderIDs, int FinYear, int LocationID, int ApplicationID)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
