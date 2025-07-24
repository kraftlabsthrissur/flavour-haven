///File created by prama on

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class EmployeeBL : IEmployeeContract
    {
        EmployeeDAL employeeDAL;

        public EmployeeBL()
        {
            employeeDAL = new EmployeeDAL();
        }

        public List<EmployeeBO> GetEmployeeList()
        {
            return employeeDAL.GetEmployeeList();
        }
        public List<EmployeeBO> GetDoctorsList()
        {
            return employeeDAL.GetDoctorsList();
        }

        public DatatableResultBO GetEmployeeList(int EmployeeCategoryID, int DefaultLocationID,string Type,string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return employeeDAL.GetEmployeeList(EmployeeCategoryID, DefaultLocationID, Type, CodeHint, NameHint, DepartmentHint, LocationHint, SortField, SortOrder, Offset, Limit);
        }

        public List<EmployeeBO> GetEmployeeForAutoComplete(string Hint, int offset = 0, int limit = 10)
        {
            return employeeDAL.GetEmployeeForAutoComplete(Hint, offset, limit);
        }

        public List<EmployeeSalaryComponentBO> GetSalaryComponentList()
        {
            return employeeDAL.GetSalaryComponentList();
        }

        public List<EmployeeBO> GetEmployeeJobTypeList()
        {
            return employeeDAL.GetEmployeeJobTypeList();
        }

        public List<EmployeeBO> GetAspNetUsersList()
        {
            return employeeDAL.GetAspNetUsersList();
        }

//
        public string Save(EmployeeBO employeeBo, List<ExEmployDetailBO> ExemployDetails, List<SalaryDetailsBO> SalaryDetails, List<AddressBO> AddressList, List<EmployeeBO> FreeMedicineLocationList)
        {
            //// TODO add your business logic hear
            if (employeeBo.ID == 0)
            {
                return employeeDAL.Save(employeeBo, ExemployDetails, SalaryDetails, AddressList, FreeMedicineLocationList);
            }
            else
            {
                return employeeDAL.Update(employeeBo, ExemployDetails, SalaryDetails, AddressList, FreeMedicineLocationList);
            }
        }
        public EmployeeBO GetEmployee(int EmployeeID)
        {
            return employeeDAL.GetEmployee(EmployeeID);
        }
        public List<ExEmployDetailBO> GetExEmployDetails(int EmployeeID)
        {
            return employeeDAL.GetExEmployDetails(EmployeeID);
        }
        public List<SalaryDetailsBO> GetSalaryDetails(int EmployeeID)
        {
            return employeeDAL.GetSalaryDetails(EmployeeID);
        }

        public List<EmployeeBO> GetUserLocationList(int ID)
        {
            return employeeDAL.GetUserLocationList(ID);
        }
        public DatatableResultBO GetEmployeeListForUserRole(string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return employeeDAL.GetEmployeeListForUserRole(CodeHint, NameHint, DepartmentHint, LocationHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetFreeMedicineEmployeeList(string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return employeeDAL.GetFreeMedicineEmployeeList(CodeHint, NameHint, DepartmentHint, LocationHint, SortField, SortOrder, Offset, Limit);
        }
        public List<EmployeeBO> GetEmployeeByDepartment(int? DepartmentID)
        {
            return employeeDAL.GetEmployeeByDepartment(DepartmentID);
        }
        public int UpdateUserID(string SerialNo, int UserID)
        {
            return employeeDAL.UpdateUserID( SerialNo, UserID);
        }

        public List<EmployeeBO> GetDoctor()
        {
            return employeeDAL.GetDoctor();
        }
        public EmployeeBO GetDepartmentID()
        {
            return employeeDAL.GetDepartmentID();
        }
        public DatatableResultBO GetTherapistAutoComplete(string Hint)
        {
            return employeeDAL.GetTherapistAutoComplete(Hint);
        }
    }
}
