using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class RoomModel
    {
        public int ID { get; set; }
        public int RoomTypeID { get; set; }
        public int StoreID { get; set; }

        public string Code { get; set; }
        public string RoomName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public string RoomType { get; set; }
        public string Store { get; set; }
        public decimal Rate { get; set; }
        public SelectList RoomTypeList { get; set; }
        public SelectList StoreList { get; set; }
    }
}