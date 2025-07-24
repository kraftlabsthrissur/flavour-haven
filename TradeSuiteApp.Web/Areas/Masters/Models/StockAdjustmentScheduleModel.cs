using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class StockAdjustmentScheduleModel
    {
        public int ID { get; set; }
        public int ItemCount { get; set; }
        public int WarehouseID { get; set; }
        public SelectList Warehouse { get; set; }
        public int TimeLimit { get; set; }
        public int FrequencyOfItem { get; set; }
        public DateTime? MorningStartTime { get { return MorningStartTimeStr.ProcessToTime(); } set { this.MorningStartTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }

        public string MorningStartTimeStr { get; set; }
        public DateTime? MorningEndTime { get { return MorningEndTimeStr.ProcessToTime(); } set { this.MorningEndTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }

        public string MorningEndTimeStr { get; set; }
        public DateTime? EveningStartTime { get { return EveningStartTimeStr.ProcessToTime(); } set { this.EveningStartTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }

        public string EveningStartTimeStr { get; set; }
        public DateTime? EveningEndTime { get { return EveningEndTimeStr.ProcessToTime(); } set { this.EveningEndTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }

        public string EveningEndTimeStr { get; set; }
        public string ExcludedDates { get; set; }
        public List<ExcludedDateModel> ExcludedDateList { get; set; }
    }
    public class ExcludedDateModel
    {
        public int ID { get; set; }
        public string Date { get; set; }
    }
    public static class ExtensionHelper
    {
        public static DateTime? ProcessToTime(this string timeStr)
        {
            if (!string.IsNullOrEmpty(timeStr))
            {
                //   timeStr = "01-01-1900 " + timeStr;
                return DateTime.ParseExact(timeStr, "hh:mm tt", CultureInfo.InvariantCulture);
            }
            return null;
        }
        public static string ToTimeStr(this DateTime date, string format = "hh:mm tt")
        {
            if (date != null && date != new DateTime())
            {
                return date.ToString(format);
            }
            //else return new DateTime(1900, 01, 01).ToString(format);
            else return string.Empty;
        }
    }
}