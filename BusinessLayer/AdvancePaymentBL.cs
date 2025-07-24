using System.Collections.Generic;
using PresentationContractLayer;
using BusinessObject;
using DataAccessLayer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;

namespace BusinessLayer
{
    public class AdvancePaymentBL : IAdvancePayment
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        decimal TotalTDSAmt = 0;
        decimal SubTotalAmt = 0;
        decimal SubTotalTDSAmt = 0;
        decimal SubTotalNetAmt = 0;
        AdvancePaymentDAL apDAL;
        AdvancePaymentBO advancePayment;
        PrintHelper PHelper;
        private IGeneralContract generalBL;
        List<AdvancePaymentPurchaseOrderBO> advancePaymentTrans;
        #region Constructor
        public AdvancePaymentBL()
        {
            apDAL = new AdvancePaymentDAL();
            PHelper = new PrintHelper();
            generalBL = new GeneralBL();
        }
        #endregion
        //code below by prama on 5-6-18
        public List<AdvancePaymentBO> GetAdvancePaymentList(int ID)
        {
            return apDAL.GetAdvancePaymentList(ID);
        }
        //
        public List<AdvancePaymentBO> GetAdvancePaymentDetails(int AdvanePaymentID)
        {
            return apDAL.GetAdvancePaymentDetails(AdvanePaymentID);
        }
        public List<AdvancePaymentBO> GetUnProcessedAdvancePaymentListSupplierWise(int SupplierID, int EmployeeID)
        {
            return apDAL.GetUnProcessedAdvancePaymentListSupplierWise(SupplierID, EmployeeID);
        }
        public List<AdvancePaymentTransBO> GetUnProcessedAdvancePaymentTransList(int PaymentID)
        {
            return apDAL.GetUnProcessedAdvancePaymentTransList(PaymentID);
        }
        public List<AdvancePaymentPurchaseOrderBO> GetAdvancePaymentTransDetails(int AdvancePaymentID)
        {
            return apDAL.GetAdvancePaymentTransDetails(AdvancePaymentID);
        }

        public List<AdvanceRequestTransBO> GetAdvanceRequest(int EmployeeID, int IsOfficial)
        {
            return apDAL.GetAdvanceRequest(EmployeeID, IsOfficial);
        }
        public List<AdvanceRequestTransBO> GetAdvanceRequestForEdit(int ID)
        {
            return apDAL.GetAdvanceRequestForEdit(ID);
        }
        public List<AdvancePaymentPurchaseOrderBO> GetPurchaseOrdersAdvancePaymentForEdit(int ID)
        {
            return apDAL.GetPurchaseOrdersAdvancePaymentForEdit(ID);
        }
        public List<AdvancePaymentPurchaseOrderBO> GetPurchaseOrders(int SupplierID)
        {
            return apDAL.GetPurchaseOrders(SupplierID);
        }

        public DatatableResultBO GetAdvancePaymentListForDataTable(string Type, string AdvancePaymentNo, string AdvancePaymentDate, string Category, string Name, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return apDAL.GetAdvancePaymentListForDataTable(Type, AdvancePaymentNo, AdvancePaymentDate, Category, Name, Amount, SortField, SortOrder, Offset, Limit);
        }
        public string GetPrintTextFile(int AdvancePaymentID)
        {
            BodyLines = Convert.ToInt32(8);
            string FileName;
            string FilePath;
            string URL;
            advancePayment = new AdvancePaymentBO();
            advancePayment = apDAL.GetAdvancePaymentDetails(AdvancePaymentID).FirstOrDefault();
            advancePaymentTrans = GetAdvancePaymentTransDetails(AdvancePaymentID);
            string TransNo = advancePayment.AdvancePaymentNo;
            FileName = TransNo + ".txt";

            string path = AppDomain.CurrentDomain.BaseDirectory;

            FilePath = path + "/Outputs/AdvancePayment/" + FileName;
            TotalLines = advancePaymentTrans.Count();
            SetPageVariables();
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintAdvancePaymentHeader(writer);
                PrintAdvancePaymentBody(writer);
                PrintAdvancePaymentFooter(writer);
            }
            URL = "/Outputs/AdvancePayment/" + FileName;
            return URL;
        }

        private void PrintAdvancePaymentHeader(StreamWriter writer)
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
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("Advance Payment Voucher", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine();
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Voucher No.            : " + PHelper.Bold(advancePayment.AdvancePaymentNo), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("From Bank      : " + PHelper.Bold(advancePayment.BankDetail), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Voucher Date           : " + PHelper.Bold(Convert.ToDateTime(advancePayment.AdvancePaymentDate).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Payment Type   : " + PHelper.Bold(advancePayment.ModeOfPaymentName), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Category               : " + PHelper.Bold(advancePayment.Category), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Instrument No. : " + PHelper.Bold(advancePayment.ReferenceNo), PrintHelper.CharAlignment.AlignLeft, 47)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Supplier/Employee Name : " + PHelper.Bold(advancePayment.SelectedName), PrintHelper.CharAlignment.AlignLeft, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-8}{1,-18}{2,-37}{3,-15}{4,-15}{5,-15}{6,-29}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl. No.", "| Document Date", "| Document No.", "| Gross Amount", "| TDS Amount", "| Net Amount", "| Remarks");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        private void PrintAdvancePaymentBody(StreamWriter writer)
        {
            int FooterLines = 3;
            foreach (var items in advancePaymentTrans.Select((value, i) => new { i = (i + 1), value }))
            {
                string ItemLineTemplate = "{0,-9}{1,-17}{2,-37}{3,15}{4,15}{5,15}{6,-29}";
                if (PrintedLines == BodyLines)
                {
                    PrintAdvancePaymentFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintAdvancePaymentHeader(writer);
                    PrintedLines = 0;
                    SubTotalAmt = 0;
                    SubTotalTDSAmt = 0;
                    SubTotalNetAmt = 0;
                }
                TotalTDSAmt += items.value.TDSAmount;
                SubTotalAmt += items.value.Amount;
                SubTotalTDSAmt += items.value.TDSAmount;
                SubTotalNetAmt += (items.value.Amount - items.value.TDSAmount);
                string Content = string.Format(ItemLineTemplate,
                    (items.i).ToString(),
                    Convert.ToDateTime(items.value.PurchaseOrderDate).ToString("dd MMM yyyy"),
                    items.value.TransNo,
                    string.Format("{0:0.00}", items.value.Amount),
                    string.Format("{0:0.00}", items.value.TDSAmount),
                    string.Format("{0:0.00}", items.value.Amount - items.value.TDSAmount),
                    items.value.Remarks);
                writer.WriteLine(PHelper.Condense(Content));
                ++PrintedLines;
            }
        }

        private void PrintAdvancePaymentFooter(StreamWriter writer)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;
            if (CurrentPages == TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("AdvancePaymentForwardLines"));
                int BlankLines = BodyLines - PrintedLines;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                string FooterLineTemplate = "{0,-9}{1,-17}{2,-37}{3,15}{4,15}{5,15}{6,-29}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Total :",
                "",
                "",
                string.Format("{0:0.00}", advancePayment.Amt),
                string.Format("{0:0.00}", TotalTDSAmt),
                string.Format("{0:0.00}", advancePayment.NetAmount),
                "");
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterContent)));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Recepient Signature", PrintHelper.CharAlignment.AlignRight, 137)));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.Bold("Paid Rupees" + generalBL.NumberToText(Convert.ToInt32(advancePayment.NetAmount)) + "Only To")));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Prepared By", PrintHelper.CharAlignment.AlignLeft, 35)
                    + PHelper.AlignText("Recommended By", PrintHelper.CharAlignment.AlignLeft, 34)
                    + PHelper.AlignText("Approved By", PrintHelper.CharAlignment.AlignLeft, 34)
                    + PHelper.AlignText("Director", PrintHelper.CharAlignment.AlignLeft, 34)));
                for (int index = 1; index <= ForwardLines; ++index)
                    FLineFeed = FLineFeed + PHelper.ForwardLineFeed();
                writer.WriteLine(FLineFeed);
            }
            else
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("AdvancePaymentContinueForwardLines"));
                string FooterLineTemplate = "{0,-9}{1,-17}{2,-37}{3,15}{4,15}{5,15}{6,-29}";
                string FooterContent = string.Format(FooterLineTemplate,
                "Sub Total",
                "",
                "",
                string.Format("{0:0.00}", SubTotalAmt),
                string.Format("{0:0.00}", SubTotalTDSAmt),
                string.Format("{0:0.00}", SubTotalNetAmt),
                "");
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
