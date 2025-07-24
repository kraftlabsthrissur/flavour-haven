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
   public class DiscountCategoryBL: IDiscountCategoryContract
    {
        DiscountCategoryDAL discountCategoryDAL;
        public DiscountCategoryBL()
        {
            discountCategoryDAL = new DiscountCategoryDAL();
        }
        public int Save(DiscountCategoryBO discountCategoryBO)
        {
            if (discountCategoryBO.ID == 0)
            {
                return discountCategoryDAL.Save(discountCategoryBO);
            }
            else
            {
                return discountCategoryDAL.Update(discountCategoryBO);
            }
        }

        public List<DiscountCategoryBO> GetDiscountCategoryList()
        {
            return discountCategoryDAL.GetDiscountCategoryList();
        }

        public DiscountCategoryBO GetDiscountCategoryDetails(int ID)
        {
            return discountCategoryDAL.GetDiscountCategoryDetails(ID);
        }

        public List<DiscountCategoryBO> GetDiscountList()
        {
            return discountCategoryDAL.GetDiscountList();
        }

    }
}
