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
    public class LocationHeadBL :ILocationHeadContract
    {
        LocationHeadDAL locationHeadDAL;
        public LocationHeadBL()
        {
            locationHeadDAL = new LocationHeadDAL();
        }
        public List<LocationHeadBO> GetLocationHeadList()
        {
            return locationHeadDAL.GetLocationHeadList();
        }
    }
}
