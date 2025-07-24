using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
   public interface IRateAdjustmentContract
    {
        List<RateAdjustmentItemBO> GetRateAdjustmentItems( int ItemCategoryID, int ItemID);
        int Save(RateAdjustmentBO RateAdjBO, List<RateAdjustmentItemBO> items);
        List<RateAdjustmentBO> GetRateAdjustmentList();
        List<RateAdjustmentItemBO> GetRateAdjustmentTrans(int ID);
        List<RateAdjustmentBO> GetRateAdjustmentDetail(int ID);
        DatatableResultBO GetRateAdjustmentListForDataTable(string Type, string TransNo, string TransDate,string SortField, string SortOrder, int Offset, int Limit);

    }
}
