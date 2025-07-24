using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class PeriodClosingModel
    {
        public int FinYear { get; set; }
        public List<PeriodClosingDays> Items { get; set; }
        public SelectList StatusList { get; set; }
    }
    public class PeriodClosingDays
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