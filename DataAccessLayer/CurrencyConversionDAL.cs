using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class CurrencyConversionDAL
    {
        public IList<CurrencyConversionBO> GetCurrencyConversionList()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SPGetCurrencyConversionList().Select(a => new CurrencyConversionBO
                    {
                        Id = (int?)a.ID,
                        BaseCurrencyCode = a.BaseCurrencyCode,
                        ConversionCurrencyCode = a.ConversionCurrencyCode,
                        ExchangeRate = a.ExchangeRate,
                        InverseExchangeRate = a.InverseExchangeRate,
                        Description = a.Description,
                        IsActive = a.IsActive,
                        FromDate = a.FromDate
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public string CreateCurrencyConversion(CurrencyConversionBO currencyConversionBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var outMessageParameter = new ObjectParameter("OutMessage", "");
                    dbEntity.spCreateCurrencyConversion(currencyConversionBO.BaseCurrencyID, currencyConversionBO.ConversionCurrencyID, currencyConversionBO.ExchangeRate, currencyConversionBO.InverseExchangeRate, currencyConversionBO.FromDate, currencyConversionBO.Description, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID, outMessageParameter);
                    return outMessageParameter.Value.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CurrencyConversionBO GetCurrencyConversionDetails(int CurrencyID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetCurrencyConversionByID(CurrencyID).Select(a => new CurrencyConversionBO
                    {
                        Id = (int?)a.ID,
                        BaseCurrencyCode = a.BaseCurrencyCode,
                        ConversionCurrencyCode = a.ConversionCurrencyCode,
                        ExchangeRate = a.ExchangeRate,
                        InverseExchangeRate = a.InverseExchangeRate,
                        Description = a.Description,
                        IsActive = a.IsActive,
                        FromDate = a.FromDate
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateConversionCurrency(CurrencyConversionBO currencyConversionBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.spUpdateCurrencyConversion(currencyConversionBO.Id,currencyConversionBO.BaseCurrencyID, currencyConversionBO.ConversionCurrencyID, currencyConversionBO.ExchangeRate, currencyConversionBO.InverseExchangeRate, currencyConversionBO.FromDate, currencyConversionBO.Description, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<CurrencyConversionBO> GetCurrencyDetails(int BaseCurrencyID, int ConversionCurrencyID)
        {
            List<CurrencyConversionBO> list = new List<CurrencyConversionBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetCurrencyExchangeDetails(BaseCurrencyID, ConversionCurrencyID,GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CurrencyConversionBO
                {
                    BaseCurrencyCode = a.BaseCurrencyCode,
                    BaseCurrencyID = a.BaseCurrencyID,
                    ConversionCurrencyCode = a.ConversionCurrencyCode,
                    ConversionCurrencyID = a.ConversionCurrencyID,
                    ExchangeRate = a.ExchangeRate,
                    InverseExchangeRate = a.InverseExchangeRate

                }).ToList();
                return list;
            }
        }
    }
}
