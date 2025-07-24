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
    public class OpeningStockBL : IOpeningStockContract
    {
        OpeningStockDAL openingStockDAL;
        public OpeningStockBL()
        {
            openingStockDAL = new OpeningStockDAL();
        }

        public List<OpeningStockBO> GetOpeningStocks()
        {
            return openingStockDAL.GetOpeningStocks();

        }
        public OpeningStockBO GetOpeningStock(int OpeningStockID)
        {
            return openingStockDAL.GetOpeningStock(OpeningStockID);
        }


        public List<OpeningStockItemBO> GetOpeningStockItems(int OpeningStockID)
        {
            return openingStockDAL.GetOpeningStockItems(OpeningStockID);
        }

        public int Save(OpeningStockBO openingStockBO, List<OpeningStockItemBO> openingStockItemsBO)
        {
            //// TODO add your business logic hear
            if (openingStockBO.ID == 0)
            {
                return openingStockDAL.Save(openingStockBO, openingStockItemsBO);
            }
            else
            {
                return openingStockDAL.Update(openingStockBO, openingStockItemsBO);
            }
        }

        public DatatableResultBO GetOpeningStockListForDataTable(string Type, string TransNo, string Date, string Store, string SortField, string SortOrder, int Offset, int Limit)
        {
            return openingStockDAL.GetOpeningStockListForDataTable(Type, TransNo, Date, Store, SortField, SortOrder, Offset, Limit);
        }

        public List<OpeningStockMRPBO> GetMRPForOpeningStock(int ItemID, int BatchTypeID, string Batch)
        {
            return openingStockDAL.GetMRPForOpeningStock(ItemID, BatchTypeID, Batch);
        }

    }
}
