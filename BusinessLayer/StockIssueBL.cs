//File created by prama on 19-4-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using System.IO;

namespace BusinessLayer
{
    public class StockIssueBL : IStockIssueContract
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        private int ItemLengthLimit;
        StockIssueDAL stockIssueDAL;
        StockIssueBO stockIssue;
        PrintHelper PHelper;
        List<AddressBO> billingAddressBO;
        List<AddressBO> shippingAddressBO;
        AddressDAL addressDAL;
        private IGeneralContract generalBL;

        decimal TotalGrossAmt = 0;
        decimal SubTotalGrossAmt = 0;
        decimal SubTotalTaxableAmt = 0;
        decimal SubTotalDiscAmt = 0;
        decimal SubTotalAddDiscAmt = 0;
        decimal SubTotalTODAmt = 0;
        decimal CessPer5Amt = 0;
        decimal CessPer12Amt = 0;
        decimal CessPer18Amt = 0;
        decimal TaxableValue5PerAmt = 0;
        decimal TaxableValue12PerAmt = 0;
        decimal TaxableValue18PerAmt = 0;

        public StockIssueBL()
        {
            stockIssueDAL = new StockIssueDAL();
            PHelper = new PrintHelper();
            generalBL = new GeneralBL();
            addressDAL = new AddressDAL();
        }

        public bool SaveStockIssue(StockIssueBO stockIssueBO, List<StockIssueItemBO> stockIssueItems, List<StockIssuePackingDetailsBO> PackingDetails)

        {
            if (stockIssueBO.ID > 0)
            {
                return stockIssueDAL.UpdateStockIssue(stockIssueBO, stockIssueItems, PackingDetails);

            }
            else
            {
                return stockIssueDAL.SaveStockIssue(stockIssueBO, stockIssueItems, PackingDetails);
            }
        }

        public List<StockIssueBO> GetStockIssueList()
        {
            return stockIssueDAL.GetStockIssueList();
        }

        public List<StockIssueBO> GetStockIssueDetail(int ID)
        {
            return stockIssueDAL.GetStockIssueDetail(ID);
        }

        public List<StockIssueItemBO> GetIssueTrans(int ID)
        {
            return stockIssueDAL.GetIssueTrans(ID);
        }

        public List<StockIssuePackingDetailsBO> GetPackingDetails(int ID, string Type)
        {
            return stockIssueDAL.GetPackingDetails(ID, Type);

        }
        public List<StockIssueBO> GetUnProcessedSIList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID)
        {
            return stockIssueDAL.GetUnProcessedSIList(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID);

        }

        public List<StockIssueItemBO> GetUnProcessedSITransList(int ID)
        {
            return stockIssueDAL.GetUnProcessedSITransList(ID);
        }

        public int Cancel(int ID, string Table)
        {
            return stockIssueDAL.Cancel(ID, Table);
        }

        public List<StockIssueItemBO> GetBatchwiseItem(int ItemID, int BatchTypeID, decimal Qty, int WarehouseID, int UnitID)
        {
            return stockIssueDAL.GetBatchwiseItem(ItemID, BatchTypeID, Qty, WarehouseID, UnitID);
        }

        public DatatableResultBO GetStockIssueList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return stockIssueDAL.GetStockIssueList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
        }
        public List<StockIssueItemBO> ReadExcel(string Path)
        {
            ItemContract ItemBL = new ItemBL();
            ItemBO Item;
            IDictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(1, "Code");
            dict.Add(3, "Name");
            dict.Add(4, "BatchType");
            dict.Add(5, "Stock");
            dict.Add(6, "IssueQty");
            StockIssueItemBO UploadItemList = new StockIssueItemBO();
            GeneralBL generalBL = new GeneralBL();
            List<StockIssueItemBO> ItemList;
            try
            {
                ItemList = generalBL.ReadExcel(Path, UploadItemList, dict);
                ItemList = ItemList.Where(a => a.Code != "" && a.IssueQty > 0).ToList();
                var ItemCodes = ItemList.Select(a => a.Code);
                List<ItemBO> Items = ItemBL.GetItemsByItemCodes(ItemCodes.ToArray());

                ItemList.Select(a =>
                {
                    Item = Items.Where(b => b.Code == a.Code).FirstOrDefault();
                    a.BatchTypeID = a.BatchType == "OSK" ? 2 : a.BatchType == "ISK" ? 1 : a.BatchType == "Export" ? 3 : 0;
                    a.ItemID = Item.ID;
                    a.Unit = Item.Unit;
                    a.UnitID = Item.UnitID;

                    return a;
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return ItemList.ToList();
        }
        public List<StockIssueItemBO> GetItemsToGrid(List<StockIssueItemBO> stockIssueItems, int IssuePremiseID)
        {
            return stockIssueDAL.GetItemsToGrid(stockIssueItems, IssuePremiseID);

        }

        public List<StockIssueBO> GetIssueNoAutoCompleteForReport(string CodeHint, DateTime FromDate, DateTime ToDate)
        {
            return stockIssueDAL.GetIssueNoAutoCompleteForReport(CodeHint, FromDate, ToDate);
        }

        public string GetPrintTextFile(int StockIssueID)
        {
            BodyLines = Convert.ToInt32(25);
            ItemLengthLimit = 30;
            PrintedLines = 0;
            string FileName;
            string FilePath;
            string URL;
            stockIssue = new StockIssueBO();
            stockIssue = stockIssueDAL.GetStockIssueDetail(StockIssueID).FirstOrDefault();
            stockIssue.Items = GetIssueTrans(StockIssueID);
            billingAddressBO = addressDAL.GetBillingAddress("Location", stockIssue.IssueLocationID, "").ToList();
            shippingAddressBO = addressDAL.GetShippingAddress("Location", stockIssue.ReceiptLocationID, "").ToList();
            string TransNo = stockIssue.IssueNo;
            FileName = TransNo + ".txt";

            string path = AppDomain.CurrentDomain.BaseDirectory;

            FilePath = path + "/Outputs/StockIssue/" + FileName;
            TotalLines = getNoOfPrintLines(stockIssue.Items);
            SetPageVariables();
            PrintedLines = 0;
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintStockIssueHeader(writer);
                PrintStockIssueBody(writer);
                PrintStockIssueFooter(writer);
            }
            URL = "/Outputs/StockIssue/" + FileName;
            return URL;
        }

        public int getNoOfPrintLines(List<StockIssueItemBO> stockIssueBOList)
        {
            int count = 0;
            foreach (StockIssueItemBO item in stockIssueBOList)
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

        //private void PrintStockIssueHeader(StreamWriter writer)
        //{
        //    if (CurrentPages == 1)
        //    {
        //        int ReverseLines = Convert.ToInt32(generalBL.GetConfig("ProformaInvoiceReverseLines"));
        //        string RLineFeed = string.Empty;
        //        for (int index = 1; index <= ReverseLines; ++index)
        //            RLineFeed = RLineFeed + PHelper.ReverseLineFeed();
        //        writer.WriteLine(RLineFeed);
        //    }
        //    writer.WriteLine(PHelper.Condense(PHelper.Bold("GST : " + GeneralBO.GSTNo)));
        //    writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("PICK LIST", PrintHelper.CharAlignment.AlignCenter, 137))));
        //    writer.WriteLine(PHelper.Condense(PHelper.AlignText("Vehicle No.  : " + PHelper.Bold(stockIssue.VehicleNo), PrintHelper.CharAlignment.AlignRight, 137)));
        //    writer.WriteLine(PHelper.Condense(PHelper.AlignText("Invoice No.    : " + PHelper.Bold(stockIssue.IssueNo), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Invoice Date     : " + PHelper.Bold(Convert.ToDateTime(stockIssue.Date).ToString("dd MMM yyyy")), PrintHelper.CharAlignment.AlignLeft, 47)));
        //    writer.WriteLine(PHelper.Condense(PHelper.AlignText("Issue Location : " + PHelper.Bold(stockIssue.IssueLocationName), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Receipt Location : " + PHelper.Bold(stockIssue.ReceiptLocationName), PrintHelper.CharAlignment.AlignLeft, 47)));
        //    writer.WriteLine(PHelper.Condense(PHelper.AlignText("Issue Premise  : " + PHelper.Bold(stockIssue.IssuePremiseName), PrintHelper.CharAlignment.AlignLeft, 90) + PHelper.AlignText("Receipt Premise  : " + PHelper.Bold(stockIssue.ReceiptPremiseName), PrintHelper.CharAlignment.AlignLeft, 47)));
        //    writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
        //    writer.WriteLine(PHelper.FillChar('-', 80));
        //    string HeadingTemplate = "{0,-4}{1,-61}{2,-11}{3,-20}{4,-8}{5,-10}{6,-10}{7,-12}";
        //    string HeadLine1 = string.Format(HeadingTemplate, "Sl.", "| Item Name", "| Pack Size", "| Batch No.", "| Batch", "| UOM", "| Qty", "| MRP");
        //    string HeadLine2 = string.Format(HeadingTemplate, "No.", "|", "|", "|", "| Type", "|", "|", "|");
        //    writer.WriteLine(PHelper.Condense(HeadLine1));
        //    writer.WriteLine(PHelper.Condense(HeadLine2));
        //    writer.WriteLine(PHelper.FillChar('-', 80));
        //}

        //private void PrintStockIssueBody(StreamWriter writer)
        //{
        //    int FooterLines = 3;
        //    int PrintLines = stockIssue.Items.Count();
        //    var category = "";
        //    var i = 0;
        //    string ItemLineTemplate = "{0,-5}{1,-61}{2,-11}{3,-20}{4,-8}{5,-10}{6,10}{7,12}";
        //    while (true)
        //    {
        //        PrintedLines++;
        //        if (category != stockIssue.Items[i].Category)
        //        {
        //            category = stockIssue.Items[i].Category;
        //            string Content1 = string.Format(ItemLineTemplate,
        //            "",
        //            category,
        //            "",
        //            "",
        //            "",
        //            "",
        //            "",
        //            "");
        //            writer.WriteLine(PHelper.Condense(PHelper.Bold(Content1)));
        //            PrintedLines++;
        //        }
        //        string Content = string.Format(ItemLineTemplate,
        //                    ((i + 1)).ToString(),
        //                    stockIssue.Items[i].Name,
        //                    string.Format("{0:0}", stockIssue.Items[i].PackSize) + stockIssue.Items[i].SecondaryUnit,
        //                    stockIssue.Items[i].BatchName,
        //                    stockIssue.Items[i].BatchType,
        //                    stockIssue.Items[i].Unit,
        //                    string.Format("{0:0}", stockIssue.Items[i].IssueQty),
        //                    string.Format("{0:0.00}", stockIssue.Items[i].Rate));
        //        writer.WriteLine(PHelper.Condense(Content));
        //        i++;
        //        if (PrintedLines >= BodyLines)
        //        {
        //            PrintStockIssueFooter(writer);
        //            ++CurrentPages;
        //            for (int index = 1; index <= FooterLines; ++index)
        //                writer.WriteLine(PHelper.LineSpacing1_6());
        //            PrintStockIssueHeader(writer);
        //            PrintedLines = 0;
        //        }
        //        if (i == PrintLines)
        //        {
        //            break;
        //        }
        //    }
        //}

        //private void PrintStockIssueFooter(StreamWriter writer)
        //{
        //    int ForwardLines;
        //    string FLineFeed = string.Empty;
        //    var packs = stockIssue.Items.OrderBy(a => a.Category).GroupBy(g => Convert.ToInt16(g.PackSize).ToString() + " " + g.SecondaryUnit)
        //        .Select(a => new { Count = a.Count(), Pack = a.Key, TotalQuantity = a.Sum(b => b.IssueQty) });
        //    if (CurrentPages == TotalPages)
        //    {
        //        ForwardLines = Convert.ToInt32(generalBL.GetConfig("ProformaInvoiceForwardLines"));
        //        int BlankLines = BodyLines - PrintedLines;
        //        for (int index = 1; index <= BlankLines; ++index)
        //            writer.WriteLine();
        //        writer.WriteLine(PHelper.FillChar('-', 80));
        //        writer.WriteLine();
        //        string FooterHeadTemplate = "{0,-10}{1,-10}";
        //        string FooterLineTemplate = "{0,-10}{1,10}";
        //        string FooterHeadContent = string.Format(FooterHeadTemplate, "Pack Size", "| Nos");
        //        writer.WriteLine(PHelper.FillChar('-', 20));
        //        writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterHeadContent)));
        //        writer.WriteLine(PHelper.FillChar('-', 20));
        //        foreach (var pack in packs)
        //        {
        //            string FooterContent = string.Format(FooterLineTemplate, pack.Pack, string.Format("{0:0}", pack.TotalQuantity));
        //            writer.WriteLine(PHelper.Condense(FooterContent));
        //        }
        //        writer.WriteLine(PHelper.FillChar('-', 20));
        //        writer.WriteLine();
        //        writer.WriteLine(PHelper.Condense(PHelper.AlignText("Authorised Signatory", PrintHelper.CharAlignment.AlignRight, 137)));
        //        writer.WriteLine(PHelper.Condense(PHelper.AlignText("[With Status & Seal]", PrintHelper.CharAlignment.AlignRight, 137)));
        //        writer.WriteLine(PHelper.FormFeed());
        //    }
        //    else
        //    {
        //        ForwardLines = Convert.ToInt32(generalBL.GetConfig("ProformaInvoiceContinueForwardLines"));
        //        writer.WriteLine(PHelper.FillChar('-', 80));
        //        writer.WriteLine();
        //        writer.WriteLine(PHelper.Condense(PHelper.AlignText("Continue..", PrintHelper.CharAlignment.AlignRight, 137)));
        //        for (int index = 1; index <= ForwardLines; ++index)
        //            FLineFeed = FLineFeed + PHelper.ForwardLineFeed();
        //        writer.WriteLine(FLineFeed);
        //    }
        //}

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

        private void PrintStockIssueHeader(StreamWriter writer)
        {
            if (CurrentPages == 1)
            {
                int ReverseLines = Convert.ToInt32(generalBL.GetConfig("SalesInvoiceReverseLines"));
                string RLineFeed = string.Empty;
                for (int index = 1; index <= ReverseLines; ++index)
                    RLineFeed = RLineFeed + PHelper.ReverseLineFeed();
                writer.WriteLine(RLineFeed);
            }
            writer.WriteLine(PHelper.Condense(PHelper.Bold("GST : " + GeneralBO.GSTNo + PHelper.AlignText("CIN : " + GeneralBO.CINNo, PrintHelper.CharAlignment.AlignRight, 115))));
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("Stock Transfer", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine(PHelper.Bold(PHelper.AlignText(GeneralBO.CompanyName.ToUpper(), PrintHelper.CharAlignment.AlignCenter, 80)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address1 + "," + GeneralBO.Address2, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address3 + "," + GeneralBO.Address4, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address5 + "," + GeneralBO.PIN + "," + GeneralBO.LandLine1, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Issue No.  : " + PHelper.Bold(stockIssue.IssueNo), PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText("Vehicle No.  : " + PHelper.Bold(stockIssue.VehicleNo), PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense("Issue Date : " + PHelper.Bold(Convert.ToDateTime(stockIssue.Date).ToString("dd MMM yyyy"))));
            writer.WriteLine(PHelper.FillChar('-', 80));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Issue Location :", PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText("Receipt Location :", PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText(stockIssue.IssueLocationName, PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(stockIssue.ReceiptLocationName, PrintHelper.CharAlignment.AlignLeft, 67))));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(billingAddressBO.FirstOrDefault().AddressLine1.ToString(), PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(shippingAddressBO.FirstOrDefault().AddressLine1.ToString(), PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(billingAddressBO.FirstOrDefault().AddressLine2.ToString(), PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(shippingAddressBO.FirstOrDefault().AddressLine2.ToString(), PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(billingAddressBO.FirstOrDefault().AddressLine3.ToString(), PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(shippingAddressBO.FirstOrDefault().AddressLine3.ToString(), PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Dist : " + billingAddressBO.FirstOrDefault().District.ToString(), PrintHelper.CharAlignment.AlignLeft, 30) + PHelper.AlignText("State : " + billingAddressBO.FirstOrDefault().State.ToString(), PrintHelper.CharAlignment.AlignLeft, 40)
                + PHelper.AlignText("Dist : " + shippingAddressBO.FirstOrDefault().District.ToString(), PrintHelper.CharAlignment.AlignLeft, 30) + PHelper.AlignText("State : " + shippingAddressBO.FirstOrDefault().State.ToString(), PrintHelper.CharAlignment.AlignLeft, 37)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("PIN  : " + billingAddressBO.FirstOrDefault().PIN, PrintHelper.CharAlignment.AlignLeft, 30) + PHelper.AlignText("Ph    : " + shippingAddressBO.FirstOrDefault().MobileNo, PrintHelper.CharAlignment.AlignLeft, 40) + PHelper.AlignText("PIN  : " + shippingAddressBO.FirstOrDefault().PIN, PrintHelper.CharAlignment.AlignLeft, 30) + PHelper.AlignText("Ph    : " + shippingAddressBO.FirstOrDefault().MobileNo, PrintHelper.CharAlignment.AlignLeft, 37)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("GST  : " + stockIssue.IssueLocationGSTNo, PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText("GST  : " + stockIssue.ReceiptLocationGSTNo, PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(string.Concat(new object[4] { "Page: ", CurrentPages, " of ", TotalPages }), PrintHelper.CharAlignment.AlignRight, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            string HeadingTemplate = "{0,-3}{1,-33}{2,-6}{3,-9}{4,-6}{5,-10}{6,-9}{7,-7}{8,-9}{9,-9}{10,-9}{11,-9}{12,-10}{13,-8}";
            string HeadLine1 = string.Format(HeadingTemplate, "Sl.", "| Commodity / Item", "| UOM ", "| Batch", "| Exp.", "| MRP",
                "| Basic", "| Qty", "| Gross", "| Disc", "| Addl", "| TOD", "| Taxable", "| GST");
            string HeadLine2 = string.Format(HeadingTemplate, "No.", "|", "|", "|", "| Date", "|", "| Price", "|", "| Amt", "| Amt", "| Disc", "|", "| Amt", "|%");
            writer.WriteLine(PHelper.Condense(HeadLine1));
            writer.WriteLine(PHelper.Condense(HeadLine2));
            writer.WriteLine(PHelper.FillChar('-', 80));
        }

        private void PrintStockIssueBody(StreamWriter writer)
        {
            int FooterLines = 3;
            foreach (var items in stockIssue.Items.Select((value, i) => new { i = (i + 1), value }))
            {
                string ItemLineTemplate = "{0,-3}{1,-33}{2,-6}{3,-9}{4,-6}{5,10}{6,9}{7,7}{8,9}{9,9}{10,9}{11,9}{12,10}{13,8}";
                if (PrintedLines == BodyLines)
                {
                    PrintStockIssueFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintStockIssueHeader(writer);
                    PrintedLines = 0;
                    SubTotalGrossAmt = 0;
                    SubTotalDiscAmt = 0;
                    SubTotalAddDiscAmt = 0;
                    SubTotalTODAmt = 0;
                    SubTotalTaxableAmt = 0;
                }

                decimal GstPer = items.value.SGSTPercentage + items.value.CGSTPercentage;
                TotalGrossAmt += items.value.GrossAmount;
                SubTotalGrossAmt += items.value.GrossAmount;
                SubTotalDiscAmt += items.value.TradeDiscount;
                SubTotalAddDiscAmt += 0;
                SubTotalTODAmt += 0;
                SubTotalTaxableAmt += items.value.TaxableAmount;
                if (items.value.IGSTPercentage == 5)
                {
                    CessPer5Amt += 0;
                    TaxableValue5PerAmt += items.value.TaxableAmount;
                }
                else if (items.value.IGSTPercentage == 12)
                {
                    CessPer12Amt += 0;
                    TaxableValue12PerAmt += items.value.TaxableAmount;
                }
                else if (items.value.IGSTPercentage == 18)
                {
                    CessPer18Amt += 0;
                    TaxableValue18PerAmt += items.value.TaxableAmount;
                }
                ItemLengthLimit = 30;
                string name = PHelper.SplitString(items.value.Name, ItemLengthLimit);
                string[] itemname = name.Split(new char[] { '\r' }, 2);
                if (itemname[1] != " " && (PrintedLines == BodyLines - 1))
                {
                    writer.WriteLine();
                    PrintStockIssueFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintStockIssueHeader(writer);
                    PrintedLines = 0;
                    SubTotalGrossAmt = 0;
                    SubTotalDiscAmt = 0;
                    SubTotalAddDiscAmt = 0;
                    SubTotalTODAmt = 0;
                    SubTotalTaxableAmt = 0;
                }
                string Content = string.Format(ItemLineTemplate,
                 (items.i).ToString(),
                 itemname[0],
                 items.value.Unit,
                 items.value.BatchName,
                 Convert.ToDateTime(items.value.ExpiryDate).ToString("MMM y"),
                 string.Format("{0:0.00}", items.value.Rate),
                 string.Format("{0:0.00}", items.value.BasicPrice),
                 string.Format("{0:0}", items.value.IssueQty),
                 string.Format("{0:0.00}", items.value.GrossAmount),
                 string.Format("{0:0.00}", items.value.TradeDiscount),
                 string.Format("{0:0.00}", 0),
                 string.Format("{0:0.00}", 0),
                 string.Format("{0:0.00}", items.value.TaxableAmount),
                 GstPer.ToString());
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

        private void PrintStockIssueFooter(StreamWriter writer)
        {
            int ForwardLines;
            string FLineFeed = string.Empty;
            decimal SGST5Amt = 0;
            decimal CGST5Amt = 0;
            decimal IGST5Amt = 0;
            decimal SGST6Amt = 0;
            decimal CGST6Amt = 0;
            decimal IGST12Amt = 0;
            decimal SGST9Amt = 0;
            decimal CGST9Amt = 0;
            decimal IGST18Amt = 0;
            decimal CessPer = 0;
            decimal CessAmt = 0;
            decimal TotalSGSTAmt = 0;
            decimal TotalCGSTAmt = 0;
            decimal TotalIGSTAmt = 0;
            if (CurrentPages == TotalPages)
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("SalesInvoiceForwardLines"));
                int BlankLines = BodyLines - PrintedLines;
                foreach (var items in stockIssue.Items.Select((value, i) => new { i = (i + 1), value }))
                {
                    if (items.value.SGSTPercentage == Convert.ToDecimal(2.50) && items.value.SGSTAmount > 0)
                    {
                        SGST5Amt += items.value.SGSTAmount;
                    }
                    if (items.value.SGSTPercentage == Convert.ToDecimal(6) && items.value.SGSTAmount > 0)
                    {
                        SGST6Amt += items.value.SGSTAmount;
                    }
                    if (items.value.SGSTPercentage == Convert.ToDecimal(9) && items.value.SGSTAmount > 0)
                    {
                        SGST9Amt += items.value.SGSTAmount;
                    }
                    if (items.value.SGSTPercentage == Convert.ToDecimal(2.50) && items.value.CGSTAmount > 0)
                    {
                        CGST5Amt += items.value.CGSTAmount;
                    }
                    if (items.value.SGSTPercentage == Convert.ToDecimal(6) && items.value.CGSTAmount > 0)
                    {
                        CGST6Amt += items.value.CGSTAmount;
                    }
                    if (items.value.SGSTPercentage == Convert.ToDecimal(9) && items.value.CGSTAmount > 0)
                    {
                        CGST9Amt += items.value.CGSTAmount;
                    }
                    if (items.value.IGSTPercentage == Convert.ToDecimal(5) && items.value.IGSTAmount > 0)
                    {
                        IGST5Amt += items.value.IGSTAmount;
                    }
                    if (items.value.IGSTPercentage == Convert.ToDecimal(12) && items.value.IGSTAmount > 0)
                    {
                        IGST12Amt += items.value.IGSTAmount;
                    }
                    if (items.value.IGSTPercentage == Convert.ToDecimal(18) && items.value.IGSTAmount > 0)
                    {
                        IGST18Amt += items.value.IGSTAmount;
                    }
                }
                TotalSGSTAmt = SGST5Amt + SGST6Amt + SGST9Amt;
                TotalCGSTAmt = CGST5Amt + CGST6Amt + CGST9Amt;
                TotalIGSTAmt = IGST5Amt + IGST12Amt + IGST18Amt;
                for (int index = 1; index <= BlankLines; ++index)
                    writer.WriteLine();
                string FooterLineTemplate = "{0,-3}{1,-33}{2,-6}{3,-9}{4,-6}{5,10}{6,9}{7,-7}{8,9}{9,9}{10,9}{11,9}{12,10}{13,8}";
                string FooterContent = string.Format(FooterLineTemplate,
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                string.Format("{0:0.00}", TotalGrossAmt),
                string.Format("{0:0.00}", stockIssue.TradeDiscount),
                string.Format("{0:0.00}", 0),
                string.Format("{0:0.00}", 0),
                string.Format("{0:0.00}", stockIssue.TaxableAmount),
                "");
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.Bold(FooterContent)));
                writer.WriteLine(PHelper.FillChar('-', 80));
                string GSTFooter = "{0,-21}{1,10}{2,-4}{3,-21}{4,10}{5,-4}{6,-21}{7,10}{8,-4}{9,-21}{10,10}";
                string TaxableValueFooterContent = string.Format(GSTFooter,
                    "Taxable Value 5 %   :",
                    string.Format("{0:0.00}", TaxableValue5PerAmt),
                    "",
                    "Taxable Value 12 %  :",
                    string.Format("{0:0.00}", TaxableValue12PerAmt),
                    "",
                    "Taxable Value 18 %  :",
                    string.Format("{0:0.00}", TaxableValue18PerAmt),
                    "",
                    "Total Taxable Amt   :",
                    string.Format("{0:0.00}", stockIssue.TaxableAmount));
                writer.WriteLine(PHelper.Condense(TaxableValueFooterContent));
                if (TotalIGSTAmt > 0)
                {
                    string IGSTFooterContent = string.Format(GSTFooter,
                        "IGST 5 %          :",
                        string.Format("{0:0.00}", IGST5Amt),
                        "",
                        "IGST 6 %            :",
                        string.Format("{0:0.00}", IGST12Amt),
                        "",
                        "IGST 9 %            :",
                        string.Format("{0:0.00}", IGST18Amt),
                        "",
                        "IGST Total          :",
                        string.Format("{0:0.00}", TotalIGSTAmt));
                    writer.WriteLine(PHelper.Condense(IGSTFooterContent));
                    writer.WriteLine();
                    writer.WriteLine();
                    writer.WriteLine();
                }
                else
                {
                    string SGSTFooterContent = string.Format(GSTFooter,
                        "SGST 2.5 %          :",
                        string.Format("{0:0.00}", SGST5Amt),
                        "",
                        "SGST 6 %            :",
                        string.Format("{0:0.00}", SGST6Amt),
                        "",
                        "SGST 9 %            :",
                        string.Format("{0:0.00}", SGST9Amt),
                        "",
                        "SGST Total          :",
                        string.Format("{0:0.00}", TotalSGSTAmt));
                    string CGSTFooterContent = string.Format(GSTFooter,
                        "CGST 2.5 %          :",
                        string.Format("{0:0.00}", CGST5Amt),
                        "",
                        "CGST 6 %            :",
                        string.Format("{0:0.00}", CGST6Amt),
                        "",
                        "CGST 9 %            :",
                        string.Format("{0:0.00}", CGST9Amt),
                        "",
                        "CGST Total          :",
                        string.Format("{0:0.00}", TotalCGSTAmt));
                    string GSTFooterContent = string.Format(GSTFooter,
                        "Total GST           :",
                        string.Format("{0:0.00}", CGST5Amt + SGST5Amt),
                        "",
                        "",
                        string.Format("{0:0.00}", CGST6Amt + SGST6Amt),
                        "",
                        "",
                        string.Format("{0:0.00}", CGST9Amt + SGST9Amt),
                        "",
                        "GST Total           :",
                        string.Format("{0:0.00}", TotalSGSTAmt + TotalCGSTAmt));
                    string CessFooterContent = string.Format(GSTFooter,
                        "Cess 1 % (GST 5 %)  :",
                        string.Format("{0:0.00}", CessPer5Amt),
                        "",
                        "Cess 1 % (GST 12 %) :",
                        string.Format("{0:0.00}", CessPer12Amt),
                        "",
                        "Cess 1 % (GST 18 %) :",
                        string.Format("{0:0.00}", CessPer18Amt),
                        "",
                        "Cess Total          :",
                        string.Format("{0:0.00}", CessAmt));
                    writer.WriteLine(PHelper.Condense(SGSTFooterContent));
                    writer.WriteLine(PHelper.Condense(CGSTFooterContent));
                    writer.WriteLine(PHelper.Condense(GSTFooterContent));
                    writer.WriteLine(PHelper.Condense(CessFooterContent));
                }
                string TotalFooterContent = string.Format(GSTFooter,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "Tax Total           :",
                    string.Format("{0:0.00}", CessAmt + TotalSGSTAmt + TotalCGSTAmt + TotalIGSTAmt));
                writer.WriteLine(PHelper.Condense(TotalFooterContent));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("", PrintHelper.CharAlignment.AlignLeft, 85)
                    + PHelper.AlignText("Grand Total(Rounded off : " + string.Format("{0:0.00}", stockIssue.RoundOff) + ")  :  ", PrintHelper.CharAlignment.AlignRight, 40)
                    + PHelper.Bold(PHelper.AlignText(string.Format("{0:0.00}", stockIssue.NetAmount), PrintHelper.CharAlignment.AlignRight, 12))));
                writer.WriteLine(PHelper.Condense("Total       :  Rupees " + PHelper.Bold(generalBL.NumberToText(Convert.ToInt32(stockIssue.NetAmount)) + "Only")));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("Authorised Signatory", PrintHelper.CharAlignment.AlignRight, 137)));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("[With Status & Seal]", PrintHelper.CharAlignment.AlignRight, 137)));
                writer.WriteLine(PHelper.FormFeed());
            }
            else
            {
                ForwardLines = Convert.ToInt32(generalBL.GetConfig("SalesInvoiceContinueForwardLines"));
                string FooterLineTemplate = "{0,-3}{1,-33}{2,-6}{3,-9}{4,-6}{5,10}{6,9}{7,-7}{8,9}{9,9}{10,9}{11,9}{12,10}{13,8}";
                string FooterContent = string.Format(FooterLineTemplate,
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                string.Format("{0:0.00}", SubTotalGrossAmt),
                string.Format("{0:0.00}", SubTotalDiscAmt),
                string.Format("{0:0.00}", SubTotalAddDiscAmt),
                string.Format("{0:0.00}", SubTotalTODAmt),
                string.Format("{0:0.00}", SubTotalTaxableAmt),
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

    }
}
