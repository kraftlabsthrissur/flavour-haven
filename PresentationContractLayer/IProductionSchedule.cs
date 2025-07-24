using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IProductionSchedule
    {
        List<KeyValuePair<int, string>> GetProductionGroups(String ItemHind);
        List<ProductionScheduleItemBO> GetProductionIssueItemsByProductionGroup(int productionGroupID);
        int Save(ProductionScheduleBO productionScheduleBO);
        DatatableResultBO GetProductionScheduleList(string Type, string TransNoHint, string TransDateHint, string ProductionGroupHint, string StartDateHint, string BatchsizeHint ,string BatchHint, string SortField, string SortOrder, int Offset, int Limit);
        List<ProductionScheduleBO> GetProductionSchedulesByItem(int ItemID);
        ProductionScheduleBO GetProductionSchedule(int productionScheduleID);
        int Cancel(int ID,string Table);
    }
}
