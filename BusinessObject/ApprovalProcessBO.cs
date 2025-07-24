using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ApprovalProcessBO
    {
        public int ID { get; set; }
        public int ApprovalID { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveForUser { get; set; }
        public DateTime Date { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string Requirement { get; set; }
        public bool IsApproved { get; set; }
        public decimal? SortOrder { get; set; }
        public int NextActionUserID { get; set; }
    }
}
