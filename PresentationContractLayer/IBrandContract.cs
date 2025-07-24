using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IBrandContract
    {
        List<BrandBO> GetBrandList();
        int CreateBrand(BrandBO BrandBO);
        BrandBO GetBrandDetails(int ID);
        int EditBrand(BrandBO BrandBO);
    }
}
