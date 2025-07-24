using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using DataAccessLayer;
using BusinessObject;
using BusinessLayer;

namespace BusinessLayer
{
    public class CategoryBL : ICategoryContract
    {
        CategoryDAL categoryDAL;
        public CategoryBL()
        {
            categoryDAL = new CategoryDAL();
        }
        public List<CategoryBO> GetCategoryList()
        {
            return categoryDAL.GetCategoryList();
        }

        public int CreateCategory(CategoryBO categoryBO)
        {
            return categoryDAL.CreateCategory(categoryBO);
        }
        public int CreateCategories(CategoryBO categoryBO)
        {
            return categoryDAL.CreateCategories(categoryBO);
        }
        public int UpdateCategories(CategoryBO categoryBO)
        {
            return categoryDAL.UpdateCategories(categoryBO);
        }
        
        public int EditCategory(CategoryBO categoryBO)
        {
            return categoryDAL.UpdateCategory(categoryBO);
        }

        public CategoryBO GetCategoryDetails(int CategoryID)
        {
            return categoryDAL.GetCategoryDetails(CategoryID);
        }

        public List<CategoryGroupBO> GetCategoryGroup()
        {
            return categoryDAL.GetCategoryGroup();
        }

        public List<CategoryBO> GetItemCategoryForSales()
        {
            return categoryDAL.GetItemCategoryForSales();
        }

        public List<CategoryBO> GetItemsWithPackSize(int ProductGroupID)
        {
            return categoryDAL.GetItemsWithPackSize(ProductGroupID);
        }
        public List<CategoryBO> GetAllItemCategoryList()
        {
            return categoryDAL.GetAllItemCategoryList();
        }
        public List<CategoryBO> GetSalesCategory(int ItemCategoryID)
        {
            return categoryDAL.GetSalesCategory(ItemCategoryID);
        }
        public List<CategoryBO> GetDiscountCategory()
        {
            return categoryDAL.GetDiscountCategory();
        }       

        public List<CategoryBO> GetSuppliersCategoryList()
        {
            return categoryDAL.GetSuppliersCategoryList();
        }
        public int GetCategoryID(string Category)
        {
            return categoryDAL.GetCategoryID(Category);
        }
            
        public List<CategoryBO> GetSuppliersAccountCategoryGroup()
        {
            return categoryDAL.GetSuppliersAccountCategoryGroup();
        }
        public List<CategoryBO> GetSuppliersTaxCategoryGroup()
        {
            return categoryDAL.GetSupplierTaxCategoryList();
        }
        public List<CategoryBO> GetSuppliersSubTaxCategoryGroup()
        {
            return categoryDAL.GetSuppliersTaxSubCategoryList();
        }
        public List<CategoryBO> GetSupplierItemCategoryList()
        {
            return categoryDAL.GetSupplierItemCategoryList();
        }
        public List<CategoryBO> GetSupplierItemCategoryBySupplierID(int SupplierID)
        {
            return categoryDAL.GetSupplierItemCategoryBySupplierID(SupplierID);
        }
        public List<CategoryBO> GetCustomerCategoryList()
        {
            return categoryDAL.GetCustomerCategoryList();
        }

        public List<CategoryBO> GetCustomerTaxCategoryList()
        {
            return categoryDAL.GetCustomerTaxCategoryList();
        }
        public List<CategoryBO> GetCustomerAccountsCategoryList()
        {
            return categoryDAL.GetCustomerAccountsCategoryList();
        }
        public List<CategoryBO> GetCategoryListByCategoryGroupID(int CategoryGroupID)
        {
            return categoryDAL.GetCategoryListByCategoryGroupID(CategoryGroupID);
        }
        public List<CategoryBO> GetEmployeeCategoryList()
        {
            return categoryDAL.GetEmployeeCategoryList();
        }
        public List<CategoryBO> GetPayrollCategoryList()
        {
            return categoryDAL.GetPayrollCategoryList();
        }

        public List<CategoryBO> GetItemCategoryList()
        {
            return categoryDAL.GetItemCategoryList();
        }

        public List<CategoryBO> GetPurchaseCategoryList(int ItemCategoryID)
        {
            return categoryDAL.GetPurchaseCategoryList(ItemCategoryID);
        }
        
        public List<CategoryBO> GetCategoriesList()
        {
            return categoryDAL.GetCategoriesList();
        }
        public List<CategoryBO> GetCategoriesDetails(int id, string CategoryType)
        {
            return categoryDAL.GetCategoriesDetails(id, CategoryType);
        }

        public List<CategoryBO> GetSalesManagerCategory()
        {
            return categoryDAL.GetSalesManagerCategory();
        }

        public List<CategoryBO> GetAreaManagerCategory()
        {
            return categoryDAL.GetAreaManagerCategory();
        }

        public List<CategoryBO> GetRegionalSalesManagerCateogry()
        {
            return categoryDAL.GetRegionalSalesManagerCateogry();
        }

        public List<CategoryBO> GetZonalManagerCategory()
        {
            return categoryDAL.GetZonalManagerCategory();
        }

        public List<CategoryBO> GetBusinessCategoryList(int ItemCategoryID)
        {
            return categoryDAL.GetBusinessCategoryList();
        }

        public List<CategoryBO> GetSalesIncentiveCategoryList(int ItemCategoryID)
        {
            return categoryDAL.GetSalesIncentiveCategoryList(ItemCategoryID);
        }

        public List<CategoryBO> GetSalesCategoryList(int ItemCategoryID)
        {
            return categoryDAL.GetSalesCategoryList(ItemCategoryID);
        }

        public List<CategoryBO> GetAccountsCategoryList()
        {
            return categoryDAL.GetCategoryList("Accounts");
        }
        public List<CategoryBO> GetStockValuationItemCategory(string CategoryName)
        {
            return categoryDAL.GetStockValuationItemCategory(CategoryName);
        }
        public List<CategoryBO> GetItemCategories(string Type)
        {
            return categoryDAL.GetItemCategories(Type);
        }
        public List<CategoryBO> GetCostCategoryList()
        {
            return categoryDAL.GetCostCategoryList();
        }
        public List<CategoryBO> ManufacturerList()
        {
            return categoryDAL.ManufacturerList();
        }
    }
}

