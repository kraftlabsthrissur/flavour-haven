using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class StockAdjustmentScheduleBO
    {
        public int ID { get; set; }
        public int ItemCount { get; set; }
        public int TimeLimit { get; set; }
        public int FrequencyOfItem { get; set; }
        public DateTime MorningStartTime { get; set; }
        public DateTime MorningEndTime { get; set; }
        public DateTime EveningStartTime { get; set; }
        public DateTime EveningEndTime { get; set; }
        public string ExcludedDates { get; set; }
        public List<ExcludedDateBO> ExcludedDateList { get; set; }
    }
    public class ExcludedDateBO
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
    }
}
