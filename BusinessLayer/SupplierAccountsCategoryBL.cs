using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System.Collections.Generic;


namespace BusinessLayer
{
    public class SupplierAccountsCategoryBL : ISupplierAccountsCategoryContract
    {

        SupplierAccountsCategoryDAL supplierAccountsCategoryDAL;

        public SupplierAccountsCategoryBL()
        {
            supplierAccountsCategoryDAL = new SupplierAccountsCategoryDAL();
        }

        public int Save(SupplierAccountsCategoryBO SupplieraccountscategoryBO)
        {
            if (SupplieraccountscategoryBO.ID != 0)
            {
                return supplierAccountsCategoryDAL.Update(SupplieraccountscategoryBO);
            }
            else
            {
                return supplierAccountsCategoryDAL.Save(SupplieraccountscategoryBO);
            }

        }

        public List<SupplierAccountsCategoryBO> GetAllSupplierAccountsCategory()
        {
            return supplierAccountsCategoryDAL.GetAllSupplierAccountsCategory();
        }

        public SupplierAccountsCategoryBO GetSupplierAccountsCategoryDetails(int ID)
        {
            return supplierAccountsCategoryDAL.GetSupplierAccountsCategoryDetails(ID);
        }

    }
}
