using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class DACalculationsModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public SelectList LocationList { get; set; }
        public int EmployeeCategoryID { get; set; }
        public string EmployeeCategory { get; set; }
        public SelectList EmployeeCategoryList { get; set; }
        public int EmployeeStatusID { get; set; }
        public string EmployeeStatus { get; set; }
        public SelectList EmployeeStatusList { get; set; }
        public int PayrollCategoryID { get; set; }
        public string PayrollCategory { get; set; }
        public SelectList PayrollCategoryList { get; set; }
        public int DepartmentID { get; set; }
        public string Department { get; set; }
        public SelectList DepartmentList { get; set; }
        public string Month { get; set; }
        public int  BasicPoints { get; set; }
        public int AdditionalPoints { get; set; }
        public int BasicValue { get; set; }
        public int AdditionalValue { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }

        public DateTime? StartDate
        {
            get
            {
                return StartDateStr == "" ? null : StartDateStr.ToDateTime();
            }
            set { StartDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public string StartDateStr { get; set; }

        public DateTime? EndDate
        {
            get
            {
                return EndDateStr == "" ? null : EndDateStr.ToDateTime();
            }
            set { EndDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }

        public string EndDateStr { get; set; }
    }
}
public static class ExtensionHelper
{
    public static DateTime? ToDateTime(this string dateStr, string format = "dd-MM-yyyy")
    {
        //if (string.IsNullOrEmpty(format))
        //format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

        if (!string.IsNullOrEmpty(dateStr))
        {
            return DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
        }
        return null;
    }
    public static string ToDateStr(this DateTime date, string format = "dd-MM-yyyy")
    {
        if (date != null && date != new DateTime())
        {
            return date.ToString(format);
        }
        //else return new DateTime(1900, 01, 01).ToString(format);
        else return string.Empty;
    }
}