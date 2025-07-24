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
    public class ConsultationScheduleBL : IConsultationScheduleContract
    {
        ConsultationScheduleDAL scheduleDAL;
        public ConsultationScheduleBL()
        {
            scheduleDAL = new ConsultationScheduleDAL();
        }

        public int Save(ConsultationScheduleBO appointmentScheduleBO, List<ConsultationScheduleItemBO> Items)
        {
            if (appointmentScheduleBO.ID > 0)
            {
                return scheduleDAL.Update(appointmentScheduleBO, Items);
            }
            else
            {
                return scheduleDAL.Save(appointmentScheduleBO, Items);
            }
        }
        public List<ConsultationScheduleBO> GetConsultationScheduleDetails(int ID)
        {
            return scheduleDAL.GetConsultationScheduleDetails(ID);
        }
        public List<ConsultationScheduleItemBO> GetConsultationScheduleItemDetails(int ID)
        {
            return scheduleDAL.GetConsultationScheduleItemDetails(ID);
        }
        public DatatableResultBO GetConsultationScheduleList(string Name, string SortField, string SortOrder, int Offset, int Limit)
        {
            return scheduleDAL.GetConsultationScheduleList(Name, SortField, SortOrder, Offset, Limit);
        }
        public List<ConsultationScheduleBO> GetDoctorConsultationSchedule(int doctorID, DateTime scheduleDate, string StartTime, string EndTime)
        {
            return scheduleDAL.GetDoctorConsultationSchedule(doctorID, scheduleDate, StartTime, EndTime);
        }
        public List<ConsultationScheduleBO> GetDoctorConsultationTime(int doctorID, DateTime scheduleDate)
        {
            return scheduleDAL.GetDoctorConsultationTime(doctorID, scheduleDate);
        }
    }
}
