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
    public class CurrencyBL : ICurrencyContract
    {
        CurrencyDAL currencyDAL;

        public CurrencyBL()
        {
            currencyDAL = new CurrencyDAL();
        }
        public List<CurrencyBO> GetCurrencyList()
        {
            try
            {
                return currencyDAL.GetCurrencyList().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EditCurrency(CurrencyBO currencyBO)
        {
            return currencyDAL.UpdateCurrency(currencyBO);
        }

        public CurrencyBO GetCurrencyDetails(int CurrencyID)
        {
            return currencyDAL.GetCurrencyDetails(CurrencyID);
        }
        public CurrencyBO GetCurrencyByLocationID(int LocationID)
        {
            return currencyDAL.GetCurrencyByLocationID(LocationID);
        }
        public int CreateCurrency(CurrencyBO currencyBO)
        {
            return currencyDAL.CreateCurrency(currencyBO);
        }
        public DatatableResultBO GetCurrencySearchList(string Code, string Name, string CountryName, string SortField, string SortOrder, int Offset, int Limit)
        {
            return currencyDAL.GetCurrencySearchList(Code, Name, CountryName, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetCurrencySearchList2(string Code, string Name, string CountryName, string SortField, string SortOrder, int Offset, int Limit)
        {
            return currencyDAL.GetCurrencySearchList2(Code, Name, CountryName, SortField, SortOrder, Offset, Limit);
        }
    }
}
