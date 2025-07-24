using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class ConfigurationDAL
    {
        public int Save(ConfigurationBO configurationBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateConfiguration(
                configurationBO.StoreName,
                configurationBO.StoreID,
                configurationBO.UserID,
                configurationBO.LocationID,
                GeneralBO.ApplicationID
                 );
              
            }
            return 1;
        }

        public List<ConfigurationBO> GetDefaultStoreList(int LocationID)
        {
            List<ConfigurationBO> StoreList = new List<ConfigurationBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    StoreList = dbEntity.SpGetDefaultStore(LocationID).Select(a => new ConfigurationBO
                    {
                        StoreID = a.ID,
                        StoreName = a.Name
                    }).ToList();
                    return StoreList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetSelectedStore(int LocationID,int UserID)
        {
            ObjectParameter StoreID = new ObjectParameter("StoreID", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpGetSelectedDefaultStore(
                    LocationID,
                    UserID,
                    StoreID
                 );
                return Convert.ToInt16(StoreID.Value);
            }
            
        }

        public int GetCashPayementLimit()
        {
            ObjectParameter CashLimit = new ObjectParameter("CashLimit", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpGetCashPayementLimit(
                    GeneralBO.ApplicationID,
                    CashLimit
                 );
                return Convert.ToInt16(CashLimit.Value);
            }

        }
        public int GetGstCategoryForChequestatus()
        {
            ObjectParameter ID = new ObjectParameter("ID", typeof(int));
            using (MasterEntities entity = new MasterEntities())
            {
                entity.SpGetGstCategoryForChequestatus(GeneralBO.ApplicationID, ID);
                return Convert.ToInt32(ID.Value);
            }
        }
    }
}
