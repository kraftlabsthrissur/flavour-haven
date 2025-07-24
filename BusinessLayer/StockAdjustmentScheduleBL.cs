using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class StockAdjustmentScheduleBL : IStockAdjustmentScheduleContract
    {
        StockAdjustmentScheduleDAL stockAdjustmentScheduleDAL;
        public StockAdjustmentScheduleBL()
        {
            stockAdjustmentScheduleDAL = new StockAdjustmentScheduleDAL();
        }
        public int GetItemCount()
        {
            return stockAdjustmentScheduleDAL.GetItemCount();
        }
        public int save(StockAdjustmentScheduleBO stockAdjustmentScheduleBO, List<ExcludedDateBO> Date)
        {
            if (stockAdjustmentScheduleBO.ID > 0)
            {
            return stockAdjustmentScheduleDAL.update(stockAdjustmentScheduleBO, Date);
                }
            else
            {
                return stockAdjustmentScheduleDAL.save(stockAdjustmentScheduleBO, Date);

            }
        }
      public  DatatableResultBO GetStockAdjustmentList(string SortField, string SortOrder, int Offset, int Limit)
{
            return stockAdjustmentScheduleDAL.GetStockAdjustmentList(SortField, SortOrder, Offset, Limit);
        }
        public List<StockAdjustmentScheduleBO> GetStockAdjustmentScheduleDetail(int ID)
        {
            return stockAdjustmentScheduleDAL.GetStockAdjustmentScheduleDetail(ID);


        }
    public    List<ExcludedDateBO> GetExcludedDate(int ID)
        {
            return stockAdjustmentScheduleDAL.GetExcludedDate(ID);

        }
    }
}
