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
   public class MaterialRequirementPlanBL : IMaterialRequirementPlanContract
    {
        MaterialRequirementPlanDAL materialRequirementPlanDAL;

        public MaterialRequirementPlanBL()
        {
            materialRequirementPlanDAL = new MaterialRequirementPlanDAL();
        }

        public List<MaterialRequirementPlanItemBO> GetMaterialRequirmentPlanList(DateTime fromDate, DateTime toDate)
        {
            return materialRequirementPlanDAL.GetMaterialRequirmentPlanList(fromDate, toDate);
        }

        public int Save(MaterialRequirementPlanBO materialRequirementPlanBO, List<MaterialRequirementPlanItemBO> Items)
        {
            return materialRequirementPlanDAL.Save(materialRequirementPlanBO, Items);
        }
    }
}
