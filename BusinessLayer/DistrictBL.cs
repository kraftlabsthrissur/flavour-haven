using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using DataAccessLayer;
using BusinessObject;

namespace BusinessLayer
{
    public class DistrictBL:IDistrictContract
    {
        DistrictDAL districtDAL;
        public DistrictBL()
            {
                districtDAL = new DistrictDAL();
            }
        public List<DistrictBO> GetDistrictList(int StateID)
        {
            return districtDAL.GetDistrictList(StateID);
        }

        public List<DistrictBO> GetDistrict()
        {
            return districtDAL.GetDistrict();
        }

        public int CreateDistrict(DistrictBO districtBO)
        {
            return districtDAL.CreateDistrict(districtBO);
        }

        public List<StateBO> GetStateName()
        {
            return districtDAL.GetStateName();
        }

        public DistrictBO GetDistrictDetails(int DistrictID)
        {
            return districtDAL.GetDistrictDetails(DistrictID);
        }

        public int EditDistrict(DistrictBO districtBO)
        {
            return districtDAL.UpdateDistrict(districtBO);
        }
    }
}
