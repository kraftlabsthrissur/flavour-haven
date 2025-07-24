using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class LeaveTypeModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? CreditFrequecyID { get; set; }
        public string CreditFrequecy { get; set; }
        public int? NoOfDays { get; set; }
        public bool IsEncashable { get; set; }
        public bool CanCarryForward { get; set; }
        public int? MinServicePeriod { get; set; }
        public int? MaxLeaveBalance { get; set; }
        public int? MinInstanceDays { get; set; }
        public int? MaxInstanceDays { get; set; }
        public bool IsLeavePartOfWeeklyOff { get; set; }
        public bool IsLeavePartOfHoliday { get; set; }

       

        public SelectList CreditFrequecyList { get; set; }

        public string EncashResult
        {
            get
            {
                return IsEncashable ? "Yes" : "No";
            }

            set
            {
                EncashResult = value;
            }
        }

        //public string Encash
        //{
        //    get
        //    {
        //        return IsEncashable ? "Checked" : " ";
        //    }

        //    set
        //    {
        //        Encash = value;
        //    }
        //}

        public string CanCarryResult
        {
            get
            {
                return CanCarryForward ? "Yes" : "No";
            }

            set
            {
                CanCarryResult = value;
            }
        }

        //public string CanCarry
        //{
        //    get
        //    {
        //        return CanCarryForward ? "Checked" : " ";
        //    }

        //    set
        //    {
        //        CanCarry = value;
        //    }
        //}
    }
}