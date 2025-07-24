using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using System.Diagnostics;


namespace BusinessLayer
{
  public class DiscountBL: IDiscountContract
    {
        DiscountDAL discountDAL;

        public DiscountBL()
        {
            discountDAL = new DiscountDAL();
        }

        public List<DiscountBO> GetDiscountDetails(int ItemID, int CustomerID, int CustomerCategoryID, int CustomerStateID, int BusinessCategoryID, int SalesIncentiveCategoryID, int SalesCategoryID)
        {
            return discountDAL.GetDiscountDetails(ItemID, CustomerID, CustomerCategoryID, CustomerStateID, BusinessCategoryID, SalesIncentiveCategoryID, SalesCategoryID);
        }

        public int Save(List<DiscountBO> DiscountDetails)
        {
            return discountDAL.Save(DiscountDetails);
        }

        public DatatableResultBO GetDiscountList(string CodeHint, string NameHint, string CustomerNameHint, string CustomerCategoryHint, string StateHint, string BusinessCategoryHint, string SalesIncentiveCategoryHint, string SalesCategoryHint, string DiscountCategoryHint, string DiscountPercentageHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return discountDAL.GetDiscountList(CodeHint, NameHint, CustomerNameHint, CustomerCategoryHint, StateHint, BusinessCategoryHint, SalesIncentiveCategoryHint, SalesCategoryHint, DiscountCategoryHint,  DiscountPercentageHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
