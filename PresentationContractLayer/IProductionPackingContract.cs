using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IProductionPackingContract
    {
        List<PackingBO> GetProductionPacking(int PackingID);

        List<PackingBO> GetProductionPackingList();

        List<PackingProcessBO> GetPackingIssueProcess(int PackingID);

        List<PackingMaterialBO> GetProductionPackingMaterials(int PackingID);

        List<PackingMaterialBO> GetPackingMaterials(int ItemID, int BatchID, int ProductGroupID, int BatchTypeID, int StoreID);

        bool Save(PackingBO Packing, List<PackingMaterialBO> Materials, List<PackingOutputBO> Items, List<PackingProcessBO> Processes);

        List<PackingOutputBO> GetProductionPackingOutput(int ItemID, int BatchID);

        List<PackingOutputBO> GetProductionPackingOutput(int PackingID);

        int CancelPacking(int PackingID,string Table);

        List<PackingProcessBO> GetPackingProcess(int ItemID, int ProductGroupID, int BatchTypeID);

        List<PackingOutputBO> GetPackingOutputByBatchID(int BatchID);

        DatatableResultBO GetPackingList(string Type,string TransNoHint,string TransDateHint,string ProductionGroupHint,string ItemNameHint,string BatchNoHint,string PackedQtyHint,string SortField,string SortOrder,int Offset,int Limit);

    }
}