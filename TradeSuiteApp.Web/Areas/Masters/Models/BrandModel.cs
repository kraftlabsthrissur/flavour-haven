using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class BrandModel
    {

        //public int? Id { get; set; }       
        //public string Code { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string CountryName { get; set; }
        //public int CountryID { get; set; }
        //public int DecimalPlaces { get; set; }
        //public string MinimumCurrency { get; set; }
        //public string MinimumCurrencyCode { get; set; }
        public int? Id { get; set; }
        public int? BrandId { get; set; }
        public string Code { get; set; }
        public string BrandName { get; set; }
        public string Path { get; set; }
        public string image { get; set; }


    }
}