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
    public class ConsultationScheduleDAL
    {
        public int Save(ConsultationScheduleBO consultationScheduleBO, List<ConsultationScheduleItemBO> Items)
        {

            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter returnValue = new ObjectParameter("ReturnValue", typeof(int));
                        ObjectParameter retValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter errorValue = new ObjectParameter("ErrorValue", typeof(int));
                        dbEntity.SpCreateConsultationSchedule(
                            consultationScheduleBO.DoctorID,
                            consultationScheduleBO.TimeSlot,
                            consultationScheduleBO.IsDraft,
                            consultationScheduleBO.ConsultationFeeValidity,
                            consultationScheduleBO.ConsultationFee,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            returnValue,
                            retValue
                        );
                        dbEntity.SaveChanges();
                        foreach (var item in Items)
                        {

                            dbEntity.SpCreateConsultationScheduleItem(
                                Convert.ToInt32(returnValue.Value),
                                item.WeekDay,
                                item.StartTime,
                                item.EndTime,
                                errorValue
                                );

                        }
                        if (Convert.ToInt32(retValue.Value) == -1)
                        {
                            throw new Exception("Doctor's schedule already saved");
                        }
                        if (Convert.ToInt32(errorValue.Value) == -2)
                        {
                            throw new Exception("Please enter correct time-slot");
                        }
                        transaction.Commit();
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return 1;
        }

        public List<ConsultationScheduleBO> GetConsultationScheduleDetails(int ID)
        {
            List<ConsultationScheduleBO> consultationSchedule = new List<ConsultationScheduleBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                consultationSchedule = dEntity.SpGetConsultationScheduleDetails(ID, GeneralBO.FinYear, GeneralBO.ApplicationID, GeneralBO.ApplicationID).Select(a => new ConsultationScheduleBO
                {
                    ID = a.ID,
                    DoctorID = (int)a.DoctorID,
                    DoctorName = a.Name,
                    TimeSlot = (int)a.TimeSlot,
                    ConsultationFeeValidity = (int)a.ConsultationFeeValidity,
                    ConsultationFee = (decimal)a.ConsultationFee
                }).ToList();
                return consultationSchedule;
            }

        }

        public List<ConsultationScheduleItemBO> GetConsultationScheduleItemDetails(int ID)
        {
            try
            {
                List<ConsultationScheduleItemBO> ItemList = new List<ConsultationScheduleItemBO>();
                using (MasterEntities dEntity = new MasterEntities())
                {
                    ItemList = dEntity.SpGetConsultationScheduleItemDetails(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ConsultationScheduleItemBO
                    {
                        WeekDay = a.WeekDays,
                        EndTime = a.EndTime,
                        StartTime = a.StartTime

                    }).ToList();
                    return ItemList;
                }
            }
            catch (Exception e)
            {               
                throw e;
            }

        }
        public int Update(ConsultationScheduleBO consultationScheduleBO, List<ConsultationScheduleItemBO> Items)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        ObjectParameter errorValue = new ObjectParameter("ErrorValue", typeof(int));
                        var i = dbEntity.SpUpdateConsultationSchedule(consultationScheduleBO.ID, consultationScheduleBO.DoctorID, consultationScheduleBO.TimeSlot, 
                            consultationScheduleBO.IsDraft, consultationScheduleBO.ConsultationFeeValidity,consultationScheduleBO.ConsultationFee,
                            GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);

                        dbEntity.SaveChanges();

                        foreach (var itm in Items)
                        {

                            dbEntity.SpCreateConsultationScheduleItem(
                                Convert.ToInt32(consultationScheduleBO.ID),
                                itm.WeekDay,
                                itm.StartTime,
                                itm.EndTime,
                                errorValue
                                );
                        }
                        if (Convert.ToInt32(errorValue.Value) == -2)
                        {
                            throw new Exception("Please enter correct time-slot/start-end dates");
                        }
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }
        public DatatableResultBO GetConsultationScheduleList(string Name, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetConsultationScheduleList(Name, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                DoctorName = item.DoctorName
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
        public List<ConsultationScheduleBO> GetDoctorConsultationSchedule(int doctorID, DateTime scheduleDate, string StartTime, string EndTime)
        {
            try
            {
                List<ConsultationScheduleBO> consultationSchedule = new List<ConsultationScheduleBO>();
                using (MasterEntities dEntity = new MasterEntities())
                {
                    consultationSchedule = dEntity.SpGetDoctorConsultationSchedule(doctorID, scheduleDate, StartTime, EndTime, GeneralBO.FinYear, GeneralBO.ApplicationID, GeneralBO.ApplicationID).Select(a => new ConsultationScheduleBO
                    {
                        SlotName = a.SlotName,
                        Time = (DateTime)a.Time
                    }).ToList();
                    return consultationSchedule;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public List<ConsultationScheduleBO> GetDoctorConsultationTime(int doctorID, DateTime scheduleDate)
        {
            try
            {
                List<ConsultationScheduleBO> consultationSchedule = new List<ConsultationScheduleBO>();
                using (MasterEntities dEntity = new MasterEntities())
                {
                    consultationSchedule = dEntity.SpGetDoctorConsultationTime(doctorID, scheduleDate, GeneralBO.FinYear, GeneralBO.ApplicationID, GeneralBO.ApplicationID).Select(a => new ConsultationScheduleBO
                    {
                        StartTime = a.StartTime,
                        EndTime = a.EndTime
                    }).ToList();
                    return consultationSchedule;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
