using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Linq;

namespace BusinessObject
{
    public static class GeneralBO
    {
        private static int _FinYear = 2022;
        private static int _LocationID = 1;
        private static int _ApplicationID = 1;

        private static int _CreatedUserID = 1;
        private static int _DepartmentID = 0;
        private static string _FinStartDate = "01-04-2022";
        private static string _CompanyName = "VAIDYARATNAM OUSHADHASALA (P) Ltd.";
        private static string _Address1 = ""; // created by lini on 14/05/2018
        private static string _Address2 = "";
        private static string _Address3 = "";
        private static string _Address4 = " "; //"E-mail  :  mail @vaidyaratnammooss.com";
        private static string _Address5 = "Website :   www.vaidyaratnammooss.com";
        private static string AppName = ConfigurationManager.AppSettings["AppName"];
        private static string _ImagePath = "Assets\\img\\_" + AppName + "\\Reportlogo.png";
        private static string _GSTNo = "32AAKFS1090C1ZL";
        private static string _CINNo = " ";
        private static string _EmployeeName = " ";
        private static string _MobileNo = " ";
        private static string _LandLine1 = " ";
        private static string _PIN = " ";
        private static string _ReportLogoPath = "";
        private static string _ReportfooterPath = "";
        private static List<ActionBO> _RolePrivileges;

        private static ClaimsIdentity identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
        //public static int FinYear { get { return _FinYear; } set { _FinYear = value; } }

        // var FinYear = identity.Claims.FirstOrDefault(c => c.Type == "FinYear").Value;

        //public static int FinYear { get { return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "FinYear").Value); } set { _FinYear = value; } }
        //public static int LocationID { get { return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "LocationID").Value); } set { _LocationID = value; } }
        //public static int ApplicationID { get { return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "ApplicationID").Value); } set { _ApplicationID = value; } }
        //public static int CreatedUserID { get { return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "CreatedUserID").Value); } set { _CreatedUserID = value; } }


        //public static int FinYear {
        //    get
        //    {
        //        //if ((HttpContext.Current.Session["FinYear"]) == null)
        //        if (identity.Claims.FirstOrDefault(c => c.Type == "FinYear").Value==null)
        //            {
        //            return Convert.ToInt32(HttpContext.Current.Session["FinYear"] ?? _FinYear);
        //        }
        //        else
        //        {
        //            return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "FinYear").Value);
        //        }

        //    }
        // set { _FinYear = value; } }
        //public static int LocationID
        //{
        //    get
        //    {
        //        //if ((HttpContext.Current.Session["FinYear"]) == null)
        //        if (identity.Claims.FirstOrDefault(c => c.Type == "LocationID").Value == null)
        //        {
        //            return Convert.ToInt32(HttpContext.Current.Session["LocationID"] ?? _LocationID);
        //        }
        //        else
        //        {
        //            return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "LocationID").Value);
        //        }

        //    }
        //    set { _LocationID = value; }
        //}
        //public static int ApplicationID
        //{
        //    get
        //    {
        //        if (identity.Claims.FirstOrDefault(c => c.Type == "ApplicationID").Value == null)
        //        {
        //            return Convert.ToInt32(HttpContext.Current.Session["ApplicationID"] ?? _ApplicationID);
        //        }
        //        else
        //        {
        //            return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "ApplicationID").Value);
        //        }
        //    }
        //    set { _ApplicationID = value; }
        //}
        //public static int CreatedUserID
        //{
        //    get
        //    {
        //        if (identity.Claims.FirstOrDefault(c => c.Type == "CreatedUserID").Value == null)
        //        {
        //            return Convert.ToInt32(HttpContext.Current.Session["UserID"] ?? _CreatedUserID);
        //        }
        //        else
        //        {
        //            return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "CreatedUserID").Value);
        //        }
        //    }
        //    set { _CreatedUserID = value; }
        //}
        //public static int ApplicationID { get { return Convert.ToInt32(HttpContext.Current.Session["ApplicationID"] ?? identity.Claims.FirstOrDefault(c => c.Type == "ApplicationID").Value); } set { _ApplicationID = value; } }
        //public static int LocationID { get { return Convert.ToInt32(HttpContext.Current.Session["CurrentLocationID"] ?? identity.Claims.FirstOrDefault(c => c.Type == "LocationID").Value); } set { _LocationID = value; } }
        //public static int FinYear { get { return (Convert.ToInt32(HttpContext.Current.Session["FinYear"] ?? identity.Claims.FirstOrDefault(c => c.Type == "FinYear").Value)); } set { _FinYear = value; } }
        //public static int ApplicationID { get { return Convert.ToInt32(HttpContext.Current.Session["ApplicationID"] ?? identity.Claims.FirstOrDefault(c => c.Type == "ApplicationID").Value); } set { _ApplicationID = value; } }
        //public static int LocationID { get { return Convert.ToInt32(HttpContext.Current.Session["CurrentLocationID"] ?? identity.Claims.FirstOrDefault(c => c.Type == "LocationID").Value); } set { _LocationID = value; } }

        //public static int FinYear { get { return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "FinYear").Value); } set { _FinYear = value; } }
        //public static int LocationID { get { return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "LocationID").Value); } set { _LocationID = value; } }
        //public static int ApplicationID { get { return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "ApplicationID").Value); } set { _ApplicationID = value; } }
        //public static int CreatedUserID { get { return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "CreatedUserID").Value); } set { _CreatedUserID = value; } }

        public static int LocationID { get { return Convert.ToInt32(HttpContext.Current.Session["CurrentLocationID"] ?? _LocationID); } set { _LocationID = value; } }
        public static int ApplicationID { get { return Convert.ToInt32(HttpContext.Current.Session["ApplicationID"] ?? _ApplicationID); } set { _ApplicationID = value; } }
        public static int FinYear { get { return Convert.ToInt32(HttpContext.Current.Session["FinYear"] ?? _FinYear); } set { _FinYear = value; } }
        public static int CreatedUserID { get { return Convert.ToInt32(HttpContext.Current.Session["UserID"] ?? _CreatedUserID); } set { _CreatedUserID = value; } }

        public static int DepartmentID { get { return Convert.ToInt32(HttpContext.Current.Session["DepartmentID"] ?? _DepartmentID); } set { _DepartmentID = value; } }
        public static string FinStartDate { get { return (HttpContext.Current.Session["FinStartDate"] ?? _FinStartDate).ToString(); } set { _FinStartDate = value; } }
        public static string CompanyName { get { return (HttpContext.Current.Session["CompanyName"] ?? _CompanyName).ToString(); } set { _CompanyName = value; } }
        public static string Address1 { get { return (HttpContext.Current.Session["AddressLine1"] ?? _Address1).ToString() + " "; } set { _Address1 = value; } }
        public static string Address2 { get { return (HttpContext.Current.Session["AddressLine2"] ?? _Address2).ToString() + " "; } set { _Address2 = value; } }
        public static string Address3 { get { return (HttpContext.Current.Session["AddressLine3"] ?? _Address3).ToString() + " "; } set { _Address3 = value; } }
        public static string Address4 { get { return (HttpContext.Current.Session["AddressLine4"] ?? _Address4).ToString() + " "; } set { _Address4 = value; } }
        public static string Address5 { get { return (HttpContext.Current.Session["AddressLine5"] ?? _Address5).ToString() + " "; } set { _Address5 = value; } }
        public static string GSTNo { get { return (HttpContext.Current.Session["GSTNo"] ?? _GSTNo).ToString(); } set { _GSTNo = value; } }
        public static string EmployeeName { get { return (HttpContext.Current.Session["EmployeeName"] ?? _EmployeeName).ToString(); } set { _EmployeeName = value; } }
        public static string MobileNo { get { return (HttpContext.Current.Session["MobileNo"] ?? _MobileNo).ToString(); } set { _MobileNo = value; } }
        public static string CINNo { get { return (HttpContext.Current.Session["CINNo"] ?? _CINNo).ToString(); } set { _CINNo = value; } }
        public static string PIN { get { return (HttpContext.Current.Session["PIN"] ?? _PIN).ToString(); } set { _PIN = value; } }
        public static string LandLine1 { get { return (HttpContext.Current.Session["LandLine1"] ?? _LandLine1).ToString(); } set { _LandLine1 = value; } }
        public static string ReportLogoPath { get { return (HttpContext.Current.Session["ReportLogoPath"] ?? _ReportLogoPath).ToString(); } set { _ReportLogoPath = value; } }
        public static string ReportfooterPath { get { return (HttpContext.Current.Session["ReportfooterPath"] ?? _ReportfooterPath).ToString(); } set { _ReportfooterPath = value; } }

        public static string ImagePath
        {
            get
            {
                if (HttpContext.Current.Session["Logo"]?.ToString() != "")
                {
                    return "Assets\\img\\_" + AppName + "\\" + HttpContext.Current.Session["Logo"]?.ToString();
                }
                else
                {
                    return _ImagePath;
                }

            }
            set
            {
                _ImagePath = value;
            }
        }

    }
}