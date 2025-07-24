using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IEmployeeFreeMedicineCreditLimitContract
    {
        List<EmployeeFreeMedicineCreditLimitBO> GetEmployeeByFilterForFreeMedicineCreditLimit(int LocationID,int EmployeeCategoryID,int EmployeeID);
        int Save(List<EmployeeFreeMedicineCreditLimitBO> EmployeeFreeMedicineCreditLimits);
        DatatableResultBO GetEmployeeFreeMedicineCreditLimitList(string EmployeeCodeHint, string EmployeeNameHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
