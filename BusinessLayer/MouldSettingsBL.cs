using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;




namespace BusinessLayer
{
   public class MouldSettingsBL:IMouldsettingsContract
    {
        MouldSettingsDAL mouldSettingsDAL;

        public MouldSettingsBL()
        {
            mouldSettingsDAL = new MouldSettingsDAL();
        }
        public List<MouldSettingsBO> GetMachinesForMouldSettings()
        {
            return mouldSettingsDAL.GetMachinesForMouldSettings();
        }

        public MouldSettingsBO GetMachinesForMouldSettingsByID(int MachineID)
        {
            return mouldSettingsDAL.GetMachinesForMouldSettingsByID(MachineID);
        }
        public int Save(MouldSettingsBO mouldSettingsBO)
        {
            return mouldSettingsDAL.Save(mouldSettingsBO);
        }

        public List<MouldSettingsHistoryBO> GetMouldSettings(int MachineID)
        {
            return mouldSettingsDAL.GetMouldSettings(MachineID);
        }

    }
}
