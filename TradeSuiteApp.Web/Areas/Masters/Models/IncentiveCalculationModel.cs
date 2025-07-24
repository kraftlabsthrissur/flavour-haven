using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class IncentiveCalculationModel
    {
        public  int ID { get; set; }
        public SelectList DurationList { get; set; }
        public SelectList TimePeriodList { get; set; }
        public SelectList PartyList { get; set; }
        public int DurationID { get; set; }
        public int TimePeriodID { get; set; }
        public string Party { get; set; }

    }
}