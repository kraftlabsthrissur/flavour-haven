using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ISalesReturn
    {
        List<SalesReturnBO> GetSalesReturnList();
        List<SalesItemBO> GetSalesReturnTrans(int ID);
        bool SaveSalesReturn(SalesReturnBO salesReturnBO, List<SalesItemBO> salesItemBO);
        List<SalesReturnBO> GetSalesReturn(int ReturnID);
        List<SalesReturnBO> GetSalesReturnLogicCodeList();
        DatatableResultBO GetSalesReturnListForDataTable(string Type, string ReturnNo, string ReturnDate, string CustomerName, string SortField, string SortOrder, int Offset, int Limit);

    }
}
