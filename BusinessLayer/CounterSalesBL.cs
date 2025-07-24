using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessObject;
using DataAccessLayer;
using System.IO;
using DataAccessLayer.DBContext;

namespace BusinessLayer
{
    public class CounterSalesBL : ICounterSalesContract
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        private int ItemLengthLimit;

        decimal TotalGrossAmt = 0;
        decimal SubTotalGrossAmt = 0;
        decimal SubTotalSGSTAmt = 0;
        decimal SubTotalCGSTAmt = 0;
        decimal SubTotalCessAmt = 0;
        decimal SubTotalNetAmt = 0;
        CounterSalesDAL counterSalesDal;
        CounterSalesBO counterSales;
        private IGeneralContract generalBL;
        PrintHelper PHelper;

        public CounterSalesBL()
        {
            counterSalesDal = new CounterSalesDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
        }

        public List<CounterSalesBO> GetCounterSalesList(int ID)
        {
            return counterSalesDal.GetCounterSalesList(ID);
        }

        public CounterSalesBO GetIsCounterSalesAlreadyExists(string PartyName, decimal NetAmount)
        {
            return counterSalesDal.GetIsCounterSalesAlreadyExists(PartyName, NetAmount);
        }

        public List<CounterSalesBO> GetCounterSalesDetail(int ID)
        {
            return counterSalesDal.GetCounterSalesDetail(ID);
        }

        public List<CounterSalesItemsBO> GetCounterSalesListDetails(int ID)
        {
            return counterSalesDal.GetCounterSalesListDetails(ID);
        }

        public List<CounterSalesAmountDetailsBO> GetCounterSalesListAmount(int ID)
        {
            return counterSalesDal.GetCounterSalesListAmount(ID);
        }

        public int SaveCounterSalesInvoice(CounterSalesBO counterSalesBO, List<CounterSalesItemsBO> Items, List<CounterSalesAmountDetailsBO> AmountDetails)
        {
            string XMLItems = "<counterSalesTrans>";
            string XMLAmountDetails;
            foreach (var item in Items)
            {
                XMLItems += "<Items>";
                XMLItems += "<FullOrLoose>" + item.FullOrLoose + "</FullOrLoose>";
                XMLItems += "<ItemID>" + item.ItemID + "</ItemID>";
                XMLItems += "<BatchID>" + item.BatchID + "</BatchID>";
                XMLItems += "<Quantity>" + item.Quantity + "</Quantity>";
                XMLItems += "<Rate>" + item.Rate + "</Rate>";
                XMLItems += "<MRP>" + item.MRP + "</MRP>";
                XMLItems += "<SecondaryRate>" + item.SecondaryRate + "</SecondaryRate>";
                XMLItems += "<SecondaryQty>" + item.SecondaryQty + "</SecondaryQty>";
                XMLItems += "<SecondaryOfferQty>" + item.SecondaryOfferQty + "</SecondaryOfferQty>";
                XMLItems += "<SecondaryUnit>" + item.SecondaryUnit + "</SecondaryUnit>";
                XMLItems += "<SecondaryUnitSize>" + item.SecondaryUnitSize + "</SecondaryUnitSize>";
                XMLItems += "<BasicPrice>" + item.BasicPrice + "</BasicPrice>";
                XMLItems += "<GrossAmount>" + item.GrossAmount + "</GrossAmount>";
                XMLItems += "<DiscountPercentage>" + item.DiscountPercentage + "</DiscountPercentage>";
                XMLItems += "<DiscountAmount>" + item.DiscountAmount + "</DiscountAmount>";
                XMLItems += "<TaxableAmount>" + item.TaxableAmount + "</TaxableAmount>";
                XMLItems += "<SGSTPercentage>" + item.SGSTPercentage + "</SGSTPercentage>";
                XMLItems += "<CGSTPercentage>" + item.CGSTPercentage + "</CGSTPercentage>";
                XMLItems += "<IGSTPercentage>" + item.IGSTPercentage + "</IGSTPercentage>";
                XMLItems += "<SGSTAmount>" + item.SGSTAmount + "</SGSTAmount>";
                XMLItems += "<CGSTAmount>" + item.CGSTAmount + "</CGSTAmount>";
                XMLItems += "<IGSTAmount>" + item.IGSTAmount + "</IGSTAmount>";
                XMLItems += "<IsVAT>" + item.IsVAT + "</IsVAT>";
                XMLItems += "<IsGST>" + item.IsGST + "</IsGST>";
                XMLItems += "<CurrencyID>" + item.CurrencyID + "</CurrencyID>";
                XMLItems += "<VATPercentage>" + item.VATPercentage + "</VATPercentage>";
                XMLItems += "<ItemName>" + item.ItemName + "</ItemName>";
                XMLItems += "<PartsNumber>" + item.PartsNumber + "</PartsNumber>";
                XMLItems += "<DeliveryTerm>" + item.DeliveryTerm + "</DeliveryTerm>";
                XMLItems += "<Model>" + item.Model + "</Model>";
                XMLItems += "<VATAmount>" + item.VATAmount + "</VATAmount>";
                XMLItems += "<NetAmount>" + item.NetAmount + "</NetAmount>";
                XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<WareHouseID>" + item.WareHouseID + "</WareHouseID>";
                XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLItems += "<CessAmount>" + item.CessAmount + "</CessAmount>";
                XMLItems += "<CessPercentage>" + item.CessPercentage + "</CessPercentage>";
                XMLItems += "</Items>";
            }
            XMLItems += "</counterSalesTrans>";
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
            if (counterSalesBO.ID > 0)
            {
                return counterSalesDal.UpdateCounterSalesInvoice(counterSalesBO, XMLItems, XMLAmountDetails);
            }
            else
            {
                return counterSalesDal.SaveCounterSalesInvoice(counterSalesBO, XMLItems, XMLAmountDetails);
            }
        }



        public int Cancel(int CounterSalesID)
        {
            return counterSalesDal.Cancel(CounterSalesID);

        }

        public DatatableResultBO GetCounterSalesForReturn(int PartyID, string TransHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return counterSalesDal.GetCounterSalesForReturn(PartyID, TransHint, SortField, SortOrder, Offset, Limit);
        }

        public List<CounterSalesItemsBO> GetCounterSalesTransForCounterSalesReturn(int InvoiceID, int PriceListID)
        {
            return counterSalesDal.GetCounterSalesTransForCounterSalesReturn(InvoiceID, PriceListID);
        }

        public List<CounterSalesItemsBO> GetBatchwiseItemForCounterSales(int ItemID, int WarehouseID, int BatchTypeID, int UnitID, decimal Qty, string CustomerType, int TaxTypeID)
        {
            return counterSalesDal.GetBatchwiseItemForCounterSales(ItemID, WarehouseID, BatchTypeID, UnitID, Qty, CustomerType, TaxTypeID);
        }
        public List<CounterSalesItemsBO> GetGoodsReciptItemForCounterSales(int[] CounterSalesIDs)
        {
            string CommaSeparatedCounterSalesIDs = string.Join(",", CounterSalesIDs.Select(x => x.ToString()).ToArray());
            return counterSalesDal.GetGoodsReceiptItemForCounterSales(CommaSeparatedCounterSalesIDs);
        }

        public List<CounterSalesBO> GetCounterSalesType()
        {
            return counterSalesDal.GetCounterSalesType();
        }

        public string GetPrintTextFile(int counterSalesID)
        {
            BodyLines = Convert.ToInt32(10);
            PrintedLines = 0;
            ItemLengthLimit = 40;
            string FileName;
            string FilePath;
            string URL;

            counterSales = new CounterSalesBO();
            counterSales = counterSalesDal.GetCounterSalesDetail(counterSalesID).FirstOrDefault();
            counterSales.Items = GetCounterSalesListDetails(counterSalesID);
            string TransNo = counterSales.TransNo;
            FileName = TransNo + ".txt";

            string path = AppDomain.CurrentDomain.BaseDirectory;

            FilePath = path + "/Outputs/CounterSales/" + FileName;
            TotalLines = getNoOfPrintLines(counterSales.Items);
            SetPageVariables();
            PrintedLines = 0;
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintCounterSalesInvoiceHeader(writer);
                PrintCounterSalesInvoiceBody(writer);
                PrintCounterSalesInvoiceFooter(writer);
            }
            URL = "/Outputs/CounterSales/" + FileName;
            return URL;
        }

        public int getNoOfPrintLines(List<CounterSalesItemsBO> counterSalesItemBOList)
        {
            int count = 0;
            foreach (CounterSalesItemsBO item in counterSalesItemBOList)
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

        private void PrintCounterSalesInvoiceHeader(StreamWriter writer)
        {
            if (CurrentPages == 1)
            {
                int ReverseLines = Convert.ToInt32(generalBL.GetConfig("CounterSalesReverseLines"));
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
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("RETAIL INVOICE - CASH", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Invoice No.  : " + PHelper.Bold(counterSales.TransNo), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Invoice Date : " + PHelper.Bold(Convert.ToDateTime(counterSales.TransDate).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 47)));
            if (counterSales.Type == "Employee")
            {
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Employee Code: " + PHelper.Bold(counterSales.EmployeeCode), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Employee Name: " + PHelper.Bold(counterSales.EmployeeName), PrintHelper.CharAlignment.AlignLeft, 47)));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Doctor       : " + PHelper.Bold(counterSales.DoctorName), PrintHelper.CharAlignment.AlignLeft, 47)));
            }
            else
            {
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Patient      : " + PHelper.Bold(counterSales.PartyName), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Doctor       : " + PHelper.Bold(counterSales.DoctorName), PrintHelper.CharAlignment.AlignLeft, 47)));
            }
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-3}{1,-44}{2,-7}{3,-8}{4,-6}{5,-8}{6,-7}{7,-9}{8,-6}{9,-7}{10,-7}{11,-7}{12,-10}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl.", "| Commodity / Item", "| UOM", "| Batch", "| Exp.", "| Unit",
                "| Qty", "| Gross", "| GST", "| SGST", "| CGST", "| Cess", "| Total");
            string HeadLine2 = string.Format(HeadingTemplate, "No.", "|", "|", "|", "| Date", "| Price", "|", "| Value", "| %", "| Amt", "| Amt", "| Amt", "| Amt");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.Condense(HeadLine2));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        private void PrintCounterSalesInvoiceBody(StreamWriter writer)
        {
            int FooterLines = 3;
            foreach (var items in counterSales.Items.Select((value, i) => new { i = (i + 1), value }))
            {
                string ItemLineTemplate = "{0,-4}{1,-43}{2,-7}{3,-8}{4,-6}{5,8}{6,7}{7,9}{8,6}{9,7}{10,7}{11,7}{12,9}";
                if (PrintedLines == BodyLines)
                {
                    PrintCounterSalesInvoiceFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintCounterSalesInvoiceHeader(writer);
                    PrintedLines = 0;
                    SubTotalGrossAmt = 0;
                    SubTotalSGSTAmt = 0;
                    SubTotalCGSTAmt = 0;
                    SubTotalCessAmt = 0;
                    SubTotalNetAmt = 0;
                }
                decimal GstPer = items.value.SGSTPercentage + items.value.CGSTPercentage;
                TotalGrossAmt += items.value.GrossAmount;
                SubTotalGrossAmt += items.value.GrossAmount;
                SubTotalSGSTAmt += items.value.SGSTAmount;
                SubTotalCGSTAmt += items.value.CGSTAmount;
                SubTotalCessAmt += items.value.CessAmount;
                SubTotalNetAmt += items.value.NetAmount;
                ItemLengthLimit = 40;
                string name = PHelper.SplitString(items.value.Name, ItemLengthLimit);
                string[] itemname = name.Split(new char[] { '\r' }, 2);
                if (itemname[1] != " " && (PrintedLines == BodyLines - 1))
                {
                    writer.WriteLine();
                    PrintCounterSalesInvoiceFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintCounterSalesInvoiceHeader(writer);
                    PrintedLines = 0;
                    SubTotalGrossAmt = 0;
                    SubTotalSGSTAmt = 0;
                    SubTotalCGSTAmt = 0;
                    SubTotalCessAmt = 0;
                    SubTotalNetAmt = 0;
                }
                string Content = string.Format(ItemLineTemplate,
                    (items.i).ToString(),
                    itemname[0],
                    items.value.Unit,
                    items.value.BatchNo,
                    Convert.ToDateTime(items.value.ExpiryDate).ToString("MMM y"),
                    string.Format("{0:0.00}", items.value.BasicPrice),
                    string.Format("{0:0}", items.value.Quantity),
                    string.Format("{0:0.00}", items.value.GrossAmount),
                    GstPer.ToString(),
                    string.Format("{0:0.00}", items.value.SGSTAmount),
                    string.Format("{0:0.00}", items.value.CGSTAmount),
                    string.Format("{0:0.00}", items.value.CessAmount),
                    string.Format("{0:0.00}", items.value.NetAmount));
                writer.WriteLine(PHelper.Condense(Content));
                ++PrintedLines;
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
                    "");
                    writer.WriteLine(PHelper.Condense(Content2));
                    ++PrintedLines;
                }
            }
        }

        private void PrintCounterSalesInvoiceFooter(StreamWriter writer)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;
            if (CurrentPages == TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("CounterSalesForwardLines"));
                int BlankLines = BodyLines - PrintedLines;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                string FooterLineTemplate = "{0,-4}{1,-43}{2,-7}{3,-8}{4,-6}{5,8}{6,7}{7,9}{8,6}{9,7}{10,7}{11,7}{12,9}{13,9}";
                string FooterContent = string.Format(FooterLineTemplate,
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                string.Format("{0:0.00}", TotalGrossAmt),
                "",
                string.Format("{0:0.00}", counterSales.SGSTAmount),
                string.Format("{0:0.00}", counterSales.CGSTAmount),
                string.Format("{0:0.00}", counterSales.CessAmount),
                string.Format("{0:0.00}", counterSales.NetAmount),
                "");
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterContent)));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.AlignText(PHelper.Condense("Grand Total(Rounded off) "), PrintHelper.CharAlignment.AlignRight, 82) + PHelper.Bold(string.Format("{0:0.00}", counterSales.NetAmount)));
                writer.WriteLine();
                writer.WriteLine(PHelper.Condense(PHelper.Bold("Total : Rupees " + generalBL.NumberToText(Convert.ToInt32(counterSales.NetAmount)) + "Only")));
                if (counterSales.Type == "Employee")
                {
                    writer.WriteLine(PHelper.Condense("Total Outstanding Balance : ") + PHelper.Bold(string.Format("{0:0.00}", counterSales.OutstandingAmount)));
                }
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Authorised Signatory", PrintHelper.CharAlignment.AlignRight, 137)));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("[With Status & Seal]", PrintHelper.CharAlignment.AlignRight, 137)));
                for (int index = 1; index <= ForwardLines; ++index)
                    FLineFeed = FLineFeed + PHelper.ForwardLineFeed();
                writer.WriteLine(FLineFeed);
            }
            else
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("CounterSalesContinueForwardLines"));
                string FooterLineTemplate = "{0,-4}{1,-43}{2,-7}{3,-8}{4,-6}{5,8}{6,7}{7,9}{8,6}{9,7}{10,7}{11,7}{12,9}{13,9}";
                string FooterContent = string.Format(FooterLineTemplate,
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                string.Format("{0:0.00}", SubTotalGrossAmt),
                "",
                string.Format("{0:0.00}", SubTotalSGSTAmt),
                string.Format("{0:0.00}", SubTotalCGSTAmt),
                string.Format("{0:0.00}", SubTotalCessAmt),
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
        public DatatableResultBO GetCustomerCounterSalesList(int CustomerID,string TransNoHint, string TransDateHint, string PartyNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return counterSalesDal.GetCustomerCounterSalesList(CustomerID,TransNoHint, TransDateHint,PartyNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetListForCounterSales(string Type, string TransNoHint, string TransDateHint, string SalesTypeHint, string PartyNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return counterSalesDal.GetListForCounterSales(Type, TransNoHint, TransDateHint, SalesTypeHint, PartyNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetAppointmentProcessList(string TransNoHint, string TransDateHint, string PatientNameHint, string DoctorNameHint, string PhoneHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return counterSalesDal.GetAppointmentProcessList(TransNoHint, TransDateHint, PatientNameHint, DoctorNameHint, PhoneHint, SortField, SortOrder, Offset, Limit);
        }
        public List<SalesItemBO> GetBatchwisePrescriptionItems(int AppointmentProcessID, int PatientID)
        {
            return counterSalesDal.GetBatchwisePrescriptionItems(AppointmentProcessID, PatientID);
        }

        public bool IsCancelable(int counterSalesID)
        {
            return counterSalesDal.IsCancelable(counterSalesID);
        }

        public List<CounterSalesBO> GetCounterSalesSignOutPrint(string Type)
        {
            return counterSalesDal.GetCounterSalesSignOutPrint(Type);
        }

        public bool IsDotMatrixPrint()
        {
            return counterSalesDal.IsDotMatrixPrint();
        }
        public bool IsThermalPrint()
        {
            return counterSalesDal.IsThermalPrint();
        }
        public CurrencyClassBO GetCurrencyDecimalClassByCurrencyID(int CurrencyID)
        {
            return counterSalesDal.GetCurrencyDecimalClassByCurrencyID(CurrencyID);
        }
    }
}
