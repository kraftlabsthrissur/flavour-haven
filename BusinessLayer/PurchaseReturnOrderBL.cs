using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class PurchaseReturnOrderBL : IPurchaseReturnOrderContract
    {
        PurchaseReturnOrderDAL PurchaseReturnDAL;
        public PurchaseReturnOrderBL()
        {
            PurchaseReturnDAL = new PurchaseReturnOrderDAL();
        }
        public bool UpdatePurchaseOrderReturn(PurchaseReturnBO purchaseReturnBO, List<GRNTransItemBO> grnTransItems)

        {

            return PurchaseReturnDAL.UpdatePurchaseOrderReturn(purchaseReturnBO, grnTransItems);
        }
           
        public string SavePurchaseReturn(PurchaseReturnBO purchaseReturnBO, List<GRNTransItemBO> grnTransItems)

        {
            return PurchaseReturnDAL.SavePurchaseReturn(purchaseReturnBO, grnTransItems);

        }
        public List<PurchaseReturnBO> GetPurchaseReturnOrderList(int SupplierID)
        {
            return PurchaseReturnDAL.GetPurchaseReturnOrderList(SupplierID);
        }
        public List<PurchaseReturnTransItemBO> GetPurchaseReturnOrder(int orderid)
        {
            return PurchaseReturnDAL.GetPurchaseReturnOrder(orderid);
        }
        public List<PurchaseReturnBO> GetPurchaseReturnList()
        {
            return PurchaseReturnDAL.GetPurchaseReturnList();
        }
        public List<PurchaseReturnBO> GetPurchaseReturnDetail(int ID)
        {
            return PurchaseReturnDAL.GetPurchaseReturnDetail(ID);
        }
        public List<GRNTransItemBO> GetPurchaseReturnTransList(int ID)
        {
            return PurchaseReturnDAL.GetPurchaseReturnTransList(ID);
        }
        public DatatableResultBO GetPurchaseReturnOderListForDataTable(string Type, string TransNo, string TransDate, string SupplierName, string SortField, string SortOrder, int Offset, int Limit)
        {
            return PurchaseReturnDAL.GetPurchaseReturnOderListForDataTable(Type, TransNo, TransDate, SupplierName, SortField, SortOrder, Offset, Limit);
        }

    }
}
