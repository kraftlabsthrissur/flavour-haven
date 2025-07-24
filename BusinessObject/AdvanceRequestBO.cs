using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AdvanceRequestBO
    {
        public int? ID { get; set; }
        public string AdvanceRequestNo { get; set; }
        public DateTime AdvanceRequestDate { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string Status { get; set; }
        public string ItemCatagory { get; set; }
        public decimal TotalAmount { get; set; }
        public string AdvanceCategory { get; set; }
        public bool IsProcessed { get; set; }
        public string ItemName { get; set; }
        public bool IsSuspend { get; set; }
        public int SelectedQuotationID { get; set; }
        public List<AdvanceRequestTransBO> Item { get; set; }

    }
    public class AdvanceRequestTransBO
    {
        public bool IsOfficial { get; set; }
        public int EmployeeID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public int ID { get; set; }
        public string AdvanceRequestNo { get; set; }
        public DateTime AdvanceRequestDate { get; set; }
        public string DateString { get; set; }
        public decimal AdvDetAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
    }
}