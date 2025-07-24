using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TDSDAL
    {
        public List<TDSBO> GetTDSList()
        {
            try
            {
                List<TDSBO> TDS = new List<TDSBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    TDS = dbEntity.SpGetTDS().Select(a => new TDSBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        ItemAccountCategory = a.ItemAccountCategory,
                        ITSection = a.ITSection
                    }).ToList();

                    return TDS;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<TDSBO> GetTDSDetails(int TDSID)
        {
            try
            {
                List<TDSBO> TDSList = new List<TDSBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    TDSList = dbEntity.SpGetTDSDetails(TDSID).Select(a => new TDSBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        ItemAccountCategory = a.ItemAccountCategory,
                        TDSRate = (decimal)a.TDSRate,
                        CompanyType = a.CompanyType,
                        ExpenseType = a.ExpenseType,
                        ITSection = a.ITSection,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        Remarks = a.Remarks

                    }).ToList();
                    return TDSList;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public int Save(TDSBO tds)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    if (!generalDAL.IsCodeAlreadyExists("TDS", "Code", tds.Code))
                    {
                        return dbEntity.SpCreateTDS(tds.Code, tds.Name, tds.ItemAccountCategory, tds.TDSRate,tds.CompanyType,tds.ExpenseType,tds.ITSection,
                            tds.StartDate, tds.EndDate,tds.Remarks)   ;
                    }
                    else
                    {
                        throw new CodeAlreadyExistsException("TDS code already exists");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public int Update(TDSBO tds)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    if (!generalDAL.IsCodeAlreadyExists("TDS", "Code", tds.Code, tds.ID))
                    {
                        return dbEntity.SpUpdateTDS(tds.ID, tds.Code, tds.Name, tds.ItemAccountCategory, tds.TDSRate, tds.CompanyType, tds.ExpenseType, tds.ITSection,
                            tds.StartDate, tds.EndDate, tds.Remarks,GeneralBO.CreatedUserID,GeneralBO.LocationID,GeneralBO.ApplicationID);
                    }
                    else
                    {
                        throw new CodeAlreadyExistsException("TDS code already exists");
                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}
