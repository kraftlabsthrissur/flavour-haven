using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class PrintBL
    {
        private int PrintedLines;
        decimal TotalGSTAmt = 0;
        decimal GSTAmt = 0;
        decimal SubTotalTaxableAmt = 0;
        decimal SubTotalCGSTAmt = 0;
        decimal SubTotalSGSTAmt = 0;
        decimal SubTotalIGSTAmt = 0;
        decimal SubTotalGSTAmt = 0;
        decimal SubNetTotal = 0;
        PrintHelper PHelper;
        private IGeneralContract generalBL;

        public PrintBL()
        {
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
        }

        public void PrintDebitOrCreditHeader(StreamWriter writer, DebitOrCreditBO debitOrCreditBO)
        {
            if (PrintHelper.CurrentPages == 1)
            {
                string str = string.Empty;
                int ReverseLines = Convert.ToInt32(generalBL.GetConfig("DebitOrCreditReverseLines"));
                string RLineFeed = string.Empty;
                for (int index = 1; index <= ReverseLines; ++index)
                    RLineFeed = RLineFeed + PHelper.ReverseLineFeed();
                writer.WriteLine(RLineFeed);
            }
            writer.WriteLine(PHelper.Condense(PHelper.Bold("GST : " + GeneralBO.GSTNo + PHelper.AlignText("CIN : " + GeneralBO.CINNo, PrintHelper.CharAlignment.AlignRight, 115))));
            writer.WriteLine(PHelper.Bold(PHelper.AlignText(GeneralBO.CompanyName.ToUpper(), PrintHelper.CharAlignment.AlignCenter, 80)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address1 + "," + GeneralBO.Address2, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address3 + "," + GeneralBO.Address4, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address5 + "," + GeneralBO.PIN + "," + GeneralBO.LandLine1, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText(debitOrCreditBO.PartyType + " " + debitOrCreditBO.DebitOrCreditType + " Note", PrintHelper.CharAlignment.AlignCenter, 137))));
            //writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("GST : " + GeneralBO.GSTNo, PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine();
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Transaction No.   : " + PHelper.Bold(debitOrCreditBO.TransNo), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Ref. Invoice No.   : " + PHelper.Bold(""), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Transaction Date  : " + PHelper.Bold(Convert.ToDateTime(debitOrCreditBO.Date).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Ref. Invoice Date  : " + PHelper.Bold(""), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(debitOrCreditBO.PartyType + " Name     : " + PHelper.Bold(debitOrCreditBO.PartyName), PrintHelper.CharAlignment.AlignLeft, 90)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(debitOrCreditBO.PartyType + " GSTN     : " + PHelper.Bold(debitOrCreditBO.GSTNo), PrintHelper.CharAlignment.AlignLeft, 90)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(debitOrCreditBO.PartyType + " Address  : " + PHelper.Bold(debitOrCreditBO.Addresses), PrintHelper.CharAlignment.AlignLeft, 90)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("State & Code      : " + PHelper.Bold(debitOrCreditBO.BillingState + " " + debitOrCreditBO.BillingStateID), PrintHelper.CharAlignment.AlignLeft, 90)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", PrintHelper.CurrentPages, " of ", PrintHelper.TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-3}{1,-35}{2,-10}{3,-6}{4,-8}{5,-9}{6,-11}{7,-11}{8,-9}{9,-8}{10,-8}{11,-8}{12,-11}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl.", "| Item Name", "| HSN Code", "| UOM", "| Qty", "| Rate", "| Discount", "| Taxable", "| GST Amt", "", "", "", "| Net Total");
            string HeadLine2 = string.Format(HeadingTemplate, "No.", "|", "|", "|", "|", "|", "|", "| Value", "| CGST", "| SGST", "| IGST", "|Total", "|");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.Condense(HeadLine2));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        public void PrintDebitOrCreditBody(StreamWriter writer, DebitOrCreditBO debitOrCreditBO)
        {
            int FooterLines = 3;
            foreach (var items in debitOrCreditBO.Items.Select((value, i) => new { i = (i + 1), value }))
            {
                string ItemLineTemplate = "{0,-4}{1,-34}{2,-10}{3,-6}{4,8}{5,9}{6,11}{7,11}{8,9}{9,8}{10,8}{11,8}{12,11}";
                if (PrintedLines == PrintHelper.BodyLines)
                {
                    PrintDebitOrCreditFooter(writer, debitOrCreditBO);
                    ++PrintHelper.CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintDebitOrCreditHeader(writer, debitOrCreditBO);
                    PrintedLines = 0;
                    SubTotalTaxableAmt = 0;
                    SubTotalCGSTAmt = 0;
                    SubTotalSGSTAmt = 0;
                    SubTotalIGSTAmt = 0;
                    SubTotalGSTAmt = 0;
                    SubNetTotal = 0;
                }
                SubTotalTaxableAmt += items.value.TaxableAmount;
                SubTotalCGSTAmt += items.value.CGSTAmt;
                SubTotalSGSTAmt += items.value.SGSTAmt;
                SubTotalIGSTAmt += items.value.IGSTAmt;
                SubTotalGSTAmt += (items.value.CGSTAmt + items.value.SGSTAmt + items.value.IGSTAmt);
                SubNetTotal += items.value.Amount;
                GSTAmt = items.value.CGSTAmt + items.value.SGSTAmt + items.value.IGSTAmt;
                TotalGSTAmt += (items.value.CGSTAmt + items.value.SGSTAmt + items.value.IGSTAmt);
                string Content = string.Format(ItemLineTemplate,
                    (items.i).ToString(),
                    items.value.Item,
                    items.value.HSNCode,
                    items.value.Unit,
                    string.Format("{0:0}", items.value.Qty),
                    string.Format("{0:0.00}", items.value.Rate),
                    string.Format("{0:0.00}", items.value.DiscountAmount),
                    string.Format("{0:0.00}", items.value.TaxableAmount),
                    string.Format("{0:0.00}", items.value.CGSTAmt),
                    string.Format("{0:0.00}", items.value.SGSTAmt),
                    string.Format("{0:0.00}", items.value.IGSTAmt),
                    string.Format("{0:0.00}", GSTAmt),
                    string.Format("{0:0.00}", items.value.Amount));
                writer.WriteLine(PHelper.Condense(Content));
                ++PrintedLines;
            }
        }

        public void PrintDebitOrCreditFooter(StreamWriter writer, DebitOrCreditBO debitOrCreditBO)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;
            if (PrintHelper.CurrentPages == PrintHelper.TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("DebitOrCreditForwardLines"));
                int BlankLines = PrintHelper.BodyLines - PrintedLines;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                string FooterLineTemplate = "{0,-12}{1,-26}{2,-10}{3,-6}{4,8}{5,9}{6,11}{7,11}{8,9}{9,8}{10,8}{11,8}{12,11}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Total :",
                "",
                "",
                "",
                "",
                "",
                "",
                string.Format("{0:0.00}", debitOrCreditBO.TaxableAmount),
                string.Format("{0:0.00}", debitOrCreditBO.CGSTAmt),
                string.Format("{0:0.00}", debitOrCreditBO.SGSTAmt),
                string.Format("{0:0.00}", debitOrCreditBO.IGSTAmt),
                string.Format("{0:0.00}", TotalGSTAmt),
                string.Format("{0:0.00}", debitOrCreditBO.TotalAmount));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterContent)));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.Bold("Total Transaction Value in Words : " + generalBL.NumberToText(Convert.ToInt32(debitOrCreditBO.TotalAmount)) + "Only")));
                writer.WriteLine();
                string Footer = "{0,-12}{1,30}{2,-20}{3,10}{4,10}{5,55}";
                string ContentLine1 = string.Format(Footer,
                "Bank Details",
                debitOrCreditBO.BankName,
                "Total Taxable Value",
                "",
                string.Format("{0:0.00}", debitOrCreditBO.TaxableAmount),
                "For Vaidyaratnam Oushadhashala Pvt Ltd :");
                string ContentLine2 = string.Format(Footer,
                "IFSC",
                debitOrCreditBO.IFSCNo,
                "CGST",
                string.Format("{0:0.00}", debitOrCreditBO.CGSTAmt),
                "",
                "");
                string ContentLine3 = string.Format(Footer,
                "Account No.",
                debitOrCreditBO.BankACNo,
                "SGST",
                string.Format("{0:0.00}", debitOrCreditBO.SGSTAmt),
                "",
                "");
                string ContentLine4 = string.Format(Footer,
                "",
                "",
                "IGST",
                string.Format("{0:0.00}", debitOrCreditBO.IGSTAmt),
                "",
                "");
                string ContentLine5 = string.Format(Footer,
                "",
                "",
                "Total GST",
                "",
                string.Format("{0:0.00}", TotalGSTAmt),
                "");
                string ContentLine6 = string.Format(Footer,
                "",
                "",
                "Net Transaction Value",
                "",
                string.Format("{0:0.00}", debitOrCreditBO.TotalAmount),
                "");
                writer.WriteLine(PHelper.Condense(ContentLine1));
                writer.WriteLine(PHelper.Condense(ContentLine2));
                writer.WriteLine(PHelper.Condense(ContentLine3));
                writer.WriteLine(PHelper.Condense(ContentLine4));
                writer.WriteLine(PHelper.Condense(ContentLine5));
                writer.WriteLine(PHelper.Condense(ContentLine6));
                writer.WriteLine(PHelper.Condense("Terms & Conditions"));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense("Seal"));
                writer.WriteLine(PHelper.FormFeed());
            }
            else
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("DebitOrCreditContinueForwardLines"));
                string FooterLineTemplate = "{0,-12}{1,-26}{2,-10}{3,-6}{4,8}{5,9}{6,11}{7,11}{8,9}{9,8}{10,8}{11,8}{12,11}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Sub Total :",
                "",
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

    }
}
