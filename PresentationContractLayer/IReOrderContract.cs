using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IReOrderContract
    {
        List<ReOrderItemBO> ReOrderList(int ReOrderDays,int OrderDays,string ItemType);
        string Save(ReOrderBO ReOrder, List<ReOrderItemBO> Items);
        List<CategoryBO> GetReOrderSupplierList(int ItemID);
        ReOrderBO GetReOrderConfigvalues();
    }
}
