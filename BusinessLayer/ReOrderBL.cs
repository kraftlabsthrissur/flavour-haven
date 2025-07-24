using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
   public class ReOrderBL: IReOrderContract
    {

        ReOrderDAL reorderDAL;
        public ReOrderBL()
        {
            reorderDAL = new ReOrderDAL();
        }
        public List<ReOrderItemBO> ReOrderList(int ReOrderDays, int OrderDays, string ItemType)
        {
            return reorderDAL.ReOrderList(ReOrderDays, OrderDays, ItemType);
        }
        public string Save(ReOrderBO ReOrder,List<ReOrderItemBO> Items)
        {
            return reorderDAL.Save(ReOrder, Items);
        }
        public List<CategoryBO> GetReOrderSupplierList(int ItemID)
        {
            return reorderDAL.GetReOrderSupplierList(ItemID);
        }
        public ReOrderBO GetReOrderConfigvalues()
        {
            return reorderDAL.GetReOrderConfigvalues();
        }
    }
}
