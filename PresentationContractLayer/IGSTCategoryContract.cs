using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IGSTCategoryContract
    {
        List<GSTCategoryBO> GetGSTCategoryList();
        List<GSTCategoryBO> GetTaxCategoryList();
        List<GSTCategoryBO> GetGSTCategoryDetails(int GSTCategoryID);
        int Save(GSTCategoryBO GSTCategoryBO);
        int Update(GSTCategoryBO GSTCategory);
        List<GSTCategoryBO> GetGSTList();
        List<GSTCategoryBO> GetTaxCategoryListByTaxType(int TaxTypeID);
        List<GSTCategoryBO> GetVatPercentage();
    }
}
