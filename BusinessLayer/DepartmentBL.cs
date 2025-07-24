using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace BusinessLayer
{
    public class DepartmentBL : IDepartmentContract
    {
        DepartmentDAL departmentDAL;
        public DepartmentBL()
        {
            departmentDAL = new DepartmentDAL();

        }
        public List<DepartmentBO> GetDepartmentList()
        {
            return departmentDAL.GetDepartmentList();
        }
        public List<DepartmentBO> GetAllDepartment()
        {
            return departmentDAL.GetAllDepartment();
        }
        public List<DepartmentBO> GetDepartmentGroupList()
        {
            return departmentDAL.GetDepartmentGroupList();
        }
        public int Save(DepartmentBO departmentBO)
        {
            return departmentDAL.Save(departmentBO);

        }
        public List<DepartmentBO> GetDepartmentDetails(int DepartmentID)
        {
            return departmentDAL.GetDepartmentDetails(DepartmentID);
        }
        public int UpdateDepartment(DepartmentBO department)
        {
            return departmentDAL.Update(department);
        }

        public List<DepartmentBO> GetPatientDepartment()
        {
            return departmentDAL.GetPatientDepartment();
        }
        public List<DepartmentBO> GetPatientDepartmentsforAutoComplete(string Hint)
        {
            return departmentDAL.GetPatientDepartmentsforAutoComplete(Hint);
        }
    }
}
