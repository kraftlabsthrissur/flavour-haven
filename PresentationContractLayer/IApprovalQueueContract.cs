using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PresentationContractLayer
{
    public interface IApprovalQueueContract
    {
        List<ApprovalQueueBO> GetApprovalQueues();

        ApprovalQueueBO GetApprovalQueue(int ID);

        List<ApprovalQueueTransBO> GetApprovalQueueTrans(int ApprovalQueueID);

        int Save(ApprovalQueueBO openingStockBO, List<ApprovalQueueTransBO> openingStockItemsBO);
      
    }
}
