using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IAdvanceReceiptContract
    {
        List<AdvanceReceiptBO> GetCustomerCategoryList();
        List<AdvanceReceiptBO> GetReceiptList();
        List<AdvanceReceiptBO> GetAdvanceReceiptDetails(int id);
        List<SalesOrderBO> GetSalesOrders(int CustomerID);
        List<SalesOrderBO> GetItemNamesForSalesOrder(int SalesID, string TransNo, string search);
        int Save(AdvanceReceiptBO advanceReceiptBO, List<AdvanceReceiptItemBO> ReceiptItems);
        List<AdvanceReceiptBO> GetAdvanceReceiptTransDetails(int id);
    }
}
