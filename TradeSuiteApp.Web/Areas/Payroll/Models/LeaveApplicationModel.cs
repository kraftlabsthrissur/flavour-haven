using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Payroll.Models
{
    public class LeaveApplicationModel
    {
        public int ID { get; set; }
        public string EmpCategory { get; set; }
        public int EmpCategoryID { get; set; }
        public SelectList EmpCategoryList { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string DepartmentID { get; set; }
        public SelectList DepartmentList { get; set; }
        public string Designation { get; set; }
        public string LeaveType { get; set; }
        public int? LeaveTypeID { get; set; }
        public SelectList LeaveTypeList { get; set; }
        public string Reason { get; set; }
        public string LeaveAvailable { get; set; }
        public string BalanceAvailable { get; set; }
        public string NoOfDays { get; set; }
        public string ReportingToName { get; set; }
        public string LoginName { get; set; }
        public int LoginID { get; set; }
        public SelectList LoginNameList { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public string AMPM { get; set; }
        public bool IsStartForenoon { get; set; }
        public bool IsStartAfterenoon { get; set; }
        public bool IsEndForenoon { get; set; }
        public bool IsEndAfterenoon { get; set; }

        public string StartForenoonCheck
        {
            get
            {
                return IsStartForenoon ? "Forenoon" : " ";
            }

            set
            {
                StartForenoonCheck = value;
            }
        }
        public string StartAfternoonCheck
        {
            get
            {
                return IsStartAfterenoon ? "Afternoon" : " ";
            }

            set
            {
                StartAfternoonCheck = value;
            }
        }

        public string EndForenoonCheck
        {
            get
            {
                return IsEndForenoon ? "Forenoon" : " ";
            }

            set
            {
                EndForenoonCheck = value;
            }
        }
        public string EndAfternoonCheck
        {
            get
            {
                return IsEndAfterenoon ? "Afternoon" : " ";
            }

            set
            {
                EndAfternoonCheck = value;
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return StartDateStr == "" ? null : StartDateStr.ToDateTimes();
            }
            set { StartDateStr = value == null ? "" : ((DateTime)value).ToDateStrs(); }
        }
        public string StartDateStr { get; set; }

        public DateTime? EndDate
        {
            get
            {
                return EndDateStr == "" ? null : EndDateStr.ToDateTimes();
            }
            set { EndDateStr = value == null ? "" : ((DateTime)value).ToDateStrs(); }
        }

        public string EndDateStr { get; set; }
    }
}
public static class LeaveExtensionHelper
{
    public static DateTime? ToDateTimes(this string dateStr, string format = "dd-MM-yyyy")
    {
        //if (string.IsNullOrEmpty(format))
        //format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

        if (!string.IsNullOrEmpty(dateStr))
        {
            return DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
        }
        return null;
    }
    public static string ToDateStrs(this DateTime date, string format = "dd-MM-yyyy")
    {
        if (date != null && date != new DateTime())
        {
            return date.ToString(format);
        }
        //else return new DateTime(1900, 01, 01).ToString(format);
        else return string.Empty;
    }
}

