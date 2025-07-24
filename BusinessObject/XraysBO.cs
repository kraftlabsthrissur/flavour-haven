using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class XraysBO
    {
        public string Date { get; set; }
        public string Doctor { get; set; }
        public string PatientCode { get; set; }
        public string Patient { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public int PatientID{ get; set; }
    }

    public class XraysItemBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int DocumentID { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Path { get; set; }
    }
}
