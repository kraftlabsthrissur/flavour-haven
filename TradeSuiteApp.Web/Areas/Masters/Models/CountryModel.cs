using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class CountryModel
    {

        public int? Id { get; set; }       
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsIntraCountry { get; set; }
    }
}