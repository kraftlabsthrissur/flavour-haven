using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PeriodClosingBO
    {
        public int FinYear { get; set; }
        public List<PeriodClosingDaysBO> ItemList { get; set; }
    }

    public class PeriodClosingDaysBO
    {
        public int ID { get; set; }
        public string Month { get; set; }
        public string JournalStatus { get; set; }
        public string SDNStatus { get; set; }
        public string SCNStatus { get; set; }
        public string CDNStatus { get; set; }
        public string CCNStatus { get; set; }
        public int FinYear { get; set; }
    }

}