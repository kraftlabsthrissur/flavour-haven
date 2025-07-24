using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BusinessLayer
{
    public class ApprovalQueueBL : IApprovalQueueContract
    {
        ApprovalQueueDAL approvalQueue;

        public ApprovalQueueBL()
        {
            approvalQueue = new ApprovalQueueDAL();
        }


        public ApprovalQueueBO GetApprovalQueue(int ID)
        {
            return approvalQueue.GetApprovalQueue(ID);
        }

        public List<ApprovalQueueBO> GetApprovalQueues()
        {
            return approvalQueue.GetApprovalQueues();
        }

        public List<ApprovalQueueTransBO> GetApprovalQueueTrans(int ApprovalQueueID)
        {
            return approvalQueue.GetApprovalQueueTrans(ApprovalQueueID);
        }
        public int Save(ApprovalQueueBO ApprovalQueueItem, List<ApprovalQueueTransBO> ApprovalQueueItems) {

            if (ApprovalQueueItem.AppQueueID > 0)
            {
                return approvalQueue.Update(ApprovalQueueItem, ApprovalQueueItems);
            }
            else {
                return approvalQueue.Save(ApprovalQueueItem, ApprovalQueueItems);
            }
            
        }
    }
}
