using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SecondaryUnitModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public string UnitGroupName { get; set; }
        public int UnitGroupID { get; set; }
        public decimal PackSize { get; set; }
        public int? CreatedUserID { get; set; }
        public SelectList UnitList { get; set; }
        public SelectList UnitGroupList { get; set; }
    }
}