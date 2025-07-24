using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class HolidaysModel
    {
        public string LeaveYear { get; set; }
        public string Days { get; set; }
        public int DayID { get; set; }
        public SelectList DayList { get; set; }
        public string GeneralHoliday { get; set; }
        public string WeeklyHoliday { get; set; }
        public DateTime Date { get; set; }
        public string Dates { get; set; }
        public bool IsRestrictedOrOptional { get; set; }
        public string HolidayName { get; set; }
        public string HolidayType { get; set; }
        public IList<SelectListItem> LocationList { get; set; }
        public IList<SelectListItem> WeekList { get; set; }
        public int WeekID { get; set; }
        public string Week { get; set; }
        //public SelectList LocationList { get; set; }
        public string Location { get; set; }
        public int LocationID { get; set; }
        public bool Islocation { get; set; }

        public string LocationCheck
        {
            get
            {
                return Islocation ? "Yes" : "No";
            }

            set
            {
                LocationCheck = value;
            }
        }
        public string HolidayTypes
        {
            get
            {
                return IsRestrictedOrOptional ? "Yes" : "No";
            }

            set
            {
                HolidayType = value;
            }
        }
    }
}