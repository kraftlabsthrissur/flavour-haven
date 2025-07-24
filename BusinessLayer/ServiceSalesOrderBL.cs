using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BusinessLayer
{
   public class ServiceSalesOrderBL:IServiceSalesOrderContract
    {
        ServiceSalesOrderDAL servicesalesOrderDAL;
        public ServiceSalesOrderBL()
        {
            servicesalesOrderDAL = new ServiceSalesOrderDAL();
        }

        public decimal GetDiscountPercentage(int CustomerID, int ItemID)
        {
            return servicesalesOrderDAL.GetDiscountPercentage(CustomerID, ItemID);
        }

        public bool SaveServiceSalesOrder(SalesOrderBO SalesOrder, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails)
        {
            if (SalesOrder.ID == 0)
            {
                return servicesalesOrderDAL.SaveServiceSalesOrder(SalesOrder, Items, AmountDetails);
            }
            else
            {
                return servicesalesOrderDAL.UpdateServiceSalesOrder(SalesOrder, Items, AmountDetails);
            }

        }

        public DatatableResultBO GetServiceSalesOrderList(string Type, string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return servicesalesOrderDAL.GetServiceSalesOrderList(Type, CodeHint, DateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetServiceSalesUnprocessOrderList(int CustomerID, string SalesType, string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return servicesalesOrderDAL.GetServiceSalesUnprocessOrderList(CustomerID, SalesType, CodeHint, DateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }

        public SalesOrderBO GetServiceSalesOrder(int ID)
        {
            return servicesalesOrderDAL.GetServiceSalesOrder(ID);
        }

        public List<SalesItemBO> GetServiceSalesOrderItems(int SalesOrderID)
        {
            return servicesalesOrderDAL.GetServiceSalesOrderItems(SalesOrderID);
        }
        public List<SalesItemBO> GetBillableDetails(int IPID,int CustomerID)
        {
            return servicesalesOrderDAL.GetBillableDetails(IPID, CustomerID);
        }

        //public DatatableResultBO GetServiceSalesOrderList(int CustomerID, int ItemCategoryID, string SalesType, string CodeHint, string DateHint, string CustomerNameHint, string SalesTypeHint, string DespatchDateHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        //{
        //    return servicesalesOrderDAL.GetServiceSalesOrderList(CustomerID, SalesType, CodeHint, DateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        //}

        public bool IsCancelable(int SalesOrderID)
        {
            return servicesalesOrderDAL.IsCancelable(SalesOrderID);
        }

        public void Cancel(int SalesOrderID)
        {
            servicesalesOrderDAL.Cancel(SalesOrderID);
        }
        public List<SalesItemBO> GetServiceSalesOrderItemsBySalesOrderIDs(int[] SalesOrderID)
        {
            string CommaSeparatedSalesOrderIDs = string.Join(",", SalesOrderID.Select(x => x.ToString()).ToArray());
            return servicesalesOrderDAL.GetServiceSalesOrderItemsBySalesOrderIDs(CommaSeparatedSalesOrderIDs);
        }
       public  int GetCustomerID(int IPID)
        {
            return servicesalesOrderDAL.GetCustomerID(IPID);
        }
    }
}
