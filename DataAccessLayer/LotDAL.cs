using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class LotDAL
    {
        public List<LotBO> GetLotList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetLotList().Select(a => new LotBO
                    {
                        ID = a.ID,
                        LotNumber = a.LotNumber
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LotBO> GetLotListByBin(int BinID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetLotListByBinID(BinID).Select(a => new LotBO
                    {
                        ID = a.ID,
                        LotNumber = a.LotNumber
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
