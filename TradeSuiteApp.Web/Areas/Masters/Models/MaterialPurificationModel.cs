using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class MaterialPurificationModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int PurificationItemID { get; set; }
        public int PurificationUnitID { get; set; }
        public string PurificationItemName { get; set; }
        public string PurificationUnit { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList MaterialPurificationProcessList { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public int ItemCategoryID { get; set; }
        public string CategoryName { get; set; }
    }

}