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
    public class SupplierCategoryBL : ISupplierCategoryContract
    {

        SupplierCategoryDAL supplierCategoryDAL;

        public SupplierCategoryBL()
        {
            supplierCategoryDAL = new SupplierCategoryDAL();
        }

        public int Save(SupplierCategoryBO SuppliercategoryBO)
        {
            if (SuppliercategoryBO.ID == 0)
            {
                return supplierCategoryDAL.Save(SuppliercategoryBO);
            }
            else
            {
                return supplierCategoryDAL.Update(SuppliercategoryBO);
            }

        }

        public List<SupplierCategoryBO> GetAllSupplierCategory()
        {
            return supplierCategoryDAL.GetAllSupplierCategory();
        }

        public SupplierCategoryBO GetSupplierCategoryDetails(int ID)
        {
            return supplierCategoryDAL.GetSupplierCategoryDetails(ID);
        }
    }
}
