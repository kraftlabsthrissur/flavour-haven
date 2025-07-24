using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IDistrictContract
    {
        List<DistrictBO> GetDistrictList(int StateID);
        List<DistrictBO> GetDistrict();
        int CreateDistrict(DistrictBO districtBO);
        int EditDistrict(DistrictBO districtBO);
        DistrictBO GetDistrictDetails(int DistrictID);
        List<StateBO> GetStateName();
    }
}
