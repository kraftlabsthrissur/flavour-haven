using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class CountryDAL
    {
        public IList<CountryBO> GetCountryList()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SPGetCountryList().Select(a => new CountryBO
                    {
                        Id = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        IsActive = a.IsActive,
                        IsIntraCountry = a.IsIntraCountry
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public int CreateCountry(CountryBO countryBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.spCreateCountry(countryBO.Code, countryBO.Name, countryBO.IsActive, countryBO.IsIntraCountry);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CountryBO GetCountryDetails(int CountryID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCountryByID(CountryID).Select(a => new CountryBO
                    {
                        Id = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        IsActive = a.IsActive,
                        IsIntraCountry = a.IsIntraCountry
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateCountry(CountryBO countryBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateCountry(countryBO.Id, countryBO.Code, countryBO.Name, countryBO.IsActive, countryBO.IsIntraCountry, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DatatableResultBO GetGetCountrySearchList(string Code, string Name, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetCountrySearchList(Code, Name, SortField, SortOrder, Offset, Limit).ToList();
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
                                Code = item.Code,
                                Name = item.Name
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
