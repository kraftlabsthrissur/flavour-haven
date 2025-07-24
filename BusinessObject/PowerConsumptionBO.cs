using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class PowerConsumptionBO
    {
        public int ID { get; set; }
        public int Location { get; set; }
        public string LocationName { get; set; }
        public decimal Amount { get; set; }
        public string Time { get; set; }

    }

    public class PowerConsumptionItemBO
    {
        public int ID { get; set; }
        public string LocationName { get; set; }
        public int Location { get; set; }
        public decimal Amount { get; set; }
        public string Time { get; set; }
    }


}
