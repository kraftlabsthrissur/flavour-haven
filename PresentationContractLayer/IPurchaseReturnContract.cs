//File created by prama on 26-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPurchaseReturnContract
    {

        bool SavePurchaseReturn(PurchaseReturnBO purchaseReturnBO, List<PurchaseReturnTransItemBO> TransItems);

        List<PurchaseReturnBO> GetPurchaseReturnList(int ID);

        List<PurchaseReturnTransItemBO> GetPurchaseReturnTransList(int ID);

        List<PurchaseReturnBO> GetPurchaseReturnAutocompleteByID(string Term, int ItemID = 0, int SupplierID = 0);

        List<PurchaseReturnBO> GetPurchaseReturnListForIRG(int SupplierID);

        List<PurchaseReturnTransItemBO> GetPurchaseReturnForIRG(int returnids);

        DatatableResultBO GetPurchaseReturn(string Type, string TransNo, string TransDate, string SupplierName, string SortField, string SortOrder, int Offset, int Limit);


    }
}
