using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class DistrictModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> StateID { get; set; }
        public SelectList States { get; set; }
        public string StateName { get; set; }
        public Nullable<int> PIN { get; set; }
        public string OfficeName { get; set; }
        public string Taluk { get; set; }

        //public virtual State State { get; set; }
    }
}