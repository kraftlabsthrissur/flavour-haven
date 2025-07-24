using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class CurrencyDAL
    {
        public IList<CurrencyBO> GetCurrencyList()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SPGetCurrencyList().Select(a => new CurrencyBO
                    {
                        Id = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Description = a.Description,
                        CountryName = a.CountryName,
                        DecimalPlaces = a.DecimalPlaces,
                       
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public int CreateCurrency(CurrencyBO currencyBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.spCreateCurrency(currencyBO.Code, currencyBO.Name, currencyBO.Description, currencyBO.CountryID,currencyBO.DecimalPlaces,  GeneralBO.CreatedUserID, currencyBO.MinimumCurrency, currencyBO.MinimumCurrencyCode);
                    return 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CurrencyBO GetCurrencyByLocationID(int LocationID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetCurrencyByLocationID(LocationID).FirstOrDefault();
                    if (result != null)
                    {
                        CurrencyBO currency = new CurrencyBO
                        {
                            Id = result.CurrencyID ,
                            Code = result.CurrencyCode
                            
                                                     
                        };
                        return currency;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public CurrencyBO GetCurrencyDetails(int CurrencyID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCurrencyByID(CurrencyID).Select(a => new CurrencyBO
                    {
                        Id = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Description = a.Description,
                        CountryName = a.CountryName,
                        CountryID = a.CountryId,
                        DecimalPlaces= a.DecimalPlaces,
                        MinimumCurrency=a.MinimumCurrency,
                        MinimumCurrencyCode=a.MinimumCurrencyCode
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateCurrency(CurrencyBO currencyBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateCurrency(currencyBO.Id, currencyBO.Code, currencyBO.Name, currencyBO.Description, currencyBO.CountryID,currencyBO.DecimalPlaces, currencyBO.MinimumCurrency, currencyBO.MinimumCurrencyCode, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DatatableResultBO GetCurrencySearchList(string Code, string Name, string CountryName, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetCurrencySearchList(Code, Name, CountryName, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code.Trim(),
                                Name = item.Name.Trim(),
                                CountryName = item.CountryName.Trim()
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
        public DatatableResultBO GetCurrencySearchList2(string Code, string Name, string CountryName, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetCurrencySearchList(Code, Name, CountryName, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code.Trim(),
                                Name = item.Name.Trim(),
                                CountryName = item.CountryName.Trim(),
                                CountryID = item.CountryID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
    }
}
