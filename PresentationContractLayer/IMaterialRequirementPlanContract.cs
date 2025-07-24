using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IMaterialRequirementPlanContract
    {
        List<MaterialRequirementPlanItemBO> GetMaterialRequirmentPlanList(DateTime fromDate, DateTime toDate);
        int Save(MaterialRequirementPlanBO materialRequirementPlanBO, List<MaterialRequirementPlanItemBO> Items);
    }
}
