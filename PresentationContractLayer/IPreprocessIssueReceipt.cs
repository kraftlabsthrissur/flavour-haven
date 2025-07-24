using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IPreprocessIssueReceipt
    {
        List<PreprocessReceiptItemBO> GetUnProcessedMaterialPurificationIssueItemList(string search = null);
        int Save(PreprocessReceiptBO preprocessReceiptBO);
        PreprocessReceiptBO GetMaterialPurificationReceipt(int id);
        List<PreProcessReceiptPurificationItemBO> GetMaterialPurificationReceiptDetails(int id);
        List<PreProcessReceiptDisplayBO> GetMaterialPurificationReceipts();
        int Cancel(int ID,string Table);

        DatatableResultBO GetPreProcessReceiptList(string Type,string TransNoHint,string IssueItemHint,string UnitHint,string IssueQtyHint,string ReceiptItemHint,string ReceiptUnitHint,string ReceiptQtyHint,string ActivityHint,string QuantityLossHint,string SortField,string SortOrder,int Offset,int Limit);
    }

}
