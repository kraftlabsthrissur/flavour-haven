using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
  public interface IStockAdjustmentScheduleContract
    {
          int GetItemCount();
        int save(StockAdjustmentScheduleBO stockAdjustmentScheduleBO, List<ExcludedDateBO> Date);
        DatatableResultBO GetStockAdjustmentList( string SortField, string SortOrder, int Offset, int Limit);
        List<StockAdjustmentScheduleBO> GetStockAdjustmentScheduleDetail(int ID);
         List<ExcludedDateBO>   GetExcludedDate(int ID);
    }
}
