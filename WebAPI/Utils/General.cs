using Microsoft.Diagnostics.Instrumentation.Extensions.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.Utils
{
    public class General
    {
        private static string DateFormatForView = "dd-MMM-yyyy";
        private static string DateFormatForEdit = "dd-MM-yyyy";
        private static string DateTimeFormatForEdit = "dd-MM-yyyy hh:mm tt";
        private static string DateTimeFormatForView = "dd-MMM-yyyy hh:mm tt";

        public static DateTime ToDateTime(string date, string type = "date")
        {
            string format = DateFormatForEdit;
            if (type == "datetime")
            {
                format = DateTimeFormatForEdit;
            }
         
            try
            {

                var r = DateTime.ParseExact(
                    s: date,
                    format: format,
                    provider: null);
                return r;
            }
            catch (FormatException e)
            {
                throw e;
            }

        }

        public static DateTime? ToDateTimeNull(string date, string type = "date")
        {
            string format = DateFormatForEdit;
            if (type == "datetime")
            {
                format = DateTimeFormatForEdit;
            }

            try
            {
                var r = DateTime.ParseExact(
                    s: date,
                    format: format,
                    provider: null);
                return r;
            }
            catch (FormatException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static string FormatDateNull(DateTime? date, string mode = "edit")
        {
            if (date == null) {
                return "";
            }
            if (mode.Equals("view"))
            {
                return ((DateTime)date).ToString(DateFormatForView);
            }
            else
            {
                return ((DateTime)date).ToString(DateFormatForEdit);
            }

        }

        public static string FormatDate(DateTime date, string mode = "edit")
        {
            if (mode.Equals("view"))
            {
                return date.ToString(DateFormatForView);
            }
            else
            {
                return date.ToString(DateFormatForEdit);
            }

        }

        public static string FormatDateTime(DateTime date, string mode = "edit")
        {
            if (mode.Equals("view"))
            {
                return date.ToString(DateTimeFormatForView);
            }
            else
            {
                return date.ToString(DateTimeFormatForEdit);
            }

        }

        public static string FormatDateTimeNull(DateTime? date, string mode = "edit")
        {
            if (date == null)
            {
                return "";
            }
            if (mode.Equals("view"))
            {
                return ((DateTime)date).ToString(DateTimeFormatForView);
            }
            else
            {
                return ((DateTime)date).ToString(DateTimeFormatForEdit);
            }

        }

        public static string Split(string s)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(s, " ");
        }

        public static DateTime FirstDayOfMonth
        {
            get
            {
                DateTime Today = DateTime.Today;
                return new DateTime(Today.Year, Today.Month, 1);
            }
        }

    }
}