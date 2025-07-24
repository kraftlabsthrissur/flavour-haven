using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BusinessLayer
{
    public class SalesInquiryBL : ISalesInquiry
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        private int ItemLengthLimit;
        SalesInquiryDAL salesInquiryDAL;
        SalesOrderBO salesOrder;
        CustomerDAL customerDAL;
        CustomerBO customerBO;
        PrintHelper PHelper;
        List<SalesItemBO> salesItemBO;
        private IGeneralContract generalBL;
        public SalesInquiryBL()
        {
            salesInquiryDAL = new SalesInquiryDAL();
            customerDAL = new CustomerDAL();
            PHelper = new PrintHelper();
            generalBL = new GeneralBL();
        }

        public bool SaveSalesInquiry(SalesInquiryBO SalesInquiry, List<SalesItemBO> Items)
        {
            string XMLItems = "<SalesInquiryTrans>";
            foreach (var item in Items)
            {
                XMLItems += "<Item>";
                XMLItems += "<ItemID>" + item.ItemID + "</ItemID>";
                XMLItems += "<ItemCode>" + item.Code + "</ItemCode>";
                XMLItems += "<ItemName>" + item.ItemName + "</ItemName>";
                XMLItems += "<UnitName>" + item.UnitName + "</UnitName>";
                XMLItems += "<PartsNumber>" + item.PartsNumber + "</PartsNumber>";
                XMLItems += "<DeliveryTerm>" + item.DeliveryTerm + "</DeliveryTerm>";
                XMLItems += "<Year>" + item.Year + "</Year>";
                XMLItems += "<SIOrVINNumber>" + item.SIOrVINNumber + "</SIOrVINNumber>";
                XMLItems += "<Model>" + item.Model + "</Model>";
                XMLItems += "<Rate>" + item.Rate + "</Rate>";
                XMLItems += "<Remarks>" + item.Remarks + "</Remarks>";
                XMLItems += "<Qty>" + item.Qty + "</Qty>";
                XMLItems += "<GrossAmount>" + item.GrossAmount + "</GrossAmount>";
                XMLItems += "<VATPercentage>" + item.VATPercentage + "</VATPercentage>";
                XMLItems += "<VATAmount>" + item.VATAmount + "</VATAmount>";
                XMLItems += "<NetAmount>" + item.NetAmount + "</NetAmount>";
                XMLItems += "</Item>";
            }
            XMLItems += "</SalesInquiryTrans>";
            if (SalesInquiry.ID > 0)
                return salesInquiryDAL.UpdateSalesInquiry(SalesInquiry, XMLItems);
            else
                return salesInquiryDAL.SaveSalesInquiry(SalesInquiry, XMLItems);


        }

        public DiscountAndOfferBO GetDiscountAndOfferDetails(int CustomerID, int SchemeID, int ItemID, decimal Qty, int UnitID)
        {
            return salesInquiryDAL.GetDiscountAndOfferDetails(CustomerID, SchemeID, ItemID, Qty, UnitID);
        }
        public List<SalesItemBO> GetInqueryCustomerAutoComplete(string CustomerName)
        {
            return salesInquiryDAL.GetInqueryCustomerAutoComplete(CustomerName);
        }
        public List<SalesItemBO> GetSalesInquiryItems(int SalesOrderID)
        {
            return salesInquiryDAL.GetSalesInauiryItems(SalesOrderID);
        }
        public List<SalesItemBO> GetSalesInquiryItemsPurchaseRequisition(int SalesOrderID)
        {
            return salesInquiryDAL.GetSalesInquiryItemsPurchaseRequisition(SalesOrderID);
        }
        public List<SalesItemBO> GetBatchwiseSalesOrderItems(int[] SalesOrderID, int StoreID, int CustomerID, int SchemeID)
        {
            string CommaSeparatedSalesOrderIDs = string.Join(",", SalesOrderID.Select(x => x.ToString()).ToArray());
            List<SalesItemBO> Items = salesInquiryDAL.GetBatchwiseSalesOrderItems(CommaSeparatedSalesOrderIDs, StoreID, CustomerID, SchemeID);
            int[] ItemIDs = Items.Select(a => a.ItemID).Distinct().ToArray();
            List<SalesItemBO> GroupedItems;
            decimal InvoiceQty = 0;
            decimal OrderQty = 0;
            decimal OfferQty = 0;
            foreach (int ItemID in ItemIDs)
            {
                InvoiceQty = Math.Round(Items.Where(a => a.ItemID == ItemID).Sum(a => a.InvoiceQty), 2);
                GroupedItems = Items.Where(a => a.ItemID == ItemID).GroupBy(a => a.SalesOrderItemID).Select(a => a.First()).ToList();
                OrderQty = Math.Round(GroupedItems.Sum(a => a.Qty), 2);
                OfferQty = Math.Round(Items.Where(a => a.ItemID == ItemID).FirstOrDefault().ActualOfferQty, 2);
                if (InvoiceQty == OrderQty + OfferQty)
                {
                    foreach (var Item in Items.Where(a => a.ItemID == ItemID))
                    {
                        Item.InvoiceQtyMet = true;
                        Item.InvoiceOfferQtyMet = true;
                    }
                }
                else if (InvoiceQty >= OrderQty)
                {
                    foreach (var Item in Items.Where(a => a.ItemID == ItemID))
                    {
                        Item.InvoiceQtyMet = true;
                        Item.InvoiceOfferQtyMet = false;
                    }
                }
                else if (InvoiceQty < OrderQty)
                {
                    foreach (var Item in Items.Where(a => a.ItemID == ItemID))
                    {
                        Item.InvoiceQtyMet = false;
                        if (OfferQty == 0)
                        {
                            Item.InvoiceOfferQtyMet = true;
                        }
                    }
                }
            }
            return Items;
        }
        public List<SalesItemBO> GetGoodsReceiptSalesOrderItems(int[] SalesOrderID)
        {
            string CommaSeparatedSalesOrderIDs = string.Join(",", SalesOrderID.Select(x => x.ToString()).ToArray());
            return salesInquiryDAL.GetGoodsReceiptSalesOrderItems(CommaSeparatedSalesOrderIDs);
        }
        public SalesInquiryBO GetSalesInquiry(int ID)
        {
            return salesInquiryDAL.GetSalesInauiry(ID);
        }

        public int GetSchemeAllocation(int CustomerID)
        {
            return salesInquiryDAL.GetSchemeAllocation(CustomerID);
        }
        public int CheckItemCreatedForSalesInquiryItems(int SalesInquiryItemID)
        {
            return salesInquiryDAL.CheckItemCreatedForSalesInquiryItems(SalesInquiryItemID);
        }

        public DatatableResultBO GetAllSalesInquiryList(string Type, string SalesInquiryNo, string SalesInquiryDateHint, string RequestedCustomerNameHint, string PhoneNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInquiryDAL.GetAllSalesInquiryList(Type, SalesInquiryNo, SalesInquiryDateHint, RequestedCustomerNameHint, PhoneNo, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetSalesInquiryList(string Type, string SalesInquiryNo, string SalesInquiryDateHint, string RequestedCustomerNameHint, string PhoneNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInquiryDAL.GetSalesInquiryList(Type, SalesInquiryNo, SalesInquiryDateHint, RequestedCustomerNameHint, PhoneNo, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetCustomerSalesOrderList(string TransNo, string TransDateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInquiryDAL.GetCustomerSalesOrderList(TransNo, TransDateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }


        public List<DiscountAndOfferBO> GetOfferDetails(int CustomerID, int SchemeID, int[] ItemID, int[] UnitID)
        {
            return salesInquiryDAL.GetOfferDetails(CustomerID, SchemeID, ItemID, UnitID);
        }

        public List<UploadOrderBO> ReadExcel(string Path)
        {
            ItemContract itemBL = new ItemBL();
            ICustomerContract customerBL = new CustomerBL();
            IDictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(0, "PackingCode");
            dict.Add(1, "PackingName");
            dict.Add(2, "Qty");
            dict.Add(3, "OldCode");

            UploadOrderBO UploadOrder = new UploadOrderBO();
            GeneralBL generalBL = new GeneralBL();
            List<UploadOrderBO> OrderLines;
            List<UploadOrderBO> OrderLinesArranged;
            try
            {
                OrderLines = generalBL.ReadExcel(Path, UploadOrder, dict);
                var OrderLinesSorted = OrderLines.OrderBy(a => a.OldCode);
                var CustomerCodes = OrderLines.Select(a => a.OldCode).Distinct();
                var PackingCodes = OrderLines.Select(a => a.PackingCode).Distinct();

                List<ItemBO> Items = itemBL.GetItemsByPackingCodes(PackingCodes.ToArray());
                List<CustomerBO> Customers = customerBL.GetCustomersByOldCodes(CustomerCodes.ToArray());
                CustomerBO Customer;
                ItemBO Item;
                OrderLinesArranged = OrderLinesSorted.Select(a =>
                {
                    Customer = Customers.Where(c => c.OldCode == a.OldCode).FirstOrDefault();
                    Item = Items.Where(i => i.OldItemCode2 == a.PackingCode).FirstOrDefault();
                    if (Customer != null)
                    {
                        a.CustomerID = Customer.ID;
                        a.CustomerName = Customer.Name;
                        a.CustomerCode = Customer.Code;
                    }
                    if (Item != null)
                    {
                        a.ItemID = Item.ItemID;
                        a.ItemName = Item.Name;
                        a.ItemCode = Item.Code;
                        a.CGSTPercentage = Item.CGSTPercentage;
                        a.IGSTPercentage = Item.IGSTPercentage;
                        a.SGSTPercentage = Item.SGSTPercentage;
                        a.ItemCategoryID = Item.ItemCategoryID;
                        a.UnitID = Item.UnitID;
                        a.CessPercentage = Item.CessPercentage;
                    }
                    return a;
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return OrderLinesArranged;
        }

        private decimal GetOfferQuantity(decimal Qty, List<OfferBO> OfferDetails)
        {
            OfferBO Offer;
            decimal OfferQty = 0;
            while (Qty > 0)
            {
                try
                {
                    Offer = OfferDetails.Where(a => a.Qty <= Qty).OrderByDescending(a => a.Qty).FirstOrDefault();
                    OfferQty += Math.Floor(Qty / Offer.Qty) * Offer.OfferQty;
                    Qty -= Math.Floor(Qty / Offer.Qty) * Offer.Qty;
                }
                catch (Exception e)
                {
                    Qty = 0;
                }
            }
            return OfferQty;
        }

        public bool IsCessEffect(CustomerBO Customer)
        {

            if (Customer.IsGSTRegistered)
            {
                return false;
            }
            if (Customer.StateID != 32)
            {
                return false;
            }
            AddressBL addressBL = new AddressBL();
            if (addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID != 32)
            {
                return false;
            }
            return true;
        }

        public bool IsCancelable(int SalesOrderID)
        {
            return salesInquiryDAL.IsCancelable(SalesOrderID);
        }

        public void Approve(int SalesOrderID)
        {
            salesInquiryDAL.Approve(SalesOrderID);
        }


    }
}
