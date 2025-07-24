using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class CategoryModel
    {       
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryGroupID { get; set; }
        public Nullable<int> CreatedUserID { get; set; }        
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public SelectList CategoryGroup { get; set; }
        public string GroupName { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string CategoryType { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryName { get; set; }
        public SelectList CategoryTypeList { get; set; }
    }

    public class CategoryGroup
    {
        public long Id { get; set; }
        public long Name { get; set; }

    }
}