using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
   public class EmployeeFreeMedicineCreditLimitBL : IEmployeeFreeMedicineCreditLimitContract
    {
        EmployeeFreeMedicineCreditLimitDAL EmpFreeMedDAL;

        #region Constructor
        public EmployeeFreeMedicineCreditLimitBL()
        {
            EmpFreeMedDAL = new EmployeeFreeMedicineCreditLimitDAL();
        }
        #endregion

        public List<EmployeeFreeMedicineCreditLimitBO> GetEmployeeByFilterForFreeMedicineCreditLimit(int LocationID, int EmployeeCategoryID,int EmployeeID)
        {
            return EmpFreeMedDAL.GetEmployeeByFilterForFreeMedicineCreditLimit(LocationID,EmployeeCategoryID, EmployeeID);
        }
        public int  Save(List<EmployeeFreeMedicineCreditLimitBO> EmployeeFreeMedicineCreditLimits)
        {
            string StringItems = XMLHelper.Serialize(EmployeeFreeMedicineCreditLimits);
            return EmpFreeMedDAL.Save(StringItems);
        }

        public DatatableResultBO GetEmployeeFreeMedicineCreditLimitList(string EmployeeCodeHint, string EmployeeNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return EmpFreeMedDAL.GetEmployeeFreeMedicineCreditLimitList(EmployeeCodeHint, EmployeeNameHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
