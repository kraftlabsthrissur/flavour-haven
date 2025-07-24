using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ISupplierCategoryContract
    {
        int Save(SupplierCategoryBO SuppliercategoryBO);
        List<SupplierCategoryBO> GetAllSupplierCategory();
        SupplierCategoryBO GetSupplierCategoryDetails(int ID);
    }

}
