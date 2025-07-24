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
    public class SalesReturnBL : ISalesReturn
    {
        SalesReturnDAL salesReturnDAL;
        public SalesReturnBL()
        {
            salesReturnDAL = new SalesReturnDAL();
        }

        public List<SalesReturnBO> GetSalesReturnList()
        {
            return salesReturnDAL.GetSalesReturnList();
        }
        public List<SalesReturnBO> GetSalesReturn(int ReturnID)
        {
            return salesReturnDAL.GetSalesReturn(ReturnID);
        }
        public List<SalesItemBO> GetSalesReturnTrans(int ID)
        {
            return salesReturnDAL.GetSalesReturnTrans(ID);

        }

        public bool SaveSalesReturn(SalesReturnBO salesReturnBO, List<SalesItemBO> salesReturnItemBO)
        {
            if (salesReturnBO.ID <= 0)
            {
                return salesReturnDAL.SaveSalesReturn(salesReturnBO, salesReturnItemBO);
            }
            else
            {
                return salesReturnDAL.UpdateSalesReturn(salesReturnBO, salesReturnItemBO);
            }
        }

        public List<SalesReturnBO> GetSalesReturnLogicCodeList()
        {
            return salesReturnDAL.GetSalesReturnLogicCodeList();
        }
        public DatatableResultBO GetSalesReturnListForDataTable(string Type, string ReturnNo, string ReturnDate, string CustomerName, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesReturnDAL.GetSalesReturnListForDataTable(Type, ReturnNo, ReturnDate, CustomerName, SortField, SortOrder, Offset, Limit);
        }

    }
}
