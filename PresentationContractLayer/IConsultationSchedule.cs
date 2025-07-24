using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IConsultationScheduleContract
    {

        int Save(ConsultationScheduleBO consultationScheduleBO, List<ConsultationScheduleItemBO> Items);
        List<ConsultationScheduleBO> GetConsultationScheduleDetails(int ID);
        List<ConsultationScheduleItemBO> GetConsultationScheduleItemDetails(int ID);
        DatatableResultBO GetConsultationScheduleList(string Name, string SortField, string SortOrder, int Offset, int Limit);
        List<ConsultationScheduleBO> GetDoctorConsultationSchedule(int doctorID, DateTime scheduleDate, string StartTime, string EndTime);
        List<ConsultationScheduleBO> GetDoctorConsultationTime(int doctorID, DateTime scheduleDate);
    }
}
