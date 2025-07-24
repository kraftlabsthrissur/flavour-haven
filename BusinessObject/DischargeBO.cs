using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DischargeBO
    {
        public string Course { get; set; }
        public string Condition { get; set; }
        public string Diet { get; set; }
        public string Medicine { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Instructions { get; set; }
        public bool IsDischarged { get; set; }
    }
    public class DischargePatientBO
    {
        public string Patient { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Phone { get; set; }
        public string OPNo { get; set; }
        public string IPNo { get; set; }
        public string Diagnosis { get; set; }
        public DateTime AdmissionDate { get; set; } 
        public DateTime DischargeDate { get; set; }
        public string HIN { get; set; }
    }
    public class DischargeMedicineBO
    {
        public string Medicine { get; set; }
        public string Instructions { get; set; }
        public int NoOfDays { get; set; }
        public string Treatment { get; set; }
        public string Complaint1 { get; set; }
        public string Complaint2 { get; set; }
        public string Doctor { get; set; }
        public string CourseInHospital { get; set; }
    }
    
}
