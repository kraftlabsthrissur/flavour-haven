using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LocationHeadDAL
    {
        public List<LocationHeadBO> GetLocationHeadList()
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                return dbEntity.SpGetLocationHeadList().Select(a => new LocationHeadBO
                {
                    ID = a.ID,
                    Name=a.Name

                }).ToList();
            }
        }
    }
}
