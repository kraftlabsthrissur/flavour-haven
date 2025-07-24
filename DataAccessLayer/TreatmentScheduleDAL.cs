using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TreatmentScheduleDAL
    {

        public List<TreatmentScheduleBO> GetTreatmentRoomDetails()
        {
            List<TreatmentScheduleBO> item = new List<TreatmentScheduleBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetTreatmentRoomDetails().Select(a => new TreatmentScheduleBO
                    {
                        TreatmentRoomID = a.ID,
                        TreatmentRoom = a.Name

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreatmentScheduleBO> GetTherapistDetails()
        {
            List<TreatmentScheduleBO> item = new List<TreatmentScheduleBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetTherapistList().Select(a => new TreatmentScheduleBO
                    {
                        TherapistID = a.ID,
                        TherapistName = a.Name

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreatmentScheduleBO> GetDurationForTreatmentList()
        {
            List<TreatmentScheduleBO> item = new List<TreatmentScheduleBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetDurationForTreatmentList().Select(a => new TreatmentScheduleBO
                    {
                        DurationID = a.ID,
                        Duration = a.Text

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(List<TreatmentScheduleItemBO> Items, string CommaSeparatedScheduleID)
        {

            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter IsTherapistExist = new ObjectParameter("IsTherapistExist", typeof(bool));
                        ObjectParameter IsTreatmentRoomExist = new ObjectParameter("IsTreatmentRoomExist", typeof(bool));
                        ObjectParameter IsPatientExist = new ObjectParameter("IsPatientExist", typeof(bool));
                        ObjectParameter ErrorMessage = new ObjectParameter("ErrorMessage", typeof(string));
                        foreach (var item in Items)
                        {
                            dbEntity.SpCreateTreatmentSchedule(
                                   item.ScheduleID,
                                   item.ScheduledDate,
                                   item.StartTime,
                                   item.DurationID,
                                   item.TreatmentRoomID,
                                   item.TherapistID,
                                   CommaSeparatedScheduleID,
                                   IsTherapistExist,
                                   IsTreatmentRoomExist,
                                   IsPatientExist,
                                   ErrorMessage
                              );
                            if (
                                Convert.ToBoolean(IsTherapistExist.Value) == true ||
                                Convert.ToBoolean(IsTreatmentRoomExist.Value) == true ||
                                Convert.ToBoolean(IsPatientExist.Value) == true
                                )
                            {
                                throw new Exception(Convert.ToString(ErrorMessage.Value));
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    transaction.Commit();
                }
            }
            return 1;
        }

        public DatatableResultBO GetTreatmentScheduleList(string Type,string DateHint, string StartTimeHint, string EndTimeHint, string TreatmentHint, string PatientHint, string DoctorHint, string TherapistHint, string TreatmentRoomHint, string StatusHint,string MedicinesHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetTreatmentScheduleList(Type,DateHint, StartTimeHint, EndTimeHint, TreatmentHint, PatientHint, DoctorHint, TherapistHint, TreatmentRoomHint, StatusHint, MedicinesHint ,SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                Date = ((DateTime)item.ScheduledDate).ToString("dd-MMM-yyyy"),
                                StartTime = item.StartTime,
                                EndTime = item.Endtime,
                                Patient = item.Patient,
                                Doctor = item.Doctor,
                                Treatment = item.Treatment,
                                TreatmentRoom = item.TreatmentRoom,
                                Therapist = item.Therapist,
                                Medicines = item.Medicines,
                                Status = item.Status
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public List<TreatmentScheduleItemBO> GetScheduledTreatmentList(string Type, DateTime? FromDate, int PatientID, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    return dbEntity.SpGetTreatmentSchedules(Type, PatientID, AppointmentProcessID, FromDate, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new TreatmentScheduleItemBO()
                    {
                        TreatmentID = a.TreatmentID,
                        TreatmentName = a.TreatmentName,
                        PatientID = a.PatientID,
                        PatientName = a.PatientName,
                        DoctorID = a.DoctorID,
                        DoctorName = a.DoctorName,
                        ScheduledDate = a.ScheduledDate,
                        NoOfTreatment = (int)a.NoOfTreatment,
                        TreatmentRoom = a.TreatmentRoom,
                        TreatmentRoomID = a.TreatmentRoomID,
                        TherapistID = a.TherapistID,
                        TherapistName = a.TherapistName,
                        PreferedTherapist = a.PreferedTherapist,
                        PreferedTherapistID = a.PreferedTherapistID,
                        PreferedTreatmentRoom = a.PreferedTreatmentRoom,
                        PreferedTreatmentRoomID = a.PreferedTreatmentRoomID,
                        Status = a.Status,
                        DurationID = (int)a.DurationID,
                        Duration = a.Duration,
                        ScheduleID = a.ScheduleID,
                        StartTime = a.StartTime,
                        TotalTreatmentNo = (int)a.TotalTreatmentNo
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
