using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IPowerConsumptionContract
    {
        List<PowerConsumptionBO> GetLocationList();
        int Save(List<PowerConsumptionItemBO> Items, PowerConsumptionBO powerConsumptionBO);
        List<PowerConsumptionBO> GetPowerConsumptionDetails(int ID);
        List<PowerConsumptionBO> GetPowerConsumptionTransDetails(int ID);
    }
}
