using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IStockAdjustmentReasonsContract
    {
        int Save(StockAdjustmentReasonsBO stockAdjustmentReasonsBO);
        DatatableResultBO GetStockAdjustmentReasonsList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);
        StockAdjustmentReasonsBO GetstockAdjustmentReasonsDetails(int ID);
    }
}
