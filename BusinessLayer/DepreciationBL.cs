using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using BusinessLayer;

namespace BusinessLayer
{
    public class DepreciationBL : IDepreciationContract
    {
        DepreciationDAL depreciationDAL;
        public DepreciationBL()
        {
            depreciationDAL = new DepreciationDAL();
        }
        public int CalculateDepreciation()
        {
            return depreciationDAL.CalculateDepreciation();
        }
    }
}
