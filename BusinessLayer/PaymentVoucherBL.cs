//File created by prama on 28-03-2018


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System.IO;

namespace BusinessLayer
{
    public class PaymentVoucherBL : IPaymentVoucher
    {

        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        decimal TotalAmt = 0;
        decimal SubTotalAmt = 0;
        PaymentVoucherDAL paymentVoucherDAL;
        PaymentVoucherBO paymentVoucher;
        PrintHelper PHelper;
        private IGeneralContract generalBL;

        public PaymentVoucherBL()
        {
            paymentVoucherDAL = new PaymentVoucherDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
        }
        public List<PaymentVoucherBO> GetPaymentVoucher(int ID)
        {
            return paymentVoucherDAL.GetPaymentVoucher(ID);
        }


        public List<PaymentVoucherBO> GetPaymentVoucherDetail(int ID)
        {
            return paymentVoucherDAL.GetPaymentVoucherDetail(ID);
        }

        public List<PaymentVoucherItemBO> GetPaymentVoucherTrans(int ID)
        {
            return paymentVoucherDAL.GetPaymentVoucherTrans(ID);
        }

        public List<PaymentVoucherItemBO> GetPaymentVoucherTransForEdit(int ID)
        {
            return paymentVoucherDAL.GetPaymentVoucherTransForEdit(ID);
        }
        public List<PayableDetailsBO> GetDocumentAutoComplete(string term)
        {
            return paymentVoucherDAL.GetDocumentAutoComplete(term);
        }
        public DatatableResultBO GetPaymentVoucherList(string Type, string VoucherNumber, string VoucherDate, string SupplierName, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return paymentVoucherDAL.GetPaymentVoucherList( Type,VoucherNumber,VoucherDate, SupplierName, Amount, SortField, SortOrder,  Offset, Limit);
        }
        public string GetPrintTextFile(int PaymentVoucherID)
        {
            BodyLines = Convert.ToInt32(8);
            string FileName;
            string FilePath;
            string URL;
            paymentVoucher = new PaymentVoucherBO();
            paymentVoucher = paymentVoucherDAL.GetPaymentVoucherDetail(PaymentVoucherID).FirstOrDefault();
            paymentVoucher.List = GetPaymentVoucherTrans(PaymentVoucherID);
            string TransNo = paymentVoucher.VoucherNo;
            FileName = TransNo + ".txt";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FilePath = path + "/Outputs/PaymentVoucher/" + FileName;
            TotalLines = paymentVoucher.List.Count();
            SetPageVariables();
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintPaymentVoucherHeader(writer);
                PrintPaymentVoucherBody(writer);
                PrintPaymentVoucherFooter(writer);
            }
            URL = "/Outputs/PaymentVoucher/" + FileName;
            return URL;
        }

        private void PrintPaymentVoucherHeader(StreamWriter writer)
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
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("Payment Voucher", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Supplier Name    : " + PHelper.Bold(paymentVoucher.SupplierName), PrintHelper.CharAlignment.AlignLeft, 90)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Transaction No.  : " + PHelper.Bold(paymentVoucher.VoucherNo), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("From Bank      : " + PHelper.Bold(paymentVoucher.BankName), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Transaction Date : " + PHelper.Bold(Convert.ToDateTime(paymentVoucher.VoucherDate).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Payment Type   : " + PHelper.Bold(paymentVoucher.PaymentTypeName), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Remarks          : " + PHelper.Bold(paymentVoucher.Remark), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Instrument No. : " + PHelper.Bold(paymentVoucher.ReferenceNumber), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-12}{1,-80}{2,-45}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl. No.", "| Document No", "| Amount");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        private void PrintPaymentVoucherBody(StreamWriter writer)
        {
            int FooterLines = 3;
            foreach (var items in paymentVoucher.List.Select((value, i) => new { i = (i + 1), value }))
            {
                string ItemLineTemplate = "{0,-12}{1,-80}{2,45}";
                if (PrintedLines == BodyLines)
                {
                    PrintPaymentVoucherFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintPaymentVoucherHeader(writer);
                    PrintedLines = 0;
                    SubTotalAmt = 0;
                }
                TotalAmt += items.value.PaidAmount;
                SubTotalAmt += items.value.PaidAmount;
                string Content = string.Format(ItemLineTemplate,
                    (items.i).ToString(),
                    items.value.InvoiceNo,
                    string.Format("{0:0.00}", items.value.PaidAmount));
                writer.WriteLine(PHelper.Condense(Content));
                ++PrintedLines;
            }
        }

        private void PrintPaymentVoucherFooter(StreamWriter writer)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;
            if (CurrentPages == TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("AdvancePaymentForwardLines"));
                int BlankLines = BodyLines - PrintedLines;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                string FooterLineTemplate = "{0,-12}{1,-80}{2,45}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Total :",
                "",
                string.Format("{0:0.00}", TotalAmt));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterContent)));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Recipient Signature", PrintHelper.CharAlignment.AlignRight, 137)));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.Bold("Paid Rupees" + generalBL.NumberToText(Convert.ToInt32(TotalAmt)) + "Only To")));
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
                string FooterLineTemplate = "{0,-12}{1,-80}{2,45}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Sub Total",
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

        public List<PaymentVoucherBO> GetPaymentVoucherDetailV3(int ID)
        {
            return paymentVoucherDAL.GetPaymentVoucherDetailV3(ID);
        }

        public List<PaymentVoucherItemBO> GetPaymentVoucherTransForEditV3(int ID)
        {
            return paymentVoucherDAL.GetPaymentVoucherTransForEditV3(ID);
        }
        public DatatableResultBO GetPaymentVoucherListV3(string Type, string VoucherNumber, string VoucherDate, string AccountHead, string Amount, string ReconciledDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            return paymentVoucherDAL.GetPaymentVoucherListV3(Type, VoucherNumber, VoucherDate, AccountHead, Amount, ReconciledDate, SortField, SortOrder, Offset, Limit);
        }

        public List<PaymentVoucherItemBO> GetPaymentVoucherTransV3(int ID)
        {
            return paymentVoucherDAL.GetPaymentVoucherTransV3(ID);
        }
        public int SaveReconciledDate(int ID, DateTime ReconciledDate, string BankReferanceNumber, string Remarks)
        {
            return paymentVoucherDAL.SaveReconciledDate(ID, ReconciledDate, BankReferanceNumber, Remarks);
        }
    }
}
