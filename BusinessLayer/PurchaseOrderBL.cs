using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BusinessLayer
{
    public class PurchaseOrderBL : IPurchaseOrder
    {
        PurchaseOrderRepository PODAL;

        public PurchaseOrderBL()
        {
            PODAL = new PurchaseOrderRepository();
        }

        public bool CheckInvoiceNumberValid(int supplierID, string invoiceNo)
        {
            return PODAL.CheckInvoiceNumberValid(supplierID, invoiceNo);
        }

        public PurchaseOrderBO GetPurchaseOrder(int ID)
        {
            return PODAL.GetPurchaseOrder(ID);
        }

        public List<PurchaseOrderTransBO> GetPurchaseOrderItems(int ID)
        {
            return PODAL.GetPurchaseOrderItems(ID);
        }

        public bool IsPOCancellable(int POID)
        {
            return PODAL.IsPOCancellable(POID);
        }

        public bool CancelPurchaseOrder(int PurchaseOrderID)
        {
            return PODAL.CancelPurchaseOrder(PurchaseOrderID);
        }

        public decimal GetRateForInterCompany(int ItemID, string BatchType)
        {
            return PODAL.GetRateForInterCompany(ItemID, BatchType);
        }
        public JSONOutputBO SavePurchaseOrder(PurchaseOrderBO PO, List<PurchaseOrderTransBO> Items)
        {

            var _poTrans = Items.Select(x =>
            {
                x.Purchased = false;
                if (PO.InclusiveGST && !PO.IsDraft && PO.IsGSTRegistred)
                {
                    x.Rate = x.Rate * 100 / (100 + x.IGSTPercent);
                    x.Amount = x.Rate * x.Quantity;
                }
                return x;
            }).ToList();

            //string XMLItems = XMLHelper.Serialize(_poTrans);
            string XMLItems = "<ArrayOfPurchaseOrderTransBO>";
            foreach (var item in _poTrans)
            {
                var ExpDate = FormatDate(item.ExpDate);
                XMLItems += "<PurchaseOrderTransBO>";
                XMLItems += "<PurchaseOrderID>0</PurchaseOrderID>";
                XMLItems += "<PurchaseReqID>0</PurchaseReqID>";
                XMLItems += "<PRTransID>0</PRTransID>";
                XMLItems += "<ItemID>" + item.ItemID + "</ItemID>";
                XMLItems += "<Quantity>" + item.Quantity + "</Quantity>";
                XMLItems += "<Rate>" + item.Rate + "</Rate>";
                XMLItems += "<OfferQty>" + item.OfferQty + "</OfferQty>";
                XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLItems += "<MRP>" + item.MRP + "</MRP>";
                XMLItems += "<RetailMRP>" + item.RetailMRP + "</RetailMRP>";
                XMLItems += "<RetailRate>" + item.RetailRate + "</RetailRate>";
                XMLItems += "<Amount>" + item.Amount + "</Amount>";
                XMLItems += "<SGSTPercent>" + item.SGSTPercent + "</SGSTPercent>";
                XMLItems += "<CGSTPercent>" + item.CGSTPercent + "</CGSTPercent>";
                XMLItems += "<IGSTPercent>" + item.IGSTPercent + "</IGSTPercent>";
                XMLItems += "<VATPercentage>" + (item.VATPercentage.HasValue ? item.VATPercentage : 0) + "</VATPercentage>";
                XMLItems += "<GSTPercentage>" + item.GSTPercentage + "</GSTPercentage>";
                XMLItems += "<SGSTAmt>" + item.SGSTAmt + "</SGSTAmt>";
                XMLItems += "<CGSTAmt>" + item.CGSTAmt + "</CGSTAmt>";
                XMLItems += "<IGSTAmt>" + item.IGSTAmt + "</IGSTAmt>";
                XMLItems += "<VATAmount>" + (item.VATAmount.HasValue ? item.VATAmount.Value : 0) + "</VATAmount>";
                XMLItems += "<GrossAmount>" + item.GrossAmount + "</GrossAmount>";
                XMLItems += "<DiscountPercent>" + item.DiscountPercent + "</DiscountPercent>";
                XMLItems += "<Discount>" + item.Discount + "</Discount>";
                XMLItems += "<TaxableAmount>" + item.TaxableAmount + "</TaxableAmount>";
                XMLItems += "<NetAmount>" + item.NetAmount + "</NetAmount>";
                XMLItems += "<LastPurchaseRate>" + item.LastPurchaseRate + "</LastPurchaseRate>";
                XMLItems += "<LowestPR>" + item.LowestPR + "</LowestPR>";
                XMLItems += "<Purchased>" + item.Purchased + "</Purchased>";
                XMLItems += "<PurchaseRequisitionID>" + item.PurchaseRequisitionID + "</PurchaseRequisitionID>";
                XMLItems += "<PurchaseRequisitionTrasID>" + item.PurchaseRequisitionTrasID + "</PurchaseRequisitionTrasID>";
                XMLItems += "<QtyMet>" + item.QtyMet + "</QtyMet>";
                XMLItems += "<QtyInQC>" + item.QtyInQC + "</QtyInQC>";
                XMLItems += "<QtyAvailable>" + item.QtyAvailable + "</QtyAvailable>";
                XMLItems += "<QtyOrdered>" + item.QtyOrdered + "</QtyOrdered>";
                XMLItems += "<FinYear>" + item.FinYear + "</FinYear>";
                XMLItems += "<LocationID>" + item.LocationID + "</LocationID>";
                XMLItems += "<ApplicationID>1</ApplicationID>";
                XMLItems += "<QtyTolerancePercent>" + item.QtyTolerancePercent + "</QtyTolerancePercent>";
                XMLItems += "<IsQCRequired>" + item.IsQCRequired + "</IsQCRequired>";
                XMLItems += "<PendingPOQty>" + item.PendingPOQty + "</PendingPOQty>";
                XMLItems += "<ItemCategoryID>" + item.ItemCategoryID + "</ItemCategoryID>";
                XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<FGCategoryID>" + item.FGCategoryID + "</FGCategoryID>";
                XMLItems += "<TravelDate>" + item.TravelDate + "</TravelDate>";
                XMLItems += "<IsSuspended>" + item.IsSuspended + "</IsSuspended>";
                XMLItems += "<ExpDate>" + ExpDate + "</ExpDate>";
                XMLItems += "<PackSize>" + item.PackSize + "</PackSize>";
                XMLItems += "<IsGST>" + item.IsGST + "</IsGST>";
                XMLItems += "<IsVat>" + item.IsVat + "</IsVat>";
                XMLItems += "<CurrencyID>" + item.CurrencyID + "</CurrencyID>";
                XMLItems += "<ItemCode>" + item.ItemCode + "</ItemCode>";
                XMLItems += "<ItemName></ItemName>";
                XMLItems += "<PartsNumber>" + item.PartsNumber + "</PartsNumber>";
                XMLItems += "<Remark>" + item.Remark + "</Remark>";
                XMLItems += "<Model>" + item.Model + "</Model>";
                XMLItems += "<ExchangeRate>" + item.ExchangeRate + "</ExchangeRate>";
                XMLItems += "<SecondaryUnit>" + item.SecondaryUnit + "</SecondaryUnit>";
                XMLItems += "<SecondaryUnitSize>" + item.SecondaryUnitSize + "</SecondaryUnitSize>";
                XMLItems += "<SecondaryQty>" + item.SecondaryQty + "</SecondaryQty>";
                XMLItems += "<SecondaryRate>" + item.SecondaryRate + "</SecondaryRate>";
                XMLItems += "</PurchaseOrderTransBO>";
            }
            XMLItems += "</ArrayOfPurchaseOrderTransBO>";

            JSONOutputBO output;
            if (PO.ID == 0)
            {
                output = PODAL.SavePurchaseOrder(PO, XMLItems);
            }
            else
            {
                output = PODAL.UpdatePurchaseOrder(PO, XMLItems);
            }

            PO.ID = output.Data.ID;
            if (!PO.IsDraft)
            {
                ProcessInterCompanySalesOrder(PO, Items);

            }

            return output;
        }

        public void ProcessInterCompanySalesOrder(PurchaseOrderBO PO, List<PurchaseOrderTransBO> Items)
        {
            SupplierBL SupplierBL = new SupplierBL();
            SalesOrderBL SalesOrderBL = new SalesOrderBL();
            SalesOrderBO SalesOrder;
            ItemBL ItemBL = new ItemBL();
            AddressBL AddressBL = new AddressBL();
            List<SalesItemBO> SalesItems;

            int TempLocationID = GeneralBO.LocationID;
            int? LocationID;

            if (!SupplierBL.IsInterCompanySupplier(PO.SupplierID))
            {
                return;
            }

            try
            {
                SalesOrder = new SalesOrderBO()
                {
                    CustomerID = SupplierBL.GetCustomerID(),
                    Source = "Purchase Order",
                    PurchaseOrderID = PO.ID,
                    ShippingAddressID = (int)PO.ShippingAddressID,
                    BillingAddressID = (int)PO.BillingAddressID,
                    IsApproved = true,
                };

                //LocationID = SupplierBL.GetSupplierLocationID(PO.SupplierID);
                HttpContext.Current.Session["CurrentLocationID"] = PO.SalesOrderLocationID;


                SalesItems = Items.GroupBy(g => new { g.ItemID, g.Name, g.SGSTPercent, g.CGSTPercent, g.IGSTPercent, g.UnitID, g.BatchTypeID, g.SGSTAmt, g.CGSTAmt, g.IGSTAmt })
                    .Select(a => new SalesItemBO()
                    {
                        ItemID = a.Key.ItemID,
                        Name = a.Key.Name,
                        BatchTypeID = a.Key.BatchTypeID,
                        Qty = (decimal)a.Sum(b => b.Quantity),
                        SGSTPercentage = (decimal)a.Key.SGSTPercent,
                        CGSTPercentage = (decimal)a.Key.CGSTPercent,
                        IGSTPercentage = (decimal)a.Key.IGSTPercent,
                        ItemCategoryID = ItemBL.GetItemDetails(a.Key.ItemID).CategoryID,
                        SGST = (decimal)a.Key.SGSTAmt,
                        CGST = (decimal)a.Key.CGSTAmt,
                        IGST = (decimal)a.Key.IGSTAmt,
                        UnitID = a.Key.UnitID
                    }).ToList();

                SalesOrderBL.ProcessOrder(SalesOrder, SalesItems);

                HttpContext.Current.Session["CurrentLocationID"] = TempLocationID;
            }
            catch (Exception e)
            {
                HttpContext.Current.Session["CurrentLocationID"] = TempLocationID;
                throw e;
            }
        }

        public List<IsItemSuppliedBySupplier> IsItemSuppliedBySupplier(string ItemLists, int SupplierID)
        {
            return PODAL.IsItemSuppliedBySupplier(ItemLists, SupplierID);
        }

        public List<PurchaseOrderItemBO> GetUnProcessedPurchaseRequisitionTransForPO(int PurchaseRequisitionID, int SupplierID)
        {
            return PODAL.GetUnProcessedPurchaseRequisitionTransForPO(PurchaseRequisitionID, SupplierID);
        }

        public List<RequisitionBO> GetUnProcessedPurchaseRequisitionForPO()
        {
            return PODAL.GetUnProcessedPurchaseRequisitionForPO();
        }

        public int SuspendPurchaseOrder(int ID, string Table)
        {
            return PODAL.SuspendPurchaseOrder(ID, Table);
        }
        public int SuspendPurchaseOrderItem(int ID)
        {
            return PODAL.SuspendPurchaseOrderItem(ID);
        }
        public DatatableResultBO GetPurchaseOrderList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string ItemNameHint, string CategoryNameHint, string NetAmtHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return PODAL.GetPurchaseOrderList(Type, TransNoHint, TransDateHint, SupplierNameHint, ItemNameHint, CategoryNameHint, NetAmtHint, SortField, SortOrder, Offset, Limit);
        }
        private string FormatDate(DateTime date)
        {
            string DateFormatForEdit = "dd-MM-yyyy";
            return date.ToString(DateFormatForEdit);
        }
        public List<PurchaseOrderBO> GetOrderTypeList()
        {
            return PODAL.GetOrderTypeList();
        }
    }
}
