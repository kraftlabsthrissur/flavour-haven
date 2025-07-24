using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LocationGroupDAL
    {
        public List<LocationGroupBO> GetLocationGroup()
        {
            List<LocationGroupBO> LocationGroup = new List<LocationGroupBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                LocationGroup = dEntity.SpGetLocationGrouplist().Select(a => new LocationGroupBO
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList();
                return LocationGroup;
            }

        }
    }
}
