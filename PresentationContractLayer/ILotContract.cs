using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using System.Web.UI.WebControls;

namespace PresentationContractLayer
{
    public interface ILotContract
    {
        List<LotBO> GetLotList();
        List<LotBO> GetLotListByBin(int BinID);

       
    }
}
