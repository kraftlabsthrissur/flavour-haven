using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class IncentiveCalculationBO
    {
        public int PartyID { get; set; }
        public string PartyName { get; set; }
        public decimal TotalClassicalTarget { get; set; }
        public decimal TotalPatentTarget { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal TotalAchievedClassicalTarget { get; set; }
        public decimal TotalAchievedPatentTarget { get; set; }
        public decimal TotalAchievedTarget { get; set; }
        public decimal TotalAchievedClassicalPercent { get; set; }
        public decimal TotalAchievedPatentPercent { get; set; }
        public decimal TotalAchievedPercent { get; set; }
        public decimal ClassicalIncentiveAmount { get; set; }
        public decimal PatentIncentiveAmount { get; set; }
        public decimal TotalIncentiveAmount { get; set; }
       public decimal IncentiveAboveLimit   { get; set; }
       public decimal TotalEligableAmount { get; set; }

    }
}
