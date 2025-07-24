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
    public class SalesOrderBL : ISalesOrder
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        private int ItemLengthLimit;
        SalesOrderDAL salesOrderDAL;
        SalesOrderBO salesOrder;
        CustomerDAL customerDAL;
        CustomerBO customerBO;
        PrintHelper PHelper;
        List<SalesItemBO> salesItemBO;
        private IGeneralContract generalBL;
        public SalesOrderBL()
        {
            salesOrderDAL = new SalesOrderDAL();
            customerDAL = new CustomerDAL();
            PHelper = new PrintHelper();
            generalBL = new GeneralBL();
        }

        public void Cancel(int SalesOrderID)
        {
            salesOrderDAL.Cancel(SalesOrderID);
        }

        public bool SaveSalesOrder(SalesOrderBO SalesOrder, List<SalesItemBO> Items)
        {
            string XMLItems = "<SalesOrderTrans>";
            foreach (var item in Items)
            {
                XMLItems += "<Item>";
                XMLItems += "<FullOrLoose>" + 1 + "</FullOrLoose>";
                XMLItems += "<ItemID>" + item.ID + "</ItemID>";
                XMLItems += "<ItemName>" + item.ItemName + "</ItemName>";
                XMLItems += "<Quantity>" + item.Qty + "</Quantity>";
                XMLItems += "<MRP>" + item.MRP + "</MRP>";
                XMLItems += "<BasicPrice>" + item.BasicPrice + "</BasicPrice>";
                XMLItems += "<OfferQty>" + item.OfferQty + "</OfferQty>";
                XMLItems += "<GrossAmount>" + item.GrossAmount + "</GrossAmount>";
                XMLItems += "<DiscountID>" + 1 + "</DiscountID>";
                XMLItems += "<DiscountPercentage>" + item.DiscountPercentage + "</DiscountPercentage>";
                XMLItems += "<DiscountAmount>" + item.DiscountAmount + "</DiscountAmount>";
                XMLItems += "<AdditionalDiscount>" + item.AdditionalDiscount + "</AdditionalDiscount>";
                XMLItems += "<TaxableAmount>" + item.TaxableAmount + "</TaxableAmount>";
                XMLItems += "<SGSTPercentage>" + item.SGSTPercentage + "</SGSTPercentage>";
                XMLItems += "<CGSTPercentage>" + item.CGSTPercentage + "</CGSTPercentage>";
                XMLItems += "<IGSTPercentage>" + item.IGSTPercentage + "</IGSTPercentage>";
                XMLItems += "<VATAmount>" + item.VATAmount + "</VATAmount>";
                XMLItems += "<VATPercentage>" + item.VATPercentage + "</VATPercentage>";
                XMLItems += "<PartsNumber>" + item.PartsNumber + "</PartsNumber>";
                XMLItems += "<Model>" + item.Model + "</Model>";
                XMLItems += "<DeliveryTerm>" + item.DeliveryTerm + "</DeliveryTerm>";
                XMLItems += "<ExchangeRate>" + item.ExchangeRate + "</ExchangeRate>";
                XMLItems += "<CurrencyID>" + item.CurrencyID + "</CurrencyID>";
                XMLItems += "<IsGST>" + item.IsGST + "</IsGST>";
                XMLItems += "<IsVat>" + item.IsVat + "</IsVat>";
                XMLItems += "<SGSTAmt>" + item.SGST + "</SGSTAmt>";
                XMLItems += "<CGSTAmt>" + item.CGST + "</CGSTAmt>";
                XMLItems += "<IGSTAmt>" + item.IGST + "</IGSTAmt>";
                XMLItems += "<CessPercentage>" + item.CessPercentage + "</CessPercentage>";
                XMLItems += "<CessAmount>" + item.CessAmount + "</CessAmount>";
                XMLItems += "<NetAmt>" + item.NetAmount + "</NetAmt>";
                XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLItems += "<SecondaryMRP>" + item.SecondaryMRP + "</SecondaryMRP>";
                XMLItems += "<SecondaryUnit>" + item.SecondaryUnit + "</SecondaryUnit>";
                XMLItems += "<SecondaryQty>" + item.SecondaryQty + "</SecondaryQty>";
                XMLItems += "<SecondaryOfferQty>" + item.SecondaryOfferQty + "</SecondaryOfferQty>";
                XMLItems += "<SecondaryUnitSize>" + item.SecondaryUnitSize + "</SecondaryUnitSize>";
                XMLItems += "</Item>";
            }
            XMLItems += "</SalesOrderTrans>";

            if (SalesOrder.ID == 0)
            {
                return salesOrderDAL.SaveSalesOrder(SalesOrder, XMLItems);
            }
            else
            {
                return salesOrderDAL.UpdateSalesOrder(SalesOrder, XMLItems);
            }

        }

        public DiscountAndOfferBO GetDiscountAndOfferDetails(int CustomerID, int SchemeID, int ItemID, decimal Qty, int UnitID)
        {
            return salesOrderDAL.GetDiscountAndOfferDetails(CustomerID, SchemeID, ItemID, Qty, UnitID);
        }

        public List<SalesItemBO> GetSalesOrderItems(int SalesOrderID)
        {
            return salesOrderDAL.GetSalesOrderItems(SalesOrderID);
        }

        public List<SalesItemBO> GetBatchwiseSalesOrderItems(int[] SalesOrderID, int StoreID, int CustomerID, int SchemeID)
        {
            string CommaSeparatedSalesOrderIDs = string.Join(",", SalesOrderID.Select(x => x.ToString()).ToArray());
            List<SalesItemBO> Items = salesOrderDAL.GetBatchwiseSalesOrderItems(CommaSeparatedSalesOrderIDs, StoreID, CustomerID, SchemeID);
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
            return salesOrderDAL.GetGoodsReceiptSalesOrderItems(CommaSeparatedSalesOrderIDs);
        }
        public SalesOrderBO GetSalesOrder(int ID)
        {
            return salesOrderDAL.GetSalesOrder(ID);
        }

        public int GetSchemeAllocation(int CustomerID)
        {
            return salesOrderDAL.GetSchemeAllocation(CustomerID);
        }

        public DatatableResultBO GetSalesOrderList(int CustomerID, int ItemCategoryID, string SalesType, string CodeHint, string DateHint, string CustomerNameHint, string SalesTypeHint, string DespatchDateHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesOrderDAL.GetSalesOrderList(CustomerID, ItemCategoryID, SalesType, CodeHint, DateHint, CustomerNameHint, SalesTypeHint, DespatchDateHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetCustomerSalesOrderList(int CustomerID, string TransNo, string TransDateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesOrderDAL.GetCustomerSalesOrderList(CustomerID, TransNo, TransDateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }


        public List<DiscountAndOfferBO> GetOfferDetails(int CustomerID, int SchemeID, int[] ItemID, int[] UnitID)
        {
            return salesOrderDAL.GetOfferDetails(CustomerID, SchemeID, ItemID, UnitID);
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

        public void ProcessOrder(SalesOrderBO SalesOrder, List<SalesItemBO> Items)
        {
            CustomerBL CustomerBL = new CustomerBL();
            ItemBL ItemBL = new ItemBL();
            AddressBL AddressBL = new AddressBL();

            decimal GSTAmount;
            decimal NetAmount;
            try
            {
                CustomerBO Customer = CustomerBL.GetCustomer(SalesOrder.CustomerID);
                SalesOrder.SODate = DateTime.Today;

                SalesOrder.SchemeAllocationID = Customer.SchemeID;
                SalesOrder.DespatchDate = DateTime.Today;

                SalesOrder.StateID = AddressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;

                int[] ItemIDs = Items.Select(a => a.ItemID).ToArray();
                int[] UnitIDs = Items.Select(a => a.UnitID).ToArray();
                List<ItemRateBO> ItemRates = ItemBL.GetRateOfItems(ItemIDs, Customer.PriceListID);
                List<DiscountAndOfferBO> OfferDetails = GetOfferDetails(SalesOrder.CustomerID, SalesOrder.SchemeAllocationID, ItemIDs, UnitIDs);

                Items = Items.Select(item =>
                {
                    item.ID = item.ItemID;
                    item.DiscountPercentage = salesOrderDAL.GetDiscountPercentage(SalesOrder.CustomerID, item.ItemID);
                    item.OfferQty = GetOfferQuantity(item.Qty, OfferDetails.Where(o => o.ItemID == item.ItemID).FirstOrDefault().OfferDetails);
                    Debug.WriteLine(item.ItemID);
                    try
                    {

                        if (item.ItemCategoryID == 222)
                        {
                            item.BatchTypeID = item.BatchTypeID == 0 ? Customer.PriceListID : item.BatchTypeID;
                        }
                        else
                        {
                            item.BatchTypeID = 0;
                        }

                        item.MRP = ItemRates.Where(i => i.ItemID == item.ItemID && (i.BatchTypeID == item.BatchTypeID || i.BatchTypeID == 0)).FirstOrDefault().Rate;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Price not found for " + item.Name, e);
                    }
                    if (IsCessEffect(Customer))
                    {
                        item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage);
                    }
                    else
                    {
                        item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage);
                    }

                    item.GrossAmount = item.BasicPrice * (item.Qty + item.OfferQty);
                    item.DiscountAmount = item.Qty * item.BasicPrice * item.DiscountPercentage / 100;
                    item.AdditionalDiscount = item.BasicPrice * item.OfferQty;
                    item.TaxableAmount = item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount;
                    if (IsCessEffect(Customer))
                    {
                        item.CessAmount = item.TaxableAmount * item.CessPercentage / 100;
                    }
                    else
                    {
                        item.CessAmount = 0;
                    }
                    GSTAmount = item.TaxableAmount * item.IGSTPercentage / 100;
                    item.NetAmount = item.TaxableAmount + GSTAmount + item.CessAmount;

                    if (Customer.StateID == SalesOrder.StateID)
                    {
                        item.GSTPercentage = item.IGSTPercentage;
                        item.CGST = GSTAmount / 2;
                        item.SGST = GSTAmount / 2;
                        item.IGST = 0;
                    }
                    else
                    {
                        item.GSTPercentage = item.IGSTPercentage;
                        item.CGST = 0;
                        item.SGST = 0;
                        item.IGST = GSTAmount;
                    }

                    return item;
                }).ToList();

                SalesOrder.GrossAmount = Items.Sum(a => a.GrossAmount);
                SalesOrder.DiscountAmount = Items.Sum(a => a.DiscountAmount);
                SalesOrder.TaxableAmount = Items.Sum(a => a.TaxableAmount);
                SalesOrder.SGSTAmount = Items.Sum(a => a.SGST);
                SalesOrder.CGSTAmount = Items.Sum(a => a.CGST);
                SalesOrder.IGSTAmount = Items.Sum(a => a.IGST);
                NetAmount = Items.Sum(a => a.NetAmount);
                SalesOrder.NetAmount = Math.Round(NetAmount);
                SalesOrder.RoundOff = NetAmount - SalesOrder.NetAmount;
                SalesOrder.IsDraft = true;
                SalesOrder.ItemCategoryID = Items.FirstOrDefault().ItemCategoryID;

                SaveSalesOrder(SalesOrder, Items);

            }
            catch (Exception e)
            {
                throw e;
            }
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
            return salesOrderDAL.IsCancelable(SalesOrderID);
        }

        public void Approve(int SalesOrderID)
        {
            salesOrderDAL.Approve(SalesOrderID);
        }
        //public int SuspendSalesOrder(int ID,string Name)
        //{
        //    return salesOrderDAL.SuspendSalesOrder(ID, Name);
        //}

        public string GetPrintTextFile(int SalesOrderID)
        {
            BodyLines = Convert.ToInt32(40);
            ItemLengthLimit = 32;
            string FileName;
            string FilePath;
            string URL;
            salesOrder = new SalesOrderBO();
            salesOrder = salesOrderDAL.GetSalesOrder(SalesOrderID);
            customerBO = customerDAL.GetCustomerDetails(salesOrder.CustomerID);
            salesItemBO = GetSalesOrderItems(SalesOrderID);
            string TransNo = salesOrder.SONo;
            FileName = TransNo + ".txt";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FilePath = path + "/Outputs/SalesOrder/" + FileName;
            TotalLines = getNoOfPrintLines(salesItemBO);
            SetPageVariables();
            PrintedLines = 0;
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintSalesOrderHeader(writer);
                PrintSalesOrderBody(writer);
                PrintSalesOrderFooter(writer);
            }
            URL = "/Outputs/SalesOrder/" + FileName;
            return URL;
        }

        public int getNoOfPrintLines(List<SalesItemBO> salesItemBOList)
        {
            int count = 0;
            var category = "";
            foreach (SalesItemBO item in salesItemBOList)
            {
                count++;
                if (category != item.Category)
                {
                    category = item.Category;
                    count++;
                }
                if (item.Name.Length > ItemLengthLimit)
                {
                    count++;
                }
                if (PrintedLines == BodyLines)
                {
                    PrintedLines = 0;
                }
                string name = PHelper.SplitString(item.Name, ItemLengthLimit);
                string[] itemname = name.Split(new char[] { '\r' }, 2);
                if (itemname[1] != " " && (PrintedLines == BodyLines - 1))
                {
                    count++;
                    PrintedLines = 0;
                }
                ++PrintedLines;
                if (itemname[1] != " ")
                {
                    ++PrintedLines;
                }
            }
            return count;
        }

        private void PrintSalesOrderHeader(StreamWriter writer)
        {
            if (CurrentPages == 1)
            {
                int ReverseLines = Convert.ToInt32(generalBL.GetConfig("SalesOrderReverseLines"));
                string RLineFeed = string.Empty;
                for (int index = 1; index <= ReverseLines; ++index)
                    RLineFeed = RLineFeed + PHelper.ReverseLineFeed();
                writer.WriteLine(RLineFeed);
            }
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("SALES ORDER", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine();
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Customer Code  : " + PHelper.Bold(customerBO.Code), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Despatch Date     : " + PHelper.Bold(Convert.ToDateTime(salesOrder.DespatchDate).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Customer Name  : " + PHelper.Bold(salesOrder.CustomerName), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Total Order Value : " + PHelper.Bold(string.Format("{0:0.00}", salesOrder.NetAmount)), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-3}{1,-34}{2,-7}{3,-7}{4,-7}{5,-7}{6,-7}{7,-9}{8,-9}{9,-6}{10,-8}{11,-8}{12,-8}{13,-8}{14,-9}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl.", "| Item", "| UOM", "| Qty", "| MRP", "| Basic", "| Offer", "| Addnl", "| Trade", "| GST", "| CGST", "| SGST", "| IGST", "| Cess", "| Net");
            string HeadLine2 = string.Format(HeadingTemplate, "No.", "|", "|", "|", "|", "|", "| Qty", "| Disc", "| Disc", "| Rate", "|", "|", "|", "|", "| Amount");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.Condense(HeadLine2));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        private void PrintSalesOrderBody(StreamWriter writer)
        {
            int FooterLines = 3;
            int PrintLines = salesItemBO.Count();
            var category = "";
            var i = 0;
            string ItemLineTemplate = "{0,-4}{1,-34}{2,-7}{3,6}{4,7}{5,7}{6,7}{7,9}{8,9}{9,6}{10,8}{11,8}{12,8}{13,8}{14,9}";
            while (true)
            {
                PrintedLines++;
                if (category != salesItemBO[i].Category)
                {
                    category = salesItemBO[i].Category;
                    string Content1 = string.Format(ItemLineTemplate,
                    "",
                    category,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "");
                    writer.WriteLine(PHelper.Condense(PHelper.Bold(Content1)));
                    PrintedLines++;
                }
                ItemLengthLimit = 32;
                string name = PHelper.SplitString(salesItemBO[i].PrintWithItemCode ? salesItemBO[i].Name : salesItemBO[i].PartsNumber, ItemLengthLimit);
                string[] itemname = name.Split(new char[] { '\r' }, 2);
                string Content = string.Format(ItemLineTemplate,
                            ((i + 1)).ToString(),
                            itemname[0],
                            salesItemBO[i].Unit,
                            string.Format("{0:0}", salesItemBO[i].Qty),
                            string.Format("{0:0.00}", salesItemBO[i].MRP),
                            string.Format("{0:0.00}", salesItemBO[i].BasicPrice),
                            string.Format("{0:0}", salesItemBO[i].OfferQty),
                            string.Format("{0:0.00}", salesItemBO[i].AdditionalDiscount),
                            string.Format("{0:0.00}", salesItemBO[i].DiscountAmount),
                            string.Format("{0:0.00}", salesItemBO[i].GSTPercentage),
                            string.Format("{0:0.00}", salesItemBO[i].CGST),
                            string.Format("{0:0.00}", salesItemBO[i].SGST),
                            string.Format("{0:0.00}", salesItemBO[i].IGST),
                            string.Format("{0:0.00}", salesItemBO[i].CessAmount),
                            string.Format("{0:0.00}", salesItemBO[i].NetAmount));
                writer.WriteLine(PHelper.Condense(Content));
                if (itemname[1] != " ")
                {
                    string Content2 = string.Format(ItemLineTemplate,
                    "",
                    itemname[1],
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "");
                    writer.WriteLine(PHelper.Condense(Content2));
                    PrintedLines++;
                }
                i++;
                if (PrintedLines >= BodyLines)
                {
                    PrintSalesOrderFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintSalesOrderHeader(writer);
                    PrintedLines = 0;
                }
                if (i == PrintLines)
                {
                    break;
                }
            }
        }

        private void PrintSalesOrderFooter(StreamWriter writer)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;
            var packs = salesItemBO.OrderBy(a => a.Category).GroupBy(g => Convert.ToInt16(g.PackSize).ToString() + " " + g.SecondaryUnit)
                .Select(a => new { Count = a.Count(), Pack = a.Key, TotalQuantity = a.Sum(b => b.InvoiceQty), OfferQuantity = a.Sum(c => c.InvoiceOfferQty) });
            if (CurrentPages == TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("SalesOrderForwardLines"));
                int BlankLines = BodyLines - PrintedLines;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.FormFeed());
            }
            else
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("SalesOrderContinueForwardLines"));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Continue..", PrintHelper.CharAlignment.AlignRight, 137)));
                for (int index = 1; index <= ForwardLines; ++index)
                    FLineFeed = FLineFeed + PHelper.ForwardLineFeed();
                writer.WriteLine(FLineFeed);
            }
        }
        public DatatableResultBO GetSalesOrderHistory(string Type, int ItemID, string SalesOrderNo, string OrderDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesOrderDAL.GetSalesOrderHistory(Type, ItemID, SalesOrderNo, OrderDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
        }
        public void SetPageVariables()
        {
            if (BodyLines == 0)
                throw new Exception("Body length for the Print Invoice is not Set.\nPlease set its value in Parameters");
            TotalPages = TotalLines / BodyLines;
            CurrentPages = 1;
            if (TotalLines % BodyLines == 0)
                return;
            ++TotalPages;
        }
    }
}
