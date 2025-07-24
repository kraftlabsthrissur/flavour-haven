using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface ICategoryContract
    {
        List<CategoryBO> GetCategoryList();
        int CreateCategory(CategoryBO categoryBO);
        int EditCategory(CategoryBO categoryBO);
        CategoryBO GetCategoryDetails(int CategoryID);
        List<CategoryGroupBO> GetCategoryGroup();
        List<CategoryBO> GetItemCategoryForSales();
        List<CategoryBO> GetItemsWithPackSize(int ProductGroupID);
        List<CategoryBO> GetAllItemCategoryList();
        List<CategoryBO> GetSalesCategory(int ItemCategoryID);
        List<CategoryBO> GetDiscountCategory();
        List<CategoryBO> GetItemCategoryList();
        List<CategoryBO> GetPurchaseCategoryList(int ItemCategoryID);
        int GetCategoryID(string CategoryName);
        List<CategoryBO> GetSuppliersCategoryList();
        List<CategoryBO> GetStockValuationItemCategory(string CategoryName);
        List<CategoryBO> GetSuppliersAccountCategoryGroup();
        List<CategoryBO> GetSuppliersTaxCategoryGroup();
        List<CategoryBO> GetSuppliersSubTaxCategoryGroup();
        List<CategoryBO> GetSupplierItemCategoryList();
        List<CategoryBO> GetSupplierItemCategoryBySupplierID(int SupplierID);
        List<CategoryBO> GetCustomerCategoryList();
        List<CategoryBO> GetCustomerTaxCategoryList();
        List<CategoryBO> GetCustomerAccountsCategoryList();
        List<CategoryBO> GetCategoryListByCategoryGroupID(int CategoryGroupID);
        List<CategoryBO> GetEmployeeCategoryList(); 
        List<CategoryBO> GetPayrollCategoryList();
        int CreateCategories(CategoryBO categoryBO);
        int UpdateCategories(CategoryBO categoryBO);
        List<CategoryBO> GetCategoriesList();
        List<CategoryBO> GetCategoriesDetails(int id,string CategoryType);
        List<CategoryBO> GetSalesManagerCategory();
        List<CategoryBO> GetRegionalSalesManagerCateogry();
        List<CategoryBO> GetZonalManagerCategory();
        List<CategoryBO> GetAreaManagerCategory();
        List<CategoryBO> GetSalesIncentiveCategoryList(int ItemCategoryID);
        List<CategoryBO> GetBusinessCategoryList(int ItemCategoryID);
        List<CategoryBO> GetSalesCategoryList(int ItemCategoryID);
        List<CategoryBO> GetAccountsCategoryList();
        List<CategoryBO> GetItemCategories(string Type);
        List<CategoryBO> GetCostCategoryList();
        List<CategoryBO> ManufacturerList();
    }
}
