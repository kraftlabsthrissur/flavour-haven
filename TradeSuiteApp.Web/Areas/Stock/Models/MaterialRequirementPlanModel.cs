using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Stock.Models
{
    public class MaterialRequirementPlanModel
    {
        public int ID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<MaterialRequirementPlanItemModel> Items { get; set; }
    }

    public class MaterialRequirementPlanItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public decimal RequiredQty { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal QtyInQC { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal RequestedQty { get; set; }
        public string ItemName { get; set; }
        public string RequiredDate { get; set; }

    }
}