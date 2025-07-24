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
    public class BinDAL
    {
        public List<BinBO> GetBinList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetBinListMaster().Select(a => new BinBO
                    {
                        ID = a.ID,
                        BinCode = a.BinCode,
                        WareHouseName = a.WarehouseName
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int CreateBin(BinBO binBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.spCreateBin( binBO.BinCode, binBO.WareHouseID,GeneralBO.CreatedUserID);
                    return 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<BinBO> GetBinListByWateHoue(int WareHouseID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetBinListByWareHouseID(WareHouseID).Select(a => new BinBO
                    {
                        ID = a.ID,
                        BinCode = a.BinCode
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public BinBO GetBinDetailsMaster(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetBinDetailsByID(ID).Select(a => new BinBO
                    {
                        ID = a.ID,
                        BinCode = a.BinCode,
                        WareHouseName = a.WarehouseName
                       

                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int UpdateBinDetails(BinBO BinBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateBinDetails(BinBO.ID, BinBO.BinCode, BinBO.WareHouseID, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                       
                     
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
