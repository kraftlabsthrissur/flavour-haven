//file created by prama on 6-6-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class PlacesModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public String State { get; set; }
        public int StateID { get; set; }
        public String District { get; set; }
        public int DistrictID { get; set; }
        public String Country { get; set; }
        public int CountryID { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList DisitrictList { get; set; }


    }
}