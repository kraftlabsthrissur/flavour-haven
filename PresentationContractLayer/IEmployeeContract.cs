using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;

namespace PresentationContractLayer
{
    public interface IEmployeeContract
    {
        DatatableResultBO GetEmployeeList(int EmployeeCategoryID, int DefaultLocationID,string Type,string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit);
        List<EmployeeBO> GetEmployeeList();
        List<EmployeeBO> GetDoctorsList();
        List<EmployeeBO> GetEmployeeForAutoComplete(string Hint, int offset = 0, int limit = 0);
        List<EmployeeSalaryComponentBO> GetSalaryComponentList();
        List<EmployeeBO> GetEmployeeJobTypeList();
        List<EmployeeBO> GetAspNetUsersList();

        string Save(EmployeeBO employeeBo, List<ExEmployDetailBO> ExemployDetails, List<SalaryDetailsBO> SalaryDetails, List<AddressBO> AddressList, List<EmployeeBO> CustomerLocationList);
        EmployeeBO GetEmployee(int EmployeeID);
        List<ExEmployDetailBO> GetExEmployDetails(int EmployeeID);
        List<SalaryDetailsBO> GetSalaryDetails(int EmployeeID);
        List<EmployeeBO> GetUserLocationList(int ID);
        DatatableResultBO GetEmployeeListForUserRole(string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetFreeMedicineEmployeeList(string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit);
        List<EmployeeBO> GetEmployeeByDepartment(int? DepartmentID);
        int UpdateUserID(string SerialNo, int UserID);

        List<EmployeeBO> GetDoctor();
        EmployeeBO GetDepartmentID();
        DatatableResultBO GetTherapistAutoComplete(string Hint);
    }
}
