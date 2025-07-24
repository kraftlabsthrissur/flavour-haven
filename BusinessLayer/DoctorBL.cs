using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;
using DataAccessLayer;


namespace BusinessLayer
{
   public class DoctorBL:IDoctorContract
    {
        DoctorDAL doctorDAL;

        public DoctorBL()
        {
            doctorDAL = new DoctorDAL();
        }

        public DatatableResultBO GetDoctorList( string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return doctorDAL.GetDoctorList( CodeHint, NameHint,  SortField, SortOrder, Offset, Limit);
        }

        public string Save(EmployeeBO employeeBo, List<ExEmployDetailBO> ExemployDetails, List<SalaryDetailsBO> SalaryDetails, List<AddressBO> AddressList, List<EmployeeBO> FreeMedicineLocationList)
        {
            //// TODO add your business logic hear
            if (employeeBo.ID == 0)
            {
                return doctorDAL.Save(employeeBo, ExemployDetails, SalaryDetails, AddressList, FreeMedicineLocationList);
            }
            else
            {
                return doctorDAL.Update(employeeBo, ExemployDetails, SalaryDetails, AddressList, FreeMedicineLocationList);
           
            }
        }

        public List<DoctorBO> GetDoctorList()
        {
            return doctorDAL.GetDoctorList();
        }

        public EmployeeBO GetEmployee(int EmployeeID)
        {
            return doctorDAL.GetEmployee(EmployeeID);
        }
    }
}
