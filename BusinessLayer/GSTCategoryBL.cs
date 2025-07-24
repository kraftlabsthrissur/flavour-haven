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
    public class GSTCategoryBL : IGSTCategoryContract
    {
        GSTCategoryDAL gstCategoryDAL;
        public GSTCategoryBL()
        {
            gstCategoryDAL = new GSTCategoryDAL();

        }
        public List<GSTCategoryBO> GetGSTCategoryList()
        {
            return gstCategoryDAL.GetGSTCategoryList();
        }
        public List<GSTCategoryBO> GetTaxCategoryList()
        {
            return gstCategoryDAL.GetTaxCategoryList();
        }
        public List<GSTCategoryBO> GetTaxCategoryListByTaxType(int TaxTypeID)
        {
            return gstCategoryDAL.GetTaxCategoryListByTaxType(TaxTypeID);
        }

        public int Save(GSTCategoryBO GSTCategoryBO)
        {
            return gstCategoryDAL.Save(GSTCategoryBO);

        }
        public List<GSTCategoryBO> GetGSTCategoryDetails(int GSTCategoryID)
        {
            return gstCategoryDAL.GetGSTCategoryDetails(GSTCategoryID);
        }
        public int Update(GSTCategoryBO GSTCategory)
        {
            return gstCategoryDAL.Update(GSTCategory);
        }

        public List<GSTCategoryBO> GetGSTList()
        {
            return gstCategoryDAL.GetGSTList();
        }

        public List<GSTCategoryBO> GetVatPercentage()
        {
            return gstCategoryDAL.GetVatPercentage();
        }
    }
}
