using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SalesRepresentativeModel
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public bool IsSubLevel { get; set; }
        public string FSOName { get; set; }
        public string Area { get; set; }
        public int DesignationID { get; set; }
        public int AreaID { get; set; }
        public int EmployeeID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public string Designation { get; set; }
        public List<SalesRepresentativeListModel> SalesRepresentatives { get; set; }
        public SelectList DesignationList { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList AreaList { get; set; }
    }
    public class SalesRepresentativeListModel
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public bool IsSubLevel { get; set; }
        public string FSOName { get; set; }
        public int DesignationID { get; set; }
        public string Designation { get; set; }
        public int AreaID { get; set; }
        public int EmployeeID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public bool IsChild { get; set; }
        public string Area { get; set; }
    }
}