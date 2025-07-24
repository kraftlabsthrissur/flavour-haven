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
   public class PowerConsumptionBL: IPowerConsumptionContract
    {
        PowerConsumptionDAL powerConsumptionDAL;

        public PowerConsumptionBL()
        {
            powerConsumptionDAL = new PowerConsumptionDAL();
        }

        public List<PowerConsumptionBO> GetLocationList()
        {
            return powerConsumptionDAL.GetLocationList();
        }

        public int Save(List<PowerConsumptionItemBO> Items, PowerConsumptionBO powerConsumptionBO)
        {
            if (powerConsumptionBO.ID == 0)
            {
                return powerConsumptionDAL.Save(Items, powerConsumptionBO);
            }
            else
            {
                return powerConsumptionDAL.Update(Items, powerConsumptionBO);
            }
        }

        public List<PowerConsumptionBO> GetPowerConsumptionDetails(int ID)
        {
            return powerConsumptionDAL.GetPowerConsumptionDetails(ID);
        }

        public List<PowerConsumptionBO> GetPowerConsumptionTransDetails(int ID)
        {
            return powerConsumptionDAL.GetPowerConsumptionTransDetails(ID);
        }
    }
}
