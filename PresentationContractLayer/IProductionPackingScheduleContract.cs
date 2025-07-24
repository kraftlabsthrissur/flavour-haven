using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IProductionPackingScheduleContract
    {
        bool Save(PackingBO Packing, List<PackingMaterialBO> Materials);
        List<PackingMaterialBO> GetPackingScheduleItems(int ProductionScheduleID);
        PackingBO GetPackingSchedule(int PackingScheduleID);
        DatatableResultBO GetPackingScheduleList(string Type, string TransNoHint, string TransDateHint, string ItemNameHint, string BatchNoHint, string BatchTypeHint, string PackedQtyHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
