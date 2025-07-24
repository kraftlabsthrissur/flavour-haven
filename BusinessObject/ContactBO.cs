using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ContactBO
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string AlternateNo { get; set; }
        public string Designation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Company { get; set; }
        public int CustomerID { get; set; }
        public bool IsActive { get; set; }

    }
}
