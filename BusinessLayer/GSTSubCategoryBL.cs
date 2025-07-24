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
   public  class GSTSubCategoryBL: IGSTSubCategoryContract
    {
        GSTSubCategoryDAL gstSubCategoryDAL;
        public GSTSubCategoryBL()
        {
            gstSubCategoryDAL = new GSTSubCategoryDAL();
        }
        public List<GSTSubCategoryBO> GetGSTSubCategoryList()
        {
            return gstSubCategoryDAL.GetGSTSubCategoryList();
        }
        public List<GSTSubCategoryBO> GetGSTSubCategoryDetails(int ID)
        {
            return gstSubCategoryDAL.GetGSTSubCategoryDetails(ID);
        }
        public int CreateGSTSubCategory(GSTSubCategoryBO subCategory)
        {
            return gstSubCategoryDAL.CreateGSTSubCategory(subCategory);
        }
        public int UpdateGSTSubCategory(GSTSubCategoryBO subCategory)
        {
            return gstSubCategoryDAL.UpdateGSTSubCategory(subCategory);
        }


    }
}
