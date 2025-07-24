using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class ConfigurationModel
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public int UserID { get; set; }
        public int LocationID { get; set; }
    }
}