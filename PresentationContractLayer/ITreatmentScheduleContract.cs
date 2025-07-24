using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ITreatmentScheduleContract
    {

        List<TreatmentScheduleBO> GetTreatmentRoomDetails();
        List<TreatmentScheduleBO> GetTherapistDetails();
        List<TreatmentScheduleBO> GetDurationForTreatmentList();
        int Save(List<TreatmentScheduleItemBO> Items);
        DatatableResultBO GetTreatmentScheduleList(string Type, string DateHint, string StartTimeHint, string EndTimeHint, string TreatmentHint, string PatientHint, string DoctorHint, string TherapistHint, string TreatmentRoomHint, string StatusHint, string MedicinesHint, string SortField, string SortOrder, int Offset, int Limit);
        List<TreatmentScheduleItemBO> GetScheduledTreatmentList(string Type, DateTime? FromDate, int PatientID, int AppointmentProcessID);

    }
}
