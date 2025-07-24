using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ApprovalBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TransID { get; set; }
        public string TransNo { get; set; }
        public int ApprovalFlowID { get; set; }
        public bool IsApproved { get; set; }
        public string Status { get; set; }
        public int LastActionUserID { get; set; }
        public int NextActionUserID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ApprovalTypeID { get; set; }
        public string UserName { get; set; }
        public string Requirement { get; set; }
    }
}
