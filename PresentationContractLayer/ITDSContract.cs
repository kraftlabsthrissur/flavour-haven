using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ITDSContract
    {
        List<TDSBO> GetTDSList();
        List<TDSBO> GetTDSDetails(int TDSID);
        int Save(TDSBO TDS);
        int Update(TDSBO TDS);
    }
}
