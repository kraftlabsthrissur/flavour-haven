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
    public class LocationGroupBL : ILocationGroupContract
    {
        LocationGroupDAL locationGroupDAL;
        public LocationGroupBL()
        {
            locationGroupDAL = new LocationGroupDAL();
        }
        public List<LocationGroupBO> GetLocationGroup()
        {
            return locationGroupDAL.GetLocationGroup();
        }
    }
}
