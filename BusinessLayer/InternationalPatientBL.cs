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
    public class InternationalPatientBL : IInternationalPatientContract
    {
        InternationalPatientDAL patientDAL;

        public InternationalPatientBL()
        {
            patientDAL = new InternationalPatientDAL();
        }

        public int Save(CustomerBO CustomerBO, List<DiscountBO> DiscountDetails)
        {
            if (CustomerBO.ID == 0)
            {
                return patientDAL.Save(CustomerBO, DiscountDetails);
            }
            else
            {
                return patientDAL.Update(CustomerBO, DiscountDetails);
            }
        }

        public DatatableResultBO GetInternationalPatientList(string Type,string CodeHint, string NameHint,string PlaceHint, string DistrictHint, string DoctorHint, string PhoneHint, string LastVisitDateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return patientDAL.GetInternationalPatientList(Type,CodeHint, NameHint, PlaceHint, DistrictHint, DoctorHint, PhoneHint, LastVisitDateHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetInPatientList(string PatientName, string PatientCode, string InPatientNo, string RoomName, string SortField, string SortOrder, int Offset, int Limit)
        {
            return patientDAL.GetInPatientList(PatientName, PatientCode, InPatientNo, RoomName, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetAppointmentScheduledPatientList(string CodeHint, string NameHint, string OPnoHint, string PhoneHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return patientDAL.GetAppointmentScheduledPatientList(CodeHint, NameHint, OPnoHint, PhoneHint, SortField, SortOrder, Offset, Limit);
        }

        public List<CustomerBO> GetInternationalPatientDetails(int ID)
        {
            return patientDAL.GetInternationalPatientDetails(ID);
        }
        public CustomerBO GetPatientDiscount(int ID)
        {
            return patientDAL.GetPatientDiscount(ID);
        }
        
    }
}
