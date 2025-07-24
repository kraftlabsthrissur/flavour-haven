using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public  interface IGSTSubCategoryContract
    {
        List<GSTSubCategoryBO> GetGSTSubCategoryList();
        List<GSTSubCategoryBO> GetGSTSubCategoryDetails(int ID);
        int CreateGSTSubCategory(GSTSubCategoryBO subCategory);
        int UpdateGSTSubCategory(GSTSubCategoryBO subCategory);
    }
}
