using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;


namespace BusinessLayer
{
    public class RepackingBL : IRepakingContract
    {
        RepackingDAL repackingDal;
        public List<RepackingBO> GetRepackingIssue(int id)
        {
            return repackingDal.GetRepackingIssue(id);
        }
        public List<ProductionRePackingMaterialItemBO> GetRepackingMaterials(int id)
        {
            return repackingDal.GetRepackingMaterials(id);
        }
        public List<ProductionREPackingProcesItemBO> GetRepackingProcess(int id)
        {
            return repackingDal.GetRepackingProcess(id);
        }
        public List<RepakingPackingOutputBO> GetRepackingOutput(int id)
        {
            return repackingDal.GetRepackingOutput(id);
        }
        public RepackingBL()
        {
            repackingDal = new RepackingDAL();
        }
        public List<RepackingBO> GetPackingMaterials(int ItemID, int ProductGroupID, int BatchTypeID, int StoreID, int IssueItemID, decimal QuantityIn, decimal PackedQty, int IssueBatchTypeID, int BatchID)
        {
            return repackingDal.GetPackingMaterials(ItemID, ProductGroupID, BatchTypeID, StoreID, IssueItemID, QuantityIn, PackedQty, IssueBatchTypeID, BatchID);
        }
        public List<RepackingBO> GetPackingProcess(int ItemID, int ProductGroupID, int BatchTypeID)
        {
            return repackingDal.GetPackingProcess(ItemID, ProductGroupID, BatchTypeID);

        }
        public bool Save(RepackingBO repacking, List<ProductionRePackingMaterialItemBO> Materials, List<ProductionREPackingProcesItemBO> Processes, List<RepakingPackingOutputBO> Output)
        {
            if (repacking.ID > 0)
            {
                return repackingDal.Update(repacking, Materials, Processes, Output);

            }
            else
            {
                return repackingDal.Save(repacking, Materials, Processes, Output);
            }
        }
        public List<RepackingBO> GetRepakingList(Int32 ID)
        {
            return repackingDal.GetRepakingList(ID);
        }
        public int Cancel(int ID, string Table)
        {
            return repackingDal.Cancel(ID, Table);
        }

        public DatatableResultBO GetRepackingList(string Type, string TransNoHint, string TransDateHint, string ItemNameHint, string BatchNoHint, string BatchTypeHint, string QuantityInHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return repackingDal.GetRepackingList(Type, TransNoHint, TransDateHint, ItemNameHint, BatchNoHint, BatchTypeHint, QuantityInHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
