using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ICurrencyContract
    {
        List<CurrencyBO> GetCurrencyList();
        int CreateCurrency(CurrencyBO CurrencyBO);
        int EditCurrency(CurrencyBO CurrencyBO);
        CurrencyBO GetCurrencyDetails(int CurrencyID);

        CurrencyBO GetCurrencyByLocationID(int LocationID);
        DatatableResultBO GetCurrencySearchList(string Code, string Name, string CountryName, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetCurrencySearchList2(string Code, string Name, string CountryName, string SortField, string SortOrder, int Offset, int Limit);
    }
}
