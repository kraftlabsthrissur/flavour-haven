using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IMaterialPurificationContract
    {
        List<MaterialPurificationBO> GetMaterialPurificationProcessList();
        List<MaterialPurificationBO> GetMaterialPurificationList();
        List<MaterialPurificationBO> GetMaterialPurificationDetail(int ID);
        int Save(MaterialPurificationBO materialPurificationBO);
    }
}
