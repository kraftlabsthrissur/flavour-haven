using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PreprocessIssueBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int PreProcessIssueID { get; set; }
        public string IssueNo { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsDraft { get; set; }
        public int CreatedUserID { get; set; }
        public string CreatedUser { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsCancelled { get; set; }
        public string ItemName { get; set; }
        public List<PreprocessIssueItemBO> PreprocessIssueItemBOList { get; set; }
    }
    public class PreprocessIssueItemBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public decimal Stock { get; set; }
        public decimal Quantity { get; set; }
        public int BatchID { get; set; }
        public string BatchNo { get; set; }
    }
}
