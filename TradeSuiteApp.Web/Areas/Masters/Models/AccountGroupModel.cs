using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class AccountGroupModel
    {
        public int ID { get; set; }
        public string AccountGroupName { get; set; }
        public string Code { get; set; }
        public int ParentGroupID { get; set; }
        public string ParentGroup { get; set; }
        public bool IsAllowAccountsUnder { get; set; }
        public string AccountHeadCodePrefix { get; set; }
        public int AccountGroupClassificationID { get; set; }
        public string AccountGroupClassification { get; set; }

        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public int AccountGroupID { get; set; }

        public decimal OpeningAmount { get; set; }
        public string OpeningAmountType { get; set; }
        public SelectList OpeningAmountTypeList { get; set; }
    }
}