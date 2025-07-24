using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IServiceSalesOrderContract
    {
        decimal GetDiscountPercentage(int CustomerID, int ItemID);

        bool SaveServiceSalesOrder(SalesOrderBO SalesOrder, List<SalesItemBO> Items,List<SalesAmountBO> AmountDetails);

        DatatableResultBO GetServiceSalesOrderList(string Type, string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetServiceSalesUnprocessOrderList(int CustomerID, string SalesType, string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);

        SalesOrderBO GetServiceSalesOrder(int ID);

        List<SalesItemBO> GetServiceSalesOrderItems(int SalesOrderID);

        //DatatableResultBO GetServiceSalesOrderList(int CustomerID, int ItemCategoryID, string SalesType, string CodeHint, string DateHint, string CustomerNameHint, string SalesTypeHint, string DespatchDateHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);

        bool IsCancelable(int SalesOrderID);

        void Cancel(int SalesOrderID);
        List<SalesItemBO> GetServiceSalesOrderItemsBySalesOrderIDs(int[] SalesOrderID);

        List<SalesItemBO> GetBillableDetails(int IPID,int CustomerID);

        int GetCustomerID(int IPID);
    }
}
