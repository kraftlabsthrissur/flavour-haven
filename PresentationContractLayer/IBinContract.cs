using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using System.Web.UI.WebControls;

namespace PresentationContractLayer
{
    public interface IBinContract
    {
        List<BinBO> GetBinList();
        int CreateBin(BinBO BinBO);
        List<BinBO> GetBinListByWareHouse(int WareHouseID);
        BinBO GetBinDetailsMaster(int ID);
        int EditBinDetails(BinBO BinBo);

    }
}
