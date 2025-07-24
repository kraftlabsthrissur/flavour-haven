using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ConsultationScheduleBO
    {
        public int ID { get; set; }
        public string DoctorName { get; set; }
        public int DoctorID { get; set; }
        public bool IsDraft { get; set; }
        public string WeekDay { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TimeSlot { get; set; }
        public decimal ConsultationFee { get; set; }
        public int ConsultationFeeValidity { get; set; }
        public DateTime Time { get; set; }
        public string SlotName { get; set; }
    }
    public class ConsultationScheduleItemBO
    {
        public string WeekDay { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
