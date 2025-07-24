using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;
using DataAccessLayer;


namespace BusinessLayer
{
    public class MaterialPurificationBL : IMaterialPurificationContract
    {
        MaterialPurificationDAL materialPurificationDAL;

        public MaterialPurificationBL()
        {
            materialPurificationDAL = new MaterialPurificationDAL();
        }

        public List<MaterialPurificationBO> GetMaterialPurificationProcessList()
        {
            return materialPurificationDAL.GetMaterialPurificationProcessList();
        }

        public int Save(MaterialPurificationBO materialPurificationBO)
        {
            if (materialPurificationBO.ID == 0)
            {
                return materialPurificationDAL.Save(materialPurificationBO);
            }
            else
            {
                return materialPurificationDAL.Update(materialPurificationBO);

            }

        }

        public List<MaterialPurificationBO> GetMaterialPurificationList()
        {
            return materialPurificationDAL.GetMaterialPurificationList();
        }

        public List<MaterialPurificationBO> GetMaterialPurificationDetail(int ID)
        {
            return materialPurificationDAL.GetMaterialPurificationDetail(ID);
        }
    }
}
