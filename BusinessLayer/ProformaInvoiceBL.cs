using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using System.IO;
using DataAccessLayer.DBContext;

namespace BusinessLayer
{
    public class ProformaInvoiceBL : IProformaInvoice
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        ProformaInvoiceDAL proformaInvoiceDAL;
        ProformaInvoiceBO proformaInvoice;
        PrintHelper PHelper;
        List<SalesItemBO> salesItemBO;
        private IGeneralContract generalBL;

        public ProformaInvoiceBL()
        {
            proformaInvoiceDAL = new ProformaInvoiceDAL();
            PHelper = new PrintHelper();
            generalBL = new GeneralBL();
        }

        public void Cancel(int ProformaInvoiceID)
        {
            proformaInvoiceDAL.Cancel(ProformaInvoiceID);
        }

        public List<SalesBatchBO> GetItemBatchwise(int ItemID, decimal Qty, decimal OfferQty, int StoreID, int CustomerID, int UnitID)
        {
            List<SalesBatchBO> BatchwiseItems = proformaInvoiceDAL.GetItemBatchwise(ItemID, Qty, OfferQty, StoreID, CustomerID, UnitID);
            decimal InvoiceOfferQty = BatchwiseItems.Sum(a => a.InvoiceOfferQty);
            decimal InvoiceQty = BatchwiseItems.Sum(a => a.InvoiceQty);

            if (InvoiceQty == Qty + OfferQty)
            {
                foreach (var Item in BatchwiseItems.Where(a => a.ItemID == ItemID))
                {
                    Item.InvoiceQtyMet = true;
                    Item.InvoiceOfferQtyMet = true;
                }
            }
            else if (InvoiceQty >= Qty)
            {
                foreach (var Item in BatchwiseItems.Where(a => a.ItemID == ItemID))
                {
                    Item.InvoiceQtyMet = true;
                    Item.InvoiceOfferQtyMet = false;
                }
            }
            else if (InvoiceQty < Qty)
            {
                foreach (var Item in BatchwiseItems.Where(a => a.ItemID == ItemID))
                {
                    Item.InvoiceQtyMet = false;
                    Item.InvoiceOfferQtyMet = false;
                }
            }

            return BatchwiseItems;
        }

        public ProformaInvoiceBO GetProformaInvoice(int ProformaInvoiceID)
        {
            return proformaInvoiceDAL.GetProformaInvoice(ProformaInvoiceID);
        }

        public List<SalesAmountBO> GetProformaInvoiceAmountDetails(int ProformaInvoiceID)
        {
            return proformaInvoiceDAL.GetProformaInvoiceAmountDetails(ProformaInvoiceID);
        }

        public List<SalesPackingDetailsBO> GetProformaInvoicePackingDetails(int ProformaInvoiceID)
        {
            return proformaInvoiceDAL.GetProformaInvoicePackingDetails(ProformaInvoiceID);
        }

        public List<SalesItemBO> GetProformaInvoiceItems(int ProformaInvoiceID)
        {
            string StringProformaInvoiceID = ProformaInvoiceID.ToString();
            List<SalesItemBO> Items = proformaInvoiceDAL.GetProformaInvoiceItems(StringProformaInvoiceID, "Proforma");
            int[] ItemIDs = Items.Select(a => a.ItemID).Distinct().ToArray();
            List<SalesItemBO> GroupedItems;
            decimal InvoiceQty = 0;
            decimal OrderQty = 0;
            decimal OfferQty = 0;
            foreach (int ItemID in ItemIDs)
            {
                InvoiceQty = Items.Where(a => a.ItemID == ItemID).Sum(a => a.InvoiceQty);
                GroupedItems = Items.Where(a => a.ItemID == ItemID).GroupBy(a => a.SalesOrderItemID).Select(a => a.First()).ToList();
                OrderQty = GroupedItems.Sum(a => a.Qty);
                OfferQty = GroupedItems.Sum(a => a.OfferQty);
                if (InvoiceQty >= OrderQty + OfferQty)
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
                        Item.InvoiceOfferQtyMet = false;
                    }
                }

            }


            return Items;
        }

        public List<SalesItemBO> GetProformaInvoiceItems(int[] ProformaInvoiceID)
        {
            string CommaSeperatedProformaInvoiceIDs = string.Join(",", ProformaInvoiceID.Select(x => x.ToString()).ToArray());
            return proformaInvoiceDAL.GetProformaInvoiceItems(CommaSeperatedProformaInvoiceIDs, "Sales");
        }

        public DatatableResultBO GetProformaInvoiceList(string CodeHint, string DateHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string InvoiceType, int ItemCategoryID, int CustomerID, string SortField, string SortOrder, int Offset, int Limit)
        {
            return proformaInvoiceDAL.GetProformaInvoiceList(CodeHint, DateHint, CustomerNameHint, LocationHint, NetAmountHint, InvoiceType, ItemCategoryID, CustomerID, SortField, SortOrder, Offset, Limit);
        }

        public int Save(ProformaInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails, List<SalesPackingDetailsBO> PackingDetails)
        {
            string XMLInvoice;
            string XMLItems;
            string XMLAmountDetails;
            string XMLPackingDetails;

            XMLInvoice = "<Invoices>";
            XMLInvoice += "<Invoice>";
            XMLInvoice += "<TransNo>" + Invoice.TransNo + "</TransNo>";
            XMLInvoice += "<SalesTypeID>" + Invoice.SalesTypeID + "</SalesTypeID>";
            XMLInvoice += "<TransDate>" + Invoice.TransDate + "</TransDate>";
            XMLInvoice += "<CustomerID>" + Invoice.CustomerID + "</CustomerID>";
            XMLInvoice += "<SalesOrderNos>" + Invoice.SalesOrderNos + "</SalesOrderNos>";
            XMLInvoice += "<PaymentModeID>" + Invoice.PaymentModeID + "</PaymentModeID>";
            XMLInvoice += "<PaymentTypeID>" + Invoice.PaymentTypeID + "</PaymentTypeID>";
            XMLInvoice += "<SchemeID>" + Invoice.SchemeID + "</SchemeID>";
            XMLInvoice += "<DespatchDate>" + Invoice.DespatchDate + "</DespatchDate>";
            XMLInvoice += "<GrossAmount>" + Invoice.GrossAmount + "</GrossAmount>";
            XMLInvoice += "<FreightAmount>" + Invoice.FreightAmount + "</FreightAmount>";
            XMLInvoice += "<DiscountAmount>" + Invoice.DiscountAmount + "</DiscountAmount>";
            XMLInvoice += "<DiscountPercentage>" + Invoice.DiscountPercentage + "</DiscountPercentage>";
            XMLInvoice += "<VATAmount>" + Invoice.VATAmount + "</VATAmount>";
            XMLInvoice += "<VATPercentage>" + Invoice.VATPercentage + "</VATPercentage>";
            XMLInvoice += "<TurnoverDiscount>" + Invoice.TurnoverDiscount + "</TurnoverDiscount>";
            XMLInvoice += "<AdditionalDiscount>" + Invoice.AdditionalDiscount + "</AdditionalDiscount>";
            XMLInvoice += "<TaxableAmount>" + Invoice.TaxableAmount + "</TaxableAmount>";
            XMLInvoice += "<CGSTAmount>" + Invoice.CGSTAmount + "</CGSTAmount>";
            XMLInvoice += "<SGSTAmount>" + Invoice.SGSTAmount + "</SGSTAmount>";
            XMLInvoice += "<IGSTAmount>" + Invoice.IGSTAmount + "</IGSTAmount>";
            XMLInvoice += "<RoundOff>" + Invoice.RoundOff + "</RoundOff>";
            XMLInvoice += "<NetAmount>" + Invoice.NetAmount + "</NetAmount>";
            XMLInvoice += "<IsProcessed>" + Invoice.IsProcessed + "</IsProcessed>";
            XMLInvoice += "<IsDraft>" + Invoice.IsDraft + "</IsDraft>";
            XMLInvoice += "<BillingAddressID>" + Invoice.BillingAddressID + "</BillingAddressID>";
            XMLInvoice += "<ShippingAddressID>" + Invoice.ShippingAddressID + "</ShippingAddressID>";
            XMLInvoice += "<CheckStock>" + Invoice.CheckStock + "</CheckStock>";
            XMLInvoice += "<NoOfBags>" + Invoice.NoOfBags + "</NoOfBags>";
            XMLInvoice += "<NoOfBoxes>" + Invoice.NoOfBoxes + "</NoOfBoxes>";
            XMLInvoice += "<NoOfCans>" + Invoice.NoOfCans + "</NoOfCans>";
            XMLInvoice += "<CheckedBy>" + Invoice.CheckedBy + "</CheckedBy>";
            XMLInvoice += "<PackedBy>" + Invoice.PackedBy + "</PackedBy>";
            XMLInvoice += "<CreatedUserID>" + GeneralBO.CreatedUserID + "</CreatedUserID>";
            XMLInvoice += "<FinYear>" + GeneralBO.FinYear + "</FinYear>";
            XMLInvoice += "<LocationID>" + GeneralBO.LocationID + "</LocationID>";
            XMLInvoice += "<ApplicationID>" + GeneralBO.ApplicationID + "</ApplicationID>";
            XMLInvoice += "<CessAmount>" + Invoice.CessAmount + "</CessAmount>";
            XMLInvoice += "<PrintWithItemCode>" + (Invoice.PrintWithItemCode ? "1" : "0") + "</PrintWithItemCode>";
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
                XMLItems += "<ItemCode>" + item.Code + "</ItemCode>";
                XMLItems += "<ItemName>" + item.Name + "</ItemName>";
                XMLItems += "<PartsNumber>" + item.PartsNumber + "</PartsNumber>";
                XMLItems += "<DeliveryTerm>" + item.DeliveryTerm + "</DeliveryTerm>";
                XMLItems += "<Model>" + item.Model + "</Model>";
                XMLItems += "<IsGST>" + item.IsGST + "</IsGST>";
                XMLItems += "<IsVat>" + item.IsVat + "</IsVat>";
                XMLItems += "<PrintWithItemName>" + item.PrintWithItemCode + "</PrintWithItemName>";
                XMLItems += "<VATPercentage>" + item.VATPercentage + "</VATPercentage>";
                XMLItems += "<VATAmount>" + item.VATAmount + "</VATAmount>";
                XMLItems += "<SecondaryUnit>" + item.SecondaryUnit + "</SecondaryUnit>";
                XMLItems += "<SecondaryQty>" + item.SecondaryQty + "</SecondaryQty>";
                XMLItems += "<SecondaryMRP>" + item.SecondaryMRP + "</SecondaryMRP>";
                XMLItems += "<SecondaryUnitSize>" + item.SecondaryUnitSize + "</SecondaryUnitSize>";
                XMLItems += "<SecondaryOfferQty>" + item.SecondaryOfferQty + "</SecondaryOfferQty>";
                XMLItems += "<CurrencyID>" + item.CurrencyID + "</CurrencyID>";
                XMLItems += "<BatchID>" + item.BatchID + "</BatchID>";
                XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<SalesOrderItemID>" + item.SalesOrderItemID + "</SalesOrderItemID>";
                XMLItems += "<InvoiceTransID>" + item.ProformaInvoiceTransID + "</InvoiceTransID>";
                XMLItems += "<MRP>" + item.MRP + "</MRP>";
                XMLItems += "<BasicPrice>" + item.BasicPrice + "</BasicPrice>";
                XMLItems += "<Quantity>" + item.Qty + "</Quantity>";
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
                XMLItems += "<NetAmt>" + item.NetAmount + "</NetAmt>";
                XMLItems += "<StoreID>" + item.StoreID + "</StoreID>";
                XMLItems += "<SortOrder>" + i + "</SortOrder>";
                XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLItems += "<CessAmount>" + item.CessAmount + "</CessAmount>";
                XMLItems += "<CessPercentage>" + item.CessPercentage + "</CessPercentage>";
                XMLItems += "</Item>";
            }
            XMLItems += "</InvoiceTrans>";
            if (AmountDetails != null)
            {
                XMLAmountDetails = "<AmountDetails>";
                foreach (var item in AmountDetails)
                {
                    XMLAmountDetails += "<Item>";
                    XMLAmountDetails += "<Particulars>" + item.Particulars + "</Particulars>";
                    XMLAmountDetails += "<Amount>" + item.Amount + "</Amount>";
                    XMLAmountDetails += "<Percentage>" + item.Percentage + "</Percentage>";
                    XMLAmountDetails += "</Item>";
                }
                XMLAmountDetails += "</AmountDetails>";
            }
            else
            {
                XMLAmountDetails = "<AmountDetails></AmountDetails>";
            }
            XMLPackingDetails = "<PackingDetails>";
            foreach (var item in PackingDetails)
            {
                XMLPackingDetails += "<Item>";
                XMLPackingDetails += "<PackSize>" + item.PackSize + "</PackSize>";
                XMLPackingDetails += "<Quantity>" + item.Quantity + "</Quantity>";
                XMLPackingDetails += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLPackingDetails += "</Item>";
            }
            XMLPackingDetails += "</PackingDetails>";
            if (Invoice.ID == 0)
            {
                return proformaInvoiceDAL.Save(XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails);
            }
            else
            {
                return proformaInvoiceDAL.Update(XMLInvoice, XMLItems, XMLAmountDetails, Invoice.ID, XMLPackingDetails);
            }
        }

        public bool IsCancelable(int ProformaInvoiceID)
        {
            return proformaInvoiceDAL.IsCancelable(ProformaInvoiceID);
        }

        public CustomerCreditSummaryBO GetCustomerCreditSummary(int ProformaInvoiceID)
        {
            return proformaInvoiceDAL.GetCustomerCreditSummary(ProformaInvoiceID);
        }

        public string GetPrintTextFile(int proformaInvoiceID)
        {
            BodyLines = Convert.ToInt32(35);
            string FileName;
            string FilePath;
            string URL;
            proformaInvoice = new ProformaInvoiceBO();
            proformaInvoice = proformaInvoiceDAL.GetProformaInvoice(proformaInvoiceID);
            salesItemBO = GetProformaInvoiceItems(proformaInvoiceID);
            string TransNo = proformaInvoice.TransNo;
            FileName = TransNo + ".txt";

            string path = AppDomain.CurrentDomain.BaseDirectory;

            FilePath = path + "/Outputs/ProformaInvoice/" + FileName;
            TotalLines = getNoOfPrintLines(salesItemBO);
            SetPageVariables();
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintProformaInvoiceHeader(writer);
                PrintProformaInvoiceBody(writer);
                PrintProformaInvoiceFooter(writer);
            }
            URL = "/Outputs/ProformaInvoice/" + FileName;
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
            }
            return count;
        }

        public string encryption(string MalayalamName)
        {
            byte[] encrypt;
            encrypt = Encoding.UTF8.GetBytes(MalayalamName);
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
            //UTF8Encoding encode = new UTF8Encoding();
            ////encrypt the given password string into Encrypted data  
            //encrypt = encode.GetBytes(MalayalamName);
            //StringBuilder encryptdata = new StringBuilder();
            ////Create a new string by using the encrypted data  
            //for (int i = 0; i < encrypt.Length; i++)
            //{
            //    encryptdata.Append(encrypt[i].ToString());
            //}
            //return encryptdata.ToString();
        }

        private void PrintProformaInvoiceHeader(StreamWriter writer)
        {
            if (CurrentPages == 1)
            {
                int ReverseLines = Convert.ToInt32(generalBL.GetConfig("ProformaInvoiceReverseLines"));
                string RLineFeed = string.Empty;
                for (int index = 1; index <= ReverseLines; ++index)
                    RLineFeed = RLineFeed + PHelper.ReverseLineFeed();
                writer.WriteLine(RLineFeed);
            }
            writer.WriteLine(PHelper.Condense(PHelper.Bold("GST : " + GeneralBO.GSTNo)));
            writer.WriteLine(PHelper.Bold(PHelper.AlignText(GeneralBO.CompanyName.ToUpper(), PrintHelper.CharAlignment.AlignCenter, 80)));
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("PROFORMA INVOICE", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine();
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Invoice No.   : " + PHelper.Bold(proformaInvoice.TransNo), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Invoice Date : " + PHelper.Bold(Convert.ToDateTime(proformaInvoice.TransDate).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense("Customer Name : " + PHelper.Bold(PHelper.AlignText(proformaInvoice.CustomerName, PrintHelper.CharAlignment.AlignLeft, 80))));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-4}{1,-65}{2,-11}{3,-11}{4,-8}{5,-10}{6,-10}{7,-17}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl.", "| Item Name", "| Pack Size", "| Batch No.", "| Batch", "| UOM", "| Qty", "| MRP");
            string HeadLine2 = string.Format(HeadingTemplate, "No.", "|", "|", "|", "| Type", "|", "|", "|");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.Condense(HeadLine2));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        private void PrintProformaInvoiceBody(StreamWriter writer)
        {
            int FooterLines = 3;
            int PrintLines = salesItemBO.Count();
            var category = "";
            var i = 0;
            string ItemLineTemplate = "{0,-5}{1,-65}{2,-11}{3,-11}{4,-8}{5,-10}{6,10}{7,17}";
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
                    "");
                    writer.WriteLine(PHelper.Condense(PHelper.Bold(Content1)));
                    PrintedLines++;
                }
                string MalayalamName = encryption(salesItemBO[i].MalayalamName);
                string Content = string.Format(ItemLineTemplate,
                                ((i + 1)).ToString(),
                                salesItemBO[i].Name,
                                string.Format("{0:0}", salesItemBO[i].PackSize) + salesItemBO[i].SecondaryUnit,
                                salesItemBO[i].BatchName,
                                salesItemBO[i].BatchTypeName,
                                salesItemBO[i].Unit,
                                string.Format("{0:0}", salesItemBO[i].InvoiceQty),
                                string.Format("{0:0.00}", salesItemBO[i].MRP));
                writer.WriteLine(PHelper.Condense(Content));
                i++;
                if (PrintedLines >= BodyLines)
                {
                    PrintProformaInvoiceFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintProformaInvoiceHeader(writer);
                    PrintedLines = 0;
                }
                if (i == PrintLines)
                {
                    break;
                }
            }
        }

        private void PrintProformaInvoiceFooter(StreamWriter writer)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;

            var packs = salesItemBO.OrderBy(a => a.Category).GroupBy(g => Convert.ToInt16(g.PackSize).ToString() + " " + g.SecondaryUnit)
                .Select(a => new { Count = a.Count(), Pack = a.Key, TotalQuantity = a.Sum(b => b.InvoiceQty), OfferQuantity = a.Sum(c => c.InvoiceOfferQty) });
            if (CurrentPages == TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("ProformaInvoiceForwardLines"));
                int BlankLines = BodyLines - PrintedLines;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine();
                string FooterHeadTemplate = "{0,-10}{1,-10}";
                string FooterLineTemplate = "{0,-10}{1,10}";
                string FooterHeadContent = string.Format(FooterHeadTemplate, "Pack Size", "| Nos");
                writer.WriteLine(PHelper.FillChar('-', 20));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterHeadContent)));
                writer.WriteLine(PHelper.FillChar('-', 20));
                foreach (var pack in packs)
                {
                    string FooterContent = string.Format(FooterLineTemplate, pack.Pack, string.Format("{0:0}", pack.TotalQuantity));
                    writer.WriteLine(PHelper.Condense(FooterContent));
                }
                writer.WriteLine(PHelper.FillChar('-', 20));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Authorised Signatory", PrintHelper.CharAlignment.AlignRight, 137)));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("[With Status & Seal]", PrintHelper.CharAlignment.AlignRight, 137)));
                writer.WriteLine(PHelper.FormFeed());
            }
            else
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("ProformaInvoiceContinueForwardLines"));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Continue..", PrintHelper.CharAlignment.AlignRight, 137)));
                for (int index = 1; index <= ForwardLines; ++index)
                    FLineFeed = FLineFeed + PHelper.ForwardLineFeed();
                writer.WriteLine(FLineFeed);
            }
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
