using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Models
{
    public class MenuItemModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ActionID { get; set; }
        public int ParentID { get; set; }
        public int SortOrder { get; set; }
        public string IconClass { get; set; }
        public int BaseParentID { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string URL { get; set; }
        public string ReportURL { get; set; }
    }
}