using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;


namespace BusinessLayer
{
    public class RateAdjustmentBL : IRateAdjustmentContract
    {
        RateAdjustmentDAL rateAdjustmentDAL;

        public RateAdjustmentBL()
        {
            rateAdjustmentDAL = new RateAdjustmentDAL();
        }
        public List<RateAdjustmentItemBO> GetRateAdjustmentItems(int ItemCategoryID, int ItemID)
        {
            return rateAdjustmentDAL.GetRateAdjustmentItems(ItemCategoryID, ItemID);
        }

        public int Save(RateAdjustmentBO RateAdjBO, List<RateAdjustmentItemBO> items)
        {
            if (RateAdjBO.ID == 0)
            {
                return rateAdjustmentDAL.Save(RateAdjBO, items);
            }
            else
            {
                return rateAdjustmentDAL.Update(RateAdjBO, items);
            }
        }
        public List<RateAdjustmentBO> GetRateAdjustmentList()
        {
            return rateAdjustmentDAL.GetRateAdjustmentList();
        }
        public List<RateAdjustmentItemBO> GetRateAdjustmentTrans(int ID)
        {
            return rateAdjustmentDAL.GetRateAdjustmentTrans(ID);
        }
        public List<RateAdjustmentBO> GetRateAdjustmentDetail(int ID)
        {
            return rateAdjustmentDAL.GetRateAdjustmentDetail(ID);
        }
        public DatatableResultBO GetRateAdjustmentListForDataTable(string Type, string TransNo, string TransDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            return rateAdjustmentDAL.GetRateAdjustmentListForDataTable(Type, TransNo, TransDate,SortField, SortOrder, Offset, Limit);
        }

    }
}
