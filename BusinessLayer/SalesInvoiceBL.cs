using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System.IO;
using DataAccessLayer.DBContext;

namespace BusinessLayer
{
    public class SalesInvoiceBL : ISalesInvoice
    {
        private int TotalPages;
        private int CurrentPages;
        private int TotalLines;
        private int BodyLines;
        private int PrintedLines;
        private int ItemLengthLimit;
        private IGeneralContract generalBL;
        PrintHelper PHelper;

        SalesInvoiceDAL salesInvoiceDAL;
        SalesInvoiceBO salesInvoice;
        List<SalesItemBO> salesItemBO;
        List<AddressBO> billingAddressBO;
        List<AddressBO> shippingAddressBO;
        AddressDAL addressDAL;

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

        public SalesInvoiceBL()
        {
            salesInvoiceDAL = new SalesInvoiceDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
            addressDAL = new AddressDAL();
        }

        public int Cancel(int SalesInvoiceID)
        {
            return salesInvoiceDAL.Cancel(SalesInvoiceID);
        }

        public DatatableResultBO GetInvoiceListForSalesReturn(int CustomerID, string TransHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInvoiceDAL.GetInvoiceListForSalesReturn(CustomerID, TransHint, DateHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetSalesInvoiceHistory(int ItemID, string SalesOrderNos, string InvoiceDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInvoiceDAL.GetSalesInvoiceHistory(ItemID, SalesOrderNos, InvoiceDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetCounterSalesHistory(int ItemID, string TransNo, string TransDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInvoiceDAL.GetCounterSalesHistory(ItemID, TransNo, TransDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetPurchaseHistory(int ItemID, string PurchaseOrderNo, string PurchaseOrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInvoiceDAL.GetPurchaseHistory(ItemID, PurchaseOrderNo, PurchaseOrderDate, SupplierName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetPendingPOHistory(int ItemID, string PurchaseOrderNo, string PurchaseOrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInvoiceDAL.GetPendingPOHistory(ItemID, PurchaseOrderNo, PurchaseOrderDate, SupplierName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
        }
        
        public DatatableResultBO GetLegacyPurchaseHistory(int ItemID, string ReferenceOrderNo, string OrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInvoiceDAL.GetLegacyPurchaseHistory(ItemID,  ReferenceOrderNo,  OrderDate,  SupplierName,  ItemName,  PartsNumber,  SortField,  SortOrder,  Offset,  Limit);
        }
        public List<SalesInvoiceItemBO> GetInvoiceTransList(int InvoiceID, int PriceListID)
        {
            return salesInvoiceDAL.GetInvoiceTransList(InvoiceID, PriceListID);
        }

        public List<SalesInvoiceBO> GetIntercompanySalesInvoiceList(int SupplierID, int LocationID)
        {
            return salesInvoiceDAL.GetIntercompanySalesInvoiceList(SupplierID, LocationID);
        }

        public SalesInvoiceBO GetSalesInvoice(int SalesInvoiceID, int LocationID)
        {
            return salesInvoiceDAL.GetSalesInvoice(SalesInvoiceID, LocationID);
        }

        public List<SalesAmountBO> GetSalesInvoiceAmountDetails(int SalesInvoiceID, int LocationID)
        {
            return salesInvoiceDAL.GetSalesInvoiceAmountDetails(SalesInvoiceID, LocationID);
        }

        public List<SalesPackingDetailsBO> GetSalesInvoicePackingDetails(int SalesInvoiceID, int LocationID)
        {
            return salesInvoiceDAL.GetSalesInvoicePackingDetails(SalesInvoiceID, LocationID);
        }

        public List<SalesItemBO> GetSalesInvoiceItems(int SalesInvoiceID, int LocationID)
        {
            List<SalesItemBO> Items = salesInvoiceDAL.GetSalesInvoiceItems(SalesInvoiceID, LocationID);

            Items = Items.Select(item =>
            {
                if (item.InvoiceQty <= item.Stock)
                {
                    item.InvoiceQtyMet = true;
                    item.InvoiceOfferQtyMet = true;
                }
                else if (item.InvoiceQty - item.InvoiceOfferQty <= item.Stock)
                {
                    item.InvoiceQtyMet = true;
                    item.InvoiceOfferQtyMet = false;
                }
                else if (item.InvoiceQty > item.Stock)
                {
                    item.InvoiceQtyMet = false;
                    item.InvoiceOfferQtyMet = false;
                }
                return item;
            }).ToList();

            return Items;

        }

        public List<SalesItemBO> GetGoodsReceiptSalesInvoiceItems(int[] SalesInvoiceID, int LocationID)
        {
            string CommaSeparatedSalesInvoiceIDs = string.Join(",", SalesInvoiceID.Select(x => x.ToString()).ToArray());

            return salesInvoiceDAL.GetGoodsReceiptSalesInvoiceItems(CommaSeparatedSalesInvoiceIDs, LocationID);
        }

        public DatatableResultBO GetSalesInvoiceList(string Type, string CodeHint, string DateHint, string SalesTypeHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInvoiceDAL.GetSalesInvoiceList(Type, CodeHint, DateHint, SalesTypeHint, CustomerNameHint, LocationHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetCustomerSalesInvoiceList(int CustomerID,string TransNoHint, string TranDateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesInvoiceDAL.GetCustomerSalesInvoiceList(CustomerID,TransNoHint, TranDateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }

        public bool IsCancelable(int SalesInvoiceID)
        {
            return salesInvoiceDAL.IsCancelable(SalesInvoiceID);
        }

        public int Save(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails, List<SalesPackingDetailsBO> PackingDetails)
        {
            string XMLInvoice;
            string XMLItems;
            string XMLAmountDetails;
            string XMLPackingDetails;

            XMLInvoice = "<Invoices>";
            XMLInvoice += "<Invoice>";
            XMLInvoice += "<TransNo>" + Invoice.InvoiceNo + "</TransNo>";
            XMLInvoice += "<SalesTypeID>" + Invoice.SalesTypeID + "</SalesTypeID>";
            XMLInvoice += "<TransDate>" + Invoice.InvoiceDate + "</TransDate>";
            XMLInvoice += "<CustomerID>" + Invoice.CustomerID + "</CustomerID>";
            XMLInvoice += "<SalesOrderNos>" + Invoice.SalesOrderNos + "</SalesOrderNos>";
            XMLInvoice += "<PaymentModeID>" + Invoice.PaymentModeID + "</PaymentModeID>";
            XMLInvoice += "<PaymentTypeID>" + Invoice.PaymentTypeID + "</PaymentTypeID>";
            XMLInvoice += "<SchemeID>" + Invoice.SchemeID + "</SchemeID>";
            XMLInvoice += "<GrossAmount>" + Invoice.GrossAmount + "</GrossAmount>";
            XMLInvoice += "<DiscountAmount>" + Invoice.DiscountAmount + "</DiscountAmount>";
            //XMLInvoice += "<DiscountPercentage>" + Invoice.DiscountPercentage + "</DiscountPercentage>";
            XMLInvoice += "<TurnoverDiscount>" + Invoice.TurnoverDiscount + "</TurnoverDiscount>";
            XMLInvoice += "<AdditionalDiscount>" + Invoice.AdditionalDiscount + "</AdditionalDiscount>";
            XMLInvoice += "<TaxableAmount>" + Invoice.TaxableAmount + "</TaxableAmount>";
            XMLInvoice += "<CGSTAmount>" + Invoice.CGSTAmount + "</CGSTAmount>";
            XMLInvoice += "<SGSTAmount>" + Invoice.SGSTAmount + "</SGSTAmount>";
            XMLInvoice += "<IGSTAmount>" + Invoice.IGSTAmount + "</IGSTAmount>";
            XMLInvoice += "<VATAmount>" + Invoice.VATAmount + "</VATAmount>";
            XMLInvoice += "<IsGST>" + Invoice.IsGST + "</IsGST>";
            XMLInvoice += "<IsVat>" + Invoice.IsVat + "</IsVat>";
            XMLInvoice += "<CurrencyID>" + Invoice.CurrencyID + "</CurrencyID>";
            XMLInvoice += "<CurrencyExchangeRate>" + Invoice.CurrencyExchangeRate + "</CurrencyExchangeRate>";
            XMLInvoice += "<CashDiscount>" + Invoice.CashDiscount + "</CashDiscount>";
            XMLInvoice += "<FreightAmount>" + Invoice.FreightAmount + "</FreightAmount>";
            XMLInvoice += "<RoundOff>" + Invoice.RoundOff + "</RoundOff>";
            XMLInvoice += "<NetAmount>" + Invoice.NetAmount + "</NetAmount>";
            XMLInvoice += "<IsProcessed>" + Invoice.IsProcessed + "</IsProcessed>";
            XMLInvoice += "<IsDraft>" + Invoice.IsDraft + "</IsDraft>";
            XMLInvoice += "<CheckStock>" + Invoice.CheckStock + "</CheckStock>";
            XMLInvoice += "<BillingAddressID>" + Invoice.BillingAddressID + "</BillingAddressID>";
            XMLInvoice += "<ShippingAddressID>" + Invoice.ShippingAddressID + "</ShippingAddressID>";
            XMLInvoice += "<NoOfBags>" + Invoice.NoOfBags + "</NoOfBags>";
            XMLInvoice += "<NoOfBoxes>" + Invoice.NoOfBoxes + "</NoOfBoxes>";
            XMLInvoice += "<NoOfCans>" + Invoice.NoOfCans + "</NoOfCans>";
            XMLInvoice += "<OtherCharges>" + Invoice.OtherCharges + "</OtherCharges>";
            XMLInvoice += "<CustomerPONo>" + Invoice.CustomerPONo + "</CustomerPONo>";
            XMLInvoice += "<CreatedUserID>" + GeneralBO.CreatedUserID + "</CreatedUserID>";
            XMLInvoice += "<FinYear>" + GeneralBO.FinYear + "</FinYear>";
            XMLInvoice += "<LocationID>" + GeneralBO.LocationID + "</LocationID>";
            XMLInvoice += "<ApplicationID>" + GeneralBO.ApplicationID + "</ApplicationID>";
            XMLInvoice += "<CessAmount>" + Invoice.CessAmount + "</CessAmount>";
            XMLInvoice += "<PrintWithItemCode>" + (Invoice.PrintWithItemCode ? 1 : 0) + "</PrintWithItemCode>";
            XMLInvoice += "<Remarks>" + Invoice.Remarks + "</Remarks>";
            XMLInvoice += "<CustomerPODate>" + Invoice.CustomerPODate + "</CustomerPODate>";
            XMLInvoice += "<VATPercentageID>" + Invoice.VATPercentageID + "</VATPercentageID>";
            XMLInvoice += "<VATPercentage>" + Invoice.VATPercentage + "</VATPercentage>";
            XMLInvoice += "<OtherChargesVATAmount>" + Invoice.OtherChargesVATAmount + "</OtherChargesVATAmount>";
            XMLInvoice += "</Invoice>";
            XMLInvoice += "</Invoices>";

            XMLItems = "<InvoiceTrans>";
            int i = 0;
            foreach (var item in Items)
            {
                i++;
                XMLItems += "<Item>";
                XMLItems += "<ItemID>" + item.ItemID + "</ItemID>";
                XMLItems += "<ItemName>" + item.Name + "</ItemName>";
                XMLItems += "<PrintWithItemName>" + (item.PrintWithItemCode ? 1 : 0) + "</PrintWithItemName>";
                XMLItems += "<BatchID>" + item.BatchID + "</BatchID>";
                XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<StoreID>" + item.StoreID + "</StoreID>";
                XMLItems += "<SalesOrderTransID>" + item.SalesOrderItemID + "</SalesOrderTransID>";
                XMLItems += "<ProformaInvoiceTransID>" + item.ProformaInvoiceTransID + "</ProformaInvoiceTransID>";
                XMLItems += "<MRP>" + item.MRP + "</MRP>";
                XMLItems += "<BasicPrice>" + item.BasicPrice + "</BasicPrice>";
                XMLItems += "<PartsNumber>" + item.PartsNumber + "</PartsNumber>";
                XMLItems += "<DeliveryTerm>" + item.DeliveryTerm + "</DeliveryTerm>";
                XMLItems += "<Model>" + item.Model + "</Model>";
                XMLItems += "<Quantity>" + item.Qty + "</Quantity>";
                XMLItems += "<OfferQty>" + item.OfferQty + "</OfferQty>";
                XMLItems += "<InvoiceQty>" + item.InvoiceQty + "</InvoiceQty>";
                XMLItems += "<InvoiceOfferQty>" + item.InvoiceOfferQty + "</InvoiceOfferQty>";
                XMLItems += "<SecondaryUnit>" + item.SecondaryUnit + "</SecondaryUnit>";
                XMLItems += "<SecondaryQty>" + item.SecondaryQty + "</SecondaryQty>";
                XMLItems += "<SecondaryOfferQty>" + item.SecondaryOfferQty + "</SecondaryOfferQty>";
                XMLItems += "<SecondaryMRP>" + item.SecondaryMRP + "</SecondaryMRP>";
                XMLItems += "<SecondaryUnitSize>" + item.SecondaryUnitSize + "</SecondaryUnitSize>";
                XMLItems += "<GrossAmount>" + item.GrossAmount + "</GrossAmount>";
                XMLItems += "<DiscountPercentage>" + item.DiscountPercentage + "</DiscountPercentage>";
                XMLItems += "<DiscountAmount>" + item.DiscountAmount + "</DiscountAmount>";
                XMLItems += "<TurnoverDiscount>" + item.TurnoverDiscount + "</TurnoverDiscount>";
                XMLItems += "<AdditionalDiscount>" + item.AdditionalDiscount + "</AdditionalDiscount>";
                XMLItems += "<TaxableAmount>" + item.TaxableAmount + "</TaxableAmount>";
                XMLItems += "<SGSTPercentage>" + item.SGSTPercentage + "</SGSTPercentage>";
                XMLItems += "<CGSTPercentage>" + item.CGSTPercentage + "</CGSTPercentage>";
                XMLItems += "<IGSTPercentage>" + item.IGSTPercentage + "</IGSTPercentage>";
                XMLItems += "<VATPercentage>" + item.VATPercentage + "</VATPercentage>";
                XMLItems += "<SGSTAmt>" + item.SGST + "</SGSTAmt>";
                XMLItems += "<CGSTAmt>" + item.CGST + "</CGSTAmt>";
                XMLItems += "<IGSTAmt>" + item.IGST + "</IGSTAmt>";
                XMLItems += "<VATAmount>" + item.VATAmount + "</VATAmount>";
                XMLItems += "<IsGST>" + Invoice.IsGST + "</IsGST>";
                XMLItems += "<IsVat>" + Invoice.IsVat + "</IsVat>";
                XMLItems += "<CashDiscount>" + item.CashDiscount + "</CashDiscount>";
                XMLItems += "<NetAmt>" + item.NetAmount + "</NetAmt>";
                XMLItems += "<SortOrder>" + i + "</SortOrder>";
                XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLItems += "<CurrencyID>" + item.CurrencyID + "</CurrencyID>";
                XMLItems += "<ExchangeRate>" + item.ExchangeRate + "</ExchangeRate>";
                XMLItems += "<CessPercentage>" + item.CessPercentage + "</CessPercentage>";
                XMLItems += "<CessAmount>" + item.CessAmount + "</CessAmount>";
                XMLItems += "</Item>";
            }
            XMLItems += "</InvoiceTrans>";

            XMLAmountDetails = "<AmountDetails>";
            foreach (var item in AmountDetails)
            {
                XMLAmountDetails += "<Item>";
                XMLAmountDetails += "<Particulars>" + item.Particulars + "</Particulars>";
                XMLAmountDetails += "<Amount>" + item.Amount + "</Amount>";
                XMLAmountDetails += "<Percentage>" + item.Percentage + "</Percentage>";
                XMLAmountDetails += "<TaxableAmount>" + item.TaxableAmount + "</TaxableAmount>";
                XMLAmountDetails += "</Item>";
            }
            XMLAmountDetails += "</AmountDetails>";
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
            JSONOutputBO output;

            if (Invoice.ID == 0)
            {
                output = salesInvoiceDAL.Save(XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails);
                Invoice.InvoiceNo = output.Data.TransNo;
                Invoice.ID = output.Data.ID;
            }
            else
            {
                Invoice.ID = salesInvoiceDAL.Update(XMLInvoice, XMLItems, XMLAmountDetails, Invoice.ID, XMLPackingDetails);
            }

            if (!Invoice.IsDraft)
            {
                ReceivableBL receivableBL = new ReceivableBL();
                ReceivablesBO receivableBO = new ReceivablesBO()
                {
                    PartyID = Invoice.CustomerID,
                    TransDate = Invoice.InvoiceDate,
                    ReceivableType = "INVOICE",
                    ReferenceID = Invoice.ID,
                    DocumentNo = Invoice.InvoiceNo,
                    ReceivableAmount = Invoice.NetAmount,
                    Description = "Sales Invoice ",
                    ReceivedAmount = 0,
                    Status = "",
                    Discount = 0,
                };
                receivableBL.SaveReceivables(receivableBO);
            }

            return Invoice.ID;
        }

        public decimal GetCreditAmountByCustomer(int CustomerID)
        {
            return salesInvoiceDAL.GetCreditAmountByCustomer(CustomerID);
        }

        public int GetFreightTaxForEcommerceCustomer()
        {
            return salesInvoiceDAL.GetFreightTaxForEcommerceCustomer();
        }

        public List<SalesInvoiceBO> GetSalesInvoiceCodeAutoCompleteForReport(string CodeHint, DateTime FromDate, DateTime ToDate)
        {
            return salesInvoiceDAL.GetSalesInvoiceCodeAutoCompleteForReport(CodeHint, FromDate, ToDate);
        }

        public string GetPrintTextFile(int salesInvoiceID)
        {
            BodyLines = Convert.ToInt32(25);
            ItemLengthLimit = 30;
            PrintedLines = 0;
            string FileName;
            string FilePath;
            string URL;

            salesInvoice = new SalesInvoiceBO();
            salesInvoice = salesInvoiceDAL.GetSalesInvoice(salesInvoiceID, GeneralBO.LocationID);
            salesItemBO = salesInvoiceDAL.GetSalesInvoiceItems(salesInvoiceID, GeneralBO.LocationID);
            for (int i = 0; i < salesItemBO.Count; i++)
            {
                var salesItem = salesItemBO.Skip(i).Take(1).FirstOrDefault();
                salesItem.Name = salesItem.PrintWithItemCode ? salesItem.Name : salesItem.PartsNumber;
            }
            billingAddressBO = addressDAL.GetAddress(salesInvoice.BillingAddressID).ToList();
            shippingAddressBO = addressDAL.GetAddress(salesInvoice.ShippingAddressID).ToList();
            salesInvoice.AmountDetails = salesInvoiceDAL.GetSalesInvoiceAmountDetails(salesInvoiceID, GeneralBO.LocationID);
            string TransNo = salesInvoice.InvoiceNo;
            FileName = TransNo + ".txt";

            string path = AppDomain.CurrentDomain.BaseDirectory;

            FilePath = path + "/Outputs/SalesInvoice/" + FileName;
            TotalLines = getNoOfPrintLines(salesItemBO);
            SetPageVariables();
            PrintedLines = 0;
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintSalesInvoiceHeader(writer);
                PrintSalesInvoiceBody(writer);
                PrintSalesInvoiceFooter(writer);
            }
            URL = "/Outputs/SalesInvoice/" + FileName;
            return URL;
        }

        public int getNoOfPrintLines(List<SalesItemBO> salesItemBOList)
        {
            int count = 0;
            foreach (SalesItemBO item in salesItemBOList)
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

        private void PrintSalesInvoiceHeader(StreamWriter writer)
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
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText("TAX Invoice", PrintHelper.CharAlignment.AlignCenter, 137))));
            writer.WriteLine(PHelper.Bold(PHelper.AlignText(GeneralBO.CompanyName.ToUpper(), PrintHelper.CharAlignment.AlignCenter, 80)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address1 + "," + GeneralBO.Address2, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address3 + "," + GeneralBO.Address4, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(GeneralBO.Address5 + "," + GeneralBO.PIN + "," + GeneralBO.LandLine1, PrintHelper.CharAlignment.AlignCenter, 137)));
            writer.WriteLine(PHelper.FillChar('-', 80));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Invoice No.  : " + PHelper.Bold(salesInvoice.InvoiceNo), PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText("Vehicle No.  : " + PHelper.Bold(salesInvoice.VehicleNo), PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense("Invoice Date : " + PHelper.Bold(Convert.ToDateTime(salesInvoice.InvoiceDate).ToString("dd MMM yyyy"))));
            writer.WriteLine(PHelper.FillChar('-', 80));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Bill To  : ", PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText("Ship To : ", PrintHelper.CharAlignment.AlignLeft, 10)));
            writer.WriteLine(PHelper.Condense(PHelper.Bold(PHelper.AlignText(salesInvoice.CustomerName, PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(salesInvoice.CustomerName, PrintHelper.CharAlignment.AlignLeft, 67))));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(billingAddressBO.FirstOrDefault().AddressLine1.ToString(), PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(shippingAddressBO.FirstOrDefault().AddressLine1.ToString(), PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(billingAddressBO.FirstOrDefault().AddressLine2.ToString(), PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(shippingAddressBO.FirstOrDefault().AddressLine2.ToString(), PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText(billingAddressBO.FirstOrDefault().AddressLine3.ToString(), PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(shippingAddressBO.FirstOrDefault().AddressLine3.ToString(), PrintHelper.CharAlignment.AlignLeft, 67)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("Dist : " + billingAddressBO.FirstOrDefault().District.ToString(), PrintHelper.CharAlignment.AlignLeft, 30) + PHelper.AlignText("State : " + billingAddressBO.FirstOrDefault().State.ToString(), PrintHelper.CharAlignment.AlignLeft, 40)
                + PHelper.AlignText("Dist : " + shippingAddressBO.FirstOrDefault().District.ToString(), PrintHelper.CharAlignment.AlignLeft, 30) + PHelper.AlignText("State : " + shippingAddressBO.FirstOrDefault().State.ToString(), PrintHelper.CharAlignment.AlignLeft, 37)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("PIN  : " + billingAddressBO.FirstOrDefault().PIN, PrintHelper.CharAlignment.AlignLeft, 30) + PHelper.AlignText("Ph    : " + billingAddressBO.FirstOrDefault().MobileNo, PrintHelper.CharAlignment.AlignLeft, 40) + PHelper.AlignText("PIN  : " + shippingAddressBO.FirstOrDefault().PIN, PrintHelper.CharAlignment.AlignLeft, 30) + PHelper.AlignText("Ph    : " + shippingAddressBO.FirstOrDefault().MobileNo, PrintHelper.CharAlignment.AlignLeft, 37)));
            writer.WriteLine(PHelper.Condense(PHelper.AlignText("GST  : " + billingAddressBO.FirstOrDefault().CustomerGSTNo, PrintHelper.CharAlignment.AlignLeft, 70) + PHelper.AlignText(shippingAddressBO.FirstOrDefault().CustomerGSTNo, PrintHelper.CharAlignment.AlignLeft, 67)));
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

        private void PrintSalesInvoiceBody(StreamWriter writer)
        {
            int FooterLines = 3;
            foreach (var items in salesItemBO.Select((value, i) => new { i = (i + 1), value }))
            {
                string ItemLineTemplate = "{0,-3}{1,-33}{2,-6}{3,-9}{4,-6}{5,10}{6,9}{7,7}{8,9}{9,9}{10,9}{11,9}{12,10}{13,8}";
                if (PrintedLines == BodyLines)
                {
                    PrintSalesInvoiceFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintSalesInvoiceHeader(writer);
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
                SubTotalDiscAmt += items.value.DiscountAmount;
                SubTotalAddDiscAmt += items.value.AdditionalDiscount;
                SubTotalTODAmt += items.value.TurnoverDiscount;
                SubTotalTaxableAmt += items.value.TaxableAmount;
                if (items.value.GSTPercentage == 5)
                {
                    CessPer5Amt += items.value.CessAmount;
                    TaxableValue5PerAmt += items.value.TaxableAmount;
                }
                else if (items.value.GSTPercentage == 12)
                {
                    CessPer12Amt += items.value.CessAmount;
                    TaxableValue12PerAmt += items.value.TaxableAmount;
                }
                else if (items.value.GSTPercentage == 18)
                {
                    CessPer18Amt += items.value.CessAmount;
                    TaxableValue18PerAmt += items.value.TaxableAmount;
                }
                ItemLengthLimit = 30;
                string name = PHelper.SplitString(items.value.Name, ItemLengthLimit);
                string[] itemname = name.Split(new char[] { '\r' }, 2);
                if (itemname[1] != " " && (PrintedLines == BodyLines - 1))
                {
                    writer.WriteLine();
                    PrintSalesInvoiceFooter(writer);
                    ++CurrentPages;
                    for (int index = 1; index <= FooterLines; ++index)
                        writer.WriteLine(PHelper.LineSpacing1_6());
                    PrintSalesInvoiceHeader(writer);
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
                 items.value.UnitName,
                 items.value.BatchName,
                 Convert.ToDateTime(items.value.ExpiryDate).ToString("MMM y"),
                 string.Format("{0:0.00}", items.value.MRP),
                 string.Format("{0:0.00}", items.value.BasicPrice),
                 string.Format("{0:0.00}", items.value.InvoiceQty),
                 string.Format("{0:0.00}", items.value.GrossAmount),
                 string.Format("{0:0.00}", items.value.DiscountAmount),
                 string.Format("{0:0.00}", items.value.AdditionalDiscount),
                 string.Format("{0:0.00}", items.value.TurnoverDiscount),
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

        private void PrintSalesInvoiceFooter(StreamWriter writer)
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
                foreach (var items in salesInvoice.AmountDetails.Select((value, i) => new { i = (i + 1), value }))
                {
                    if (items.value.Particulars == "SGST" && items.value.Percentage == Convert.ToDecimal(2.50) && items.value.Amount > 0)
                    {
                        SGST5Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "SGST" && items.value.Percentage == Convert.ToDecimal(6) && items.value.Amount > 0)
                    {
                        SGST6Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "SGST" && items.value.Percentage == Convert.ToDecimal(9) && items.value.Amount > 0)
                    {
                        SGST9Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "CGST" && items.value.Percentage == Convert.ToDecimal(2.50) && items.value.Amount > 0)
                    {
                        CGST5Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "CGST" && items.value.Percentage == Convert.ToDecimal(6) && items.value.Amount > 0)
                    {
                        CGST6Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "CGST" && items.value.Percentage == Convert.ToDecimal(9) && items.value.Amount > 0)
                    {
                        CGST9Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "IGST" && items.value.Percentage == Convert.ToDecimal(5) && items.value.Amount > 0)
                    {
                        IGST5Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "IGST" && items.value.Percentage == Convert.ToDecimal(12) && items.value.Amount > 0)
                    {
                        IGST12Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "IGST" && items.value.Percentage == Convert.ToDecimal(18) && items.value.Amount > 0)
                    {
                        IGST18Amt = items.value.Amount;
                    }
                    if (items.value.Particulars == "Cess")
                    {
                        CessPer = items.value.Percentage;
                        CessAmt = items.value.Amount;
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
                string.Format("{0:0.00}", salesInvoice.DiscountAmount),
                string.Format("{0:0.00}", salesInvoice.AdditionalDiscount),
                string.Format("{0:0.00}", salesInvoice.TurnoverDiscount),
                string.Format("{0:0.00}", salesInvoice.TaxableAmount),
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
                    string.Format("{0:0.00}", salesInvoice.TaxableAmount));
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
                    + PHelper.AlignText("Less Cash Discount/TOD    :  ", PrintHelper.CharAlignment.AlignRight, 40)
                    + PHelper.Bold(PHelper.AlignText(string.Format("{0:0.00}", salesInvoice.TurnoverDiscount + salesInvoice.CashDiscount), PrintHelper.CharAlignment.AlignRight, 12))));
                writer.WriteLine(PHelper.Condense(PHelper.AlignText("", PrintHelper.CharAlignment.AlignLeft, 85)
                    + PHelper.AlignText("Grand Total(Rounded off : " + string.Format("{0:0.00}", salesInvoice.RoundOff) + ")  :  ", PrintHelper.CharAlignment.AlignRight, 40)
                    + PHelper.Bold(PHelper.AlignText(string.Format("{0:0.00}", salesInvoice.NetAmount), PrintHelper.CharAlignment.AlignRight, 12))));
                writer.WriteLine(PHelper.Condense("Total       :  Rupees " + PHelper.Bold(generalBL.NumberToText(Convert.ToInt32(salesInvoice.NetAmount)) + "Only")));
                //writer.WriteLine(PHelper.Condense(PHelper.AlignText("Taxable Amt :", PrintHelper.CharAlignment.AlignLeft, 15)
                //    + PHelper.Bold(PHelper.AlignText(string.Format("{0:0.00}", salesInvoice.TaxableAmount), PrintHelper.CharAlignment.AlignLeft, 10))));
                writer.WriteLine(PHelper.FillChar('-', 80));
                writer.WriteLine(PHelper.Condense("HSN 5% Items 30039011  HSN 12% Items 30039011 HSN 18% Items 34011110"));
                writer.WriteLine(PHelper.Condense(PHelper.Bold("Total Outstanding Balance   : " + (salesInvoice.OutstandingAmount))));
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