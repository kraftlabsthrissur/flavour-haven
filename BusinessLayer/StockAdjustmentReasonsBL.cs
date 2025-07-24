using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class StockAdjustmentReasonsBL : IStockAdjustmentReasonsContract
    {
        StockAdjustmentReasonsDAL stockAdjustmentReasonsDAL;

        public StockAdjustmentReasonsBL()
        {
            stockAdjustmentReasonsDAL = new StockAdjustmentReasonsDAL();
        }

        public int Save(StockAdjustmentReasonsBO stockAdjustmentReasonsBO)
        {
            if (stockAdjustmentReasonsBO.ID == 0)
            {
                return stockAdjustmentReasonsDAL.Save(stockAdjustmentReasonsBO);
            }
            else
            {
                return stockAdjustmentReasonsDAL.Update(stockAdjustmentReasonsBO);
            }
        }

        public DatatableResultBO GetStockAdjustmentReasonsList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return stockAdjustmentReasonsDAL.GetStockAdjustmentReasonsList(CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
        }

        public StockAdjustmentReasonsBO GetstockAdjustmentReasonsDetails(int ID)
        {
            return stockAdjustmentReasonsDAL.GetstockAdjustmentReasonsDetails(ID);
        }
    }
}
