using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObject;


namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class RoleModel
    {
        public string Code { get; set; }
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string Remarks { get; set; }
        public string ActionNames { get; set; }
        public string Tabs { get; set; }

        public List<ActionBO> Actions { get; set; }
        public List<ActionBO> Areas { get; set; }
        public List<ActionBO> Controllers { get; set; }
        public List<ActionBO> Name { get; set; }
        public List<ActionIDModel> ActionID { get; set; }
        public List<ActionIDModel> TabID { get; set; }
    }
    public class ActionIDModel
    {
        public int ID { get; set; }
    }


}