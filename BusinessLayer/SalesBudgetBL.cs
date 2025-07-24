using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
   public class SalesBudgetBL : ISalesBudgetContract
    {
        SalesBudgetDAL salesBudgetDAL;

        public SalesBudgetBL()
        {
            salesBudgetDAL = new SalesBudgetDAL();
        }

        public List<SalesBudgetItemBO> ReadExcel(string Path)
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(0, "ItemCode");
            dict.Add(1, "ItemName");
            dict.Add(2, "SalesCategory");
            dict.Add(3, "Month");
            dict.Add(4, "BatchType");
            dict.Add(5, "Branch");
            dict.Add(6, "BudgetQtyInNos");
            dict.Add(7, "BudgetQtyInKgs");
            dict.Add(8, "BudgetGrossRevenue");
            dict.Add(9, "ForecastsQtyInNos");
            dict.Add(10, "ForecastsQtyInKgs");
            dict.Add(11, "ForecastsGrossRevenue");
            dict.Add(12, "ActualQtyInNos");
            dict.Add(13, "ActualQtyInKgs");
            dict.Add(14, "ActualGrossRevenue");

            SalesBudgetItemBO UploadSalesBudget = new SalesBudgetItemBO();
            GeneralBL generalBL = new GeneralBL();
            List<SalesBudgetItemBO> Items;
            try
            {
                Items = generalBL.ReadExcel(Path, UploadSalesBudget, dict);
                string StringItems = XMLHelper.Serialize(Items);
                 salesBudgetDAL.ProcessUploadedSalesBudget(StringItems);
            }
            catch (Exception e)
            {
                throw e;
            }
            return Items;
        }

        public int Save(SalesBudgetBO salesBudgetBO)
        {
              return salesBudgetDAL.Save(salesBudgetBO);
        }

        public DatatableResultBO GetSalesBudgetList(string ItemCodeHint, string ItemNameHint,string MonthHint, string SalesCategoryHint, string BatchTypeHint, string BranchHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesBudgetDAL.GetSalesBudgetList(ItemCodeHint, ItemNameHint, MonthHint, SalesCategoryHint, BatchTypeHint, BranchHint, SortField, SortOrder, Offset, Limit);
        }


    }
}
