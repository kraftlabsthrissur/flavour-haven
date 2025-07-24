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
    public class CountryBL : ICountryContract
    {
         CountryDAL countryDAL ;

        public CountryBL()
        {
            countryDAL = new CountryDAL();
        }
        public List<CountryBO> GetCountryList()
        {
            try
            {
                return countryDAL.GetCountryList().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EditCountry(CountryBO countryBO)
        {
            return countryDAL.UpdateCountry(countryBO);
        }

        public CountryBO GetCountryDetails(int CountryID)
        {
            return countryDAL.GetCountryDetails(CountryID);
        }

        public int CreateCountry(CountryBO countryBO)
        {
            return countryDAL.CreateCountry(countryBO);
        }
        public DatatableResultBO GetCountrySearchList(string Code, string Name, string SortField, string SortOrder, int Offset, int Limit)
        {
            return countryDAL.GetGetCountrySearchList(Code, Name, SortField, SortOrder, Offset, Limit);
        }
    }
}
