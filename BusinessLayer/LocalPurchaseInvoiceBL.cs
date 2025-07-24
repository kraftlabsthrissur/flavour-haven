using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BusinessLayer
{
    public class LocalPurchaseInvoiceBL : ILocalPurchaseInvoiceContract
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        private int ItemLengthLimit;
        decimal TotalTaxableAmt = 0;
        decimal TotalGSTAmt = 0;
        decimal TotalCGSTAmt = 0;
        decimal TotalSGSTAmt = 0;
        decimal TotalIGSTAmt = 0;
        decimal SubTotalTaxableAmt = 0;
        decimal SubTotalCGSTAmt = 0;
        decimal SubTotalSGSTAmt = 0;
        decimal SubTotalIGSTAmt = 0;
        decimal SubTotalGSTAmt = 0;
        decimal SubNetTotal = 0;
        LocalPurchaseInvoiceDAL LocalPurchaseInvoiceDAL;
        LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO;
        List<PurchaseOrderTransBO> LocalPurchaseInvoiceItems;
        PrintHelper PHelper;
        private IGeneralContract generalBL;

        public LocalPurchaseInvoiceBL()
        {
            LocalPurchaseInvoiceDAL = new LocalPurchaseInvoiceDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
        }

        public int Save(LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO, List<PurchaseOrderTransBO> LocalPurchaseInvoiceItems)
        {
            string XMLItems = XMLHelper.Serialize(LocalPurchaseInvoiceItems);
            if (LocalPurchaseInvoiceBO.ID == 0)
            {
                return LocalPurchaseInvoiceDAL.Save(LocalPurchaseInvoiceBO, XMLItems);
            }
            else
            {
                return LocalPurchaseInvoiceDAL.Update(LocalPurchaseInvoiceBO, XMLItems);
            }
        }

        public DatatableResultBO GetLocalPurchases(string Type, string TransNoHint, string TransDateHint, string SupplierHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return LocalPurchaseInvoiceDAL.GetLocalPurchases(Type, TransNoHint, TransDateHint, SupplierHint, SortField, SortOrder, Offset, Limit);
        }

        public LocalPurchaseInvoiceBO GetLocalPurchaseOrder(int ID)
        {
            return LocalPurchaseInvoiceDAL.GetLocalPurchaseOrder(ID);
        }

        public List<PurchaseOrderTransBO> GetLocalPurchaseOrderItems(int ID)
        {
            return LocalPurchaseInvoiceDAL.GetLocalPurchaseOrderItems(ID);
        }

        public LocalPurchaseInvoiceBO GetLocalPurchaseID()
        {
            return LocalPurchaseInvoiceDAL.GetLocalPurchaseID();
        }

        public bool GetIsLocalPurchase(int id)
        {
            return LocalPurchaseInvoiceDAL.GetIsLocalPurchase(id);
        }

        public string GetPrintTextFile(int ID)
        {
            BodyLines = Convert.ToInt32(45);
            PrintedLines = 0;
            ItemLengthLimit = 40;
            string FileName;
            string FilePath;
            string URL;
            LocalPurchaseInvoiceBO = new LocalPurchaseInvoiceBO();
            LocalPurchaseInvoiceBO = LocalPurchaseInvoiceDAL.GetLocalPurchaseOrder(ID);
            LocalPurchaseInvoiceItems = GetLocalPurchaseOrderItems((int)ID);
            string TransNo = LocalPurchaseInvoiceBO.PurchaseOrderNo;
            FileName = TransNo + ".txt";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FilePath = path + "/Outputs/LocalPurchaseInvoice/" + FileName;
            TotalLines = getNoOfPrintLines(LocalPurchaseInvoiceItems);
            SetPageVariables();
            PrintedLines = 0;
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintLocalPurchaseInvoiceHeader(writer);
                PrintLocalPurchaseInvoiceBody(writer);
                PrintLocalPurchaseInvoiceFooter(writer);
            }
            URL = "/Outputs/LocalPurchaseInvoice/" + FileName;
            return URL;
        }

        public int getNoOfPrintLines(List<PurchaseOrderTransBO> purchaseOrderTransBOList)
        {
            int count = 0;
            foreach (PurchaseOrderTransBO item in purchaseOrderTransBOList)
            {
                count++;
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

        private void PrintLocalPurchaseInvoiceHeader(StreamWriter writer)
        {
            if (CurrentPages == 1)
            {
                int ReverseLines = Convert.ToInt32(generalBL.GetConfig("LocalPurchaseReverseLines"));
                string RLineFeed = string.Empty;
                for (int index = 1; index <= ReverseLines; ++index)
                    RLineFeed = RLineFeed + PHelper.ReverseLineFeed();
                writer.WriteLine(RLineFeed);
            }
            writer.WriteLine(PHelper.Bold(PHelper.AlignText(GeneralBO.CompanyName.ToUpper(), PrintHelper.CharAlignment.AlignCenter, 80)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address1 + "," + GeneralBO.Address2, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address3 + "," + GeneralBO.Address4, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address5 + "," + GeneralBO.PIN + "," + GeneralBO.LandLine1, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.Bold("GST : " + GeneralBO.GSTNo)));
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("Local Purchase Invoice", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine();
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Details of Supplier : " + PHelper.Bold(LocalPurchaseInvoiceBO.SupplierReference), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Transaction No    : " + PHelper.Bold(LocalPurchaseInvoiceBO.PurchaseOrderNo), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Reverse Charge      : " + PHelper.Bold("YES"), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Transaction Date  : " + PHelper.Bold(Convert.ToDateTime(LocalPurchaseInvoiceBO.PurchaseOrderDate).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-3}{1,-44}{2,-10}{3,-6}{4,-8}{5,-9}{6,-12}{7,-9}{8,-8}{9,-8}{10,-8}{11,-12}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl.", "| Item Name", "| HSN Code", "| UOM", "| Qty", "| Rate", "| Taxable", "| GST Amt", "", "", "", "| Net Total");
            string HeadLine2 = string.Format(HeadingTemplate, "No.", "|", "|", "|", "|", "|", "| Value", "| CGST", "| SGST", "| IGST", "| Total", "|");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.Condense(HeadLine2));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        private void PrintLocalPurchaseInvoiceBody(StreamWriter writer)
        {
            int FooterLines = 3;
            foreach (var items in LocalPurchaseInvoiceItems.Select((value, i) => new { i = (i + 1), value }))
            {
                string ItemLineTemplate = "{0,-4}{1,-43}{2,-10}{3,-6}{4,8}{5,9}{6,12}{7,9}{8,8}{9,8}{10,8}{11,12}";
                if (PrintedLines == BodyLines)
                {
                    PrintLocalPurchaseInvoiceFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintLocalPurchaseInvoiceHeader(writer);
                    PrintedLines = 0;
                    SubTotalTaxableAmt = 0;
                    SubTotalCGSTAmt = 0;
                    SubTotalSGSTAmt = 0;
                    SubTotalIGSTAmt = 0;
                    SubTotalGSTAmt = 0;
                    SubNetTotal = 0;
                }
                SubTotalTaxableAmt += (decimal)items.value.Amount;
                SubTotalCGSTAmt += (decimal)items.value.CGSTAmt;
                SubTotalSGSTAmt += (decimal)items.value.SGSTAmt;
                SubTotalIGSTAmt = 0;
                SubTotalGSTAmt += (decimal)(items.value.CGSTAmt + items.value.SGSTAmt);
                SubNetTotal += (decimal)items.value.Amount;
                TotalTaxableAmt += (decimal)items.value.Amount;
                TotalCGSTAmt += (decimal)items.value.CGSTAmt;
                TotalSGSTAmt += (decimal)items.value.SGSTAmt;
                TotalIGSTAmt = 0;
                TotalGSTAmt += (decimal)(items.value.CGSTAmt + items.value.SGSTAmt);
                ItemLengthLimit = 40;
                string name = PHelper.SplitString(items.value.Name, ItemLengthLimit);
                string[] itemname = name.Split(new char[] { '\r' }, 2);
                if (itemname[1] != " " && (PrintedLines == BodyLines - 1))
                {
                    writer.WriteLine();
                    PrintLocalPurchaseInvoiceFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintLocalPurchaseInvoiceHeader(writer);
                    PrintedLines = 0;
                    SubTotalTaxableAmt = 0;
                    SubTotalCGSTAmt = 0;
                    SubTotalSGSTAmt = 0;
                    SubTotalIGSTAmt = 0;
                    SubTotalGSTAmt = 0;
                    SubNetTotal = 0;
                }
                string Content = string.Format(ItemLineTemplate,
                    (items.i).ToString(),
                    itemname[0],
                    items.value.HSNCode,
                    items.value.Unit,
                    string.Format("{0:0}", items.value.QtyOrdered),
                    string.Format("{0:0.00}", items.value.Rate),
                    string.Format("{0:0.00}", items.value.Amount),
                    string.Format("{0:0.00}", items.value.CGSTAmt),
                    string.Format("{0:0.00}", items.value.SGSTAmt),
                    string.Format("{0:0.00}", 0),
                    string.Format("{0:0.00}", items.value.CGSTAmt + items.value.SGSTAmt),
                    string.Format("{0:0.00}", items.value.NetAmount));
                writer.WriteLine(PHelper.Condense(Content));
                ++PrintedLines;
                if (itemname[1] != "")
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
                    "");
                    writer.WriteLine(PHelper.Condense(Content2));
                    ++PrintedLines;
                }
            }
        }

        private void PrintLocalPurchaseInvoiceFooter(StreamWriter writer)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;
            if (CurrentPages == TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("LocalPurchaseForwardLines"));
                int BlankLines = BodyLines - PrintedLines;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                string FooterLineTemplate = "{0,-12}{1,-35}{2,-10}{3,-6}{4,8}{5,9}{6,12}{7,9}{8,8}{9,8}{10,8}{11,12}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Total :",
                "",
                "",
                "",
                "",
                "",
                string.Format("{0:0.00}", TotalTaxableAmt),
                string.Format("{0:0.00}", TotalCGSTAmt),
                string.Format("{0:0.00}", TotalSGSTAmt),
                string.Format("{0:0.00}", TotalIGSTAmt),
                string.Format("{0:0.00}", TotalGSTAmt),
                string.Format("{0:0.00}", LocalPurchaseInvoiceBO.NetAmount));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterContent)));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.Bold("Total Invoice Value In Words : " + generalBL.NumberToText(Convert.ToInt32(LocalPurchaseInvoiceBO.NetAmount)) + "Only")));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("", PrintHelper.CharAlignment.AlignLeft, 93)
                    + PHelper.AlignText("Invoice Amount                : ", PrintHelper.CharAlignment.AlignLeft, 32)
                    + PHelper.Bold(PHelper.AlignText(string.Format("{0:0.00}", LocalPurchaseInvoiceBO.NetAmount), PrintHelper.CharAlignment.AlignRight, 12))));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("", PrintHelper.CharAlignment.AlignLeft, 93)
                    + PHelper.AlignText("GST Payable on Reverse Charge : ", PrintHelper.CharAlignment.AlignLeft, 32)
                    + PHelper.Bold(PHelper.AlignText(string.Format("{0:0.00}", TotalGSTAmt), PrintHelper.CharAlignment.AlignRight, 12))));
                writer.WriteLine();
                writer.WriteLine(PHelper.FormFeed());
            }
            else
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("LocalPurchaseContinueForwardLines"));
                string FooterLineTemplate = "{0,-4}{1,-35}{2,-10}{3,-6}{4,8}{5,9}{6,12}{7,9}{8,8}{9,8}{10,8}{11,12}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Sub Total :",
                "",
                "",
                "",
                "",
                "",
                string.Format("{0:0.00}", SubTotalTaxableAmt),
                string.Format("{0:0.00}", SubTotalCGSTAmt),
                string.Format("{0:0.00}", SubTotalSGSTAmt),
                string.Format("{0:0.00}", SubTotalIGSTAmt),
                string.Format("{0:0.00}", SubTotalGSTAmt),
                string.Format("{0:0.00}", SubNetTotal));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterContent)));
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
