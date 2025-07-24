using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class ProductionDefinitionBL : IProductionDefinitionContract
    {
        ProductionDefinitionDAL productionDefinitionDAL;

        public ProductionDefinitionBL()
        {
            productionDefinitionDAL = new ProductionDefinitionDAL();
        }

        public List<ProductionDefinitionBO> GetProductionDefinitionList()
        {
            return productionDefinitionDAL.GetProductionDefinitionList();
        }

        public int Save(ProductionDefinitionBO productionDefinitionBO, List<ProductionDefinitionSequenceBO> SequenceList, List<ProductionDefinitionMaterialBO> MaterialList,
            List<ProductionDefinitionProcessBO> ProcessList, List<ProductionDefinitionMaterialBO> DeleteMaterialList, List<ProductionDefinitionProcessBO> DeleteProcessList,
            List<ProductionDefinitionBO> LocationList)
        {
            if (productionDefinitionBO.ProductionGroupID == 0)
            {
                return productionDefinitionDAL.Save(productionDefinitionBO, SequenceList, MaterialList, ProcessList, DeleteMaterialList, DeleteProcessList, LocationList);
            }
            else
            {
                return productionDefinitionDAL.Update(productionDefinitionBO, SequenceList, MaterialList, ProcessList, DeleteMaterialList, DeleteProcessList, LocationList);
            }
        }

        public List<ProductionDefinitionBO> GetProductionDefinition(int ID)
        {
            return productionDefinitionDAL.GetProductionDefinition(ID);
        }

        public List<ProductionDefinitionMaterialBO> GetProductionDefinitionMaterials(int ID)
        {
            return productionDefinitionDAL.GetProductionDefinitionMaterials(ID);
        }

        public List<ProductionDefinitionProcessBO> GetProductionDefinitionProcesses(int ID)
        {
            return productionDefinitionDAL.GetProductionDefinitionProcesses(ID);
        }

        public bool IsEditable(int ProductionGroupID)
        {
            return productionDefinitionDAL.IsEditable(ProductionGroupID);
        }

        public DatatableResultBO GetProductionDefinitionList(string ProductionGroup, string Name, string BatchSize, string OutputQty, string SortField, string SortOrder, int Offset, int Limit)
        {
            return productionDefinitionDAL.GetProductionDefinitionList(ProductionGroup, Name, BatchSize, OutputQty, SortField, SortOrder, Offset, Limit);
        }
    }
}
