using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Admin.Models
{
    public class AppModel
    {
        public List<EnabledItemModel> enable { get; set; }
       
    }

    public class EnabledItemModel
    {
        public int ID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}