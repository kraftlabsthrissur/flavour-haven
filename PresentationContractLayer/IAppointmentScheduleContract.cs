using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IAppointmentScheduleContract
    {
        int Save(AppointmentScheduleBO appointmentScheduleBO, List<AppointmentScheduleItemBO> Items);
        List<AppointmentScheduleItemBO> GetAppointmentItems(int DoctorID, DateTime FromDate);
        bool IsDeletable(int DoctorID, int PatientID, DateTime Date);
        bool IsAppointmentProcessed(int DoctorID, int PatientID);
        bool GetAppointmentConfirmation(int DoctorID, int PatientID, DateTime Date, int AppointmentScheduleItemID, int BillablesID);
        DatatableResultBO GetAppointmentScheduleList(string Type, string DoctorNameHint, string DateHint, string PatientCodeHint, string PatientHint, string TimeHint, string TokenNoHint, string SortField, string SortOrder, int Offset, int Limit);
        AppointmentScheduleBO GetAppointmentFee(int AppointmentScheduleItemID);
        List<ConsultationBO> GetConsulationItem(int AppointmentScheduleItemID);
        int SaveAppointment(AppointmentScheduleBO Appointment, List<ConsultationBO> Items);
        int UpdateAppointment(AppointmentScheduleBO Appointment);
        List<AppointmentScheduleBO> GetAppointmentScheduleDetailsForPrint(int ID);
        List<AppointmentScheduleBO> GetPatientDetailsForPatientCard(int ID);
        List<AppointmentScheduleBO> GetPatientDetails(int ID);
        List<AppointmentScheduleBO> GetPatientForBarCodeGenerator(int ID);
        List<AppointmentScheduleBO> GetPatientForBarCodeGeneratorWithImage(int ID);
        bool CreateAppointmentCancellation(int AppointmentScheduleItemID, int PatientID, DateTime Date);
        int SaveAndConfirmAppointment(AppointmentScheduleBO Appointment);
        bool DeleteAppointmentScheduleItems(int AppointmentScheduleItemID);
    }
}
