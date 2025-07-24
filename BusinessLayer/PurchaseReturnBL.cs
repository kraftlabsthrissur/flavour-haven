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
    public class PurchaseReturnBL : IPurchaseReturnContract
    {
        PurchaseReturnDAL PurchaseReturnDAL;
        public PurchaseReturnBL()
        {
            PurchaseReturnDAL = new PurchaseReturnDAL();
        }

        public GRNTransItemBO GetGRNTransForPurchaseReturn(int ItemID, int GRNID)
        {

            return PurchaseReturnDAL.GetGRNTransForPurchaseReturn(ItemID, GRNID);

        }
        public bool SavePurchaseReturn(PurchaseReturnBO purchaseReturnBO, List<PurchaseReturnTransItemBO> TransItems)

        {
            if (purchaseReturnBO.Id == 0)
            {
                return PurchaseReturnDAL.SavePurchaseReturn(purchaseReturnBO, TransItems);
            }
            else
            {
                return PurchaseReturnDAL.UpdatePurchaseReturn(purchaseReturnBO, TransItems);
            }
        }
        public List<PurchaseReturnBO> GetPurchaseReturnList(int ID)
        {
            return PurchaseReturnDAL.GetPurchaseReturnList(ID);
        }
        public List<PurchaseReturnTransItemBO> GetPurchaseReturnTransList(int ID)
        {
            return PurchaseReturnDAL.GetPurchaseReturnTransList(ID);
        }

        public List<PurchaseReturnBO> GetPurchaseReturnAutocompleteByID(string term, int ItemID, int SupplierID)
        {
            return PurchaseReturnDAL.GetPurchaseReturnAutocompleteByID(term, ItemID, SupplierID);
        }
        public List<PurchaseReturnBO> GetPurchaseReturnListForIRG(int SupplierID)
        {
            return PurchaseReturnDAL.GetPurchaseReturnListForIRG(SupplierID);
        }
        public List<PurchaseReturnTransItemBO> GetPurchaseReturnForIRG(int returnid)
        {
            return PurchaseReturnDAL.GetPurchaseReturnForIRG(returnid);
        }

        public DatatableResultBO GetPurchaseReturn(string Type, string TransNo, string TransDate, string SupplierName,string SortField, string SortOrder, int Offset, int Limit)
        {
            return PurchaseReturnDAL.GetPurchaseReturn(Type, TransNo, TransDate, SupplierName,SortField, SortOrder, Offset, Limit);
        }
    }
}
