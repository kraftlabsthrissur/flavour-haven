using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class AccountHeadModel
    {
        public int ID { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string GroupClassification { get; set; }
        public decimal OpeningAmt { get; set; }
    }
}