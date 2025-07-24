using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IProductionDefinitionContract
    {
        bool IsEditable(int ProductionGroupID);

        List<ProductionDefinitionBO> GetProductionDefinitionList();

        List<ProductionDefinitionBO> GetProductionDefinition(int ID);

        List<ProductionDefinitionMaterialBO> GetProductionDefinitionMaterials(int ID);

        List<ProductionDefinitionProcessBO> GetProductionDefinitionProcesses(int ID);

        int Save(ProductionDefinitionBO productionDefinitionBO, List<ProductionDefinitionSequenceBO> SequenceList, List<ProductionDefinitionMaterialBO> ItemList, List<ProductionDefinitionProcessBO> ProcessList, List<ProductionDefinitionMaterialBO> DeleteMaterialList, List<ProductionDefinitionProcessBO> DeleteProcessList, List<ProductionDefinitionBO> LocationList);

        DatatableResultBO GetProductionDefinitionList(string ProductionGroup,string Name,string BatchSize,string OutputQty,string SortField,string SortOrder,int Offset,int Limit);
    }
}
