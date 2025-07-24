using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class ChartOfAccountsModel
    {
        public int ID { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public int ParentID { get; set; }
        public int Level { get; set; }
        public decimal OpeningAmount { get; set; }
        public bool IsManual { get; set; }
        public string ParentName { get; set; }
        public List<ChartOfAccountBO> AccountList { get; set; }
        public string ParentAccountCode { get; set; }
    }
}