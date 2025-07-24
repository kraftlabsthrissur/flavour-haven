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
    public class IRGBL : IIRGContract
    {
        IRGDAL PurchaseReturnDAL;
        public IRGBL()
        {
            PurchaseReturnDAL = new IRGDAL();
        }


        public string SaveIRG(PurchaseReturnBO purchaseReturnBO, List<PurchaseReturnTransItemBO> TransItems)

        {
            if (purchaseReturnBO.Id == 0)

            {
                return PurchaseReturnDAL.SaveIRG(purchaseReturnBO, TransItems);
            }
            else
            {
                return PurchaseReturnDAL.UpdateIRG(purchaseReturnBO, TransItems);
            }
        }
        public List<PurchaseReturnBO> GetIRGList()
        {
            return PurchaseReturnDAL.GetIRGList();
        }
        public List<PurchaseReturnBO> GetIRGDetail(int ID)
        {
            return PurchaseReturnDAL.GetIRGDetail(ID);
        }
        public List<PurchaseReturnTransItemBO> GetIRGTransList(int ID)
        {
            return PurchaseReturnDAL.GetIRGTransList(ID);
        }

        public DatatableResultBO GetIRGList(string Type, string TransNoHint, string TransDateHint, string SupplierHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return PurchaseReturnDAL.GetIRGList(Type, TransNoHint, TransDateHint, SupplierHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
