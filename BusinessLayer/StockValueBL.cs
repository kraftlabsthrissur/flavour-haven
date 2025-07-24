using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using BusinessLayer;


namespace BusinessLayer
{
   public class StockValueBL:IStockValueContract
    {
        StockValueDAL stockValueDAL;

        public StockValueBL()
        {
            stockValueDAL = new StockValueDAL();
        }

        public List<StockValueBO> GetItemStockValue()
        {
            return stockValueDAL.GetItemStockValue();
        }

        public List<StockValueBO> Execute()
        {
            return stockValueDAL.Execute();
        }

        public DatatableResultBO GetStockValueList(string NameHint,  string SortField, string SortOrder, int Offset, int Limit)
        {
            return stockValueDAL.GetStockValueList(NameHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
