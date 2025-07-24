using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPurchaseReturn
    {
        int SavePurchaseReturn(PurchaseReturnBO PurchaseReturnBO, int createdUserID, int finYear, int locationID, int applicationID);

    }
}
