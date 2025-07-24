using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class BillTypeModel
    {
        public int BillTypeID { get; set; }
        public string BillTypeName{ get; set; }
        public string Type { get; set; }
        public List<BillTypeItemModel> OPItems { get; set; }
        public List<BillTypeItemModel> IPItems { get; set; }
    }
    public class BillTypeItemModel
    {
        public int BillTypeID { get; set; }
        public string BillTypeName { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
    }
}