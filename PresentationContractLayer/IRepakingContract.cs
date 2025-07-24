using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IRepakingContract
    {
        List<RepackingBO> GetRepackingIssue(int id);
        List<ProductionRePackingMaterialItemBO> GetRepackingMaterials(int id);
        List<ProductionREPackingProcesItemBO> GetRepackingProcess(int id);
        List<RepakingPackingOutputBO> GetRepackingOutput(int id);
        List<RepackingBO> GetPackingMaterials(int ItemID, int ProductGroupID, int BatchTypeID, int StoreID, int IssueItemID, decimal QuantityIn, decimal PackedQty, int IssueBatchTypeID, int BatchID);
        List<RepackingBO> GetPackingProcess(int ItemID, int ProductGroupID, int BatchTypeID);
        //bool Save(PackingBO Packing, List<PackingMaterialBO> Materials, List<PackingOutputBO> Items, List<PackingProcessBO> Processes);
        List<RepackingBO> GetRepakingList(int id);
        int Cancel(int ID, string Table);
        bool Save(RepackingBO repacking, List<ProductionRePackingMaterialItemBO> Materials, List<ProductionREPackingProcesItemBO> Processes, List<RepakingPackingOutputBO> Output);

        DatatableResultBO GetRepackingList(string Type, string TransNoHint, string TransDateHint, string ItemNameHint, string BatchNoHint, string BatchTypeHint, string QuantityInHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
