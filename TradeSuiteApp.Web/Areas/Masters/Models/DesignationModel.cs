using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class DesignationModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public SelectList DepartmentList { get; set; }
        public bool IsActive { get; set; }
        public string State { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}