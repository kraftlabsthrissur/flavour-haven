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
    public class ConfigurationBL : IConfigurationContract
    {
        ConfigurationDAL configurationDAL;

        public ConfigurationBL()
        {
            configurationDAL = new ConfigurationDAL();
        }

        public int Save(ConfigurationBO configurationBO)
        {
            return configurationDAL.Save(configurationBO);
        }

        public List<ConfigurationBO> GetDefaultStoreList(int LocationID)
        {
            return configurationDAL.GetDefaultStoreList(LocationID);
        }

        public int GetSelectedStore(int LocationID,int UserID)
        {
            return configurationDAL.GetSelectedStore(LocationID, UserID);
        }
        public int GetCashPayementLimit()
        {
            return configurationDAL.GetCashPayementLimit();
        }
        public int GetGstCategoryForChequestatus()
        {
            return configurationDAL.GetGstCategoryForChequestatus();
        }
    }
}
