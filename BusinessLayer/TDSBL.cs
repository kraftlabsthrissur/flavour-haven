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
    public class TDSBL : ITDSContract
    {
        TDSDAL tdsDAL;
        public TDSBL()
        {
            tdsDAL = new TDSDAL();

        }

        public List<TDSBO> GetTDSList()
        {
            return tdsDAL.GetTDSList();
        }

        public List<TDSBO> GetTDSDetails(int TDSIID)
        {
            return tdsDAL.GetTDSDetails(TDSIID);
        }

        public int Save(TDSBO TDS)
        {
            return tdsDAL.Save(TDS);
        }

        public int Update(TDSBO TDS)
        {
            return tdsDAL.Update(TDS);
        }
    }
}
