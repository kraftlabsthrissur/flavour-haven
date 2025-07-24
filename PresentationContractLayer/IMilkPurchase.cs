using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace PresentationContractLayer
{
    public interface IMilkPurchase
    {
        List<MilkPurchaseBO> GetAllMilkPurchase(int Id);
        List<MilkPurchseTransBO> GetAllMilkPurchaseTrans(int Id);
        List<MilkFatRangeBO> GetMilkFatRange(string hint);
        bool SaveMilkPurchase(MilkPurchaseBO _milkMaster, List<MilkPurchseTransBO> _milkTrans);
       
        MilkFatRangeBO GetMilkFatRangePrice(int ID);
        List<MilkSupplierBO> GetMilkSupplierList(string hint);
        List<MilkPurchaseRequisitionBO> GetMilkPurchaseRqusition(DateTime Date);

        DatatableResultBO GetMilkPurchaseList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string QuantityHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit);



    }
}
