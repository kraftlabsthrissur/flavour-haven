using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IDoctorContract
    {
        DatatableResultBO GetDoctorList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);
        string Save(EmployeeBO employeeBo, List<ExEmployDetailBO> ExemployDetails, List<SalaryDetailsBO> SalaryDetails, List<AddressBO> AddressList, List<EmployeeBO> FreeMedicineLocationList);
        List<DoctorBO> GetDoctorList();
        EmployeeBO GetEmployee(int EmployeeID);
    }
}
