using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TaxTypeDAL
    {
        public List<TaxTypeBO> GetTaxTypeDDLList()
        {
            try
            {
                List<TaxTypeBO> taxType = new List<TaxTypeBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    taxType = dbEntity.SPGetTaxType().Select(a => new TaxTypeBO
                    {
                        ID = a.ID,
                        Name = a.Name,

                    }).ToList();

                    return taxType;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<TaxTypeBO> GetTaxTypeListByLocation(int LocationID)
        {
            try
            {
                List<TaxTypeBO> taxType = new List<TaxTypeBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    taxType = dbEntity.SPGetTaxTypeByLocationID(LocationID).Select(a => new TaxTypeBO
                    {
                        ID = a.ID,
                        Name = a.Name,

                    }).ToList();

                    return taxType;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<TaxTypeBO> GetTaxTypeList()
        {
            try
            {
                List<TaxTypeBO> taxType = new List<TaxTypeBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    taxType = dbEntity.spGetTaxtypeList().Select(a => new TaxTypeBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                     //LocationName=a.locationname,
                     Description=a.Description

                    }).ToList();

                    return taxType;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int CreateTaxtype(TaxTypeBO TaxTypeBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.spCreateTaxtype(TaxTypeBO.Name, TaxTypeBO.Description, GeneralBO.LocationID,  GeneralBO.CreatedUserID);
                    return 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TaxTypeBO GetTaxtypeDetails(int TaxtypeID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetTaxtypeByID(TaxtypeID).Select(a => new TaxTypeBO
                    {
                        ID = a.ID,
                      
                        Name = a.Name,
                        Description = a.Description,
                        LocationName = a.LocationName,
                        LocationID = a.LocationId
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int UpdateTaxtype(TaxTypeBO TaxTypeBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdatetaxtype(TaxTypeBO.ID, TaxTypeBO.Name, TaxTypeBO.Description, GeneralBO.LocationID, GeneralBO.CreatedUserID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
