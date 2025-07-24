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
    public class BrandBL : IBrandContract
    {
        BrandDAL brandDAL;

        public BrandBL()
        {
            brandDAL = new BrandDAL();
        }
        public int CreateBrand(BrandBO brandBO)
        {
            return brandDAL.CreateBrand(brandBO);
        }
        public List<BrandBO> GetBrandList()
        {
            try
            {
                return brandDAL.GetBrandList().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BrandBO GetBrandDetails(int ID)
        {
            return brandDAL.GetBrandDetails(ID);
        }

        public int EditBrand(BrandBO BrandBO)
        {
            return brandDAL.UpdateBrand(BrandBO);
        }
    }
}
