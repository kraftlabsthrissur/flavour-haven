using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class PatientModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ReferalName { get; set; }
        public string CallerName { get; set; }
        public string Code { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string DOB { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Place { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string MobileNo { get; set; }
        public string ReferalContactNo { get; set; }
        public string PinCode { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public int LocationID { get; set; }
        public int PatientReferedByID { get; set; }
        public SelectList PatientReferedList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList DistrictList { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList PatientSexList { get; set; }

    }
}