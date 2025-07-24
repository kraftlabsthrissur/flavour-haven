using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class UnitGroupDAL
    {
        public List<UnitGroupBO> GetUnitGroupList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetUnitGroupList().Select(a => new UnitGroupBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

      

    }
}
