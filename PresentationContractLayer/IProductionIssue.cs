using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IProductionIssue
    {

        List<MaterialProductionIssueBO> GetProductionIssueMaterials(int productionGroupID, int productionSequence, int itemID,int ProductID);
        List<MaterialProductionIssueBO> GetProductionIssueMaterials(int productionID);

        List<ProcessProductionIssueBO> GetProductionIssueProcesses(int productionGroupID, int productionSequence, int itemID);

        //List<AdditionalIssueItemBO> GetAdditionalIssueItems(int itemCategoryID, int purchaseCategoryID, string itemHint);

        List<BatchWiseStockBO> GetBatchWithStocks(int itemID);

        List<MaterialTransBO> GetMaterialTrans(int productionIssueMaterialsID);

        int Save(ProductionIssueBO productionIssueBO);

        ProductionIssueBO GetProductionIssue(int productionID);

        List<ProductionSequenceBO> GetProductionSequences(int ProductGroupID);

        bool IsProductionIssueEditable(int productionID);

        DatatableResultBO GetProductionIssueList(string Type,string TransNoHint,string TransDateHint, string ExpectedDateHint, string ProductionGroupNameHint, string BatchNoHint,string BatchsizeHint,string UnitHint,string SortField, string SortOrder,int Offset,int Limit);
    }
}
 