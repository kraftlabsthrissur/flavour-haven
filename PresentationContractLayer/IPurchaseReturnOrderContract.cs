using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPurchaseReturnOrderContract
    {
        string SavePurchaseReturn(PurchaseReturnBO purchaseReturnBO, List<GRNTransItemBO> grnTransItems);
        bool UpdatePurchaseOrderReturn(PurchaseReturnBO purchaseReturnBO, List<GRNTransItemBO> grnTransItems);
        List<PurchaseReturnBO> GetPurchaseReturnList();
        List<PurchaseReturnBO> GetPurchaseReturnDetail(int ID);
        List<GRNTransItemBO> GetPurchaseReturnTransList(int ID);
        List<PurchaseReturnBO> GetPurchaseReturnOrderList(int SupplierID);
        List<PurchaseReturnTransItemBO> GetPurchaseReturnOrder(int orderids);
        DatatableResultBO GetPurchaseReturnOderListForDataTable(string Type, string TransNo, string TransDate, string SupplierName, string SortField, string SortOrder, int Offset, int Limit);
    }
}
