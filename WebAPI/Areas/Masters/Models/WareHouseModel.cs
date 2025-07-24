using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Areas.Masters.Models
{
    public class WareHouseModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public int? ItemTypeID { get; set; }
        public string Remarks { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string ItemTypeName { get; set; }
    }
}