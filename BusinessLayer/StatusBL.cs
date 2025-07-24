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
   public class StatusBL : IStatusContract
    {
        StatusDAL statusDAL;

        public StatusBL()
        {
            statusDAL = new StatusDAL();
        }

        public List<StatusBO> GetStatusList(string type)
        {
            return statusDAL.GetStatusList(type);
        }
    }
}
