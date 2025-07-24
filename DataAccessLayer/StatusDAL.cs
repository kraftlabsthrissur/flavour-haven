using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class StatusDAL
    {
        public List<StatusBO> GetStatusList(string type)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetStatusList(type).Select(a => new StatusBO
                {
                    ID = a.ID,
                    Text = a.Text,
                    Value = a.Value
                   
                }).ToList();

            }
        }
    }
}
