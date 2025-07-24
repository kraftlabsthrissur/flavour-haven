using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SerialNumberBO
    {
        public int ID { get; set; }
        public string Code { get; set; }

        
        public string FormName { get; set; }
        public string Field { get; set; }
        public string LocationPrefix { get; set; }
        public string Prefix { get; set; }
        public string SpecialPrefix { get; set; }
        public string FinYearPrefix { get; set; }
        public int Value { get; set; }
        public bool IsLeadingZero { get; set; }
        public int NumberOfDigits { get; set; }
        public string Suffix { get; set; }
        public int LocationID { get; set; }
        
        public int NewFinYear { get; set; }
        public string NewFinPrefix { get; set; }
        public string Location { get; set; }
        public bool IsMaster { get; set; }

    }
}
