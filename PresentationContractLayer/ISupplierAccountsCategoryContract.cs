using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface ISupplierAccountsCategoryContract
    {
        int Save(SupplierAccountsCategoryBO SupplieraccountscategoryBO);
        List<SupplierAccountsCategoryBO> GetAllSupplierAccountsCategory();
        SupplierAccountsCategoryBO GetSupplierAccountsCategoryDetails(int ID);
    }
}
