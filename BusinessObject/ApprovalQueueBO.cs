using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ApprovalQueueBO
    {
        public int AppQueueID { get; set; }
        public string QueueName { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }

    }

    public class ApprovalQueueTransBO
    {
        public int ID { get; set; }
        public int ApprovalQueueID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int SortOrder { get; set; }
        
    }


}
