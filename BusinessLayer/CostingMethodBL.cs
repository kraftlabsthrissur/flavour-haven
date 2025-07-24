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
    public class CostingMethodBL : ICostingMethodContract
    {
        CostingMethodDAL costingMethodDAL;
        public CostingMethodBL()
        {
            costingMethodDAL = new CostingMethodDAL();

        }
        public List<CostingMethodBO> GetCostingMethodList()
        {
            return costingMethodDAL.GetCostingMethodDList().ToList();
        }
    }
}
