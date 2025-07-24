using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IDepartmentContract
    {
        List<DepartmentBO> GetDepartmentList();
        List<DepartmentBO> GetAllDepartment();
        List<DepartmentBO> GetDepartmentGroupList();
        List<DepartmentBO> GetDepartmentDetails(int DepartmentID);
        int Save(DepartmentBO departmentBO);
        int UpdateDepartment(DepartmentBO Department);
        List<DepartmentBO> GetPatientDepartment();
        List<DepartmentBO> GetPatientDepartmentsforAutoComplete(string Hint);
    }
}
