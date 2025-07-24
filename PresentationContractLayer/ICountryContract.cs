using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ICountryContract
    {
        List<CountryBO> GetCountryList();
        int CreateCountry(CountryBO countryBO);
        int EditCountry(CountryBO countryBO);
        CountryBO GetCountryDetails(int CountryID);
        DatatableResultBO GetCountrySearchList(string Code, string Name, string SortField, string SortOrder, int Offset, int Limit);
    }
}
