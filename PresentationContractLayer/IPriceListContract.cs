using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IPriceListContract
    {
        List<PriceListBO> GetPriceList();
        List<PriceListBO> GetPriceListDetails(int ID);
        List<PriceListItemBO> GetPriceListTransDetails(int ID);
        int Save(List<PriceListItemBO> Items, PriceListBO priceListBO);
        List<PriceListItemBO> ReadExcel(string Path);
    }
}
