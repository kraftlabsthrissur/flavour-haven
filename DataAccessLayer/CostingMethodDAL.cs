using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CostingMethodDAL
    {
        public List<CostingMethodBO> GetCostingMethodDList()
        {
            try
            {
                List<CostingMethodBO> taxType = new List<CostingMethodBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var method = dbEntity.SPGetCostingMethod().Select(a => new CostingMethodBO
                    {
                        ID = a.ID,
                        Name = a.Name,

                    }).ToList();

                    return method;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
