using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IOpeningStockContract
    {
        List<OpeningStockBO> GetOpeningStocks();

        OpeningStockBO GetOpeningStock(int OpeningStockID);

        List<OpeningStockItemBO> GetOpeningStockItems(int OpeningStockID);

        int Save(OpeningStockBO openingStockBO, List<OpeningStockItemBO> openingStockItemsBO);

        DatatableResultBO GetOpeningStockListForDataTable(string Type, string TransNo, string Date, string Store, string SortField, string SortOrder, int Offset, int Limit);

        List<OpeningStockMRPBO> GetMRPForOpeningStock(int ItemID, int BatchTypeID, string Batch);
    }
}
