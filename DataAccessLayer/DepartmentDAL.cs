using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class DepartmentDAL
    {
        public List<DepartmentBO> GetDepartmentList()
        {
            try
            {
                List<DepartmentBO> Department = new List<DepartmentBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Department = dbEntity.SpGetDepartment().Select(a => new DepartmentBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        DepartmentGroupID = a.DepartmentGroupID
                    }).ToList();

                    return Department;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DepartmentBO> GetAllDepartment()
        {
            try
            {
                List<DepartmentBO> Department = new List<DepartmentBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Department = dbEntity.SpGetDepartmentList().Select(a => new DepartmentBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        DepartmentGroupID = a.DepartmentGroupID
                    }).ToList();

                    return Department;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int Save(DepartmentBO department)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    if (!generalDAL.IsCodeAlreadyExists("Department", "Code", department.Code))
                    {
                        return dbEntity.SpCreateDepartment(department.Code, department.Name, department.DepartmentGroupID, department.IsActive, department.StartDate, department.EndDate,
                            GeneralBO.CreatedUserID);
                    }
                    else
                    {
                        throw new CodeAlreadyExistsException("Department code already exists");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public int Update(DepartmentBO department)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    if (!generalDAL.IsCodeAlreadyExists("Department", "Code", department.Code, department.ID))
                    {
                        return dbEntity.SpUpdateDepartment(department.ID, department.Code, department.Name, department.DepartmentGroupID, department.IsActive, department.StartDate,
                            department.EndDate, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    }
                    else
                    {
                        throw new CodeAlreadyExistsException("Department code already exists");
                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<DepartmentBO> GetDepartmentGroupList()
        {
            try
            {
                List<DepartmentBO> DepartmentGroup = new List<DepartmentBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    DepartmentGroup = dbEntity.SpGetDepartmentGroup().Select(a => new DepartmentBO
                    {
                        ID = a.ID,
                        Name = a.Name,

                    }).ToList();

                    return DepartmentGroup;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<DepartmentBO> GetDepartmentDetails(int DepartmentID)
        {
            try
            {
                List<DepartmentBO> Department = new List<DepartmentBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Department = dbEntity.SpGetDepartmentDetails(DepartmentID).Select(a => new DepartmentBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        DepartmentGroupID = a.DepartmentGroupID,
                        DepartmentGroup = a.DepartmentGroup,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        IsActive = a.IsActive

                    }).ToList();
                    return Department;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<DepartmentBO> GetPatientDepartment()
        {
            try
            {
                List<DepartmentBO> Department = new List<DepartmentBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Department = dbEntity.SpGetPatientDepartments().Select(a => new DepartmentBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,


                    }).ToList();
                    return Department;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<DepartmentBO> GetPatientDepartmentsforAutoComplete(string Hint)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPatientDepartmentsforAutoComplete(Hint).Select(a => new DepartmentBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
