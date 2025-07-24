using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class ConsultationScheduleModel
    {
        public int ID { get; set; }
        public string DoctorName { get; set; }
        public int DoctorID { get; set; }
        public SelectList WeekDayList { get; set; }
        public string WeekDay { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TimeSlot { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsDraft { get; set; }
        public List<ConsultationScheduleItemModel> Items { get; set; }
        public int ConsultationFeeValidity { get; set; }
        public string Time { get; set; }
        public string SlotName { get; set; }
        public int EmployeeCategoryID { get; set; }
    }
    public class ConsultationScheduleItemModel
    {
        public string WeekDay { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}