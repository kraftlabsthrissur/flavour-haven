using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CurrencyConversionBL : ICurrencyConversionContract
    {
        CurrencyConversionDAL currencyConversionDAL;

        public CurrencyConversionBL()
        {
            currencyConversionDAL = new CurrencyConversionDAL();
        }
        public List<CurrencyConversionBO> GetCurrencyConversionList()
        {
            try
            {
                return currencyConversionDAL.GetCurrencyConversionList().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EditCurrencyConversion(CurrencyConversionBO currencyConversionBO)
        {
            return currencyConversionDAL.UpdateConversionCurrency(currencyConversionBO);
        }

        public CurrencyConversionBO GetCurrencyConversionDetails(int CurrencyConversionID)
        {
            return currencyConversionDAL.GetCurrencyConversionDetails(CurrencyConversionID);
        }

        public string CreateCurrencyConversion(CurrencyConversionBO currencyConversionBO)
        {
            return currencyConversionDAL.CreateCurrencyConversion(currencyConversionBO);
        }
        public List<CurrencyConversionBO> GetCurrencyDetails(int BaseCurrencyID, int ConversionCurrencyID)
        {
            return currencyConversionDAL.GetCurrencyDetails(BaseCurrencyID, ConversionCurrencyID);
        }
    }
}
