using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class ApprovalQueueModel
    {
        public int ID { get; set; }
        public string QueueName { get; set; }        
        public string Status { get; set; }
        public string Location { get; set; }
        public SelectList LocationList { get; set; }
        public int LocationID { get; set; }
        public SelectList EmployeeList { get; set; }
        public int EmployeeID { get; set; }
        public int SortOrder { get; set; }
        public List<ApprovalQueueTransModel> QueueTrans { get; set; }
    }
    public class ApprovalQueueTransModel
    {
        public int ID { get; set; }
        public int ApprovalQueueID { get; set; }
        public int UserID { get; set; }
        public String UserName { get; set; }
        public int SortOrder { get; set; }

    }


}