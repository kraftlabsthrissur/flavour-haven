using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IIRGContract
    {

        string SaveIRG(PurchaseReturnBO purchaseReturnBO, List<PurchaseReturnTransItemBO> TransItems);
        List<PurchaseReturnBO> GetIRGList();
        List<PurchaseReturnTransItemBO> GetIRGTransList(int ID);
        List<PurchaseReturnBO> GetIRGDetail(int ID);

        DatatableResultBO GetIRGList(string Type,string TransNoHint,string TransDateHint,string SupplierHint,string SortField,string SortOrder,int Offset,int Limit);

    }
}
