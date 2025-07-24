using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PackingOutputBO
    {
        public int ItemID { get; set; }
        public int PackingIssueID { get; set; }
        public string ItemName { get; set; }
        public string BatchNo { get; set; }
        public decimal PackedQty { get; set; }
        public string BatchType { get; set; }
        public int BatchTypeID { get; set; }
        public int BatchID { get; set; }
        public int StoreID { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int ProductionSequence { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDraft { get; set; }
        public bool IsQCCompleted { get; set; }     
        public DateTime? Date { get; set; }
    }
}
