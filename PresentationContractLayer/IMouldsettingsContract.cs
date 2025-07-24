using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
   public interface IMouldsettingsContract
    {
        List<MouldSettingsBO> GetMachinesForMouldSettings();
        MouldSettingsBO GetMachinesForMouldSettingsByID(int MachineID);
        int Save(MouldSettingsBO mouldSettingsBO);
        List<MouldSettingsHistoryBO> GetMouldSettings(int MachineID);
    }
}
