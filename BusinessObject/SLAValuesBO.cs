using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SLAValuesBO
    {
        public string TransationType { get; set; }
        public string KeyValue { get; set; }
        public decimal Amount { get; set; }
        public string Event { get; set; }
        public int TransationID { get; set; }
        public DateTime Date { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public int TablePrimaryID { get; set; }
        public string DocumentTable { get; set; }
        public string DocumentNo { get; set; }
    }
}
