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
    public class SalesForecastBL : ISalesForecastContract
    {
        SalesForecastDAL salesForecastDAL;

        public SalesForecastBL()
        {
            salesForecastDAL = new SalesForecastDAL();
        }

        public int Process(SalesForecastBO SalesForecast)
        {
            return salesForecastDAL.Process(SalesForecast);
        }

        public int Save(int ID)
        {
            return salesForecastDAL.Save(ID);
        }

        public int IsSalesForecastExist(int Month)
        {
            return salesForecastDAL.IsSalesForecastExist(Month);
        }
        

        public DatatableResultBO GetSalesForecasts(string TransNoHint, string MonthHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesForecastDAL.GetSalesForecasts(TransNoHint, MonthHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetSalesForecastItem(int SalesForecastID, string ItemNameHint, string CodeHint, string SalesCategoryHint, string LocationNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesForecastDAL.GetSalesForecastItem(SalesForecastID, ItemNameHint, CodeHint, SalesCategoryHint, LocationNameHint, SortField, SortOrder, Offset,Limit);
        }

        public List<SalesForecastItemBO> ReadExcel(string Path)
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(0, "Location");
            dict.Add(1, "ItemCode");
            dict.Add(2, "ItemName");
            dict.Add(3, "Unit");
            dict.Add(4, "Price");
            dict.Add(5, "ComputedForecast");
            dict.Add(8, "FinalForecast");

            SalesForecastItemBO UploadSalesForeCastList = new SalesForecastItemBO();
            GeneralBL generalBL = new GeneralBL();
            List<SalesForecastItemBO> SalesForeCastList;
            try
            {
                SalesForeCastList = generalBL.ReadExcel(Path, UploadSalesForeCastList, dict);
            }
            catch (Exception e)
            {
                throw e;
            }
            return SalesForeCastList;
        }

        public int UploadSalesForecastItems(int ID,List<SalesForecastItemBO> SalesForecastItems)
        {
            string XMLItems = XMLHelper.Serialize(SalesForecastItems);
            return salesForecastDAL.UploadSalesForecastItems(ID, XMLItems);
        }

        public SalesForecastBO GetSalesForecast(int SalesForecastID)
        {
            return salesForecastDAL.GetSalesForecast(SalesForecastID);
        }
    }

    
}
