using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;


namespace DataAccessLayer
{
   public class DiscountDAL
    {
        public List<DiscountBO> GetDiscountDetails(int ItemID, int CustomerID, int CustomerCategoryID, int CustomerStateID, int BusinessCategoryID, int SalesIncentiveCategoryID, int SalesCategoryID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDiscountDetails(ItemID, CustomerID, CustomerCategoryID, CustomerStateID, BusinessCategoryID, SalesIncentiveCategoryID, SalesCategoryID).Select(a => new DiscountBO()
                    {
                        ItemID =(int)a.ItemID,
                        ItemName = a.ItemName,
                        CustomerCategoryID=(int)a.CustomerCategoryID,
                        CustomerCategoryName=a.CustomerCategory,
                        BusinessCategoryID=(int)a.BusinessCategoryID,
                        BusinessCategoryName=a.BusinessCategory,
                        CustomerStateID=(int)a.CustomerStateID,
                        CustomerStateName=a.CustomerState,
                        SalesIncentiveCategoryID=(int)a.SalesIncentiveCategoryID,
                        SalesIncentiveCategoryName=a.SalesIncentiveCategory,
                        SalesCategoryID=(int)a.SalesCategoryID,
                        SalesCategoryName=a.SalesCategory,
                        CustomerID=(int)a.CustomerID,
                        CustomerName=a.Customer,
                        DiscountCategoryID=(int)a.DiscountCategoryID,
                        DiscountPercentage=(decimal)a.Discountpercentage,
                        ID=(int)a.DiscountID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(List<DiscountBO> DiscountDetails)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    foreach (var item in DiscountDetails)
                    {
                        if (item.ID == 0)
                        {
                            dbEntity.SpCreateDiscount(
                                   item.ItemID,
                                   item.CustomerID,
                                   item.CustomerCategoryID,
                                   item.CustomerStateID,
                                   item.BusinessCategoryID,
                                   item.SalesIncentiveCategoryID,
                                   item.SalesCategoryID,
                                   item.DiscountCategoryID, 
                                   item.DiscountPercentage,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID
                                );
                        }
                        else
                        {
                            dbEntity.SpUpdateDiscount(
                                   item.ID,
                                   item.DiscountCategoryID,
                                   item.DiscountPercentage,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID

                            );
                        }
                    }
                    
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetDiscountList(string CodeHint, string NameHint, string CustomerNameHint, string CustomerCategoryHint, string StateHint, string BusinessCategoryHint, string SalesIncentiveCategoryHint, string SalesCategoryHint, string DiscountCategoryHint, string DiscountPercentageHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetDiscountList(CodeHint, NameHint, CustomerNameHint, CustomerCategoryHint, StateHint, BusinessCategoryHint, SalesIncentiveCategoryHint, SalesCategoryHint, DiscountCategoryHint, DiscountPercentageHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.ItemName,
                                CustomerName = item.CustomerName,
                                CustomerCategory = item.CustomerCategory,
                                State = item.State,
                                BusinessCategory = item.BusinessCategory,
                                SalesIncentiveCategory = item.SalesIncentiveCategory,
                                SalesCategory = item.SalesCategory,
                                DiscountCategory = item.DiscountCategory,
                                DiscountPercentage = item.DiscountPercentage
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

    }
}
