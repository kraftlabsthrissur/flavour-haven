using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class DischargeModel
    {
        public int IPID { get; set; }
        public string Course { get; set; }
        public string Condition { get; set; }
        public string Diet { get; set; }
        public bool IsDischarged { get; set; }
        public List<DischargeModelItems> MedicineList { get; set; }
    }
    public class DischargeModelItems
    {
        public string Medicine { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Instructions { get; set; }
    }
}