using BusinessObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IConfigurationContract
    {
        int Save(ConfigurationBO configurationBO);
        List<ConfigurationBO> GetDefaultStoreList(int LocationID);
        int GetSelectedStore(int LocationID, int UserID);
        int GetCashPayementLimit();
        int GetGstCategoryForChequestatus();
    }
}
