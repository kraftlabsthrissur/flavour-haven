using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class ProductionPackingBL : IProductionPackingContract
    {
        ProductionPackingDAL productionPackingDAL;
        public ProductionPackingBL()
        {
            productionPackingDAL = new ProductionPackingDAL();
        }
        public List<PackingBO> GetProductionPacking(int PackingID)
        {
            return productionPackingDAL.GetProductionPacking(PackingID);
        }
        public List<PackingBO> GetProductionPackingList()
        {
            return productionPackingDAL.GetProductionPackingList();
        }
        //public List<PackingProcessBO> GetProductionPackingProcess(int ID)
        //{
        //    return productionPackingDAL.GetProductionPackingProcess(ID);
        //}
        public List<PackingMaterialBO> GetProductionPackingMaterials(int PackingID)
        {
            return productionPackingDAL.GetProductionPackingMaterials(PackingID);
        }
        public List<PackingMaterialBO> GetPackingMaterials(int ItemID, int BatchID, int ProductGroupID, int BatchTypeID, int StoreID)
        {
            return productionPackingDAL.GetPackingMaterials(ItemID, BatchID, ProductGroupID, BatchTypeID, StoreID);
        }

        public bool Save(PackingBO Packing, List<PackingMaterialBO> Materials, List<PackingOutputBO> Items, List<PackingProcessBO> Processes)
        {
            return productionPackingDAL.Save(Packing, Materials, Items, Processes);
        }

        public List<PackingOutputBO> GetProductionPackingOutput(int ItemID, int BatchID)
        {
            return productionPackingDAL.GetProductionPackingOutput(ItemID, BatchID);
        }
        public int CancelPacking(int PackingID, string Table)
        {
            return productionPackingDAL.CancelPacking(PackingID, Table);
        }
        public List<PackingProcessBO> GetPackingProcess(int ItemID, int ProductGroupID, int BatchTypeID)
        {
            return productionPackingDAL.GetPackingProcess(ItemID, ProductGroupID, BatchTypeID);

        }

        public List<PackingProcessBO> GetPackingIssueProcess(int PackingID)
        {
            return productionPackingDAL.GetPackingIssueProcess(PackingID);
        }

        public List<PackingOutputBO> GetProductionPackingOutput(int PackingID)
        {
            return productionPackingDAL.GetProductionPackingOutput(PackingID);
        }
        public List<PackingOutputBO> GetPackingOutputByBatchID(int BatchID)
        {
            return productionPackingDAL.GetPackingOutputByBatchID(BatchID);
        }

        public DatatableResultBO GetPackingList(string Type, string TransNoHint, string TransDateHint, string ProductionGroupHint, string ItemNameHint, string BatchNoHint, string PackedQtyHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return productionPackingDAL.GetPackingList(Type, TransNoHint, TransDateHint, ProductionGroupHint, ItemNameHint, BatchNoHint, PackedQtyHint, SortField, SortOrder, Offset, Limit);
        }
    }
}

