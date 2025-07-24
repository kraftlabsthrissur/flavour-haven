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
   public class ManufacturerBL:IManufacturerContract
    {
        ManufacturerDAL manufacturerDAL;
        public ManufacturerBL()
        {
            manufacturerDAL = new ManufacturerDAL();
        }
        public int Save(ManufacturerBO manufacturer)
        {
            return manufacturerDAL.Save(manufacturer);
        }
        public List<ManufacturerBO> GetManufacturerList()
        {
            return manufacturerDAL.GetManufacturerList();
        }
        public List<ManufacturerBO> GetManufacturerDetails(int ID)
        {
            return manufacturerDAL.GetManufacturerDetails(ID);
        }
        public int Update(ManufacturerBO manufacturer)
        {
            return manufacturerDAL.Update(manufacturer);
        }
    }
}
