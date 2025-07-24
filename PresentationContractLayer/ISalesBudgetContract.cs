using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface ISalesBudgetContract
    {
        List<SalesBudgetItemBO> ReadExcel(string Path);
        int Save(SalesBudgetBO salesBudgetBO);
        DatatableResultBO GetSalesBudgetList(string ItemCodeHint,string ItemNameHint,string MonthHint,string SalesCategoryHint,string BatchTypeHint,string BranchHint,string SortField,string SortOrder,int Offset,int Limit);
    }
}
