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
    public class AdvanceReceiptBL : IAdvanceReceiptContract
    {
        AdvanceReceiptDAL advanceReceiptDAL;
        
            public AdvanceReceiptBL()
        {
            advanceReceiptDAL = new AdvanceReceiptDAL();
        }

        public List<AdvanceReceiptBO> GetCustomerCategoryList()
        {
            return advanceReceiptDAL.GetCustomerCategoryList();
        }

        public List<SalesOrderBO> GetSalesOrders(int CustomerID)
        {
            return advanceReceiptDAL.GetSalesOrders(CustomerID);
        }

        public List<SalesOrderBO> GetItemNamesForSalesOrder(int SalesID, string TransNo, string search)
        {
            return advanceReceiptDAL.GetItemNamesForSalesOrder(SalesID, TransNo, search);

        }


        public int Save(AdvanceReceiptBO advanceReceiptBO, List<AdvanceReceiptItemBO> ReceiptItems)
        {
            return advanceReceiptDAL.Save(advanceReceiptBO, ReceiptItems);
        }

        public List<AdvanceReceiptBO> GetReceiptList()
        {
            return advanceReceiptDAL.GetReceiptList();
        }

        public List<AdvanceReceiptBO> GetAdvanceReceiptDetails(int id)
        {
            return advanceReceiptDAL.GetAdvanceReceiptDetails(id);
        }

        public List<AdvanceReceiptBO> GetAdvanceReceiptTransDetails(int id)
        {
            return advanceReceiptDAL.GetAdvanceReceiptTransDetails(id);
        }
    }
}
