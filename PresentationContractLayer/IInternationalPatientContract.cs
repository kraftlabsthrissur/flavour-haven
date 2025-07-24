using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IInternationalPatientContract
    {
        int Save(CustomerBO CustomerBO,List<DiscountBO> DiscountDetails);
        DatatableResultBO GetInternationalPatientList(string type,string CodeHint,string NameHint, string PlaceHint, string DistrictHint, string DoctorHint, string PhoneHint, string LastVisitDateHint, string SortField,string SortOrder,int Offset,int Limit);
        DatatableResultBO GetInPatientList(string PatientName, string PatientCode, string InPatientNo, string RoomName, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetAppointmentScheduledPatientList(string CodeHint, string NameHint, string OPnoHint, string PhoneHint, string SortField, string SortOrder, int Offset, int Limit);
        List<CustomerBO> GetInternationalPatientDetails(int ID);
        CustomerBO GetPatientDiscount(int ID);
        
    }
}
