using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class MaterialRequirementPlanBO
    {
        public int ID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<MaterialRequirementPlanItemBO> Items { get; set; }

    }

    public class MaterialRequirementPlanItemBO
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
        public DateTime RequiredDate { get; set; }

    }


}
