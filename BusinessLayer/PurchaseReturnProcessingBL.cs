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
   public class PurchaseReturnProcessingBL: IPurchaseReturnProcessingContract
    {
        PurchaseReturnProcessingDAL ReturnProcessingDAL;
        public  PurchaseReturnProcessingBL()
        {
            ReturnProcessingDAL = new PurchaseReturnProcessingDAL();
        }
        public List<PurchaseReturnProcessingItemBO> GetPurchaseReturnProcessingItem(string ProcessingType, DateTime FromTransactionDate, DateTime ToTransactionDate, DateTime AsOnDate,int Days)
        {
            return ReturnProcessingDAL.GetPurchaseReturnProcessingItem( ProcessingType, FromTransactionDate, ToTransactionDate, AsOnDate,Days);
        }
        public int Save(List<PurchaseReturnProcessingItemBO> Items)
        {
            string XMLPurchaseReturnProcessingItems;
            XMLPurchaseReturnProcessingItems = "<PurchaseReturnProcessingItems>";
            foreach (var item in Items)
            {
                XMLPurchaseReturnProcessingItems += "<Item>";
                XMLPurchaseReturnProcessingItems += "<ItemID>" + item.ItemID + "</ItemID>";
                XMLPurchaseReturnProcessingItems += "<SupplierID>" + item.SupplierID + "</SupplierID>";
                XMLPurchaseReturnProcessingItems += "<BatchID>" + item.BatchID + "</BatchID>";
                XMLPurchaseReturnProcessingItems += "<InvoiceID>" + item.InvoiceID + "</InvoiceID>";
                XMLPurchaseReturnProcessingItems += "<InvoiceTransID>" + item.InvoiceTransID + "</InvoiceTransID>";
                XMLPurchaseReturnProcessingItems += "<UnitID>" + item.UnitID + "</UnitID>";
                XMLPurchaseReturnProcessingItems += "<ReturnQty>" + item.ReturnQty + "</ReturnQty>";
                XMLPurchaseReturnProcessingItems += "<PurchaseRate>" + item.PurchaseRate + "</PurchaseRate>";
                XMLPurchaseReturnProcessingItems += "<IGSTPercentage>" + item.IGSTPercentage + "</IGSTPercentage>";
                XMLPurchaseReturnProcessingItems += "<SGSTPercentage>" + item.SGSTPercentage + "</SGSTPercentage>";
                XMLPurchaseReturnProcessingItems += "<CGSTPercentage>" + item.CGSTPercentage + "</CGSTPercentage>";
                XMLPurchaseReturnProcessingItems += "<IGSTAmount>" + item.IGSTAmount + "</IGSTAmount>";
                XMLPurchaseReturnProcessingItems += "<SGSTAmount>" + item.SGSTAmount + "</SGSTAmount>";
                XMLPurchaseReturnProcessingItems += "<CGSTAmount>" + item.CGSTAmount + "</CGSTAmount>";
                XMLPurchaseReturnProcessingItems += "<Value>" + item.Value + "</Value>";
                XMLPurchaseReturnProcessingItems += "</Item>";
            }
            XMLPurchaseReturnProcessingItems += "</PurchaseReturnProcessingItems>";
           return ReturnProcessingDAL.Save(XMLPurchaseReturnProcessingItems);
            
        }
    }
}
