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
    public class BinBL : IBinContract
    {
        BinDAL binDAL;
        public BinBL()
        {
            binDAL = new BinDAL();
        }

        public int CreateBin(BinBO binBO)
        {
            return binDAL.CreateBin(binBO);
        }
        public List<BinBO> GetBinList()
        {
            return binDAL.GetBinList();
        }
        public List<BinBO> GetBinListByWareHouse(int WareHouseID)
        {
            return binDAL.GetBinListByWateHoue(WareHouseID);
        }
        public BinBO GetBinDetailsMaster(int ID)
        {
            return binDAL.GetBinDetailsMaster(ID);
        }

        public int EditBinDetails(BinBO BinBO)
        {
            return binDAL.UpdateBinDetails(BinBO);
        }
    }
}
