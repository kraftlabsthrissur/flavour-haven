using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BusinessLayer
{
    public class ReceiptVoucherBL : IReceiptVoucher
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        decimal SubTotalAmt = 0;
        ReceiptVoucherDAL receiptVoucherDAL;
        ReceiptVoucherBO receiptVoucher;
        PrintHelper PHelper;
        private IGeneralContract generalBL;
        public ReceiptVoucherBL()
        {
            receiptVoucherDAL = new ReceiptVoucherDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
        }

        //code below by prama on 20-4-18
        public bool Save(ReceiptVoucherBO receiptVoucherBO, List<ReceiptItemBO> receiptItemBO, List<ReceiptSettlementBO> Settlements)
        {
            string StringSettlements = XMLHelper.Serialize(Settlements);
            if (receiptVoucherBO.ID > 0)
            {
                return receiptVoucherDAL.Update(receiptVoucherBO, receiptItemBO, StringSettlements);
            }
            else
            {
                return receiptVoucherDAL.Save(receiptVoucherBO, receiptItemBO, StringSettlements);
            }
        }

        public List<SalesInvoiceBO> GetInvoiceForReceiptVoucher(int CustomerID)
        {
            return receiptVoucherDAL.GetInvoiceForReceiptVoucher(CustomerID);
        }

        public List<ReceiptVoucherBO> GetReceiptList()
        {
            return receiptVoucherDAL.GetReceiptList();
        }

        public ReceiptVoucherBO GetReceiptDetails(int ReceiptID)
        {
            return receiptVoucherDAL.GetReceiptDetails(ReceiptID);
        }

        public List<ReceiptItemBO> GetReceiptTrans(int ReceiptID)
        {
            return receiptVoucherDAL.GetReceiptTrans(ReceiptID);
        }

        public List<ReceiptItemBO> GetReceiptTransForEdit(int ReceiptID)
        {
            return receiptVoucherDAL.GetReceiptTransForEdit(ReceiptID);
        }

        public DatatableResultBO GetReceiptVoucherList(string Type, string ReceiptNoHint, string InvoiceDateHint, string CustomerHint, string ReceiptAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return receiptVoucherDAL.GetReceiptVoucherList(Type, ReceiptNoHint, InvoiceDateHint, CustomerHint, ReceiptAmountHint, SortField, SortOrder, Offset, Limit);
        }

        public List<ReceiptItemBO> GetAdvanceReceiptTrans(int CustomerID)
        {
            return receiptVoucherDAL.GetAdvanceReceiptTrans(CustomerID);
        }

        public string GetPrintTextFile(int ReceipttVoucherID)
        {
            BodyLines = Convert.ToInt32(8);
            string FileName;
            string FilePath;
            string URL;
            receiptVoucher = new ReceiptVoucherBO();
            receiptVoucher = receiptVoucherDAL.GetReceiptDetails(ReceipttVoucherID);
            receiptVoucher.Item = GetReceiptTrans(ReceipttVoucherID);
            string TransNo = receiptVoucher.ReceiptNo;
            FileName = TransNo + ".txt";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FilePath = path + "/Outputs/ReceiptVoucher/" + FileName;
            TotalLines = receiptVoucher.Item.Count();
            SetPageVariables();
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintReceiptVoucherHeader(writer);
                PrintReceiptVoucherBody(writer);
                PrintReceiptVoucherFooter(writer);
            }
            URL = "/Outputs/ReceiptVoucher/" + FileName;
            return URL;
        }

        private void PrintReceiptVoucherHeader(StreamWriter writer)
        {
            if (CurrentPages == 1)
            {
                int ReverseLines = Convert.ToInt32(generalBL.GetConfig("AdvancePaymentReverseLines"));
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
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("Receipt Voucher", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Customer Name  : " + PHelper.Bold(receiptVoucher.CustomerName), PrintHelper.CharAlignment.AlignLeft, 90)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Voucher No.    : " + PHelper.Bold(receiptVoucher.ReceiptNo), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("To Bank      : " + PHelper.Bold(receiptVoucher.BankName), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Voucher Date   : " + PHelper.Bold(Convert.ToDateTime(receiptVoucher.ReceiptDate).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Receipt Type   : " + PHelper.Bold(receiptVoucher.PaymentTypeName), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Remarks        : " + PHelper.Bold(receiptVoucher.Remarks), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Instrument No. : " + PHelper.Bold(receiptVoucher.BankReferanceNumber), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-12}{1,-15}{2,-65}{3,-45}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl. No.", "| Document Date", "| Document No", "| Amount");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        private void PrintReceiptVoucherBody(StreamWriter writer)
        {
            int FooterLines = 3;
            foreach (var items in receiptVoucher.Item.Select((value, i) => new { i = (i + 1), value }))
            {
                string ItemLineTemplate = "{0,-12}{1,-15}{2,-65}{3,45}";
                if (PrintedLines == BodyLines)
                {
                    PrintReceiptVoucherFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintReceiptVoucherHeader(writer);
                    PrintedLines = 0;
                    SubTotalAmt = 0;
                }
                SubTotalAmt += items.value.Amount;
                string Content = string.Format(ItemLineTemplate,
                    (items.i).ToString(),
                    items.value.ReceivableDate.ToString("dd-MM-yyyy"),
                    items.value.DocumentNo,
                    string.Format("{0:0.00}", items.value.AmountToBeMatched));
                writer.WriteLine(PHelper.Condense(Content));
                ++PrintedLines;
            }
        }

        private void PrintReceiptVoucherFooter(StreamWriter writer)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;
            char ch10 = Convert.ToChar(10);
            string str10 = ch10.ToString();
            string ForwardLineFeed = str10;
            string str = string.Empty;
            char ch20 = Convert.ToChar(27);
            string str45 = ch20.ToString();
            ch20 = Convert.ToChar(50);
            string str46 = ch20.ToString();
            string LineSpacing1_6 = str45 + str46;
            if (CurrentPages == TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("AdvancePaymentForwardLines"));
                int BlankLines = BodyLines - PrintedLines;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                string FooterLineTemplate = "{0,-12}{1,-15}{2,-65}{3,45}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Total :",
                "",
                "",
                string.Format("{0:0.00}", receiptVoucher.ReceiptAmount));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterContent)));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Payee Signature", PrintHelper.CharAlignment.AlignRight, 137)));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.Bold("Received Rupees" + generalBL.NumberToText(Convert.ToInt32(receiptVoucher.ReceiptAmount)) + "Only From")));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Prepared By", PrintHelper.CharAlignment.AlignLeft, 33)
                    + PHelper.AlignText("Recommended By", PrintHelper.CharAlignment.AlignLeft, 33)
                    + PHelper.AlignText("Approved By", PrintHelper.CharAlignment.AlignLeft, 33)
                    + PHelper.AlignText("Director", PrintHelper.CharAlignment.AlignLeft, 33)));
                for (int index = 1; index <= ForwardLines; ++index)
                    FLineFeed = FLineFeed + PHelper.ForwardLineFeed();
                writer.WriteLine(FLineFeed);
            }
            else
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("AdvancePaymentContinueForwardLines"));
                string FooterLineTemplate = "{0,-12}{1,-15}{2,-65}{3,45}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Sub Total",
                "",
                "",
                string.Format("{0:0.00}", SubTotalAmt));
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

        //changes for version3

        public bool SaveV3(ReceiptVoucherBO receiptVoucherBO, List<ReceiptItemBO> receiptItemBO, List<ReceiptSettlementBO> Settlements)
        {
            string StringSettlements = XMLHelper.Serialize(Settlements);
            if (receiptVoucherBO.ID > 0)
            {
                return receiptVoucherDAL.UpdateV3(receiptVoucherBO, receiptItemBO, StringSettlements);
            }
            else
            {
                return receiptVoucherDAL.SaveV3(receiptVoucherBO, receiptItemBO, StringSettlements);
            }
        }

        public DatatableResultBO GetReceiptVoucherListV3(string Type, string ReceiptNoHint, string InvoiceDateHint, string AccountHeadHint, string ReceiptAmountHint,string ReconciledDateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return receiptVoucherDAL.GetReceiptVoucherListV3(Type, ReceiptNoHint, InvoiceDateHint, AccountHeadHint, ReceiptAmountHint, ReconciledDateHint, SortField, SortOrder, Offset, Limit);
        }
        public ReceiptVoucherBO GetReceiptDetailsV3(int ReceiptID)
        {
            return receiptVoucherDAL.GetReceiptDetailsV3(ReceiptID);
        }

        public List<ReceiptItemBO> GetReceiptTransV3(int ReceiptID)
        {
            return receiptVoucherDAL.GetReceiptTransV3(ReceiptID);
        }
        public List<ReceiptItemBO> GetReceiptTransForEditV3(int ReceiptID)
        {
            return receiptVoucherDAL.GetReceiptTransForEditV3(ReceiptID);
        }
        public  int SaveReconciledDate(int ID, DateTime ReconciledDate, string BankReferanceNumber, string Remarks)
        {
            return receiptVoucherDAL.SaveReconciledDate(ID, ReconciledDate,BankReferanceNumber,Remarks);
        }
    }
}
