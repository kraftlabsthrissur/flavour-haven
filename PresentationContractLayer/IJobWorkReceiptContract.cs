using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IJobWorkReceiptContract
    {
        int Save(JobWorkReceiptBO jobWorkReceiptBO, List<JobWorkIssuedItemBO> jobWorkIssuedItems, List<JobWorkReceiptItemBO> JobWorkReceiptItems);
        List<JobWorkReceiptBO> GetJobWorkReceipts();
        JobWorkReceiptBO GetJobWorkReceipt(int JobWorkReceiptID);
        List<JobWorkReceiptItemBO> GetJobWorkReceiptItems(int JobWorkReceiptID);
        List<JobWorkIssuedItemBO> GetJobWorkIssuedItems(int JobWorkReceiptID);

    }
}
