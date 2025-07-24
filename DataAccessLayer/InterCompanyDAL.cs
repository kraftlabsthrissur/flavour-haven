using BusinessObject;
using DataAccessLayer;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public class InterCompanyDAL
    {

        public List<InterCompanyBO> GetInterCompanyList()
        {
            List<InterCompanyBO> InterCompany = new List<InterCompanyBO>();
            using (MasterEntities dbEntity = new MasterEntities())
            {
                InterCompany = dbEntity.SpGetInterCompany().Select(a => new InterCompanyBO
                {
                    Name = a.Name,
                    ID = a.ID,
                    Code = a.Code,
                    Place = a.Place

                }).ToList();
                return InterCompany;
            }

        }
        public int Save(InterCompanyBO interCompany)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    if (!generalDAL.IsCodeAlreadyExists("InterCompany", "Code", interCompany.Code))
                    {
                       return dbEntity.SpCreateInterCompany(interCompany.Code, interCompany.Name, interCompany.Description, interCompany.StartDate, interCompany.EndDate,
                            GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    }
                    else
                    {
                        throw new CodeAlreadyExistsException("InterCompany code already exists");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public int Update(InterCompanyBO interCompany)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    if (!generalDAL.IsCodeAlreadyExists("InterCompany", "Code", interCompany.Code,interCompany.ID))
                    {
                        return dbEntity.SpUpdateInterCompany(interCompany.ID, interCompany.Code, interCompany.Name, interCompany.Description, interCompany.StartDate, interCompany.EndDate,
                            GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    }
                    else
                    {
                        throw new CodeAlreadyExistsException("InterCompany code already exists");
                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<InterCompanyBO> GetInterCompanyDetails(int InterCompanyID)
        {
            List<InterCompanyBO> InterCompany = new List<InterCompanyBO>();
            using (MasterEntities dbEntity = new MasterEntities())
            {
                InterCompany = dbEntity.SpGetInterCompanyDetails(InterCompanyID,GeneralBO.LocationID,GeneralBO.ApplicationID).Select(a => new InterCompanyBO
                {
                    Name = a.Name,
                    ID = a.ID,
                    Code = a.Code,
                    Description=a.Description,
                    StartDate=(DateTime)a.StartDate,
                    EndDate = (DateTime)a.EndDate,


                }).ToList();
                return InterCompany;
            }
        }

    }
}
