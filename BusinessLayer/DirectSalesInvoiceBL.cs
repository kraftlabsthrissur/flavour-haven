using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class DirectSalesInvoiceBL : IDirectSalesInvoiceContract
    {
        DirectSalesInvoiceDAL directSalesInvoiceDAL;

        public DirectSalesInvoiceBL()
        {
            directSalesInvoiceDAL = new DirectSalesInvoiceDAL();
        }

        public List<SalesInvoiceBO> GetSalesType()
        {
            return directSalesInvoiceDAL.GetSalesType();
        }

        public List<SalesInvoiceBO> GetPatientTypeList()
        {
            return directSalesInvoiceDAL.GetPatientTypeList();
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
            XMLInvoice += "<TurnoverDiscount>" + Invoice.TurnoverDiscount + "</TurnoverDiscount>";
            XMLInvoice += "<AdditionalDiscount>" + Invoice.AdditionalDiscount + "</AdditionalDiscount>";
            XMLInvoice += "<TaxableAmount>" + Invoice.TaxableAmount + "</TaxableAmount>";
            XMLInvoice += "<CGSTAmount>" + Invoice.CGSTAmount + "</CGSTAmount>";
            XMLInvoice += "<SGSTAmount>" + Invoice.SGSTAmount + "</SGSTAmount>";
            XMLInvoice += "<IGSTAmount>" + Invoice.IGSTAmount + "</IGSTAmount>";
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
            XMLInvoice += "<CreatedUserID>" + GeneralBO.CreatedUserID + "</CreatedUserID>";
            XMLInvoice += "<FinYear>" + GeneralBO.FinYear + "</FinYear>";
            XMLInvoice += "<LocationID>" + GeneralBO.LocationID + "</LocationID>";
            XMLInvoice += "<ApplicationID>" + GeneralBO.ApplicationID + "</ApplicationID>";
            XMLInvoice += "<CessAmount>" + Invoice.CessAmount + "</CessAmount>";
            XMLInvoice += "<Remarks>" + Invoice.Remarks + "</Remarks>";
            XMLInvoice += "<CustomerName>" + Invoice.CustomerName + "</CustomerName>";
            XMLInvoice += "<IsDirectSalesInvoice>" + Invoice.IsDirectSalesInvoice + "</IsDirectSalesInvoice>";
            XMLInvoice += "<DoctorID>" + Invoice.DoctorID + "</DoctorID>";
            XMLInvoice += "<BankID>" + Invoice.BankID + "</BankID>";
            XMLInvoice += "<WarehouseID>" + Invoice.WarehouseID + "</WarehouseID>";
            XMLInvoice += "<DiscountCategoryID>" + Invoice.DiscountCategoryID + "</DiscountCategoryID>";
            XMLInvoice += "<IPID>" + Invoice.IPID + "</IPID>";
            XMLInvoice += "<OPID>" + Invoice.OPID + "</OPID>";
            XMLInvoice += "<PatientType>" + Invoice.PatientType + "</PatientType>";
            XMLInvoice += "</Invoice>";
            XMLInvoice += "</Invoices>";

            XMLItems = "<InvoiceTrans>";
            int i = 0;
            foreach (var item in Items)
            {
                i++;
                XMLItems += "<Item>";
                XMLItems += "<ItemID>" + item.ItemID + "</ItemID>";
                XMLItems += "<BatchID>" + item.BatchID + "</BatchID>";
                XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<StoreID>" + item.StoreID + "</StoreID>";
                XMLItems += "<SalesOrderTransID>" + item.SalesOrderItemID + "</SalesOrderTransID>";
                XMLItems += "<ProformaInvoiceTransID>" + item.ProformaInvoiceTransID + "</ProformaInvoiceTransID>";
                XMLItems += "<MRP>" + item.MRP + "</MRP>";
                XMLItems += "<BasicPrice>" + item.BasicPrice + "</BasicPrice>";
                XMLItems += "<Quantity>" + item.Qty + "</Quantity>";
                XMLItems += "<OfferQty>" + item.OfferQty + "</OfferQty>";
                XMLItems += "<InvoiceQty>" + item.InvoiceQty + "</InvoiceQty>";
                XMLItems += "<InvoiceOfferQty>" + item.InvoiceOfferQty + "</InvoiceOfferQty>";
                XMLItems += "<GrossAmount>" + item.GrossAmount + "</GrossAmount>";
                XMLItems += "<DiscountPercentage>" + item.DiscountPercentage + "</DiscountPercentage>";
                XMLItems += "<DiscountAmount>" + item.DiscountAmount + "</DiscountAmount>";
                XMLItems += "<TurnoverDiscount>" + item.TurnoverDiscount + "</TurnoverDiscount>";
                XMLItems += "<AdditionalDiscount>" + item.AdditionalDiscount + "</AdditionalDiscount>";
                XMLItems += "<TaxableAmount>" + item.TaxableAmount + "</TaxableAmount>";
                XMLItems += "<SGSTPercentage>" + item.SGSTPercentage + "</SGSTPercentage>";
                XMLItems += "<CGSTPercentage>" + item.CGSTPercentage + "</CGSTPercentage>";
                XMLItems += "<IGSTPercentage>" + item.IGSTPercentage + "</IGSTPercentage>";
                XMLItems += "<SGSTAmt>" + item.SGST + "</SGSTAmt>";
                XMLItems += "<CGSTAmt>" + item.CGST + "</CGSTAmt>";
                XMLItems += "<IGSTAmt>" + item.IGST + "</IGSTAmt>";
                XMLItems += "<CashDiscount>" + item.CashDiscount + "</CashDiscount>";
                XMLItems += "<NetAmt>" + item.NetAmount + "</NetAmt>";
                XMLItems += "<SortOrder>" + i + "</SortOrder>";
                XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLItems += "<CessPercentage>" + item.CessPercentage + "</CessPercentage>";
                XMLItems += "<CessAmount>" + item.CessAmount + "</CessAmount>";
                XMLItems += "<Form>" + item.Form + "</Form>";
                XMLItems += "<MedicineIssueID>" + item.MedicineIssueID + "</MedicineIssueID>";
                XMLItems += "<MedicineIssueTransID>" + item.MedicineIssueTransID + "</MedicineIssueTransID>";
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
            if (PackingDetails != null)
            {
                foreach (var item in PackingDetails)
                {
                    XMLPackingDetails += "<Item>";
                    XMLPackingDetails += "<PackSize>" + item.PackSize + "</PackSize>";
                    XMLPackingDetails += "<Quantity>" + item.Quantity + "</Quantity>";
                    XMLPackingDetails += "<UnitID>" + item.UnitID + "</UnitID>";
                    XMLPackingDetails += "</Item>";
                }
            }
            XMLPackingDetails += "</PackingDetails>";
            JSONOutputBO output;

            if (Invoice.ID == 0)
            {
                output = directSalesInvoiceDAL.Save(XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails, Invoice);
                Invoice.InvoiceNo = output.Data.TransNo;
                Invoice.ID = output.Data.ID;
            }
            else
            {
                Invoice.ID = directSalesInvoiceDAL.Update(XMLInvoice, XMLItems, XMLAmountDetails, Invoice.ID, XMLPackingDetails, Invoice);
            }
            return Invoice.ID;
        }

        public DatatableResultBO GetDirectSalesInvoiceList(string Type, string CodeHint, string DateHint, string SalesTypeHint, string CustomerNameHint, string LocationHint,string DoctorHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return directSalesInvoiceDAL.GetDirectSalesInvoiceList(Type, CodeHint, DateHint, SalesTypeHint, CustomerNameHint, LocationHint, DoctorHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }

        public SalesInvoiceBO GetDirectSalesInvoice(int SalesInvoiceID, int LocationID)
        {
            return directSalesInvoiceDAL.GetDirectSalesInvoice(SalesInvoiceID, LocationID);
        }

        public List<SalesItemBO> GetDirectSalesInvoiceItems(int SalesInvoiceID, int LocationID)
        {
            List<SalesItemBO> Items = directSalesInvoiceDAL.GetDirectSalesInvoiceItems(SalesInvoiceID, LocationID);

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

        public List<SalesItemBO> GetMedicineIssueItemsForDirectSalesInvoice(string MedicineIssueType, int MedicineIssuedToID)
        {
            List<SalesItemBO> Items = directSalesInvoiceDAL.GetMedicineIssueItemsForDirectSalesInvoice(MedicineIssueType, MedicineIssuedToID);

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

        public List<SalesAmountBO> GetDirectSalesInvoiceAmountDetails(int SalesInvoiceID, int LocationID)
        {
            return directSalesInvoiceDAL.GetDirectSalesInvoiceAmountDetails(SalesInvoiceID, LocationID);
        }

        public List<SalesPackingDetailsBO> GetDirectSalesInvoicePackingDetails(int SalesInvoiceID, int LocationID)
        {
            return directSalesInvoiceDAL.GetDirectSalesInvoicePackingDetails(SalesInvoiceID, LocationID);
        }

        public int SaveV3(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails, List<SalesPackingDetailsBO> PackingDetails)
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
            XMLInvoice += "<TurnoverDiscount>" + Invoice.TurnoverDiscount + "</TurnoverDiscount>";
            XMLInvoice += "<AdditionalDiscount>" + Invoice.AdditionalDiscount + "</AdditionalDiscount>";
            XMLInvoice += "<TaxableAmount>" + Invoice.TaxableAmount + "</TaxableAmount>";
            XMLInvoice += "<CGSTAmount>" + Invoice.CGSTAmount + "</CGSTAmount>";
            XMLInvoice += "<SGSTAmount>" + Invoice.SGSTAmount + "</SGSTAmount>";
            XMLInvoice += "<IGSTAmount>" + Invoice.IGSTAmount + "</IGSTAmount>";
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
            XMLInvoice += "<CreatedUserID>" + GeneralBO.CreatedUserID + "</CreatedUserID>";
            XMLInvoice += "<FinYear>" + GeneralBO.FinYear + "</FinYear>";
            XMLInvoice += "<LocationID>" + GeneralBO.LocationID + "</LocationID>";
            XMLInvoice += "<ApplicationID>" + GeneralBO.ApplicationID + "</ApplicationID>";
            XMLInvoice += "<CessAmount>" + Invoice.CessAmount + "</CessAmount>";
            XMLInvoice += "<Remarks>" + Invoice.Remarks + "</Remarks>";
            XMLInvoice += "<CustomerName>" + Invoice.CustomerName + "</CustomerName>";
            XMLInvoice += "<IsDirectSalesInvoice>" + Invoice.IsDirectSalesInvoice + "</IsDirectSalesInvoice>";
            XMLInvoice += "<DoctorID>" + Invoice.DoctorID + "</DoctorID>";
            XMLInvoice += "<BankID>" + Invoice.BankID + "</BankID>";
            XMLInvoice += "<WarehouseID>" + Invoice.WarehouseID + "</WarehouseID>";
            XMLInvoice += "<DiscountCategoryID>" + Invoice.DiscountCategoryID + "</DiscountCategoryID>";
            XMLInvoice += "<IPID>" + Invoice.IPID + "</IPID>";
            XMLInvoice += "<OPID>" + Invoice.OPID + "</OPID>";
            XMLInvoice += "<PatientType>" + Invoice.PatientType + "</PatientType>";
            XMLInvoice += "</Invoice>";
            XMLInvoice += "</Invoices>";

            XMLItems = "<InvoiceTrans>";
            int i = 0;
            foreach (var item in Items)
            {
                i++;
                XMLItems += "<Item>";
                XMLItems += "<ItemID>" + item.ItemID + "</ItemID>";
                XMLItems += "<BatchID>" + item.BatchID + "</BatchID>";
                XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<StoreID>" + item.StoreID + "</StoreID>";
                XMLItems += "<SalesOrderTransID>" + item.SalesOrderItemID + "</SalesOrderTransID>";
                XMLItems += "<ProformaInvoiceTransID>" + item.ProformaInvoiceTransID + "</ProformaInvoiceTransID>";
                XMLItems += "<MRP>" + item.MRP + "</MRP>";
                XMLItems += "<BasicPrice>" + item.BasicPrice + "</BasicPrice>";
                XMLItems += "<Quantity>" + item.Qty + "</Quantity>";
                XMLItems += "<OfferQty>" + item.OfferQty + "</OfferQty>";
                XMLItems += "<InvoiceQty>" + item.InvoiceQty + "</InvoiceQty>";
                XMLItems += "<InvoiceOfferQty>" + item.InvoiceOfferQty + "</InvoiceOfferQty>";
                XMLItems += "<GrossAmount>" + item.GrossAmount + "</GrossAmount>";
                XMLItems += "<DiscountPercentage>" + item.DiscountPercentage + "</DiscountPercentage>";
                XMLItems += "<DiscountAmount>" + item.DiscountAmount + "</DiscountAmount>";
                XMLItems += "<TurnoverDiscount>" + item.TurnoverDiscount + "</TurnoverDiscount>";
                XMLItems += "<AdditionalDiscount>" + item.AdditionalDiscount + "</AdditionalDiscount>";
                XMLItems += "<TaxableAmount>" + item.TaxableAmount + "</TaxableAmount>";
                XMLItems += "<SGSTPercentage>" + item.SGSTPercentage + "</SGSTPercentage>";
                XMLItems += "<CGSTPercentage>" + item.CGSTPercentage + "</CGSTPercentage>";
                XMLItems += "<IGSTPercentage>" + item.IGSTPercentage + "</IGSTPercentage>";
                XMLItems += "<SGSTAmt>" + item.SGST + "</SGSTAmt>";
                XMLItems += "<CGSTAmt>" + item.CGST + "</CGSTAmt>";
                XMLItems += "<IGSTAmt>" + item.IGST + "</IGSTAmt>";
                XMLItems += "<CashDiscount>" + item.CashDiscount + "</CashDiscount>";
                XMLItems += "<NetAmt>" + item.NetAmount + "</NetAmt>";
                XMLItems += "<SortOrder>" + i + "</SortOrder>";
                XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLItems += "<CessPercentage>" + item.CessPercentage + "</CessPercentage>";
                XMLItems += "<CessAmount>" + item.CessAmount + "</CessAmount>";
                XMLItems += "<Form>" + item.Form + "</Form>";
                XMLItems += "<MedicineIssueID>" + item.MedicineIssueID + "</MedicineIssueID>";
                XMLItems += "<MedicineIssueTransID>" + item.MedicineIssueTransID + "</MedicineIssueTransID>";
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
                output = directSalesInvoiceDAL.SaveV3(XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails, Invoice);
                Invoice.InvoiceNo = output.Data.TransNo;
                Invoice.ID = output.Data.ID;
            }
            else
            {
                Invoice.ID = directSalesInvoiceDAL.Update(XMLInvoice, XMLItems, XMLAmountDetails, Invoice.ID, XMLPackingDetails, Invoice);
            }

            //    if (!Invoice.IsDraft && Invoice.SalesType == "Cash Sale")
            //    {
            //        ReceivableBL receivableBL = new ReceivableBL();
            //        ReceivablesBO receivableBO = new ReceivablesBO()
            //        {
            //            PartyID = Invoice.CustomerID,
            //            TransDate = Invoice.InvoiceDate,
            //            ReceivableType = "INVOICE",
            //            ReferenceID = Invoice.ID,
            //            DocumentNo = Invoice.InvoiceNo,
            //            ReceivableAmount = Invoice.NetAmount,
            //            Description = "Sales Invoice ",
            //            ReceivedAmount = 0,
            //            Status = "",
            //            Discount = 0,
            //        };
            //        receivableBL.SaveReceivables(receivableBO);
            //    }

            return Invoice.ID;
        //}
    }
    }
}
