using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class DepartmentModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DepartmentGroupID{ get; set; }
        public string DepartmentGroup { get; set; }
        public SelectList DepartmentGroupList { get; set; }
        public bool IsActive { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string State { get; set; }

    }
}