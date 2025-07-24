using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PackingMaterialBO : ItemBO
    {
        public int PackingIssueID { get; set; }
        public decimal StandardQty { get; set; }
        public int? BatchID { get; set; }
        public int? StoreID { get; set; }
        public string WarehouseName { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal IssueQty { get; set; }
        public bool IsCompleted { get; set; }    
        public decimal AverageRate { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Variance { get; set; }
        public decimal ActualQty { get; set; }
        public int PackingMaterialMasterID { get; set; }
        public bool IsAdditionalIssue { get; set; }
        public bool IsMaterialReturn { get; set; }

    }
}
