using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System.Collections.Generic;

namespace BusinessLayer
{
   public class JobWorkReceiptBL: IJobWorkReceiptContract
    {
        JobWorkReceiptDAL jobWorkReceiptDAL;

        public JobWorkReceiptBL()
        {
            jobWorkReceiptDAL = new JobWorkReceiptDAL();
        }
        public int Save(JobWorkReceiptBO jobWorkReceiptBO, List<JobWorkIssuedItemBO> jobWorkIssuedItems, List<JobWorkReceiptItemBO> JobWorkReceiptItems)
        {
            if (jobWorkReceiptBO.ID == 0)
             {
            return jobWorkReceiptDAL.Save(jobWorkReceiptBO, jobWorkIssuedItems, JobWorkReceiptItems);
             }
             else
            {
            return jobWorkReceiptDAL.Update(jobWorkReceiptBO, jobWorkIssuedItems, JobWorkReceiptItems);
             }

        }

        public List<JobWorkReceiptBO> GetJobWorkReceipts()
        {
            return jobWorkReceiptDAL.GetJobWorkReceipts();
        }
        public JobWorkReceiptBO GetJobWorkReceipt(int JobWorkReceiptID)
        {
            return jobWorkReceiptDAL.GetJobWorkReceipt(JobWorkReceiptID);
        }
        public List<JobWorkIssuedItemBO> GetJobWorkIssuedItems(int JobWorkReceiptID)
        {
            return jobWorkReceiptDAL.GetJobWorkIssuedItems(JobWorkReceiptID);
        }
        public List<JobWorkReceiptItemBO> GetJobWorkReceiptItems(int JobWorkReceiptID)
        {
            return jobWorkReceiptDAL.GetJobWorkReceiptItems(JobWorkReceiptID);
        }

    }
}
