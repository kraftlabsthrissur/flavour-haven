using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ISalesForecastContract
    {
        int Process(SalesForecastBO SalesForecast);
        int Save(int ID);
        int IsSalesForecastExist(int Month);
        SalesForecastBO GetSalesForecast(int SalesForecastID);
        DatatableResultBO GetSalesForecasts(string TransNoHint, string MonthHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetSalesForecastItem(int SalesForecastID, string ItemNameHint, string CodeHint, string SalesCategoryHint, string LocationNameHint, string SortField, string SortOrder, int Offset, int Limit);
        List<SalesForecastItemBO> ReadExcel(string Path);
        int UploadSalesForecastItems(int ID,List<SalesForecastItemBO> SalesForecastItems);
    }
}
