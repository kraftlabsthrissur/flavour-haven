using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class PowerConsumptionModel
    {
        public int ID { get; set; }
        public string LocationName { get; set; }
        public int Location { get; set; }
        public string Time { get; set; }
        public decimal Amount { get; set; }
        public SelectList LocationList { get; set; }
        public List<PowerConsumptionItemModel> Items { get; set; }
    }

    public class PowerConsumptionItemModel
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public string Time { get; set; }
    }
}