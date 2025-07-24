using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class BinModel
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
        public int? ID { get; set; }
      
        public string BinCode { get; set; }
       // public string Name { get; set; }
       public int WareHouseID {  get; set; }
        public string WarehouseName { get; set; }
        public List<ItemWareHouse> ItemWareHouseList { get; set; }
        public SelectList WareHouseList { get; set; }



    }
}