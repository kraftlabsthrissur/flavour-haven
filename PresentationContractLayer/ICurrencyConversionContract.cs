using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ICurrencyConversionContract
    {
        List<CurrencyConversionBO> GetCurrencyConversionList();
        string CreateCurrencyConversion(CurrencyConversionBO currencyConversionBO);
        int EditCurrencyConversion(CurrencyConversionBO currencyConversionBO);
        CurrencyConversionBO GetCurrencyConversionDetails(int CurrencyConversionID);
        List<CurrencyConversionBO> GetCurrencyDetails(int BaseCurrencyID, int ConversionCurrencyID);
    }
}
