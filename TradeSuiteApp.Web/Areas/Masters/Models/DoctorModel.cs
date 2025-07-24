using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class DoctorModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Place { get; set; }
        public string Qualification { get; set; }
    }
}