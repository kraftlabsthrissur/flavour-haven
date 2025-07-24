using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface ITurnOverDiscountContract
    {
        List<DiscountItemBO> ReadExcel(string Path);
        DatatableResultBO GetCustomerListForLocation(int CustomerLocationID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
        int Save(List<DiscountItemBO> Items, TurnOverDiscountsBO turnOverDiscountBO);
        List<TurnOverDiscountsBO> GetTurnOverDiscountList();
        List<TurnOverDiscountsBO> GetTurnOverDiscountDetails(int ID);
        List<DiscountItemBO> GetTurnOverDiscountTransDetails(int ID);
    }
}
