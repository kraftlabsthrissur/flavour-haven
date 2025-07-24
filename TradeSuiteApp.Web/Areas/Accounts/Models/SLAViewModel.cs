using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class SLAViewModel
    {
        public List<SLAValues> SLAValues { get; set; }
        public List<SLAToBePosted> SLAToBePosted { get; set; }
        public List<SLAPosted> SLAPosted{ get; set; }
        public List<SLAError> SLAError { get; set; }
        public string TransactionType { get; set; }
    }
}