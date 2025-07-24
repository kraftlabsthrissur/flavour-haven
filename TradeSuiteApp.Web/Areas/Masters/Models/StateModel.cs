using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class StateModel
    {
        public int? ID { get; set; }        
        public string Name { get; set; }
        public string GstState { get; set; }
        public string  Country { get; set; }
        public int CountryID { get; set; }
        public SelectList CountryList { get; set; }
    }
}