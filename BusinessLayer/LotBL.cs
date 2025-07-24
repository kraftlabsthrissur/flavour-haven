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
    public class LotBL : ILotContract
    {
        LotDAL lotDAL;
        public LotBL()
        {
            lotDAL = new LotDAL();
        }


        public List<LotBO> GetLotList()
        {
            return lotDAL.GetLotList();
        }
        public List<LotBO> GetLotListByBin(int BinID)
        {
            return lotDAL.GetLotListByBin(BinID);
        }


    }
}
