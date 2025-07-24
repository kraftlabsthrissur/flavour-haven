using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class CategoryDAL
    {
        public List<CategoryBO> GetCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCategoryList().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        CategoryGroupID = a.CategoryGroupID,
                        GroupName = a.GroupName
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }


        }

        public List<CategoryBO> GetCategoryListByCategoryGroupID(int CategoryGroupID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCategoryByGroupID(CategoryGroupID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        CategoryGroupID = a.CategoryGroupID,
                        Type = a.Type,
                        Code = a.code
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }


        }

        public List<CategoryGroupBO> GetCategoryGroup()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCategoryGroup().Select(a => new CategoryGroupBO   ///TO DO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }


        }

        public List<CategoryBO> GetItemCategoryForSales()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpSGetItemCategory().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> GetItemsWithPackSize(int ProductGroupID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetItemsWithPackSizeByProductionGroupID(ProductGroupID,GeneralBO.ApplicationID).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name =a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public List<CategoryBO> GetAllItemCategoryList()
        {
            List<CategoryBO> itm = new List<CategoryBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetAllItemCategory().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CategoryBO> GetSalesCategory(int ItemCategoryID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpSGetSalesCategory(ItemCategoryID).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public List<CategoryBO> GetDiscountCategory()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDiscountCategory().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Value = (decimal)a.Value
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public CategoryBO GetCategoryDetails(int CategoryID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCategoryByID(CategoryID).Select(a => new CategoryBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        CategoryGroupID = a.CategoryGroupID,
                        GroupName = a.GroupName

                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateCategory(CategoryBO categoryBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateCategory(
                        categoryBO.Name,
                        categoryBO.CategoryGroupID,
                        GeneralBO.CreatedUserID,
                        DateTime.Now
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int CreateCategories(CategoryBO categoryBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateCategories(
                        categoryBO.CategoryType,
                        categoryBO.CategoryName,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int UpdateCategory(CategoryBO categoryBO)
        {
            try
            {

                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateCategory(categoryBO.ID, categoryBO.Name, categoryBO.CategoryGroupID,GeneralBO.CreatedUserID,GeneralBO.LocationID,GeneralBO.ApplicationID);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        public List<CategoryBO> GetSupplierTaxCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSupplierTaxCategory().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }


        }

        public List<CategoryBO> GetSuppliersTaxSubCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSupplierTaxSubCategory().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> GetSupplierItemCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetItemCategory().Select(a => new CategoryBO
                    {
                        CategoryID = a.ID,
                        CategoryName = a.Name,
                    }
                    ).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> GetSupplierItemCategoryBySupplierID(int SupplierID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetSupplierItemCategory(SupplierID).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        CategoryName = a.CategoryName,
                        CategoryID = a.CategoryID
                    }).ToList();

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CategoryBO> GetSuppliersAccountCategoryGroup()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSupplierAccountCategory().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> GetSuppliersCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSuppliersCategory().Select(a => new CategoryBO
                    {
                        ID = (int)a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public int GetCategoryID(string Category)
        {
            try
            {
                ObjectParameter CategoryID = new ObjectParameter("CategoryID", typeof(int));

                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var a = dbEntity.SpGetCategoryIDByCategory(Category, CategoryID);
                    return Convert.ToInt32(CategoryID.Value);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> GetCustomerCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    return dbEntity.CustomerCategories.Select(a => new CategoryBO
                    {
                        ID = (int)a.ID,
                        Name = a.Name,
                    }
                    ).OrderBy(a => a.Name).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public List<CategoryBO> GetCustomerTaxCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.CustomerTaxCategories.Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CategoryBO> GetCustomerAccountsCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.CustomerAccountsCategories.Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CategoryBO> GetEmployeeCategoryList()
        {
            List<CategoryBO> EmployeeCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                EmployeeCategory = dEntity.SpGetEmployeeCategoryList().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return EmployeeCategory;
            }

        }

        public List<CategoryBO> GetItemCategoryList()
        {
            List<CategoryBO> ItemCategory = new List<CategoryBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                ItemCategory = dEntity.SpGetItemCategoryForPurchaseRequisition().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return ItemCategory;
            }

        }

        public List<CategoryBO> GetPayrollCategoryList()
        {
            List<CategoryBO> PayrollCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                PayrollCategory = dEntity.SpGetPayrollCategoryList().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return PayrollCategory;
            }

        }


        public List<CategoryBO> GetPurchaseCategoryList(int ItemCategoryID = 0)
        {
            List<CategoryBO> itm = new List<CategoryBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetPurchaseCategory(ItemCategoryID).Select(a => new CategoryBO
                    {

                        ID = a.PurchaseCategoryID,
                        Name = a.PurchaseCategory
                    }).ToList();
                    return itm;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CategoryBO> GetCategoriesList()
        {
            List<CategoryBO> itm = new List<CategoryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    itm = dbEntity.SpGetCategories().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        CategoryType = a.Type
                    }).ToList();
                    return itm;
                }


            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }
        public List<CategoryBO> GetCategoriesDetails(int id, string CategoryType)
        {
            List<CategoryBO> Category = new List<CategoryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Category = dbEntity.SpGetCategoriesDetails(id, CategoryType, GeneralBO.ApplicationID).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        CategoryName = a.Name,
                        CategoryTypeID = a.CategoryTypeID

                    }).ToList();
                }
                return Category;
            }

            catch (Exception Ex)
            {

                throw Ex;
            }
        }
        public int UpdateCategories(CategoryBO Categories)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateCategories(Categories.ID, Categories.CategoryName, Categories.CategoryType, GeneralBO.ApplicationID,GeneralBO.CreatedUserID,GeneralBO.LocationID);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CategoryBO> GetSalesManagerCategory()
        {
            List<CategoryBO> ItemCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                ItemCategory = dEntity.SpGetSalesManagerCategory().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return ItemCategory;
            }

        }

        public List<CategoryBO> GetAreaManagerCategory()
        {
            List<CategoryBO> ItemCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                ItemCategory = dEntity.SpGetAreaManagerCategory().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return ItemCategory;
            }

        }

        public List<CategoryBO> GetRegionalSalesManagerCateogry()
        {
            List<CategoryBO> ItemCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                ItemCategory = dEntity.SpGetRegionalSalesManagerCateogry().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return ItemCategory;
            }

        }

        public List<CategoryBO> GetZonalManagerCategory()
        {
            List<CategoryBO> ItemCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                ItemCategory = dEntity.SpGetZonalManagerCategory().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return ItemCategory;
            }

        }

        public List<CategoryBO> GetBusinessCategoryList()
        {
            List<CategoryBO> ItemCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                ItemCategory = dEntity.SpGetBusinessCategoryList().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return ItemCategory;
            }
        }

        public List<CategoryBO> GetSalesIncentiveCategoryList(int ItemCategoryID)
        {
            List<CategoryBO> ItemCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                ItemCategory = dEntity.SpGetSalesIncentiveCategoryList(ItemCategoryID).Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return ItemCategory;
            }

        }

        public List<CategoryBO> GetSalesCategoryList(int ItemCategoryID)
        {
            List<CategoryBO> ItemCategory = new List<CategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                ItemCategory = dEntity.SpGetSalesCategoryList(ItemCategoryID).Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return ItemCategory;
            }

        }

        public List<CategoryBO> GetCategoryList(string CategoryGroup)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCategoryListByGroup(CategoryGroup).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        CategoryGroupID = a.CategoryGroupID,
                        GroupName = a.GroupName
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> GetStockValuationItemCategory(string CategoryName)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetStockValuationItemCategory(CategoryName).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> GetItemCategories(string Type)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetItemCategories(Type).Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> GetCostCategoryList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCostCategory().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CategoryBO> ManufacturerList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetManufacturer().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        

    }
}
