using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class SalesBudgetDAL
    {
        public int ProcessUploadedSalesBudget(string Items)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpProcessUploadedSalesBudget(
                       Items,
                       GeneralBO.FinYear,
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID
                    );
                }
                return 1;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Save(SalesBudgetBO salesBudgetBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpCreateSalesBudget(
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID
                    );
                }
                return 1;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetSalesBudgetList(string ItemCodeHint, string ItemNameHint, string MonthHint,string SalesCategoryHint, string BatchTypeHint, string BranchHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetSalesBudgetList(ItemCodeHint, ItemNameHint, MonthHint, SalesCategoryHint, BatchTypeHint, BranchHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                ItemID = (int)item.ItemID,
                                ItemName = item.ItemName,
                                ItemCode = item.ItemCode,
                                SalesCategoryID = item.SalesCategoryID,
                                Category = item.Category,
                                Month = item.Month,
                                BatchTypeID = item.BatchTypeID,
                                BatchType = item.BatchType,
                                BranchID = item.BranchID,
                                Branch = item.Branch,
                                BudgetQtyInNos = item.BudgetQtyInNos,
                                BudgetQtyInKgs = item.BudgetQtyInKgs,
                                BudgetGrossRevenue = item.BudgetGrossRevenue,
                                ActualQtyInNos = item.ActualQtyInNos,
                                ActualQtyInKgs = item.ActualQtyInKgs,
                                ActualGrossRevenue = item.ActualGrossRevenue
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
