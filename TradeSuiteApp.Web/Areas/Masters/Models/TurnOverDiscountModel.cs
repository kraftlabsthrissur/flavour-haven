using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class TurnOverDiscountModel
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public decimal Amount { get; set; }
        public string CustomerName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public SelectList LocationList { get; set; }
        public int CustomerID { get; set; }
        public string Code { get; set; }
        public SelectList MonthList { get; set; }
        public int MonthID { get; set; }

        public List<DiscountItemModel> Items { get; set; }
    }

    public class DiscountItemModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal TurnOverDiscount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Location { get; set; }
        public string Month { get; set; }
    }
}