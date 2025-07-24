using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SerialNumberModel
    {
        public int ID { get; set; }
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
        public string LocationName { get; set; }
        public int FinYear { get; set; }
        public bool IsMaster { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList MasterList { get; set; }

    }
    public class NewFinYearModel
    {
        public int NewFinYear { get; set; }
        public string NewFinPrefix { get; set; }
        public List<SerialNumberModel> Trans { get; set; }
    }
}