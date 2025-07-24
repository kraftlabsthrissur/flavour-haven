using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DesignationDAL
    {
        public List<DesignationBO> GetDesignationList()
        {
            List<DesignationBO> Designation = new List<DesignationBO>();
            using (MasterEntities dbEntity = new MasterEntities())
            {
                Designation = dbEntity.SpGetDesignationList().Select(a => new DesignationBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name

                }).ToList();
                return Designation;
            }
        }
        public List<DesignationBO> GetDesignationDetails(int DesignationID)
        {
            List<DesignationBO> Designation = new List<DesignationBO>();
            using (MasterEntities dbEntity = new MasterEntities())
            {
                Designation = dbEntity.SpGetDesignationDetails(DesignationID).Select(a => new DesignationBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    DepartmentID=(int)a.DepartmentID,
                    DepartmentName=a.Department,
                    IsActive=(bool)a.IsActive,
                    StartDate=(DateTime) a.StartDate,
                    EndDate= (DateTime)a.EndDate
                }).ToList();
                return Designation;
            }
        }
        public int Save(DesignationBO designation)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    if (!generalDAL.IsCodeAlreadyExists("Designation", "Code", designation.Code))
                    {
                        return dbEntity.SpCreateDesignation(designation.Code, designation.Name, designation.DepartmentID, designation.IsActive, designation.StartDate, designation.EndDate,
                            GeneralBO.CreatedUserID);
                    }
                    else
                    {
                        throw new CodeAlreadyExistsException("Designation code already exists");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public int Update(DesignationBO designation)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    if (!generalDAL.IsCodeAlreadyExists("Designation", "Code", designation.Code, designation.ID))
                    {
                        return dbEntity.SpUpdateDesignation(designation.ID, designation.Code, designation.Name, designation.DepartmentID, designation.IsActive, designation.StartDate,
                            designation.EndDate,GeneralBO.CreatedUserID,GeneralBO.LocationID,GeneralBO.ApplicationID);
                    }
                    else
                    {
                        throw new CodeAlreadyExistsException("Designation code already exists");
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
