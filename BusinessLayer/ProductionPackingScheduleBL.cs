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
    public class ProductionPackingScheduleBL : IProductionPackingScheduleContract
    {
        ProductionPackingScheduleDAL packingScheduleDAL;
        public ProductionPackingScheduleBL()
        {
            packingScheduleDAL = new ProductionPackingScheduleDAL();
        }

        public bool Save(PackingBO Packing, List<PackingMaterialBO> Materials)
        {
            if (Packing.ID>0)
            {
                return packingScheduleDAL.Update(Packing, Materials);
            }
            else
            {
                return packingScheduleDAL.Save(Packing, Materials);
            }
            
        }

        public List<PackingBO> GetPackingScheduleList()
        {
            return packingScheduleDAL.GetPackingScheduleList();
        }

        public List<PackingMaterialBO> GetPackingScheduleItems(int ProductionScheduleID)
        {
            return packingScheduleDAL.GetPackingScheduleItems(ProductionScheduleID);
        }
        public PackingBO GetPackingSchedule(int PackingScheduleID)
        {
            return packingScheduleDAL.GetPackingSchedule(PackingScheduleID);
        }

        public DatatableResultBO GetPackingScheduleList(string Type, string TransNoHint, string TransDateHint, string ItemNameHint, string BatchNoHint, string BatchTypeHint, string PackedQtyHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return packingScheduleDAL .GetPackingScheduleList(Type, TransNoHint, TransDateHint, ItemNameHint, BatchNoHint, BatchTypeHint, PackedQtyHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
