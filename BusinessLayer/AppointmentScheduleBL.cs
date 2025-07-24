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
    public class AppointmentScheduleBL : IAppointmentScheduleContract
    {
        AppointmentScheduleDAL appointmentScheduleDAL;

        public AppointmentScheduleBL()
        {
            appointmentScheduleDAL = new AppointmentScheduleDAL();
        }

        public int Save(AppointmentScheduleBO appointmentScheduleBO, List<AppointmentScheduleItemBO> Items)
        {
            return appointmentScheduleDAL.Save(appointmentScheduleBO, Items);
        }

        public List<AppointmentScheduleItemBO> GetAppointmentItems(int DoctorID, DateTime FromDate)
        {
            return appointmentScheduleDAL.GetAppointmentItems(DoctorID, FromDate);
        }

        public bool IsDeletable(int DoctorID, int PatientID, DateTime Date)
        {
            return appointmentScheduleDAL.IsDeletable(DoctorID, PatientID, Date);
        }

        public DatatableResultBO GetAppointmentScheduleList(string Type, string DoctorNameHint, string DateHint, string PatientCodeHint, string PatientHint, string TimeHint, string TokenNoHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return appointmentScheduleDAL.GetAppointmentScheduleList(Type, DoctorNameHint, DateHint, PatientCodeHint, PatientHint, TimeHint, TokenNoHint, SortField, SortOrder, Offset, Limit);
        }
        public bool IsAppointmentProcessed(int DoctorID, int PatientID)
        {
            return appointmentScheduleDAL.IsAppointmentProcessed(DoctorID, PatientID);
        }

        public bool GetAppointmentConfirmation(int DoctorID, int PatientID, DateTime Date, int AppointmentScheduleItemID, int BillablesID)
        {
            return appointmentScheduleDAL.GetAppointmentConfirmation(DoctorID, PatientID, Date, AppointmentScheduleItemID, BillablesID);
        }

        public AppointmentScheduleBO GetAppointmentFee(int AppointmentScheduleItemID)
        {
            return appointmentScheduleDAL.GetAppointmentFee(AppointmentScheduleItemID);
        }
        public List<ConsultationBO> GetConsulationItem(int AppointmentScheduleItemID)
        {
            return appointmentScheduleDAL.GetConsulationItem(AppointmentScheduleItemID);
        }
        public int SaveAppointment(AppointmentScheduleBO Appointment, List<ConsultationBO> Items)
        {
            return appointmentScheduleDAL.SaveAppointment(Appointment, Items);
        }
        public int UpdateAppointment(AppointmentScheduleBO Appointment)
        {
            return appointmentScheduleDAL.UpdateAppointment(Appointment);
        }
        public List<AppointmentScheduleBO> GetAppointmentScheduleDetailsForPrint(int ID)
        {
            return appointmentScheduleDAL.GetAppointmentScheduleDetailsForPrint(ID);
        }
        public List<AppointmentScheduleBO> GetPatientDetailsForPatientCard(int ID)
        {
            return appointmentScheduleDAL.GetPatientDetailsForPatientCard(ID);
        }
        public List<AppointmentScheduleBO> GetPatientDetails(int ID)
        {
            return appointmentScheduleDAL.GetPatientDetails(ID);
        }
        public bool CreateAppointmentCancellation(int AppointmentScheduleItemID, int PatientID, DateTime Date)
        {
            return appointmentScheduleDAL.CreateAppointmentCancellation(AppointmentScheduleItemID, PatientID, Date);
        }
        public int SaveAndConfirmAppointment(AppointmentScheduleBO Appointment)
        {
            return appointmentScheduleDAL.SaveAndConfirmAppointment(Appointment);
        }
        public bool DeleteAppointmentScheduleItems(int AppointmentScheduleItemID)
        {
            return appointmentScheduleDAL.DeleteAppointmentScheduleItems(AppointmentScheduleItemID);
        }
        public List<AppointmentScheduleBO> GetPatientForBarCodeGenerator(int ID)
        {
            return appointmentScheduleDAL.GetPatientForBarCodeGenerator(ID);
        }
        public List<AppointmentScheduleBO> GetPatientForBarCodeGeneratorWithImage(int ID)
        {
            return appointmentScheduleDAL.GetPatientForBarCodeGeneratorWithImage(ID);
        }

    }
}
