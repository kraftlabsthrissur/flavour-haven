using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;


namespace PresentationContractLayer
{
   public interface IDiscountContract
    {
        DatatableResultBO GetDiscountList(string CodeHint, string NameHint, string CustomerNameHint,string CustomerCategoryHint,string StateHint, string BusinessCategoryHint, string SalesIncentiveCategoryHint, string SalesCategoryHint, string DiscountCategoryHint, string DiscountPercentageHint, string SortField, string SortOrder, int Offset, int Limit);
        List<DiscountBO> GetDiscountDetails(int ItemID , int CustomerID, int CustomerCategoryID, int CustomerStateID, int BusinessCategoryID, int SalesIncentiveCategoryID, int SalesCategoryID);
        int Save(List<DiscountBO> DiscountDetails);
    }
}
