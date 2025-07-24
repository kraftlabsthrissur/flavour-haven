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
    public class TreatmentScheduleBL : ITreatmentScheduleContract
    {
        TreatmentScheduleDAL treatmentScheduleDAL;
        public TreatmentScheduleBL()
        {
            treatmentScheduleDAL = new TreatmentScheduleDAL();
        }
        public List<TreatmentScheduleItemBO> GetScheduledTreatmentList(string Type, DateTime? FromDate, int PatientID = 0, int AppointmentProcessID = 0)
        {
            return treatmentScheduleDAL.GetScheduledTreatmentList(Type, FromDate, PatientID, AppointmentProcessID);
        }
        public List<TreatmentScheduleBO> GetTreatmentRoomDetails()
        {
            return treatmentScheduleDAL.GetTreatmentRoomDetails();
        }
        public List<TreatmentScheduleBO> GetTherapistDetails()
        {
            return treatmentScheduleDAL.GetTherapistDetails();
        }
        public List<TreatmentScheduleBO> GetDurationForTreatmentList()
        {
            return treatmentScheduleDAL.GetDurationForTreatmentList();
        }
        public int Save(List<TreatmentScheduleItemBO> Items)
        {

            string[] ScheduleIDList = Items.Select(I => Convert.ToString(I.ScheduleID)).ToArray();
            string CommaSeparatedScheduleID = string.Join(",", ScheduleIDList.Select(x => x.ToString()).ToArray());
            return treatmentScheduleDAL.Save(Items, CommaSeparatedScheduleID);
        }
        public DatatableResultBO GetTreatmentScheduleList(string Type, string DateHint, string StartTimeHint, string EndTimeHint, string TreatmentHint, string PatientHint, string DoctorHint, string TherapistHint, string TreatmentRoomHint, string StatusHint, string MedicinesHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return treatmentScheduleDAL.GetTreatmentScheduleList(Type,DateHint, StartTimeHint, EndTimeHint, TreatmentHint, PatientHint, DoctorHint, TherapistHint, TreatmentRoomHint, StatusHint, MedicinesHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
