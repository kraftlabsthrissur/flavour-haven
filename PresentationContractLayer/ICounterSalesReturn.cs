using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace PresentationContractLayer
{
    public interface ICounterSalesReturn
    {
        List<CounterSalesReturnBO> GetSalesReturnList();
        List<CounterSalesReturnItemBO> GetCounterSalesReturnTrans(int ID);        
        List<CounterSalesReturnBO> GetCounterSalesReturn(int ReturnID);
        bool SaveCounterSalesReturn(CounterSalesReturnBO counterSalesBO, List<CounterSalesReturnItemBO> Items);
        DatatableResultBO GetCounterSalesReturnListForDataTable(string Type, string ReturnNo, string ReturnDate,string SortField, string SortOrder, int Offset, int Limit);



    }
}
