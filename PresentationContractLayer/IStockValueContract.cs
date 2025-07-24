using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IStockValueContract
    {
        List<StockValueBO> GetItemStockValue();

        List<StockValueBO> Execute();

        DatatableResultBO GetStockValueList(string NameHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
