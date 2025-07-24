using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IDiscountCategoryContract
    {
        int Save (DiscountCategoryBO discountCategoryBO);
        List<DiscountCategoryBO> GetDiscountCategoryList();
        DiscountCategoryBO GetDiscountCategoryDetails(int ID);
         List<DiscountCategoryBO> GetDiscountList();
    }
}
