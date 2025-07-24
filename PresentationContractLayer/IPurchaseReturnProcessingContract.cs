using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PresentationContractLayer
{
   public interface IPurchaseReturnProcessingContract
    {
        List<PurchaseReturnProcessingItemBO> GetPurchaseReturnProcessingItem(string ProcessingType, DateTime FromTransactionDate, DateTime ToTransactionDate, DateTime AsOnDate,int Days);
        int Save(List<PurchaseReturnProcessingItemBO> Items);


    }
}
