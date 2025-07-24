using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class WareHouseModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public int? ItemTypeID { get; set; }
        public string Remarks { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string ItemTypeName { get; set; }
        public SelectList ItemTypeGroup { get; internal set; }
        public SelectList LocationList { get; set; }
    }
    public class ItemTypeGroup
    {
        public int ItemTypeID { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }
}