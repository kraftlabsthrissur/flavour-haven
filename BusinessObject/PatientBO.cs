using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class PatientBO
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public string Code { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public DateTime? DOB { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Place { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string PinCode { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
    }
}
