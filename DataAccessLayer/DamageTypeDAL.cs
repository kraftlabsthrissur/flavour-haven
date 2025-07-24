using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class DamageTypeDAL
    {
        public List<DamageTypeBO> GetDamageTypeList()
        {
            List<DamageTypeBO> damageType = new List<DamageTypeBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                damageType = dEntity.SpGetDamageType(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DamageTypeBO
                {
                    ID = a.ID,
                    Name = a.Name

                }).ToList();
                return damageType;
            }
        }
    }
}
