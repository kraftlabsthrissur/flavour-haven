using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using DataAccessLayer;
using BusinessObject;

namespace BusinessLayer
{
    public class ABCCodeBL : IABCCodeContract
    {
        ABCCodeDAL aBCCodeDAL;
        public ABCCodeBL()
        {
            aBCCodeDAL = new ABCCodeDAL();
        }


        public List<ABCCodeBO> GetABCCodeList()
        {
            return aBCCodeDAL.GetABCCodeList();
        }


    }
}
