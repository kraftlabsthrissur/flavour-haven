using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class PaymentGroupModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int PaymentWeek { get; set; }
    }
}