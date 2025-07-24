using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IDropdownContract
    {       
        
        List<ItemCategoryBO> GetItemCategoryList();        
        List<ItemCategoryBO> GetTransactionTypeList();
    

        List<ItemBO> GetItemList(string hintint, int ItemCategoryID = 0, int PurchaseCategoryID = 0);

        #region Service Purchase requisition
        List<InterCompanyBO> GetInterCompanyList();
        List<ProjectBO> GetProjectList();
        #endregion

          
        List<PurchaseOrderItemBO> GetPurchaseOrderItems(string hint,int SupplierID, int ItemCategoryID = 0, int PurchaseCategoryID = 0,int BusinessCategoryID=0);
        List<TreasuryDetailBO> GetBankDetails();
        List<PurchaseOrderBO> GetUnProcessedPurchaseOrderForGrn(int SupplierID);
        List<TDSBO> GetTDS();
        List<ItemCategoryBO> GetItemCategoryForService();

    }
}
