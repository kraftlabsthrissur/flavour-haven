using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IManufacturerContract
    {
        int Save(ManufacturerBO manufacturer);
        List<ManufacturerBO> GetManufacturerList();
        List<ManufacturerBO> GetManufacturerDetails(int ID);
        int Update(ManufacturerBO manufacturer);
    }
}
